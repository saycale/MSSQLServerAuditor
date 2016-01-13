using System;

namespace MSSQLServerAuditor.Gui
{
	partial class DirectConnectionDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectConnectionDialog));
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pnlConnection = new System.Windows.Forms.Panel();
			this.grpModuleTypes = new System.Windows.Forms.GroupBox();
			this.cmbModuleTypes = new System.Windows.Forms.ComboBox();
			this.bsModuleType = new System.Windows.Forms.BindingSource(this.components);
			this.grpDataBaseType = new System.Windows.Forms.GroupBox();
			this.cmbDataBaseType = new System.Windows.Forms.ComboBox();
			this.bsConnectionType = new System.Windows.Forms.BindingSource(this.components);
			this.grpTemplate = new System.Windows.Forms.GroupBox();
			this.cmbPathToFile = new System.Windows.Forms.ComboBox();
			this.optOpenTemplateFromFile = new System.Windows.Forms.RadioButton();
			this.optSelectExistTemplate = new System.Windows.Forms.RadioButton();
			this.btnOpenTemplate = new System.Windows.Forms.Button();
			this.cmbTemplate = new System.Windows.Forms.ComboBox();
			this.bsTemplateFileSetting = new System.Windows.Forms.BindingSource(this.components);
			this.grpServerInfo = new System.Windows.Forms.GroupBox();
			this.txtGroupName = new System.Windows.Forms.TextBox();
			this.btnRemoveConnectionString = new System.Windows.Forms.Button();
			this.cmbConnection = new System.Windows.Forms.ComboBox();
			this.btnAddConnectionString = new System.Windows.Forms.Button();
			this.btnSelectConnection = new System.Windows.Forms.Button();
			this.lstConnectionStrings = new System.Windows.Forms.ListBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.pnlButtons.SuspendLayout();
			this.pnlConnection.SuspendLayout();
			this.grpModuleTypes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bsModuleType)).BeginInit();
			this.grpDataBaseType.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bsConnectionType)).BeginInit();
			this.grpTemplate.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bsTemplateFileSetting)).BeginInit();
			this.grpServerInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			//
			// pnlButtons
			//
			this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlButtons.Controls.Add(this.btnOk);
			this.pnlButtons.Controls.Add(this.btnCancel);
			this.pnlButtons.Location = new System.Drawing.Point(0, 452);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Size = new System.Drawing.Size(619, 31);
			this.pnlButtons.TabIndex = 1;
			//
			// btnOk
			//
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(451, 3);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 24);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			//
			// btnCancel
			//
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(532, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 24);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			// pnlConnection
			//
			this.pnlConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlConnection.Controls.Add(this.grpModuleTypes);
			this.pnlConnection.Controls.Add(this.grpDataBaseType);
			this.pnlConnection.Controls.Add(this.grpTemplate);
			this.pnlConnection.Controls.Add(this.grpServerInfo);
			this.pnlConnection.Location = new System.Drawing.Point(0, 0);
			this.pnlConnection.Name = "pnlConnection";
			this.pnlConnection.Size = new System.Drawing.Size(619, 452);
			this.pnlConnection.TabIndex = 0;
			//
			// grpModuleTypes
			//
			this.grpModuleTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpModuleTypes.Controls.Add(this.cmbModuleTypes);
			this.grpModuleTypes.Location = new System.Drawing.Point(3, 261);
			this.grpModuleTypes.Name = "grpModuleTypes";
			this.grpModuleTypes.Size = new System.Drawing.Size(613, 57);
			this.grpModuleTypes.TabIndex = 2;
			this.grpModuleTypes.TabStop = false;
			this.grpModuleTypes.Text = "Module type";
			//
			// cmbModuleTypes
			//
			this.cmbModuleTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbModuleTypes.DataSource = this.bsModuleType;
			this.cmbModuleTypes.DisplayMember = "Value";
			this.cmbModuleTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbModuleTypes.Enabled = false;
			this.cmbModuleTypes.FormattingEnabled = true;
			this.cmbModuleTypes.Location = new System.Drawing.Point(10, 19);
			this.cmbModuleTypes.Name = "cmbModuleTypes";
			this.cmbModuleTypes.Size = new System.Drawing.Size(538, 21);
			this.cmbModuleTypes.TabIndex = 0;
			this.cmbModuleTypes.ValueMember = "Value";
			//
			// grpDataBaseType
			//
			this.grpDataBaseType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpDataBaseType.Controls.Add(this.cmbDataBaseType);
			this.grpDataBaseType.Location = new System.Drawing.Point(3, 8);
			this.grpDataBaseType.Name = "grpDataBaseType";
			this.grpDataBaseType.Size = new System.Drawing.Size(613, 54);
			this.grpDataBaseType.TabIndex = 0;
			this.grpDataBaseType.TabStop = false;
			this.grpDataBaseType.Text = "Data Source type";
			//
			// cmbDataBaseType
			//
			this.cmbDataBaseType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbDataBaseType.DataSource = this.bsConnectionType;
			this.cmbDataBaseType.DisplayMember = "Type";
			this.cmbDataBaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDataBaseType.FormattingEnabled = true;
			this.cmbDataBaseType.Location = new System.Drawing.Point(6, 20);
			this.cmbDataBaseType.Name = "cmbDataBaseType";
			this.cmbDataBaseType.Size = new System.Drawing.Size(542, 21);
			this.cmbDataBaseType.TabIndex = 0;
			this.cmbDataBaseType.ValueMember = "Type";
			//
			// grpTemplate
			//
			this.grpTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpTemplate.Controls.Add(this.cmbPathToFile);
			this.grpTemplate.Controls.Add(this.optOpenTemplateFromFile);
			this.grpTemplate.Controls.Add(this.optSelectExistTemplate);
			this.grpTemplate.Controls.Add(this.btnOpenTemplate);
			this.grpTemplate.Controls.Add(this.cmbTemplate);
			this.grpTemplate.Location = new System.Drawing.Point(3, 324);
			this.grpTemplate.Name = "grpTemplate";
			this.grpTemplate.Size = new System.Drawing.Size(613, 124);
			this.grpTemplate.TabIndex = 3;
			this.grpTemplate.TabStop = false;
			this.grpTemplate.Text = "Template";
			//
			// cmbPathToFile
			//
			this.cmbPathToFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbPathToFile.Enabled = false;
			this.cmbPathToFile.FormattingEnabled = true;
			this.cmbPathToFile.Location = new System.Drawing.Point(27, 90);
			this.cmbPathToFile.Name = "cmbPathToFile";
			this.cmbPathToFile.Size = new System.Drawing.Size(521, 21);
			this.cmbPathToFile.TabIndex = 3;
			//
			// optOpenTemplateFromFile
			//
			this.optOpenTemplateFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.optOpenTemplateFromFile.AutoSize = true;
			this.optOpenTemplateFromFile.Location = new System.Drawing.Point(10, 69);
			this.optOpenTemplateFromFile.Name = "optOpenTemplateFromFile";
			this.optOpenTemplateFromFile.Size = new System.Drawing.Size(108, 17);
			this.optOpenTemplateFromFile.TabIndex = 2;
			this.optOpenTemplateFromFile.Text = "Open from the file";
			this.optOpenTemplateFromFile.UseVisualStyleBackColor = true;
			//
			// optSelectExistTemplate
			//
			this.optSelectExistTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.optSelectExistTemplate.AutoSize = true;
			this.optSelectExistTemplate.Checked = true;
			this.optSelectExistTemplate.Location = new System.Drawing.Point(10, 19);
			this.optSelectExistTemplate.Name = "optSelectExistTemplate";
			this.optSelectExistTemplate.Size = new System.Drawing.Size(116, 17);
			this.optSelectExistTemplate.TabIndex = 0;
			this.optSelectExistTemplate.TabStop = true;
			this.optSelectExistTemplate.Text = "Choose the module";
			this.optSelectExistTemplate.UseVisualStyleBackColor = true;
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
			//
			// cmbTemplate
			//
			this.cmbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbTemplate.DataSource = this.bsTemplateFileSetting;
			this.cmbTemplate.DisplayMember = "Display";
			this.cmbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTemplate.FormattingEnabled = true;
			this.cmbTemplate.Location = new System.Drawing.Point(27, 42);
			this.cmbTemplate.Name = "cmbTemplate";
			this.cmbTemplate.Size = new System.Drawing.Size(521, 21);
			this.cmbTemplate.TabIndex = 1;
			this.cmbTemplate.ValueMember = "Type";
			//
			// bsTemplateFileSetting
			//
			this.bsTemplateFileSetting.DataSource = typeof(MSSQLServerAuditor.Model.Template);
			//
			// grpServerInfo
			//
			this.grpServerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpServerInfo.Controls.Add(this.txtGroupName);
			this.grpServerInfo.Controls.Add(this.btnRemoveConnectionString);
			this.grpServerInfo.Controls.Add(this.cmbConnection);
			this.grpServerInfo.Controls.Add(this.btnAddConnectionString);
			this.grpServerInfo.Controls.Add(this.btnSelectConnection);
			this.grpServerInfo.Controls.Add(this.lstConnectionStrings);
			this.grpServerInfo.Location = new System.Drawing.Point(3, 68);
			this.grpServerInfo.Name = "grpServerInfo";
			this.grpServerInfo.Size = new System.Drawing.Size(613, 187);
			this.grpServerInfo.TabIndex = 1;
			this.grpServerInfo.TabStop = false;
			this.grpServerInfo.Text = "Instance information";
			//
			// txtGroupName
			//
			this.txtGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGroupName.Location = new System.Drawing.Point(6, 161);
			this.txtGroupName.Name = "txtGroupName";
			this.txtGroupName.Size = new System.Drawing.Size(542, 20);
			this.txtGroupName.TabIndex = 5;
			//
			// btnRemoveConnectionString
			//
			this.btnRemoveConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemoveConnectionString.Image = global::MSSQLServerAuditor.Properties.Resources.minus;
			this.btnRemoveConnectionString.Location = new System.Drawing.Point(550, 76);
			this.btnRemoveConnectionString.Name = "btnRemoveConnectionString";
			this.btnRemoveConnectionString.Size = new System.Drawing.Size(60, 22);
			this.btnRemoveConnectionString.TabIndex = 4;
			this.btnRemoveConnectionString.UseVisualStyleBackColor = true;
			//
			// cmbConnection
			//
			this.cmbConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbConnection.DisplayMember = "ConnectionString";
			this.cmbConnection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbConnection.FormattingEnabled = true;
			this.cmbConnection.Location = new System.Drawing.Point(6, 18);
			this.cmbConnection.Name = "cmbConnection";
			this.cmbConnection.Size = new System.Drawing.Size(542, 21);
			this.cmbConnection.TabIndex = 0;
			//
			// btnAddConnectionString
			//
			this.btnAddConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddConnectionString.Image = global::MSSQLServerAuditor.Properties.Resources.plus;
			this.btnAddConnectionString.Location = new System.Drawing.Point(550, 48);
			this.btnAddConnectionString.Name = "btnAddConnectionString";
			this.btnAddConnectionString.Size = new System.Drawing.Size(60, 22);
			this.btnAddConnectionString.TabIndex = 3;
			this.btnAddConnectionString.UseVisualStyleBackColor = true;
			//
			// btnSelectConnection
			//
			this.btnSelectConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectConnection.Location = new System.Drawing.Point(550, 16);
			this.btnSelectConnection.Name = "btnSelectConnection";
			this.btnSelectConnection.Size = new System.Drawing.Size(60, 22);
			this.btnSelectConnection.TabIndex = 1;
			this.btnSelectConnection.Text = "Add...";
			this.btnSelectConnection.UseVisualStyleBackColor = true;
			//
			// lstConnectionStrings
			//
			this.lstConnectionStrings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstConnectionStrings.DisplayMember = "ConnectionString";
			this.lstConnectionStrings.FormattingEnabled = true;
			this.lstConnectionStrings.Location = new System.Drawing.Point(6, 48);
			this.lstConnectionStrings.Name = "lstConnectionStrings";
			this.lstConnectionStrings.Size = new System.Drawing.Size(542, 108);
			this.lstConnectionStrings.TabIndex = 2;
			//
			// errorProvider
			//
			this.errorProvider.ContainerControl = this;
			//
			// DirectConnectionDialog
			//
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(619, 483);
			this.Controls.Add(this.pnlConnection);
			this.Controls.Add(this.pnlButtons);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DirectConnectionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Connection";
			this.pnlButtons.ResumeLayout(false);
			this.pnlConnection.ResumeLayout(false);
			this.grpModuleTypes.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.bsModuleType)).EndInit();
			this.grpDataBaseType.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.bsConnectionType)).EndInit();
			this.grpTemplate.ResumeLayout(false);
			this.grpTemplate.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.bsTemplateFileSetting)).EndInit();
			this.grpServerInfo.ResumeLayout(false);
			this.grpServerInfo.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.Panel pnlButtons;
		protected System.Windows.Forms.Button btnOk;
		protected System.Windows.Forms.Button btnCancel;
		protected System.Windows.Forms.Panel pnlConnection;
		protected System.Windows.Forms.GroupBox grpServerInfo;
		protected System.Windows.Forms.Button btnSelectConnection;
		protected System.Windows.Forms.GroupBox grpTemplate;
		protected System.Windows.Forms.ComboBox cmbTemplate;
		protected System.Windows.Forms.BindingSource bsTemplateFileSetting;
		protected System.Windows.Forms.ErrorProvider errorProvider;
		protected System.Windows.Forms.ComboBox cmbConnection;
		protected System.Windows.Forms.GroupBox grpDataBaseType;
		protected System.Windows.Forms.ComboBox cmbDataBaseType;
		protected System.Windows.Forms.Button btnAddConnectionString;
		protected System.Windows.Forms.ListBox lstConnectionStrings;
		protected System.Windows.Forms.TextBox txtGroupName;
		protected System.Windows.Forms.Button btnOpenTemplate;
		protected System.Windows.Forms.RadioButton optOpenTemplateFromFile;
		protected System.Windows.Forms.RadioButton optSelectExistTemplate;
		protected System.Windows.Forms.ComboBox cmbPathToFile;
		protected System.Windows.Forms.GroupBox grpModuleTypes;
		protected System.Windows.Forms.ComboBox cmbModuleTypes;
		protected System.Windows.Forms.Button btnRemoveConnectionString;
		protected System.Windows.Forms.BindingSource bsConnectionType;
		protected System.Windows.Forms.BindingSource bsModuleType;
	}
}
