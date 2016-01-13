using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using MSSQLServerAuditor.SQLite.Commands;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	internal abstract class AttachingSqliteQueryConnection : SqliteQueryConnection
	{
		private static readonly ILog                  Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		protected readonly Dictionary<string, string> _databasesToAttach;

		protected AttachingSqliteQueryConnection(SQLiteConnection dbConnection) : base(dbConnection)
		{
			this._databasesToAttach = new Dictionary<string, string>();
		}

		public void AddDatabaseToAttach(string name, string dbFilePath)
		{
			if (!this._databasesToAttach.ContainsKey(name))
			{
				this._databasesToAttach.Add(name, dbFilePath);
			}
		}

		public override void Open()
		{
			base.Open();

			if (base.Connection != null)
			{
				foreach (KeyValuePair<string, string> dbToAttach in this._databasesToAttach)
				{
					string dbName     = dbToAttach.Key;
					string dbFileName = dbToAttach.Value;

					AttachDb(dbFileName, dbName);
				}
			}
		}

		public override void ChangeDatabase(string database)
		{
		}

		private void AttachDb(string dbFileName, string dbName)
		{
			string strSql = string.Format(
				"ATTACH '{0}' AS '{1}';",
				dbFileName,
				dbName
			);

			// Log.DebugFormat("AttachDb:Database:'{0}';sql:'{1}'", dbName, strSql);

			try
			{
				if (!IsDatabaseAttached(dbName))
				{
					// Log.DebugFormat(
					//    "Do database AttachDb:Database:'{0}';sql:'{1}'",
					//    dbName ?? "<Null>",
					//    strSql ?? "<Null>"
					// );

					new SqlCustomCommand(base.Connection, strSql).Execute(100);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorFormat(
					"AttachDb:FileName'{0}',dbName:'{1}',sql:'{2}',Error:'{3}'",
					dbFileName ?? "<Null>",
					dbName     ?? "<Null>",
					strSql     ?? "<Null>",
					ex
				);
			}
		}

		//
		// SQLite query to get the list of attached databases
		//
		private bool IsDatabaseAttached(string dbName)
		{
			const string sqlAttachedDbsList = "PRAGMA database_list;";
			List<string> attachedDbs        = new List<string>();

			SqlSelectCommand selectAttachedDbsCommand = new SqlSelectCommand(
				base.Connection,
				sqlAttachedDbsList,
				reader =>
				{
					while (reader.Read())
					{
						string attachedDb = (reader.GetValue(1)).ToString();
						attachedDbs.Add(attachedDb);
					}
				},
				null
			);

			selectAttachedDbsCommand.Execute(100);

			return attachedDbs.Contains(
				dbName,
				StringComparer.OrdinalIgnoreCase
			);
		}
	}
}
