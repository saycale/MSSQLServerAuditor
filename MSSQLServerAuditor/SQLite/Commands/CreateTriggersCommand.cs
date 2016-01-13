namespace MSSQLServerAuditor.SQLite.Commands
{
	using System;
	using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;
    using System.Reflection;

    using log4net;

    using MSSQLServerAuditor.SQLite.Common.Definitions;

    internal class CreateTriggersCommand : TableCommandBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public CreateTriggersCommand(TableCommandBase tableCommandBase)
            : base (tableCommandBase)
        {
        }

        public CreateTriggersCommand(SQLiteConnection connection, TableDefinition tableDefinition)
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

        private IEnumerable<string> GetTriggersCommands()
        {
            return this.TableDefinition.Triggers
                .Select(t => t.GetCommand());
        }

        protected override long InternalExecute()
        {
			long iRows = 0L;

			foreach (var triggerCommand in this.GetTriggersCommands())
            {
                // Log.DebugFormat(
				//    "Trigger: '{0}'",
				//    triggerCommand
				// );

				iRows = this.ExecuteNonQuery(triggerCommand, null);
            }

			return 0L;
        }
    }
}