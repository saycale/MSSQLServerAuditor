namespace MSSQLServerAuditor.Licenser.Gui
{
    partial class FileSelector
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
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.selectFileName = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            //
            // txtFileName
            //
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.errorProvider.SetIconAlignment(this.txtFileName, System.Windows.Forms.ErrorIconAlignment.TopRight);
            this.txtFileName.Location = new System.Drawing.Point(2, 1);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(271, 20);
            this.txtFileName.TabIndex = 23;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            //
            // selectFileName
            //
            this.selectFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFileName.Location = new System.Drawing.Point(277, 0);
            this.selectFileName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.selectFileName.Name = "selectFileName";
            this.selectFileName.Size = new System.Drawing.Size(26, 22);
            this.selectFileName.TabIndex = 24;
            this.selectFileName.Text = "...";
            this.selectFileName.UseVisualStyleBackColor = true;
            this.selectFileName.Click += new System.EventHandler(this.selectConnFileName_Click);
            //
            // errorProvider
            //
            this.errorProvider.ContainerControl = this;
            //
            // FileSelector
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.selectFileName);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FileSelector";
            this.Size = new System.Drawing.Size(303, 22);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button selectFileName;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
