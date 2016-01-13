using Microsoft.Data.ConnectionUI;
using log4net;

namespace MSSQLServerAuditor.Model.Connections
{
	public static class SelectConnection
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static InstanceInfo Create(string connection, QuerySource sourceType, bool isOdbcConnection)
		{
			if (IsDatabaseSource(sourceType))
			{
				return CreateDatabaseSelectConnection(connection, sourceType, isOdbcConnection);
			}

			return CreateNonDatabaseSelectConnection(connection, sourceType);
		}

		private static InstanceInfo CreateNonDatabaseSelectConnection(string connection, QuerySource sourceType)
		{
			var instanceInfo = new InstanceInfo(true)
			{
				Instance       = connection,
				IsEnabled      = true,
				IsODBC         = false,
				Name           = connection,
				Authentication = new AuthenticationInfo
				{
					IsWindows = false,
					Password  = string.Empty,
					Username  = string.Empty
				},
				DbType         = sourceType.ToString()
			};

			return instanceInfo;
		}

		private static bool IsDatabaseSource(QuerySource sourceType)
		{
			switch (sourceType)
			{
				case QuerySource.MSSQL:
				case QuerySource.SQLite:
				case QuerySource.TDSQL:
					return true;
			}

			return false;
		}

		private static InstanceInfo CreateDatabaseSelectConnection(string connection, QuerySource sourceType, bool isOdbcConnection)
		{
			bool   AuthenticationInfoIsWindows = false;
			string AuthenticationInfoUsername  = string.Empty;
			string AuthenticationInfoPassword  = string.Empty;

			var connectionDialog = new DataConnectionDialog();

			connectionDialog.DataSources.Add(DataSource.OdbcDataSource);
			connectionDialog.DataSources.Add(DataSource.SqlDataSource);
			connectionDialog.SelectedDataSource = isOdbcConnection ? DataSource.OdbcDataSource : DataSource.SqlDataSource;
			connectionDialog.UnspecifiedDataSource.Providers.Add(DataProvider.SqlDataProvider);
			connectionDialog.ConnectionString = connection;

			var properties = connectionDialog.ConnectionProperties;

			if (sourceType == QuerySource.MSSQL)
			{
				AuthenticationInfoIsWindows = true;
				AuthenticationInfoUsername  = string.Empty;
				AuthenticationInfoPassword  = string.Empty;
			}
			else if (sourceType == QuerySource.TDSQL)
			{
				AuthenticationInfoIsWindows = true;
				AuthenticationInfoUsername  = string.Empty;
				AuthenticationInfoPassword  = string.Empty;
			}
			else if (sourceType == QuerySource.SQLite)
			{
				AuthenticationInfoIsWindows = true;
				AuthenticationInfoUsername  = string.Empty;
				AuthenticationInfoPassword  = string.Empty;
			}
			else if (sourceType == QuerySource.SQLiteExternal)
			{
				AuthenticationInfoIsWindows = true;
				AuthenticationInfoUsername  = string.Empty;
				AuthenticationInfoPassword  = string.Empty;
			}
			else if (sourceType == QuerySource.ActiveDirectory)
			{
				AuthenticationInfoIsWindows = true;
				AuthenticationInfoUsername  = string.Empty;
				AuthenticationInfoPassword  = string.Empty;
			}
			else if (sourceType == QuerySource.EventLog)
			{
				AuthenticationInfoIsWindows = true;
				AuthenticationInfoUsername  = string.Empty;
				AuthenticationInfoPassword  = string.Empty;
			}
			else if (sourceType == QuerySource.NetworkInformation)
			{
				AuthenticationInfoIsWindows = true;
				AuthenticationInfoUsername  = string.Empty;
				AuthenticationInfoPassword  = string.Empty;
			}
			else
			{
				AuthenticationInfoIsWindows = true;
				AuthenticationInfoUsername  = string.Empty;
				AuthenticationInfoPassword  = string.Empty;
			}

			if (properties["Integrated Security"] != null)
			{
				if (properties["Integrated Security"] as bool)
				{
					AuthenticationInfoIsWindows = true;
				}
				else
				{
					AuthenticationInfoIsWindows = false;
				}
			}

			if (properties["uid"] != null)
			{
				if (!string.IsNullOrEmpty(properties["uid"] as string))
				{
					AuthenticationInfoIsWindows = false;
					AuthenticationInfoUsername  = properties["uid"] as string;
				}
			}

			if (properties["User ID"] != null)
			{
				if (!string.IsNullOrEmpty(properties["User ID"] as string))
				{
					AuthenticationInfoIsWindows = false;
					AuthenticationInfoUsername  = properties["User ID"] as string;
				}
			}

			if (properties["pwd"] != null)
			{
				if (!string.IsNullOrEmpty(properties["pwd"] as string))
				{
					AuthenticationInfoIsWindows = false;
					AuthenticationInfoPassword  = properties["pwd"] as string;
				}
			}

			if (properties["Password"] != null)
			{
				if (!string.IsNullOrEmpty(properties["Password"] as string))
				{
					AuthenticationInfoIsWindows = false;
					AuthenticationInfoPassword  = properties["Password"] as string;
				}
			}

			log.InforFormat("IsWindows:'{0}';Username:'{1}';Password:'{2}',
				AuthenticationInfoIsWindows
				AuthenticationInfoUsername,
				AuthenticationInfoPassword,
			);

			var instance = new InstanceInfo(true)
			{
				Authentication = new AuthenticationInfo
					{
						// IsWindows = properties["Integrated Security"] as bool?   ?? false,
						// Username = (properties["uid"]                 as string) ?? (properties["User ID"]  as string) ?? string.Empty,
						// Password = (properties["pwd"]                 as string) ?? (properties["Password"] as string) ?? string.Empty
						IsWindows = AuthenticationInfoIsWindows,
						Username  = AuthenticationInfoUsername,
						Password  = AuthenticationInfoPassword
					},
				Instance  = (properties["Dsn"] as string) ?? (properties["Data Source"] as string),
				IsEnabled = true,
				Name      = (properties["Dsn"] as string) ?? (properties["Data Source"] as string),
				IsODBC    = isOdbcConnection,
				DbType    = sourceType.ToString()
			};

			if (instance.IsODBC)
			{
				instance.SetInnerODBCCOnnectionString(connection);
			}

			return instance;
		}
	}
}
