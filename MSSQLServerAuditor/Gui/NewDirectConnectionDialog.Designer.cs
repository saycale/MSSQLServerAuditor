namespace MSSQLServerAuditor.Gui
{
	partial class NewDirectConnectionDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDirectConnectionDialog));
			this.panel1 = new System.Windows.Forms.Panel();
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.moduleType = new System.Windows.Forms.GroupBox();
			this.cbModuleType = new System.Windows.Forms.ComboBox();
			this.gbDataBaseType = new System.Windows.Forms.GroupBox();
			this.cbDataBaseType = new System.Windows.Forms.ComboBox();
			this.dataBaseTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.gbTemplate = new System.Windows.Forms.GroupBox();
			this.cbPathToFile = new System.Windows.Forms.ComboBox();
			this.rbOpenTemplateFromFile = new System.Windows.Forms.RadioButton();
			this.rbSelectExistTemplate = new System.Windows.Forms.RadioButton();
			this.btnOpenTemplate = new System.Windows.Forms.Button();
			this.cbTemplate = new System.Windows.Forms.ComboBox();
			this.templateFileSettingBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.gbServerInfo = new System.Windows.Forms.GroupBox();
			this.tbGroupName = new System.Windows.Forms.TextBox();
			this.removeConnectionStringButton = new System.Windows.Forms.Button();
			this.cbConnection = new System.Windows.Forms.ComboBox();
			this.addConnectionStringButton = new System.Windows.Forms.Button();
			this.btSelectConnection = new System.Windows.Forms.Button();
			this.connectionStringsListBox = new System.Windows.Forms.ListBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.moduleType.SuspendLayout();
			this.gbDataBaseType.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataBaseTypeBindingSource)).BeginInit();
			this.gbTemplate.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.templateFileSettingBindingSource)).BeginInit();
			this.gbServerInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			//
			// panel1
			//
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.btOk);
			this.panel1.Controls.Add(this.btCancel);
			this.panel1.Location = new System.Drawing.Point(0, 452);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(619, 31);
			this.panel1.TabIndex = 1;
			//
			// btOk
			//
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.Location = new System.Drawing.Point(451, 3);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(75, 24);
			this.btOk.TabIndex = 0;
			this.btOk.Text = "OK";
			this.btOk.UseVisualStyleBackColor = true;
			this.btOk.Click += new System.EventHandler(this.BtOkClick);
			//
			// btCancel
			//
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(537, 2);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(75, 24);
			this.btCancel.TabIndex = 1;
			this.btCancel.Text = "Cancel";
			this.btCancel.UseVisualStyleBackColor = true;
			//
			// panel2
			//
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.moduleType);
			this.panel2.Controls.Add(this.gbDataBaseType);
			this.panel2.Controls.Add(this.gbTemplate);
			this.panel2.Controls.Add(this.gbServerInfo);
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(619, 452);
			this.panel2.TabIndex = 0;
			//
			// moduleType
			//
			this.moduleType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.moduleType.Controls.Add(this.cbModuleType);
			this.moduleType.Location = new System.Drawing.Point(3, 261);
			this.moduleType.Name = "moduleType";
			this.moduleType.Size = new System.Drawing.Size(613, 57);
			this.moduleType.TabIndex = 2;
			this.moduleType.TabStop = false;
			this.moduleType.Text = "Module type";
			//
			// cbModuleType
			//
			this.cbModuleType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbModuleType.DisplayMember = "Key";
			this.cbModuleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbModuleType.Enabled = false;
			this.cbModuleType.FormattingEnabled = true;
			this.cbModuleType.Location = new System.Drawing.Point(10, 19);
			this.cbModuleType.Name = "cbModuleType";
			this.cbModuleType.Size = new System.Drawing.Size(538, 21);
			this.cbModuleType.TabIndex = 0;
			this.cbModuleType.ValueMember = "Value";
			this.cbModuleType.SelectedIndexChanged += new System.EventHandler(this.cbModuleType_SelectedIndexChanged);
			//
			// gbDataBaseType
			//
			this.gbDataBaseType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbDataBaseType.Controls.Add(this.cbDataBaseType);
			this.gbDataBaseType.Location = new System.Drawing.Point(3, 8);
			this.gbDataBaseType.Name = "gbDataBaseType";
			this.gbDataBaseType.Size = new System.Drawing.Size(613, 54);
			this.gbDataBaseType.TabIndex = 0;
			this.gbDataBaseType.TabStop = false;
			this.gbDataBaseType.Text = "Data Source type";
			//
			// cbDataBaseType
			//
			this.cbDataBaseType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbDataBaseType.DataSource = this.dataBaseTypeBindingSource;
			this.cbDataBaseType.DisplayMember = "Name";
			this.cbDataBaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataBaseType.FormattingEnabled = true;
			this.cbDataBaseType.Location = new System.Drawing.Point(6, 20);
			this.cbDataBaseType.Name = "cbDataBaseType";
			this.cbDataBaseType.Size = new System.Drawing.Size(542, 21);
			this.cbDataBaseType.TabIndex = 0;
			this.cbDataBaseType.ValueMember = "Id";
			this.cbDataBaseType.SelectedIndexChanged += new System.EventHandler(this.cbDataBaseType_SelectedIndexChanged);
			//
			// dataBaseTypeBindingSource
			//
			this.dataBaseTypeBindingSource.DataSource = typeof(MSSQLServerAuditor.Gui.NewDirectConnectionDialog.DBType);
			//
			// gbTemplate
			//
			this.gbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbTemplate.Controls.Add(this.cbPathToFile);
			this.gbTemplate.Controls.Add(this.rbOpenTemplateFromFile);
			this.gbTemplate.Controls.Add(this.rbSelectExistTemplate);
			this.gbTemplate.Controls.Add(this.btnOpenTemplate);
			this.gbTemplate.Controls.Add(this.cbTemplate);
			this.gbTemplate.Location = new System.Drawing.Point(3, 324);
			this.gbTemplate.Name = "gbTemplate";
			this.gbTemplate.Size = new System.Drawing.Size(613, 124);
			this.gbTemplate.TabIndex = 3;
			this.gbTemplate.TabStop = false;
			this.gbTemplate.Text = "Template";
			//
			// cbPathToFile
			//
			this.cbPathToFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbPathToFile.Enabled = false;
			this.cbPathToFile.FormattingEnabled = true;
			this.cbPathToFile.Location = new System.Drawing.Point(27, 90);
			this.cbPathToFile.Name = "cbPathToFile";
			this.cbPathToFile.Size = new System.Drawing.Size(521, 21);
			this.cbPathToFile.TabIndex = 3;
			//
			// rbOpenTemplateFromFile
			//
			this.rbOpenTemplateFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rbOpenTemplateFromFile.AutoSize = true;
			this.rbOpenTemplateFromFile.Location = new System.Drawing.Point(10, 69);
			this.rbOpenTemplateFromFile.Name = "rbOpenTemplateFromFile";
			this.rbOpenTemplateFromFile.Size = new System.Drawing.Size(108, 17);
			this.rbOpenTemplateFromFile.TabIndex = 2;
			this.rbOpenTemplateFromFile.Text = "Open from the file";
			this.rbOpenTemplateFromFile.UseVisualStyleBackColor = true;
			this.rbOpenTemplateFromFile.CheckedChanged += new System.EventHandler(this.rbOpenTemplateFromFile_CheckedChanged);
			//
			// rbSelectExistTemplate
			//
			this.rbSelectExistTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rbSelectExistTemplate.AutoSize = true;
			this.rbSelectExistTemplate.Checked = true;
			this.rbSelectExistTemplate.Location = new System.Drawing.Point(10, 19);
			this.rbSelectExistTemplate.Name = "rbSelectExistTemplate";
			this.rbSelectExistTemplate.Size = new System.Drawing.Size(116, 17);
			this.rbSelectExistTemplate.TabIndex = 0;
			this.rbSelectExistTemplate.TabStop = true;
			this.rbSelectExistTemplate.Text = "Choose the module";
			this.rbSelectExistTemplate.UseVisualStyleBackColor = true;
			this.rbSelectExistTemplate.CheckedChanged += new System.EventHandler(this.rbSelectExistTemplate_CheckedChanged);
			//
			// btnOpenTemplate
			//
			this.btnOpenTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenTemplate.Enabled = false;
			this.btnOpenTemplate.Location = new System.Drawing.Point(550, 90);
			this.btnOpenTemplate.Name = "btnOpenTemplate";
			this.btnOpenTemplate.Size = new System.Drawing.Size(60, 22);
			this.btnOpenTemplate.TabIndex = 4;
			this.btnOpenTemplate.Text = "...";
			this.btnOpenTemplate.UseVisualStyleBackColor = true;
			this.btnOpenTemplate.Click += new System.EventHandler(this.btnOpenTemplate_Click);
			//
			// cbTemplate
			//
			this.cbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbTemplate.DataSource = this.templateFileSettingBindingSource;
			this.cbTemplate.DisplayMember = "Display";
			this.cbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTemplate.FormattingEnabled = true;
			this.cbTemplate.Location = new System.Drawing.Point(27, 42);
			this.cbTemplate.Name = "cbTemplate";
			this.cbTemplate.Size = new System.Drawing.Size(521, 21);
			this.cbTemplate.TabIndex = 1;
			this.cbTemplate.ValueMember = "Id";
			//
			// templateFileSettingBindingSource
			//
			this.templateFileSettingBindingSource.DataSource = typeof(MSSQLServerAuditor.Model.Template);
			//
			// gbServerInfo
			//
			this.gbServerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbServerInfo.Controls.Add(this.tbGroupName);
			this.gbServerInfo.Controls.Add(this.removeConnectionStringButton);
			this.gbServerInfo.Controls.Add(this.cbConnection);
			this.gbServerInfo.Controls.Add(this.addConnectionStringButton);
			this.gbServerInfo.Controls.Add(this.btSelectConnection);
			this.gbServerInfo.Controls.Add(this.connectionStringsListBox);
			this.gbServerInfo.Location = new System.Drawing.Point(3, 68);
			this.gbServerInfo.Name = "gbServerInfo";
			this.gbServerInfo.Size = new System.Drawing.Size(613, 187);
			this.gbServerInfo.TabIndex = 1;
			this.gbServerInfo.TabStop = false;
			this.gbServerInfo.Text = "Instance information";
			//
			// tbGroupName
			//
			this.tbGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGroupName.Location = new System.Drawing.Point(6, 161);
			this.tbGroupName.Name = "tbGroupName";
			this.tbGroupName.Size = new System.Drawing.Size(542, 20);
			this.tbGroupName.TabIndex = 5;
			this.tbGroupName.TextChanged += new System.EventHandler(this.tbGroupName_TextChanged);
			this.tbGroupName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbGroupName_KeyDown);
			//
			// removeConnectionStringButton
			//
			this.removeConnectionStringButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.removeConnectionStringButton.Image = global::MSSQLServerAuditor.Properties.Resources.minus;
			this.removeConnectionStringButton.Location = new System.Drawing.Point(550, 76);
			this.removeConnectionStringButton.Name = "removeConnectionStringButton";
			this.removeConnectionStringButton.Size = new System.Drawing.Size(60, 22);
			this.removeConnectionStringButton.TabIndex = 4;
			this.removeConnectionStringButton.UseVisualStyleBackColor = true;
			this.removeConnectionStringButton.Click += new System.EventHandler(this.removeTemplateButton_Click);
			//
			// cbConnection
			//
			this.cbConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbConnection.DisplayMember = "ConnectionString";
			this.cbConnection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbConnection.FormattingEnabled = true;
			this.cbConnection.Location = new System.Drawing.Point(6, 18);
			this.cbConnection.Name = "cbConnection";
			this.cbConnection.Size = new System.Drawing.Size(542, 21);
			this.cbConnection.TabIndex = 0;
			this.cbConnection.SelectedIndexChanged += new System.EventHandler(this.cbConnection_SelectedIndexChanged);
			//
			// addConnectionStringButton
			//
			this.addConnectionStringButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.addConnectionStringButton.Image = global::MSSQLServerAuditor.Properties.Resources.plus;
			this.addConnectionStringButton.Location = new System.Drawing.Point(550, 48);
			this.addConnectionStringButton.Name = "addConnectionStringButton";
			this.addConnectionStringButton.Size = new System.Drawing.Size(60, 22);
			this.addConnectionStringButton.TabIndex = 3;
			this.addConnectionStringButton.UseVisualStyleBackColor = true;
			this.addConnectionStringButton.Click += new System.EventHandler(this.addConnectionStringButton_Click);
			//
			// btSelectConnection
			//
			this.btSelectConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btSelectConnection.Location = new System.Drawing.Point(550, 16);
			this.btSelectConnection.Name = "btSelectConnection";
			this.btSelectConnection.Size = new System.Drawing.Size(60, 22);
			this.btSelectConnection.TabIndex = 1;
			this.btSelectConnection.Text = "Add...";
			this.btSelectConnection.UseVisualStyleBackColor = true;
			this.btSelectConnection.Click += new System.EventHandler(this.BtSelectConnectionClick);
			//
			// connectionStringsListBox
			//
			this.connectionStringsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.connectionStringsListBox.DisplayMember = "ConnectionString";
			this.connectionStringsListBox.FormattingEnabled = true;
			this.connectionStringsListBox.Location = new System.Drawing.Point(6, 48);
			this.connectionStringsListBox.Name = "connectionStringsListBox";
			this.connectionStringsListBox.Size = new System.Drawing.Size(542, 108);
			this.connectionStringsListBox.TabIndex = 2;
			this.connectionStringsListBox.SelectedIndexChanged += new System.EventHandler(this.connectionStringsListBox_SelectedIndexChanged);
			//
			// errorProvider
			//
			this.errorProvider.ContainerControl = this;
			//
			// NewDirectConnectionDialog
			//
			this.AcceptButton = this.btOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(619, 483);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "NewDirectConnectionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Connection";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.moduleType.ResumeLayout(false);
			this.gbDataBaseType.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataBaseTypeBindingSource)).EndInit();
			this.gbTemplate.ResumeLayout(false);
			this.gbTemplate.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.templateFileSettingBindingSource)).EndInit();
			this.gbServerInfo.ResumeLayout(false);
			this.gbServerInfo.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btOk;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox gbServerInfo;
		private System.Windows.Forms.Button btSelectConnection;
		private System.Windows.Forms.GroupBox gbTemplate;
		private System.Windows.Forms.ComboBox cbTemplate;
		private System.Windows.Forms.BindingSource templateFileSettingBindingSource;
		private System.Windows.Forms.BindingSource dataBaseTypeBindingSource;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.ComboBox cbConnection;
		private System.Windows.Forms.GroupBox gbDataBaseType;
		private System.Windows.Forms.ComboBox cbDataBaseType;
		private System.Windows.Forms.Button removeConnectionStringButton;
		private System.Windows.Forms.Button addConnectionStringButton;
		private System.Windows.Forms.ListBox connectionStringsListBox;
		private System.Windows.Forms.TextBox tbGroupName;
		private System.Windows.Forms.Button btnOpenTemplate;
		private System.Windows.Forms.RadioButton rbOpenTemplateFromFile;
		private System.Windows.Forms.RadioButton rbSelectExistTemplate;
		private System.Windows.Forms.ComboBox cbPathToFile;
		private System.Windows.Forms.GroupBox moduleType;
		private System.Windows.Forms.ComboBox cbModuleType;
	}
}
