using MSSQLServerAuditor.Model.Internationalization;

namespace MSSQLServerAuditor.Gui
{
	public partial class NonSqlDataConnectionDialog : LocalizableForm, IConnectionStringDialog
	{
		public NonSqlDataConnectionDialog()
		{
			InitializeComponent();
		}

		public string ConnectionString
		{
			get { return string.Format("Data Source={0}", tbConnectionString.Text); }
		}

		public string ConnectionName
		{
			get { return this.ConnectionString; }
		}

		public bool IsOdbc
		{
			get { return false; }
		}
	}
}
