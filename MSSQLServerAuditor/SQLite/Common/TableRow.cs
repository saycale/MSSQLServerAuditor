using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.Linq;
using MSSQLServerAuditor.SQLite.Common.Definitions;

namespace MSSQLServerAuditor.SQLite.Common
{
	/// <summary>
	/// Table row in SQLite DB
	/// </summary>
	public class TableRow : ITableRow
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="tableDefinition">Table definition</param>
		public TableRow(TableDefinition tableDefinition)
		{
			this.TableDefinition = tableDefinition;
			this.Values          = new Dictionary<string, object>();
		}

		public TableDefinition TableDefinition { get; private set; }

		/// <summary>
		/// Values of row
		/// </summary>
		public Dictionary<string, object> Values { get; private set; }

		public PrimaryKey PrimaryKey
		{
			get
			{
				return new PrimaryKey(TableDefinition.PrimaryKey
					.Fields.ToDictionary(pkField => pkField, pkField => Values[pkField]));
			}
		}

		/// <summary>
		/// Read rows
		/// </summary>
		/// <param name="tableDefinition">Table definition</param>
		/// <param name="reader">Reader</param>
		/// <returns></returns>
		public static ITableRow Read(TableDefinition tableDefinition, SQLiteDataReader reader)
		{
			ITableRow           result = new TableRow(tableDefinition);
			NameValueCollection values = reader.GetValues();
			string[]            fields = values.AllKeys;

			foreach (FieldDefinition fieldDefinition in result.TableDefinition.Fields.Values)
			{
				string fieldName = fieldDefinition.Name;

				if (fields.Contains(fieldName))
				{
					result.Values.Add(fieldName, reader[fieldName]);
				}
			}

			if (values.AllKeys.Contains("rowid"))
			{
				result.Values["rowid"] = (Int64) reader["rowid"];
			}

			return result;
		}

		/// <summary>
		/// Copy values to new row
		/// </summary>
		/// <param name="destinationRow">Destination row</param>
		public bool CopyValues(ITableRow destinationRow)
		{
			bool hasChanges = false;

			foreach (KeyValuePair<string, object> pair in this.Values)
			{
				object existingValue;

				if (destinationRow.Values.TryGetValue(pair.Key, out existingValue))
				{
					hasChanges = hasChanges || !this.AreEqual(existingValue, pair.Value);

					destinationRow.Values[pair.Key] = pair.Value;
				}
				else
				{
					destinationRow.Values.Add(pair.Key, pair.Value);
				}
			}

			return hasChanges;
		}

		private bool AreEqual(object value1, object value2)
		{
			if ((value1 is DBNull && value2 == null) || (value2 is DBNull && value1 == null))
			{
				return true;
			}

			return Equals(value1, value2);
		}

		public T GetValue<T>(string name, T defaultValue = default(T))
		{
			if (this.Values.ContainsKey(name))
			{
				object value = this.Values[name];

				if (value is T)
				{
					return (T)value;
				}
			}

			return defaultValue;
		}

		public void SetValue(string name, object value)
		{
			this.Values[name] = value;
		}
	}
}
