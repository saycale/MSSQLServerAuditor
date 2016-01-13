namespace MSSQLServerAuditor.Gui
{
    partial class frmAboutLicense
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAboutLicense));
			this.okButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.pbCopyHost = new System.Windows.Forms.Button();
			this.logoPictureBox = new System.Windows.Forms.PictureBox();
			this.txtBHosts = new System.Windows.Forms.TextBox();
			this.linkLblNewLicense = new System.Windows.Forms.LinkLabel();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
			this.SuspendLayout();
			//
			// okButton
			//
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel.SetColumnSpan(this.okButton, 2);
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(355, 125);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 10;
			this.okButton.Text = "&OK";
			//
			// tableLayoutPanel
			//
			this.tableLayoutPanel.ColumnCount = 4;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.Controls.Add(this.pbCopyHost, 3, 0);
			this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.txtBHosts, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.linkLblNewLicense, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.okButton, 2, 2);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 3;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(433, 151);
			this.tableLayoutPanel.TabIndex = 1;
			//
			// pbCopyHost
			//
			this.pbCopyHost.AutoSize = true;
			this.pbCopyHost.Image = global::MSSQLServerAuditor.Properties.Resources.copy_paste;
			this.pbCopyHost.Location = new System.Drawing.Point(406, 3);
			this.pbCopyHost.Name = "pbCopyHost";
			this.pbCopyHost.Size = new System.Drawing.Size(24, 24);
			this.pbCopyHost.TabIndex = 5;
			this.pbCopyHost.UseVisualStyleBackColor = true;
			this.pbCopyHost.Click += new System.EventHandler(this.pbCopyHost_Click);
			//
			// logoPictureBox
			//
			this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
			this.logoPictureBox.Location = new System.Drawing.Point(3, 3);
			this.logoPictureBox.Name = "logoPictureBox";
			this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 3);
			this.logoPictureBox.Size = new System.Drawing.Size(114, 145);
			this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.logoPictureBox.TabIndex = 12;
			this.logoPictureBox.TabStop = false;
			//
			// txtBHosts
			//
			this.txtBHosts.BackColor = System.Drawing.SystemColors.Control;
			this.txtBHosts.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tableLayoutPanel.SetColumnSpan(this.txtBHosts, 2);
			this.txtBHosts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtBHosts.Location = new System.Drawing.Point(126, 3);
			this.txtBHosts.Margin = new System.Windows.Forms.Padding(6, 3, 3, 0);
			this.txtBHosts.MaximumSize = new System.Drawing.Size(0, 34);
			this.txtBHosts.Multiline = true;
			this.txtBHosts.Name = "txtBHosts";
			this.txtBHosts.ReadOnly = true;
			this.txtBHosts.Size = new System.Drawing.Size(274, 34);
			this.txtBHosts.TabIndex = 3;
			this.txtBHosts.Text = "Hosts";
			//
			// linkLblNewLicense
			//
			this.linkLblNewLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.linkLblNewLicense.AutoSize = true;
			this.linkLblNewLicense.Location = new System.Drawing.Point(126, 132);
			this.linkLblNewLicense.Margin = new System.Windows.Forms.Padding(6, 0, 3, 6);
			this.linkLblNewLicense.MaximumSize = new System.Drawing.Size(0, 17);
			this.linkLblNewLicense.Name = "linkLblNewLicense";
			this.linkLblNewLicense.Size = new System.Drawing.Size(142, 13);
			this.linkLblNewLicense.TabIndex = 4;
			this.linkLblNewLicense.TabStop = true;
			this.linkLblNewLicense.Text = "Заказать новую лицензию";
			this.linkLblNewLicense.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.linkLblNewLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblNewLicense_LinkClicked);
			//
			// frmAboutLicense
			//
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.okButton;
			this.ClientSize = new System.Drawing.Size(451, 169);
			this.Controls.Add(this.tableLayoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAboutLicense";
			this.Padding = new System.Windows.Forms.Padding(9);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmAboutLicense";
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.TextBox txtBHosts;
        private System.Windows.Forms.LinkLabel linkLblNewLicense;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button pbCopyHost;
    }
}