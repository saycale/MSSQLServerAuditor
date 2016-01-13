namespace MSSQLServerAuditor.Gui
{
    partial class frmSaveDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSaveDialog));
			this.gbScope = new System.Windows.Forms.GroupBox();
			this.lbCurScopeName = new System.Windows.Forms.Label();
			this.rbAll = new System.Windows.Forms.RadioButton();
			this.rbCurrent = new System.Windows.Forms.RadioButton();
			this.gbFile = new System.Windows.Forms.GroupBox();
			this.chbCurrent = new System.Windows.Forms.CheckBox();
			this.chbHistoric = new System.Windows.Forms.CheckBox();
			this.lbFolder = new System.Windows.Forms.Label();
			this.txFolderName = new System.Windows.Forms.TextBox();
			this.btBrowse = new System.Windows.Forms.Button();
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.gbScope.SuspendLayout();
			this.gbFile.SuspendLayout();
			this.SuspendLayout();
			//
			// gbScope
			//
			this.gbScope.Controls.Add(this.lbCurScopeName);
			this.gbScope.Controls.Add(this.rbAll);
			this.gbScope.Controls.Add(this.rbCurrent);
			this.gbScope.Location = new System.Drawing.Point(12, 46);
			this.gbScope.Name = "gbScope";
			this.gbScope.Size = new System.Drawing.Size(200, 100);
			this.gbScope.TabIndex = 0;
			this.gbScope.TabStop = false;
			this.gbScope.Text = "Scope";
			//
			// lbCurScopeName
			//
			this.lbCurScopeName.AutoSize = true;
			this.lbCurScopeName.Location = new System.Drawing.Point(114, 33);
			this.lbCurScopeName.Name = "lbCurScopeName";
			this.lbCurScopeName.Size = new System.Drawing.Size(35, 13);
			this.lbCurScopeName.TabIndex = 8;
			this.lbCurScopeName.Text = "label1";
			//
			// rbAll
			//
			this.rbAll.AutoSize = true;
			this.rbAll.Location = new System.Drawing.Point(27, 54);
			this.rbAll.Name = "rbAll";
			this.rbAll.Size = new System.Drawing.Size(36, 17);
			this.rbAll.TabIndex = 2;
			this.rbAll.Text = "All";
			this.rbAll.UseVisualStyleBackColor = true;
			//
			// rbCurrent
			//
			this.rbCurrent.AutoSize = true;
			this.rbCurrent.Checked = true;
			this.rbCurrent.Location = new System.Drawing.Point(27, 31);
			this.rbCurrent.Name = "rbCurrent";
			this.rbCurrent.Size = new System.Drawing.Size(59, 17);
			this.rbCurrent.TabIndex = 1;
			this.rbCurrent.TabStop = true;
			this.rbCurrent.Text = "Current";
			this.rbCurrent.UseVisualStyleBackColor = true;
			//
			// gbFile
			//
			this.gbFile.Controls.Add(this.chbCurrent);
			this.gbFile.Controls.Add(this.chbHistoric);
			this.gbFile.Location = new System.Drawing.Point(218, 46);
			this.gbFile.Name = "gbFile";
			this.gbFile.Size = new System.Drawing.Size(200, 100);
			this.gbFile.TabIndex = 1;
			this.gbFile.TabStop = false;
			this.gbFile.Text = "File";
			//
			// chbCurrent
			//
			this.chbCurrent.AutoSize = true;
			this.chbCurrent.Checked = true;
			this.chbCurrent.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbCurrent.Location = new System.Drawing.Point(30, 32);
			this.chbCurrent.Name = "chbCurrent";
			this.chbCurrent.Size = new System.Drawing.Size(60, 17);
			this.chbCurrent.TabIndex = 4;
			this.chbCurrent.Text = "Current";
			this.chbCurrent.UseVisualStyleBackColor = true;
			this.chbCurrent.CheckedChanged += new System.EventHandler(this.chbCurrent_CheckedChanged);
			//
			// chbHistoric
			//
			this.chbHistoric.AutoSize = true;
			this.chbHistoric.Checked = true;
			this.chbHistoric.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chbHistoric.Location = new System.Drawing.Point(30, 55);
			this.chbHistoric.Name = "chbHistoric";
			this.chbHistoric.Size = new System.Drawing.Size(61, 17);
			this.chbHistoric.TabIndex = 0;
			this.chbHistoric.Text = "Historic";
			this.chbHistoric.UseVisualStyleBackColor = true;
			this.chbHistoric.CheckedChanged += new System.EventHandler(this.chbHistoric_CheckedChanged);
			//
			// lbFolder
			//
			this.lbFolder.AutoSize = true;
			this.lbFolder.Location = new System.Drawing.Point(12, 15);
			this.lbFolder.Name = "lbFolder";
			this.lbFolder.Size = new System.Drawing.Size(39, 13);
			this.lbFolder.TabIndex = 2;
			this.lbFolder.Text = "Folder:";
			//
			// txFolderName
			//
			this.txFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txFolderName.Location = new System.Drawing.Point(78, 12);
			this.txFolderName.Name = "txFolderName";
			this.txFolderName.ReadOnly = true;
			this.txFolderName.Size = new System.Drawing.Size(259, 20);
			this.txFolderName.TabIndex = 3;
			//
			// btBrowse
			//
			this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btBrowse.Location = new System.Drawing.Point(343, 10);
			this.btBrowse.Name = "btBrowse";
			this.btBrowse.Size = new System.Drawing.Size(75, 23);
			this.btBrowse.TabIndex = 4;
			this.btBrowse.Text = "Browse";
			this.btBrowse.UseVisualStyleBackColor = true;
			this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
			//
			// btOk
			//
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.Location = new System.Drawing.Point(265, 162);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(75, 23);
			this.btOk.TabIndex = 6;
			this.btOk.Text = "OK";
			this.btOk.UseVisualStyleBackColor = true;
			//
			// btCancel
			//
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(346, 162);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(75, 23);
			this.btCancel.TabIndex = 5;
			this.btCancel.Text = "Отмена";
			this.btCancel.UseVisualStyleBackColor = true;
			//
			// frmSaveDialog
			//
			this.AcceptButton = this.btOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(433, 197);
			this.Controls.Add(this.btOk);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btBrowse);
			this.Controls.Add(this.txFolderName);
			this.Controls.Add(this.lbFolder);
			this.Controls.Add(this.gbFile);
			this.Controls.Add(this.gbScope);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSaveDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Save";
			this.gbScope.ResumeLayout(false);
			this.gbScope.PerformLayout();
			this.gbFile.ResumeLayout(false);
			this.gbFile.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbScope;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbCurrent;
        private System.Windows.Forms.GroupBox gbFile;
        private System.Windows.Forms.CheckBox chbCurrent;
        private System.Windows.Forms.CheckBox chbHistoric;
        private System.Windows.Forms.Label lbFolder;
        private System.Windows.Forms.TextBox txFolderName;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lbCurScopeName;
    }
}