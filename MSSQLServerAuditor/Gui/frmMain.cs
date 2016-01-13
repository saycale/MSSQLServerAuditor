using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MSSQLServerAuditor.BusinessLogic.LocalStorage;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Loaders;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.Utils.Cryptography;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	/// Main form of application
	/// </summary>
	public partial class frmMain : LocalizableForm, IMainForm
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private          SystemMenu        _systemMenu;
		private          SystemMenuItem    _showHideMainMenuLineItem;
		private          MsSqlAuditorModel _model;
		private readonly int               intMonitor;
		private readonly int               intPosX;
		private readonly int               intPosY;
		private readonly int               intWidth;
		private readonly int               intHeight;
		private readonly bool              isDisableStatusLine;
		private readonly bool              isDisableMainMenu;
		private readonly string            strServerName;
		private readonly string            strTemplate;
		private readonly string            strDatabaseType;
		private readonly bool              isDisableNavigationPanel;
		private bool                       _windowInitialized;
		private ConnectionViewArrangeMode? _connectionArrangeMode;

		/// <summary>
		/// Initializing object frmMain.
		/// </summary>
		public frmMain(
			int    intMonitor,
			int    intPosX,
			int    intPosY,
			int    intWidth,
			int    intHeight,
			bool   isDisableStatusLine,
			bool   isDisableMainMenu,
			string strServerName,
			string strTemplate,
			string strDatabaseType,
			bool   isDisableNavigationPanel
		) {
			InitializeComponent();

			this.intMonitor               = intMonitor;
			this.intPosX                  = intPosX;
			this.intPosY                  = intPosY;
			this.intWidth                 = intWidth;
			this.intHeight                = intHeight;
			this.isDisableStatusLine      = isDisableStatusLine;
			this.isDisableMainMenu        = isDisableMainMenu;
			this.strServerName            = strServerName;
			this.strTemplate              = strTemplate;
			this.strDatabaseType          = strDatabaseType;
			this.isDisableNavigationPanel = isDisableNavigationPanel;
			this._windowInitialized       = false;
			this._connectionArrangeMode   = null;

			this.CinfigureMainMenu();

			Text += " - " + Application.ProductVersion;

			WebBrowserComp.SetIeComp(null);
		}

		private void CinfigureMainMenu()
		{
			if (AppVersionHelper.IsTrial())
			{
				this.mnuNewTab.Visible = false;
				this.mnuLicenses.Visible = false;
			}
		}

		private void RestoreSizeAndLocation(SettingsInfo settings)
		{
			int      intShowOnMonitor = 0;
			Screen[] arrScreens       = Screen.AllScreens;

			this._windowInitialized = false;

			try
			{
				// this is the default
				this.WindowState = FormWindowState.Normal;
				this.StartPosition = FormStartPosition.WindowsDefaultBounds;

				if (settings.MainWindowState.Size.Width != 0 || settings.MainWindowState.Size.Height != 0)
				{
					// check if the saved bounds are nonzero and visible on any screen
					if (!settings.MainWindowState.IsEmpty() && IsVisibleOnAnyScreen(settings.MainWindowState.GetBounds()))
					{
						// first set the bounds
						this.StartPosition = FormStartPosition.Manual;
						this.DesktopBounds = settings.MainWindowState.GetBounds();

						// afterwards set the window state to the saved value (which could be Maximized)
						this.WindowState = settings.MainWindowState.WindowState;
					}
					else
					{
						// this resets the upper left corner of the window to windows standards
						this.StartPosition = FormStartPosition.WindowsDefaultLocation;

						// we can still apply the saved size
						// msorens: added gatekeeper, otherwise first time appears as just a title bar!
						if (!settings.MainWindowState.IsEmpty())
						{
							this.Size = settings.MainWindowState.Size;
						}
					}
				}

				//
				// Start to process commanl line parameters if any exists.
				// Command line parameters (if any given) are overwrite the default setting from the
				// configuratins.
				//
				if (intMonitor >= 0 && intMonitor < arrScreens.Length)
				{
					intShowOnMonitor = intMonitor;
				}
				else
				{
					intShowOnMonitor = 0;
				}

				if (intPosX >= 0)
				{
					this.Left = arrScreens[intShowOnMonitor].Bounds.Left + intPosX;
				}
				if (intPosY >= 0)
				{
					this.Top = arrScreens[intShowOnMonitor].Bounds.Top + intPosY;
				}
				if (intWidth > 0)
				{
					this.Width = intWidth;
				}
				if (intHeight > 0)
				{
					this.Height = intHeight;
				}
			}
			finally
			{
				this._windowInitialized = true;
			}
		}

		private void UpdateMainMenuVisibility(SettingsInfo settings, bool? isDisableMainMenu)
		{
			if (isDisableMainMenu != null && isDisableMainMenu == true)
			{
				mainMenu.Visible = false;
			}
			else
			{
				mainMenu.Visible = settings.ShowMainMenu;
			}
		}

		private static bool IsVisibleOnAnyScreen(Rectangle rect)
		{
			return Screen.AllScreens.Any(screen => screen.WorkingArea.IntersectsWith(rect));
		}

		/// <summary>
		/// Occurs when closed.
		/// </summary>
		/// <param name="e">EventArgs.</param>
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			if (this._model != null)
			{
				// only save the WindowState if Normal or Maximized
				switch (this.WindowState)
				{
					case FormWindowState.Normal:
					case FormWindowState.Maximized:
						this._model.Settings.MainWindowState = this._model.Settings.MainWindowState.NewWindowState(this.WindowState);
						break;

					default:
						this._model.Settings.MainWindowState = this._model.Settings.MainWindowState.NewWindowState(FormWindowState.Normal);
						break;
				}

				SettingsLoader.SaveToXml(this._model.FilesProvider.UserSettingsFileName, this._model.Settings);

				// SettingsLoader.SaveTemplateToXml(this._model.FilesProvider.UserTemplateSettingsFileName, _model.TemplateSettings);
				this._model.TemplateSettings.CreateDocumentoXML(this._model.FilesProvider.UserTemplateSettingsFileName);
			}
		}

		#region window size/position

		/// <summary>
		/// Occurs in case of size change.
		/// </summary>
		/// <param name="e">EventArgs.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			TrackWindowState();
		}

		/// <summary>
		/// Occurs in case of move.
		/// </summary>
		/// <param name="e">EventArgs.</param>
		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);
			TrackWindowState();
		}

		// On a move or resize in Normal state, record the new values as they occur.
		// This solves the problem of closing the app when minimized or maximized.
		private void TrackWindowState()
		{
			if (this._model != null)
			{
				// Don't record the window setup, otherwise we lose the persistent values!
				if (this._windowInitialized)
				{
					this._model.Settings.MainWindowState = this._model.Settings.MainWindowState.NewState(WindowState);

					if (WindowState == FormWindowState.Normal)
					{
						this._model.Settings.MainWindowState = this._model.Settings.MainWindowState.NewBounds(this.DesktopBounds);
					}
				}
			}
		}

		/// <summary>
		/// Resize main window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmMain_ResizeEnd(object sender, EventArgs e)
		{
			/*var connectionTabControl = FindSingleConnectionTab();

			if (connectionTabControl != null)
			{
				connectionTabControl.VisualizeProcessor_VisualizeDataChanged(null,
					new VisualizeDataChangedEventArgs(
						connectionTabControl.CurrentConcreteTemplateNodeDefinition
					)
				);
			}*/
		}

		#endregion window size/position

		/// <summary>
		/// Set model
		/// </summary>
		/// <param name="model">Model</param>
		public void SetModel(MsSqlAuditorModel model)
		{
			this._model = model;
			ApplySettings();
		}

		/// <summary>
		/// Connection arrange mode.
		/// </summary>
		private ConnectionViewArrangeMode? ConnectionArrangeMode
		{
			get { return this._connectionArrangeMode; }

			set
			{
				if (this._connectionArrangeMode == null || this._connectionArrangeMode != value)
				{
					this._connectionArrangeMode = value;
					RearangeConnections();
				}
			}
		}

		private void RearangeConnections()
		{
			using (new WaitCursor())
			{
				var connections =
					tabMain.TabPages.OfType<Control>()
						 .SelectMany(page => page.Controls.OfType<ConnectionTabControl>()
							 .SelectMany(tab => tab.Connections)).ToList();

				foreach (Control tabPage in tabMain.TabPages)
				{
					foreach (var ConnectionTab in tabPage.Controls.OfType<ConnectionTabControl>())
					{
						ConnectionTab.ConntectionUpdated -= OnConnectionUpdated;
					}
				}

				tabMain.TabPages.Clear();

				foreach (var cnn in connections)
				{
					var targetTab = GetTargetTab(cnn.Title);
					targetTab.AppendConnectionData(cnn);
				}
			}
		}

		private void EstablishNewConnection()
		{
			frmConnectionSelection selector = new frmConnectionSelection();

			selector.SetConnectionGroupInfos(this._model.GetAvailableConnections());

			if (selector.ShowDialog() == DialogResult.OK)
			{
				ConnectionGroupInfo connectionGroup = selector.SelectedConnectionGroupInfo.ExtractSelectedGroup();

				ConnectionTabControl targetTab = GetTargetTab(connectionGroup.Name + "{" + connectionGroup.TemplateFileName + "}");

				targetTab.OpenConnection(connectionGroup);
			}
		}

		private ConnectionTabControl GetTargetTab(string name)
		{
			return ConnectionArrangeMode == ConnectionViewArrangeMode.AsTabs ||
				FindSingleConnectionTab() == null
					? CreateNewCntConnectionTab(name)
					: FindSingleConnectionTab();
		}

		private void mnuOpenRaw_Click(object sender, EventArgs e)
		{
			var openDialog = new OpenFileDialog
			{
				Filter = @"MSD|*.msd|MSH|*.msh|MSR|*.msr",
				RestoreDirectory = true
			};

			if (openDialog.ShowDialog() == DialogResult.OK)
			{
				ICryptoService cryptoService = this._model.CryptoService;
				List<ConnectionGroupInfo> storedConnections = 
					StorageSerializer.GetStoredConnections(openDialog.FileName, cryptoService);

				if (storedConnections != null && !storedConnections.Any())
				{
					return;
				}

				OpenConnectionDialog openConnectionDialog = new OpenConnectionDialog();
				openConnectionDialog.SetStoredConnections(storedConnections);

				if (openConnectionDialog.ShowDialog() == DialogResult.OK)
				{
					ConnectionGroupInfo connectionGroup = storedConnections != null
						? openConnectionDialog.SelectedConnectionGroup
						: new ConnectionGroupInfo()
						{
							Connections = new List<InstanceInfo>(),
							Name = Path.GetFileName(openDialog.FileName)
						};

					foreach (InstanceInfo instanceInfo in connectionGroup.Connections)
					{
						instanceInfo.DbType = openConnectionDialog.DataBaseType.ToString();
					}

					string inMsd = openDialog.FileName;
					string inMsh = StorageSerializer.GetMshFromMsd(inMsd);
					string inMsr = StorageSerializer.GetMsrFromMsd(inMsd);

					if (!StorageSerializer.IsMsdFile(inMsd))
					{
						inMsd = null;
					}

					var vaultProcessor = StorageSerializer.GetReadonlyVaultProcessor(
						this._model,
						inMsd,
						inMsh,
						inMsr
					);

					this._model.AssociateVaultProcessor(vaultProcessor, connectionGroup);

					var connectionTab = GetTargetTab(connectionGroup.Name + "    ");

					connectionTab.OpenConnection(connectionGroup);
				}
			}
		}

		private ConnectionTabControl FindSingleConnectionTab()
		{
			return tabMain.TabPages.OfType<Control>().SelectMany(page => page.Controls.OfType<ConnectionTabControl>()).FirstOrDefault();
		}

		private ConnectionTabControl CreateNewCntConnectionTab(string text)
		{
			var connectionPage = new TabPage
			{
				Margin    = new Padding(0),
				Padding   = new Padding(0),
				BackColor = Color.White
			};

			tabMain.TabPages.Add(connectionPage);
			tabMain.SelectedTab = connectionPage;

			var connectionTab = new ConnectionTabControl(
				this._model,
				isDisableStatusLine,
				isDisableNavigationPanel,
				ConnectionArrangeMode == ConnectionViewArrangeMode.AsRootNodes
			);

			connectionTab.ConntectionUpdated += OnConnectionUpdated;
			connectionPage.Text = text;
			connectionPage.Controls.Add(connectionTab);
			connectionTab.Dock = DockStyle.Fill;

			this._model.LocaleManager.LocalizeDeep(this, connectionTab.Controls);

			return connectionTab;
		}

		private void OnConnectionUpdated(object sender, ConnectionTabControl.ConntectionUpdatedArgs args)
		{
			string strTemplateName   = String.Empty;
			string strConnectionName = String.Empty;

			BeginInvoke(new Action(() =>
			{
				if (sender == this._ActiveTab && sender != null)
				{
					if (args != null)
					{
						if (args.TemplateName != null)
						{
							strTemplateName = args.TemplateName;
						}

						if (args.ConnectionName != null)
						{
							strConnectionName = args.ConnectionName;
						}
					}

					setTitle(strConnectionName.TrimmedOrEmpty(), strTemplateName.TrimmedOrEmpty());
				}
			}));
		}

		private void button6_Click(object sender, EventArgs e)
		{
			EstablishNewConnection();
			//NewTab();
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void mnuNewTab_Click(object sender, EventArgs e)
		{
			button6_Click(sender, e);
		}

		private void mnuAbout_Click(object sender, EventArgs e)
		{
			(new frmAboutBox(this._model)).ShowDialog();
		}

		private void mnuSettings_Click(object sender, EventArgs e)
		{
			ChangeSettings();
		}

		private void ChangeSettings()
		{
			frmSettings form = new frmSettings(this._model)
			{
				SettingsInfo = this._model.Settings.GetCopy()
			};

			if (form.ShowDialog() == DialogResult.OK)
			{
				this._model.SetSettings(form.SettingsInfo);
			}
		}

		private void setTitle(string connectionName,string templateName)
		{
			string macroStr    = String.Empty;
			string macroFmtStr = String.Empty;

			if (string.IsNullOrEmpty(connectionName) && string.IsNullOrEmpty(templateName))
			{
				// default format string
				macroFmtStr = "$ApplicationProductName$ - $ApplicationProductVersion$";

				macroStr = Program.Model.Settings.SystemSettings.MainFormWindowTitle.Locales.FirstOrDefault(
					l => l.Language == Program.Model.Settings.InterfaceLanguage).Text;

				if (!string.IsNullOrEmpty(macroStr))
				{
					macroFmtStr = macroStr;
				}
			}
			else
			{
				// default format string
				macroFmtStr = "$ConnectionName$ - $ModuleName$ - $ApplicationProductName$ - $ApplicationProductVersion$";

				if (string.IsNullOrEmpty(templateName))
				{
					if (this._ActiveTab != null && this._ActiveTab.SelectedTemplate != null)
					{
						TemplateNodeLocaleInfo linf = this._ActiveTab.SelectedTemplate.Locales.FirstOrDefault(
							l => l.Language == Program.Model.Settings.InterfaceLanguage);

						if (linf != null)
						{
							templateName = linf.Text;
						}
					}
				}

				if (this._ActiveTab != null
					&& this._ActiveTab.SelectedTemplate != null
					&& this._ActiveTab.SelectedTemplate.MainFormWindowTitle != null
				)
				{
					macroStr = this._ActiveTab.SelectedTemplate.MainFormWindowTitle.Locales.FirstOrDefault(
						l => l.Language == Program.Model.Settings.InterfaceLanguage).Text;

					if (!string.IsNullOrEmpty(macroStr))
					{
						macroFmtStr = macroStr;
					}
				}
			}

			macroFmtStr = macroFmtStr.Replace("$ApplicationProductName$",    "{0}");
			macroFmtStr = macroFmtStr.Replace("$ApplicationProductVersion$", "{1}");
			macroFmtStr = macroFmtStr.Replace("$ConnectionName$",            "{2}");
			macroFmtStr = macroFmtStr.Replace("$ModuleName$",                "{3}");

			this.Text = string.Format(macroFmtStr,
				Application.ProductName,
				Application.ProductVersion,
				connectionName,
				templateName
			);
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			try
			{
				setTitle(null, null);

				this._systemMenu = new SystemMenu(this);
				this._systemMenu.AppendSeparator();

				this._showHideMainMenuLineItem = this._systemMenu.AppendNewItem("", (s, args) =>
				{
					this._model.Settings.ShowMainMenu = !this._model.Settings.ShowMainMenu;

					UpdateMainMenuVisibility(this._model.Settings, null);
				});

				UpdateShowHideMainMenuLineItem();

				if (this.isDisableMainMenu)
				{
					this._model.Settings.ShowMainMenu = false;
					UpdateMainMenuVisibility(this._model.Settings, true);
				}

				RestoreSizeAndLocation(this._model.Settings);

				this._model.SettingsChanged += (o, args) => ApplySettings();

				if (strServerName != null && strTemplate != null && strDatabaseType != null)
				{
					// log.DebugFormat(
					//    "strServerName:'{0}';strTemplate:'{1}';strDatabaseType:'{2}'",
					//    strServerName ?? "<Null>",
					//    strTemplate ?? "<Null>",
					//    strDatabaseType ?? "<Null>"
					// );

					InstanceInfo instanceInfo = new InstanceInfo
					{
						Authentication = new AuthenticationInfo
						{
							IsWindows = true,
							Password = string.Empty,
							Username = string.Empty
						},
						Instance  = strServerName,
						IsEnabled = true,
						Name      = strServerName,
						IsODBC    = false,
						DbType    = strDatabaseType
					};

					ConnectionGroupInfo connectionGroupInfo = new ConnectionGroupInfo();
					connectionGroupInfo.Connections.Add(instanceInfo);

					string strTemplateFileName       = Path.GetFileName(strTemplate);
					string strFullPathToTemplateFile = Path.GetFullPath(strTemplate);
					string strTemplateDir            = Path.GetDirectoryName(strTemplate);

					if (string.IsNullOrEmpty(strTemplateDir))
					{
						strTemplateDir                 = Program.Model.Settings.TemplateDirectory;
						strFullPathToTemplateFile      = Path.GetFullPath(strTemplateDir + "\\" + strTemplate);
						connectionGroupInfo.IsExternal = false;
					}
					else
					{
						connectionGroupInfo.IsExternal = true;
					}

					connectionGroupInfo.IsDirectConnection = true;

					connectionGroupInfo.TemplateDir      = strTemplateDir;
					connectionGroupInfo.TemplateFileName = strTemplateFileName;
					connectionGroupInfo.Name             = strServerName;

					var targetTab = GetTargetTab(connectionGroupInfo.Name + "{" + connectionGroupInfo.TemplateFileName + "}");

					Template selectedTemplate = TemplateNodesLoader.GetTemplateByFile(strFullPathToTemplateFile);
					targetTab.SelectedTemplate = selectedTemplate;

					targetTab.OpenConnection(connectionGroupInfo);
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		private void UpdateShowHideMainMenuLineItem()
		{
			if (this._showHideMainMenuLineItem != null)
			{
				this._showHideMainMenuLineItem.Text = this._model.LocaleManager.GetLocalizedText(
					this.Name,
					mainMenu.Visible ? "mnuHideMainMenu" : "mnuShowMainMenu"
				);
			}
		}

		private void ApplySettings()
		{
			tabMain.ShowSingleTab = this._model.Settings.ShowConnectionTabIfSingle && this._model.Settings.ConnectionsInTabs;

			// 12/11/2014 Aleksey A. Saychenko
			//
			// Window size and location don't need to be changed after setting changes
			// but only during the initial form windows load.
			//
			// RestoreSizeAndLocation(this._model.Settings);

			UpdateMainMenuVisibility(this._model.Settings, null);

			ConnectionArrangeMode = this._model.Settings.ConnectionsInTabs
				? ConnectionViewArrangeMode.AsTabs
				: ConnectionViewArrangeMode.AsRootNodes;
		}

		private void mainMenu_VisibleChanged(object sender, EventArgs e)
		{
			UpdateShowHideMainMenuLineItem();
		}

		private void mnuSettings_DropDownOpening(object sender, EventArgs e)
		{
			mnuHideMainMenu.Visible = mainMenu.Visible;

			hideReportsTreeMenuItem.Visible = this._ActiveTab != null && this._ActiveTab.TreeIsVisible;
			showReportsTreeMenuItem.Visible = this._ActiveTab != null && !this._ActiveTab.TreeIsVisible;
			hideStatusPanelItem.Visible     = this._ActiveTab != null && !this._ActiveTab.HideStatusPanel;
			showStatusPanelItem.Visible     = this._ActiveTab != null && this._ActiveTab.HideStatusPanel;
		}

		private void hideMainMenuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this._model.Settings.ShowMainMenu = false;
			ApplySettings();
		}

		private void frmMain_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F5)
			{
				if (tabMain.SelectedTab != null)
				{
					var activeConnectionTab = tabMain.SelectedTab.Controls.OfType<ConnectionTabControl>().FirstOrDefault();

					if (activeConnectionTab != null)
					{
						activeConnectionTab.F5RefreshView();
					}
				}
			}
		}

		private string LicensesLocalizedText()
		{
			return this._model.LocaleManager.GetLocalizedText(this.Name, "mnuLicenses");
		}

		private void mnuAddLicense_Click(object sender, EventArgs e)
		{
			List<ConnectionGroupInfo> licenses;

			using (var dlg = new OpenFileDialog {Filter = @"XML|*.xml"})
			{
				if (dlg.ShowDialog() != DialogResult.OK)
				{
					return;
				}

				try
				{
					licenses = ConnectionsLoader.LoadFromXml(dlg.FileName).ToList();
				}
				catch (Exception ex)
				{
					licenses = new List<ConnectionGroupInfo>();
					log.Error(ex);
				}
			}

			if (licenses.Count == 0)
			{
				MessageBox.Show(
					this._model.LocaleManager.GetLocalizedText(this.Name, @"msgNoLicenseFound"),
					LicensesLocalizedText(),
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);

				return;
			}

			ConnectionsLoader.SaveToXml(this._model.FilesProvider.ComposeNewFileNameForAddedConnection(), licenses);

			var form = new frmLicenseErrors
			{
				StartPosition = FormStartPosition.CenterParent,
				Text          = this._model.LocaleManager.GetLocalizedText(this.Name, @"msgLicenseSuccessfulyAdded")
			};

			form.IgnoreLocalizationProperties.Add("Text");
			form.SetLicenseProblems(licenses.SelectMany(cnn => cnn.Connections.Select(l => l.ValidateLicense(true))).ToList());
			form.ShowDialog();

			//MessageBox.Show(this._model.LocaleManager.GetLocalizedText(this.Name, @"msgLicenseSuccessfulyAdded"), LicensesLocalizedText(),
			//    MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mnuRemoveLicense_Click(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog
				{
					// Filter = LicensesLocalizedText() + "|" + FilesProvider.ConnectionsFileNamePattern,
					Filter = LicensesLocalizedText() + "|" + this._model.FilesProvider.ConnectionsFileNamePattern,
					InitialDirectory = this._model.FilesProvider.LicensesDirectory
				}
			)
			{
				if (dlg.ShowDialog() != DialogResult.OK)
				{
					return;
				}

				if (MessageBox.Show(
					this._model.LocaleManager.GetLocalizedText(this.Name, @"msgLicenseWillBeRemoved"),
					LicensesLocalizedText(),
					MessageBoxButtons.OKCancel,
					MessageBoxIcon.Warning
					) == DialogResult.OK
				)
				{
					File.Delete(dlg.FileName);
				}
			}
		}

		private ConnectionTabControl _ActiveTab
		{
			get
			{
				return tabMain != null && tabMain.Visible && tabMain.SelectedTab != null
					? tabMain.SelectedTab.Controls.OfType<ConnectionTabControl>().FirstOrDefault()
					: null;
			}
		}

		private void mnuFile_DropDownOpening(object sender, EventArgs e)
		{
			var            enabled     = false;
			ConnectionData selectedCnn = null;

			if (this._ActiveTab != null)
			{
				enabled = this._ActiveTab.CanSaveRaw;
				selectedCnn = this._ActiveTab.GetSelectedConnection();
			}

			mnuCloseConnection.Enabled = this._ActiveTab != null
				&& (this._ActiveTab.ConnectionsAsNodes && this._ActiveTab.GetSelectedConnection() != null);

			mnuCloseConnection.Text = ConnectionTabControl.GetCloseConnectionCommandText(this._model.LocaleManager);

			mnuSaveRaw.Enabled = enabled;

			mnuSaveRaw.Text = this._model.LocaleManager.GetLocalizedText(this.Name, mnuSaveRaw.Name)
				+ (enabled && selectedCnn != null ? "  \"" + selectedCnn.Title + "\"": "")
				+ "...";
		}

		private void mnuCloseConnection_Click(object sender, EventArgs e)
		{
			if (this._ActiveTab != null)
			{
				if (this._ActiveTab.ConnectionsAsNodes)
				{
					this._ActiveTab.CloseConnection(this._ActiveTab.GetSelectedConnection());
				}
				else
				{
					tabMain.CloseTab(tabMain.SelectedTab);
				}

				// SettingsLoader.SaveTemplateToXml(this._model.FilesProvider.UserTemplateSettingsFileName, this._model.TemplateSettings);
				this._model.TemplateSettings.CreateDocumentoXML(
					this._model.FilesProvider.UserTemplateSettingsFileName
				);
			}
		}

		private void mnuComponents_Click(object sender, EventArgs e)
		{
			(new frmComponents()).ShowDialog();
		}

		private void mnuOptionLicense_Click(object sender, EventArgs e)
		{
			(new frmAboutLicense()).ShowDialog();
		}

		private void mnuNewDirectConnection_Click(object sender, EventArgs e)
		{
			using (CreateDirectConnectionDialog createConnectionDialog = new CreateDirectConnectionDialog(this._model))
			{
				if (createConnectionDialog.ShowDialog() == DialogResult.OK)
				{
					ConnectionData      connectionData = createConnectionDialog.ResultConnection;
					ConnectionGroupInfo group          = connectionData.ConnectionGroup;

					Template selectedTemplate = TemplateNodesLoader.GetTemplateByFile(
						createConnectionDialog.SelectedTemplateFileFullPath
					);

					ConnectionTabControl targetTab = GetTargetTab(
						group.Name + " {" + group.TemplateFileName + "}"
					);

					targetTab.SelectedTemplate = selectedTemplate;

					targetTab.OpenConnection(group);
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmSaveDialog        saveDialog = new frmSaveDialog();
			ConnectionData       connection = null;
			ConnectionTabControl currentTab = (ConnectionTabControl)tabMain.SelectedTab.Controls[0];

			if (currentTab != null)
			{
				connection = currentTab.GetSelectedConnection();
			}

			if (connection == null)
			{
				saveDialog.DisableCurrentScope();
			}
			else
			{
				saveDialog.SetCurScopeName(connection.SourceConnectionName);
			}

			if (saveDialog.ShowDialog() == DialogResult.OK)
			{
				if (saveDialog.Scope == frmSaveDialog.ScopeToSave.All)
				{
					currentTab = null;
				}

				StorageSerializationInfo serializationInfo = new StorageSerializationInfo(
					this._model,
					currentTab,
					saveDialog.Folder,
					saveDialog.SaveCurrent,
					saveDialog.SaveHistoric
				);

				ProgressForm form = new ProgressForm
				{
					Text = this._model.LocaleManager.GetLocalizedText(
						this.Name,
						"lbSerialization",
						this._model.Settings.InterfaceLanguage
					),
					StartPosition = FormStartPosition.CenterParent
				};

				form.ProgressBar.Style = ProgressBarStyle.Marquee;

				form.Argument = serializationInfo;
				form.DoWork  += DoSerializationWork;

				StartSerializationForm(form);
			}
		}

		private void StartSerializationForm(ProgressForm form)
		{
			DialogResult result = form.ShowDialog();

			if (result == DialogResult.Cancel)
			{
				MessageBox.Show(
					this._model.LocaleManager.GetLocalizedText(
						"common",
						"operationCancelled",
						this._model.Settings.InterfaceLanguage
					),
					null,
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
			}

			if (result == DialogResult.Abort)
			{
				log.Error("Exception at Save MSD", form.Result.Error);

				MessageBox.Show(
					this._model.LocaleManager.GetLocalizedText(
						LocaleManager.Exceptions,
						"saveMsdProblem",
						this._model.Settings.InterfaceLanguage
					),
					null,
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		private void hideReportsTreeMenuItem_Click(object sender, EventArgs e)
		{
			if (this._ActiveTab != null)
			{
				this._ActiveTab.TreeIsVisible = false;
			}
		}

		private void showReportsTreeMenuItem_Click(object sender, EventArgs e)
		{
			if (this._ActiveTab != null)
			{
				this._ActiveTab.TreeIsVisible = true;
			}
		}

		private void hideStatusPanel_Click(object sender, EventArgs e)
		{
			if (this._ActiveTab != null)
			{
				this._model.Settings.ShowStatusPanel = false;
				this._ActiveTab.HideStatusPanel = true;
			}
		}

		private void showStatusPanelItem_Click(object sender, EventArgs e)
		{
			if (this._ActiveTab != null)
			{
				this._model.Settings.ShowStatusPanel = true;
				this._ActiveTab.HideStatusPanel = false;
			}
		}

		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._ActiveTab != null)
			{
				if (this._ActiveTab.Connections != null)
				{
					if (this._ActiveTab.Connections.Count() > 0)
					{
						string connName = this._ActiveTab.Connections.ElementAt(0).Title;
						setTitle(connName, null);
					}
				}
			}
		}

		private void DoSerializationWork(ProgressForm progressForm, DoWorkEventArgs e)
		{
			StorageSerializationInfo serializationInfo = e.Argument as StorageSerializationInfo;

			if (serializationInfo != null)
			{
				var remoteVaultSerializer = new StorageSerializer(
					serializationInfo.Model,
					serializationInfo.TabControl
				);

				ISerializationCancellationToken canceller = new FormSerializationCancellationToken(progressForm);

				try
				{
					remoteVaultSerializer.SaveData(
						serializationInfo.OutputFolder,
						serializationInfo.SaveCurrentDatabase,
						serializationInfo.SaveHistoricDatabase,
						canceller
					);
				}
				catch (OperationCanceledException ex)
				{
					log.Error("Exception at Concelled", ex);
					e.Cancel = true;
				}
			}
		}
	}

	/// <summary>
	/// Interface for main form
	/// </summary>
	public interface IMainForm
	{
		/// <summary>
		/// Set model
		/// </summary>
		/// <param name="model">Model </param>
		void SetModel(MsSqlAuditorModel model);
	}
}
