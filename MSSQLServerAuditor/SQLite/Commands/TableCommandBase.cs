namespace MSSQLServerAuditor.SQLite.Commands
{
    using System.Data.SQLite;

    using MSSQLServerAuditor.SQLite.Common.Definitions;

    public abstract class TableCommandBase : CommandBase
    {
        protected TableDefinition TableDefinition { get; private set; }

		protected TableCommandBase(TableCommandBase tableCommandBase)
            : this(tableCommandBase.Connection, tableCommandBase.TableDefinition)
        {
        }

        protected TableCommandBase(SQLiteConnection connection, TableDefinition tableDefinition)
            : base (connection)
        {
            this.TableDefinition = tableDefinition;
        }
    }
}