using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Common.Triggers
{
	/// <summary>
	/// Trigger for setting new value at row update
	/// </summary>
	internal class SetNewValueAtUpdateTrigger : UpdateTrigger
	{
		public SetNewValueAtUpdateTrigger(
			TableDefinition tableDefinition,
			string          targetColumn,
			string          identityColumn,
			string          newValue
		) : base(
				string.Format(
					"update_{0}_{1}",
					tableDefinition.Name,
					targetColumn
				),
				tableDefinition,
				string.Format(
					"UPDATE {0} SET {2} = {3} WHERE [{1}] = old.[{1}] AND ({2} IS NULL OR {2} != {3});",
					tableDefinition.Name.AsDbName(),
					identityColumn,
					targetColumn,
					newValue
				),
				null
			)
		{
		}
	}
}
