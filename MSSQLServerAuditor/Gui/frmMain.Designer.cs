using MSSQLServerAuditor.Gui.Base;
using MSSQLServerAuditor.Model.Internationalization;

namespace MSSQLServerAuditor.Gui
{
	partial class frmMain : LocalizableForm, IMainForm
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
			System.Windows.Forms.ToolStripMenuItem mnuFile;
			System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
			System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
			System.Windows.Forms.ToolStripMenuItem mnuSettings;
			System.Windows.Forms.ToolStripMenuItem mnuHelp;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.mnuNewTab = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuNewDirectConnection = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCloseConnection = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenRaw = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveRaw = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHideMainMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.hideReportsTreeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showReportsTreeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hideStatusPanelItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showStatusPanelItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuComponents = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.mnuLicenses = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAddLicense = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRemoveLicense = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOptionLicense = new System.Windows.Forms.ToolStripMenuItem();
			this.tabMain = new MSSQLServerAuditor.Gui.Base.CloseableTabControl();
			mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			mnuSettings = new System.Windows.Forms.ToolStripMenuItem();
			mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			//
			// mnuFile
			//
			mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.mnuNewTab,
			this.mnuNewDirectConnection,
			this.mnuCloseConnection,
			toolStripMenuItem1,
			this.mnuOpenRaw,
			this.mnuSaveRaw,
			toolStripMenuItem2,
			this.mnuClose});
			mnuFile.Name = "mnuFile";
			mnuFile.Size = new System.Drawing.Size(37, 20);
			mnuFile.Text = "File";
			mnuFile.DropDownOpening += new System.EventHandler(this.mnuFile_DropDownOpening);
			//
			// mnuNewTab
			//
			this.mnuNewTab.Name = "mnuNewTab";
			this.mnuNewTab.Size = new System.Drawing.Size(202, 22);
			this.mnuNewTab.Text = "New connection";
			this.mnuNewTab.Click += new System.EventHandler(this.mnuNewTab_Click);
			//
			// mnuNewDirectConnection
			//
			this.mnuNewDirectConnection.Name = "mnuNewDirectConnection";
			this.mnuNewDirectConnection.Size = new System.Drawing.Size(202, 22);
			this.mnuNewDirectConnection.Text = "New connection (direct)";
			this.mnuNewDirectConnection.Click += new System.EventHandler(this.mnuNewDirectConnection_Click);
			//
			// mnuCloseConnection
			//
			this.mnuCloseConnection.Name = "mnuCloseConnection";
			this.mnuCloseConnection.Size = new System.Drawing.Size(202, 22);
			this.mnuCloseConnection.Text = "Close";
			this.mnuCloseConnection.Click += new System.EventHandler(this.mnuCloseConnection_Click);
			//
			// toolStripMenuItem1
			//
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(199, 6);
			//
			// mnuOpenRaw
			//
			this.mnuOpenRaw.Name = "mnuOpenRaw";
			this.mnuOpenRaw.Size = new System.Drawing.Size(202, 22);
			this.mnuOpenRaw.Text = "Open";
			this.mnuOpenRaw.Click += new System.EventHandler(this.mnuOpenRaw_Click);
			//
			// mnuSaveRaw
			//
			this.mnuSaveRaw.Name = "mnuSaveRaw";
			this.mnuSaveRaw.Size = new System.Drawing.Size(202, 22);
			this.mnuSaveRaw.Text = "Save";
			this.mnuSaveRaw.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			//
			// toolStripMenuItem2
			//
			toolStripMenuItem2.Name = "toolStripMenuItem2";
			toolStripMenuItem2.Size = new System.Drawing.Size(199, 6);
			//
			// mnuClose
			//
			this.mnuClose.Name = "mnuClose";
			this.mnuClose.Size = new System.Drawing.Size(202, 22);
			this.mnuClose.Text = "Exit";
			this.mnuClose.Click += new System.EventHandler(this.mnuExit_Click);
			//
			// mnuSettings
			//
			mnuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.mnuOptions,
			this.mnuHideMainMenu,
			this.hideReportsTreeMenuItem,
			this.showReportsTreeMenuItem,
			this.hideStatusPanelItem,
			this.showStatusPanelItem});
			mnuSettings.Name = "mnuSettings";
			mnuSettings.Size = new System.Drawing.Size(61, 20);
			mnuSettings.Text = "Settings";
			mnuSettings.DropDownOpening += new System.EventHandler(this.mnuSettings_DropDownOpening);
			//
			// mnuOptions
			//
			this.mnuOptions.Name = "mnuOptions";
			this.mnuOptions.Size = new System.Drawing.Size(221, 22);
			this.mnuOptions.Text = "Options";
			this.mnuOptions.Click += new System.EventHandler(this.mnuSettings_Click);
			//
			// mnuHideMainMenu
			//
			this.mnuHideMainMenu.Name = "mnuHideMainMenu";
			this.mnuHideMainMenu.Size = new System.Drawing.Size(221, 22);
			this.mnuHideMainMenu.Text = "Hide main menu";
			this.mnuHideMainMenu.Click += new System.EventHandler(this.hideMainMenuToolStripMenuItem_Click);
			//
			// hideReportsTreeMenuItem
			//
			this.hideReportsTreeMenuItem.Name = "hideReportsTreeMenuItem";
			this.hideReportsTreeMenuItem.Size = new System.Drawing.Size(221, 22);
			this.hideReportsTreeMenuItem.Text = "Hide tree of reports";
			this.hideReportsTreeMenuItem.Click += new System.EventHandler(this.hideReportsTreeMenuItem_Click);
			//
			// showReportsTreeMenuItem
			//
			this.showReportsTreeMenuItem.Name = "showReportsTreeMenuItem";
			this.showReportsTreeMenuItem.Size = new System.Drawing.Size(221, 22);
			this.showReportsTreeMenuItem.Text = "ShowReportsTreeMenuItem";
			this.showReportsTreeMenuItem.Click += new System.EventHandler(this.showReportsTreeMenuItem_Click);
			//
			// hideStatusPanelItem
			//
			this.hideStatusPanelItem.Name = "hideStatusPanelItem";
			this.hideStatusPanelItem.Size = new System.Drawing.Size(221, 22);
			this.hideStatusPanelItem.Text = "Hide status panel";
			this.hideStatusPanelItem.Click += new System.EventHandler(this.hideStatusPanel_Click);
			//
			// showStatusPanelItem
			//
			this.showStatusPanelItem.Name = "showStatusPanelItem";
			this.showStatusPanelItem.Size = new System.Drawing.Size(221, 22);
			this.showStatusPanelItem.Text = "Show status panel";
			this.showStatusPanelItem.Click += new System.EventHandler(this.showStatusPanelItem_Click);
			//
			// mnuHelp
			//
			mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.mnuComponents,
			this.mnuAbout});
			mnuHelp.Name = "mnuHelp";
			mnuHelp.Size = new System.Drawing.Size(44, 20);
			mnuHelp.Text = "Help";
			//
			// mnuComponents
			//
			this.mnuComponents.Name = "mnuComponents";
			this.mnuComponents.Size = new System.Drawing.Size(143, 22);
			this.mnuComponents.Text = "Components";
			this.mnuComponents.Click += new System.EventHandler(this.mnuComponents_Click);
			//
			// mnuAbout
			//
			this.mnuAbout.Name = "mnuAbout";
			this.mnuAbout.Size = new System.Drawing.Size(143, 22);
			this.mnuAbout.Text = "About";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			//
			// mainMenu
			//
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			mnuFile,
			mnuSettings,
			this.mnuLicenses,
			mnuHelp});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(864, 24);
			this.mainMenu.TabIndex = 16;
			this.mainMenu.Text = "menuStrip1";
			this.mainMenu.VisibleChanged += new System.EventHandler(this.mainMenu_VisibleChanged);
			//
			// mnuLicenses
			//
			this.mnuLicenses.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.mnuAddLicense,
			this.mnuRemoveLicense,
			this.mnuOptionLicense});
			this.mnuLicenses.Name = "mnuLicenses";
			this.mnuLicenses.Size = new System.Drawing.Size(63, 20);
			this.mnuLicenses.Text = "Licenses";
			//
			// mnuAddLicense
			//
			this.mnuAddLicense.Name = "mnuAddLicense";
			this.mnuAddLicense.Size = new System.Drawing.Size(192, 22);
			this.mnuAddLicense.Text = "Add license from file...";
			this.mnuAddLicense.Click += new System.EventHandler(this.mnuAddLicense_Click);
			//
			// mnuRemoveLicense
			//
			this.mnuRemoveLicense.Name = "mnuRemoveLicense";
			this.mnuRemoveLicense.Size = new System.Drawing.Size(192, 22);
			this.mnuRemoveLicense.Text = "Remove license...";
			this.mnuRemoveLicense.Click += new System.EventHandler(this.mnuRemoveLicense_Click);
			//
			// mnuOptionLicense
			//
			this.mnuOptionLicense.Name = "mnuOptionLicense";
			this.mnuOptionLicense.Size = new System.Drawing.Size(192, 22);
			this.mnuOptionLicense.Text = "Ask for a license...";
			this.mnuOptionLicense.Click += new System.EventHandler(this.mnuOptionLicense_Click);
			//
			// tabMain
			//
			this.tabMain.ClosableOn = true;
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabMain.Location = new System.Drawing.Point(0, 24);
			this.tabMain.Margin = new System.Windows.Forms.Padding(0);
			this.tabMain.Name = "tabMain";
			this.tabMain.Padding = new System.Drawing.Point(0, 0);
			this.tabMain.SelectedIndex = 0;
			this.tabMain.ShowSingleTab = true;
			this.tabMain.Size = new System.Drawing.Size(864, 513);
			this.tabMain.TabIndex = 17;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			//
			// frmMain
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(864, 537);
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.mainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.mainMenu;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MSSQLServerAuditor";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuNewTab;
		private System.Windows.Forms.ToolStripMenuItem mnuClose;
		private System.Windows.Forms.ToolStripMenuItem mnuAbout;
		private System.Windows.Forms.ToolStripMenuItem mnuOptions;
		private CloseableTabControl tabMain;
		private System.Windows.Forms.ToolStripMenuItem mnuOpenRaw;
		private System.Windows.Forms.ToolStripMenuItem mnuHideMainMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuCloseConnection;
		private System.Windows.Forms.ToolStripMenuItem mnuComponents;
		private System.Windows.Forms.ToolStripMenuItem mnuNewDirectConnection;
		private System.Windows.Forms.ToolStripMenuItem mnuAddLicense;
		private System.Windows.Forms.ToolStripMenuItem mnuRemoveLicense;
		private System.Windows.Forms.ToolStripMenuItem mnuOptionLicense;
		private System.Windows.Forms.ToolStripMenuItem mnuLicenses;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveRaw;
		private System.Windows.Forms.ToolStripMenuItem hideReportsTreeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showReportsTreeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hideStatusPanelItem;
		private System.Windows.Forms.ToolStripMenuItem showStatusPanelItem;
	}
}
