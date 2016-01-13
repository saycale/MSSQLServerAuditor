using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Common.Triggers;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Commands
{
	internal class ReplaceCommand : TableCommandBase
	{
		private static readonly ILog     Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly List<ITableRow> _rows;
		private SQLiteTransaction        _transaction;

		public ReplaceCommand(
			SQLiteConnection  connection,
			TableDefinition   tableDefinition,
			SQLiteTransaction transaction = null
		) : base(
				connection,
				tableDefinition
			)
		{
			this._rows        = new List<ITableRow>();
			this._transaction = transaction;
		}

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		public void AddRowForReplacing(ITableRow row)
		{
			this._rows.Add(row);
		}

		protected override long InternalExecute()
		{
			long   iTotalRows      = 0L;
			long   iRows           = 0L;
			string queryColumns    = "(";
			string queryValues     = "(";

			Dictionary<string, SQLiteParameter> parametersCache = 
				new Dictionary<string, SQLiteParameter>();

			if (this.TableDefinition != null)
			{
				IEnumerable<string> uniques =
					(from field in this.TableDefinition.Fields.Values
					 where field.Unique
					 select field.Name.AsSqlClausePair());

				string uniquePairs = uniques.Join(" AND ");

				bool ifFirst = true;

				foreach (FieldDefinition field in this.TableDefinition.Fields.Values)
				{
					parametersCache.Add(
						field.Name,
						new SQLiteParameter(field.Name.AsParamName(), field.SqlType.ToDbType())
					);

					string paramValue = field.Name.AsParamName();

					if (
						this.TableDefinition.Triggers.Any(
							trigger => trigger is PreserveTrigger && ((PreserveTrigger)trigger).Column == field.Name))
					{
						paramValue = string.Format(
							"COALESCE((SELECT {0} FROM {1} WHERE {2}), {3})",
							field.Name,
							this.TableDefinition.Name.AsDbName(),
							uniquePairs,
							field.Name.AsParamName()
						);
					}

					if (ifFirst)
					{
						queryColumns += field.Name.AsDbName();
						queryValues  += paramValue;

						ifFirst = false;
					}
					else
					{
						queryColumns += "," + field.Name.AsDbName();
						queryValues  += "," + paramValue;
					}
				}

				queryColumns += ")";
				queryValues += ")";

				string sql = string.Format(
					"INSERT OR REPLACE INTO {0} {1} VALUES {2};",
					this.TableDefinition.Name.AsDbName(),
					queryColumns,
					queryValues
				);

				for (int i = 0; i < this._rows.Count; i++)
				{
					ITableRow row = this._rows[i];

					foreach (KeyValuePair<string, object> pair in row.Values)
					{
						parametersCache[pair.Key].Value = pair.Value;
					}

					iRows = this.ExecuteNonQuery(
						sql,
						parametersCache.Values,
						this._transaction
					);

					iTotalRows += iRows;
				}

				this._rows.Clear();
			}

			return iTotalRows;
		}
	}
}
