using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.SQLite.Common
{
	public class CurrentStorageTable : Table
	{
		protected CurrentStorageTable(CurrentStorage storage, TableDefinition tableDefinition)
			: base(storage.Connection, tableDefinition)
		{
			Storage = storage;
		}

		internal CurrentStorage Storage { get; private set; }
	}
}
