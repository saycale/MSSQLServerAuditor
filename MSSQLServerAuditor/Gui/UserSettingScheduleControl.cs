using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using MSSQLServerAuditor.Entities;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	public partial class UserSettingScheduleControl : UserControl
	{
		public TemplateNodeUpdateJob job;

		public UserSettingScheduleControl(TemplateNodeUpdateJob job)
		{
			InitializeComponent();

			//this.lbFrequencyInterval.Text = GetLocalizedText("frequencyInterval");

			this.job                         = job;
			this.lblEnabled.Text             = GetLocalizedText("enabled");
			this.lbFrequencyType.Text        = GetLocalizedText("frequencyType");
			this.lbStartDateTime.Text        = GetLocalizedText("startDateTime");
			this.lbEndDateTime.Text          = GetLocalizedText("endDateTime");
			this.lbName.Text                 = GetLocalizedText("lbName");
			this.lbScheduleType.Text         = GetLocalizedText("lbScheduleType");
			this.lbOneTimeheader.Text        = GetLocalizedText("lbOneTimeheader");
			this.lbDateOnce.Text             = GetLocalizedText("lbDateOnce");
			this.lbTimeOnce.Text             = GetLocalizedText("lbTimeOnce");
			this.lbOccurs.Text               = GetLocalizedText("lbOccurs");
			this.label1.Text                 = GetLocalizedText("label1");
			this.lbDailyFreequency.Text      = GetLocalizedText("lbDailyFreequency");
			this.rbDailyFreqOccursOnce.Text  = GetLocalizedText("rbOccyrsType");
			this.rbDailyFreqOccursEvery.Text = GetLocalizedText("rbOccyrsTypeEvery");
			this.label4.Text                 = GetLocalizedText("label4");
			this.lbStartDate.Text            = GetLocalizedText("lbStartDate");
			this.rbEndDate.Text              = GetLocalizedText("radioButton1");
			this.rbNoEndDate.Text            = GetLocalizedText("radioButton2");
			this.cbEmailNotification.Text    = GetLocalizedText("cbEmailNotification");

			dtDateStartOneTime.Format =
				dtDateDurationStart.Format =
				dtTimeStartOneTime.Format =
				dtTimeOccursOnly.Format =
				dtDaylyEveryStart.Format =
				dtDaylyEveryEnd.Format =
				dtDateDurationEnd.Format =
				DateTimePickerFormat.Custom;

			dtDateStartOneTime.CustomFormat = dtDateDurationStart.CustomFormat = dtDateDurationEnd.CustomFormat = "dd/MM/yyyy";

			dtTimeStartOneTime.CustomFormat =
				dtTimeOccursOnly.CustomFormat =
					dtDaylyEveryStart.CustomFormat = dtDaylyEveryEnd.CustomFormat = "HH:mm:ss";

			dtDateStartOneTime.MaxDate =
				dtDateDurationStart.MaxDate =
					dtTimeStartOneTime.MaxDate =
						dtTimeOccursOnly.MaxDate =
							dtDaylyEveryStart.MaxDate =
								dtDaylyEveryEnd.MaxDate =
									dtDateDurationEnd.MaxDate =
										DateTime.MaxValue;

			dtDateStartOneTime.MinDate =
				dtDateDurationStart.MinDate =
					dtTimeStartOneTime.MinDate =
						dtTimeOccursOnly.MinDate =
							dtDaylyEveryStart.MinDate =
								dtDaylyEveryEnd.MinDate =
									dtDateDurationEnd.MinDate =
										DateTime.MinValue;

			cbFrequencyType.DisplayMember = cbFrequencyInterval.DisplayMember = cbScheduleType.DisplayMember = cbOccursEveryType.DisplayMember = cbOccursEvery.DisplayMember = "Key";
			cbFrequencyType.ValueMember = cbFrequencyInterval.ValueMember = cbScheduleType.ValueMember = cbOccursEveryType.ValueMember = cbOccursEvery.ValueMember = "Value";

			cbFrequencyType.Items.Clear();
			cbFrequencyInterval.Items.Clear();
			cbScheduleType.Items.Clear();
			cbOccursEveryType.Items.Clear();
			cbOccursEvery.Items.Clear();

			cbFrequencyInterval.DropDownStyle =
				cbFrequencyType.DropDownStyle =
				cbOccursEvery.DropDownStyle =
				cbOccursEveryType.DropDownStyle =
				cbScheduleType.DropDownStyle =
					ComboBoxStyle.DropDownList;

			cbScheduleType.DataSource = ToLocalized<ScheduleType>().ToList();
			cbOccursEveryType.DataSource = ToLocalized<DailyFrequency.TimeUnit>().ToList();
			cbFrequencyType.DataSource = ToLocalized<ReccurPeriodTimeUnit>().ToList();

			var numberList = new List<KeyValuePair<int, int>>();

			for (var i = 1; i < 61; i++)
			{
				numberList.Add(new KeyValuePair<int, int>(i, i));
			}

			cbFrequencyInterval.DataSource = numberList.ToList();
			cbOccursEvery.DataSource = numberList.ToList();

			// set default values
			tbId.Text                   = job.Settings.Id;
			tbName.Text                 = job.Settings.Name;
			cbScheduleType.SelectedItem = cbScheduleType.Items.Cast<KeyValuePair<string, ScheduleType>>().FirstOrDefault();

			DateTime tmpDateTime = DateTime.Now;
			dtDateStartOneTime.Value = new DateTime(tmpDateTime.Date.Ticks);
			dtTimeStartOneTime.Value = new DateTime(tmpDateTime.Ticks);

			dtDateDurationStart.Value = new DateTime(tmpDateTime.Ticks);
			dtDateDurationEnd.Value = new DateTime(tmpDateTime.Ticks > dtDateDurationEnd.MaxDate.Ticks ? dtDateDurationEnd.MaxDate.Date.Ticks : tmpDateTime.Ticks);

			if (new DateTime(tmpDateTime.Ticks) == DateTime.MaxValue || dtDateDurationEnd.Value == dtDateDurationEnd.MaxDate.Date)
			{
				rbNoEndDate.Checked = true;
			}
			else
			{
				rbEndDate.Checked = true;
			}

			radioButton1_CheckedChanged(rbEndDate, new EventArgs());

			cbScheduleType_SelectedIndexChanged(cbScheduleType, new EventArgs());

			cbFrequencyType.SelectedItem = cbFrequencyType.Items.Cast<KeyValuePair<string, ReccurPeriodTimeUnit>>()
				.FirstOrDefault();

			cbFrequencyInterval.SelectedItem = cbFrequencyInterval.Items.Cast<KeyValuePair<int, int>>()
				.FirstOrDefault();

			cbJobEnabled.Checked = true;

			cbOccursEvery.SelectedItem = cbOccursEvery.Items.Cast<KeyValuePair<int, int>>()
				.FirstOrDefault();

			cbOccursEveryType.SelectedItem = cbOccursEveryType.Items.Cast<KeyValuePair<string, DailyFrequency.TimeUnit>>()
				.FirstOrDefault();

			dtTimeOccursOnly.Value =
				new DateTime(tmpDateTime.Ticks);

			dtDaylyEveryStart.Value = new DateTime(tmpDateTime.Ticks);
			dtDaylyEveryEnd.Value = new DateTime(tmpDateTime.Ticks);

			rbDailyFreqOccursOnce.Checked = false;
			rbDailyFreqOccursEvery.Checked = true;
			rbDailyFreqOccursOnce_CheckedChanged(rbDailyFreqOccursOnce, new EventArgs());

			cbEmailNotification.Checked = job.Settings.IsSendMessage;
			refreshEmailNotificationControlsState();
		}

		protected string GetLocalizedText(string name)
		{
			if (Program.Model == null)
			{
				return name;
			}

			return Program.Model.LocaleManager != null
				? Program.Model.LocaleManager.GetLocalizedText(Name, name)
				: name;
		}

		#region Private objects and Initializing

		private IEnumerable<KeyValuePair<string, TEnum>> ToLocalized<TEnum>()
		{
			Debug.Assert(typeof (TEnum).IsEnum);

			return from v in Enum.GetValues(typeof (TEnum)).Cast<TEnum>()
				let name = Enum.GetName(typeof (TEnum), v)
				let displayName = GetLocalizedText(name) ?? name
				select new KeyValuePair<string, TEnum>(displayName, v);
		}
		#endregion

		#region Events methods

		private void UserSettingScheduleControl_Load(object sender, EventArgs e)
		{
			if (job == null)
			{
				dtDaylyEveryStart.Value           = DateTime.Now.Date.Add(TimeSpan.Zero);
				dtDaylyEveryEnd.Value             = DateTime.Now.Date.Add(TimeSpan.FromHours(24).Subtract(TimeSpan.FromTicks(1)));
				cbScheduleType.SelectedValue      = ScheduleType.Recurring;
				cbFrequencyType.SelectedValue     = ReccurPeriodTimeUnit.Daily;
				cbFrequencyInterval.SelectedValue = 1;
				rbDailyFreqOccursOnce.Checked     = true;
				dtDateDurationStart.Value         = DateTime.Now.Date;
				rbNoEndDate.Checked               = true;
				cbJobEnabled.Checked              = false;

				return;
			}
			//this.mnuAddNewSchedule.Text = GetLocalizedText("mnuAddNewSchedule");

			cbScheduleType.SelectedValue = !job.Settings.Recuring ? ScheduleType.One_time : ScheduleType.Recurring;

			DateTime startDateTime;
			DateTime.TryParseExact(job.Settings.StartDate.ToString(), "yyyyMMdd", CultureInfo.CurrentCulture,
				DateTimeStyles.None, out startDateTime);

			if (job.Settings.OccursOnceDateTimeEnabled)
			{
				dtDateStartOneTime.Value = job.Settings.OccursOnceDateTime.FitToBouds(dtDateStartOneTime.MinDate, dtDateStartOneTime.MaxDate);
				dtTimeStartOneTime.Value = job.Settings.OccursOnceDateTime.FitToBouds(dtTimeStartOneTime.MinDate, dtTimeStartOneTime.MaxDate);
			}

			dtDateDurationStart.Value = job.Settings.StartDate;

			if (job.Settings.HasEndDate)
			{
				dtDateDurationEnd.Value = job.Settings.EndDate;
			}

			rbEndDate.Checked = job.Settings.HasEndDate;
			rbNoEndDate.Checked = !job.Settings.HasEndDate;

			radioButton1_CheckedChanged(rbEndDate, new EventArgs());

			cbScheduleType_SelectedIndexChanged(cbScheduleType, new EventArgs());

			if (job.Settings.ReccurPeriod != null)
			{
				cbFrequencyType.SelectedValue     = job.Settings.ReccurPeriod.TimeUnit;
				cbFrequencyInterval.SelectedValue = job.Settings.ReccurPeriod.TimeUnitCount;
			}

			cbJobEnabled.Checked = job.Enabled;

			if (job.Settings.DailyFrequency.PeriodTimeUnit != null)
			{
				cbOccursEvery.SelectedValue     = job.Settings.DailyFrequency.PeriodTimeUnitCount;
				cbOccursEveryType.SelectedValue = job.Settings.DailyFrequency.PeriodTimeUnit;
			}

			dtTimeOccursOnly.Value = DateTime.Now.Date.Add(job.Settings.DailyFrequency.OccursOnceTime);

			dtDaylyEveryStart.Value = DateTime.Now.Date.Add(job.Settings.DailyFrequency.StartingAt ?? TimeSpan.Zero);
			dtDaylyEveryEnd.Value = DateTime.Now.Date.Add(job.Settings.DailyFrequency.EndingAt ?? new TimeSpan(23, 59, 59));

			rbDailyFreqOccursOnce.Checked = job.Settings.DailyFrequency.OccursOnce;

			rbDailyFreqOccursEvery.Checked = !job.Settings.DailyFrequency.OccursOnce;

			cbMonday.Checked    = job.Settings.ActiveWeekDays.Contains(DayOfWeek.Monday);
			cbTuesday.Checked   = job.Settings.ActiveWeekDays.Contains(DayOfWeek.Tuesday);
			cbWednesday.Checked = job.Settings.ActiveWeekDays.Contains(DayOfWeek.Wednesday);
			cbThursday.Checked  = job.Settings.ActiveWeekDays.Contains(DayOfWeek.Thursday);
			cbFriday.Checked    = job.Settings.ActiveWeekDays.Contains(DayOfWeek.Friday);
			cbSaturday.Checked  = job.Settings.ActiveWeekDays.Contains(DayOfWeek.Saturday);
			cbSunday.Checked    = job.Settings.ActiveWeekDays.Contains(DayOfWeek.Sunday);

			if (job.Settings.DayOfMonth != null)
			{
				dayOfMonthUpDown.Value = job.Settings.DayOfMonth.DayNumber;
			}

			rbDailyFreqOccursOnce_CheckedChanged(rbDailyFreqOccursOnce, new EventArgs());
			cbFrequencyType_SelectedValueChanged(daysOfWeekPanel, EventArgs.Empty);
		}

		private void cbScheduleType_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateEnableness();
		}

		private void rbDailyFreqOccursOnce_CheckedChanged(object sender, EventArgs e)
		{
			UpdateEnableness();
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			UpdateEnableness();
		}

		#endregion

		private void UpdateEnableness()
		{
			Extentions.EnableControls(cbJobEnabled.Checked
				&& (ScheduleType)cbScheduleType.SelectedValue == ScheduleType.One_time,
				dtDateStartOneTime, dtTimeStartOneTime);

			Extentions.EnableControls(cbJobEnabled.Checked
				&& (ScheduleType)cbScheduleType.SelectedValue == ScheduleType.Recurring,
				cbFrequencyType, cbFrequencyInterval,
				rbDailyFreqOccursEvery, rbDailyFreqOccursOnce, dtDateDurationStart, rbEndDate, rbNoEndDate, dtDateDurationEnd,
				daysOfWeekPanel, dayOfMonthPanel);

			Extentions.EnableControls(cbJobEnabled.Checked && rbDailyFreqOccursOnce.Checked
				&& (ScheduleType)cbScheduleType.SelectedValue == ScheduleType.Recurring,
				dtTimeOccursOnly);

			Extentions.EnableControls(cbJobEnabled.Checked
				&& (ScheduleType)cbScheduleType.SelectedValue == ScheduleType.Recurring
				&& rbDailyFreqOccursEvery.Checked,
				cbOccursEvery, cbOccursEveryType, dtDaylyEveryStart, dtDaylyEveryEnd);

			Extentions.EnableControls(cbJobEnabled.Checked
				&& (ScheduleType)cbScheduleType.SelectedValue == ScheduleType.Recurring
				&& rbEndDate.Checked,
				dtDateDurationEnd);

			refreshEmailNotificationControlsState();
		}

		private void cbJobEnabled_CheckedChanged(object sender, EventArgs e)
		{
			UpdateEnableness();
		}

		private void cbFrequencyType_SelectedValueChanged(object sender, EventArgs e)
		{
			if (cbFrequencyType.SelectedValue == null)
			{
				return;
			}

			var weekly  = (ReccurPeriodTimeUnit)cbFrequencyType.SelectedValue == ReccurPeriodTimeUnit.Weekly;
			var monthly = (ReccurPeriodTimeUnit)cbFrequencyType.SelectedValue == ReccurPeriodTimeUnit.Monthly;

			var oldVisible = daysOfWeekPanel.Visible || dayOfMonthPanel.Visible;

			if ((weekly || monthly) != oldVisible)
			{
				if (weekly || monthly)
				{
					this.Height = this.Height + daysOfWeekPanel.Height;
				}
				else
				{
					this.Height = this.Height - daysOfWeekPanel.Height;
				}
			}

			daysOfWeekPanel.Visible = weekly;
			dayOfMonthPanel.Visible = monthly;
		}

		public bool scheduleEnabled
		{
			get { return cbJobEnabled.Checked; }
		}

		public TemplateNodeUpdateJob getJob()
		{
			TemplateNodeUpdateJob newJob = new TemplateNodeUpdateJob();

			newJob.Settings = new ScheduleSettings();

			newJob.Settings.Id            = job.Settings.Id;
			newJob.Settings.Name          = tbName.Text;
			newJob.Settings.IsSendMessage = cbEmailNotification.Checked;
			newJob.Settings.Enabled       = cbJobEnabled.Checked;

			var schedType = (ScheduleType) cbScheduleType.SelectedValue;

			switch (schedType)
			{
				case ScheduleType.One_time:
					newJob.Settings.OccursOnceDateTime =
						dtDateStartOneTime.Value.Date.Add(dtTimeStartOneTime.Value.TimeOfDay);
					newJob.Settings.OccursOnceDateTimeEnabled = true;
					break;

				case ScheduleType.Recurring:
					if (cbFrequencyType.SelectedIndex == -1)
					{
						cbFrequencyType.SelectedIndex = 0;
					}

					if (cbFrequencyInterval.SelectedIndex == -1)
					{
						cbFrequencyInterval.SelectedIndex = 0;
					}

					newJob.Settings.ReccurPeriod =
						new TimeUnitBasedPeriod(
							(ReccurPeriodTimeUnit) cbFrequencyType.SelectedValue,
							(int) cbFrequencyInterval.SelectedValue
						);

					newJob.Settings.OccursOnceDateTimeEnabled = false;

					break;
			}

			newJob.Settings.DailyFrequency = new DailyFrequency
			{
				OccursOnce = rbDailyFreqOccursOnce.Checked
			};

			if (rbDailyFreqOccursOnce.Checked)
			{
				newJob.Settings.DailyFrequency.OccursOnceTime = dtTimeOccursOnly.Value.TimeOfDay;
			}
			else
			{
				newJob.Settings.DailyFrequency.PeriodTimeUnit      = (DailyFrequency.TimeUnit) cbOccursEveryType.SelectedValue;
				newJob.Settings.DailyFrequency.PeriodTimeUnitCount = (int) cbOccursEvery.SelectedValue;
				newJob.Settings.DailyFrequency.StartingAt          = dtDaylyEveryStart.Value.TimeOfDay;
				newJob.Settings.DailyFrequency.EndingAt            = dtDaylyEveryEnd.Value.TimeOfDay;
			}

			newJob.Settings.StartDate  = dtDateDurationStart.Value;
			newJob.Settings.EndDate    = dtDateDurationEnd.Value;
			newJob.Settings.HasEndDate = rbEndDate.Checked;

			newJob.Settings.ActiveWeekDays.Clear();

			if (cbMonday.Checked)
			{
				newJob.Settings.ActiveWeekDays.Add(DayOfWeek.Monday);
			}

			if (cbTuesday.Checked)
			{
				newJob.Settings.ActiveWeekDays.Add(DayOfWeek.Tuesday);
			}

			if (cbWednesday.Checked)
			{
				newJob.Settings.ActiveWeekDays.Add(DayOfWeek.Wednesday);
			}

			if (cbThursday.Checked)
			{
				newJob.Settings.ActiveWeekDays.Add(DayOfWeek.Thursday);
			}

			if (cbFriday.Checked)
			{
				newJob.Settings.ActiveWeekDays.Add(DayOfWeek.Friday);
			}

			if (cbSaturday.Checked)
			{
				newJob.Settings.ActiveWeekDays.Add(DayOfWeek.Saturday);
			}

			if (cbSunday.Checked)
			{
				newJob.Settings.ActiveWeekDays.Add(DayOfWeek.Sunday);
			}

			if (dayOfMonthPanel.Visible)
			{
				newJob.Settings.DayOfMonth = new DayOfMonthSettings {DayNumber = (int) dayOfMonthUpDown.Value};
			}

			return newJob;
		}

		private void UserSettingScheduleControl_VisibleChanged(object sender, EventArgs e)
		{
			cbFrequencyType_SelectedValueChanged(daysOfWeekPanel, EventArgs.Empty);
		}

		private void refreshEmailNotificationControlsState()
		{
			cbEmailNotification.Enabled = cbJobEnabled.Checked;
		}

		private void cbEmailNotification_Click(object sender, EventArgs e)
		{
			refreshEmailNotificationControlsState();
		}
	}
}
