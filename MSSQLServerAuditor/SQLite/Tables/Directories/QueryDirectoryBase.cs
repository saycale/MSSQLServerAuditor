using System;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public abstract class QueryDirectoryBase : TableDirectory
	{
		protected QueryDirectoryBase(
			CurrentStorage   storage,
			TableDefinition  tableDefinition
		) : base(
				storage,
				tableDefinition
			)
		{
		}

		public abstract long? GetQueryId(
			TemplateNodeInfo      node,
			TemplateNodeQueryInfo templateNodeQuery,
			InstanceInfo          instance,
			DateTime              dateCreated,
			bool                  onlyFind
		);

		public abstract string DirectoryTableName { get; }
	}
}
