namespace MSSQLServerAuditor.Gui
{
    partial class frmSettings
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
			this.systemSettingsInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lbReportLang = new System.Windows.Forms.Label();
			this.cmbReportLang = new System.Windows.Forms.ComboBox();
			this.settingsInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cmbInterfaceLang = new System.Windows.Forms.ComboBox();
			this.lbInterfaceLang = new System.Windows.Forms.Label();
			this.lbTimeout = new System.Windows.Forms.Label();
			this.udTimeout = new System.Windows.Forms.NumericUpDown();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.cbShowCloseTabConfirm = new System.Windows.Forms.CheckBox();
			this.cbShowSingleConnectionTab = new System.Windows.Forms.CheckBox();
			this.cbCreateTabsForConnection = new System.Windows.Forms.CheckBox();
			this.udThreadLimit = new System.Windows.Forms.NumericUpDown();
			this.lbMaxThreadCount = new System.Windows.Forms.Label();
			this.cbShowXML = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.systemSettingsInfoBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.settingsInfoBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udTimeout)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udThreadLimit)).BeginInit();
			this.SuspendLayout();
			//
			// systemSettingsInfoBindingSource
			//
			this.systemSettingsInfoBindingSource.DataSource = typeof(MSSQLServerAuditor.Model.Settings.SystemSettingsInfo);
			//
			// lbReportLang
			//
			this.lbReportLang.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbReportLang.Location = new System.Drawing.Point(4, 41);
			this.lbReportLang.Name = "lbReportLang";
			this.lbReportLang.Size = new System.Drawing.Size(154, 20);
			this.lbReportLang.TabIndex = 10;
			this.lbReportLang.Text = "Report language ";
			//
			// cmbReportLang
			//
			this.cmbReportLang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbReportLang.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.settingsInfoBindingSource, "ReportLanguage", true));
			this.cmbReportLang.FormattingEnabled = true;
			this.cmbReportLang.Location = new System.Drawing.Point(164, 41);
			this.cmbReportLang.Name = "cmbReportLang";
			this.cmbReportLang.Size = new System.Drawing.Size(71, 21);
			this.cmbReportLang.TabIndex = 1;
			//
			// settingsInfoBindingSource
			//
			this.settingsInfoBindingSource.DataSource = typeof(MSSQLServerAuditor.Model.Settings.SettingsInfo);
			//
			// cmbInterfaceLang
			//
			this.cmbInterfaceLang.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbInterfaceLang.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.settingsInfoBindingSource, "InterfaceLanguage", true));
			this.cmbInterfaceLang.FormattingEnabled = true;
			this.cmbInterfaceLang.Location = new System.Drawing.Point(164, 13);
			this.cmbInterfaceLang.Name = "cmbInterfaceLang";
			this.cmbInterfaceLang.Size = new System.Drawing.Size(71, 21);
			this.cmbInterfaceLang.TabIndex = 0;
			//
			// lbInterfaceLang
			//
			this.lbInterfaceLang.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbInterfaceLang.Location = new System.Drawing.Point(4, 13);
			this.lbInterfaceLang.Name = "lbInterfaceLang";
			this.lbInterfaceLang.Size = new System.Drawing.Size(154, 20);
			this.lbInterfaceLang.TabIndex = 7;
			this.lbInterfaceLang.Text = "Interface language ";
			//
			// lbTimeout
			//
			this.lbTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbTimeout.Location = new System.Drawing.Point(4, 69);
			this.lbTimeout.Name = "lbTimeout";
			this.lbTimeout.Size = new System.Drawing.Size(154, 20);
			this.lbTimeout.TabIndex = 14;
			this.lbTimeout.Text = "Connection Timeout (s):";
			//
			// udTimeout
			//
			this.udTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.udTimeout.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsInfoBindingSource, "SqlTimeout", true));
			this.udTimeout.Location = new System.Drawing.Point(164, 69);
			this.udTimeout.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.udTimeout.Name = "udTimeout";
			this.udTimeout.Size = new System.Drawing.Size(71, 20);
			this.udTimeout.TabIndex = 2;
			this.udTimeout.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
			//
			// btnCancel
			//
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(166, 246);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			//
			// btnOk
			//
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(85, 246);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 8;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			//
			// cbShowCloseTabConfirm
			//
			this.cbShowCloseTabConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbShowCloseTabConfirm.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsInfoBindingSource, "ShowCloseTabConfirmation", true));
			this.cbShowCloseTabConfirm.Location = new System.Drawing.Point(4, 190);
			this.cbShowCloseTabConfirm.Name = "cbShowCloseTabConfirm";
			this.cbShowCloseTabConfirm.Size = new System.Drawing.Size(231, 17);
			this.cbShowCloseTabConfirm.TabIndex = 6;
			this.cbShowCloseTabConfirm.Text = "Show confirmation on tab close";
			this.cbShowCloseTabConfirm.UseVisualStyleBackColor = true;
			//
			// cbShowSingleConnectionTab
			//
			this.cbShowSingleConnectionTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbShowSingleConnectionTab.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsInfoBindingSource, "ShowConnectionTabIfSingle", true));
			this.cbShowSingleConnectionTab.Location = new System.Drawing.Point(26, 158);
			this.cbShowSingleConnectionTab.Name = "cbShowSingleConnectionTab";
			this.cbShowSingleConnectionTab.Size = new System.Drawing.Size(209, 17);
			this.cbShowSingleConnectionTab.TabIndex = 5;
			this.cbShowSingleConnectionTab.Text = "Show single connection tab";
			this.cbShowSingleConnectionTab.UseVisualStyleBackColor = true;
			//
			// cbCreateTabsForConnection
			//
			this.cbCreateTabsForConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbCreateTabsForConnection.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsInfoBindingSource, "ConnectionsInTabs", true));
			this.cbCreateTabsForConnection.Location = new System.Drawing.Point(4, 126);
			this.cbCreateTabsForConnection.Name = "cbCreateTabsForConnection";
			this.cbCreateTabsForConnection.Size = new System.Drawing.Size(231, 17);
			this.cbCreateTabsForConnection.TabIndex = 4;
			this.cbCreateTabsForConnection.Text = "Create tabs for connections";
			this.cbCreateTabsForConnection.UseVisualStyleBackColor = true;
			this.cbCreateTabsForConnection.CheckedChanged += new System.EventHandler(this.cbConnectionsInTabs_CheckedChanged);
			//
			// udThreadLimit
			//
			this.udThreadLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.udThreadLimit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.settingsInfoBindingSource, "MaximumDBRequestsThreadCount", true));
			this.udThreadLimit.Location = new System.Drawing.Point(164, 97);
			this.udThreadLimit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.udThreadLimit.Name = "udThreadLimit";
			this.udThreadLimit.Size = new System.Drawing.Size(71, 20);
			this.udThreadLimit.TabIndex = 3;
			this.udThreadLimit.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			//
			// lbMaxThreadCount
			//
			this.lbMaxThreadCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbMaxThreadCount.Location = new System.Drawing.Point(4, 97);
			this.lbMaxThreadCount.Name = "lbMaxThreadCount";
			this.lbMaxThreadCount.Size = new System.Drawing.Size(154, 20);
			this.lbMaxThreadCount.TabIndex = 26;
			this.lbMaxThreadCount.Text = "Max threads count:";
			//
			// cbShowXML
			//
			this.cbShowXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbShowXML.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsInfoBindingSource, "ShowXML", true));
			this.cbShowXML.Location = new System.Drawing.Point(4, 222);
			this.cbShowXML.Name = "cbShowXML";
			this.cbShowXML.Size = new System.Drawing.Size(231, 17);
			this.cbShowXML.TabIndex = 7;
			this.cbShowXML.Text = "Show XML tab";
			this.cbShowXML.UseVisualStyleBackColor = true;
			//
			// frmSettings
			//
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(247, 273);
			this.Controls.Add(this.cbShowXML);
			this.Controls.Add(this.lbMaxThreadCount);
			this.Controls.Add(this.udThreadLimit);
			this.Controls.Add(this.cbCreateTabsForConnection);
			this.Controls.Add(this.cbShowSingleConnectionTab);
			this.Controls.Add(this.cbShowCloseTabConfirm);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.udTimeout);
			this.Controls.Add(this.lbTimeout);
			this.Controls.Add(this.lbReportLang);
			this.Controls.Add(this.cmbReportLang);
			this.Controls.Add(this.cmbInterfaceLang);
			this.Controls.Add(this.lbInterfaceLang);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSettings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Settings";
			((System.ComponentModel.ISupportInitialize)(this.systemSettingsInfoBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.settingsInfoBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udTimeout)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udThreadLimit)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbReportLang;
        private System.Windows.Forms.ComboBox cmbReportLang;
        private System.Windows.Forms.ComboBox cmbInterfaceLang;
        private System.Windows.Forms.Label lbInterfaceLang;
        private System.Windows.Forms.Label lbTimeout;
		private System.Windows.Forms.NumericUpDown udTimeout;
        private System.Windows.Forms.BindingSource systemSettingsInfoBindingSource;
        private System.Windows.Forms.BindingSource settingsInfoBindingSource;
        private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox cbShowCloseTabConfirm;
        private System.Windows.Forms.CheckBox cbShowSingleConnectionTab;
        private System.Windows.Forms.CheckBox cbCreateTabsForConnection;
        private System.Windows.Forms.NumericUpDown udThreadLimit;
        private System.Windows.Forms.Label lbMaxThreadCount;
		private System.Windows.Forms.CheckBox cbShowXML;
    }
}