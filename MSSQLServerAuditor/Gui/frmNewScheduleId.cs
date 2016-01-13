using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Scheduling;

namespace MSSQLServerAuditor.Gui
{
	public partial class frmNewScheduleId : LocalizableForm
	{
		private List<TemplateNodeUpdateJob> _jobList;

		public frmNewScheduleId(List<TemplateNodeUpdateJob> jobList)
		{
			InitializeComponent();

			this._jobList = jobList;
		}

		private bool validate()
		{
			errorProvider1.SetError(tbScheduleId, string.Empty);

			if (this.isSheduleIdExists(this.tbScheduleId.Text.Trim()))
			{
				string errMes = GetLocalizedText("errThisIdAlreadyExists");

				errorProvider1.SetError(tbScheduleId, errMes);

				return false;
			}

			return true;
		}

		public bool isSheduleIdExists(string strJobScheduleId)
		{
			if (this._jobList != null)
			{
				foreach (TemplateNodeUpdateJob job in this._jobList)
				{
					if (job.Settings.Id.CompareTo(strJobScheduleId) == 0)
					{
						return true;
					}
				}
			}

			return false;
		}

		public string Value
		{
			get { return tbScheduleId.Text.Trim(); }
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(tbScheduleId.Text.Trim()) && this.validate())
			{
				this.DialogResult = DialogResult.OK;

				this.Close();
			}
		}
	}
}
