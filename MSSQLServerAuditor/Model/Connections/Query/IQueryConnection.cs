using System;
using System.Collections.Generic;
using System.Data;
using MSSQLServerAuditor.Model.Commands;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	public interface IQueryConnection : IDisposable
	{
		void Open();

		void ChangeDatabase(string database);

		void Close();

		ConnectionState? State { get; }

		IQueryCommand GetCommand(
			string                          sqlText,
			int                             commandTimeout,
			IEnumerable<QueryParameterInfo> parameters,
			out List<Tuple<int, string>>    parametersQueueForODBC
		);
	}
}
