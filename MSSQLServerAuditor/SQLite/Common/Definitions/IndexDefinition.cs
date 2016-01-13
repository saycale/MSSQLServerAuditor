using System.Linq;

namespace MSSQLServerAuditor.SQLite.Common.Definitions
{
	public class IndexDefinition
	{
		private readonly string[]        _fieldNames;
		private readonly string          _name;
		private readonly TableDefinition _table;
		private readonly bool            _isUnique;

		private IndexDefinition()
		{
			this._fieldNames = null;
			this._name       = null;
			this._table      = null;
			this._isUnique   = false;
		}

		public IndexDefinition(
			TableDefinition table,
			string          name,
			bool            isUnique,
			params string[] fieldNames
		) : this()
		{
			this._fieldNames = fieldNames.ToArray();
			this._table      = table;
			this._name       = name;
			this._isUnique   = isUnique;
		}

		public bool IsComposedUnique
		{
			get
			{
				bool isComposedUnique = false;

				if (this._fieldNames != null)
				{
					isComposedUnique = ((this._isUnique) && (this._fieldNames.Length > 1));
				}

				return isComposedUnique;
			}
		}

		public string[] FieldNames
		{
			get
			{
				return this._fieldNames;
			}
		}

		public string GetDdl()
		{
			string strCreateIndexDDL = string.Empty;

			if (this._isUnique)
			{
				strCreateIndexDDL = string.Format(
					"CREATE UNIQUE INDEX IF NOT EXISTS [{0}] ON [{1}] ({2});",
					this._name,
					this._table.Name,
					string.Join(", ", this._fieldNames)
				);
			}
			else
			{
				strCreateIndexDDL = string.Format(
					"CREATE INDEX IF NOT EXISTS [{0}] ON [{1}] ({2});",
					this._name,
					this._table.Name,
					string.Join(", ", this._fieldNames)
				);
			}

			return strCreateIndexDDL;
		}
	}
}
