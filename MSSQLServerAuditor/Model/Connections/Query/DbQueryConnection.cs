using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MSSQLServerAuditor.Model.Commands;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	public abstract class DbQueryConnection<TConnection> : IQueryConnection where TConnection : DbConnection
	{
		private readonly TConnection _dbConnection;

		protected DbQueryConnection(TConnection dbConnection)
		{
			this._dbConnection = dbConnection;
		}

		public virtual void Dispose()
		{
			if (this._dbConnection != null)
			{
				this._dbConnection.Dispose();
			}
		}

		public virtual void Open()
		{
			if (this._dbConnection != null)
			{
				this._dbConnection.Open();
			}
		}

		public virtual void ChangeDatabase(string database)
		{
			if (!string.IsNullOrEmpty(database))
			{
				if (this._dbConnection != null)
				{
					this._dbConnection.ChangeDatabase(database);
				}
			}
		}

		public virtual void Close()
		{
			if (this._dbConnection != null)
			{
				this._dbConnection.Close();
			}
		}

		public ConnectionState? State
		{
			get
			{
				if (this._dbConnection != null)
				{
					return this._dbConnection.State;
				}

				return null;
			}
		}

		protected TConnection Connection
		{
			get { return this._dbConnection; }
		}

		public abstract IQueryCommand GetCommand(
			string                          sqlText,
			int                             commandTimeout,
			IEnumerable<QueryParameterInfo> parameters,
			out List<Tuple<int, string>>    parametersQueueForODBC
		);
	}
}
