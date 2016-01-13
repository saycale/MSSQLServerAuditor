using System.Collections.Generic;
using System.Data.SQLite;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common.Definitions;

namespace MSSQLServerAuditor.SQLite.Common.Queries
{
	internal class QueryRow
	{
		private readonly QueryDefinition            queryDefinition;
		private readonly Dictionary<string, object> values;

		public QueryRow()
		{
			this.queryDefinition = new QueryDefinition();
			this.values          = new Dictionary<string, object>();
		}

		public Dictionary<string, object> Values
		{
			get
			{
				return this.values;
			}
		}

		public static QueryRow Read(SQLiteDataReader reader)
		{
			var result = new QueryRow();

			reader.GetValues();

			for (var i = 0; i < reader.FieldCount; i++)
			{
				result.queryDefinition.Fields.Add(
					reader.GetName(i),
					new FieldDefinition(
						reader.GetName(i),
						reader.GetFieldType(i).ToSqlDbType(),
						false,
						false
					)
				);

				result.values.Add(
					reader.GetName(i),
					reader.GetValue(i)
				);
			}

			return result;
		}
	}
}
