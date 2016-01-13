namespace MSSQLServerAuditor.Gui
{
	partial class InternalSqliteConnectionDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InternalSqliteConnectionDialog));
			this.dataTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cmbDataType = new System.Windows.Forms.ComboBox();
			this.lblDataType = new System.Windows.Forms.Label();
			this.lblServerInstance = new System.Windows.Forms.Label();
			this.lblConnectionGroup = new System.Windows.Forms.Label();
			this.cmbServerInstance = new System.Windows.Forms.ComboBox();
			this.cmbConnectionGroup = new System.Windows.Forms.ComboBox();
			this.lblTemplate = new System.Windows.Forms.Label();
			this.cmbTemplate = new System.Windows.Forms.ComboBox();
			this.cmbLogin = new System.Windows.Forms.ComboBox();
			this.lblLogin = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dataTypeBindingSource)).BeginInit();
			this.SuspendLayout();
			//
			// btnOk
			//
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(206, 231);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			//
			// btnCancel
			//
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(287, 231);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			// cmbDataType
			//
			this.cmbDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbDataType.DataSource = this.dataTypeBindingSource;
			this.cmbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDataType.FormattingEnabled = true;
			this.cmbDataType.Location = new System.Drawing.Point(12, 26);
			this.cmbDataType.Name = "cmbDataType";
			this.cmbDataType.Size = new System.Drawing.Size(350, 21);
			this.cmbDataType.TabIndex = 7;
			//
			// lblDataType
			//
			this.lblDataType.AutoSize = true;
			this.lblDataType.Location = new System.Drawing.Point(9, 9);
			this.lblDataType.Name = "lblDataType";
			this.lblDataType.Size = new System.Drawing.Size(91, 13);
			this.lblDataType.TabIndex = 6;
			this.lblDataType.Text = "Data source type:";
			//
			// lblServerInstance
			//
			this.lblServerInstance.AutoSize = true;
			this.lblServerInstance.Location = new System.Drawing.Point(9, 90);
			this.lblServerInstance.Name = "lblServerInstance";
			this.lblServerInstance.Size = new System.Drawing.Size(84, 13);
			this.lblServerInstance.TabIndex = 3;
			this.lblServerInstance.Text = "Server instance:";
			//
			// lblConnectionGroup
			//
			this.lblConnectionGroup.AutoSize = true;
			this.lblConnectionGroup.Location = new System.Drawing.Point(9, 50);
			this.lblConnectionGroup.Name = "lblConnectionGroup";
			this.lblConnectionGroup.Size = new System.Drawing.Size(94, 13);
			this.lblConnectionGroup.TabIndex = 2;
			this.lblConnectionGroup.Text = "Connection group:";
			//
			// cmbServerInstance
			//
			this.cmbServerInstance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbServerInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbServerInstance.FormattingEnabled = true;
			this.cmbServerInstance.Location = new System.Drawing.Point(12, 106);
			this.cmbServerInstance.Name = "cmbServerInstance";
			this.cmbServerInstance.Size = new System.Drawing.Size(350, 21);
			this.cmbServerInstance.TabIndex = 1;
			//
			// cmbConnectionGroup
			//
			this.cmbConnectionGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbConnectionGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbConnectionGroup.FormattingEnabled = true;
			this.cmbConnectionGroup.Location = new System.Drawing.Point(12, 66);
			this.cmbConnectionGroup.Name = "cmbConnectionGroup";
			this.cmbConnectionGroup.Size = new System.Drawing.Size(350, 21);
			this.cmbConnectionGroup.TabIndex = 0;
			//
			// lblTemplate
			//
			this.lblTemplate.AutoSize = true;
			this.lblTemplate.Location = new System.Drawing.Point(9, 130);
			this.lblTemplate.Name = "lblTemplate";
			this.lblTemplate.Size = new System.Drawing.Size(54, 13);
			this.lblTemplate.TabIndex = 9;
			this.lblTemplate.Text = "Template:";
			//
			// cmbTemplate
			//
			this.cmbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTemplate.FormattingEnabled = true;
			this.cmbTemplate.Location = new System.Drawing.Point(12, 146);
			this.cmbTemplate.Name = "cmbTemplate";
			this.cmbTemplate.Size = new System.Drawing.Size(350, 21);
			this.cmbTemplate.TabIndex = 8;
			//
			// cmbLogin
			//
			this.cmbLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbLogin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLogin.FormattingEnabled = true;
			this.cmbLogin.Location = new System.Drawing.Point(12, 186);
			this.cmbLogin.Name = "cmbLogin";
			this.cmbLogin.Size = new System.Drawing.Size(350, 21);
			this.cmbLogin.TabIndex = 8;
			//
			// lblLogin
			//
			this.lblLogin.AutoSize = true;
			this.lblLogin.Location = new System.Drawing.Point(9, 170);
			this.lblLogin.Name = "lblLogin";
			this.lblLogin.Size = new System.Drawing.Size(36, 13);
			this.lblLogin.TabIndex = 9;
			this.lblLogin.Text = "Login:";
			//
			// InternalSqliteConnectionDialog
			//
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(373, 278);
			this.Controls.Add(this.lblLogin);
			this.Controls.Add(this.lblTemplate);
			this.Controls.Add(this.cmbLogin);
			this.Controls.Add(this.cmbTemplate);
			this.Controls.Add(this.cmbDataType);
			this.Controls.Add(this.lblDataType);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblServerInstance);
			this.Controls.Add(this.lblConnectionGroup);
			this.Controls.Add(this.cmbServerInstance);
			this.Controls.Add(this.cmbConnectionGroup);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(300, 305);
			this.Name = "InternalSqliteConnectionDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Internal SQLite connection";
			((System.ComponentModel.ISupportInitialize)(this.dataTypeBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cmbConnectionGroup;
		private System.Windows.Forms.ComboBox cmbServerInstance;
		private System.Windows.Forms.Label lblConnectionGroup;
		private System.Windows.Forms.Label lblServerInstance;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblDataType;
		private System.Windows.Forms.ComboBox cmbDataType;
		private System.Windows.Forms.BindingSource dataTypeBindingSource;
		private System.Windows.Forms.Label lblTemplate;
		private System.Windows.Forms.ComboBox cmbTemplate;
		private System.Windows.Forms.ComboBox cmbLogin;
		private System.Windows.Forms.Label lblLogin;

	}
}
