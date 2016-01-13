using System;
using System.Collections.Generic;
using System.Data.Common;
using log4net;

namespace MSSQLServerAuditor.Model
{
	public static class InstanceInfoResolver
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static InstanceInfo ResolveDynamicInstance(
			string      connection,
			QuerySource sourceType,
			bool        isOdbcConnection
		)
		{
			if (IsDatabaseSource(sourceType))
			{
				return ResolveDatabase(
					connection,
					sourceType,
					isOdbcConnection,
					true
				);
			}

			return ResolveNonDatabase(
				connection,
				sourceType,
				true
			);
		}

		public static List<InstanceInfo> ResolveInstances(
			List<Tuple<DbConnectionStringBuilder, bool>> connectionProperties,
			QuerySource                                  dbType
		)
		{
			List<InstanceInfo> instances = new List<InstanceInfo>();

			foreach (Tuple<DbConnectionStringBuilder, bool> properties in connectionProperties)
			{
				DbConnectionStringBuilder cnn = properties.Item1;
				bool isOdbc                   = properties.Item2;
				InstanceInfo instance         = ResolveInstance(cnn, isOdbc, dbType);

				instances.Add(instance);
			}

			return instances;
		}

		public static InstanceInfo ResolveInstance(
			DbConnectionStringBuilder builder,
			bool                      isOdbc,
			QuerySource               dbType
		)
		{
			InstanceInfo instance;

			if (dbType == QuerySource.SQLite || dbType == QuerySource.NetworkInformation)
			{
				instance = ResolveNonDatabase(
					builder.ConnectionString,
					dbType
				);
			}
			else
			{
				instance = ResolveDatabase(
					builder,
					dbType,
					isOdbc
				);
			}

			return instance;
		}

		private static InstanceInfo ResolveNonDatabase(
			string      connection,
			QuerySource sourceType,
			bool        isDynamicConnection = false
		)
		{
			InstanceInfo instanceInfo = new InstanceInfo(isDynamicConnection)
			{
				Instance  = connection,
				IsEnabled = true,
				IsODBC    = false,
				Name      = connection,

				Authentication = new AuthenticationInfo
				{
					IsWindows = false,
					Password  = string.Empty,
					Username  = string.Empty
				},

				DbType = sourceType.ToString()
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

		private static InstanceInfo ResolveDatabase(
			string      connection,
			QuerySource sourceType,
			bool        isOdbcConnection,
			bool        isDynamicConnection = false
		)
		{
			DbConnectionStringBuilder connectionBuilder = new DbConnectionStringBuilder
			{
				ConnectionString = connection
			};

			return ResolveDatabase(
				connectionBuilder,
				sourceType,
				isOdbcConnection,
				isDynamicConnection
			);
		}

		private static InstanceInfo ResolveDatabase(
			DbConnectionStringBuilder cnn,
			QuerySource               sourceType,
			bool                      isOdbcConnection,
			bool                      isDynamicConnection = false
		)
		{
			AuthenticationInfo authentication = GetAuthentication(cnn, sourceType);

			log.InfoFormat("IsWindows:'{0}';Username:'{1}';Password:'{2}'",
				authentication.IsWindows,
				authentication.Username,
				authentication.Password
			);

			string instanceName = ReadProp(cnn, "Dsn", "Data Source") as string;

			InstanceInfo instance = new InstanceInfo(isDynamicConnection)
			{
				Authentication = authentication,
				Instance       = instanceName,
				IsEnabled      = true,
				Name           = instanceName,
				IsODBC         = isOdbcConnection,
				DbType         = sourceType.ToString()
			};

			if (isOdbcConnection)
			{
				instance.InnerOdbcConnectionString = cnn.ConnectionString;
			}

			return instance;
		}

		private static AuthenticationInfo GetAuthentication(
			DbConnectionStringBuilder cnnBuilder,
			QuerySource               sourceType
		)
		{
			switch (sourceType)
			{
				case QuerySource.MSSQL:
				case QuerySource.TDSQL:
					return new AuthenticationInfo
					{
						IsWindows = ReadBoolProp(cnnBuilder, "Integrated Security"),
						Username  = ReadProp(cnnBuilder, "uid", "User ID")  as string ?? string.Empty,
						Password  = ReadProp(cnnBuilder, "pwd", "Password") as string ?? string.Empty
					};

				default:
					return new AuthenticationInfo
					{
						IsWindows = true,
						Username = string.Empty,
						Password = string.Empty
					};
			}
		}

		private static bool ReadBoolProp(
			DbConnectionStringBuilder cnn,
			params string[]           aliases
		)
		{
			object propObj = ReadProp(cnn, aliases);
			bool   propValue;

			if (propObj != null && bool.TryParse(propObj.ToString(), out propValue))
			{
				return propValue;
			}

			return false;
		}

		private static object ReadProp(
			DbConnectionStringBuilder cnn,
			params string[]           aliases
		)
		{
			if (aliases.Length == 0)
			{
				return null;
			}

			foreach (string alias in aliases)
			{
				object val;

				if (cnn.TryGetValue(alias, out val))
				{
					return val;
				}
			}

			return null;
		}
	}
}
