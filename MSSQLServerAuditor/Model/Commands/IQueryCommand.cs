using System;
using System.Collections.Generic;
using System.Data;

namespace MSSQLServerAuditor.Model.Commands
{
	public interface IQueryCommand : IDisposable
	{
		void AssignParameters(
			IEnumerable<QueryParameterInfo> parameters,
			IEnumerable<ParameterValue>     parameterValues,
			List<Tuple<int, string>>        parametersQueueForOdbc
		);

		IAsyncResult BeginExecuteReader(AsyncCallback callback);

		IDataReader EndExecuteReader(IAsyncResult result);

		void Cancel();
	}
}
