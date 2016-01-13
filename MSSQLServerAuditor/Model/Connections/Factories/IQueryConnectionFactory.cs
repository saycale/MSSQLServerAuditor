using System;
using MSSQLServerAuditor.Model.Connections.Query;

namespace MSSQLServerAuditor.Model.Connections.Factories
{
	public interface IQueryConnectionFactory : IDisposable
	{
		IQueryConnection CreateQueryConnection(
			QuerySource  sourceType,
			InstanceInfo instance
		);
	}
}
