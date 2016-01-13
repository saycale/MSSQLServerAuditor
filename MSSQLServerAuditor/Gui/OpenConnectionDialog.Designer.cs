namespace MSSQLServerAuditor.Gui
{
	partial class OpenConnectionDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenConnectionDialog));
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.gbDataBaseType = new System.Windows.Forms.GroupBox();
			this.cbDataBaseType = new System.Windows.Forms.ComboBox();
			this.gbTemplate = new System.Windows.Forms.GroupBox();
			this.cbTemplate = new System.Windows.Forms.ComboBox();
			this.templateFileSettingBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.gbServerInfo = new System.Windows.Forms.GroupBox();
			this.cbConnection = new System.Windows.Forms.ComboBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.dataBaseTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.gbDataBaseType.SuspendLayout();
			this.gbTemplate.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.templateFileSettingBindingSource)).BeginInit();
			this.gbServerInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataBaseTypeBindingSource)).BeginInit();
			this.SuspendLayout();
			//
			// panel1
			//
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.btOk);
			this.panel1.Controls.Add(this.btCancel);
			this.panel1.Location = new System.Drawing.Point(0, 230);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(619, 31);
			this.panel1.TabIndex = 0;
			//
			// label1
			//
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(608, 2);
			this.label1.TabIndex = 1;
			//
			// btOk
			//
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.Location = new System.Drawing.Point(439, 3);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(75, 23);
			this.btOk.TabIndex = 1;
			this.btOk.Text = "OK";
			this.btOk.UseVisualStyleBackColor = true;
			this.btOk.Click += new System.EventHandler(this.BtOkClick);
			//
			// btCancel
			//
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(520, 3);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(75, 23);
			this.btCancel.TabIndex = 0;
			this.btCancel.Text = "Cancel";
			this.btCancel.UseVisualStyleBackColor = true;
			//
			// panel2
			//
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.gbDataBaseType);
			this.panel2.Controls.Add(this.gbTemplate);
			this.panel2.Controls.Add(this.gbServerInfo);
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(619, 230);
			this.panel2.TabIndex = 1;
			//
			// gbDataBaseType
			//
			this.gbDataBaseType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbDataBaseType.Controls.Add(this.cbDataBaseType);
			this.gbDataBaseType.Location = new System.Drawing.Point(3, 58);
			this.gbDataBaseType.Name = "gbDataBaseType";
			this.gbDataBaseType.Size = new System.Drawing.Size(613, 54);
			this.gbDataBaseType.TabIndex = 3;
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
			this.cbDataBaseType.Location = new System.Drawing.Point(10, 20);
			this.cbDataBaseType.Name = "cbDataBaseType";
			this.cbDataBaseType.Size = new System.Drawing.Size(582, 21);
			this.cbDataBaseType.TabIndex = 0;
			this.cbDataBaseType.ValueMember = "Id";
			this.cbDataBaseType.SelectedIndexChanged += new System.EventHandler(this.cbDataBaseType_SelectedIndexChanged);
			//
			// gbTemplate
			//
			this.gbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbTemplate.Controls.Add(this.cbTemplate);
			this.gbTemplate.Location = new System.Drawing.Point(3, 118);
			this.gbTemplate.Name = "gbTemplate";
			this.gbTemplate.Size = new System.Drawing.Size(613, 54);
			this.gbTemplate.TabIndex = 1;
			this.gbTemplate.TabStop = false;
			this.gbTemplate.Text = "Template";
			//
			// cbTemplate
			//
			this.cbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbTemplate.DataSource = this.templateFileSettingBindingSource;
			this.cbTemplate.DisplayMember = "Display";
			this.cbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTemplate.FormattingEnabled = true;
			this.cbTemplate.Location = new System.Drawing.Point(10, 20);
			this.cbTemplate.Name = "cbTemplate";
			this.cbTemplate.Size = new System.Drawing.Size(582, 21);
			this.cbTemplate.TabIndex = 0;
			this.cbTemplate.ValueMember = "Value";
			//
			// templateFileSettingBindingSource
			//
			this.templateFileSettingBindingSource.DataSource = typeof(MSSQLServerAuditor.Model.Template);
			//
			// gbServerInfo
			//
			this.gbServerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbServerInfo.Controls.Add(this.cbConnection);
			this.gbServerInfo.Location = new System.Drawing.Point(3, 3);
			this.gbServerInfo.Name = "gbServerInfo";
			this.gbServerInfo.Size = new System.Drawing.Size(613, 49);
			this.gbServerInfo.TabIndex = 0;
			this.gbServerInfo.TabStop = false;
			this.gbServerInfo.Text = "Instance information";
			//
			// cbConnection
			//
			this.cbConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbConnection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbConnection.FormattingEnabled = true;
			this.cbConnection.Location = new System.Drawing.Point(10, 18);
			this.cbConnection.Name = "cbConnection";
			this.cbConnection.Size = new System.Drawing.Size(582, 21);
			this.cbConnection.TabIndex = 2;
			//
			// errorProvider
			//
			this.errorProvider.ContainerControl = this;
			//
			// dataBaseTypeBindingSource
			//
			this.dataBaseTypeBindingSource.DataSource = typeof(MSSQLServerAuditor.Gui.OpenConnectionDialog.DBType);
			//
			// OpenConnectionDialog
			//
			this.AcceptButton = this.btOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(619, 261);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "OpenConnectionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New connection";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.gbDataBaseType.ResumeLayout(false);
			this.gbTemplate.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.templateFileSettingBindingSource)).EndInit();
			this.gbServerInfo.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataBaseTypeBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btOk;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox gbServerInfo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox gbTemplate;
		private System.Windows.Forms.ComboBox cbTemplate;
		private System.Windows.Forms.BindingSource templateFileSettingBindingSource;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.ComboBox cbConnection;
		private System.Windows.Forms.GroupBox gbDataBaseType;
		private System.Windows.Forms.ComboBox cbDataBaseType;
		private System.Windows.Forms.BindingSource dataBaseTypeBindingSource;
	}
}
