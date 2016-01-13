// -------------------------------------------------------------------------------------------------
// <copyright file="NewDirectConnectionDialog.cs" company="City24 Pty. Ltd.">
// </copyright>
//
// <summary>
//   The new direct connection dialog.
// </summary>
// -------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Microsoft.Data.ConnectionUI;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Loaders;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	///     The new direct connection dialog.
	/// </summary>
	public partial class NewDirectConnectionDialog : LocalizableForm
	{
		protected Lazy<List<Template>> Templates = new Lazy<List<Template>>(TemplateNodesLoader.GetTemplates);

		protected class DBType
		{
			public QuerySource  Id          { get; set; }
			public string       Name        { get; set; }
			public ModuleType[] ModuleTypes { get; set; }
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="NewDirectConnectionDialog" /> class.
		/// </summary>
		public NewDirectConnectionDialog()
		{
			ConnectionPropertiesList = new List<Tuple<DbConnectionStringBuilder, bool>>();

			this.SetLocale();
			this.InitializeComponent();
			this.LoadSettings();
		}

		/// <summary>
		///     Gets the connection properties. bool value = IsODBC
		/// </summary>
		public List<Tuple<DbConnectionStringBuilder, bool>> ConnectionPropertiesList
		{
			get;
			private set;
		}

		/// <summary>
		/// Connection group name.
		/// </summary>
		public string ConnectionGroupName { get; private set; }

		/// <summary>
		/// Selected database type.
		/// </summary>
		public QuerySource DataBaseType
		{
			get
			{
				DBType selectedDb = this.cbDataBaseType.SelectedItem as DBType;

				if (selectedDb != null)
				{
					return selectedDb.Id;
				}

				return QuerySource.MSSQL;
			}
		}

		/// <summary>
		///     Gets the selected template.
		/// </summary>
		public string SelectedTemplateFile
		{
			get
			{
				if (this.rbOpenTemplateFromFile.Checked)
				{
					return this.cbPathToFile.Text;
				}
				else if (this.rbSelectExistTemplate.Checked)
				{
					Template template = this.cbTemplate.SelectedItem as Template;

					if (template == null)
					{
						return string.Empty;
					}

					return Program.Model.Settings.TemplateDirectory + @"\" +
						TemplateNodesLoader.GetTemplateById(template.Id).Item1;
				}

				return string.Empty;
			}
		}

		public string SelectedTemplateFileFullPath
		{
			get
			{
				if (this.rbOpenTemplateFromFile.Checked)
				{
					return this.cbPathToFile.Text;
				}
				else if (this.rbSelectExistTemplate.Checked)
				{
					Template template = this.cbTemplate.SelectedItem as Template;

					if (template == null)
					{
						return string.Empty;
					}

					return Environment.CurrentDirectory + @"\" + Program.Model.Settings.TemplateDirectory + @"\" +
						TemplateNodesLoader.GetTemplateById(template.Id).Item1;
				}

				return string.Empty;
			}
		}

		/// <summary>
		///     Gets is External template.
		/// </summary>
		public bool IsExternalTemplateFile
		{
			get { return this.rbOpenTemplateFromFile.Checked; }
		}

		/// <summary>
		///     The set locale.
		/// </summary>
		private void SetLocale()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(Program.Model.Settings.InterfaceLanguage);
		}

		/// <summary>
		/// Returns true if ldcGroup is last selected element in the combobox
		/// </summary>
		///
		private bool IsConnStrGroupWasSelected(LastDirectConnectionStringGroup ldcGroup)
		{
			List<string> singleConnNames   = Program.Model.Settings.LastDirectConnectionStrings.Select(x => x.Name).ToList();
			string[]     groupNameElements = ldcGroup.GroupName.Split(','); // Group name consists from names of single connections

			for (int i = 0; i < groupNameElements.Length;i++)
			{
				if (i > singleConnNames.Count - 1)
				{
					return false;
				}

				if (singleConnNames.ElementAt(i) != groupNameElements[i])
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		///     The load settings.
		/// </summary>
		private void LoadSettings()
		{
			List<DBType> dataBaseTypes = new List<DBType>();

			if (Program.Model.Settings.SystemSettings.ConnectionTypes != null)
			{
				foreach (ConnectionType currentModuleType in Program.Model.Settings.SystemSettings.ConnectionTypes)
				{
					QuerySource currentSource;

					if (Enum.TryParse(currentModuleType.Id, true, out currentSource))
					{
						i18n localeItem = currentModuleType.Locales.FirstOrDefault(
							l => l.Language == Program.Model.Settings.InterfaceLanguage);

						string localeText = localeItem != null ? localeItem.Text : currentModuleType.Id;

						dataBaseTypes.Add(new DBType() { Id = currentSource, Name = localeText, ModuleTypes = currentModuleType.ModuleTypes });
					}
				}
			}

			this.dataBaseTypeBindingSource.DataSource = dataBaseTypes;

			if (this.cbDataBaseType.Items.Count > 0)
			{
				if (!string.IsNullOrEmpty(Program.Model.Settings.LastSelectedDataBaseType))
				{
					DBType selItem = dataBaseTypes.FirstOrDefault(d => d.Name == Program.Model.Settings.LastSelectedDataBaseType);

					if (selItem != null)
					{
						this.cbDataBaseType.SelectedItem = selItem;
						setModuleTypeItems(selItem);
					}
					else
					{
						this.cbDataBaseType.SelectedItem = 0;
					}
				}
				else
				{
					this.cbDataBaseType.SelectedItem = 0;
				}
			}

			cbConnectionUpdate();

			if (cbModuleType.Items.Count > 0)
			{
				if (!string.IsNullOrEmpty(Program.Model.Settings.LastSelectedTemplateType))
				{
					List<KeyValuePair<string, string>> moduleTypes = cbModuleType.DataSource as List<KeyValuePair<string, string>>;
					KeyValuePair<string,string>        selItem     = moduleTypes.FirstOrDefault(m => m.Value == Program.Model.Settings.LastSelectedTemplateType);

					if (selItem.Value != null)
					{
						this.cbModuleType.SelectedItem = selItem;
					}
					else
					{
						this.cbModuleType.SelectedItem = 0;
					}
				}
				else
				{
					this.cbModuleType.SelectedItem = 0;
				}
			}

			if (!string.IsNullOrEmpty(Program.Model.Settings.LastSelectedTemplateId) &&
				this.cbTemplate.Items.Cast<Template>()
					.Any(x => x.Id == Program.Model.Settings.LastSelectedTemplateId))
			{
				this.cbTemplate.SelectedValue = Program.Model.Settings.LastSelectedTemplateId;
			}

			UpdateAddConnectionButtonState();
		}

		private void cbConnectionUpdate()
		{
			string                          selDbType     = String.Empty;
			LastDirectConnectionStringGroup selectedGroup = null;

			if (cbDataBaseType.SelectedItem != null)
			{
				DBType selItem = cbDataBaseType.SelectedItem as DBType;
				setModuleTypeItems(selItem);
				selDbType = selItem.Id.ToString();
			}

			cbConnection.Items.Clear();

			foreach (LastDirectConnectionString connectionString in Program.Model.Settings.LastDirectConnectionStringsItems.Where(c => c.DataBaseType == selDbType))
			{
				this.cbConnection.Items.Add(connectionString);
			}

			foreach (LastDirectConnectionStringGroup connectionStringGroup in Program.Model.Settings.LastDirectConnectionStringsGroups)
			{
				LastDirectConnectionString firstGroupConnection = connectionStringGroup.ConnectionStrings.ElementAt(0);
				if (firstGroupConnection.DataBaseType != selDbType)
				{
					continue;
				}

				if (connectionStringGroup.ConnectionStrings.Count == 1 &&
				    firstGroupConnection.ConnectionString.Equals(connectionStringGroup.GroupName))
				{
					continue;
				}

				this.cbConnection.Items.Add(connectionStringGroup);

				if (IsConnStrGroupWasSelected(connectionStringGroup) && selectedGroup == null)
				{
					selectedGroup = connectionStringGroup;
				}
			}

			if (this.cbConnection.Items.Count == 1)
			{
				this.cbConnection.SelectedIndex = 0;
				return;
			}

			if (selectedGroup != null)
			{
				this.cbConnection.SelectedItem = selectedGroup;
			}
			else
			{
				LastDirectConnectionString ldcStr = Program.Model.Settings.LastDirectConnectionStrings.FirstOrDefault();

				if (ldcStr != null)
				{
					foreach (object item in cbConnection.Items)
					{

						if (!(item is LastDirectConnectionString))
						{
							continue;
						}

						string itemName = (item as LastDirectConnectionString).Name;

						if (itemName == ldcStr.Name)
						{
							cbConnection.SelectedItem = item;
						}
					}
				}
			}
		}

		private void DisplayModules()
		{
			List<Template> templates = Templates.Value;

			this.templateFileSettingBindingSource.DataSource = templates.Where(x=>x.Type == (String)cbModuleType.SelectedValue).ToList();
			cbTemplate.Enabled = (cbModuleType.SelectedItem != null);
		}

		/// <summary>
		/// The set connection properties.
		/// </summary>
		/// <param name="connectionString">
		/// The connection string.
		/// </param>
		private void SetConnectionProperties(List<LastDirectConnectionString> connectionStrings)
		{
			this.ConnectionPropertiesList.Clear();

			foreach (LastDirectConnectionString connectionString in connectionStrings)
			{
				DbConnectionStringBuilder builder = GetConnectionStringBuilder(connectionString);

				Tuple<DbConnectionStringBuilder, bool> connectionProperty =
					new Tuple<DbConnectionStringBuilder, bool>(
						builder,
						connectionString.IsODBC
					);

				IEnumerable<string> existingString =
					ConnectionPropertiesList.Select(x => x.Item1.ConnectionString);

				if (!existingString.Contains(connectionProperty.Item1.ConnectionString))
				{
					this.ConnectionPropertiesList.Add(connectionProperty);
				}
			}
		}

		private static DbConnectionStringBuilder GetConnectionStringBuilder(LastDirectConnectionString connectionString)
		{
			if (connectionString.IsODBC)
			{
				return new OdbcConnectionStringBuilder(connectionString.ConnectionString);
			}

			DbConnectionStringBuilder builder;
			QuerySource               dbType;

			if (Enum.TryParse(connectionString.DataBaseType, out dbType))
			{
				switch (dbType)
				{
					case QuerySource.MSSQL:
						builder = new SqlConnectionStringBuilder();
						break;
					case QuerySource.SQLite:
						// ODBC connection string builder is used
						// for SQLite internal connection
						// since it handles special connection string adequately
						builder = new OdbcConnectionStringBuilder();
						break;
					default:
						builder = new DbConnectionStringBuilder();
						break;
				}
			}
			else
			{
				builder = new DbConnectionStringBuilder();
			}

			builder.ConnectionString = connectionString.ConnectionString;
			return builder;
		}

		/// <summary>
		/// The bt select connection_ click.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="e">
		/// The e.
		/// </param>
		private void BtSelectConnectionClick(object sender, EventArgs e)
		{
			DBType dbType           = null;
			string connectionString = null;
			string connectionName   = null;
			bool   isODBC           = false;
			bool   isDialogResultOk = false;

			if (cbDataBaseType.SelectedItem != null)
			{
				dbType = cbDataBaseType.SelectedItem as DBType;

				switch (dbType.Id)
				{
					case QuerySource.MSSQL:
						DataConnectionDialog dataConnectionDialog = GetDataConnectionDialog();

						if (DataConnectionDialog.Show(dataConnectionDialog) == DialogResult.OK)
						{
							isDialogResultOk = true;

							connectionString = dataConnectionDialog.ConnectionString;
							connectionName   = (dataConnectionDialog.ConnectionProperties["Dsn"] as string)
								?? (dataConnectionDialog.ConnectionProperties["Data Source"] as string);
							isODBC           = (dataConnectionDialog.SelectedDataSource == DataSource.OdbcDataSource);
						}
						break;

					case QuerySource.SQLite:
						InternalSqliteConnectionDialog sqliteConnectionDialog = CreateInternalSqliteConnectionDialog();

						if (sqliteConnectionDialog.ShowDialog() == DialogResult.OK)
						{
							isDialogResultOk = true;

							connectionString = sqliteConnectionDialog.ConnectionString;
							connectionName   = sqliteConnectionDialog.ConnectionString;
							isODBC = false;
						}
						break;

					case QuerySource.NetworkInformation:
						NetworkInformationConnectionDialog networkConnectionDialog = new NetworkInformationConnectionDialog();

						if (networkConnectionDialog.ShowDialog() == DialogResult.OK)
						{
							isDialogResultOk = true;

							connectionString = networkConnectionDialog.ConnectionString;
							connectionName   = networkConnectionDialog.ConnectionString;
							isODBC = false;
						}
						break;

					default:
						NonSqlDataConnectionDialog nonSqlDataConnectionDialog = new NonSqlDataConnectionDialog();

						if (nonSqlDataConnectionDialog.ShowDialog() == DialogResult.OK)
						{
							isDialogResultOk = true;

							connectionString = nonSqlDataConnectionDialog.ConnectionString;
							connectionName   = nonSqlDataConnectionDialog.ConnectionString;
							isODBC           = false;
						}
						break;
				}

				if (isDialogResultOk)
				{
					selectSqlConnection(
						connectionString,
						dbType,
						connectionName,
						isODBC
					);
				}
			}
		}

		private InternalSqliteConnectionDialog CreateInternalSqliteConnectionDialog()
		{
			CurrentStorage storage = Program.Model.DefaultVaultProcessor.CurrentStorage;
			return new InternalSqliteConnectionDialog(storage);
		}

		private void selectSqlConnection(string connectionString, DBType dbType, string connectionName, bool isODBC)
		{
			if (!this.cbConnection.Items.OfType<LastDirectConnectionString>().Any(
				x => x.ConnectionString == connectionString
				&& x.IsODBC == isODBC
				)
			)
			{
				LastDirectConnectionString newItem = new LastDirectConnectionString()
				{
					DataBaseType     = dbType.Id.ToString(),
					ConnectionString = connectionString,
					IsODBC           = isODBC,
					Name             = connectionName
				};

				this.cbConnection.Items.Add(newItem);
				this.cbConnection.SelectedItem = newItem;
			}
			else
			{
				LastDirectConnectionString item =
					this.cbConnection.Items.OfType<LastDirectConnectionString>()
						.First(x => x.ConnectionString == connectionString && x.IsODBC == isODBC);

				this.cbConnection.SelectedItem = item;
			}
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
			if (!this.ValidateForm())
			{
				return;
			}

			SettingsInfo settingsCopy = Program.Model.Settings.GetCopy();

			List<LastDirectConnectionString> connectionStrings =
				this.connectionStringsListBox.Items.OfType<LastDirectConnectionString>()
					.ToList();

			foreach (LastDirectConnectionString connectionString in connectionStrings)
			{
				if (connectionString != null &&
					!settingsCopy.LastDirectConnectionStringsItems.Any(
						x => x.ConnectionString == connectionString.ConnectionString
						&& x.IsODBC == connectionString.IsODBC
					)
				)
				{
					settingsCopy.LastDirectConnectionStringsItems.Add(connectionString);
				}
			}

			LastDirectConnectionStringGroup newGroup = new LastDirectConnectionStringGroup()
			{
				ConnectionStrings = connectionStrings,
				GroupName = ConnectionGroupName
			};

			// check if the group does not exist
			if (!settingsCopy.LastDirectConnectionStringsGroups.Any(x => x.Equals(newGroup)))
			{
				bool addNewGroup = true;

				// check whether the group with the same name
				// (but with different settings) exists
				foreach (LastDirectConnectionStringGroup connGroup in settingsCopy.LastDirectConnectionStringsGroups)
				{
					if (LastDirectConnectionStringGroup.InSettingsComparer.Equals(newGroup, connGroup))
					{
						// update it with new settings
						connGroup.ConnectionStrings = newGroup.ConnectionStrings;
						addNewGroup = false;
						break;
					}
				}

				if (addNewGroup)
				{
					settingsCopy.LastDirectConnectionStringsGroups.Add(newGroup);
				}
			}

			settingsCopy.LastDirectConnectionStrings = connectionStrings;

			if (rbSelectExistTemplate.Checked)
			{
				Template templateFile = this.cbTemplate.SelectedItem as Template;

				if (templateFile != null)
				{
					settingsCopy.LastSelectedTemplateId = templateFile.Id;
				}
			}
			else if (rbOpenTemplateFromFile.Checked)
			{
				string templateFile = this.cbPathToFile.Text;

				if (!String.IsNullOrWhiteSpace(templateFile) && File.Exists(templateFile))
				{
					Program.Model.Settings.LastExternalTemplatesItems.Add(templateFile);
					settingsCopy.LastSelectedTemplateId = templateFile;
				}
			}

			if (this.cbDataBaseType.SelectedItem != null)
			{
				settingsCopy.LastSelectedDataBaseType = (this.cbDataBaseType.SelectedItem as DBType).Name;
			}

			if (this.cbModuleType.SelectedItem !=null)
			{
				settingsCopy.LastSelectedTemplateType = ((KeyValuePair<string, string>)this.cbModuleType.SelectedItem).Value;
			}

			Program.Model.SetSettings(settingsCopy);

			if (Program.Model.Settings.LastDirectConnectionStrings != null &&
				Program.Model.Settings.LastDirectConnectionStrings.Any(
					x => !string.IsNullOrEmpty(x.ConnectionString)
				)
			)
			{
				this.SetConnectionProperties(
					Program.Model.Settings.LastDirectConnectionStrings.Where(
						x => !string.IsNullOrEmpty(x.ConnectionString)
					).ToList()
				);
			}

			this.DialogResult = DialogResult.OK;
		}

		/// <summary>
		///     The validate form.
		/// </summary>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		private bool ValidateForm()
		{
			bool   result = true;
			string path   = null;

			if (this.rbSelectExistTemplate.Checked && this.cbTemplate.SelectedItem!=null)
			{
				path = (this.cbTemplate.SelectedItem as Template).Id;
			}
			else if (this.rbOpenTemplateFromFile.Checked)
			{
				path = this.cbPathToFile.Text;
			}

			if (String.IsNullOrWhiteSpace(path))
			{
				if (this.rbSelectExistTemplate.Checked)
				{
					this.errorProvider.SetError(this.cbTemplate, this.GetLocalizedText("SelectTemplateError"));
				}
				else if (this.rbOpenTemplateFromFile.Checked && String.IsNullOrWhiteSpace(this.cbPathToFile.Text))
				{
					this.errorProvider.SetError(this.btnOpenTemplate, this.GetLocalizedText("SelectTemplateFileError"));
				}

				result = false;
			}
			else
			{
				if (this.rbOpenTemplateFromFile.Checked && !File.Exists(path))
				{
					this.errorProvider.SetError(this.btnOpenTemplate, this.GetLocalizedText("SelectTemplateFileNotExistError"));
					result = false;
				}
				else
				{
					this.errorProvider.SetError(this.cbTemplate, string.Empty);
				}
			}

			foreach (LastDirectConnectionString currentConnection in connectionStringsListBox.Items.OfType<LastDirectConnectionString>()
				.Cast<LastDirectConnectionString>())
			{
				if (currentConnection == null || string.IsNullOrEmpty(currentConnection.ConnectionString))
				{
					this.errorProvider.SetError(this.connectionStringsListBox, this.GetLocalizedText("SelectServerError"));
					result = false;
				}
				else
				{
					this.errorProvider.SetError(this.connectionStringsListBox, string.Empty);
				}
			}

			if (!connectionStringsListBox.Items.OfType<LastDirectConnectionString>().Any())
			{
				this.errorProvider.SetError(this.connectionStringsListBox, this.GetLocalizedText("SelectServerError"));

				result = false;
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

		private bool ValidateAddConnectionForm()
		{
			bool result = true;

			LastDirectConnectionString connectionString =
				this.cbConnection.SelectedItem as LastDirectConnectionString;

			LastDirectConnectionStringGroup connectionStringGroup =
				this.cbConnection.SelectedItem as LastDirectConnectionStringGroup;

			if ((connectionString == null || string.IsNullOrEmpty(connectionString.ConnectionString)) &&
				(connectionStringGroup == null || connectionStringGroup.ConnectionStrings == null))
			{
				this.errorProvider.SetError(this.cbConnection, this.GetLocalizedText("SelectServerError"));

				result = false;
			}
			else
			{
				this.errorProvider.SetError(this.cbConnection, string.Empty);
			}

			return result;
		}

		private DataConnectionDialog GetDataConnectionDialog()
		{
			DataConnectionDialog connectionDialog = new DataConnectionDialog();

			connectionDialog.DataSources.Add(DataSource.OdbcDataSource);
			connectionDialog.DataSources.Add(DataSource.SqlDataSource);
			connectionDialog.SelectedDataSource = DataSource.SqlDataSource;

			DataConnectionConfiguration dcs = new DataConnectionConfiguration(null);

			dcs.LoadConfiguration(connectionDialog);

			return connectionDialog;
		}

		private void cbDataBaseType_SelectedIndexChanged(object sender, EventArgs e)
		{
			DBType selectedDbType = cbDataBaseType.SelectedItem as DBType;
			setModuleTypeItems(selectedDbType);
			cbConnectionUpdate();
		}

		private void setModuleTypeItems(DBType selectedDbType)
		{
			if (selectedDbType != null && selectedDbType.ModuleTypes != null && selectedDbType.ModuleTypes.Count() > 0)
			{
				List<KeyValuePair<string, string>> moduleTypes = new List<KeyValuePair<string, string>>();

				foreach (ModuleType currentModuleType in selectedDbType.ModuleTypes)
				{
					i18n localeItem = currentModuleType.Locales.FirstOrDefault(
						l => l.Language == Program.Model.Settings.InterfaceLanguage);

					string localeText = localeItem != null ? localeItem.Text : currentModuleType.Id;

					moduleTypes.Add(new KeyValuePair<string, string>(localeText, currentModuleType.Id));
				}

				cbModuleType.DataSource = moduleTypes;
				cbModuleType.Enabled    = true;
			}
			else
			{
				cbModuleType.Enabled = false;
			}
		}

		private void addConnectionStringButton_Click(object sender, EventArgs e)
		{
			if (this.ValidateAddConnectionForm())
			{
				if (cbConnection.SelectedItem is LastDirectConnectionString)
				{
					if (
						!connectionStringsListBox.Items.OfType<LastDirectConnectionString>()
							.Any(x => x.Equals(cbConnection.SelectedItem)))
					{
						connectionStringsListBox.Items.Add(cbConnection.SelectedItem);
						UpdateAddConnectionButtonState();
					}
				}
				else if (cbConnection.SelectedItem is LastDirectConnectionStringGroup)
				{
					LastDirectConnectionStringGroup connectionGroup = cbConnection.SelectedItem as LastDirectConnectionStringGroup;

					connectionStringsListBox.Items.Clear();
					connectionStringsListBox.Items.AddRange(connectionGroup.ConnectionStrings.ToArray());

					UpdateAddConnectionButtonState(connectionGroup.GroupName);
				}
			}
		}

		private void removeTemplateButton_Click(object sender, EventArgs e)
		{
			connectionStringsListBox.Items.Remove(connectionStringsListBox.SelectedItem);
			UpdateAddConnectionButtonState();
		}

		private void UpdateAddConnectionButtonState(string groupName = null)
		{
			if (cbConnection.SelectedItem != null)
			{
				addConnectionStringButton.Enabled = true;
			}
			else
			{
				addConnectionStringButton.Enabled = false;
			}

			if (connectionStringsListBox.SelectedItem != null)
			{
				removeConnectionStringButton.Enabled = true;
			}
			else
			{
				removeConnectionStringButton.Enabled = false;
			}

			List<LastDirectConnectionString> connections =
				connectionStringsListBox.Items.OfType<LastDirectConnectionString>().ToList();

			string name = !string.IsNullOrEmpty(groupName)
				? groupName
				: string.Join(",", connections.Select(x => x.Name));

			name = name.TrimEnd(',');

			ConnectionGroupName = tbGroupName.Text = name;

			if (connections != null)
			{
				tbGroupName.Enabled = connections.Count > 0;
			}
			else
			{
				tbGroupName.Enabled = false;
			}
		}

		private void cbConnection_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateAddConnectionButtonState(tbGroupName.Text);
		}

		private void connectionStringsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateAddConnectionButtonState(tbGroupName.Text);
		}

		private void tbGroupName_KeyDown(object sender, KeyEventArgs e)
		{
			ConnectionGroupName = tbGroupName.Text;
		}

		private void tbGroupName_TextChanged(object sender, EventArgs e)
		{
			ConnectionGroupName = tbGroupName.Text;
		}

		private void rbSelectExistTemplate_CheckedChanged(object sender, EventArgs e)
		{
			cbTemplate.Enabled = rbSelectExistTemplate.Checked;
		}

		private void rbOpenTemplateFromFile_CheckedChanged(object sender, EventArgs e)
		{
			cbPathToFile.Enabled    = rbOpenTemplateFromFile.Checked;
			btnOpenTemplate.Enabled = rbOpenTemplateFromFile.Checked;
		}

		private void btnOpenTemplate_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog fileDialog = new OpenFileDialog())
			{
				// "XML template files (*.xml)|*.xml";
				fileDialog.Filter           = Program.Model.LocaleManager.GetLocalizedText("NewDirectConnectionDialog", "templateFileFilter");
				fileDialog.FilterIndex      = 1;
				fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				fileDialog.Multiselect      = false;

				if (fileDialog.ShowDialog() == DialogResult.OK)
				{
					cbPathToFile.Text = fileDialog.FileName;
				}
			}
		}

		private void cbModuleType_SelectedIndexChanged(object sender, EventArgs e)
		{
			DisplayModules();
		}
	}
}
