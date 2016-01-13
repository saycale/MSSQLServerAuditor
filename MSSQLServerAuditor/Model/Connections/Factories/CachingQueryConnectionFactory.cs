using System.Collections.Generic;
using System.Data;
using MSSQLServerAuditor.Model.Connections.Query;

namespace MSSQLServerAuditor.Model.Connections.Factories
{
	public class CachingQueryConnectionFactory : QueryConnectionFactoryDecorator
	{
		private readonly object                                     _connectionPoolLock;
		private readonly Dictionary<InstanceInfo, IQueryConnection> _connectionCache;

		public CachingQueryConnectionFactory(IQueryConnectionFactory connectionFactory)
			: base(connectionFactory)
		{
			this._connectionPoolLock = new object();
			this._connectionCache    = new Dictionary<InstanceInfo, IQueryConnection>();
		}

		public override void Dispose()
		{
			foreach (KeyValuePair<InstanceInfo, IQueryConnection> pair in this._connectionCache)
			{
				pair.Value.Dispose();
			}
		}

		public override IQueryConnection CreateQueryConnection(
			QuerySource  sourceType,
			InstanceInfo instance
		)
		{
			IQueryConnection connection;

			lock (this._connectionPoolLock)
			{
				if (!this._connectionCache.ContainsKey(instance))
				{
					connection = base.CreateQueryConnection(sourceType, instance);

					this._connectionCache.Add(instance, connection);
				}

				connection = this._connectionCache[instance];
			}

			if (connection != null)
			{
				if (connection.State != ConnectionState.Closed)
				{
					connection.Close();
				}
			}

			return connection;
		}
	}
}
