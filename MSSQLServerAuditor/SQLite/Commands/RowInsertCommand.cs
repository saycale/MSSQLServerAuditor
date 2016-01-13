using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Commands
{
	internal class RowInsertCommand : TableCommandBase
	{
		private static readonly ILog            Log  = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly        List<ITableRow> _rows;

		public RowInsertCommand(TableCommandBase tableCommandBase)
			: base(tableCommandBase)
		{
			this._rows = null;
		}

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		public RowInsertCommand(SQLiteConnection connection, TableDefinition tableDefinition)
			: base(connection, tableDefinition)
		{
			this._rows = new List<ITableRow>();
		}

		public void AddRowForInserting(ITableRow row)
		{
			this._rows.Add(row);
		}

		protected override long InternalExecute()
		{
			long   iRows           = 0L;
			long   iTotalRows      = 0L;
			bool   ifFirst         = true;
			string strQueryColumns = null;
			string strQueryValues  = null;

			if (this.TableDefinition != null)
			{
				Dictionary<string, SQLiteParameter> parametersCache =
					this.TableDefinition.Fields.Values.ToDictionary(
						field => field.Name,
						field => new SQLiteParameter(
							field.Name.AsParamName(),
							field.SqlType.ToDbType()
						)
					);

				foreach (ITableRow row in this._rows)
				{
					ifFirst         = true;
					strQueryColumns = "(";
					strQueryValues  = "(";

					foreach (FieldDefinition field in this.TableDefinition.Fields.Values)
					{
						parametersCache[field.Name].Value = null;

						if ((field.Default == null) || (row.Values.ContainsKey(field.Name)))
						{
							if (ifFirst)
							{
								ifFirst = false;
							}
							else
							{
								strQueryColumns += ",";
								strQueryValues  += ",";
							}

							strQueryColumns += ""  + field.Name.AsDbName();
							strQueryValues  += "@" + field.Name;
						}
					}

					strQueryColumns += ")";
					strQueryValues += ")";

					string sql = string.Format("INSERT INTO {0} {1} VALUES {2};",
						this.TableDefinition.Name.AsDbName(),
						strQueryColumns,
						strQueryValues
					);

					// Log.DebugFormat("Table:'{0}',Query:'{1}'",
					//    this.TableDefinition.Name.AsDbName(),
					//    sql
					// );

					foreach (KeyValuePair<string, object> pair in row.Values)
					{
						if (parametersCache.ContainsKey(pair.Key))
						{
							// Log.DebugFormat("Key:'{0}',Value:'{1}'",
							//    pair.Key,
							//    pair.Value
							// );

							parametersCache[pair.Key].Value = pair.Value;
						}
					}

					iRows = this.ExecuteNonQuery(
						sql,
						parametersCache.Values
					);

					iTotalRows += iRows;
				}

				this._rows.Clear();
			}

			return iTotalRows;
		}
	}
}
