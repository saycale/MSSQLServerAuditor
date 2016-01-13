namespace MSSQLServerAuditor.Gui
{
	partial class UserSettingScheduleControl
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dayOfMonthPanel = new System.Windows.Forms.Panel();
			this.dayOfMonthUpDown = new System.Windows.Forms.NumericUpDown();
			this.ofMonthLabel = new System.Windows.Forms.Label();
			this.dayLabel = new System.Windows.Forms.Label();
			this.daysOfWeekPanel = new System.Windows.Forms.Panel();
			this.cbSunday = new System.Windows.Forms.CheckBox();
			this.cbSaturday = new System.Windows.Forms.CheckBox();
			this.cbFriday = new System.Windows.Forms.CheckBox();
			this.cbThursday = new System.Windows.Forms.CheckBox();
			this.cbWednesday = new System.Windows.Forms.CheckBox();
			this.cbTuesday = new System.Windows.Forms.CheckBox();
			this.cbMonday = new System.Windows.Forms.CheckBox();
			this.dtDateDurationEnd = new System.Windows.Forms.DateTimePicker();
			this.panel2 = new System.Windows.Forms.Panel();
			this.rbNoEndDate = new System.Windows.Forms.RadioButton();
			this.rbEndDate = new System.Windows.Forms.RadioButton();
			this.lbStartDate = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.dtDaylyEveryEnd = new System.Windows.Forms.DateTimePicker();
			this.dtDaylyEveryStart = new System.Windows.Forms.DateTimePicker();
			this.cbOccursEveryType = new System.Windows.Forms.ComboBox();
			this.cbOccursEvery = new System.Windows.Forms.ComboBox();
			this.dtTimeOccursOnly = new System.Windows.Forms.DateTimePicker();
			this.panel1 = new System.Windows.Forms.Panel();
			this.rbDailyFreqOccursOnce = new System.Windows.Forms.RadioButton();
			this.rbDailyFreqOccursEvery = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.lbDailyFreequency = new System.Windows.Forms.Label();
			this.lbOccurs = new System.Windows.Forms.Label();
			this.lbHor2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.dtTimeStartOneTime = new System.Windows.Forms.DateTimePicker();
			this.lbTimeOnce = new System.Windows.Forms.Label();
			this.lbDateOnce = new System.Windows.Forms.Label();
			this.lbOneTimeheader = new System.Windows.Forms.Label();
			this.lbHor1 = new System.Windows.Forms.Label();
			this.lbScheduleType = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.cbScheduleType = new System.Windows.Forms.ComboBox();
			this.lbName = new System.Windows.Forms.Label();
			this.dtDateDurationStart = new System.Windows.Forms.DateTimePicker();
			this.dtDateStartOneTime = new System.Windows.Forms.DateTimePicker();
			this.cbJobEnabled = new System.Windows.Forms.CheckBox();
			this.cbFrequencyInterval = new System.Windows.Forms.ComboBox();
			this.cbFrequencyType = new System.Windows.Forms.ComboBox();
			this.lbEndDateTime = new System.Windows.Forms.Label();
			this.lbStartDateTime = new System.Windows.Forms.Label();
			this.lbFrequencyType = new System.Windows.Forms.Label();
			this.lblEnabled = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.cbEmailNotification = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tbId = new System.Windows.Forms.TextBox();
			this.dayOfMonthPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dayOfMonthUpDown)).BeginInit();
			this.daysOfWeekPanel.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			//
			// dayOfMonthPanel
			//
			this.dayOfMonthPanel.Controls.Add(this.dayOfMonthUpDown);
			this.dayOfMonthPanel.Controls.Add(this.ofMonthLabel);
			this.dayOfMonthPanel.Controls.Add(this.dayLabel);
			this.dayOfMonthPanel.Location = new System.Drawing.Point(39, 248);
			this.dayOfMonthPanel.Name = "dayOfMonthPanel";
			this.dayOfMonthPanel.Size = new System.Drawing.Size(512, 51);
			this.dayOfMonthPanel.TabIndex = 159;
			//
			// dayOfMonthUpDown
			//
			this.dayOfMonthUpDown.Location = new System.Drawing.Point(154, 6);
			this.dayOfMonthUpDown.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
			this.dayOfMonthUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.dayOfMonthUpDown.Name = "dayOfMonthUpDown";
			this.dayOfMonthUpDown.Size = new System.Drawing.Size(54, 20);
			this.dayOfMonthUpDown.TabIndex = 8;
			this.dayOfMonthUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			//
			// ofMonthLabel
			//
			this.ofMonthLabel.AutoSize = true;
			this.ofMonthLabel.Location = new System.Drawing.Point(214, 10);
			this.ofMonthLabel.Name = "ofMonthLabel";
			this.ofMonthLabel.Size = new System.Drawing.Size(48, 13);
			this.ofMonthLabel.TabIndex = 7;
			this.ofMonthLabel.Text = "of month";
			//
			// dayLabel
			//
			this.dayLabel.AutoSize = true;
			this.dayLabel.Location = new System.Drawing.Point(52, 10);
			this.dayLabel.Name = "dayLabel";
			this.dayLabel.Size = new System.Drawing.Size(32, 13);
			this.dayLabel.TabIndex = 6;
			this.dayLabel.Text = "Day: ";
			//
			// daysOfWeekPanel
			//
			this.daysOfWeekPanel.Controls.Add(this.cbSunday);
			this.daysOfWeekPanel.Controls.Add(this.cbSaturday);
			this.daysOfWeekPanel.Controls.Add(this.cbFriday);
			this.daysOfWeekPanel.Controls.Add(this.cbThursday);
			this.daysOfWeekPanel.Controls.Add(this.cbWednesday);
			this.daysOfWeekPanel.Controls.Add(this.cbTuesday);
			this.daysOfWeekPanel.Controls.Add(this.cbMonday);
			this.daysOfWeekPanel.Location = new System.Drawing.Point(39, 248);
			this.daysOfWeekPanel.Name = "daysOfWeekPanel";
			this.daysOfWeekPanel.Size = new System.Drawing.Size(512, 51);
			this.daysOfWeekPanel.TabIndex = 158;
			//
			// cbSunday
			//
			this.cbSunday.AutoSize = true;
			this.cbSunday.Location = new System.Drawing.Point(348, 26);
			this.cbSunday.Name = "cbSunday";
			this.cbSunday.Size = new System.Drawing.Size(62, 17);
			this.cbSunday.TabIndex = 49;
			this.cbSunday.Text = "Sunday";
			this.cbSunday.UseVisualStyleBackColor = true;
			//
			// cbSaturday
			//
			this.cbSaturday.AutoSize = true;
			this.cbSaturday.Location = new System.Drawing.Point(348, 3);
			this.cbSaturday.Name = "cbSaturday";
			this.cbSaturday.Size = new System.Drawing.Size(68, 17);
			this.cbSaturday.TabIndex = 48;
			this.cbSaturday.Text = "Saturday";
			this.cbSaturday.UseVisualStyleBackColor = true;
			//
			// cbFriday
			//
			this.cbFriday.AutoSize = true;
			this.cbFriday.Location = new System.Drawing.Point(233, 3);
			this.cbFriday.Name = "cbFriday";
			this.cbFriday.Size = new System.Drawing.Size(54, 17);
			this.cbFriday.TabIndex = 47;
			this.cbFriday.Text = "Friday";
			this.cbFriday.UseVisualStyleBackColor = true;
			//
			// cbThursday
			//
			this.cbThursday.AutoSize = true;
			this.cbThursday.Location = new System.Drawing.Point(120, 26);
			this.cbThursday.Name = "cbThursday";
			this.cbThursday.Size = new System.Drawing.Size(70, 17);
			this.cbThursday.TabIndex = 46;
			this.cbThursday.Text = "Thursday";
			this.cbThursday.UseVisualStyleBackColor = true;
			//
			// cbWednesday
			//
			this.cbWednesday.AutoSize = true;
			this.cbWednesday.Location = new System.Drawing.Point(120, 3);
			this.cbWednesday.Name = "cbWednesday";
			this.cbWednesday.Size = new System.Drawing.Size(83, 17);
			this.cbWednesday.TabIndex = 45;
			this.cbWednesday.Text = "Wednesday";
			this.cbWednesday.UseVisualStyleBackColor = true;
			//
			// cbTuesday
			//
			this.cbTuesday.AutoSize = true;
			this.cbTuesday.Location = new System.Drawing.Point(7, 26);
			this.cbTuesday.Name = "cbTuesday";
			this.cbTuesday.Size = new System.Drawing.Size(67, 17);
			this.cbTuesday.TabIndex = 44;
			this.cbTuesday.Text = "Tuesday";
			this.cbTuesday.UseVisualStyleBackColor = true;
			//
			// cbMonday
			//
			this.cbMonday.AutoSize = true;
			this.cbMonday.Location = new System.Drawing.Point(7, 3);
			this.cbMonday.Name = "cbMonday";
			this.cbMonday.Size = new System.Drawing.Size(64, 17);
			this.cbMonday.TabIndex = 43;
			this.cbMonday.Text = "Monday";
			this.cbMonday.UseVisualStyleBackColor = true;
			//
			// dtDateDurationEnd
			//
			this.dtDateDurationEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.dtDateDurationEnd.Location = new System.Drawing.Point(451, 489);
			this.dtDateDurationEnd.Name = "dtDateDurationEnd";
			this.dtDateDurationEnd.Size = new System.Drawing.Size(111, 20);
			this.dtDateDurationEnd.TabIndex = 157;
			//
			// panel2
			//
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panel2.Controls.Add(this.rbNoEndDate);
			this.panel2.Controls.Add(this.rbEndDate);
			this.panel2.Location = new System.Drawing.Point(322, 477);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(119, 75);
			this.panel2.TabIndex = 156;
			//
			// rbNoEndDate
			//
			this.rbNoEndDate.AutoSize = true;
			this.rbNoEndDate.Location = new System.Drawing.Point(11, 43);
			this.rbNoEndDate.Name = "rbNoEndDate";
			this.rbNoEndDate.Size = new System.Drawing.Size(87, 17);
			this.rbNoEndDate.TabIndex = 1;
			this.rbNoEndDate.TabStop = true;
			this.rbNoEndDate.Text = "No end date:";
			this.rbNoEndDate.UseVisualStyleBackColor = true;
			this.rbNoEndDate.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
			//
			// rbEndDate
			//
			this.rbEndDate.AutoSize = true;
			this.rbEndDate.Location = new System.Drawing.Point(11, 15);
			this.rbEndDate.Name = "rbEndDate";
			this.rbEndDate.Size = new System.Drawing.Size(71, 17);
			this.rbEndDate.TabIndex = 0;
			this.rbEndDate.TabStop = true;
			this.rbEndDate.Text = "End date:";
			this.rbEndDate.UseVisualStyleBackColor = true;
			this.rbEndDate.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
			//
			// lbStartDate
			//
			this.lbStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbStartDate.AutoSize = true;
			this.lbStartDate.Location = new System.Drawing.Point(39, 489);
			this.lbStartDate.Name = "lbStartDate";
			this.lbStartDate.Size = new System.Drawing.Size(56, 13);
			this.lbStartDate.TabIndex = 155;
			this.lbStartDate.Text = "Start date:";
			//
			// label3
			//
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.SystemColors.AppWorkspace;
			this.label3.Location = new System.Drawing.Point(82, 457);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(481, 13);
			this.label3.TabIndex = 154;
			this.label3.Text = "_______________________________________________________________________________";
			//
			// label4
			//
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(22, 457);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(47, 13);
			this.label4.TabIndex = 153;
			this.label4.Text = "Duration";
			//
			// dtDaylyEveryEnd
			//
			this.dtDaylyEveryEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.dtDaylyEveryEnd.Checked = false;
			this.dtDaylyEveryEnd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.dtDaylyEveryEnd.Location = new System.Drawing.Point(483, 418);
			this.dtDaylyEveryEnd.Name = "dtDaylyEveryEnd";
			this.dtDaylyEveryEnd.ShowUpDown = true;
			this.dtDaylyEveryEnd.Size = new System.Drawing.Size(75, 20);
			this.dtDaylyEveryEnd.TabIndex = 152;
			//
			// dtDaylyEveryStart
			//
			this.dtDaylyEveryStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.dtDaylyEveryStart.Checked = false;
			this.dtDaylyEveryStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.dtDaylyEveryStart.Location = new System.Drawing.Point(483, 384);
			this.dtDaylyEveryStart.Name = "dtDaylyEveryStart";
			this.dtDaylyEveryStart.ShowUpDown = true;
			this.dtDaylyEveryStart.Size = new System.Drawing.Size(75, 20);
			this.dtDaylyEveryStart.TabIndex = 151;
			//
			// cbOccursEveryType
			//
			this.cbOccursEveryType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbOccursEveryType.FormattingEnabled = true;
			this.cbOccursEveryType.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.cbOccursEveryType.Location = new System.Drawing.Point(267, 387);
			this.cbOccursEveryType.Name = "cbOccursEveryType";
			this.cbOccursEveryType.Size = new System.Drawing.Size(108, 21);
			this.cbOccursEveryType.TabIndex = 150;
			//
			// cbOccursEvery
			//
			this.cbOccursEvery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbOccursEvery.FormattingEnabled = true;
			this.cbOccursEvery.Location = new System.Drawing.Point(192, 387);
			this.cbOccursEvery.Name = "cbOccursEvery";
			this.cbOccursEvery.Size = new System.Drawing.Size(55, 21);
			this.cbOccursEvery.TabIndex = 149;
			//
			// dtTimeOccursOnly
			//
			this.dtTimeOccursOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.dtTimeOccursOnly.Checked = false;
			this.dtTimeOccursOnly.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.dtTimeOccursOnly.Location = new System.Drawing.Point(193, 349);
			this.dtTimeOccursOnly.Name = "dtTimeOccursOnly";
			this.dtTimeOccursOnly.ShowUpDown = true;
			this.dtTimeOccursOnly.Size = new System.Drawing.Size(70, 20);
			this.dtTimeOccursOnly.TabIndex = 148;
			//
			// panel1
			//
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panel1.Controls.Add(this.rbDailyFreqOccursOnce);
			this.panel1.Controls.Add(this.rbDailyFreqOccursEvery);
			this.panel1.Location = new System.Drawing.Point(25, 346);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(161, 67);
			this.panel1.TabIndex = 147;
			//
			// rbDailyFreqOccursOnce
			//
			this.rbDailyFreqOccursOnce.AutoSize = true;
			this.rbDailyFreqOccursOnce.Location = new System.Drawing.Point(18, 3);
			this.rbDailyFreqOccursOnce.Name = "rbDailyFreqOccursOnce";
			this.rbDailyFreqOccursOnce.Size = new System.Drawing.Size(101, 17);
			this.rbDailyFreqOccursOnce.TabIndex = 28;
			this.rbDailyFreqOccursOnce.TabStop = true;
			this.rbDailyFreqOccursOnce.Text = "Occurs once at:";
			this.rbDailyFreqOccursOnce.UseVisualStyleBackColor = true;
			this.rbDailyFreqOccursOnce.CheckedChanged += new System.EventHandler(this.rbDailyFreqOccursOnce_CheckedChanged);
			//
			// rbDailyFreqOccursEvery
			//
			this.rbDailyFreqOccursEvery.AutoSize = true;
			this.rbDailyFreqOccursEvery.Location = new System.Drawing.Point(18, 36);
			this.rbDailyFreqOccursEvery.Name = "rbDailyFreqOccursEvery";
			this.rbDailyFreqOccursEvery.Size = new System.Drawing.Size(90, 17);
			this.rbDailyFreqOccursEvery.TabIndex = 29;
			this.rbDailyFreqOccursEvery.TabStop = true;
			this.rbDailyFreqOccursEvery.Text = "Occyrs every:";
			this.rbDailyFreqOccursEvery.UseVisualStyleBackColor = true;
			//
			// label2
			//
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.AppWorkspace;
			this.label2.Location = new System.Drawing.Point(105, 320);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(457, 13);
			this.label2.TabIndex = 146;
			this.label2.Text = "___________________________________________________________________________";
			//
			// lbDailyFreequency
			//
			this.lbDailyFreequency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbDailyFreequency.AutoSize = true;
			this.lbDailyFreequency.Location = new System.Drawing.Point(13, 320);
			this.lbDailyFreequency.Name = "lbDailyFreequency";
			this.lbDailyFreequency.Size = new System.Drawing.Size(86, 13);
			this.lbDailyFreequency.TabIndex = 145;
			this.lbDailyFreequency.Text = "Daily freequency";
			//
			// lbOccurs
			//
			this.lbOccurs.AutoSize = true;
			this.lbOccurs.Location = new System.Drawing.Point(44, 189);
			this.lbOccurs.Name = "lbOccurs";
			this.lbOccurs.Size = new System.Drawing.Size(44, 13);
			this.lbOccurs.TabIndex = 144;
			this.lbOccurs.Text = "Occurs:";
			//
			// lbHor2
			//
			this.lbHor2.AutoSize = true;
			this.lbHor2.ForeColor = System.Drawing.SystemColors.AppWorkspace;
			this.lbHor2.Location = new System.Drawing.Point(76, 155);
			this.lbHor2.Name = "lbHor2";
			this.lbHor2.Size = new System.Drawing.Size(481, 13);
			this.lbHor2.TabIndex = 143;
			this.lbHor2.Text = "_______________________________________________________________________________";
			//
			// label1
			//
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 159);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(57, 13);
			this.label1.TabIndex = 142;
			this.label1.Text = "Frequency";
			//
			// dtTimeStartOneTime
			//
			this.dtTimeStartOneTime.Checked = false;
			this.dtTimeStartOneTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.dtTimeStartOneTime.Location = new System.Drawing.Point(409, 116);
			this.dtTimeStartOneTime.Name = "dtTimeStartOneTime";
			this.dtTimeStartOneTime.ShowUpDown = true;
			this.dtTimeStartOneTime.Size = new System.Drawing.Size(81, 20);
			this.dtTimeStartOneTime.TabIndex = 141;
			//
			// lbTimeOnce
			//
			this.lbTimeOnce.AutoSize = true;
			this.lbTimeOnce.Location = new System.Drawing.Point(358, 123);
			this.lbTimeOnce.Name = "lbTimeOnce";
			this.lbTimeOnce.Size = new System.Drawing.Size(33, 13);
			this.lbTimeOnce.TabIndex = 140;
			this.lbTimeOnce.Text = "Time:";
			//
			// lbDateOnce
			//
			this.lbDateOnce.AutoSize = true;
			this.lbDateOnce.Location = new System.Drawing.Point(37, 123);
			this.lbDateOnce.Name = "lbDateOnce";
			this.lbDateOnce.Size = new System.Drawing.Size(33, 13);
			this.lbDateOnce.TabIndex = 139;
			this.lbDateOnce.Text = "Date:";
			//
			// lbOneTimeheader
			//
			this.lbOneTimeheader.AutoSize = true;
			this.lbOneTimeheader.Location = new System.Drawing.Point(10, 94);
			this.lbOneTimeheader.Name = "lbOneTimeheader";
			this.lbOneTimeheader.Size = new System.Drawing.Size(102, 13);
			this.lbOneTimeheader.TabIndex = 138;
			this.lbOneTimeheader.Text = "One-time occurency";
			//
			// lbHor1
			//
			this.lbHor1.AutoSize = true;
			this.lbHor1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
			this.lbHor1.Location = new System.Drawing.Point(118, 89);
			this.lbHor1.Name = "lbHor1";
			this.lbHor1.Size = new System.Drawing.Size(439, 13);
			this.lbHor1.TabIndex = 137;
			this.lbHor1.Text = "________________________________________________________________________";
			//
			// lbScheduleType
			//
			this.lbScheduleType.AutoSize = true;
			this.lbScheduleType.Location = new System.Drawing.Point(10, 71);
			this.lbScheduleType.Name = "lbScheduleType";
			this.lbScheduleType.Size = new System.Drawing.Size(78, 13);
			this.lbScheduleType.TabIndex = 136;
			this.lbScheduleType.Text = "Schedule type:";
			//
			// tbName
			//
			this.tbName.Location = new System.Drawing.Point(193, 36);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(297, 20);
			this.tbName.TabIndex = 135;
			//
			// cbScheduleType
			//
			this.cbScheduleType.FormattingEnabled = true;
			this.cbScheduleType.Location = new System.Drawing.Point(193, 68);
			this.cbScheduleType.Name = "cbScheduleType";
			this.cbScheduleType.Size = new System.Drawing.Size(182, 21);
			this.cbScheduleType.TabIndex = 134;
			this.cbScheduleType.SelectedIndexChanged += new System.EventHandler(this.cbScheduleType_SelectedIndexChanged);
			//
			// lbName
			//
			this.lbName.AutoSize = true;
			this.lbName.Location = new System.Drawing.Point(11, 39);
			this.lbName.Name = "lbName";
			this.lbName.Size = new System.Drawing.Size(38, 13);
			this.lbName.TabIndex = 133;
			this.lbName.Text = "Name:";
			//
			// dtDateDurationStart
			//
			this.dtDateDurationStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.dtDateDurationStart.Location = new System.Drawing.Point(192, 489);
			this.dtDateDurationStart.Name = "dtDateDurationStart";
			this.dtDateDurationStart.Size = new System.Drawing.Size(107, 20);
			this.dtDateDurationStart.TabIndex = 132;
			//
			// dtDateStartOneTime
			//
			this.dtDateStartOneTime.ImeMode = System.Windows.Forms.ImeMode.On;
			this.dtDateStartOneTime.Location = new System.Drawing.Point(193, 116);
			this.dtDateStartOneTime.Name = "dtDateStartOneTime";
			this.dtDateStartOneTime.Size = new System.Drawing.Size(107, 20);
			this.dtDateStartOneTime.TabIndex = 131;
			//
			// cbJobEnabled
			//
			this.cbJobEnabled.AutoSize = true;
			this.cbJobEnabled.Location = new System.Drawing.Point(423, 72);
			this.cbJobEnabled.Name = "cbJobEnabled";
			this.cbJobEnabled.Size = new System.Drawing.Size(15, 14);
			this.cbJobEnabled.TabIndex = 130;
			this.cbJobEnabled.UseVisualStyleBackColor = true;
			this.cbJobEnabled.CheckedChanged += new System.EventHandler(this.cbJobEnabled_CheckedChanged);
			//
			// cbFrequencyInterval
			//
			this.cbFrequencyInterval.FormattingEnabled = true;
			this.cbFrequencyInterval.Location = new System.Drawing.Point(193, 218);
			this.cbFrequencyInterval.Name = "cbFrequencyInterval";
			this.cbFrequencyInterval.Size = new System.Drawing.Size(54, 21);
			this.cbFrequencyInterval.TabIndex = 129;
			//
			// cbFrequencyType
			//
			this.cbFrequencyType.FormattingEnabled = true;
			this.cbFrequencyType.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.cbFrequencyType.Location = new System.Drawing.Point(193, 186);
			this.cbFrequencyType.Name = "cbFrequencyType";
			this.cbFrequencyType.Size = new System.Drawing.Size(167, 21);
			this.cbFrequencyType.TabIndex = 128;
			this.cbFrequencyType.SelectedIndexChanged += new System.EventHandler(this.cbFrequencyType_SelectedValueChanged);
			//
			// lbEndDateTime
			//
			this.lbEndDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbEndDateTime.AutoSize = true;
			this.lbEndDateTime.Location = new System.Drawing.Point(383, 418);
			this.lbEndDateTime.Name = "lbEndDateTime";
			this.lbEndDateTime.Size = new System.Drawing.Size(55, 13);
			this.lbEndDateTime.TabIndex = 127;
			this.lbEndDateTime.Text = "Ending at:";
			//
			// lbStartDateTime
			//
			this.lbStartDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbStartDateTime.AutoSize = true;
			this.lbStartDateTime.Location = new System.Drawing.Point(383, 390);
			this.lbStartDateTime.Name = "lbStartDateTime";
			this.lbStartDateTime.Size = new System.Drawing.Size(58, 13);
			this.lbStartDateTime.TabIndex = 126;
			this.lbStartDateTime.Text = "Starting at:";
			//
			// lbFrequencyType
			//
			this.lbFrequencyType.AutoSize = true;
			this.lbFrequencyType.Location = new System.Drawing.Point(44, 221);
			this.lbFrequencyType.Name = "lbFrequencyType";
			this.lbFrequencyType.Size = new System.Drawing.Size(79, 13);
			this.lbFrequencyType.TabIndex = 125;
			this.lbFrequencyType.Text = "Reccurs every:";
			//
			// lblEnabled
			//
			this.lblEnabled.AutoSize = true;
			this.lblEnabled.Location = new System.Drawing.Point(444, 72);
			this.lblEnabled.Name = "lblEnabled";
			this.lblEnabled.Size = new System.Drawing.Size(46, 13);
			this.lblEnabled.TabIndex = 124;
			this.lblEnabled.Text = "Enabled";
			//
			// label5
			//
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.AutoSize = true;
			this.label5.ForeColor = System.Drawing.SystemColors.AppWorkspace;
			this.label5.Location = new System.Drawing.Point(23, 555);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(541, 13);
			this.label5.TabIndex = 160;
			this.label5.Text = "_________________________________________________________________________________" +
    "________";
			//
			// cbEmailNotification
			//
			this.cbEmailNotification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbEmailNotification.AutoSize = true;
			this.cbEmailNotification.Location = new System.Drawing.Point(25, 586);
			this.cbEmailNotification.Name = "cbEmailNotification";
			this.cbEmailNotification.Size = new System.Drawing.Size(108, 17);
			this.cbEmailNotification.TabIndex = 161;
			this.cbEmailNotification.Text = "E-mail notification";
			this.cbEmailNotification.UseVisualStyleBackColor = true;
			this.cbEmailNotification.Click += new System.EventHandler(this.cbEmailNotification_Click);
			//
			// label6
			//
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(11, 6);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(21, 13);
			this.label6.TabIndex = 162;
			this.label6.Text = "ID:";
			//
			// tbId
			//
			this.tbId.Location = new System.Drawing.Point(192, 3);
			this.tbId.Name = "tbId";
			this.tbId.ReadOnly = true;
			this.tbId.Size = new System.Drawing.Size(183, 20);
			this.tbId.TabIndex = 163;
			//
			// UserSettingScheduleControl
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tbId);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.cbEmailNotification);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.dayOfMonthPanel);
			this.Controls.Add(this.daysOfWeekPanel);
			this.Controls.Add(this.dtDateDurationEnd);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.lbStartDate);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.dtDaylyEveryEnd);
			this.Controls.Add(this.dtDaylyEveryStart);
			this.Controls.Add(this.cbOccursEveryType);
			this.Controls.Add(this.cbOccursEvery);
			this.Controls.Add(this.dtTimeOccursOnly);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lbDailyFreequency);
			this.Controls.Add(this.lbOccurs);
			this.Controls.Add(this.lbHor2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dtTimeStartOneTime);
			this.Controls.Add(this.lbTimeOnce);
			this.Controls.Add(this.lbDateOnce);
			this.Controls.Add(this.lbOneTimeheader);
			this.Controls.Add(this.lbHor1);
			this.Controls.Add(this.lbScheduleType);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.cbScheduleType);
			this.Controls.Add(this.lbName);
			this.Controls.Add(this.dtDateDurationStart);
			this.Controls.Add(this.dtDateStartOneTime);
			this.Controls.Add(this.cbJobEnabled);
			this.Controls.Add(this.cbFrequencyInterval);
			this.Controls.Add(this.cbFrequencyType);
			this.Controls.Add(this.lbEndDateTime);
			this.Controls.Add(this.lbStartDateTime);
			this.Controls.Add(this.lbFrequencyType);
			this.Controls.Add(this.lblEnabled);
			this.Name = "UserSettingScheduleControl";
			this.Size = new System.Drawing.Size(574, 623);
			this.Load += new System.EventHandler(this.UserSettingScheduleControl_Load);
			this.VisibleChanged += new System.EventHandler(this.UserSettingScheduleControl_VisibleChanged);
			this.dayOfMonthPanel.ResumeLayout(false);
			this.dayOfMonthPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dayOfMonthUpDown)).EndInit();
			this.daysOfWeekPanel.ResumeLayout(false);
			this.daysOfWeekPanel.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel dayOfMonthPanel;
		private System.Windows.Forms.NumericUpDown dayOfMonthUpDown;
		private System.Windows.Forms.Label ofMonthLabel;
		private System.Windows.Forms.Label dayLabel;
		private System.Windows.Forms.Panel daysOfWeekPanel;
		private System.Windows.Forms.CheckBox cbSunday;
		private System.Windows.Forms.CheckBox cbSaturday;
		private System.Windows.Forms.CheckBox cbFriday;
		private System.Windows.Forms.CheckBox cbThursday;
		private System.Windows.Forms.CheckBox cbWednesday;
		private System.Windows.Forms.CheckBox cbTuesday;
		private System.Windows.Forms.CheckBox cbMonday;
		private System.Windows.Forms.DateTimePicker dtDateDurationEnd;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.RadioButton rbNoEndDate;
		private System.Windows.Forms.RadioButton rbEndDate;
		private System.Windows.Forms.Label lbStartDate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.DateTimePicker dtDaylyEveryEnd;
		private System.Windows.Forms.DateTimePicker dtDaylyEveryStart;
		private System.Windows.Forms.ComboBox cbOccursEveryType;
		private System.Windows.Forms.ComboBox cbOccursEvery;
		private System.Windows.Forms.DateTimePicker dtTimeOccursOnly;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton rbDailyFreqOccursOnce;
		private System.Windows.Forms.RadioButton rbDailyFreqOccursEvery;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lbDailyFreequency;
		private System.Windows.Forms.Label lbOccurs;
		private System.Windows.Forms.Label lbHor2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DateTimePicker dtTimeStartOneTime;
		private System.Windows.Forms.Label lbTimeOnce;
		private System.Windows.Forms.Label lbDateOnce;
		private System.Windows.Forms.Label lbOneTimeheader;
		private System.Windows.Forms.Label lbHor1;
		private System.Windows.Forms.Label lbScheduleType;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.ComboBox cbScheduleType;
		private System.Windows.Forms.Label lbName;
		private System.Windows.Forms.DateTimePicker dtDateDurationStart;
		private System.Windows.Forms.DateTimePicker dtDateStartOneTime;
		private System.Windows.Forms.CheckBox cbJobEnabled;
		private System.Windows.Forms.ComboBox cbFrequencyInterval;
		private System.Windows.Forms.ComboBox cbFrequencyType;
		private System.Windows.Forms.Label lbEndDateTime;
		private System.Windows.Forms.Label lbStartDateTime;
		private System.Windows.Forms.Label lbFrequencyType;
		private System.Windows.Forms.Label lblEnabled;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox cbEmailNotification;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbId;
	}
}
