using System.Collections.Generic;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Common
{
	public class PrimaryKey : ReadOnlyDictionary<string, object>
	{
		public PrimaryKey(IDictionary<string, object> keyValues) : base(keyValues)
		{
		}
	}
}
