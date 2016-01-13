using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using log4net;

namespace MSSQLServerAuditor.SQLite.Commands
{
    internal class SelectTableCommand : TableCommandBase
    {
        private readonly string _query;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IEnumerable<SQLiteParameter> _parameters;

        protected override ILog Logger
        {
            get
            {
                return Log;
            }
        }

        public SelectTableCommand(
            SQLiteConnection connection,
            TableDefinition tableDefinition,
            string query,
            IEnumerable<SQLiteParameter> parameters)
            : base(connection, tableDefinition)
        {
            this._query      = query;
            this._parameters = parameters;
        }

        public List<ITableRow> Result { get; private set; }

        protected override long InternalExecute()
        {
			long iRows = 0L;

			this.Result = new List<ITableRow>();

            // Log.DebugFormat("Table:'{0}',Query:'{1}'",
            //     this.TableDefinition == null
            //         ? String.Empty
            //         : this.TableDefinition.Name,
            //     this.query
            // );

            iRows = this.ExecuteQuery(
                this._query,
                reader => this.Result.Add(TableRow.Read(this.TableDefinition, reader)),
                this._parameters
            );

			return iRows;
        }
    }
}
