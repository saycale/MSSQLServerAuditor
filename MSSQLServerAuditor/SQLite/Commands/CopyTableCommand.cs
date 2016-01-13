using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Commands
{
	internal class CopyTableCommand : TableCommandBase
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private IEnumerable<FieldDefinition> fieldDefinitions;

		private readonly TableDefinition toTable;

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		public CopyTableCommand(SQLiteConnection connection, TableDefinition fromTable, TableDefinition toTable)
			: base(connection, fromTable)
		{
			this.toTable = toTable;
		}

		public void SetFieldDefinitions(IEnumerable<FieldDefinition> fieldDefinitions)
		{
			this.fieldDefinitions = fieldDefinitions;
		}

		protected override long InternalExecute()
		{
			var fields        = "*";
			var insertCommand = new RowInsertCommand(this.Connection, this.toTable);

			if (this.fieldDefinitions != null)
			{
				fields = this.fieldDefinitions.Select(definition => definition.Name).Join(",");
			}

			if (string.IsNullOrEmpty(fields))
			{
				return 0;
			}

			if (this.TableDefinition != null)
			{
				string sql = string.Format("SELECT {1} FROM {0}",
					this.TableDefinition.Name.AsDbName(),
					fields
				);

				// Log.DebugFormat(
				// 	"Copy Table:From:'{0}',To:'{1}',Query:'{2}'",
				// 	this.TableDefinition.Name.AsDbName(),
				// 	this.toTable,
				// 	sql
				// 	);

				this.ExecuteQuery(
					sql,
					reader =>
						{
							var row = TableRow.Read(this.TableDefinition, reader);

							insertCommand.AddRowForInserting(row);

							insertCommand.Execute(100);
						});
			}

			return 0L;
		}
	}
}
