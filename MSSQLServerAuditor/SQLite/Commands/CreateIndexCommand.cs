namespace MSSQLServerAuditor.SQLite.Commands
{
    #region

    using System;
    using System.Data.SQLite;
    using System.Linq;
    using System.Reflection;

    using log4net;

    using MSSQLServerAuditor.SQLite.Common.Definitions;
    using MSSQLServerAuditor.Utils;

    #endregion

    internal class CreateIndexCommand : TableCommandBase
    {
        private static readonly ILog Log          = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string               newTableName = null;

        public CreateIndexCommand(SQLiteConnection connection, TableDefinition tableDefinition)
            : base(connection, tableDefinition)
        {
        }

		public CreateIndexCommand(TableCommandBase tableCommandBase)
            : base(tableCommandBase)
        {
        }

        protected override ILog Logger
        {
            get
            {
                return Log;
            }
        }

        internal CreateIndexCommand UpdateTableName(string newTableName)
        {
            this.newTableName = newTableName;
            return this;
        }

        private string GetTableName()
        {
            return string.IsNullOrEmpty(this.newTableName)
                       ? this.TableDefinition.Name
                       : this.newTableName;
        }

        private string GetUniqueFields()
        {
            return this.TableDefinition.Fields.Values
                .Where(fieldDefinition => fieldDefinition.Unique)
                .Select(fieldDefinition => string.Format("{0}", fieldDefinition.Name.AsDbName()))
                .Join(",");
        }

        protected override long InternalExecute()
        {
			long iRows        = 0L;
			var  uniqueFields = this.GetUniqueFields();

            if (!string.IsNullOrEmpty(uniqueFields))
            {
                string query = String.Format(
                    "CREATE UNIQUE INDEX IF NOT EXISTS '{0}' ON {1}({2});",
                    this.GetTableName().AsIndexName(),
                    this.GetTableName().AsDbName(),
                    uniqueFields
                );

                Log.DebugFormat(
                    "Table:'{0}',Index:'{1}',Statement:'{2}'",
                    this.GetTableName().AsDbName(),
                    this.GetTableName().AsIndexName(),
                    query
                );

				iRows = this.ExecuteNonQuery(query, null);
            }

			return iRows;
        }
    }
}