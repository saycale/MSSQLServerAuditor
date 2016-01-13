using System;
using System.Linq;
using MSSQLServerAuditor.SQLite.Common.Definitions;

namespace MSSQLServerAuditor.SQLite.Common
{
	public interface IAutoincrementTableRow : ITableRow
	{
		long Identity { get; }
	}

	public abstract class AutoincrementTableRow : TableRow, IAutoincrementTableRow
	{
		protected AutoincrementTableRow(TableDefinition tableDefinition)
			: base(tableDefinition)
		{
			PrimaryKeyDefinition keyDef = tableDefinition.PrimaryKey;

			if (!keyDef.IsAutoincrement)
			{
				throw new ArgumentException("AutoincrementTableRow should have numeric autoincrement primary key.");
			}
		}

		public long Identity
		{
			get
			{
				return (long) this.PrimaryKey.First().Value;
			}
		}
	}
}
