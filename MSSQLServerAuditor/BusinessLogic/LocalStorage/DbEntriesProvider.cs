using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Connections.Factories;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.Utils.Cryptography;

namespace MSSQLServerAuditor.BusinessLogic.LocalStorage
{
	public class DbEntriesProvider
	{
		private readonly string         _databasePath;
		private readonly ICryptoService _cryptoService;

		public DbEntriesProvider(Database database, ICryptoService cryptoService)
		{
			this._databasePath  = database.FileName;
			this._cryptoService = cryptoService;
		}

		public List<ConnectionGroupInfo> CloneStoredConnections()
		{
			return GetStoredConnections();
		}

		private List<ConnectionGroupInfo> GetStoredConnections()
		{
			string sql = String.Format(
				@"SELECT DISTINCT
					 {1}.{0}
					,{1}.{12}
					,{1}.{13}
					,{2}.{14}
					,{2}.{15}
					,{2}.{16}
				FROM
					{1}
					INNER JOIN {3} ON
						{1}.{0} = {3}.{7}
					INNER JOIN {4} ON
						{3}.{17} = {4}.{8}
					INNER JOIN {5} ON
						{4}.{9} = {5}.{20}
					INNER JOIN {6} ON
						{6}.{18} = {5}.{10}
					INNER JOIN {2} ON
						{2}.{19} = {6}.{11}",
				ConnectionGroupDirectory.TableIdentityField,
				ConnectionGroupDirectory.TableName,
				TemplateDirectory.TableName,
				ServerInstanceDirectory.TableName,
				QueryDirectory.TableName,
				TemplateNodeQueryDirectory.TableName,
				TemplateNodeDirectory.TableName,
				ConnectionGroupDirectory.TableName.AsFk(),
				ServerInstanceDirectory.TableName.AsFk(),
				TemplateNodeQueryDirectory.TableName.AsFk(),
				TemplateNodeDirectory.TableName.AsFk(),
				TemplateDirectory.TableName.AsFk(),
				ConnectionGroupDirectory.NameFn,
				ConnectionGroupDirectory.IsDirectConnectionFn,
				TemplateDirectory.NameFieldName,
				TemplateDirectory.IdFieldName,
				TemplateDirectory.DirFieldName,

				ServerInstanceDirectory.TableIdentityField,
				TemplateNodeDirectory.TableIdentityField,
				TemplateDirectory.TableIdentityField,
				TemplateNodeQueryDirectory.TableIdentityField
			);

			List<ConnectionGroupInfo> result;

			using (SQLiteConnection connection = ConnectionFactory.CreateSQLiteConnection(this._databasePath, true))
			{
				connection.Open();

				result = ExecuteSelectGroupsCommand(connection, sql);
			}

			foreach (ConnectionGroupInfo group in result)
			{
				group.Connections = group.Identity.HasValue
					? GetInstanceInfos(group.Identity.Value)
					: GetAllInstanceInfos();

				group.Connections.ForEach(
					instanceInfo => instanceInfo.ConnectionGroup = group
				);
			}

			return result.ToList();
		}

		private List<InstanceInfo> GetAllInstanceInfos()
		{
			string sql = string.Format(
				@"SELECT DISTINCT
					 si.[{0}]
					,si.[{1}]
					,si.[{2}]
					,si.[{3}]
					,si.[{4}]
					,l.[{5}]
					,l.[{6}]
					,l.[{7}]
				FROM
					[{8}] q
					INNER JOIN [{9}] si ON
						q.[{10}] = si.[{14}]
					INNER JOIN [{11}] l ON
						si.[{12}] = l.[{15}]
				WHERE
					si.[{13}] = 0",
				ServerInstanceDirectory.ConnectionNameFn,
				ServerInstanceDirectory.ServerInstanceNameFn,
				ServerInstanceDirectory.DbTypeFn,
				ServerInstanceDirectory.IsOdbcFn,
				ServerInstanceDirectory.ServerInstanceVersionFn,
				LoginDirectory.LoginFn,
				LoginDirectory.PasswordFn,
				LoginDirectory.IsWinAuthFn,
				QueryDirectory.TableName,
				ServerInstanceDirectory.TableName,
				ServerInstanceDirectory.TableName.AsFk(),
				LoginDirectory.TableName,
				LoginDirectory.TableName.AsFk(),
				ServerInstanceDirectory.IsDynamicConnectionFn,
				ServerInstanceDirectory.TableIdentityField,
				LoginDirectory.TableIdentityField
			);

			using (SQLiteConnection connection = ConnectionFactory.CreateSQLiteConnection(this._databasePath, true))
			{
				connection.Open();

				return ExecuteSelectInstancesCommand(connection, sql);
			}
		}

