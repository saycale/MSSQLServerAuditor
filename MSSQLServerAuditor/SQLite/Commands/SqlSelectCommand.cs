using System;
using System.Data.SQLite;
using System.Reflection;
using log4net;

namespace MSSQLServerAuditor.SQLite.Commands
{
	public class SqlSelectCommand : CommandBase
	{
		private static readonly ILog              Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly SQLiteParameter[]        _parameters;
		private readonly string                   _query;
		private readonly Action<SQLiteDataReader> _selectAction;

		public SqlSelectCommand(
			SQLiteConnection         connection,
			string                   query,
			Action<SQLiteDataReader> selectAction,
			params SQLiteParameter[] parameters
		) : base(connection)
		{
			this._query        = query;
			this._selectAction = selectAction;
			this._parameters   = parameters;
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
			return this.ExecuteQuery(
				this._query,
				this._selectAction,
				this._parameters
			);
		}
	}
}
