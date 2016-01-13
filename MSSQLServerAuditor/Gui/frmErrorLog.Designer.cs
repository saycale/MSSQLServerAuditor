using System.Drawing;

namespace MSSQLServerAuditor.Gui
{
    partial class frmErrorLog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmErrorLog));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lbErrorItem = new System.Windows.Forms.ListBox();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.lbInstance = new System.Windows.Forms.Label();
			this.txtSQL = new System.Windows.Forms.TextBox();
			this.txtError = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			//
			// splitContainer1
			//
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			//
			// splitContainer1.Panel1
			//
			this.splitContainer1.Panel1.Controls.Add(this.lbErrorItem);
			//
			// splitContainer1.Panel2
			//
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(433, 264);
			this.splitContainer1.SplitterDistance = 172;
			this.splitContainer1.TabIndex = 0;
			//
			// lbErrorItem
			//
			this.lbErrorItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbErrorItem.FormattingEnabled = true;
			this.lbErrorItem.Location = new System.Drawing.Point(3, 3);
			this.lbErrorItem.Name = "lbErrorItem";
			this.lbErrorItem.Size = new System.Drawing.Size(166, 251);
			this.lbErrorItem.TabIndex = 0;
			this.lbErrorItem.SelectedIndexChanged += new System.EventHandler(this.lbErrorItem_SelectedIndexChanged);
			//
			// splitContainer2
			//
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			//
			// splitContainer2.Panel1
			//
			this.splitContainer2.Panel1.Controls.Add(this.lbInstance);
			this.splitContainer2.Panel1.Controls.Add(this.txtSQL);
			//
			// splitContainer2.Panel2
			//
			this.splitContainer2.Panel2.Controls.Add(this.txtError);
			this.splitContainer2.Size = new System.Drawing.Size(257, 264);
			this.splitContainer2.SplitterDistance = 101;
			this.splitContainer2.TabIndex = 0;
			//
			// lbInstance
			//
			this.lbInstance.AutoSize = true;
			this.lbInstance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.lbInstance.Location = new System.Drawing.Point(4, 3);
			this.lbInstance.Name = "lbInstance";
			this.lbInstance.Size = new System.Drawing.Size(41, 13);
			this.lbInstance.TabIndex = 1;
			this.lbInstance.Text = "label1";
			//
			// txtSQL
			//
			this.txtSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSQL.Location = new System.Drawing.Point(4, 25);
			this.txtSQL.Multiline = true;
			this.txtSQL.Name = "txtSQL";
			this.txtSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSQL.Size = new System.Drawing.Size(250, 73);
			this.txtSQL.TabIndex = 0;
			//
			// txtError
			//
			this.txtError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtError.ForeColor = System.Drawing.Color.Red;
			this.txtError.Location = new System.Drawing.Point(4, 3);
			this.txtError.Multiline = true;
			this.txtError.Name = "txtError";
			this.txtError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtError.Size = new System.Drawing.Size(250, 146);
			this.txtError.TabIndex = 1;
			//
			// frmErrorLog
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(433, 264);
			this.Controls.Add(this.splitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmErrorLog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Errors found";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txtSQL;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.Label lbInstance;
        private System.Windows.Forms.ListBox lbErrorItem;
    }
}