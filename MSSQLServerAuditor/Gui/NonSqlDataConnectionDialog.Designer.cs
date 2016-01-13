namespace MSSQLServerAuditor.Gui
{
	partial class NonSqlDataConnectionDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NonSqlDataConnectionDialog));
			this.tbConnectionString = new System.Windows.Forms.TextBox();
			this.gbConnectionString = new System.Windows.Forms.GroupBox();
			this.btCancel = new System.Windows.Forms.Button();
			this.btOk = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.gbConnectionString.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			//
			// tbConnectionString
			//
			this.tbConnectionString.AllowDrop = true;
			this.tbConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbConnectionString.Location = new System.Drawing.Point(12, 21);
			this.tbConnectionString.Name = "tbConnectionString";
			this.tbConnectionString.Size = new System.Drawing.Size(407, 20);
			this.tbConnectionString.TabIndex = 0;
			//
			// gbConnectionString
			//
			this.gbConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbConnectionString.Controls.Add(this.tbConnectionString);
			this.gbConnectionString.Location = new System.Drawing.Point(0, 2);
			this.gbConnectionString.Name = "gbConnectionString";
			this.gbConnectionString.Size = new System.Drawing.Size(425, 54);
			this.gbConnectionString.TabIndex = 4;
			this.gbConnectionString.TabStop = false;
			this.gbConnectionString.Tag = "";
			this.gbConnectionString.Text = "Server name:";
			//
			// btCancel
			//
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(344, 1);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(75, 23);
			this.btCancel.TabIndex = 2;
			this.btCancel.Text = "Cancel";
			this.btCancel.UseVisualStyleBackColor = true;
			//
			// btOk
			//
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.Location = new System.Drawing.Point(263, 1);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(75, 23);
			this.btOk.TabIndex = 1;
			this.btOk.Text = "OK";
			this.btOk.UseVisualStyleBackColor = true;
			//
			// panel1
			//
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.btOk);
			this.panel1.Controls.Add(this.btCancel);
			this.panel1.Location = new System.Drawing.Point(0, 61);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(425, 31);
			this.panel1.TabIndex = 5;
			//
			// NonSqlDataConnectionDialog
			//
			this.AcceptButton = this.btOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(427, 91);
			this.Controls.Add(this.gbConnectionString);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NonSqlDataConnectionDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connection properties";
			this.gbConnectionString.ResumeLayout(false);
			this.gbConnectionString.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox tbConnectionString;
		private System.Windows.Forms.GroupBox gbConnectionString;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.Button btOk;
		private System.Windows.Forms.Panel panel1;
	}
}
