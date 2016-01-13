using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Loaders;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables;
using MSSQLServerAuditor.SQLite.Tables.Directories;

namespace MSSQLServerAuditor.Gui
{
	public partial class CreateDirectConnectionDialog : DirectConnectionDialog
	{
		private readonly MsSqlAuditorModel         _model;
		private readonly CurrentStorage            _storage;
		private readonly ConnectionsManager        _connectionsManager;
		private Dictionary<string, ConnectionInfo> _lastConnections;
		private ConnectionData                     _resultConnection;

		private class ConnectionInfo
		{
			public long   ConnectionId;
			public string ConnectionType;
			public string GroupName;
			public string TemplateId;
			public string TemplateFile;
			public string TemplateDir;
		}

		#region Constuctors

		public CreateDirectConnectionDialog() : this(Program.Model)
		{
		}

		public CreateDirectConnectionDialog(MsSqlAuditorModel model)
		{
			this._model              = model;
			this._storage            = this._model.DefaultVaultProcessor.CurrentStorage;
			this._connectionsManager = new ConnectionsManager(model);

			SetLocale();

			InitializeComponent();
			InitializeEventHandlers();
			InitializeBindings();
		}

		#endregion

		#region Public API

		public ConnectionData ResultConnection
		{
			get { return this._resultConnection; }
		}

		private string ConnectionGroupName
		{
			get { return txtGroupName.Text; }
		}

		private string GetSelectedTemplateFile(bool fullPath)
		{
			if (this.optOpenTemplateFromFile.Checked)
			{
				return this.cmbPathToFile.Text;
			}

			if (this.optSelectExistTemplate.Checked)
			{
				Template template = this.cmbTemplate.SelectedItem as Template;

				if (template == null)
				{
					return string.Empty;
				}

				string templateName = TemplateNodesLoader.GetTemplateById(template.Id).Item1;
				string path         = Settings.TemplateDirectory + @"\" + templateName;

				if (fullPath)
				{
					path = Environment.CurrentDirectory + @"\" + path;
				}

				return path;
			}

			return string.Empty;
		}

		private string SelectedTemplateFile
		{
			get { return GetSelectedTemplateFile(false); }
		}

		public string SelectedTemplateFileFullPath
		{
			get { return GetSelectedTemplateFile(true); }
		}

		private bool IsExternalTemplateFile
		{
			get { return this.optOpenTemplateFromFile.Checked; }
		}

		#endregion

		#region Event handlers

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (!ValidateForm())
			{
				return;
			}

			ConnectionGroupInfo group = PrepareGroupResult();
			this._resultConnection    = new ConnectionData(this._model, group);

			group.ReadGroupIdFrom(this._storage.ConnectionGroupDirectory);

			long? groupId    = group.Identity;
			long? templateId = this._storage.TemplateDirectory.GetId(group);
			if (groupId != null && templateId != null)
			{
				SaveLastConnection(
					groupId.Value,
					templateId.Value);
			}

			DialogResult = DialogResult.OK;
		}

		private void cmbModuleTypes_SelectedIndexChanged(object sender, EventArgs e)
		{
			ConnectionInfo restoreConnection = GetLastConnectionForCurrentProtocol();

			BindTemplate(restoreConnection);
		}

		private void cmbDataBaseType_SelectedIndexChanged(object sender, EventArgs e)
		{
			ConnectionInfo restoreConnection = GetLastConnectionForCurrentProtocol();

			BindModuleType(restoreConnection);
			UpdateConnections(restoreConnection);
		}

		private void optOpenTemplateFromFile_CheckedChanged(object sender, EventArgs e)
		{
			bool openFile = this.optOpenTemplateFromFile.Checked;

			this.cmbPathToFile.Enabled   = openFile;
			this.btnOpenTemplate.Enabled = openFile;
		}

		private void optSelectExistTemplate_CheckedChanged(object sender, EventArgs e)
		{
			this.cmbTemplate.Enabled = this.optSelectExistTemplate.Checked;
		}

