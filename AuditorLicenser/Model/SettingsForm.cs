using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MSSQLServerAuditor.Model.Internationalization;

namespace MSSQLServerAuditor.Licenser.Model
{
	public partial class SettingsForm : LocalizableForm
	{
		private readonly LocaleManager _lM;
		private Settings               _settings;

		public SettingsForm()
		{
			InitializeComponent();

			this._lM = new LocaleManager(() => (Settings ?? Settings.UserOnes).UiLanguage, Path.Combine(Application.StartupPath, "AuditorLicenser.i18n.xml"))
			{
				FormIfOmited = this
			};

			List<string> availableLocales = Settings.SystemOnes.AvailableUiLanguages;

			uiLangCombo.Items.Clear();
			uiLangCombo.Items.AddRange(availableLocales.Cast<object>().ToArray());

			this._lM.LocalizeForm(this);
			this._lM.LocalizeDeep(this, Controls);
		}

		public Settings Settings
		{
			get
			{
				return this._settings;
			}

			set
			{
				this._settings = value;
				settingsBindingSource.DataSource = value;
			}
		}
	}
}
