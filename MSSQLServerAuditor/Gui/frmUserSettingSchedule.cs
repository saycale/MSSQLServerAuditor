using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	public partial class frmUserSettingSchedule : LocalizableForm
	{
		private ConcreteTemplateNodeDefinition _nodeDefinition;

		public frmUserSettingSchedule(ConcreteTemplateNodeDefinition nodeDefinition)
		{
			if (nodeDefinition == null)
			{
				throw new ArgumentNullException("nodeDefinition");
			}

			InitializeComponent();

			string fmtStr = GetLocalizedText("captionText");

			if (!string.IsNullOrEmpty(fmtStr))
			{
				this.Text = String.Format(fmtStr, CurrentAssembly.Title);
			}

			this.NodeDefinition = nodeDefinition;
		}

		public ConcreteTemplateNodeDefinition NodeDefinition
		{
			get { return this._nodeDefinition; }

			set
			{
				string strTabPageText = string.Empty;

				this._nodeDefinition = value;

				this.tabControl1.TabPages.Clear();

				if (this._nodeDefinition.TemplateNode != null)
				{
					List<TemplateNodeUpdateJob> jobs = this._nodeDefinition.TemplateNode.GetRefreshJob(true);

					foreach (TemplateNodeUpdateJob job in jobs)
					{
						UserSettingScheduleControl usc = new UserSettingScheduleControl(job)
						{
							Name = "UserSettingScheduleControl1"
						};

						if (string.IsNullOrEmpty(job.Settings.Name))
						{
							strTabPageText = job.Settings.Id;
						}
						else
						{
							strTabPageText = job.Settings.Name;
						}

						TabPage tabPage = new TabPage(strTabPageText);

						tabPage.Controls.Add(usc);

						this.tabControl1.TabPages.Add(tabPage);
					}
				}
				else
				{
					this.tabControl1.TabPages.Add(this._nodeDefinition.TemplateNode.Title);

					UserSettingScheduleControl usc = new UserSettingScheduleControl(null);

					this.tabControl1.TabPages[0].Controls.Add(usc);
				}
			}
		}

		public void AddNewSchedule()
		{
			frmNewScheduleId dlg = new frmNewScheduleId(this.GetUpdateJobs());

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				TemplateNodeUpdateJob newJob = new TemplateNodeUpdateJob();

				newJob.Settings = new ScheduleSettings();

				newJob.Settings.Id        = dlg.Value;
				newJob.Settings.StartDate = DateTime.Now;
				newJob.Settings.Enabled   = false;

				UserSettingScheduleControl usc = new UserSettingScheduleControl(newJob)
				{
					Name = "UserSettingScheduleControl1"
				};

				TabPage tabPage = new TabPage("New schedule");
				tabPage.Controls.Add(usc);

				this.tabControl1.TabPages.Add(tabPage);

				this.tabControl1.SelectedTab = tabPage;
			}
		}

		public List<TemplateNodeUpdateJob> GetUpdateJobs()
		{
			List<TemplateNodeUpdateJob> updateJobs = new List<TemplateNodeUpdateJob>();

			if (this.NodeDefinition != null && this.NodeDefinition.TemplateNode != null)
			{
				foreach (TabPage page in tabControl1.TabPages)
				{
					UserSettingScheduleControl usc = page.Controls["UserSettingScheduleControl1"] as UserSettingScheduleControl;

					if (usc.job == null || usc.job.IsEmpty())
					{
						continue;
					}

					TemplateNodeUpdateJob newJob = usc.getJob();

					updateJobs.Add(newJob);
				}
			}

			return updateJobs;
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			AddNewSchedule();
		}
	}
}
