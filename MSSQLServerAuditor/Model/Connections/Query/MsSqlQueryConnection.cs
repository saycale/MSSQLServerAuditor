using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MSSQLServerAuditor.Model.Commands;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	internal class MsSqlQueryConnection : DbQueryConnection<SqlConnection>
	{
		public MsSqlQueryConnection(SqlConnection sqlConnection) : base(sqlConnection)
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

			SqlCommand cmd = new SqlCommand
			{
				Connection     = base.Connection,
				CommandText    = sqlText,
				CommandTimeout = commandTimeout
			};

			return new MsSqlCommand(cmd);
		}

		private class MsSqlCommand : IQueryCommand
		{
			private readonly SqlCommand _command;

			public MsSqlCommand(SqlCommand command)
			{
				this._command = command;
			}

			public void Dispose()
			{
				this._command.Dispose();
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
						SqlParameter p  = this._command.CreateParameter();
						p.ParameterName = parameter.Name;
						p.IsNullable    = true;
						p.SqlDbType     = parameter.Type;
						p.Value         = parameter.GetDefaultValue();
						this._command.Parameters.Add(p);
					}
				}

				if (parameterValues != null)
				{
					foreach (ParameterValue value in parameterValues)
					{
						SqlParameter parameter =
							(from SqlParameter p in this._command.Parameters
							 where p.ParameterName == value.Name
							 select p)
							.FirstOrDefault();

						if (parameter != null)
						{
							parameter.Value = value.GetValue(parameter.SqlDbType);
						}
					}
				}
			}

			public IAsyncResult BeginExecuteReader(AsyncCallback callback)
			{
				return this._command.BeginExecuteReader(callback, this);
			}

			public IDataReader EndExecuteReader(IAsyncResult result)
			{
				return this._command.EndExecuteReader(result);
			}

			public void Cancel()
			{
				this._command.Cancel();
			}
		}
	}
}
