using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;

namespace MSSQLServerAuditor.SQLite.Common.Definitions
{
	/// <summary>
	/// Definition for SQLite Query
	/// </summary>
	public class QueryDefinition
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Query fields
		/// </summary>
		public Dictionary<string, FieldDefinition> Fields { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public QueryDefinition()
		{
			this.Fields = new Dictionary<string, FieldDefinition>();
		}

		public bool IsUnique(string name)
		{
			if (this.Fields.ContainsKey(name))
			{
				return this.Fields[name].Unique;
			}

			return false;
		}

		/// <summary>
		/// Get minimum common defintion
		/// </summary>
		/// <param name="equalityComparision">Field comparision function</param>
		/// <param name="tableDefinitions">Table definitions to find minimum</param>
		/// <returns></returns>
		public static IEnumerable<FieldDefinition> GetMinimumDefinitions(
			Func<FieldDefinition, FieldDefinition, bool> equalityComparision,
			params TableDefinition[]                     tableDefinitions
		)
		{
			bool                  canAdd = true;
			List<FieldDefinition> result = new List<FieldDefinition>();

			if (tableDefinitions.Length <= 0)
			{
				return result;
			}

			foreach (FieldDefinition field in tableDefinitions[0].Fields.Values)
			{
				// Log.DebugFormat(
				//    @"Column:Name:'{0}';Type:'{1}';IsPrimaryKey:'{2}';IsNotNull:'{3}';Unique:'{4}'",
				//    field.Name,
				//    field.SqlType,
				//    field.IsPrimaryKey,
				//    field.IsNotNull,
				//    field.Unique
				// );

				canAdd = true;

				for (int i = 1; i < tableDefinitions.Length; i++)
				{
					if (!tableDefinitions[i].Fields.Any(
						definition => equalityComparision(
							definition.Value,
							field
						)
					))
					{
						canAdd = false;

						break;
					}
				}

				// Log.DebugFormat(
				//    @"canAdd:'{0}'",
				//    canAdd
				// );

				if (canAdd)
				{
					result.Add(field);
				}
			}

			return result;
		}
	}
}
