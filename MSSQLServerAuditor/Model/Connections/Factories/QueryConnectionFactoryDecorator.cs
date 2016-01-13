using System;
using MSSQLServerAuditor.Model.Connections.Query;

namespace MSSQLServerAuditor.Model.Connections.Factories
{
	public abstract class QueryConnectionFactoryDecorator : IQueryConnectionFactory
	{
		protected readonly IQueryConnectionFactory _connectionFactory;

		public QueryConnectionFactoryDecorator(IQueryConnectionFactory connectionFactory)
		{
			if (connectionFactory == null)
			{
				throw new ArgumentNullException("connectionFactory");
			}

			this._connectionFactory = connectionFactory;
		}

		public virtual void Dispose()
		{
		}

		public virtual IQueryConnection CreateQueryConnection(QuerySource sourceType, InstanceInfo instance)
		{
			return this._connectionFactory.CreateQueryConnection(sourceType, instance);
		}
	}
}
