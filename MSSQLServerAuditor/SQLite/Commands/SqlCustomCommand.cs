using System;
using System.Data.SQLite;
using System.Reflection;
using log4net;

namespace MSSQLServerAuditor.SQLite.Commands
{
	public class SqlCustomCommand : CommandBase
	{
		private static readonly ILog       Log         = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly SQLiteParameter[] _parameters = null;
		private readonly string            _sql        = null;

		public SqlCustomCommand(
			SQLiteConnection         connection,
			string                   sql,
			params SQLiteParameter[] parameters
		) : base(
				connection
			)
		{
			this._sql        = sql;
			this._parameters = parameters;
		}

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		protected override long InternalExecute()
		{
			Log.DebugFormat("Datasource:'{0}';sql:'{1}'",
				this.Connection.DataSource,
				this._sql
			);

			return this.ExecuteNonQuery(
				this._sql,
				this._parameters
			);
		}
	}
}
