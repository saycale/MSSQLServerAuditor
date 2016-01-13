using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Loaders;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	public partial class DirectConnectionDialog : LocalizableForm
	{
		protected readonly Lazy<List<Template>> _templates = new Lazy<List<Template>>(TemplateNodesLoader.GetTemplates);

		protected DirectConnectionDialog()
		{
			InitializeComponent();
		}

		protected void RunSelectConnectionDialog(MsSqlAuditorModel model)
		{
			BindingWrapper<ConnectionType> connectionWrapper = cmbDataBaseType.SelectedItem as BindingWrapper<ConnectionType>;

			if (connectionWrapper != null)
			{
				ConnectionType connectionType = connectionWrapper.Item;
				QuerySource    querySource;

				if (!Enum.TryParse(connectionType.Id, true, out querySource))
				{
					return;
				}

				IConnectionStringDialog dialog = ConnectionStringDialogFactory.CreateDialog(
					querySource,
					model
				);

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					DbConnectionStringBuilder builder = GetConnectionStringBuilder(
						querySource,
						dialog.ConnectionString,
						dialog.IsOdbc
					);

					InstanceInfo newInstance = InstanceInfoResolver.ResolveInstance(
						builder,
						dialog.IsOdbc,
						querySource
					);

					SelectInstance(newInstance);
				}
			}
		}

		protected void LoadConnectionGroups(
			ConnectionType     connectionType,
			ConnectionsManager connectionsManager)
		{
			List<ConnectionGroupInfo> groups = connectionsManager
				.GetDirectGroups(connectionType.Id);

			foreach (ConnectionGroupInfo groupInfo in groups)
			{
				List<InstanceInfo> instances     = groupInfo.Connections;
				InstanceInfo       firstInstance = instances.FirstOrDefault();

				if (firstInstance != null)
				{
					if (instances.Count == 1 && firstInstance.GetConnectionString().Equals(groupInfo.Name))
					{
						continue;
					}

					if (!instances.TrueForAll(i => i.IsDynamicConnection))
					{
						this.cmbConnection.Items.Add(groupInfo);
					}
				}
			}
		}

		protected void LoadConnectionInstances(
			ConnectionType     connectionType,
			ConnectionsManager connectionsManager
		)
		{
			// get distinct instances from sqlite database
			List<InstanceInfo> instances = connectionsManager
				.GetDistinctInstances(connectionType.Id);

			foreach (InstanceInfo instance in instances)
			{
				this.cmbConnection.Items.Add(instance);
			}
		}

		private void SelectInstance(InstanceInfo instanceInfo)
		{
			IEnumerable<InstanceInfo> instances =
				this.cmbConnection.Items.OfType<InstanceInfo>();

			InstanceInfo firstConnection = instances.FirstOrDefault(
				x => x.GetConnectionString() == instanceInfo.GetConnectionString() && x.IsODBC == instanceInfo.IsODBC);

			if (firstConnection == null)
			{
				this.cmbConnection.Items.Add(instanceInfo);
				this.cmbConnection.SelectedItem = instanceInfo;
			}
			else
			{
				this.cmbConnection.SelectedItem = firstConnection;
			}
		}

		protected static DbConnectionStringBuilder GetConnectionStringBuilder(
			QuerySource source,
			string      connectionString,
			bool        isOdbc
		)
		{
			if (isOdbc)
			{
				return new OdbcConnectionStringBuilder(connectionString);
			}

			DbConnectionStringBuilder builder;

			switch (source)
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

			builder.ConnectionString = connectionString;

			return builder;
		}

		#region Validation

		protected bool ValidateTemplate()
		{
			bool   result = true;
			string path   = null;

			if (this.optSelectExistTemplate.Checked && this.cmbTemplate.SelectedItem != null)
			{
				Template template = this.cmbTemplate.SelectedItem as Template;

				if (template != null)
				{
					path = template.Id;
				}
				else
				{
					result = false;
				}
			}
			else if (this.optOpenTemplateFromFile.Checked)
			{
				path = this.cmbPathToFile.Text;
			}

			if (string.IsNullOrWhiteSpace(path))
			{
				if (this.optSelectExistTemplate.Checked)
				{
					this.errorProvider.SetError(this.cmbTemplate, this.GetLocalizedText("SelectTemplateError"));
				}
				else if (this.optOpenTemplateFromFile.Checked && String.IsNullOrWhiteSpace(this.cmbPathToFile.Text))
				{
					this.errorProvider.SetError(this.btnOpenTemplate, this.GetLocalizedText("SelectTemplateFileError"));
				}

				result = false;
			}
			else
			{
				if (this.optOpenTemplateFromFile.Checked && !File.Exists(path))
				{
					this.errorProvider.SetError(this.btnOpenTemplate, this.GetLocalizedText("SelectTemplateFileNotExistError"));
					result = false;
				}
				else
				{
					this.errorProvider.SetError(this.cmbTemplate, string.Empty);
				}
			}

			return result;
		}

		protected bool ValidateConnections()
		{
			bool result = true;

			foreach (InstanceInfo instance in this.lstConnectionStrings.Items.OfType<InstanceInfo>())
			{
				if (instance == null || string.IsNullOrEmpty(instance.GetConnectionString()))
				{
					this.errorProvider.SetError(this.lstConnectionStrings, this.GetLocalizedText("SelectServerError"));

					result = false;
				}
				else
				{
					this.errorProvider.SetError(this.lstConnectionStrings, string.Empty);
				}
			}

			if (!lstConnectionStrings.Items.OfType<InstanceInfo>().Any())
			{
				this.errorProvider.SetError(this.lstConnectionStrings, this.GetLocalizedText("SelectServerError"));

				result = false;
			}

			if (this.cmbDataBaseType.SelectedItem == null)
			{
				this.errorProvider.SetError(this.cmbDataBaseType, this.GetLocalizedText("SelectDataBaseError"));

				result = false;
			}
			else
			{
				this.errorProvider.SetError(this.cmbDataBaseType, string.Empty);
			}

			return result;
		}

		protected bool ValidateForm()
		{
			bool result = ValidateTemplate();

			if (!ValidateConnections())
			{
				result = false;
			}

			return result;
		}

		protected bool ValidateAddConnectionForm()
		{
			InstanceInfo        instance = this.cmbConnection.SelectedItem as InstanceInfo;
			ConnectionGroupInfo group    = this.cmbConnection.SelectedItem as ConnectionGroupInfo;

			if ((instance == null || string.IsNullOrEmpty(instance.GetConnectionString())) &&
				(group    == null || group.Connections.IsNullOrEmpty())
			)
			{
				this.errorProvider.SetError(this.cmbConnection, this.GetLocalizedText("SelectServerError"));

				return false;
			}

			this.errorProvider.SetError(this.cmbConnection, string.Empty);

			return true;
		}

		#endregion
	}
}
