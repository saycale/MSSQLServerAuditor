using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using log4net;

namespace MSSQLServerAuditor.SQLite.Commands
{
	public class ReadTableCommand : CommandBase
	{
		private static readonly ILog                  Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IEnumerable<SQLiteParameter> parameters;
		private readonly string                       query;

		public ReadTableCommand(
			SQLiteConnection         connection,
			string                   query,
			params SQLiteParameter[] parameters
		) : base(connection)
		{
			this.query      = query;
			this.parameters = parameters;
		}

		protected override ILog Logger
		{
			get
			{
				return Log;
			}
		}

		public DataTable Result
		{
			get;
			private set;
		}

		public DataTable ReadDataTable(
			string                       sql,
			IEnumerable<SQLiteParameter> parameters = null)
		{
			DataTable result = null;

			try
			{
				using (var command = this.Connection.CreateCommand())
				{
					command.CommandText = sql;

					if (parameters != null)
					{
						foreach (var parameter in parameters)
						{
							command.Parameters.Add(parameter);
						}
					}

					result = new DataTable();

					using (var reader = command.ExecuteReader())
					{
						result.Load(reader);
					}
				}
			}
			catch (Exception ex)
			{
				this.Logger.Error(sql, ex);
			}

			return result;
		}

		protected override long InternalExecute()
		{
			this.Result = this.ReadDataTable(this.query, this.parameters);

			return 0L;
		}
	}
}
