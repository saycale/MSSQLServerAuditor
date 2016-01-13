using System;
using System.Data.SQLite;
using System.Text;
using MSSQLServerAuditor.Cache;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	/// <summary>
	/// Abstract table directory
	/// </summary>
	public abstract class TableDirectory : CurrentStorageTable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="tableDefinition"></param>
		/// <param name="readOnly">Is table read-only?</param>
		/// <param name="tableName">Table name</param>
		protected TableDirectory(
			CurrentStorage   storage,
			TableDefinition  tableDefinition
		) : base(
				storage,
				tableDefinition
			)
		{
		}

		public bool ReadOnly
		{
			get { return Storage.ReadOnly; }
		}

		public MemoryCache Cache
		{
			get { return Storage.MemoryCache; }
		}

		protected virtual void BeforeRowUpdate(ITableRow row)
		{
		}

		protected virtual void BeforeRowAdd(ITableRow row)
		{
		}

		/// <summary>
		/// Get Row id for field
		/// </summary>
		/// <param name="customFields">Custom fields</param>
		/// <param name="field1">Field value</param>
		/// <returns></returns>
		protected Int64? GetRecordIdByFields(params Tuple<string, object>[] fields)
		{
			return this.Cache.GetOrAdd(
				this.GetCacheKey(fields),
				() => this.AddRecordIdByFields(fields)
			);
		}

		public Int64? AddRecordIdByFields(params Tuple<string, object>[] fields)
		{
			var row = this.NewRow();

			foreach (var field in fields)
			{
				row.Values.Add(field.Item1, field.Item2);
			}

			if (this.ReadOnly)
			{
				return this.GetRow(row);
			}

			return this.InsertOrUpdateRow(row, this.BeforeRowUpdate, this.BeforeRowAdd);
		}

		protected string GetCacheKey(params Tuple<string, object>[] fields)
		{
			var sb = new StringBuilder()
				.Append(this.TableDefinition.Name);

			foreach (var field in fields)
			{
				if (this.TableDefinition.IsUnique(field.Item1))
				{
					sb.Append(field.Item2);
				}
			}

			return sb.ToString();
		}

		protected Tuple<string, object> CreateField(string name, object value)
		{
			return Tuple.Create(name, value);
		}
	}
}
