using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Loaders;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	///     The new direct connection dialog.
	/// </summary>
	public partial class OpenConnectionDialog : LocalizableForm
	{
		private readonly Lazy<List<Template>> _templates;

		private class DBType
		{
			public QuerySource Id { get; set; }
			public string Name    { get; set; }
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="OpenConnectionDialog" /> class.
		/// </summary>
		public OpenConnectionDialog()
		{
			this._templates = new Lazy<List<Template>>(TemplateNodesLoader.GetTemplates);

			SetLocale();
			InitializeComponent();
			LoadSettings();
		}

		/// <summary>
		/// Selected database type.
		/// </summary>
		public QuerySource DataBaseType
		{
			get
			{
				QuerySource selectedQuerySourceId = QuerySource.MSSQL;

				if (this.cbDataBaseType != null)
				{
					DBType selectedDb = this.cbDataBaseType.SelectedItem as DBType;

					if (selectedDb != null)
					{
						selectedQuerySourceId = selectedDb.Id;
					}
				}

				return selectedQuerySourceId;
			}
		}

		/// <summary>
		///     Gets the selected template.
		/// </summary>
		public Template SelectedTemplate
		{
			get
			{
				return cbTemplate.SelectedItem as Template;
			}
		}

		/// <summary>
		/// Selected connection group
		/// </summary>
		public ConnectionGroupInfo SelectedConnectionGroup
		{
			get { return ((Tuple<string, ConnectionGroupInfo>)cbConnection.SelectedItem).Item2; }
		}

		/// <summary>
		///     The load settings.
		/// </summary>
		private void LoadSettings()
		{
			templateFileSettingBindingSource.DataSource = this._templates.Value;

			var dataBaseTypes = new List<DBType>();

			if (Program.Model.Settings.SystemSettings.ConnectionTypes != null)
			{
				foreach (ConnectionType currentModuleType in Program.Model.Settings.SystemSettings.ConnectionTypes)
				{
					QuerySource currentSource;

					if (Enum.TryParse(currentModuleType.Id, true, out currentSource))
					{
						var localeItem = currentModuleType.Locales.FirstOrDefault(
							l => l.Language == Program.Model.Settings.InterfaceLanguage
						);

						var localeText = localeItem != null ? localeItem.Text : currentModuleType.Id;

						dataBaseTypes.Add(
							new DBType()
							{
								Id   = currentSource,
								Name = localeText
							}
						);
					}
				}
			}

			this.dataBaseTypeBindingSource.DataSource = dataBaseTypes;

			if (this.cbDataBaseType.Items.Count > 0)
			{
				this.cbDataBaseType.SelectedItem = 0;
				LoadTemplateList();
			}
		}

		/// <summary>
		///     The set locale.
		/// </summary>
		private void SetLocale()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(Program.Model.Settings.InterfaceLanguage);
		}

		/// <summary>
		/// The bt ok_ click.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="e">
		/// The e.
		/// </param>
		private void BtOkClick(object sender, EventArgs e)
		{
			if (this.ValidateForm())
			{
				// TODO: save last selected template id
				this.DialogResult = DialogResult.OK;
			}
		}

		/// <summary>
		///     The validate form.
		/// </summary>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		private bool ValidateForm()
		{
			bool result = true;

			if ((cbTemplate.SelectedItem == null) && (cbTemplate.Enabled))
			{
				errorProvider.SetError(cbTemplate, GetLocalizedText("SelectTemplateError"));
				result = false;
			}
			else
			{
				errorProvider.SetError(cbTemplate, string.Empty);
			}

			string connectionString = this.cbConnection.Text;

			if (string.IsNullOrEmpty(connectionString) && cbConnection.Enabled)
			{
				this.errorProvider.SetError(this.cbConnection, this.GetLocalizedText("SelectServerError"));
				result = false;
			}
			else
			{
				this.errorProvider.SetError(this.cbConnection, string.Empty);
			}

			if (this.cbDataBaseType.SelectedItem == null)
			{
				this.errorProvider.SetError(this.cbDataBaseType, this.GetLocalizedText("SelectDataBaseError"));
				result = false;
			}
			else
			{
				this.errorProvider.SetError(this.cbDataBaseType, string.Empty);
			}

			return result;
		}

		internal void SetStoredConnections(IEnumerable<ConnectionGroupInfo> storedConnections)
		{
			cbConnection.Enabled = storedConnections != null;

			cbConnection.DisplayMember = "Item1";
			cbConnection.ValueMember   = "Item2";

			cbConnection.Items.Clear();

			if (storedConnections != null)
			{
				foreach (ConnectionGroupInfo connectionGroup in storedConnections)
				{
					string templateName = (from template in _templates.Value
						where template.Id.ToLower() == connectionGroup.TemplateId.ToLower()
						select template.Display).FirstOrDefault();

					if (templateName == null)
					{
						templateName = connectionGroup.TemplateFileName.Replace(".xml", "");
					}

					string name = connectionGroup.Name + " (" + templateName + ")";

					cbConnection.Items.Add(new Tuple<string, ConnectionGroupInfo>(name, connectionGroup));
				}

				if (cbConnection.Items.Count == 1)
				{
					cbConnection.SelectedIndex = 0;
				}
			}
		}

		private void cbDataBaseType_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadTemplateList();
		}

		private void LoadTemplateList()
		{
			List<Template> templates = _templates.Value;

			this.templateFileSettingBindingSource.DataSource =
				templates.Where(x => x.DBType == (QuerySource)cbDataBaseType.SelectedValue).ToList();
		}
	}
}
