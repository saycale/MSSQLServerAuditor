namespace MSSQLServerAuditor.Gui
{
    partial class frmExceptionBox
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
			System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExceptionBox));
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnDetails = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.txtWholeText = new System.Windows.Forms.TextBox();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			flowLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			//
			// flowLayoutPanel1
			//
			flowLayoutPanel1.AutoSize = true;
			flowLayoutPanel1.Controls.Add(this.lblDescription);
			flowLayoutPanel1.Controls.Add(this.lblMessage);
			flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new System.Drawing.Size(458, 69);
			flowLayoutPanel1.TabIndex = 5;
			//
			// lblDescription
			//
			this.lblDescription.AutoSize = true;
			this.lblDescription.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblDescription.Location = new System.Drawing.Point(0, 0);
			this.lblDescription.Margin = new System.Windows.Forms.Padding(0);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this.lblDescription.Size = new System.Drawing.Size(414, 36);
			this.lblDescription.TabIndex = 3;
			this.lblDescription.Text = "Unhandled exception has occurred in a component in your application. If you click" +
    " Continue, the application will ignore this error and attempt to continue.\r\n";
			this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// lblMessage
			//
			this.lblMessage.AutoSize = true;
			this.lblMessage.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblMessage.Location = new System.Drawing.Point(3, 36);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Padding = new System.Windows.Forms.Padding(10);
			this.lblMessage.Size = new System.Drawing.Size(118, 33);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "exception message";
			//
			// panel1
			//
			this.panel1.Controls.Add(this.btnDetails);
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 67);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(458, 47);
			this.panel1.TabIndex = 4;
			//
			// btnDetails
			//
			this.btnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDetails.Location = new System.Drawing.Point(12, 12);
			this.btnDetails.Name = "btnDetails";
			this.btnDetails.Size = new System.Drawing.Size(108, 23);
			this.btnDetails.TabIndex = 4;
			this.btnDetails.Text = "Details";
			this.btnDetails.UseVisualStyleBackColor = true;
			this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
			//
			// btnClose
			//
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(338, 12);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(108, 23);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Resume";
			this.btnClose.UseVisualStyleBackColor = true;
			//
			// txtWholeText
			//
			this.txtWholeText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtWholeText.Location = new System.Drawing.Point(0, 69);
			this.txtWholeText.Multiline = true;
			this.txtWholeText.Name = "txtWholeText";
			this.txtWholeText.ReadOnly = true;
			this.txtWholeText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtWholeText.Size = new System.Drawing.Size(458, 0);
			this.txtWholeText.TabIndex = 0;
			this.txtWholeText.Visible = false;
			//
			// frmExceptionBox
			//
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(458, 114);
			this.Controls.Add(this.txtWholeText);
			this.Controls.Add(flowLayoutPanel1);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmExceptionBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Exception";
			this.Resize += new System.EventHandler(this.frmExceptionBox_Resize);
			flowLayoutPanel1.ResumeLayout(false);
			flowLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }
        private System.Windows.Forms.Panel panel1;

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtWholeText;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDetails;
    }
}