using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Common.Definitions;

namespace MSSQLServerAuditor.SQLite.Commands
{
	internal class RowDeleteCommand : TableCommandBase
	{
		private static readonly ILog         Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private string                       _clause = null;
		private IEnumerable<SQLiteParameter> _parameters = null;
		private SQLiteTransaction            _transaction;

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		public RowDeleteCommand(
			SQLiteConnection connection,
			TableDefinition  tableDefinition
		) : base(
				connection,
				tableDefinition
			)
		{
			this._clause      = null;
			this._parameters  = null;
			this._transaction = null;
		}

		public void SetCommandConstraints(
			string                       clause,
			IEnumerable<SQLiteParameter> parameters,
			SQLiteTransaction            transaction = null
		)
		{
			this._clause      = clause;
			this._parameters  = parameters;
			this._transaction = transaction;
		}

		protected override long InternalExecute()
		{
			long iRows = 0L;

			if (this.TableDefinition != null)
			{
				string sql = string.Format(
					"DELETE FROM [{0}]{1};",
					this.TableDefinition.Name,
					string.IsNullOrEmpty(this._clause)
						? ""
						: " WHERE " + this._clause
				);

				Log.DebugFormat(
					"Table:'{0}',Where:'{1}',Query:'{2}'",
					this.TableDefinition.Name,
					this._clause,
					sql
				);

				iRows = this.ExecuteNonQuery(
					sql,
					this._parameters,
					this._transaction
				);
			}

			return iRows;
		}
	}
}
