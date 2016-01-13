namespace MSSQLServerAuditor.SQLite.Common.Triggers
{
    using MSSQLServerAuditor.SQLite.Common.Definitions;

    /// <summary>
    /// Base Trigger class for SQLite DB Table
    /// </summary>
    public abstract class Trigger
    {
        private readonly string name;

        private readonly string sql;

        private readonly TableDefinition tableDefinition;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Trigger name</param>
        /// <param name="tableDefinition">Table definition for the trigger</param>
        /// <param name="sql">SQLite SQL code for trigger</param>
        protected Trigger(string name, TableDefinition tableDefinition, string sql)
        {
            this.name = name;
            this.tableDefinition = tableDefinition;
            this.sql = sql;
        }

        /// <summary>
        /// Trigger name
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Table definition
        /// </summary>
        public TableDefinition TableDefinition
        {
            get
            {
                return this.tableDefinition;
            }
        }

        /// <summary>
        /// Trigger SQL code
        /// </summary>
        public string Sql
        {
            get
            {
                return this.sql;
            }
        }

        /// <summary>
        /// Combine complete trigger code
        /// </summary>
        /// <returns></returns>
        public abstract string GetCommand();
    }
}