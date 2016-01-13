using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	/// <summary>
	/// SQLite directory for instance
	/// </summary>
	public class ServerInstanceDirectory : TableDirectory
	{
		internal const string TableName               = "d_ServerInstance";
		internal const string TableIdentityField      = "d_ServerInstance_id";
		internal const string ConnectionGroupIdFn     = "d_ConnectionGroup_id";
		internal const string LoginIdFn               = "d_Login_id";
		internal const string ConnectionNameFn        = "ConnectionName";
		internal const string ServerInstanceNameFn    = "ServerInstanceName";
		internal const string ServerInstanceVersionFn = "ServerInstaceVersion";
		internal const string IsDynamicConnectionFn   = "IsDynamicConnection";
		internal const string IsDeletedFn             = "IsDeleted";
		internal const string IsOdbcFn                = "IsODBC";
		internal const string DbTypeFn                = "DbType";

		public ServerInstanceDirectory(
			CurrentStorage   storage
		) : base (
				storage,
				CreateTableDefinition()
			)
		{
		}

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddBigIntField(ConnectionGroupIdFn,        true,  false)
				.AddBigIntField(LoginIdFn,                  true,  false)
				.AddNVarCharField(ConnectionNameFn,         true,  false)
				.AddNVarCharField(ServerInstanceNameFn,     false, false)
				.AddNVarCharField(ServerInstanceVersionFn,  false, false)
				.AddNVarCharField(DbTypeFn,                 true,  false)
				.AddBitField(IsOdbcFn,                      false, false)
				.AddBitField(IsDynamicConnectionFn,         true,  false)
				.AddField(IsDeletedFn, SqlDbType.Bit,       false, true, 0, null)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		/// <summary>
		/// Get Id for data
		/// </summary>
		/// <param name="connectionGroup">Connection group</param>
		/// <param name="instance">Instance</param>
		/// <returns></returns>
		public Int64? GetId(ConnectionGroupInfo connectionGroup, InstanceInfo instance)
		{
			connectionGroup.ReadGroupIdFrom(Storage.ConnectionGroupDirectory);

			Int64?           loginId    = Storage.LoginDirectory.GetId(instance);
			ServerProperties props      = instance.ServerProperties;
			string           serverName = null;
			string           serverVers = null;

			if (props != null)
			{
				serverName = props.Name;
				serverVers = props.Version.ToString();
			}

			return this.GetRecordIdByFields(
				this.CreateField(ConnectionGroupIdFn,     connectionGroup.Identity),
				this.CreateField(LoginIdFn,               loginId),
				this.CreateField(ConnectionNameFn,        instance.Name),
				this.CreateField(ServerInstanceNameFn,    serverName),
				this.CreateField(ServerInstanceVersionFn, serverVers),
				this.CreateField(DbTypeFn,                instance.DbType),
				this.CreateField(IsOdbcFn,                instance.IsODBC),
				this.CreateField(IsDynamicConnectionFn,   instance.IsDynamicConnection)
			);
		}

		public ServerProperties GetServerProperties(
			InstanceInfo instance
		)
		{
			ConnectionGroupInfo connectionGroup = instance.ConnectionGroup;

			connectionGroup.ReadGroupIdFrom(Storage.ConnectionGroupDirectory);

			Int64? loginId = Storage.LoginDirectory.GetId(instance);

			string clause = string.Join(" AND ",
				new List<string>
				{
					ConnectionGroupIdFn.AsSqlClausePair(),
					LoginIdFn.AsSqlClausePair(),
					ConnectionNameFn.AsSqlClausePair(),
					DbTypeFn.AsSqlClausePair(),
					IsDynamicConnectionFn.AsSqlClausePair()
				}
			);

			List<ITableRow> rows = this.GetRows(
				clause,
				new List<SQLiteParameter>
				{
					SQLiteHelper.GetParameter(ConnectionGroupIdFn,   connectionGroup.Identity),
					SQLiteHelper.GetParameter(LoginIdFn,             loginId),
					SQLiteHelper.GetParameter(ConnectionNameFn,      instance.Name),
					SQLiteHelper.GetParameter(DbTypeFn,              instance.DbType),
					SQLiteHelper.GetParameter(IsDynamicConnectionFn, instance.IsDynamicConnection)
				},
				1
			);

			if (rows != null)
			{
				if (rows.Count != 0)
				{
					ITableRow row    = rows.ElementAt(0);
					string   version = row.GetValue<string>(ServerInstanceVersionFn);
					string   name    = row.GetValue<string>(ServerInstanceNameFn);

					if (!string.IsNullOrEmpty(version))
					{
						return new ServerProperties(
							new InstanceVersion(version),
							name,
							DateTime.Now
						);
					}
				}
			}

			return null;
		}
	}
}
