using System;
using System.Data.SQLite;

namespace MSSQLServerAuditor.SQLite.Common
{
	public static class ConnectionFactory
	{
		public static SQLiteConnection CreateDbConnection(Database database, bool readOnly = true)
		{
			return CreateDbConnection(database.FileName, readOnly);
		}

		public static SQLiteConnection CreateDbConnection(string fileName, bool readOnly = true)
		{
			return CreateSQLiteConnection(fileName, readOnly);
		}

		private static SQLiteConnection CreateSQLiteConnection(string fileName, bool readOnly)
		{
			string connectionString = String.Empty;

			if (fileName.Equals("file::memory:"))
			{
				connectionString =
					"FullUri=file:memdb1?mode=memory&cache=shared; PRAGMA journal_mode=off; PRAGMA temp_store = MEMORY; PRAGMA synchronous=off; PRAGMA count_changes=off; PRAGMA encoding = \"UTF-8\";";
			}
			else
			{
				connectionString = string.Format(
					"Data Source={0}; PRAGMA locking_mode = NORMAL; Pooling=True; PRAGMA page_size = 4096; PRAGMA cache_size=10000; PRAGMA journal_mode=WAL; PRAGMA synchronous=off; PRAGMA count_changes=off; PRAGMA temp_store=2; PRAGMA encoding = \"UTF-8\"; PRAGMA query_only={1};{2}",
					fileName,
					readOnly,
					readOnly ? "mode=ro" : string.Empty
				);
			}

			return new SQLiteConnection(connectionString);
		}
	}
}
