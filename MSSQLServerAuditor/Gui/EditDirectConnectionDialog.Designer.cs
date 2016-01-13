namespace MSSQLServerAuditor.Gui
{
	partial class EditDirectConnectionDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditDirectConnectionDialog));
			this.pnlButtons.SuspendLayout();
			this.pnlConnection.SuspendLayout();
			this.grpServerInfo.SuspendLayout();
			this.grpTemplate.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bsTemplateFileSetting)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.grpDataBaseType.SuspendLayout();
			this.grpModuleTypes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bsConnectionType)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsModuleType)).BeginInit();
			this.SuspendLayout();
			//
			// cmbTemplate
			//
			this.cmbTemplate.Enabled = false;
			//
			// cmbDataBaseType
			//
			this.cmbDataBaseType.Enabled = false;
			//
			// txtGroupName
			//
			this.txtGroupName.Enabled = false;
			//
			// optOpenTemplateFromFile
			//
			this.optOpenTemplateFromFile.Enabled = false;
			//
			// optSelectExistTemplate
			//
			this.optSelectExistTemplate.Enabled = false;
			//
			// EditDirectConnectionDialog
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(619, 483);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EditDirectConnectionDialog";
			this.pnlButtons.ResumeLayout(false);
			this.pnlConnection.ResumeLayout(false);
			this.grpServerInfo.ResumeLayout(false);
			this.grpServerInfo.PerformLayout();
			this.grpTemplate.ResumeLayout(false);
			this.grpTemplate.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.bsTemplateFileSetting)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.grpDataBaseType.ResumeLayout(false);
			this.grpModuleTypes.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.bsConnectionType)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsModuleType)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
	}
}
