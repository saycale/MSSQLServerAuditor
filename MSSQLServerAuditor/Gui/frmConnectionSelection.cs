using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	/// Form for select connections
	/// </summary>
	public partial class frmConnectionSelection : LocalizableForm
	{
		private const string                   preprocessor              = "frmConnectionSelection";
		private const string                   _frmId                    = "";
		private const string                   _templateConnectionPrefix = "dgvCurrentConnection";
		private const string                   listTemplatsPrefix        = "listConnections";
		private bool                           _isLoaded;
		private frmConnectionSelectionSettings _settings;

		#region Initialize/Destruct
		/// <summary>
		/// Default constructor
		/// </summary>
		public frmConnectionSelection()
		{
			this._settings = null;
			this._isLoaded = false;

			InitializeComponent();

			dgvCurrentConnection.DefaultCellStyle.SelectionBackColor = dgvCurrentConnection.DefaultCellStyle.BackColor;
			dgvCurrentConnection.DefaultCellStyle.SelectionForeColor = dgvCurrentConnection.DefaultCellStyle.ForeColor;
		}

		/// <summary>
		/// Form loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmConnectionSelection_Load(object sender, EventArgs e)
		{
			if (!LoadSettings(this._settings))
			{
				SaveSettings();
			}

			this._isLoaded = true;
		}
		#endregion

		/// <summary>
		/// Sets available connection groups
		/// </summary>
		/// <param name="infos"></param>
		public void SetConnectionGroupInfos(IEnumerable<ConnectionGroupInfo> infos)
		{
			connectionGroupInfoBindingSource.DataSource = infos;
		}

		/// <summary>
		/// Get selected connection group
		/// </summary>
		public ConnectionGroupInfo SelectedConnectionGroupInfo
		{
			get { return listConnections.CurrentRow.DataBoundItem as ConnectionGroupInfo; }
		}

		void ForceCloseOk()
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void listConnections_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				e.Handled = true;
				ForceCloseOk();
			}
		}

		private void frmConnectionSelection_Shown(object sender, EventArgs e)
		{
			listConnections.Focus();
		}

		private void listConnections_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			ForceCloseOk();
		}

		private void listConnections_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			var info = SelectedConnectionGroupInfo;
			connectionInfoBindingSource.DataSource = info.Connections;
			btnOk.Enabled = info != null;

			foreach (DataGridViewRow currentSelectedRow in dgvCurrentConnection.SelectedRows)
			{
				var instance = currentSelectedRow.DataBoundItem as InstanceInfo;

				if (instance != null)
				{
					var validate = instance.ValidateLicense(true,false);

					if (validate.Problems != null &&
						((validate.Problems.Any(x => x == LicenseProblemType.BuildExpiryDateNotValid)) ||
						 (validate.Problems.Any(x => x == LicenseProblemType.Expired))))
					{
						btnOk.Enabled = false;
					}
				}
			}
		}

		private void DgvCurrentConnectionCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			DataGridView dgv      = sender as DataGridView;
			InstanceInfo instance = dgv.Rows[e.RowIndex].DataBoundItem as InstanceInfo;

			if (instance != null)
			{
				if (instance.ValidateLicense(false,false).IsCorrect)
				{
					e.CellStyle.BackColor = Color.FromKnownColor(KnownColor.Control);
				}
				else
				{
					e.CellStyle.BackColor = Color.IndianRed;
				}
			}

			if (dgv.CurrentRow != null && dgv.CurrentRow.Index == e.RowIndex)
			{
				e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
			}
		}

		private void frmConnectionSelection_Resize(object sender, EventArgs e)
		{
			listConnections.Height = panel1.Height - 62;

			if (this._isLoaded && (this._settings == null || this._settings.Size.Height != this.Size.Height || this._settings.Size.Width != this.Size.Width))
			{
				SaveSettings();
			}
		}

		private void dgvCurrentConnection_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if (this._isLoaded)
			{
				SaveNewColumnChange(_templateConnectionPrefix, e.Column);
			}
		}

		private void listConnections_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if (this._isLoaded)
			{
				SaveNewColumnChange(listTemplatsPrefix, e.Column);
			}
		}

		#region Settings
		/// <summary>
		/// Load GUI settings from XML
		/// </summary>
		/// <param name="defValue"></param>
		/// <returns>true - settings loaded, false - no</returns>
		private bool LoadSettings(frmConnectionSelectionSettings defValue = null)
		{
			bool result = false;

			this._settings = Program.Model.LayoutSettings.GetExtendedSettings<frmConnectionSelectionSettings>(
				_frmId,
				preprocessor,
				Program.Model.Settings.InterfaceLanguage
			);

			result = this._settings != null;

			if (result)
			{
				ApplySettings();
			}

			if (this._settings == null && defValue != null)
			{
				this._settings = defValue;
			}

			if (this._settings != null)
			{
				return result;
			}

			UpdateSettings();

			return result;
		}

		/// <summary>
		/// Apply settings from object _settings to GUI
		/// </summary>
		private void ApplySettings()
		{
			if(this._settings == null)
			{
				return;
			}

			this.Location = this._settings.Location;
			this.Size     = this._settings.Size;

			if(this._settings.ColumnSettings.IsNullOrEmpty())
			{
				return;
			}

			foreach (DataGridViewColumn column in dgvCurrentConnection.Columns)
			{
				if (column == null)
				{
					continue;
				}

				var id = String.Format("{0}{1}", _templateConnectionPrefix, column.Name);

				if (this._settings.ColumnSettings.All(el => el.Id != id))
				{
					continue;
				}

				column.Width   = this._settings.GetColumnSettings(id).Width;
				column.Visible = this._settings.GetColumnSettings(id).Visible;
			}

			foreach (DataGridViewColumn column in listConnections.Columns)
			{
				if (column == null)
				{
					continue;
				}

				var id = String.Format("{0}{1}", listTemplatsPrefix, column.Name);

				if (this._settings.ColumnSettings.All(el => el.Id != id))
				{
					continue;
				}

				column.Width   = this._settings.GetColumnSettings(id).Width;
				column.Visible = this._settings.GetColumnSettings(id).Visible;
			}
		}

		/// <summary>
		/// Update _settings object by real GUI settings
		/// </summary>
		private void UpdateSettings()
		{
			if (this._settings == null)
			{
				return;
			}

			this._settings.Size     = this.Size;
			this._settings.Location = this.Location;

			foreach (DataGridViewColumn column in dgvCurrentConnection.Columns)
			{
				if (column == null)
				{
					continue;
				}

				var id = String.Format("{0}{1}", _templateConnectionPrefix, column.Name);

				this._settings.SetColumnSettings(id, column.Visible, column.Width);
			}

			foreach (DataGridViewColumn column in listConnections.Columns)
			{
				if (column == null)
				{
					continue;
				}

				var id = String.Format("{0}{1}", listTemplatsPrefix, column.Name);

				this._settings.SetColumnSettings(id, column.Visible, column.Width);
			}
		}

		/// <summary>
		/// Save GUI settings from XML
		/// </summary>
		private void SaveSettings()
		{
			if (this._settings == null)
			{
				this._settings = new frmConnectionSelectionSettings();
			}

			UpdateSettings();

			Program.Model.LayoutSettings.SetExtendedSettings<frmConnectionSelectionSettings>(
				_frmId,
				preprocessor,
				Program.Model.Settings.InterfaceLanguage,
				this._settings
			);
		}

		private void SaveNewColumnChange(string prefix, DataGridViewColumn column)
		{
			var id = String.Format("{0}{1}", prefix, column.Name);

			if (this._settings == null)
			{
				this._settings = new frmConnectionSelectionSettings();
			}

			this._settings.SetColumnSettings(id, column.Visible, column.Width);

			SaveSettings();
		}
		#endregion
	}
}
