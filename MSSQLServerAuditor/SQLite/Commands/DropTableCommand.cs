namespace MSSQLServerAuditor.SQLite.Commands
{
	using System;
	using System.Data.SQLite;
    using System.Reflection;

    using log4net;

    using MSSQLServerAuditor.SQLite.Common.Definitions;
    using MSSQLServerAuditor.Utils;

    internal class DropTableCommand : TableCommandBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DropTableCommand(SQLiteConnection connection, TableDefinition tableDefinition)
            : base(connection, tableDefinition)
        {
        }

        protected override ILog Logger
        {
            get
            {
                return Log;
            }
        }

        protected override long InternalExecute()
        {
            string sql = string.Format("DROP TABLE {0}",
				this.TableDefinition.Name.AsDbName()
			);

            Log.DebugFormat(
                "Table:'{0}',Query: '{1}'",
                this.TableDefinition.Name.AsDbName(),
                sql
			);

			return this.ExecuteNonQuery(sql, null);
        }
    }
}