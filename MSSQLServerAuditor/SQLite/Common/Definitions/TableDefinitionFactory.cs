namespace MSSQLServerAuditor.SQLite.Common.Definitions
{
	public static class TableDefinitionFactory
	{
		public static TableDefinition CreateWithAutoincrementKey(string tableName, string autoincrementField)
		{
			return new TableDefinition(tableName)
				.AddIntField(autoincrementField, false, true)
				.SetSimplePrimaryKey(autoincrementField, true);
		}
	}
}
