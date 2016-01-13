using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Common.Triggers
{
	/// <summary>
	/// Value preserve trigger
	/// </summary>
	internal class PreserveTrigger : UpdateTrigger
	{
		public PreserveTrigger(
			TableDefinition tableDefinition,
			string          column,
			string          triggerAssociatedColumn = "rowid"
		) : base(
				string.Format(
					"preserve_{0}_{1}",
					tableDefinition.Name,
					column
				),
				tableDefinition,
				string.Format(
					"UPDATE {0} SET {1} = old.{1} WHERE [{2}] = old.[{2}] AND {1} != old.{1};",
					tableDefinition.Name.AsDbName(),
					column, 
					triggerAssociatedColumn
				),
				column
			)
		{
		}
	}
}
