using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Commands
{
	internal class GetTableDefinitionCommand : CommandBase
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly string      tableName;

		public GetTableDefinitionCommand(SQLiteConnection connection, string strTableName)
			: base(connection)
		{
			this.tableName       = strTableName;
			this.TableDefinition = null;
		}

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		public TableDefinition TableDefinition
		{
			get;
			private set;
		}

		protected override long InternalExecute()
		{
			this.TableDefinition = new TableDefinition(this.tableName);

			HashSet<string> unique = new HashSet<string>();

			string sql = string.Format(
				@"PRAGMA index_info('{0}')",
				this.TableDefinition.GetIndexName()
			);

			// Log.DebugFormat(
			//    "Table:'{0}',Index:'{1}',Query:'{2}'",
			//    this.tableName,
			//    this.TableDefinition.GetIndexName(),
			//    sql
			// );

			this.ExecuteQuery(
				sql,
				reader => unique.Add((string)reader["name"])
			);

			if (this.tableName != null)
			{
				sql = string.Format(
					@"PRAGMA table_info({0})",
					this.tableName.AsDbName()
				);

				// Log.DebugFormat("Table:'{0}',Query:'{1}'",
				//    this.tableName,
				//    sql
				// );

				List<string> primaryKeyFields = new List<string>();
				this.ExecuteQuery(
					sql,
					reader =>
						{
							string fieldName = (string) reader["name"];
							SQLiteFieldType fieldType = SQLiteTypeHelper.Parse(reader["type"].ToString());
							this.TableDefinition.AddField(
								new FieldDefinition(
									fieldName,
									fieldType.ToSqlDbType(),
									unique.Contains(fieldName),
									(reader["notnull"] != DBNull.Value) && (long) reader["notnull"] == 1L,
									(reader["dflt_value"] == DBNull.Value)
										? null
										: reader["dflt_value"]
									)
								);

							object pk = reader["pk"];

							if (pk != DBNull.Value && (long) pk > 0L)
							{
								primaryKeyFields.Add(fieldName);
							}
						}
					);

				if (this.TableDefinition.Fields.Count == 0)
				{
					this.TableDefinition = null;
				}
				else
				{
					if (primaryKeyFields.Count == 1)
					{
						string keyField = primaryKeyFields[0];

						TableDefinition.SetSimplePrimaryKey(
							keyField,
							TableDefinition.Fields[keyField].SqlType == SqlDbType.BigInt
						);
					}
					else
					{
						TableDefinition.SetCompoundPrimaryKey(
							primaryKeyFields
						);
					}
				}
			}

			return 0L;
		}
	}
}
