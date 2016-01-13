using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.BusinessLogic
{
	public class ConnectionGroupManager
	{
		private readonly CurrentStorage _storage;

		public ConnectionGroupManager(CurrentStorage storage)
		{
			this._storage = storage;
		}

		public List<ConnectionGroupRow> GetAllGroups()
		{
			List<ConnectionGroupRow> rows = this._storage.ConnectionGroupDirectory
				.GetRows(null).Select(RowConverter.Convert<ConnectionGroupRow>).ToList();

			return rows;
		}

		public List<ConnectionGroupRow> GetDirectGroups()
		{
			string clause = ConnectionGroupDirectory.IsDirectConnectionFn.AsSqlClausePair();

			List<ConnectionGroupRow> rows =
				this._storage.ConnectionGroupDirectory.GetRows(
					clause,
					Lists.Of(new SQLiteParameter(ConnectionGroupDirectory.IsDirectConnectionFn, true))
				).Select(RowConverter.Convert<ConnectionGroupRow>).ToList();

			return rows;
		}

		public ConnectionGroupRow GetGroupById(long id)
		{
			ITableRow row = this._storage.ConnectionGroupDirectory.GetRowByIdentity(id);

			if (row == null)
			{
				return null;
			}

			return RowConverter.Convert<ConnectionGroupRow>(row);
		}

		public ConnectionGroupRow GetGroupByName(string name)
		{
			string clause = string.Join(" AND ", Lists.Of(
				ConnectionGroupDirectory.NameFn.AsSqlClausePair(),
				ConnectionGroupDirectory.IsDirectConnectionFn.AsSqlClausePair()
			));

			List<SQLiteParameter> parameters = Lists.Of(
				new SQLiteParameter(ConnectionGroupDirectory.NameFn, name),
				new SQLiteParameter(ConnectionGroupDirectory.IsDirectConnectionFn, true)
			);

			ConnectionGroupRow row =
				this._storage.ConnectionGroupDirectory.GetRows(
					clause,
					parameters,
					1
				)
				.Select(RowConverter.Convert<ConnectionGroupRow>)
				.FirstOrDefault();

			return row;
		}
	}
}
