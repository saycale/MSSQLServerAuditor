using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MSSQLServerAuditor.Model.Commands;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	internal class ActiveDirectoryQueryConnection : IQueryConnection
	{
		private readonly ActiveDirectoryConnection _activeDirectoryConnection;

		public ActiveDirectoryQueryConnection(ActiveDirectoryConnection activeDirectoryConnection)
		{
			this._activeDirectoryConnection = activeDirectoryConnection;
		}

		public void Dispose()
		{
		}

		public void Open()
		{
		}

		public void ChangeDatabase(string database)
		{
		}

		public void Close()
		{
		}

		public ConnectionState? State
		{
			get { return ConnectionState.Open; }
		}

		public IQueryCommand GetCommand(
			string                          sqlText,
			int                             commandTimeout,
			IEnumerable<QueryParameterInfo> parameters,
			out List<Tuple<int, string>>    parametersQueueForODBC
		)
		{
			parametersQueueForODBC = new List<Tuple<int, string>>();

			List<string> queryParams = parameters.Select(param => param.Name).ToList();

			return new ActiveDirectoryCommand(this._activeDirectoryConnection, sqlText, queryParams);
		}

		private class ActiveDirectoryCommand : IQueryCommand
		{
			private const string GroupFilter     = "filter";
			private const string GroupProperties = "properties";
			private const string GroupScope      = "scope";
			private const string ScopeOneLevel   = "onelevel";
			private const string ScopeBase	     = "base";

			// pattern for parsing LDAP query
			private static readonly string LdapQueryPattern = string.Format(
				"^(?<{0}>.*?)(;(?<{1}>[^();]*))?(;(?<{2}>[^();]*))?$",
				GroupFilter,
				GroupProperties,
				GroupScope
			);

			private static readonly Regex LdapQueryRegex = new Regex(LdapQueryPattern);

			private readonly ActiveDirectoryConnection _activeDirectoryConnection;

			private string                              _ldapFilterExpr;
			private string                              _propertiesExpr;
			private string                              _searchScopeExpr;
			private readonly Dictionary<string, string> _parameters;

			public ActiveDirectoryCommand()
			{
				this._ldapFilterExpr  = string.Empty;
				this._propertiesExpr  = string.Empty;
				this._searchScopeExpr = string.Empty;
				this._parameters      = new Dictionary<string, string>();
			}

			public ActiveDirectoryCommand(
				ActiveDirectoryConnection activeDirectoryConnection,
				string                    query,
				IEnumerable<string>       parameters
			) : this()
			{
				this._activeDirectoryConnection = activeDirectoryConnection;

				foreach (var paramName in parameters)
				{
					// initalize parameters with empty values
					this._parameters[paramName] = string.Empty;
				}

				ParseQuery(query.TrimmedOrEmpty());
			}

			private void ParseQuery(string query)
			{
				Match match = LdapQueryRegex.Match(query);

				this._ldapFilterExpr = match.Groups[GroupFilter].Value;

				Group grpProperties = match.Groups[GroupProperties];

				if (grpProperties.Success)
				{
					this._propertiesExpr = grpProperties.Value;
				}

				Group grpScope = match.Groups[GroupScope];

				if (grpScope.Success)
				{
					this._searchScopeExpr = grpScope.Value;
				}
			}

			public void Dispose()
			{
			}

			public void AssignParameters(
				IEnumerable<QueryParameterInfo> parameters,
				IEnumerable<ParameterValue>     parameterValues,
				List<Tuple<int, string>>        parametersQueueForOdbc
			)
			{
				foreach (ParameterValue paramValue in parameterValues)
				{
					string paramName = paramValue.Name;

					if (this._parameters.ContainsKey(paramValue.Name))
					{
						this._parameters[paramName] =
							paramValue.UserValue ?? paramValue.StringValue;
					}
				}
			}

			public IAsyncResult BeginExecuteReader(AsyncCallback callback)
			{
				AsyncResult<IDataReader> asyncResult = new AsyncResult<IDataReader>(callback, this);

				Task.Factory.StartNew(() =>
				{
					try
					{
						IDataReader dataReader = ExecuteDirectoryReader();
						asyncResult.Complete(dataReader, false);
					}
					catch (Exception exc)
					{
						asyncResult.HandleException(exc, false);
					}
				});

				return asyncResult;
			}

			public IDataReader EndExecuteReader(IAsyncResult result)
			{
				AsyncResult<IDataReader> asyncResult = (AsyncResult<IDataReader>) result;

				if (asyncResult.Exception != null)
				{
					throw asyncResult.Exception;
				}

				return asyncResult.Result;
			}

			private IDataReader ExecuteDirectoryReader()
			{
				DirectoryEntry    entry    = new DirectoryEntry(_activeDirectoryConnection.ConnectionPath);
				DirectorySearcher searcher = new DirectorySearcher(entry)
				{
					Filter = FillParameters(this._ldapFilterExpr),
					SearchScope = GetSearchScope()
				};

				string propertiesExpr = FillParameters(this._propertiesExpr);

				string[] propertiesToRead = propertiesExpr
					.TrimmedOrEmpty()
					.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);

				bool readAllProperties = propertiesToRead.Length == 0;

				if (!readAllProperties)
				{
					foreach (string property in propertiesToRead.ToList())
					{
						searcher.PropertiesToLoad.Add(property);
					}

					return ReadDirectoryEntries(searcher);
				}

				return ReadDirectoryEntriesWithAllProperties(entry, searcher);
			}

			private SearchScope GetSearchScope()
			{
				string scope = FillParameters(this._searchScopeExpr);

				switch (scope.ToLower())
				{
					case ScopeOneLevel:
						return SearchScope.OneLevel;
					case ScopeBase:
						return SearchScope.Base;
					default:
						return SearchScope.Subtree;
				}
			}

			// retrieves LDAP filter that is filled with parameters' values
			private string FillParameters(string expression)
			{
				return this._parameters.Aggregate(
					expression, (current, parameter) => current.Replace(parameter.Key, parameter.Value));
			}

			private static IDataReader ReadDirectoryEntriesWithAllProperties(
				DirectoryEntry    entry,
				DirectorySearcher searcher
			)
			{
				DataTable   dt              = new DataTable();
				ICollection entryProperties = entry.Properties.PropertyNames;

				foreach (string property in entryProperties)
				{
					string colName = GetValidColumnName(property);
					dt.Columns.Add(colName, typeof(string));
				}

				SearchResultCollection searchResults = searcher.FindAll();

				foreach (SearchResult searchResult in searchResults)
				{
					DataRow row = dt.NewRow();

					foreach (string property in entryProperties)
					{
						ResultPropertyValueCollection propValues = searchResult.Properties[property];
						string colName = GetValidColumnName(property);
						row[colName] = ConcatPropertyValues(propValues);
					}

					dt.Rows.Add(row);
				}

				return dt.CreateDataReader();
			}

			private static IDataReader ReadDirectoryEntries(DirectorySearcher searcher)
			{
				SearchResultCollection searchResults = searcher.FindAll();
				DataTable              dt            = new DataTable();
				string[]               propsLoaded   = searchResults.PropertiesLoaded;

				foreach (string propertyLoaded in propsLoaded)
				{
					dt.Columns.Add(propertyLoaded, typeof(string));
				}

				foreach (SearchResult searchResult in searchResults)
				{
					DataRow row = dt.NewRow();

					foreach (var propertyName in propsLoaded)
					{
						ResultPropertyValueCollection propValues = searchResult.Properties[propertyName];
						row[propertyName] = ConcatPropertyValues(propValues);
					}

					dt.Rows.Add(row);
				}

				return dt.CreateDataReader();
			}

			private static string ConcatPropertyValues(ResultPropertyValueCollection values)
			{
				List<string> propValues = new List<string>();

				foreach (var propValue in values)
				{
					propValues.Add(propValue.ToString());
				}

				return string.Join(", ", propValues);
			}

			private static string GetValidColumnName(string propName)
			{
				const string validSqlColumnPattern = @"[^a-zA-Z0-9_]";
				Regex        columnPattern         = new Regex(validSqlColumnPattern);

				return columnPattern.Replace(propName.ToLower(), "_");
			}

			public void Cancel()
			{
			}
		}
	}
}
