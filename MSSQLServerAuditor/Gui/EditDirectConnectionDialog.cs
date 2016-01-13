using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	public partial class EditDirectConnectionDialog : DirectConnectionDialog
	{
		private readonly MsSqlAuditorModel   _model;
		private readonly ConnectionsManager  _connectionsManager;
		private readonly ConnectionGroupInfo _connectionGroup;
		private          bool                _updatesMade;

		public EditDirectConnectionDialog(
			MsSqlAuditorModel   model,
			ConnectionGroupInfo connectionGroup
		)
		{
			this._model = model;

			CurrentStorage storage   = this._model.DefaultVaultProcessor.CurrentStorage;
			this._connectionsManager = new ConnectionsManager(model);

			this._connectionGroup = connectionGroup;
			this._updatesMade     = false;

			SetLocale();

			InitializeComponent();
			InitializeEventHandlers();
			InitializeBindings();

			UpdateControlsState();
		}

		private void InitializeEventHandlers()
		{
			this.btnOk.Click                               += this.btnOk_Click;
			this.btnRemoveConnectionString.Click           += this.btnRemoveConnectionString_Click;
			this.btnAddConnectionString.Click              += this.btnAddConnectionString_Click;
			this.btnSelectConnection.Click                 += this.btnSelectConnection_Click;
			this.lstConnectionStrings.SelectedIndexChanged += this.lstConnectionStrings_SelectedIndexChanged;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (!ValidateConnections())
			{
				return;
			}

			if (this._updatesMade)
			{
				if (this._connectionGroup.Connections.Any())
				{
					QuerySource dbType = this._connectionGroup.Connections.First().Type;

					List<InstanceInfo> instances = this.lstConnectionStrings.Items
						.OfType<InstanceInfo>().Where(i => i.Type == dbType).ToList();

					this._connectionGroup.Connections = instances;

					foreach (InstanceInfo instanceInfo in instances)
					{
						instanceInfo.ConnectionGroup = this._connectionGroup;
					}

					this._connectionsManager.UpdateGroupInstances(
						this._connectionGroup, dbType.ToString()
					);
				}

				DialogResult = DialogResult.OK;
			}
			else
			{
				DialogResult = DialogResult.Cancel;
			}
		}

		private void lstConnectionStrings_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateControlsState();
		}

		private void btnSelectConnection_Click(object sender, EventArgs e)
		{
			RunSelectConnectionDialog(this._model);
		}

		private void btnAddConnectionString_Click(object sender, EventArgs e)
		{
			if (ValidateAddConnectionForm())
			{
				InstanceInfo instance = this.cmbConnection.SelectedItem as InstanceInfo;

				if (instance != null)
				{
					string                    cnnString = instance.GetConnectionString();
					IEnumerable<InstanceInfo> instances = this.lstConnectionStrings.Items.OfType<InstanceInfo>();

					if (!instances.Any(x => cnnString.Equals(x.GetConnectionString())))
					{
						lstConnectionStrings.Items.Add(instance);
						UpdateControlsState();
					}

					this._updatesMade = true;

					return;
				}

				ConnectionGroupInfo groupInfo = cmbConnection.SelectedItem as ConnectionGroupInfo;

				if (groupInfo != null)
				{
					lstConnectionStrings.Items.Clear();

					InstanceInfo[] instances = groupInfo.Connections.ToArray();
					lstConnectionStrings.Items.AddRange(instances);

					UpdateControlsState();
				}

				this._updatesMade = true;
			}
		}

		private void btnRemoveConnectionString_Click(object sender, EventArgs e)
		{
			lstConnectionStrings.Items.Remove(lstConnectionStrings.SelectedItem);

			this._updatesMade = true;

			UpdateControlsState();
		}

		private void UpdateControlsState()
		{
			btnAddConnectionString.Enabled    = this.cmbConnection.SelectedItem        != null;
			btnRemoveConnectionString.Enabled = this.lstConnectionStrings.SelectedItem != null;
		}

		private void InitializeBindings()
		{
			// retrieve connection type
			ConnectionType connectionType = null;
			InstanceInfo   firstInstance = this._connectionGroup.Connections.FirstOrDefault();

			if (firstInstance != null)
			{
				BindingWrapper<ConnectionType> typeWrapper = this._model.GetConnectionType(firstInstance.Type);

				if (typeWrapper != null)
				{
					connectionType = typeWrapper.Item;
					cmbDataBaseType.DataSource = Lists.Of(typeWrapper);
				}
			}

			Template template = null;

			if (this._connectionGroup.IsExternal)
			{
				this.optOpenTemplateFromFile.Checked = true;

				string path = Path.Combine(
					this._connectionGroup.TemplateDir,
					this._connectionGroup.TemplateFileName
				);

				this.cmbPathToFile.DataSource = Lists.Of(path);

				XmlSerializer serializer = new XmlSerializer(typeof(Template));
				XmlDocument docTemplate = new XmlDocument();

				docTemplate.Load(path);

				using (XmlNodeReader nodeReader = new XmlNodeReader(docTemplate))
				{
					using (XmlReader xmlReader = XmlReader.Create(nodeReader, XmlUtils.GetXmlReaderSettings()))
					{
						template = (Template) serializer.Deserialize(xmlReader);
					}
				}
			}
			else
			{
				this.optSelectExistTemplate.Checked = true;

				template = this._templates.Value.FirstOrDefault(t => t.Id == this._connectionGroup.TemplateId);

				if (template != null)
				{
					TemplateNodeLocaleInfo locale = template.Locales.FirstOrDefault(
						l => l.Language == Settings.InterfaceLanguage
					);

					string templateName = (locale ?? template.Locales.First()).Text;

					this.cmbTemplate.DataSource = Lists.Of(templateName.RemoveWhitespaces());
				}
			}

			if (template != null && connectionType != null)
			{
				string     templateType = template.Type;
				ModuleType moduleType   = connectionType.ModuleTypes.FirstOrDefault(
					m => m.Id == templateType
				);

				if (moduleType != null)
				{
					BindingWrapper<ModuleType> moduleWrapper = new BindingWrapper<ModuleType>(
						moduleType,
						module =>
						{
							i18n localeItem = module.Locales.FirstOrDefault(
								l => l.Language == Settings.InterfaceLanguage);

							string displayName = localeItem != null
								? localeItem.Text
								: module.Id;

							return displayName.RemoveWhitespaces();
						}
					);

					cmbModuleTypes.DataSource = Lists.Of(moduleWrapper);
				}

				FillConnections(connectionType);

				SelectCurrentGroup();

				txtGroupName.Text = this._connectionGroup.Name;
			}
		}

		private void SelectCurrentGroup()
		{
			// select current connection group in dropdown list
			if (this.cmbConnection.Items.Count > 0)
			{
				bool selected = false;

				foreach (ConnectionGroupInfo group in this.cmbConnection.Items.OfType<ConnectionGroupInfo>())
				{
					if (this._connectionGroup.Name.Equals(group.Name))
					{
						this.cmbConnection.SelectedItem = group;
						selected = true;
						break;
					}
				}

				if (!selected)
				{
					this.cmbConnection.SelectedIndex = 0;
				}
			}
		}

		private void FillConnections(ConnectionType connectionType)
		{
			this.cmbConnection.Items.Clear();

			LoadConnectionGroups(connectionType, this._connectionsManager);
			LoadConnectionInstances(connectionType, this._connectionsManager);

			lstConnectionStrings.Items.Clear();

			foreach (InstanceInfo instance in this._connectionGroup.Connections)
			{
				lstConnectionStrings.Items.Add(instance);
			}
		}

		private void SetLocale()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.InterfaceLanguage);
		}

		private SettingsInfo Settings
		{
			get { return this._model.Settings; }
		}
	}
}
