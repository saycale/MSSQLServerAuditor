using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MSSQLServerAuditor.Model.Commands;
using MSSQLServerAuditor.SQLite.Commands;
using Teradata.Client.Provider;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	internal class TeradataSqlQueryConnection : DbQueryConnection<TdConnection>
	{
		public TeradataSqlQueryConnection(TdConnection tdConnection) : base(tdConnection)
		{
		}

		public override IQueryCommand GetCommand(
			string                          sqlText,
			int                             commandTimeout,
			IEnumerable<QueryParameterInfo> parameters,
			out List<Tuple<int, string>>    parametersQueueForODBC
		)
		{
			parametersQueueForODBC = new List<Tuple<int, string>>();

			TdCommand cmd = new TdCommand
			{
				Connection     = base.Connection,
				CommandText    = sqlText,
				CommandTimeout = commandTimeout
			};

			return new TeradataSqlCommand(cmd);
		}

		private class TeradataSqlCommand : IQueryCommand
		{
			private readonly TdCommand _command;

			public TeradataSqlCommand(TdCommand command)
			{
				this._command = command;
			}

			public void Dispose()
			{
				if (this._command != null)
				{
					this._command.Dispose();
				}
			}

			public void AssignParameters(
				IEnumerable<QueryParameterInfo> parameters,
				IEnumerable<ParameterValue>     parameterValues,
				List<Tuple<int, string>>        parametersQueueForOdbc
			)
			{
				if (parameters != null)
				{
					foreach (QueryParameterInfo parameter in parameters)
					{
						TdParameter p   = this._command.CreateParameter();
						p.ParameterName = parameter.Name;
						p.IsNullable    = true;
						p.DbType        = parameter.Type.ToDbType();
						p.Value         = parameter.GetDefaultValue();

						this._command.Parameters.Add(p);
					}
				}

				if (parameterValues != null)
				{
					foreach (ParameterValue value in parameterValues)
					{
						var parameter =
							(from TdParameter p in this._command.Parameters
							 where p.ParameterName == value.Name
							 select p)
							 .FirstOrDefault();

						if (parameter != null)
						{
							parameter.Value = value.GetValue(parameter.DbType);
						}
					}
				}
			}

			public IAsyncResult BeginExecuteReader(AsyncCallback callback)
			{
				if (this._command != null)
				{
					return this._command.BeginExecuteReader(callback, this);
				}

				return null;
			}

			public IDataReader EndExecuteReader(IAsyncResult result)
			{
				if (this._command != null)
				{
					return this._command.EndExecuteReader(result);
				}

				return null;
			}

			public void Cancel()
			{
				if (this._command != null)
				{
					this._command.Cancel();
				}
			}
		}
	}
}
