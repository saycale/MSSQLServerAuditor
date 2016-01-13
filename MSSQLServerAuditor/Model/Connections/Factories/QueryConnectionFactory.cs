using System;
using System.Data.Odbc;
using System.Data.SQLite;
using System.Reflection;
using System.Data.SqlClient;
using Teradata.Client.Provider;
using log4net;
using MSSQLServerAuditor.BusinessLogic.LocalStorage;
using MSSQLServerAuditor.Model.Connections.Query;

namespace MSSQLServerAuditor.Model.Connections.Factories
{
	public class QueryConnectionFactory : IQueryConnectionFactory
	{
		private static readonly ILog       Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly MsSqlAuditorModel _model;

		public QueryConnectionFactory(MsSqlAuditorModel model)
		{
			this._model = model;
		}

		public IQueryConnection CreateQueryConnection(QuerySource sourceType, InstanceInfo instance)
		{
			bool        isOdbc    = instance.IsODBC;
			QuerySource queryType = instance.Type;

			instance.SetSettings(this._model.Settings);

			if (sourceType == QuerySource.SQLite)
			{
				IStorageManager storageManager = this._model.GetVaultProcessor(
					instance.ConnectionGroup ?? new ConnectionGroupInfo()
				);

				AttachingSqliteQueryConnection sqliteQueryConnection;
				string                         connectionString = instance.GetConnectionString();
				SqliteConnectionParameters     connectionParameters = SqliteConnectionParameters.Parse(connectionString);

				if (connectionParameters.IsValid)
				{
					sqliteQueryConnection = new SqliteInnerQueryConnection(
						storageManager.CurrentStorage,
						connectionParameters
					);
				}
				else
				{
					sqliteQueryConnection = new SqliteInternalQueryConnection(
						storageManager.CurrentStorage,
						instance
					);
				}

				if (storageManager.HistoryStorage != null)
				{
					foreach (var historyStorage in storageManager.HistoryStorage)
					{
						sqliteQueryConnection.AddDatabaseToAttach(
							historyStorage.Alias,
							historyStorage.FileName
						);
					}
				}

				if (storageManager.ReportStorage != null)
				{
					sqliteQueryConnection.AddDatabaseToAttach(
						"report",
						storageManager.ReportStorage.FileName
					);
				}

				return sqliteQueryConnection;
			}

			if (isOdbc)
			{
				OdbcConnection odbcConnection = ConnectionFactory.CreateOdbcConnection(instance, true);

				return new OdbcQueryConnection(odbcConnection);
			}

			if (sourceType == queryType)
			{
				switch (sourceType)
				{
					case QuerySource.MSSQL:
						SqlConnection sqlConnection =
							ConnectionFactory.CreateSqlConnection(instance, true);

						return new MsSqlQueryConnection(sqlConnection);

					case QuerySource.TDSQL:
						TdConnection tdConnection =
							ConnectionFactory.CreateTdConnection(instance, true);

						return new TeradataSqlQueryConnection(tdConnection);

					case QuerySource.SQLiteExternal:
						SQLiteConnection sqliteConnection =
							ConnectionFactory.CreateSQLiteExternalConnection(instance);

						return new SqliteExternalQueryConnection(sqliteConnection);

					case QuerySource.ActiveDirectory:
						ActiveDirectoryConnection activeDirectoryConnection =
							ConnectionFactory.CreateActiveDirectoryConnection(instance);

						return new ActiveDirectoryQueryConnection(activeDirectoryConnection);

					case QuerySource.EventLog:
						EventLogConnection eventLogConnection =
							ConnectionFactory.CreateEventLogConnection(instance);

						return new EventLogQueryConnection(eventLogConnection);

					case QuerySource.NetworkInformation:
						NetworkInformationConnection networkInfoConnection =
							ConnectionFactory.CreateNetworkInformationConnection(instance);

						return new NetworkInformationQueryConnection(networkInfoConnection);
				}
			}

			string errorMessage = String.Format(
				"There is no QueryConnection defined. QuerySource: {0}, QueryType: {1}",
				sourceType,
				queryType
			);

			Log.ErrorFormat(errorMessage);

			throw new ArgumentException(errorMessage);
		}

		public void Dispose()
		{
		}
	}
}
