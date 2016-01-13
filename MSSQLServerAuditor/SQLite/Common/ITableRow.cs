using System.Collections.Generic;
using MSSQLServerAuditor.SQLite.Common.Definitions;

namespace MSSQLServerAuditor.SQLite.Common
{
	public interface ITableRow
	{
		TableDefinition TableDefinition { get; }

		PrimaryKey PrimaryKey { get; }

		/// <summary>
		/// Values of row
		/// </summary>
		Dictionary<string, object> Values { get; }

		/// <summary>
		/// Copy values to new row
		/// </summary>
		/// <param name="destinationRow">Destination row</param>
		bool CopyValues(ITableRow destinationRow);

		T GetValue<T>(string name, T defaultValue = default(T));

		void SetValue(string name, object value);
	}
}
