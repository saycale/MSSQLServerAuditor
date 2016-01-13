using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.BusinessLogic
{
	public class ServerInstanceManager
	{
		private readonly CurrentStorage _storage;

		public ServerInstanceManager(CurrentStorage storage)
		{
			this._storage = storage;
		}

		public List<ServerInstanceRow> GetInstancesOfType(string protocolType)
		{
			string clause = string.Join(" AND ", Lists.Of(
				ServerInstanceDirectory.DbTypeFn.AsSqlClausePair(),
				ServerInstanceDirectory.IsDynamicConnectionFn.AsSqlClausePair()
			));

			List<SQLiteParameter> parameters = Lists.Of(
				new SQLiteParameter(ServerInstanceDirectory.DbTypeFn, protocolType),
				new SQLiteParameter(ServerInstanceDirectory.IsDynamicConnectionFn, false)
			);

			List<ServerInstanceRow> rows =
				this._storage.ServerInstanceDirectory.GetRows(
					clause,
					parameters
				).Select(ServerInstanceRow.Copy).ToList();

			return rows;
		}

		public List<ServerInstanceRow> GetAllGroupInstances(
			long   groupId,
			string protocolType
		)
		{
			string clause = string.Join(" AND ", Lists.Of(
				ServerInstanceDirectory.ConnectionGroupIdFn.AsSqlClausePair(),
				ServerInstanceDirectory.DbTypeFn.AsSqlClausePair()
			));

			List<SQLiteParameter> parameters = Lists.Of(
				new SQLiteParameter(ServerInstanceDirectory.DbTypeFn, protocolType),
				new SQLiteParameter(ServerInstanceDirectory.ConnectionGroupIdFn, groupId)
			);

			List<ServerInstanceRow> rows =
				this._storage.ServerInstanceDirectory.GetRows(
					clause,
					parameters
				).Select(ServerInstanceRow.Copy).ToList();

			return rows;
		}

		public List<ServerInstanceRow> GetGroupInstances(
			long   groupId,
			string protocolType = null,
			bool   deleted      = false
		)
		{
			List<string> clausePairs = Lists.Of(
				ServerInstanceDirectory.ConnectionGroupIdFn.AsSqlClausePair(),
				ServerInstanceDirectory.IsDeletedFn.AsSqlClausePair()
			);

			List<SQLiteParameter> parameters = Lists.Of(
				new SQLiteParameter(ServerInstanceDirectory.ConnectionGroupIdFn, groupId),
				new SQLiteParameter(ServerInstanceDirectory.IsDeletedFn, deleted)
			);

			if (!string.IsNullOrEmpty(protocolType))
			{
				clausePairs.Add(ServerInstanceDirectory.DbTypeFn.AsSqlClausePair());
				parameters.Add(new SQLiteParameter(ServerInstanceDirectory.DbTypeFn, protocolType));
			}

			string clause = string.Join(" AND ", clausePairs);

			List<ServerInstanceRow> rows =
				this._storage.ServerInstanceDirectory.GetRows(
					clause,
					parameters
				).Select(ServerInstanceRow.Copy).ToList();

			return rows;
		}

		public void DeleteInstance(ServerInstanceRow savedRow)
		{
			savedRow.IsDeleted = true;

			this._storage.ServerInstanceDirectory.UpdateRow(savedRow);
		}

		public void RestoreInstance(ServerInstanceRow savedRow)
		{
			savedRow.IsDeleted = false;

			this._storage.ServerInstanceDirectory.UpdateRow(savedRow);
		}
	}
}
