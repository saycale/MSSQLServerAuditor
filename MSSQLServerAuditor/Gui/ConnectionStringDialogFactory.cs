using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.Gui
{
	public static class ConnectionStringDialogFactory
	{
		public static IConnectionStringDialog CreateDialog(QuerySource querySource, MsSqlAuditorModel model)
		{
			switch (querySource)
			{
				case QuerySource.MSSQL:
					return new CommonConnectionStringDialog();
				case QuerySource.SQLite:
					return new InternalSqliteConnectionDialog(model);
				case QuerySource.NetworkInformation:
					return new NetworkInformationConnectionDialog();
				default:
					return new NonSqlDataConnectionDialog();
			}
		}
	}
}
