using System;
using System.Collections.Generic;
using System.Linq;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;

namespace MSSQLServerAuditor.BusinessLogic
{
	public class ConnectionsManager
	{
		private static class Predicates
		{
			public static Func<InstanceInfo, bool> MatchesTo(InstanceInfo instance)
			{
				Func<InstanceInfo, bool> matchingPredicate =
					x => x.GetConnectionString() == instance.GetConnectionString();

				return matchingPredicate;
			}
		}

		private readonly ServerInstanceManager  _instanceManager;
		private readonly ConnectionGroupManager _groupManager;
		private readonly LoginManager           _loginManager;

		public ConnectionsManager(MsSqlAuditorModel model)
		{
			CurrentStorage storage = model.DefaultVaultProcessor.CurrentStorage;

			this._groupManager    = new ConnectionGroupManager(storage);
			this._instanceManager = new ServerInstanceManager(storage);
			this._loginManager    = new LoginManager(storage);
		}

		public ConnectionGroupInfo GetGroup(long groupId)
		{
			ConnectionGroupRow groupRow = this._groupManager
				.GetGroupById(groupId);

			if (groupRow == null)
			{
				return null;
			}

			ConnectionGroupInfo group = new ConnectionGroupInfo
			{
				Identity           = groupId,
				Name               = groupRow.Name,
				IsDirectConnection = groupRow.IsDirect
			};

			group.Connections = GetGroupInstances(groupId);

			return group;
		}

		public List<ConnectionGroupInfo> GetAllGroups(string protocolType)
		{
			List<ConnectionGroupRow> groupRows = this._groupManager.GetAllGroups();

			return GetGroupsFromRows(protocolType, groupRows);
		}

		public List<ConnectionGroupInfo> GetDirectGroups(string protocolType)
		{
			List<ConnectionGroupRow> groupRows = this._groupManager.GetDirectGroups();

			return GetGroupsFromRows(protocolType, groupRows);
		}

		private List<ConnectionGroupInfo> GetGroupsFromRows(string protocolType, List<ConnectionGroupRow> groupRows)
		{
			List<ConnectionGroupInfo> groups = new List<ConnectionGroupInfo>();

			foreach (ConnectionGroupRow groupRow in groupRows)
			{
				long? groupId = groupRow.Identity;

				ConnectionGroupInfo group = new ConnectionGroupInfo
				{
					Identity           = groupId,
					Name               = groupRow.Name,
					IsDirectConnection = groupRow.IsDirect
				};

				group.Connections = GetGroupInstances(groupId.Value, protocolType);

				groups.Add(group);
			}

			return groups;
		}

		public void UpdateGroupInstances(ConnectionGroupInfo group, string protocolType)
		{
			ConnectionGroupRow savedGroupRow = this._groupManager.GetGroupByName(group.Name);

			if (savedGroupRow == null)
			{
				return;
			}

			long id = savedGroupRow.Identity;

			List<ServerInstanceRow> savedRows = this._instanceManager
				.GetAllGroupInstances(id, protocolType);

			List<InstanceInfo> newInstances = group.Connections;

			foreach (ServerInstanceRow savedRow in savedRows)
			{
				InstanceInfo savedInstance = CreateFromRow(savedRow);
				bool         exist         = newInstances.Any(Predicates.MatchesTo(savedInstance));

				if (savedRow.IsDeleted)
				{
					if (exist)
					{
						this._instanceManager.RestoreInstance(savedRow);
					}
				}
				else
				{
					if (!exist)
					{
						this._instanceManager.DeleteInstance(savedRow);
					}
				}
			}
		}

		public List<InstanceInfo> GetGroupInstances(long groupId, string protocolType)
		{
			List<ServerInstanceRow> instanceRows = this._instanceManager
				.GetGroupInstances(groupId, protocolType);

			return instanceRows.Select(CreateFromRow).ToList();
		}

		public List<InstanceInfo> GetGroupInstances(long groupId)
		{
			List<ServerInstanceRow> instanceRows = this._instanceManager
				.GetGroupInstances(groupId);

			return instanceRows.Select(CreateFromRow).ToList();
		}

		public List<InstanceInfo> GetGroupInstances(long groupId, long serverInstanceId)
		{
			List<ServerInstanceRow> instanceRows = new List<ServerInstanceRow>()
			{
				this._instanceManager.GetGroupInstances(groupId).FirstOrDefault(
					row => row.Identity == serverInstanceId
				)
			};

			return instanceRows.Select(CreateFromRow).ToList();
		}

		public List<InstanceInfo> GetDistinctInstances(string protocolType)
		{
			return GetInstances(protocolType)
				.GroupBy(i => i.GetConnectionString()) // group by connection strings
				.Select(r  => r.First())
				.ToList();
		}

		public List<InstanceInfo> GetInstances(string protocolType)
		{
			List<ServerInstanceRow> instanceRows = this._instanceManager
				.GetInstancesOfType(protocolType);

			List<InstanceInfo> instances = new List<InstanceInfo>();

			foreach (ServerInstanceRow instanceRow in instanceRows)
			{
				InstanceInfo instanceInfo = CreateFromRow(instanceRow);

				instances.Add(instanceInfo);
			}

			return instances;
		}

		private InstanceInfo CreateFromRow(ServerInstanceRow instanceRow)
		{
			long     loginId  = instanceRow.LoginId;
			LoginRow loginRow = this._loginManager.GetLogin(loginId);

			AuthenticationInfo authInfo = new AuthenticationInfo
			{
				Username  = loginRow.Login,
				Password  = loginRow.Password,
				IsWindows = loginRow.IsWinAuth
			};

			string conName = instanceRow.ConnectionName;

			InstanceInfo instanceInfo = new InstanceInfo(instanceRow.IsDynamicConnection)
			{
				Authentication = authInfo,
				IsODBC         = instanceRow.IsOdbc,
				DbType         = instanceRow.DbType,
				IsEnabled      = true,
				Name           = conName,
				Instance       = conName
			};

			instanceInfo.SetServerProperties(
				new ServerProperties(
					new InstanceVersion(instanceRow.ServerInstanceVersion),
					instanceRow.ServerInstanceName,
					DateTime.Now
				)
			);

			return instanceInfo;
		}
	}
}