		private List<InstanceInfo> GetInstanceInfos(long connectionGroupId)
		{
			string sql = string.Format(
				@"SELECT DISTINCT
					si.[{0}],
					si.[{1}],
					si.[{2}],
					si.[{3}],
					si.[{4}],
					l.[{5}],
					l.[{6}],
					l.[{7}]
				FROM
					[{8}] q
					INNER JOIN [{9}] si ON
						q.[{10}] = si.[{16}]
					INNER JOIN [{11}] l ON
						si.[{12}] = l.[{17}]
				WHERE
					si.[{13}] = 0
					AND si.[{14}] = {15}",
				ServerInstanceDirectory.ConnectionNameFn,
				ServerInstanceDirectory.ServerInstanceNameFn,
				ServerInstanceDirectory.DbTypeFn,
				ServerInstanceDirectory.IsOdbcFn,
				ServerInstanceDirectory.ServerInstanceVersionFn,
				LoginDirectory.LoginFn,
				LoginDirectory.PasswordFn,
				LoginDirectory.IsWinAuthFn,
				QueryDirectory.TableName,
				ServerInstanceDirectory.TableName,
				ServerInstanceDirectory.TableName.AsFk(),
				LoginDirectory.TableName,
				LoginDirectory.TableName.AsFk(),
				ServerInstanceDirectory.IsDynamicConnectionFn,
				ServerInstanceDirectory.ConnectionGroupIdFn,
				connectionGroupId,
				ServerInstanceDirectory.TableIdentityField,
				LoginDirectory.TableIdentityField
			);

			using (SQLiteConnection connection = ConnectionFactory.CreateSQLiteConnection(this._databasePath, true))
			{
				connection.Open();

				return ExecuteSelectInstancesCommand(connection, sql);
			}
		}

		private static List<ConnectionGroupInfo> ExecuteSelectGroupsCommand(SQLiteConnection connection, string sql)
		{
			List<ConnectionGroupInfo> result = new List<ConnectionGroupInfo>();

			new SqlSelectCommand(
				connection,
				sql,
				reader =>
				{
					long id = (long)reader[ConnectionGroupDirectory.TableIdentityField];

					ConnectionGroupInfo connectionGroup = new ConnectionGroupInfo
					{
						Identity           = id,
						Name               = reader[ConnectionGroupDirectory.NameFn].ToString(),
						IsDirectConnection = (bool)reader[ConnectionGroupDirectory.IsDirectConnectionFn],
						TemplateFileName   = reader[TemplateDirectory.IdFieldName].ToString(),
						TemplateDir        = reader[TemplateDirectory.DirFieldName].ToString(),
						TemplateId         = reader[TemplateDirectory.NameFieldName].ToString(),
					};

					connectionGroup.Identity = id;

					result.Add(connectionGroup);
				}
			).Execute(100);

			return result;
		}

		private List<InstanceInfo> ExecuteSelectInstancesCommand(SQLiteConnection connection, string sql)
		{
			List<InstanceInfo> result = new List<InstanceInfo>();

			new SqlSelectCommand(
				connection,
				sql,
				reader =>
				{
					InstanceVersion version = new InstanceVersion(reader[ServerInstanceDirectory.ServerInstanceVersionFn].ToString());

					ServerProperties props  = new ServerProperties(
						version,
						reader[ServerInstanceDirectory.ServerInstanceNameFn].ToString(),
						DateTime.Now
					);

					AuthenticationInfo auth = new AuthenticationInfo
					{
						IsWindows = (bool) reader[LoginDirectory.IsWinAuthFn],
						Username  = reader[LoginDirectory.LoginFn].ToString(),
						Password  = this._cryptoService.Decrypt(reader[LoginDirectory.PasswordFn].ToString())
					};

					result.Add(
						new InstanceInfo(props)
						{
							Authentication = auth,
							Instance       = reader[ServerInstanceDirectory.ConnectionNameFn].ToString(),
							IsEnabled      = true,
							Name           = reader[ServerInstanceDirectory.ConnectionNameFn].ToString(),
							DbType         = reader[ServerInstanceDirectory.DbTypeFn].ToString(),
							IsODBC         = (bool) reader[ServerInstanceDirectory.IsOdbcFn]
						}
					);
				}
			).Execute(100);

			return result;
		}
	}
}
