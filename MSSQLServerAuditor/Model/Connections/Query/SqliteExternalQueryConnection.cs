using System.Data.SQLite;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	internal class SqliteExternalQueryConnection : SqliteQueryConnection
	{
		public SqliteExternalQueryConnection(SQLiteConnection dbConnection) : base(dbConnection)
		{
		}

		public override SqliteQueryCommand GetSqliteCommand(
			string sqlText,
			int commandTimeout
		)
		{
			SQLiteCommand cmd = new SQLiteCommand
			{
				Connection     = base.Connection,
				CommandText    = sqlText,
				CommandTimeout = commandTimeout
			};

			return new SqliteExternalQueryCommand(cmd);
		}

		private class SqliteExternalQueryCommand : SqliteQueryCommand
		{
			public SqliteExternalQueryCommand(SQLiteCommand command) : base(command)
			{
			}

			protected override void AssignDefaultParameters()
			{
				// no default parameters to assign
			}
		}
	}
}