		private void btnOpenTemplate_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog fileDialog = new OpenFileDialog())
			{
				// "XML template files (*.xml)|*.xml";
				fileDialog.Filter           = GetLocalizedText("templateFileFilter");
				fileDialog.FilterIndex      = 1;
				fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				fileDialog.Multiselect      = false;

				if (fileDialog.ShowDialog() == DialogResult.OK)
				{
					this.cmbPathToFile.Text = fileDialog.FileName;
				}
			}
		}

		private void lstConnectionStrings_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateControlsState(txtGroupName.Text);
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

					return;
				}

				ConnectionGroupInfo groupInfo = cmbConnection.SelectedItem as ConnectionGroupInfo;

				if (groupInfo != null)
				{
					lstConnectionStrings.Items.Clear();

					InstanceInfo[] instances = groupInfo.Connections.ToArray();
					lstConnectionStrings.Items.AddRange(instances);

					UpdateControlsState(groupInfo.Name);
				}
			}
		}

		private void btnRemoveConnectionString_Click(object sender, EventArgs e)
		{
			lstConnectionStrings.Items.Remove(lstConnectionStrings.SelectedItem);
			UpdateControlsState();
		}

		private void cmbConnection_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateControlsState(txtGroupName.Text);
		}

		#endregion

		#region Helper methods

		private void InitializeEventHandlers()
		{
			this.btnOk.Click                               += this.btnOk_Click;
			this.cmbModuleTypes.SelectedIndexChanged       += this.cmbModuleTypes_SelectedIndexChanged;
			this.cmbDataBaseType.SelectedIndexChanged      += this.cmbDataBaseType_SelectedIndexChanged;
			this.optOpenTemplateFromFile.CheckedChanged    += this.optOpenTemplateFromFile_CheckedChanged;
			this.optSelectExistTemplate.CheckedChanged     += this.optSelectExistTemplate_CheckedChanged;
			this.btnOpenTemplate.Click                     += this.btnOpenTemplate_Click;
			this.btnRemoveConnectionString.Click           += this.btnRemoveConnectionString_Click;
			this.cmbConnection.SelectedIndexChanged        += this.cmbConnection_SelectedIndexChanged;
			this.btnAddConnectionString.Click              += this.btnAddConnectionString_Click;
			this.btnSelectConnection.Click                 += this.btnSelectConnection_Click;
			this.lstConnectionStrings.SelectedIndexChanged += this.lstConnectionStrings_SelectedIndexChanged;
		}

		private void InitializeBindings()
		{
			// set data source type bindings
			List<BindingWrapper<ConnectionType>> connectionTypes = this._model.ConnectionTypes;

			this._lastConnections = ReadLastConnections(connectionTypes.Select(bw => bw.Item).ToList());

			// retrieve last connection group
			LastConnectionRow lastConnectionRow =
				this._storage.LastConnectionTable.GetLastConnection(Environment.MachineName);

			ConnectionInfo lastConnection = null;
			if (lastConnectionRow != null)
			{
				lastConnection = this._lastConnections.Values
					.FirstOrDefault(lc => lastConnectionRow.LastConnectionProtocolId == lc.ConnectionId);
			}

			this.bsConnectionType.DataSource = connectionTypes;
			this.bsConnectionType.DataMember = "Item";

			this.cmbDataBaseType.DataSource = bsConnectionType.DataSource;

			// select data source type used in previous session
			if (this.cmbDataBaseType.Items.Count > 0)
			{
				this.cmbDataBaseType.SelectedItem = 0;
				if (lastConnection != null)
				{
					BindingWrapper<ConnectionType> selItem = connectionTypes.FirstOrDefault(
						ct => ct.Item.Id == lastConnection.ConnectionType);

					if (selItem != null)
					{
						this.cmbDataBaseType.SelectedItem = selItem;
					}
				}

				// set module type bindings
				BindModuleType(lastConnection);
			}

			// load external templates paths from sqlite db
			cmbPathToFile.Items.Clear();
			List<TemplateRow> externalTemplates = this._storage.TemplateDirectory.GetExternalTemplates();
			foreach (TemplateRow externalTemplate in externalTemplates)
			{
				string path = Path.Combine(externalTemplate.Directory, externalTemplate.Id);
				cmbPathToFile.Items.Add(path);
			}

			UpdateConnections(lastConnection);
			BindTemplate(lastConnection);

			UpdateControlsState();
		}

		private void BindModuleType(ConnectionInfo restoreConnection)
		{
			this.cmbModuleTypes.Enabled = false;

			BindingWrapper<ConnectionType> cnnWrapper = (BindingWrapper<ConnectionType>)this.cmbDataBaseType.SelectedItem;

			if (cnnWrapper != null)
			{
				ConnectionType cnnType = cnnWrapper.Item;

				List<BindingWrapper<ModuleType>> moduleTypes = this._model.GetModuleTypes(cnnType);

				if (moduleTypes.Count > 0)
				{
					this.bsModuleType.DataSource   = moduleTypes;
					this.bsModuleType.DataMember   = "Item";
					this.cmbModuleTypes.DataSource = this.bsModuleType.DataSource;
					this.cmbModuleTypes.Enabled    = true;

					// select module type used in previous session
					if (restoreConnection != null)
					{
						bool selected = false;
						Template template = this._templates.Value.FirstOrDefault(t => restoreConnection.TemplateId == t.Id);
						if (template != null)
						{
							BindingWrapper<ModuleType> lastModule = moduleTypes.FirstOrDefault(
								m => m.Item.Id == template.Type);

							if (lastModule != null)
							{
								this.cmbModuleTypes.SelectedItem = lastModule;
								selected = true;
							}
						}

						if (!selected)
						{
							this.cmbModuleTypes.SelectedIndex = 0;
						}
					}
				}
			}
		}

		private void UpdateConnections(ConnectionInfo restoreConnection)
		{
			cmbConnection.Items.Clear();

			BindingWrapper<ConnectionType> typeWrapper = this.cmbDataBaseType.SelectedItem as BindingWrapper<ConnectionType>;

			if (typeWrapper != null)
			{
				ConnectionType selectedConnection = typeWrapper.Item;

				LoadConnectionGroups(selectedConnection,    this._connectionsManager);
				LoadConnectionInstances(selectedConnection, this._connectionsManager);

				UpdateConnectionSelection(restoreConnection);
			}
		}

		private void BindTemplate(ConnectionInfo restoreConnection)
		{
			BindingWrapper<ModuleType> selectedModule = cmbModuleTypes.SelectedValue as BindingWrapper<ModuleType>;

			if (selectedModule != null)
			{
				List<Template> templates = this._templates.Value;

				this.bsTemplateFileSetting.DataSource = templates.Where(
					x => x.Type == selectedModule.Item.Id).ToList();

				cmbTemplate.Enabled = (cmbModuleTypes.SelectedItem != null);

				optOpenTemplateFromFile.Checked = false;
				optSelectExistTemplate.Checked  = true;
				cmbPathToFile.ResetText();
				if (restoreConnection != null)
				{
					if (this.cmbTemplate.Items.Count > 0)
					{
						this.cmbTemplate.SelectedIndex = 0;
					}

					string templateDir = restoreConnection.TemplateDir;
					if (!string.IsNullOrEmpty(templateDir) && Path.IsPathRooted(templateDir))
					{
						optOpenTemplateFromFile.Checked = true;
						optSelectExistTemplate.Checked  = false;

						cmbPathToFile.Text = Path.Combine(templateDir, restoreConnection.TemplateFile);
					}
					else
					{
						foreach (Template template in this.cmbTemplate.Items.OfType<Template>())
						{
							if (template != null && template.Id == restoreConnection.TemplateId)
							{
								this.cmbTemplate.SelectedItem = template;
								break;
							}
						}
					}
				}
			}
		}

		private void UpdateConnectionSelection(ConnectionInfo restoreConnection)
		{
			if (this.cmbConnection.Items.Count < 1)
			{
				return;
			}

			if (this.cmbConnection.Items.Count == 1)
			{
				this.cmbConnection.SelectedIndex = 0;
				return;
			}

			if (restoreConnection != null)
			{
				List<ConnectionGroupInfo> groups = this.cmbConnection.Items
					.OfType<ConnectionGroupInfo>().ToList();

				foreach (ConnectionGroupInfo group in groups)
				{
					if (group.Name == restoreConnection.GroupName)
					{
						this.cmbConnection.SelectedItem = group;
						return;
					}
				}
			}
		}

		private void UpdateControlsState(string groupName = null)
		{
			btnAddConnectionString.Enabled    = this.cmbConnection.SelectedItem        != null;
			btnRemoveConnectionString.Enabled = this.lstConnectionStrings.SelectedItem != null;

			List<InstanceInfo> connections =
				this.lstConnectionStrings.Items.OfType<InstanceInfo>().ToList();

			string name = !string.IsNullOrEmpty(groupName)
				? groupName
				: string.Join(",", connections.Select(x => x.Name));

			this.txtGroupName.Text    = name.TrimEnd(',');
			this.txtGroupName.Enabled = connections.Count > 0;
		}

		private void SetLocale()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.InterfaceLanguage);
		}

		private SettingsInfo Settings
		{
			get { return this._model.Settings; }
		}

		private ConnectionGroupInfo PrepareGroupResult()
		{
			List<InstanceInfo> instances = this.lstConnectionStrings.Items
				.OfType<InstanceInfo>().ToList();

			ConnectionGroupInfo group = new ConnectionGroupInfo
			{
				Connections        = instances,
				IsExternal         = IsExternalTemplateFile,
				TemplateDir        = Path.GetDirectoryName(SelectedTemplateFile),
				TemplateFileName   = Path.GetFileName(SelectedTemplateFile),
				Name               = ConnectionGroupName,
				IsDirectConnection = true
			};

			foreach (InstanceInfo instanceInfo in instances)
			{
				instanceInfo.ConnectionGroup = group;
			}

			UpdateGroupInstances(group);

			return group;
		}

		private ConnectionType SelectedConnectionType
		{
			get
			{
				BindingWrapper<ConnectionType> typeWrapper =
					this.cmbDataBaseType.SelectedItem as BindingWrapper<ConnectionType>;

				if (typeWrapper != null)
				{
					ConnectionType selectedConnection = typeWrapper.Item;
					return selectedConnection;
				}

				return null;
			}
		}

		private void UpdateGroupInstances(ConnectionGroupInfo group)
		{
			ConnectionType cnnType = SelectedConnectionType;
			if (cnnType != null)
			{
				this._connectionsManager.UpdateGroupInstances(group, cnnType.Id);
			}
		}

		private Dictionary<string, ConnectionInfo> ReadLastConnections(List<ConnectionType> connectionTypes)
		{
			Dictionary<string, ConnectionInfo> lastConnections = new Dictionary<string, ConnectionInfo>();
			string                             machineName     = Environment.MachineName;

			foreach (ConnectionType connectionType in connectionTypes)
			{
				string cnnType = connectionType.Id;

				LastConnectionProtocolRow row = this._storage.LastConnectionProtocolTable
					.GetLastConnection(machineName, cnnType);

				if (row != null)
				{
					ConnectionGroupInfo group = this._connectionsManager.GetGroup(row.GroupId);
					TemplateRow templateRow   = this._storage.TemplateDirectory.GetTemplate(row.TemplateId);

					if (group != null && templateRow != null)
					{
						ConnectionInfo connectionInfo = new ConnectionInfo
						{
							TemplateFile   = templateRow.Id,
							TemplateDir    = templateRow.Directory,
							TemplateId     = templateRow.Name,
							ConnectionId   = row.Identity,
							GroupName      = group.Name,
							ConnectionType = cnnType
						};

						lastConnections.Add(cnnType, connectionInfo);
					}
				}
			}

			return lastConnections;
		}

		private ConnectionInfo GetLastConnectionForCurrentProtocol()
		{
			ConnectionInfo connectionInfo = null;
			ConnectionType currentCnnType = SelectedConnectionType;
			if (currentCnnType != null)
			{
				this._lastConnections.TryGetValue(currentCnnType.Id, out connectionInfo);
			}

			return connectionInfo;
		}

		private void SaveLastConnection(
			long groupId,
			long templateId
		)
		{
			ConnectionType cnnType = SelectedConnectionType;

			if (cnnType != null)
			{
				string machineName = Environment.MachineName;

				LastConnectionProtocolRow lastConnectionProt = new LastConnectionProtocolRow
				{
					GroupId     = groupId,
					TemplateId  = templateId,
					MachineName = machineName,
					DbType      = cnnType.Id
				};

				long? lastConnectionId = this._storage.LastConnectionProtocolTable
					.SaveLastConnection(lastConnectionProt);

				if (lastConnectionId != null)
				{
					LastConnectionRow lastConnectionRow = new LastConnectionRow
					{
						MachineName              = machineName,
						LastConnectionProtocolId = lastConnectionId.Value
					};

					this._storage.LastConnectionTable
						.SaveLastConnection(lastConnectionRow);
				}
			}
		}
		#endregion
	}
}
