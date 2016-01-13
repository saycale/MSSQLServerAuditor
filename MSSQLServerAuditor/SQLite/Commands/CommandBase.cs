using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;
using log4net;

namespace MSSQLServerAuditor.SQLite.Commands
{
	public abstract class CommandBase : IDbCommand
	{
		protected SQLiteConnection Connection { get; private set; }
		protected abstract ILog    Logger     { get; }

		protected CommandBase(CommandBase command) : this(command.Connection)
		{
		}

		protected CommandBase(SQLiteConnection connection)
		{
			this.Connection = connection;
		}

		private SQLiteCommand GetCommand(string sql, SQLiteTransaction transaction)
		{
			return new SQLiteCommand(
				sql,
				this.Connection,
				transaction
			);
		}

		protected long ExecuteNonQuery(
			string                       sql,
			IEnumerable<SQLiteParameter> parameters,
			SQLiteTransaction            transaction = null
		)
		{
			long iRows = 0L;

			this.Logger.DebugFormat("Datasource:'{0}';sql:'{1}'",
				this.Connection.DataSource,
				sql
			);

			using (SQLiteCommand command = this.GetCommand(sql, transaction))
			{
				if (parameters != null)
				{
					foreach (SQLiteParameter parameter in parameters)
					{
						command.Parameters.Add(parameter);
					}
				}

				iRows = command.ExecuteNonQuery();
			}

			// this.Logger.DebugFormat("ExecuteNonQuery:ExecuteNonQuery:sql:'{0}';rows:'{1}'",
			//    sql,
			//    iRows
			// );

			return iRows;
		}

		protected long ExecuteQuery(
			string                       sql,
			Action<SQLiteDataReader>     readFunc,
			IEnumerable<SQLiteParameter> parameters = null
		)
		{
			long iRows = 0L;

			using (var command = this.GetCommand(sql, null))
			{
				if (parameters != null)
				{
					foreach (SQLiteParameter parameter in parameters)
					{
						command.Parameters.Add(parameter);
					}
				}

				this.Logger.DebugFormat("Connection:'{0}';Datasource:'{1}';sql:'{2}'",
					command.Connection,
					this.Connection.DataSource,
					sql
				);

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						iRows++;

						readFunc(reader);
					}

					reader.Close();
				}
			}

			// this.Logger.DebugFormat("ExecuteQuery:sql:'{0}';rows:'{1}'",
			//    sql,
			//    iRows
			// );

			return iRows;
		}

		protected object ExecuteScalar(string sql, IEnumerable<SQLiteParameter> parameters = null)
		{
			// this.Logger.DebugFormat("ExecuteScalar:sql:'{0}'",
			//    sql
			// );

			using (var command = this.GetCommand(sql, null))
			{
				if (parameters != null)
				{
					foreach (SQLiteParameter parameter in parameters)
					{
						// this.Logger.DebugFormat("Name:'{0}',Value:'{1}'",
						//    parameter.ParameterName,
						//    parameter.Value
						// );

						command.Parameters.Add(parameter);
					}
				}

				return command.ExecuteScalar();
			}
		}

		public long Execute(int intCommandAttempts)
		{
			bool   boolContinueTrying      = true;
			int    intCommandAttemptNumber = 0;
			long   iRows                   = 0L;
			Random myRandomValue           = new Random();

			while (boolContinueTrying)
			{
				intCommandAttemptNumber++;

				try
				{
					iRows              = this.InternalExecute();
					boolContinueTrying = false;
				}
				catch (SQLiteException ex)
				{
					if (intCommandAttemptNumber < intCommandAttempts)
					{
						if (ex.ResultCode == SQLiteErrorCode.Busy)
						{
							this.Logger.ErrorFormat("SQLite is busy:Attempt:'{0}',Error:'{1}'",
								intCommandAttemptNumber,
								ex
							);

							Thread.Sleep(myRandomValue.Next(100, 200));
						}
						else if (ex.ResultCode == SQLiteErrorCode.Locked)
						{
							this.Logger.ErrorFormat("SQLite is locked:Attempt:'{0}',Error:'{1}'",
								intCommandAttemptNumber,
								ex
							);

							Thread.Sleep(myRandomValue.Next(100, 200));
						}
						else
						{
							boolContinueTrying = false;

							this.Logger.Error("Error:", ex);

							// throw;
						}
					}
					else
					{
						boolContinueTrying = false;

						this.Logger.Error("Error:", ex);

						// throw;
					}
				}
				catch (Exception ex)
				{
					boolContinueTrying = false;

					this.Logger.Error("Error:", ex);

					// throw;
				}
			}

			return iRows;
		}

		protected abstract long InternalExecute();
	}
}
