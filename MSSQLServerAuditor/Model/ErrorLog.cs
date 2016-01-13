using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Error log item
	/// </summary>
	internal class ErrorLogItem
	{
		private readonly DateTime      _date;
		private readonly InstanceInfo  _instance;
		private readonly Exception     _error;
		private readonly QueryItemInfo _queryItem;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="date">DateTime of error</param>
		/// <param name="instance">Server instance of error</param>
		/// <param name="queryItem">Query</param>
		/// <param name="error">Thrown exception</param>
		public ErrorLogItem(DateTime date, InstanceInfo instance, QueryItemInfo queryItem, Exception error)
		{
			this._date      = date;
			this._instance  = instance;
			this._queryItem = queryItem;
			this._error     = error;
		}

		/// <summary>
		/// DateTime of error
		/// </summary>
		public DateTime Date
		{
			get { return this._date; }
		}

		/// <summary>
		/// Server instance of error
		/// </summary>
		public InstanceInfo Instance
		{
			get { return this._instance; }
		}

		/// <summary>
		/// Thrown exception
		/// </summary>
		public Exception Error
		{
			get { return this._error; }
		}

		/// <summary>
		/// Query
		/// </summary>
		public QueryItemInfo QueryItem
		{
			get { return this._queryItem; }
		}

		public override string ToString()
		{
			return this._date.ToShortDateString() + " " + this._date.ToShortTimeString();
		}
	}

	/// <summary>
	/// Error log
	/// </summary>
	internal class ErrorLog
	{
		readonly List<ErrorLogItem> _errorItems;
		readonly object             _errorItemsLock;

		public ErrorLog()
		{
			this._errorItems     = new List<ErrorLogItem>();
			this._errorItemsLock = new object();
		}

		/// <summary>
		/// Error list
		/// </summary>
		public ReadOnlyCollection<ErrorLogItem> ErrorItems
		{
			get { return this._errorItems.AsReadOnly(); }
		}

		public void AddItem(ErrorLogItem item)
		{
			lock (this._errorItemsLock)
			{
				this._errorItems.Add(item);
			}
		}

		/// <summary>
		/// Append errors to log
		/// </summary>
		/// <param name="multyQueryResultInfo">from multyquery result</param>
		public void AppendErrorLog(MultyQueryResultInfo multyQueryResultInfo)
		{
			foreach (TemplateNodeResultItem item in multyQueryResultInfo.List)
			{
				if (item.QueryResult.ErrorInfo == null)
				{
					AppendErrorLog(item.QueryResult);
				}
				else
				{
					this._errorItems.Add(new ErrorLogItem(item.QueryResult.ErrorInfo.DateTime, null,
						new QueryItemInfo() {Text = item.TemplateNodeQuery.QueryName},
						item.QueryResult.ErrorInfo.Exception)
					);
				}
			}
		}

		/// <summary>
		/// Append errors to log
		/// </summary>
		/// <param name="queryResult">from query result</param>
		public void AppendErrorLog(QueryResultInfo queryResult)
		{
			foreach (KeyValuePair<InstanceInfo, QueryInstanceResultInfo> instancePair in queryResult.InstancesResult)
			{
				var instance = instancePair.Value;

				if (instance.ErrorInfo != null)
				{
					this._errorItems.Add(new ErrorLogItem(instance.ErrorInfo.DateTime, instance.Instance, null, instance.ErrorInfo.Exception));
				}
				else
					foreach (KeyValuePair<string, QueryDatabaseResultInfo> databasePair in instance.DatabasesResult)
					{
						var database = databasePair.Value;

						if (database.ErrorInfo != null)
						{
							this._errorItems.Add(new ErrorLogItem(database.ErrorInfo.DateTime, instance.Instance, database.QueryItem, database.ErrorInfo.Exception));
						}
					}
			}
		}
	}
}
