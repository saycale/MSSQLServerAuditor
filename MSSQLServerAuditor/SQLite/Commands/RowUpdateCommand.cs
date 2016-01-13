using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Commands
{
	internal class RowUpdateCommand : TableCommandBase
	{
		private static readonly ILog     Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly List<ITableRow> _rows;
		private readonly string          _identityField;

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		public RowUpdateCommand(
			SQLiteConnection connection,
			TableDefinition  tableDefinition,
			string           identityField
		) : base(
				connection,
				tableDefinition
			)
		{
			this._rows          = new List<ITableRow>();
			this._identityField = identityField;
		}

		public void AddRowForUpdating(ITableRow row)
		{
			this._rows.Add(row);
		}

		protected override long InternalExecute()
		{
			long iRows                   = 0L;
			List<string>    queryColumns = new List<string>();
			SQLiteParameter idParam      = new SQLiteParameter("@" + this._identityField, DbType.Int64);

			Dictionary<string, SQLiteParameter> parametersCache = new Dictionary<string, SQLiteParameter>
			{
				{ this._identityField.ToUpper(), idParam }
			};

			if (this.TableDefinition != null)
			{
				foreach (FieldDefinition field in this.TableDefinition.Fields.Values)
				{
					parametersCache.Add(field.Name, new SQLiteParameter("@" + field.Name, field.SqlType.ToDbType()));

					queryColumns.Add(field.Name.AsSqlClausePair());
				}

				string updateSql = queryColumns.Join(", ");

				string sql = string.Format(
					"UPDATE [{0}] SET {1} WHERE [{2}] = @{2};",
					this.TableDefinition.Name,
					updateSql,
					this._identityField
				);

				Log.DebugFormat(
					"update: Table: '{0}', Query: '{1}'",
					this.TableDefinition.Name,
					sql
				);

				foreach (ITableRow row in this._rows)
				{
					foreach (KeyValuePair<string, object> pair in row.Values)
					{
						if (parametersCache.ContainsKey(pair.Key))
						{
							parametersCache[pair.Key].Value = pair.Value;
						}
					}

					idParam.Value = row.Values[this._identityField];

					iRows = this.ExecuteNonQuery(
						sql,
						parametersCache.Values
					);
				}
			}

			this._rows.Clear();

			return iRows;
		}
	}
}
