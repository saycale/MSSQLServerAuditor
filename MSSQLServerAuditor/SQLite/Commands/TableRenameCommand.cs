using System;
using System.Data.SQLite;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Common.Definitions;

namespace MSSQLServerAuditor.SQLite.Commands
{
	internal class TableRenameCommand : TableCommandBase
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly string      _newTableName;

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		public TableRenameCommand(SQLiteConnection connection, TableDefinition tableDefinition, string newTableName)
			: base(connection, tableDefinition)
		{
			this._newTableName = newTableName;
		}

		protected override long InternalExecute()
		{
			long   iTotalRows = 0L;
			long   iRows      = 0L;
			string sql        = string.Format("ALTER TABLE '{0}' RENAME TO '{1}'",
				this.TableDefinition.Name,
				this._newTableName
			);

			// Log.DebugFormat(
			//    "rename table: Table: '{0}', NewTable: '{1}', Query: '{2}'",
			//    this.TableDefinition.Name,
			//    this._newTableName,
			//    sql
			// );

			iRows = this.ExecuteNonQuery(sql, null);

			iTotalRows += iRows;

			sql = string.Format("DROP INDEX IF EXISTS '{0}'", this.TableDefinition.GetIndexName());

			Log.DebugFormat(
				"drop index: Index: '{0}', Query: '{1}'",
				this.TableDefinition.GetIndexName(),
				sql
			);

			iRows = this.ExecuteNonQuery(sql, null);

			iTotalRows += iRows;

			new CreateIndexCommand(this.Connection, this.TableDefinition)
				.UpdateTableName(this._newTableName)
				.Execute(100);

			return iTotalRows;
		}
	}
}
