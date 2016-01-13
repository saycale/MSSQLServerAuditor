using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.Model.Connections.Query;
using MSSQLServerAuditor.Model.Loaders;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	public partial class InternalSqliteConnectionDialog : LocalizableForm, IConnectionStringDialog
	{
		private static readonly ILog        log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		protected readonly List<Template>   _templates;
		private readonly MsSqlAuditorModel  _model;
		private readonly CurrentStorage     _storage;
		private readonly ConnectionsManager _connectionsManager;
		private readonly LoginManager       _loginManager;
		private readonly TemplateManager    _templateManager;

		public InternalSqliteConnectionDialog(MsSqlAuditorModel model)
		{
			this._model              = model;
			this._storage            = model.DefaultVaultProcessor.CurrentStorage;
			this._connectionsManager = new ConnectionsManager(model);
			this._loginManager       = new LoginManager(this._storage);
			this._templateManager    = new TemplateManager(this._storage);
			this._templates          = TemplateNodesLoader.GetTemplates();

			InitializeComponent();

			List<BindingWrapper<ConnectionType>> connectionTypes = this._model.ConnectionTypes;

			this.dataTypeBindingSource.DataSource = connectionTypes;
			this.dataTypeBindingSource.DataMember = "Item";
			this.cmbDataType.DataSource           = dataTypeBindingSource.DataSource;

			this.cmbDataType.SelectionChangeCommitted += this.cmbDataType_SelectedIndexChanged;

			this.cmbConnectionGroup.SelectionChangeCommitted += this.cmbConnectionGroup_SelectedIndexChanged;
			this.cmbServerInstance.SelectionChangeCommitted  += this.cmbServerInstance_SelectedIndexChanged;
			this.cmbTemplate.SelectionChangeCommitted        += this.cmbTemplate_SelectedIndexChanged;
			this.cmbLogin.SelectionChangeCommitted           += this.cmbLogin_SelectedIndexChanged;

			if (connectionTypes.Count > 0)
			{
				UpdateConnectionGroupList();
			}

			UpdateButtonsState();
		}

		public string ConnectionString
		{
			get
			{
				if (SelectedConnectionType != null &&
					SelectedGroup          != null &&
					SelectedInstance       != null &&
					SelectedTemplate       != null &&
					SelectedLogin          != null
				)
				{
					SqliteConnectionParameters parameters = new SqliteConnectionParameters
					{
						Group      = SelectedGroup.Name    ?? string.Empty,
						Connection = SelectedInstance.Name ?? string.Empty,
						Login      = SelectedLogin.Login   ?? string.Empty,
						Template   = SelectedTemplate.Name ?? string.Empty
					};

					log.InfoFormat("ConnectionString:'{0}'",
						parameters.ToConnectionString()
					);

					return parameters.ToConnectionString();
				}

				return string.Empty;
			}
		}

		public string ConnectionName
		{
			get { return this.ConnectionString; }
		}

		public bool IsOdbc
		{
			get { return false; }
		}

		private void cmbDataType_SelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			UpdateConnectionGroupList();
			UpdateButtonsState();
		}

		private void cmbConnectionGroup_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateServerInstanceList();
			UpdateButtonsState();
		}

		private void cmbServerInstance_SelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			UpdateTemplateList();
			UpdateButtonsState();
		}

		private void cmbTemplate_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateLoginList();
			UpdateButtonsState();
		}

		private void cmbLogin_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateButtonsState();
		}

		private void UpdateConnectionGroupList()
		{
			BindingWrapper<ConnectionType> connectionWrapper = cmbDataType.SelectedItem as BindingWrapper<ConnectionType>;

			if (connectionWrapper != null)
			{
				cmbConnectionGroup.Enabled = false;
				ClearConnectionGroupList();

				string protocol = connectionWrapper.Item.Id;
				List<ConnectionGroupInfo> groups = this._connectionsManager.GetAllGroups(protocol)
					.Where(groupInfo =>
					{
						List<InstanceInfo> instances = groupInfo.Connections;

						if (!instances.IsNullOrEmpty() && instances.TrueForAll(i => !i.IsDynamicConnection))
						{
							return true;
						}

						return false;
					}).ToList();

				foreach (ConnectionGroupInfo groupInfo in groups)
				{
					BindingWrapper<ConnectionGroupInfo> groupWrapper = new BindingWrapper<ConnectionGroupInfo>(
						groupInfo, g => g.Name);

					cmbConnectionGroup.Items.Add(groupWrapper);
				}

				cmbConnectionGroup.Enabled = true;

				if (groups.Count > 0)
				{
					cmbConnectionGroup.SelectedIndex = 0;

					UpdateServerInstanceList();
				}
			}
		}

		private void UpdateServerInstanceList()
		{
			cmbServerInstance.Enabled = false;

			ClearServerInstanceList();

			ConnectionGroupInfo group = SelectedGroup;

			if (group != null)
			{
				foreach (InstanceInfo instanceInfo in group.Connections)
				{
					if (!string.IsNullOrEmpty(instanceInfo.Name))
					{
						BindingWrapper<InstanceInfo> instanceWrapper = new BindingWrapper<InstanceInfo>(
							instanceInfo, i => i.Name
						);

						cmbServerInstance.Items.Add(instanceWrapper);
					}
				}

				if (cmbServerInstance.Items.Count > 0)
				{
					cmbServerInstance.SelectedIndex = 0;

					UpdateTemplateList();
				}
			}

			cmbServerInstance.Enabled = true;
		}

		private void UpdateTemplateList()
		{
			cmbTemplate.Enabled = false;
			ClearTemplateList();

			ConnectionGroupInfo group    = SelectedGroup;
			InstanceInfo        instance = SelectedInstance;

			if (group != null && instance != null)
			{
				List<TemplateRow> templates = this._templateManager
					.GetTemplates(group.Name, instance.Name, instance.DbType)
					.Where(row => !Path.IsPathRooted(row.Directory))
					.ToList();

				foreach (TemplateRow templateRow in templates)
				{
					string displayName = templateRow.Id;

					Template template = this._templates.Find(t => t.Id == templateRow.Name);

					if (template != null)
					{
						displayName = string.Format("{0} ({1})", template.Display, templateRow.Name);
					}

					BindingWrapper<TemplateRow> templateWrapper = new BindingWrapper<TemplateRow>(
						templateRow, row => displayName
					);

					cmbTemplate.Items.Add(templateWrapper);
				}

				if (cmbTemplate.Items.Count > 0)
				{
					cmbTemplate.SelectedIndex = 0;

					UpdateLoginList();
				}
			}

			cmbTemplate.Enabled = true;
		}

		private void UpdateLoginList()
		{
			cmbLogin.Enabled = false;

			ClearLoginList();

			ConnectionGroupInfo group    = SelectedGroup;
			InstanceInfo        instance = SelectedInstance;
			TemplateRow         template = SelectedTemplate;

			if (group != null && instance != null && template != null)
			{
				List<LoginRow> logins = this._loginManager.GetLogins(
					template.Name,
					group.Name,
					instance.Name,
					instance.DbType
				);

				if (logins.Count > 0)
				{
					foreach (LoginRow loginRow in logins)
					{
						BindingWrapper<LoginRow> loginWrapper = new BindingWrapper<LoginRow>(
							loginRow,
							DisplayLogin
						);

						cmbLogin.Items.Add(loginWrapper);
					}

					cmbLogin.SelectedIndex = 0;
				}
			}

			cmbLogin.Enabled = true;
		}

		private static string DisplayLogin(LoginRow row)
		{
			if (row.IsWinAuth)
			{
				return "[WinAuth]";
			}

			string login = string.IsNullOrEmpty(row.Login)
				? "[No login]"
				: row.Login;

			if (row.Password.Length > 0)
			{
				return string.Format(
					"{0} (pwd: {1}{2})",
					login,
					row.Password[0],
					new string('*', row.Password.Length - 1)
				);
			}

			return login;
		}

		private string SelectedConnectionType
		{
			get
			{
				BindingWrapper<ConnectionType> typeWrapper = cmbDataType.SelectedItem
					as BindingWrapper<ConnectionType>;

				return typeWrapper != null ? typeWrapper.Item.Id : null;
			}
		}

		private ConnectionGroupInfo SelectedGroup
		{
			get
			{
				BindingWrapper<ConnectionGroupInfo> groupWrapper = cmbConnectionGroup.SelectedItem
					as BindingWrapper<ConnectionGroupInfo>;

				return groupWrapper != null ? groupWrapper.Item : null;
			}
		}

		private InstanceInfo SelectedInstance
		{
			get
			{
				BindingWrapper<InstanceInfo> instanceWrapper = cmbServerInstance.SelectedItem
					as BindingWrapper<InstanceInfo>;

				return instanceWrapper != null ? instanceWrapper.Item : null;
			}
		}

		private TemplateRow SelectedTemplate
		{
			get
			{
				BindingWrapper<TemplateRow> templateWrapper = cmbTemplate.SelectedItem
					as BindingWrapper<TemplateRow>;
				return templateWrapper != null ? templateWrapper.Item : null;
			}
		}

		private LoginRow SelectedLogin
		{
			get
			{
				BindingWrapper<LoginRow> loginWrapper = cmbLogin.SelectedItem as BindingWrapper<LoginRow>;

				return loginWrapper != null ? loginWrapper.Item : null;
			}
		}

		private void ClearConnectionGroupList()
		{
			this.cmbConnectionGroup.Items.Clear();

			ClearServerInstanceList();
		}

		private void ClearServerInstanceList()
		{
			this.cmbServerInstance.Items.Clear();
			ClearTemplateList();
		}

		private void ClearTemplateList()
		{
			this.cmbTemplate.Items.Clear();

			ClearLoginList();
		}

		private void ClearLoginList()
		{
			this.cmbLogin.Items.Clear();
		}

		private void UpdateButtonsState()
		{
			bool instanceSelected =
				this.cmbServerInstance.SelectedItem  != null &&
				this.cmbConnectionGroup.SelectedItem != null &&
				this.cmbDataType.SelectedItem        != null &&
				this.cmbTemplate.SelectedItem        != null &&
				this.cmbLogin.SelectedItem           != null;

			this.btnOk.Enabled = instanceSelected;
		}
	}
}
