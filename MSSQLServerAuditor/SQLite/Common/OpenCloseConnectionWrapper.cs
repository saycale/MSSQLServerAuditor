using System;
using System.Data;
using System.Data.SQLite;

namespace MSSQLServerAuditor.SQLite.Common
{
	////////////////////////////////////////////////////////////////////////////////////////////////
	// TODO: This thing for refactoring. This class manages the connection (open/close), but not creates it.
	// Pattern Factory for creation connections more suitable for this.
	////////////////////////////////////////////////////////////////////////////////////////////////
	public class OpenCloseConnectionWrapper : IDisposable
	{
		public SQLiteConnection Connection { get; private set; }

		private readonly bool _wasOpened;

		public OpenCloseConnectionWrapper()
		{
			this._wasOpened = false;
		}

		public OpenCloseConnectionWrapper(SQLiteConnection connection) : this()
		{
			this.Connection = connection;

			if (this.Connection != null)
			{
				if (this.Connection.State == ConnectionState.Closed)
				{
					this.Connection.Open();
					this._wasOpened = true;
				}
			}
		}

		public void Dispose()
		{
			if (this._wasOpened)
			{
				if (this.Connection != null)
				{
					this.Connection.Close();
				}
			}
		}
	}
}
