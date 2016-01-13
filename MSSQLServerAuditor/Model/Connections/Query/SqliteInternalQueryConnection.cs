using System.Data.SQLite;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.Model.Connections.Factories;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	internal class SqliteInternalQueryConnection : AttachingSqliteQueryConnection
	{
		private static readonly ILog    Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly CurrentStorage _currentScope;
		private readonly InstanceInfo   _instance;

		public SqliteInternalQueryConnection(
			CurrentStorage currentStorage,
			InstanceInfo   instance
		) : base(ConnectionFactory.CreateSQLiteConnection(currentStorage.FileName, false))
		{
			this._instance        = instance;
			this._currentScope    = currentStorage;
		}

		public override SqliteQueryCommand GetSqliteCommand(
			string                          sqlText,
			int                             commandTimeout
		)
		{
			SQLiteCommand sqliteCommand  = Connection.CreateCommand();
			sqliteCommand.CommandText    = sqlText;
			sqliteCommand.CommandTimeout = commandTimeout;

			return new SqliteInternalQueryCommand(
				sqliteCommand,
				this._currentScope,
				this._instance
			);
		}

		private class SqliteInternalQueryCommand : SqliteQueryCommand
		{
			private readonly InstanceInfo    _instanceInfo;
			private readonly CurrentStorage  _currentStorage;

			public SqliteInternalQueryCommand(
				SQLiteCommand    command,
				CurrentStorage   currentScope,
				InstanceInfo     instanceInfo
			) : base(command)
			{
				this._instanceInfo   = instanceInfo;
				this._currentStorage = currentScope;
			}

			protected override void AssignDefaultParameters()
			{
				InstanceInfo        instance        = this._instanceInfo;
				ConnectionGroupInfo connectionGroup = instance.ConnectionGroup;

				connectionGroup.ReadGroupIdFrom(this._currentStorage.ConnectionGroupDirectory);

				long? instanceId = this._currentStorage.ServerInstanceDirectory.GetId(connectionGroup, instance);
				long? loginId    = this._currentStorage.LoginDirectory.GetId(instance);
				long? templateId = this._currentStorage.TemplateDirectory.GetId(connectionGroup);

				this._sqliteCommand.Parameters.Add(
					SQLiteHelper.GetParameter(ConnectionGroupDirectory.TableName.AsFk(), connectionGroup.Identity)
				);

				this._sqliteCommand.Parameters.Add(
					SQLiteHelper.GetParameter(ServerInstanceDirectory.TableName.AsFk(), instanceId)
				);

				this._sqliteCommand.Parameters.Add(
					SQLiteHelper.GetParameter(LoginDirectory.TableName.AsFk(), loginId)
				);

				this._sqliteCommand.Parameters.Add(
					SQLiteHelper.GetParameter(TemplateDirectory.TableName.AsFk(), templateId)
				);
			}
		}
	}
}
