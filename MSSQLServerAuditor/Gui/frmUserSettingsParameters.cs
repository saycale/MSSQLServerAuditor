using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MSSQLServerAuditor.Gui.LayoutSettings;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.UserSettingsParameters;

namespace MSSQLServerAuditor.Gui
{
	public partial class frmUserSettingsParameters : LocalizableForm
	{
		private readonly IQueryParameters                                         _model;
		private readonly ILayoutSettingsProvider<frmUserSettingsParametersLayout> _layoutSettingsProvider;

		public frmUserSettingsParameters()
		{
			InitializeComponent();

			this._model                  = null;
			this._layoutSettingsProvider = new LayoutSettingsProvider<frmUserSettingsParametersLayout>(this);
		}

		internal frmUserSettingsParameters(IQueryParameters model) : this()
		{
			dgvParameters.AutoGenerateColumns = false;

			this._model                              = model;
			iQueryParametersBindingSource.DataSource = this._model;

			if (this._model != null)
			{
				this.Text = String.Format("{0} : '{1}'",
					 GetLocalizedText("captionText")
					,this._model.Name != null ? this._model.Name : "?"
				);
			}
			else
			{
				this.Text = String.Format("{0}",
					GetLocalizedText("captionText")
				);
			}
		}

		private void frmUserSettingsParameters_Load(object sender, EventArgs e)
		{
			if (this._model != null)
			{
				this._model.Init();

				splitContainer1.Panel1Collapsed = (listBox1.Items.Count <= 1 && this._model.IsHideTabs);
				TypeColumn.Visible              = !this._model.IsHideTypeColumn;
			}

			if (this._layoutSettingsProvider != null)
			{
				if (this._layoutSettingsProvider.LoadSettings())
				{
					this._layoutSettingsProvider.ApplySettings();
				}
				else
				{
					this._layoutSettingsProvider.UpdateSettings();
					this._layoutSettingsProvider.SaveSettings();
				}

				this._layoutSettingsProvider.AttachToChangeLayout();
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this._model != null)
			{
				this._model.ApplyChanged();
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._model != null)
			{
				this._model.SelectedQuery = (QueryKey)listBox1.SelectedItem;

				splitContainer1.Panel1Collapsed = (listBox1.Items.Count <= 1 && this._model.IsHideTabs);
				TypeColumn.Visible              = !this._model.IsHideTypeColumn;
			}
		}

		private void frmUserSettingsParameters_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this._layoutSettingsProvider != null)
			{
				this._layoutSettingsProvider.DettachToChangeLayout();
				this._layoutSettingsProvider.Dispose();
			}
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (this._model != null)
			{
				this._model.RemoveSelectedParameters();
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (this._model != null)
			{
				this._model.AddParameter();
			}
		}

		private void dgvParameters_SelectionChanged(object sender, EventArgs e)
		{
			var selectedParameters = (from object cell in dgvParameters.SelectedCells select cell as ParameterInfoLocalizable).ToList();

			if (this._model != null)
			{
				this._model.SetSelectedParametes(selectedParameters);
			}
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			if (this._model != null)
			{
				this._model.ApplyChanged();
				this._model.TryUpdate();
			}
		}
	}
}
