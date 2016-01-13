namespace MSSQLServerAuditor.SQLite.Common.Triggers
{
    using MSSQLServerAuditor.SQLite.Common.Definitions;
    using MSSQLServerAuditor.Utils;

    /// <summary>
    /// Default UPDATE trigger
    /// </summary>
    internal class UpdateTrigger : Trigger
    {
        private readonly string column;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Trigger name</param>
        /// <param name="tableDefinition">Table definition</param>
        /// <param name="sql">SQL code</param>
        /// <param name="column">Column name to update</param>
        public UpdateTrigger(string name, TableDefinition tableDefinition, string sql, string column)
            : base(name, tableDefinition, sql)
        {
            this.column = column;
        }

        /// <summary>
        /// Column name to update
        /// </summary>
        public string Column
        {
            get
            {
                return this.column;
            }
        }

        public override string GetCommand()
        {
            return
                string.Format(
                    "DROP TRIGGER IF EXISTS {0}; CREATE TRIGGER IF NOT EXISTS {0} AFTER UPDATE {2} ON [{1}] FOR EACH ROW BEGIN {3} END;",
                    this.Name.AsDbName(),
                    this.TableDefinition.Name,
                    (this.Column != null)
                        ? " OF " + this.Column
                        : "",
                    this.Sql);
        }
    }
}