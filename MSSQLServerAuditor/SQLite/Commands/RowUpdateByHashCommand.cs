using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Commands
{
	internal class RowUpdateByHashCommand : TableCommandBase
	{
		private static readonly ILog  Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private string                _clause;
		private string                _setString;
		private bool                  _updateTable;
		private List<SQLiteParameter> _parameters;
		private ITableRow             _row;
		private SQLiteTransaction     _transaction;

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		public RowUpdateByHashCommand(
			SQLiteConnection connection,
			TableDefinition  tableDefinition
		) : base(
				connection,
				tableDefinition
			)
		{
			this._clause      = null;
			this._setString   = null;
			this._updateTable = false;
			this._parameters  = null;
			this._row         = null;
			this._transaction = null;
		}

		public void SetCommandConstraints(
			string                clause,
			List<SQLiteParameter> parameters,
			string                setString,
			bool                  updateTable,
			ITableRow             row,
			SQLiteTransaction     transaction = null
		)
		{
			this._clause      = clause;
			this._parameters  = parameters;
			this._setString   = setString;
			this._updateTable = updateTable;
			this._row         = row;
			this._transaction = transaction;
		}

		protected override long InternalExecute()
		{
			long   iRows            = 0L;
			var    queryColumns     = new List<string>();
			string strUpdateColumns = String.Empty;
			string strSQLUpdate     = String.Empty;

			if (this.TableDefinition != null)
			{
				if (this._updateTable)
				{
					var parametersCache = new Dictionary<string, SQLiteParameter>();

					foreach (var field in this.TableDefinition.Fields.Values)
					{
						parametersCache.Add(field.Name, new SQLiteParameter("@" + field.Name, field.SqlType.ToDbType()));

						queryColumns.Add(field.Name.AsSqlClausePair());
					}

					foreach (var pair in this._row.Values)
					{
						if (parametersCache.ContainsKey(pair.Key))
						{
							parametersCache[pair.Key].Value = pair.Value;
						}
					}

					this._parameters.AddRange(parametersCache.Values);

					strUpdateColumns = queryColumns.Join(", ");
				}
				else
				{
					strUpdateColumns = this._setString;
				}

				strSQLUpdate = string.Format(
					"Update [{0}] Set {1} {2};",
					this.TableDefinition.Name,
					strUpdateColumns,
					string.IsNullOrEmpty(this._clause)
						? ""
						: " WHERE " + this._clause
					);

				// Log.DebugFormat(
				//    "Table:'{0}',Where:'{1}',Query:'{2}'",
				//    this.TableDefinition.Name,
				//    this._clause,
				//    strSQLUpdate
				// );

				iRows = this.ExecuteNonQuery(
					strSQLUpdate,
					this._parameters,
					this._transaction
				);
			}

			return iRows;
		}
	}
}
