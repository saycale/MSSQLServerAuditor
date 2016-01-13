using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Commands
{
	internal class CreateTableCommand : TableCommandBase
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public CreateTableCommand(
			SQLiteConnection connection,
			TableDefinition  tableDefinition
		) : base(
				connection,
				tableDefinition
			)
		{
		}

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		private string GetTableFields()
		{
			PrimaryKeyDefinition primaryKey = TableDefinition.PrimaryKey;
			List<string> keyFields          = primaryKey.Fields.ToList();

			return this.TableDefinition.Fields.Values
				.Select(
					fieldDefinition =>
						{
							string strDefaultValue = string.Empty;
							string strPrimaryKey   = string.Empty;
							string strIsNotNull    = fieldDefinition.IsNotNull
								? " NOT NULL"
								: string.Empty;

							if (fieldDefinition.Default != null)
							{
								strDefaultValue = string.Format(
									" DEFAULT {0}",
									fieldDefinition.Default
								);
							}

							if (!primaryKey.IsEmpty && !primaryKey.IsCompound)
							{
								if (keyFields.Contains(fieldDefinition.Name))
								{
									strPrimaryKey = "PRIMARY KEY ASC";

									if (primaryKey.IsAutoincrement)
									{
										strPrimaryKey += " AUTOINCREMENT";
									}
								}
							}

							return string.Format(
								"{0} {1} {2} {3} {4}",
								fieldDefinition.Name.AsDbName(),
								fieldDefinition.SqlType.ToSQLiteDbType(),
								strIsNotNull,
								strDefaultValue,
								strPrimaryKey
							).Trim();
						}
				)
				.Join(",");
		}

		protected override long InternalExecute()
		{
			long   iRows                 = 0L;
			long   iTotalRows            = 0L;
			string tableFields           = this.GetTableFields();
			string tablePrimaryKeyFields = string.Empty;

			if (string.IsNullOrEmpty(tableFields))
			{
				return 0L;
			}

			PrimaryKeyDefinition key = TableDefinition.PrimaryKey;

			if (key.IsCompound)
			{
				string fields = key.Fields.Select(f => f.AsDbName()).Join(",");

				if (!fields.IsNullOrEmpty())
				{
					tablePrimaryKeyFields = string.Format(
						@", PRIMARY KEY ({0})",
						fields
					);
				}
			}

			string createTableQuery = string.Format(
				"CREATE TABLE IF NOT EXISTS {0}({1}{2});",
				this.TableDefinition.Name.AsDbName(),
				tableFields,
				tablePrimaryKeyFields
			);

			Log.DebugFormat("Create table:'{0}';Query:'{1}'",
				this.TableDefinition.Name.AsDbName(),
				createTableQuery
			);

			iRows = this.ExecuteNonQuery(createTableQuery, null);

			iTotalRows += iRows;

			// ticket #400: index for all columns is not required
			// new CreateIndexCommand(this).Execute(100);  // add index for ALL fields!

			new CreateTriggersCommand(this).Execute(100);

			foreach (IndexDefinition index in this.TableDefinition.Indexes)
			{
				this.ExecuteNonQuery(index.GetDdl(), null);

				iTotalRows += iRows;
			}

			return iTotalRows;
		}
	}
}
