namespace MSSQLServerAuditor.ReportViewer
{
    partial class ReportViewrControl
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
            this.rvReport = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            //
            // rvReport
            //
            this.rvReport.AutoSize = true;
            this.rvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rvReport.DocumentMapWidth = 1;
            this.rvReport.Location = new System.Drawing.Point(0, 0);
            this.rvReport.Margin = new System.Windows.Forms.Padding(0);
            this.rvReport.Name = "rvReport";
            this.rvReport.ShowBackButton = false;
            this.rvReport.ShowPageNavigationControls = false;
            this.rvReport.ShowRefreshButton = false;
            this.rvReport.ShowStopButton = false;
            this.rvReport.Size = new System.Drawing.Size(642, 391);
            this.rvReport.TabIndex = 0;
            this.rvReport.Load += new System.EventHandler(this.rvReport_Load);
            //
            // ReportViewrControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.rvReport);
            this.Name = "ReportViewrControl";
            this.Size = new System.Drawing.Size(642, 391);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rvReport;
    }
}
