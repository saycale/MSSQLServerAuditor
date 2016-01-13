using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MSSQLServerAuditor.Model.Commands;
using MSSQLServerAuditor.SQLite.Commands;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	internal class OdbcQueryConnection : DbQueryConnection<OdbcConnection>
	{
		public OdbcQueryConnection(OdbcConnection odbcConnection) : base(odbcConnection)
		{
		}

		public override IQueryCommand GetCommand(
			string                          sqlText,
			int                             commandTimeout,
			IEnumerable<QueryParameterInfo> parameters,
			out List<Tuple<int, string>>    parametersQueueForODBC
		)
		{
			OdbcCommand cmd = new OdbcCommand();

			parametersQueueForODBC = new List<Tuple<int, string>>();

			foreach (var currentParameter in parameters)
			{
				var rg      = new Regex(currentParameter.Name);
				var matches = rg.Matches(sqlText);

				foreach (Match currentMatch in matches)
				{
					if (!parametersQueueForODBC.Any(x => x.Item1 == currentMatch.Index && x.Item2 == currentMatch.Value))
					{
						parametersQueueForODBC.Add(new Tuple<int, string>(currentMatch.Index, currentMatch.Value));
					}
				}

				sqlText = sqlText.Replace(currentParameter.Name, "?");
			}

			parametersQueueForODBC.Sort(
				(x, y) =>
					{
						if (x.Item1 > y.Item1)
						{
							return 1;
						}
						if (x.Item1 < y.Item1)
						{
							return -1;
						}

						return 0;
					});

			cmd.Connection     = base.Connection;
			cmd.CommandText    = sqlText;
			cmd.CommandTimeout = commandTimeout;

			return new OdbcQueryCommand(cmd);
		}

		private class OdbcQueryCommand : IQueryCommand
		{
			private delegate IDataReader HostPingHandler(string host);

			private readonly OdbcCommand _command;

			public OdbcQueryCommand(OdbcCommand command)
			{
				this._command = command;
			}

			public void Dispose()
			{
				if (this._command != null)
				{
					this._command.Dispose();
				}
			}

			public void AssignParameters(
				IEnumerable<QueryParameterInfo> parameters,
				IEnumerable<ParameterValue>     parameterValues,
				List<Tuple<int, string>>        parametersQueueForOdbc
			)
			{
				if (parametersQueueForOdbc != null && parametersQueueForOdbc.Any() && parameters != null)
				{
					foreach (Tuple<int, string> currentTuple in parametersQueueForOdbc)
					{
						QueryParameterInfo currentParam = parameters.FirstOrDefault(x => x.Name == currentTuple.Item2);

						if (currentParam != null)
						{
							OdbcParameter p = this._command.CreateParameter();
							p.ParameterName = currentParam.Name;
							p.IsNullable    = true;
							p.DbType        = currentParam.Type.ToDbType();
							p.Value         = currentParam.GetDefaultValue();

							this._command.Parameters.Add(p);
						}
					}
				}
				else if (parameters != null)
				{
					foreach (QueryParameterInfo parameter in parameters)
					{
						OdbcParameter p = this._command.CreateParameter();
						p.ParameterName = parameter.Name;
						p.IsNullable    = true;
						p.DbType        = parameter.Type.ToDbType();
						p.Value         = parameter.GetDefaultValue();

						this._command.Parameters.Add(p);
					}
				}

				if (parameterValues != null)
				{
					foreach (ParameterValue value in parameterValues)
					{
						List<OdbcParameter> currentParameters =
							(from OdbcParameter p in this._command.Parameters
							 where p.ParameterName == value.Name
							 select p)
							 .ToList();

						foreach (OdbcParameter currentParameter in currentParameters)
						{
							if (currentParameter != null)
							{
								currentParameter.Value = value.GetValue(currentParameter.DbType);
							}
						}
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
						IDataReader dataReader = _command.ExecuteReader();
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

			public void Cancel()
			{
				if (this._command != null)
				{
					this._command.Cancel();
				}
			}
		}
	}
}
