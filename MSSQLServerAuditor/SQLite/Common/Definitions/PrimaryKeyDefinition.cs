using System.Collections.Generic;
using System.Linq;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Common.Definitions
{
	public class PrimaryKeyDefinition
	{
		private readonly List<string> _fields;
		private readonly bool         _isAutoIncrement;

		public static readonly PrimaryKeyDefinition Empty = new PrimaryKeyDefinition(
			Enumerable.Empty<string>()
		);

		private PrimaryKeyDefinition(IEnumerable<string> fields)
		{
			this._fields          = new List<string>(fields);
			this._isAutoIncrement = false;
		}

		private PrimaryKeyDefinition(
			string field,
			bool   isAutoIncrement = false
		)
		{
			this._fields          = Lists.Of(field);
			this._isAutoIncrement = isAutoIncrement;
		}

		public static PrimaryKeyDefinition CreateSimple(
			string fieldName,
			bool   isAutoIncrement
		)
		{
			return new PrimaryKeyDefinition(fieldName, isAutoIncrement);
		}

		public static PrimaryKeyDefinition CreateCompound(
			IEnumerable<string> fields
		)
		{
			return new PrimaryKeyDefinition(fields);
		}

		public bool IsEmpty
		{
			get { return this == Empty; }
		}

		public IEnumerable<string> Fields
		{
			get { return this._fields; }
		}

		public bool IsCompound
		{
			get { return this._fields.Count > 1; }
		}

		public bool IsAutoincrement
		{
			get
			{
				if (this._fields.Count > 1)
				{
					return false;
				}

				return this._isAutoIncrement;
			}
		}
	}
}