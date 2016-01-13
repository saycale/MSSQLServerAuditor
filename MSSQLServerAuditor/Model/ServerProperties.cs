using System;
using System.Data;
using System.Data.Common;
using MSSQLServerAuditor.Model.Connections.Factories;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.Model
{
	public class ServerProperties
	{
		public ServerProperties(
			InstanceVersion version,
			string          name,
			DateTime        date
		)
		{
			this.Version = version;
			this.Name    = name;
			this.Date    = date;
		}

		public InstanceVersion Version { get; private set; }
		public string          Name    { get; private set; }
		public DateTime        Date    { get; private set; }

		public static ServerProperties Query(
			InstanceInfo instance,
			int          timeout = 0
		)
		{
			switch (instance.Type)
			{
				case QuerySource.MSSQL:
					return QueryMssql(instance, timeout);
				case QuerySource.TDSQL:
					return QueryTeradata(instance, timeout);
				default:
					return GetBlackProperties(instance);
			}
		}

		public static ServerProperties Load(
			InstanceInfo        instance,
			CurrentStorage      storage
		)
		{
			ServerProperties storedProps = storage.ServerInstanceDirectory
				.GetServerProperties(instance);

			return storedProps;
		}

		private static ServerProperties GetBlackProperties(InstanceInfo instance)
		{
			return new ServerProperties(new InstanceVersion(), instance.Name, DateTime.Now);
		}

		private static ServerProperties QueryMssql(
			InstanceInfo instance,
			int          timeout = 0
		)
		{
			string   sqlString  = string.Empty;
			string   serverName = string.Empty;
			DateTime serverDate = DateTime.MaxValue;

			using (DbConnection dbConnection = CreateMssqlConnection(instance))
			{
				dbConnection.Open();

				InstanceVersion version = new InstanceVersion(dbConnection.ServerVersion);

				bool isNewServer = version.Major >= 8;  // MS SQL Server 2000

				if (isNewServer)
				{
					sqlString = "SELECT SERVERPROPERTY(N'ServerName') AS [ServerName], " +
					            "SERVERPROPERTY(N'InstanceName') AS [InstanceName], " +
					            "GetDate() AS [InstanceDate];";
				}
				else
				{
					sqlString = "SELECT GetDate() AS [InstanceDate];";
				}

				using (DbCommand command = dbConnection.CreateCommand())
				{
					command.CommandText = sqlString;

					if (timeout != 0)
					{
						command.CommandTimeout = timeout;
					}

					using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
					{
						if (reader.Read())
						{
							serverName = isNewServer
								? reader["ServerName"].ToString()
								: dbConnection.DataSource;

							serverDate = DateTime.Parse(reader["InstanceDate"].ToString());
						}
					}
				}

				return new ServerProperties(version, serverName, serverDate);
			}
		}

		private static ServerProperties QueryTeradata(
			InstanceInfo instance,
			int          timeout = 0
		)
		{
			using (DbConnection dbConnection = CreateTeradataConnection(instance))
			{
				dbConnection.Open();

				InstanceVersion version = new InstanceVersion(dbConnection.ServerVersion);
				DateTime  serverDate    = DateTime.MaxValue;

				using (DbCommand command = dbConnection.CreateCommand())
				{
					command.CommandText = "SELECT CURRENT_DATE AS InstanceDate";

					if (timeout != 0)
					{
						command.CommandTimeout = timeout;
					}

					using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
					{
						if (reader.Read())
						{
							serverDate = DateTime.Parse(reader["InstanceDate"].ToString());
						}
					}
				}

				return new ServerProperties(
					version,
					dbConnection.DataSource,
					serverDate);
			}
		}

		private static DbConnection CreateMssqlConnection(InstanceInfo instance)
		{
			if (instance.IsODBC)
			{
				return ConnectionFactory.CreateOdbcConnection(instance);
			}

			return ConnectionFactory.CreateSqlConnection(instance);
		}

		private static DbConnection CreateTeradataConnection(InstanceInfo instance)
		{
			if (instance.IsODBC)
			{
				return ConnectionFactory.CreateOdbcConnection(instance);
			}

			return ConnectionFactory.CreateTdConnection(instance);
		}
	}
}
