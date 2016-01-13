using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Settings;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	/// Settings form
	/// </summary>
	internal partial class frmSettings : LocalizableForm
	{
		private SettingsInfo               _settingsInfo;
		private readonly MsSqlAuditorModel _model;

		private frmSettings()
		{
			this._settingsInfo = null;
			this._model        = null;

			InitializeComponent();
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="model">Model</param>
		public frmSettings(MsSqlAuditorModel model) : this()
		{
			this._model = model;

			cmbInterfaceLang.Items.AddRange(_model.Settings.SystemSettings.AvailableUiLanguages.ToArray());
			cmbReportLang.Items.AddRange(_model.Settings.SystemSettings.AvailableReportLanguages.ToArray());
		}

		/// <summary>
		/// Changing settings
		/// </summary>
		public SettingsInfo SettingsInfo
		{
			get { return this._settingsInfo; }

			set
			{
				this._settingsInfo = value;

				settingsInfoBindingSource.DataSource = value;
				systemSettingsInfoBindingSource.DataSource = value.SystemSettings;

				cbShowSingleConnectionTab.Enabled = cbCreateTabsForConnection.Checked;
			}
		}

		private void cbConnectionsInTabs_CheckedChanged(object sender, System.EventArgs e)
		{
			cbShowSingleConnectionTab.Enabled = cbCreateTabsForConnection.Checked;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
		}
	}
}
