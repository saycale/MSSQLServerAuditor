namespace MSSQLServerAuditor.Licenser.Gui
{
    partial class FolderSelector
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
            this.components = new System.ComponentModel.Container();
            this.txtDstFolder = new System.Windows.Forms.TextBox();
            this.selectDstFolder = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            //
            // txtDstFolder
            //
            this.txtDstFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDstFolder.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.errorProvider.SetIconAlignment(this.txtDstFolder, System.Windows.Forms.ErrorIconAlignment.TopRight);
            this.txtDstFolder.Location = new System.Drawing.Point(2, 1);
            this.txtDstFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDstFolder.Name = "txtDstFolder";
            this.txtDstFolder.Size = new System.Drawing.Size(259, 20);
            this.txtDstFolder.TabIndex = 26;
            this.txtDstFolder.TextChanged += new System.EventHandler(this.txtDstFolder_TextChanged);
            //
            // selectDstFolder
            //
            this.selectDstFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectDstFolder.Location = new System.Drawing.Point(265, 0);
            this.selectDstFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.selectDstFolder.Name = "selectDstFolder";
            this.selectDstFolder.Size = new System.Drawing.Size(26, 22);
            this.selectDstFolder.TabIndex = 27;
            this.selectDstFolder.Text = "...";
            this.selectDstFolder.UseVisualStyleBackColor = true;
            this.selectDstFolder.Click += new System.EventHandler(this.selectDstFolder_Click);
            //
            // errorProvider
            //
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            //
            // FolderSelector
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtDstFolder);
            this.Controls.Add(this.selectDstFolder);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FolderSelector";
            this.Size = new System.Drawing.Size(293, 22);
            this.Load += new System.EventHandler(this.FolderSelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDstFolder;
        private System.Windows.Forms.Button selectDstFolder;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
