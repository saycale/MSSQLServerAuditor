using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;

namespace MSSQLServerAuditor.Gui
{
	public class CommonConnectionStringDialog : IConnectionStringDialog
	{
		private readonly DataConnectionDialog _connectionDialog;

		public CommonConnectionStringDialog()
		{
			this._connectionDialog = new DataConnectionDialog();

			this._connectionDialog.DataSources.Add(DataSource.OdbcDataSource);
			this._connectionDialog.DataSources.Add(DataSource.SqlDataSource);
			this._connectionDialog.SelectedDataSource = DataSource.SqlDataSource;

			DataConnectionConfiguration dcs = new DataConnectionConfiguration(null);

			dcs.LoadConfiguration(_connectionDialog);
		}

		public string ConnectionString
		{
			get
			{
				return this._connectionDialog.ConnectionString;
			}
		}

		public string ConnectionName
		{
			get
			{
				return (this._connectionDialog.ConnectionProperties["Dsn"] as string) ??
					   (this._connectionDialog.ConnectionProperties["Data Source"] as string);
			}
		}

		public bool IsOdbc
		{
			get
			{
				return (this._connectionDialog.SelectedDataSource == DataSource.OdbcDataSource);
			}
		}

		public DialogResult ShowDialog()
		{
			return DataConnectionDialog.Show(this._connectionDialog);
		}
	}
}
