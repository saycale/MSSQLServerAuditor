using System;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Reflection;
using Teradata.Client.Provider;
using log4net;
using MSSQLServerAuditor.SQLite.Common;

namespace MSSQLServerAuditor.Model.Connections.Factories
{
	public static class ConnectionFactory
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static DbConnection CreateDbConnection(InstanceInfo instanceInfo, bool validateLicense = false)
		{
			if (instanceInfo.IsODBC)
			{
				return CreateOdbcConnection(instanceInfo, validateLicense);
			}

			switch (instanceInfo.Type)
			{
				case QuerySource.MSSQL:
					return CreateSqlConnection(instanceInfo, validateLicense);
				case QuerySource.TDSQL:
					return CreateTdConnection(instanceInfo, validateLicense);
				default:
					string errorMessage = string.Format(
						"Can not create DbConnection for Query type:{0}",
						instanceInfo.Type
					);

					Log.ErrorFormat(errorMessage);

					throw new ArgumentException(errorMessage);
			}
		}

		public static OdbcConnection CreateOdbcConnection(InstanceInfo instanceInfo, bool validateLicense = false)
		{
			if (validateLicense)
			{
				instanceInfo.ValidateLicense(true).ThrowIfNotCorrect();
			}

			return new OdbcConnection(instanceInfo.GetConnectionString());
		}

		public static SqlConnection CreateSqlConnection(InstanceInfo instanceInfo, bool validateLicense = false)
		{
			if (validateLicense)
			{
				instanceInfo.ValidateLicense(true).ThrowIfNotCorrect();
			}

			return new SqlConnection(instanceInfo.GetConnectionString());
		}

		public static TdConnection CreateTdConnection(InstanceInfo instanceInfo, bool validateLicense = false)
		{
			if (validateLicense)
			{
				instanceInfo.ValidateLicense(true).ThrowIfNotCorrect();
			}

			return new TdConnection(instanceInfo.GetConnectionString());
		}

		public static SQLiteConnection CreateSQLiteExternalConnection(InstanceInfo instanceInfo)
		{
			return new SQLiteConnection(instanceInfo.GetConnectionString());
		}

		public static ActiveDirectoryConnection CreateActiveDirectoryConnection(InstanceInfo instanceInfo)
		{
			string connectionPath = instanceInfo.GetConnectionString();

			return new ActiveDirectoryConnection(connectionPath);
		}

		public static EventLogConnection CreateEventLogConnection(InstanceInfo instanceInfo)
		{
			string strMachineName = instanceInfo.Instance;

			return new EventLogConnection(strMachineName);
		}

		public static NetworkInformationConnection CreateNetworkInformationConnection(InstanceInfo instanceInfo)
		{
			return NetworkInformationConnection.Parse(
				instanceInfo.GetConnectionString()
			);
		}

		public static SQLiteConnection CreateSQLiteConnection(Database database, bool readOnly = true)
		{
			return CreateSQLiteConnection(database.FileName, readOnly);
		}

		public static SQLiteConnection CreateSQLiteConnection(string fileName, bool readOnly = true)
		{
			string connectionString;

			if (fileName.Equals("file::memory:"))
			{
				connectionString =
					"FullUri=file:memdb1?mode=memory&cache=shared; PRAGMA journal_mode=off; PRAGMA temp_store = MEMORY; PRAGMA synchronous=off; PRAGMA count_changes=off; PRAGMA encoding = \"UTF-8\";";
			}
			else
			{
				connectionString = string.Format(
					"Data Source={0}; PRAGMA locking_mode = NORMAL; Pooling=True; PRAGMA page_size = 4096; PRAGMA cache_size=10000; PRAGMA journal_mode=WAL; PRAGMA synchronous=off; PRAGMA count_changes=off; PRAGMA temp_store=2; PRAGMA encoding = \"UTF-8\"; PRAGMA query_only={1};{2}",
					fileName,
					readOnly,
					readOnly ? "mode=ro" : string.Empty
				);
			}

			return new SQLiteConnection(connectionString);
		}
	}
}
