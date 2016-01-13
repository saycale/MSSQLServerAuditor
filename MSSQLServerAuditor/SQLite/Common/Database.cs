using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Tables;
using MSSQLServerAuditor.Model.Connections.Factories;

namespace MSSQLServerAuditor.SQLite.Common
{
	/// <summary>
	/// Wrapper for SQLite database
	/// </summary>
	public class Database
	{
		public string             FileName   { get; private set; }
		public bool               ReadOnly   { get; private set; }
		internal SQLiteConnection Connection { get; private set; }

		private readonly HashSet<string> _attachedDatabases;

		protected Database(string fileName, bool readOnly = false)
		{
			this.FileName = fileName;
			this.ReadOnly = readOnly;

			CreateIfNotExists(fileName);

			this.Connection         = ConnectionFactory.CreateSQLiteConnection(fileName, readOnly).OpenAndReturn();
			this._attachedDatabases = new HashSet<string>();
		}

		public virtual void Initialize()
		{
		}

		/// <summary>
		/// Initialize SQLite DB
		/// </summary>
		/// <param name="fileName">File name of DB</param>
		/// <param name="readOnly">Is DB read-only?</param>
		/// <returns></returns>
		public static Database GetOrCreate(string fileName, bool readOnly = false)
		{
			if (!File.Exists(fileName))
			{
				SQLiteConnection.CreateFile(fileName);
			}

			return new Database(fileName, readOnly);
		}

		public static void CreateIfNotExists(string fileName)
		{
			if (!fileName.Equals("file::memory:"))
			{
				if (!File.Exists(fileName))
				{
					SQLiteConnection.CreateFile(fileName);
				}
			}
		}

		protected void RunBuildScripts(string postBuildSctiptFile)
		{
			List<QueryInfo> queryInfo = Program.Model.LoadQueries(postBuildSctiptFile, false);

			using (this.Connection.OpenWrapper())
			{
				foreach (QueryInfo info in queryInfo)
				{
					foreach (QueryItemInfo itemInfo in info.Items)
					{
						new SqlCustomCommand(this.Connection, itemInfo.Text)
							.Execute(100);
					}
				}
			}
		}

		public void DropAllTables()
		{
			var tables = new List<string>();

			using (this.Connection.OpenWrapper())
			{
				using (
					var cmd = new SQLiteCommand(
						"SELECT [name] FROM [sqlite_master] WHERE [type] = 'table'",
						this.Connection
					)
				)
				{
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							tables.Add((string)reader["name"]);
						}
					}
				}

				foreach (var tbl in tables)
				{
					if (tbl == ServiceInfoTable.TableName)
					{
						continue;
					}

					using (var cmd = new SQLiteCommand("DROP TABLE " + tbl, this.Connection))
					{
						cmd.ExecuteNonQuery();
					}
				}
			}
		}

		public void AttachDatabase(string strDatabaseFileName, string strDatabaseAlias)
		{
			string strSql = string.Format("ATTACH '{0}' AS '{1}';",
				strDatabaseFileName,
				strDatabaseAlias
			);

			if (!this._attachedDatabases.Contains(strDatabaseAlias))
			{
				// Log.DebugFormat("AttachDatabase:sql:'{0}'",
				//    strSql ?? "<Null>"
				// );

				using (this.Connection.OpenWrapper())
				{
					new SqlCustomCommand(this.Connection, strSql).Execute(100);

					this._attachedDatabases.Add(strDatabaseAlias);
				}
			}
		}
	}
}
