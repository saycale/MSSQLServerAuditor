namespace AuditorLicenser
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.configTabPage = new System.Windows.Forms.TabPage();
            this.settingsButton = new System.Windows.Forms.Button();
            this.doConfigurationButton = new System.Windows.Forms.Button();
            this.loadCfgRb = new System.Windows.Forms.RadioButton();
            this.createCfgRb = new System.Windows.Forms.RadioButton();
            this.keysTabPage = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.keysSignTab = new System.Windows.Forms.TabPage();
            this.signNewButton = new System.Windows.Forms.Button();
            this.txtSignPriKey = new System.Windows.Forms.TextBox();
            this.signPrivateKeyLabel = new System.Windows.Forms.Label();
            this.txtSignPubKey = new System.Windows.Forms.TextBox();
            this.signPublicKeyLabel = new System.Windows.Forms.Label();
            this.keysCryptTab = new System.Windows.Forms.TabPage();
            this.cryptNewButton = new System.Windows.Forms.Button();
            this.txtEncPriKey = new System.Windows.Forms.TextBox();
            this.cryptPrivateKeyLabel = new System.Windows.Forms.Label();
            this.txtEncPubKey = new System.Windows.Forms.TextBox();
            this.cryptPublicKeyLabel = new System.Windows.Forms.Label();
            this.keysSaveButton = new System.Windows.Forms.Button();
            this.licensesTabPage = new System.Windows.Forms.TabPage();
            this.licenseToSignLabel = new System.Windows.Forms.Label();
            this.subscribeLicenseButton = new System.Windows.Forms.Button();
            this.licenseOutputLabel = new System.Windows.Forms.Label();
            this.srcTabPage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLicenseFileName = new System.Windows.Forms.TextBox();
            this.еиДшсутыуАшдуТфьу = new System.Windows.Forms.Label();
            this.lbRu = new System.Windows.Forms.Label();
            this.lbProgramName = new System.Windows.Forms.Label();
            this.tbProgramName = new System.Windows.Forms.TextBox();
            this.tbShorcName = new System.Windows.Forms.TextBox();
            this.lbShorcName = new System.Windows.Forms.Label();
            this.lbDirName = new System.Windows.Forms.Label();
            this.tbDirName = new System.Windows.Forms.TextBox();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.lbFileName = new System.Windows.Forms.Label();
            this.tbAdditionalTemplates = new System.Windows.Forms.TextBox();
            this.btAdditionalTemplates = new System.Windows.Forms.Button();
            this.lblAdditionalTemplate = new System.Windows.Forms.Label();
            this.lblAppPath = new System.Windows.Forms.Label();
            this.lbAdditionalSql = new System.Windows.Forms.Label();
            this.tbAdditionalSql = new System.Windows.Forms.TextBox();
            this.addTemplateFileAsRelativePathCheckBox = new System.Windows.Forms.CheckBox();
            this.removeTemplateButton = new System.Windows.Forms.Button();
            this.addTemplateButton = new System.Windows.Forms.Button();
            this.templeateFilesListBox = new System.Windows.Forms.ListBox();
            this.templatesLabel = new System.Windows.Forms.Label();
            this.sourceFolderLabel = new System.Windows.Forms.Label();
            this.userSettingsFileLabel = new System.Windows.Forms.Label();
            this.sysSettingsFileLabel = new System.Windows.Forms.Label();
            this.langFileLabel = new System.Windows.Forms.Label();
            this.srcFilesSignButton = new System.Windows.Forms.Button();
            this.dstFolderLabel = new System.Windows.Forms.Label();
            this.getAssemblyTabPage = new System.Windows.Forms.TabPage();
            this.MSSQLAuditorVersion = new System.Windows.Forms.Label();
            this.dnGuardExeLabel = new System.Windows.Forms.Label();
            this.dnGuardExeNameTextBox = new System.Windows.Forms.TextBox();
            this.resetDnGuardOptionsButton = new System.Windows.Forms.Button();
            this.dnGuardOptionsTextBox = new System.Windows.Forms.TextBox();
            this.dnGuardOptionsLabel = new System.Windows.Forms.Label();
            this.platformComboBox = new System.Windows.Forms.ComboBox();
            this.chbDnGuardUse = new System.Windows.Forms.CheckBox();
            this.dnGuardFolderLabel = new System.Windows.Forms.Label();
            this.txtNetVersion = new System.Windows.Forms.TextBox();
            this.frameworkVerLabel = new System.Windows.Forms.Label();
            this.frameworkFolderLabel = new System.Windows.Forms.Label();
            this.buildApplicationButton = new System.Windows.Forms.Button();
            this.makeDistribTabPage = new System.Windows.Forms.TabPage();
            this.installPackPutputFileLabel = new System.Windows.Forms.Label();
            this.wixFolderLabel = new System.Windows.Forms.Label();
            this.billboardLabel = new System.Windows.Forms.Label();
            this.buildInstallPackButton = new System.Windows.Forms.Button();
            this.wixFileLabel = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtLoadConfFileName = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.txtCreateConfFileName = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.licenseToSignFileSelector = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.signedLicenseOutFileSelector = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.fsLicenseFolder = new MSSQLServerAuditor.Licenser.Gui.FolderSelector();
            this.fileAppPath = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.txtSrcFolder = new MSSQLServerAuditor.Licenser.Gui.FolderSelector();
            this.txtUserSettingsFileName = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.txtDstFolder = new MSSQLServerAuditor.Licenser.Gui.FolderSelector();
            this.txtLangFileName = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.txtSysSettingsFileName = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.txtDnGuardFolder = new MSSQLServerAuditor.Licenser.Gui.FolderSelector();
            this.txtNetFolder = new MSSQLServerAuditor.Licenser.Gui.FolderSelector();
            this.txtOutputMsi = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.txtWixFolder = new MSSQLServerAuditor.Licenser.Gui.FolderSelector();
            this.txtWixBanner = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.txtWixFileName = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.tbProgramType = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.configTabPage.SuspendLayout();
            this.keysTabPage.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.keysSignTab.SuspendLayout();
            this.keysCryptTab.SuspendLayout();
            this.licensesTabPage.SuspendLayout();
            this.srcTabPage.SuspendLayout();
            this.getAssemblyTabPage.SuspendLayout();
            this.makeDistribTabPage.SuspendLayout();
            this.SuspendLayout();
            //
            // tabControl1
            //
            this.tabControl1.Controls.Add(this.configTabPage);
            this.tabControl1.Controls.Add(this.keysTabPage);
            this.tabControl1.Controls.Add(this.licensesTabPage);
            this.tabControl1.Controls.Add(this.srcTabPage);
            this.tabControl1.Controls.Add(this.getAssemblyTabPage);
            this.tabControl1.Controls.Add(this.makeDistribTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(822, 624);
            this.tabControl1.TabIndex = 21;
            //
            // configTabPage
            //
            this.configTabPage.Controls.Add(this.settingsButton);
            this.configTabPage.Controls.Add(this.txtLoadConfFileName);
            this.configTabPage.Controls.Add(this.txtCreateConfFileName);
            this.configTabPage.Controls.Add(this.doConfigurationButton);
            this.configTabPage.Controls.Add(this.loadCfgRb);
            this.configTabPage.Controls.Add(this.createCfgRb);
            this.configTabPage.Location = new System.Drawing.Point(4, 22);
            this.configTabPage.Name = "configTabPage";
            this.configTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.configTabPage.Size = new System.Drawing.Size(814, 598);
            this.configTabPage.TabIndex = 0;
            this.configTabPage.Text = "Конфигурация";
            this.configTabPage.UseVisualStyleBackColor = true;
            //
            // settingsButton
            //
            this.settingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.settingsButton.FlatAppearance.BorderSize = 0;
            this.settingsButton.Image = global::MSSQLServerAuditor.Licenser.Properties.Resources.settings;
            this.settingsButton.Location = new System.Drawing.Point(6, 567);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(30, 26);
            this.settingsButton.TabIndex = 31;
            this.settingsButton.TabStop = false;
            this.toolTip1.SetToolTip(this.settingsButton, "Settings");
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.button1_Click_1);
            //
            // doConfigurationButton
            //
            this.doConfigurationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.doConfigurationButton.Location = new System.Drawing.Point(660, 554);
            this.doConfigurationButton.Name = "doConfigurationButton";
            this.doConfigurationButton.Size = new System.Drawing.Size(147, 38);
            this.doConfigurationButton.TabIndex = 26;
            this.doConfigurationButton.Text = "Выполнить";
            this.doConfigurationButton.UseVisualStyleBackColor = true;
            this.doConfigurationButton.Click += new System.EventHandler(this.button1_Click);
            //
            // loadCfgRb
            //
            this.loadCfgRb.AutoSize = true;
            this.loadCfgRb.Location = new System.Drawing.Point(6, 58);
            this.loadCfgRb.Name = "loadCfgRb";
            this.loadCfgRb.Size = new System.Drawing.Size(77, 17);
            this.loadCfgRb.TabIndex = 22;
            this.loadCfgRb.Text = "Загрузить";
            this.loadCfgRb.UseVisualStyleBackColor = true;
            this.loadCfgRb.CheckedChanged += new System.EventHandler(this.createCfgRb_CheckedChanged);
            //
            // createCfgRb
            //
            this.createCfgRb.AutoSize = true;
            this.createCfgRb.Checked = true;
            this.createCfgRb.Location = new System.Drawing.Point(6, 18);
            this.createCfgRb.Name = "createCfgRb";
            this.createCfgRb.Size = new System.Drawing.Size(67, 17);
            this.createCfgRb.TabIndex = 21;
            this.createCfgRb.TabStop = true;
            this.createCfgRb.Text = "Создать";
            this.createCfgRb.UseVisualStyleBackColor = true;
            this.createCfgRb.CheckedChanged += new System.EventHandler(this.createCfgRb_CheckedChanged);
            //
            // keysTabPage
            //
            this.keysTabPage.Controls.Add(this.tabControl2);
            this.keysTabPage.Controls.Add(this.keysSaveButton);
            this.keysTabPage.Location = new System.Drawing.Point(4, 22);
            this.keysTabPage.Name = "keysTabPage";
            this.keysTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.keysTabPage.Size = new System.Drawing.Size(814, 598);
            this.keysTabPage.TabIndex = 4;
            this.keysTabPage.Text = "Ключи";
            this.keysTabPage.UseVisualStyleBackColor = true;
            //
            // tabControl2
            //
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.keysSignTab);
            this.tabControl2.Controls.Add(this.keysCryptTab);
            this.tabControl2.Location = new System.Drawing.Point(9, 6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(797, 470);
            this.tabControl2.TabIndex = 28;
            //
            // keysSignTab
            //
            this.keysSignTab.Controls.Add(this.signNewButton);
            this.keysSignTab.Controls.Add(this.txtSignPriKey);
            this.keysSignTab.Controls.Add(this.signPrivateKeyLabel);
            this.keysSignTab.Controls.Add(this.txtSignPubKey);
            this.keysSignTab.Controls.Add(this.signPublicKeyLabel);
            this.keysSignTab.Location = new System.Drawing.Point(4, 22);
            this.keysSignTab.Name = "keysSignTab";
            this.keysSignTab.Padding = new System.Windows.Forms.Padding(3);
            this.keysSignTab.Size = new System.Drawing.Size(789, 444);
            this.keysSignTab.TabIndex = 0;
            this.keysSignTab.Text = "Подпись";
            this.keysSignTab.UseVisualStyleBackColor = true;
            //
            // signNewButton
            //
            this.signNewButton.Location = new System.Drawing.Point(454, 3);
            this.signNewButton.Name = "signNewButton";
            this.signNewButton.Size = new System.Drawing.Size(80, 22);
            this.signNewButton.TabIndex = 8;
            this.signNewButton.Text = "Новый";
            this.signNewButton.UseVisualStyleBackColor = true;
            this.signNewButton.Click += new System.EventHandler(this.button6_Click);
            //
            // txtSignPriKey
            //
            this.txtSignPriKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSignPriKey.Location = new System.Drawing.Point(6, 102);
            this.txtSignPriKey.Multiline = true;
            this.txtSignPriKey.Name = "txtSignPriKey";
            this.txtSignPriKey.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSignPriKey.Size = new System.Drawing.Size(775, 81);
            this.txtSignPriKey.TabIndex = 7;
            //
            // signPrivateKeyLabel
            //
            this.signPrivateKeyLabel.AutoSize = true;
            this.signPrivateKeyLabel.Location = new System.Drawing.Point(6, 86);
            this.signPrivateKeyLabel.Name = "signPrivateKeyLabel";
            this.signPrivateKeyLabel.Size = new System.Drawing.Size(62, 13);
            this.signPrivateKeyLabel.TabIndex = 6;
            this.signPrivateKeyLabel.Text = "Закрытый:";
            //
            // txtSignPubKey
            //
            this.txtSignPubKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSignPubKey.Location = new System.Drawing.Point(6, 27);
            this.txtSignPubKey.Multiline = true;
            this.txtSignPubKey.Name = "txtSignPubKey";
            this.txtSignPubKey.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSignPubKey.Size = new System.Drawing.Size(775, 56);
            this.txtSignPubKey.TabIndex = 5;
            //
            // signPublicKeyLabel
            //
            this.signPublicKeyLabel.AutoSize = true;
            this.signPublicKeyLabel.Location = new System.Drawing.Point(6, 12);
            this.signPublicKeyLabel.Name = "signPublicKeyLabel";
            this.signPublicKeyLabel.Size = new System.Drawing.Size(62, 13);
            this.signPublicKeyLabel.TabIndex = 4;
            this.signPublicKeyLabel.Text = "Открытый:";
            //
            // keysCryptTab
            //
            this.keysCryptTab.Controls.Add(this.cryptNewButton);
            this.keysCryptTab.Controls.Add(this.txtEncPriKey);
            this.keysCryptTab.Controls.Add(this.cryptPrivateKeyLabel);
            this.keysCryptTab.Controls.Add(this.txtEncPubKey);
            this.keysCryptTab.Controls.Add(this.cryptPublicKeyLabel);
            this.keysCryptTab.Location = new System.Drawing.Point(4, 22);
            this.keysCryptTab.Name = "keysCryptTab";
            this.keysCryptTab.Padding = new System.Windows.Forms.Padding(3);
            this.keysCryptTab.Size = new System.Drawing.Size(789, 444);
            this.keysCryptTab.TabIndex = 1;
            this.keysCryptTab.Text = "Шифрование";
            this.keysCryptTab.UseVisualStyleBackColor = true;
            //
            // cryptNewButton
            //
            this.cryptNewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cryptNewButton.Location = new System.Drawing.Point(702, 3);
            this.cryptNewButton.Name = "cryptNewButton";
            this.cryptNewButton.Size = new System.Drawing.Size(80, 22);
            this.cryptNewButton.TabIndex = 9;
            this.cryptNewButton.Text = "Новый";
            this.cryptNewButton.UseVisualStyleBackColor = true;
            this.cryptNewButton.Click += new System.EventHandler(this.button7_Click);
            //
            // txtEncPriKey
            //
            this.txtEncPriKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEncPriKey.Location = new System.Drawing.Point(6, 102);
            this.txtEncPriKey.Multiline = true;
            this.txtEncPriKey.Name = "txtEncPriKey";
            this.txtEncPriKey.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEncPriKey.Size = new System.Drawing.Size(775, 81);
            this.txtEncPriKey.TabIndex = 3;
            //
            // cryptPrivateKeyLabel
            //
            this.cryptPrivateKeyLabel.AutoSize = true;
            this.cryptPrivateKeyLabel.Location = new System.Drawing.Point(6, 86);
            this.cryptPrivateKeyLabel.Name = "cryptPrivateKeyLabel";
            this.cryptPrivateKeyLabel.Size = new System.Drawing.Size(62, 13);
            this.cryptPrivateKeyLabel.TabIndex = 2;
            this.cryptPrivateKeyLabel.Text = "Закрытый:";
            //
            // txtEncPubKey
            //
            this.txtEncPubKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEncPubKey.Location = new System.Drawing.Point(6, 27);
            this.txtEncPubKey.Multiline = true;
            this.txtEncPubKey.Name = "txtEncPubKey";
            this.txtEncPubKey.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEncPubKey.Size = new System.Drawing.Size(775, 56);
            this.txtEncPubKey.TabIndex = 1;
            //
            // cryptPublicKeyLabel
            //
            this.cryptPublicKeyLabel.AutoSize = true;
            this.cryptPublicKeyLabel.Location = new System.Drawing.Point(6, 12);
            this.cryptPublicKeyLabel.Name = "cryptPublicKeyLabel";
            this.cryptPublicKeyLabel.Size = new System.Drawing.Size(62, 13);
            this.cryptPublicKeyLabel.TabIndex = 0;
            this.cryptPublicKeyLabel.Text = "Открытый:";
            //
            // keysSaveButton
            //
            this.keysSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.keysSaveButton.Location = new System.Drawing.Point(660, 554);
            this.keysSaveButton.Name = "keysSaveButton";
            this.keysSaveButton.Size = new System.Drawing.Size(147, 38);
            this.keysSaveButton.TabIndex = 27;
            this.keysSaveButton.Text = "Запомнить";
            this.keysSaveButton.UseVisualStyleBackColor = true;
            this.keysSaveButton.Click += new System.EventHandler(this.button5_Click);
            //
            // licensesTabPage
            //
            this.licensesTabPage.Controls.Add(this.licenseToSignLabel);
            this.licensesTabPage.Controls.Add(this.subscribeLicenseButton);
            this.licensesTabPage.Controls.Add(this.licenseOutputLabel);
            this.licensesTabPage.Controls.Add(this.licenseToSignFileSelector);
            this.licensesTabPage.Controls.Add(this.signedLicenseOutFileSelector);
            this.licensesTabPage.Location = new System.Drawing.Point(4, 22);
            this.licensesTabPage.Name = "licensesTabPage";
            this.licensesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.licensesTabPage.Size = new System.Drawing.Size(814, 598);
            this.licensesTabPage.TabIndex = 5;
            this.licensesTabPage.Text = "Лицензии";
            this.licensesTabPage.UseVisualStyleBackColor = true;
            //
            // licenseToSignLabel
            //
            this.licenseToSignLabel.AutoSize = true;
            this.licenseToSignLabel.Location = new System.Drawing.Point(6, 20);
            this.licenseToSignLabel.Name = "licenseToSignLabel";
            this.licenseToSignLabel.Size = new System.Drawing.Size(109, 13);
            this.licenseToSignLabel.TabIndex = 44;
            this.licenseToSignLabel.Text = "Файл \"на подпись\":";
            //
            // subscribeLicenseButton
            //
            this.subscribeLicenseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.subscribeLicenseButton.Location = new System.Drawing.Point(663, 554);
            this.subscribeLicenseButton.Name = "subscribeLicenseButton";
            this.subscribeLicenseButton.Size = new System.Drawing.Size(147, 38);
            this.subscribeLicenseButton.TabIndex = 39;
            this.subscribeLicenseButton.Text = "Подписать";
            this.subscribeLicenseButton.UseVisualStyleBackColor = true;
            this.subscribeLicenseButton.Click += new System.EventHandler(this.subscribeLicenseButton_Click);
            //
            // licenseOutputLabel
            //
            this.licenseOutputLabel.AutoSize = true;
            this.licenseOutputLabel.Location = new System.Drawing.Point(6, 52);
            this.licenseOutputLabel.Name = "licenseOutputLabel";
            this.licenseOutputLabel.Size = new System.Drawing.Size(89, 13);
            this.licenseOutputLabel.TabIndex = 37;
            this.licenseOutputLabel.Text = "Выходной файл:";
            //
            // srcTabPage
            //
            this.srcTabPage.Controls.Add(this.tbProgramType);
            this.srcTabPage.Controls.Add(this.label1);
            this.srcTabPage.Controls.Add(this.tbLicenseFileName);
            this.srcTabPage.Controls.Add(this.еиДшсутыуАшдуТфьу);
            this.srcTabPage.Controls.Add(this.lbRu);
            this.srcTabPage.Controls.Add(this.lbProgramName);
            this.srcTabPage.Controls.Add(this.tbProgramName);
            this.srcTabPage.Controls.Add(this.tbShorcName);
            this.srcTabPage.Controls.Add(this.lbShorcName);
            this.srcTabPage.Controls.Add(this.lbDirName);
            this.srcTabPage.Controls.Add(this.tbDirName);
            this.srcTabPage.Controls.Add(this.tbFileName);
            this.srcTabPage.Controls.Add(this.lbFileName);
            this.srcTabPage.Controls.Add(this.tbAdditionalTemplates);
            this.srcTabPage.Controls.Add(this.btAdditionalTemplates);
            this.srcTabPage.Controls.Add(this.lblAdditionalTemplate);
            this.srcTabPage.Controls.Add(this.lblAppPath);
            this.srcTabPage.Controls.Add(this.lbAdditionalSql);
            this.srcTabPage.Controls.Add(this.tbAdditionalSql);
            this.srcTabPage.Controls.Add(this.addTemplateFileAsRelativePathCheckBox);
            this.srcTabPage.Controls.Add(this.removeTemplateButton);
            this.srcTabPage.Controls.Add(this.addTemplateButton);
            this.srcTabPage.Controls.Add(this.fsLicenseFolder);
            this.srcTabPage.Controls.Add(this.fileAppPath);
            this.srcTabPage.Controls.Add(this.templeateFilesListBox);
            this.srcTabPage.Controls.Add(this.templatesLabel);
            this.srcTabPage.Controls.Add(this.sourceFolderLabel);
            this.srcTabPage.Controls.Add(this.userSettingsFileLabel);
            this.srcTabPage.Controls.Add(this.sysSettingsFileLabel);
            this.srcTabPage.Controls.Add(this.langFileLabel);
            this.srcTabPage.Controls.Add(this.srcFilesSignButton);
            this.srcTabPage.Controls.Add(this.dstFolderLabel);
            this.srcTabPage.Controls.Add(this.txtSrcFolder);
            this.srcTabPage.Controls.Add(this.txtUserSettingsFileName);
            this.srcTabPage.Controls.Add(this.txtDstFolder);
            this.srcTabPage.Controls.Add(this.txtLangFileName);
            this.srcTabPage.Controls.Add(this.txtSysSettingsFileName);
            this.srcTabPage.Location = new System.Drawing.Point(4, 22);
            this.srcTabPage.Name = "srcTabPage";
            this.srcTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.srcTabPage.Size = new System.Drawing.Size(814, 598);
            this.srcTabPage.TabIndex = 1;
            this.srcTabPage.Text = "Исходные данные";
            this.srcTabPage.UseVisualStyleBackColor = true;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 564);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 72;
            this.label1.Text = "Тип программы";
            //
            // tbLicenseFileName
            //
            this.tbLicenseFileName.Location = new System.Drawing.Point(138, 530);
            this.tbLicenseFileName.Name = "tbLicenseFileName";
            this.tbLicenseFileName.Size = new System.Drawing.Size(663, 20);
            this.tbLicenseFileName.TabIndex = 70;
            //
            // еиДшсутыуАшдуТфьу
            //
            this.еиДшсутыуАшдуТфьу.AutoSize = true;
            this.еиДшсутыуАшдуТфьу.Location = new System.Drawing.Point(16, 530);
            this.еиДшсутыуАшдуТфьу.Name = "еиДшсутыуАшдуТфьу";
            this.еиДшсутыуАшдуТфьу.Size = new System.Drawing.Size(116, 13);
            this.еиДшсутыуАшдуТфьу.TabIndex = 66;
            this.еиДшсутыуАшдуТфьу.Text = "Название файла лац.";
            //
            // lbRu
            //
            this.lbRu.AutoSize = true;
            this.lbRu.Location = new System.Drawing.Point(16, 504);
            this.lbRu.Name = "lbRu";
            this.lbRu.Size = new System.Drawing.Size(92, 13);
            this.lbRu.TabIndex = 64;
            this.lbRu.Text = "Папка лиц. согл.";
            //
            // lbProgramName
            //
            this.lbProgramName.AutoSize = true;
            this.lbProgramName.Location = new System.Drawing.Point(13, 478);
            this.lbProgramName.Name = "lbProgramName";
            this.lbProgramName.Size = new System.Drawing.Size(122, 13);
            this.lbProgramName.TabIndex = 62;
            this.lbProgramName.Text = "Название программы:";
            //
            // tbProgramName
            //
            this.tbProgramName.Location = new System.Drawing.Point(138, 475);
            this.tbProgramName.Name = "tbProgramName";
            this.tbProgramName.Size = new System.Drawing.Size(663, 20);
            this.tbProgramName.TabIndex = 61;
            //
            // tbShorcName
            //
            this.tbShorcName.Location = new System.Drawing.Point(138, 448);
            this.tbShorcName.Name = "tbShorcName";
            this.tbShorcName.Size = new System.Drawing.Size(663, 20);
            this.tbShorcName.TabIndex = 60;
            //
            // lbShorcName
            //
            this.lbShorcName.AutoSize = true;
            this.lbShorcName.Location = new System.Drawing.Point(13, 451);
            this.lbShorcName.Name = "lbShorcName";
            this.lbShorcName.Size = new System.Drawing.Size(101, 13);
            this.lbShorcName.TabIndex = 59;
            this.lbShorcName.Text = "Название ярлыка:";
            //
            // lbDirName
            //
            this.lbDirName.AutoSize = true;
            this.lbDirName.Location = new System.Drawing.Point(13, 417);
            this.lbDirName.Name = "lbDirName";
            this.lbDirName.Size = new System.Drawing.Size(93, 13);
            this.lbDirName.TabIndex = 58;
            this.lbDirName.Text = "Название папки:";
            //
            // tbDirName
            //
            this.tbDirName.Location = new System.Drawing.Point(138, 417);
            this.tbDirName.Name = "tbDirName";
            this.tbDirName.Size = new System.Drawing.Size(663, 20);
            this.tbDirName.TabIndex = 57;
            //
            // tbFileName
            //
            this.tbFileName.Location = new System.Drawing.Point(138, 390);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(663, 20);
            this.tbFileName.TabIndex = 56;
            //
            // lbFileName
            //
            this.lbFileName.AutoSize = true;
            this.lbFileName.Location = new System.Drawing.Point(10, 390);
            this.lbFileName.Name = "lbFileName";
            this.lbFileName.Size = new System.Drawing.Size(121, 13);
            this.lbFileName.TabIndex = 55;
            this.lbFileName.Text = "Название .exe файла: ";
            //
            // tbAdditionalTemplates
            //
            this.tbAdditionalTemplates.Enabled = false;
            this.tbAdditionalTemplates.Location = new System.Drawing.Point(219, 306);
            this.tbAdditionalTemplates.Margin = new System.Windows.Forms.Padding(2);
            this.tbAdditionalTemplates.Name = "tbAdditionalTemplates";
            this.tbAdditionalTemplates.Size = new System.Drawing.Size(490, 20);
            this.tbAdditionalTemplates.TabIndex = 54;
            //
            // btAdditionalTemplates
            //
            this.btAdditionalTemplates.Location = new System.Drawing.Point(713, 305);
            this.btAdditionalTemplates.Margin = new System.Windows.Forms.Padding(2);
            this.btAdditionalTemplates.Name = "btAdditionalTemplates";
            this.btAdditionalTemplates.Size = new System.Drawing.Size(88, 21);
            this.btAdditionalTemplates.TabIndex = 53;
            this.btAdditionalTemplates.Text = " Просмотр";
            this.btAdditionalTemplates.UseVisualStyleBackColor = true;
            this.btAdditionalTemplates.Click += new System.EventHandler(this.btAdditionalTemplates_Click);
            //
            // lblAdditionalTemplate
            //
            this.lblAdditionalTemplate.Location = new System.Drawing.Point(10, 306);
            this.lblAdditionalTemplate.Name = "lblAdditionalTemplate";
            this.lblAdditionalTemplate.Size = new System.Drawing.Size(237, 17);
            this.lblAdditionalTemplate.TabIndex = 51;
            this.lblAdditionalTemplate.Text = "Дополнительные HTML для отчетов:";
            //
            // lblAppPath
            //
            this.lblAppPath.Location = new System.Drawing.Point(10, 14);
            this.lblAppPath.Name = "lblAppPath";
            this.lblAppPath.Size = new System.Drawing.Size(125, 22);
            this.lblAppPath.TabIndex = 49;
            this.lblAppPath.Text = "Exe файл:";
            //
            // lbAdditionalSql
            //
            this.lbAdditionalSql.Location = new System.Drawing.Point(9, 333);
            this.lbAdditionalSql.Name = "lbAdditionalSql";
            this.lbAdditionalSql.Size = new System.Drawing.Size(119, 48);
            this.lbAdditionalSql.TabIndex = 48;
            this.lbAdditionalSql.Text = "Дополнительный SQL для всех запросов:";
            this.lbAdditionalSql.Click += new System.EventHandler(this.label1_Click);
            //
            // tbAdditionalSql
            //
            this.tbAdditionalSql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAdditionalSql.Location = new System.Drawing.Point(138, 330);
            this.tbAdditionalSql.Multiline = true;
            this.tbAdditionalSql.Name = "tbAdditionalSql";
            this.tbAdditionalSql.Size = new System.Drawing.Size(663, 51);
            this.tbAdditionalSql.TabIndex = 47;
            this.tbAdditionalSql.Text = "RAISERROR (\'Some error\', 16, 1);";
            //
            // addTemplateFileAsRelativePathCheckBox
            //
            this.addTemplateFileAsRelativePathCheckBox.AutoSize = true;
            this.addTemplateFileAsRelativePathCheckBox.Checked = true;
            this.addTemplateFileAsRelativePathCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addTemplateFileAsRelativePathCheckBox.Location = new System.Drawing.Point(138, 114);
            this.addTemplateFileAsRelativePathCheckBox.Name = "addTemplateFileAsRelativePathCheckBox";
            this.addTemplateFileAsRelativePathCheckBox.Size = new System.Drawing.Size(244, 17);
            this.addTemplateFileAsRelativePathCheckBox.TabIndex = 46;
            this.addTemplateFileAsRelativePathCheckBox.Text = "use relative paths for new added template files";
            this.addTemplateFileAsRelativePathCheckBox.UseVisualStyleBackColor = true;
            this.addTemplateFileAsRelativePathCheckBox.Visible = false;
            //
            // removeTemplateButton
            //
            this.removeTemplateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeTemplateButton.Image = global::MSSQLServerAuditor.Licenser.Properties.Resources.minus;
            this.removeTemplateButton.Location = new System.Drawing.Point(769, 68);
            this.removeTemplateButton.Name = "removeTemplateButton";
            this.removeTemplateButton.Size = new System.Drawing.Size(32, 22);
            this.removeTemplateButton.TabIndex = 45;
            this.removeTemplateButton.UseVisualStyleBackColor = true;
            this.removeTemplateButton.Click += new System.EventHandler(this.removeTemplateButton_Click);
            //
            // addTemplateButton
            //
            this.addTemplateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addTemplateButton.Image = global::MSSQLServerAuditor.Licenser.Properties.Resources.plus;
            this.addTemplateButton.Location = new System.Drawing.Point(769, 39);
            this.addTemplateButton.Name = "addTemplateButton";
            this.addTemplateButton.Size = new System.Drawing.Size(32, 22);
            this.addTemplateButton.TabIndex = 44;
            this.addTemplateButton.UseVisualStyleBackColor = true;
            this.addTemplateButton.Click += new System.EventHandler(this.addTemplateButton_Click);
            //
            // templeateFilesListBox
            //
            this.templeateFilesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.templeateFilesListBox.FormattingEnabled = true;
            this.templeateFilesListBox.Location = new System.Drawing.Point(138, 40);
            this.templeateFilesListBox.Name = "templeateFilesListBox";
            this.templeateFilesListBox.Size = new System.Drawing.Size(610, 82);
            this.templeateFilesListBox.TabIndex = 43;
            this.templeateFilesListBox.SelectedIndexChanged += new System.EventHandler(this.templeateFilesListBox_SelectedIndexChanged);
            //
            // templatesLabel
            //
            this.templatesLabel.AutoSize = true;
            this.templatesLabel.Location = new System.Drawing.Point(10, 40);
            this.templatesLabel.Name = "templatesLabel";
            this.templatesLabel.Size = new System.Drawing.Size(57, 13);
            this.templatesLabel.TabIndex = 42;
            this.templatesLabel.Text = "Шаблоны:";
            //
            // sourceFolderLabel
            //
            this.sourceFolderLabel.AutoSize = true;
            this.sourceFolderLabel.Location = new System.Drawing.Point(10, 178);
            this.sourceFolderLabel.Name = "sourceFolderLabel";
            this.sourceFolderLabel.Size = new System.Drawing.Size(117, 13);
            this.sourceFolderLabel.TabIndex = 40;
            this.sourceFolderLabel.Text = "Папка конфигурации:";
            //
            // userSettingsFileLabel
            //
            this.userSettingsFileLabel.AutoSize = true;
            this.userSettingsFileLabel.Location = new System.Drawing.Point(10, 280);
            this.userSettingsFileLabel.Name = "userSettingsFileLabel";
            this.userSettingsFileLabel.Size = new System.Drawing.Size(110, 13);
            this.userSettingsFileLabel.TabIndex = 38;
            this.userSettingsFileLabel.Text = "  пользовательских:";
            //
            // sysSettingsFileLabel
            //
            this.sysSettingsFileLabel.AutoSize = true;
            this.sysSettingsFileLabel.Location = new System.Drawing.Point(10, 245);
            this.sysSettingsFileLabel.Name = "sysSettingsFileLabel";
            this.sysSettingsFileLabel.Size = new System.Drawing.Size(113, 13);
            this.sysSettingsFileLabel.TabIndex = 31;
            this.sysSettingsFileLabel.Text = "Файл cис. настроек:";
            //
            // langFileLabel
            //
            this.langFileLabel.AutoSize = true;
            this.langFileLabel.Location = new System.Drawing.Point(10, 210);
            this.langFileLabel.Name = "langFileLabel";
            this.langFileLabel.Size = new System.Drawing.Size(74, 13);
            this.langFileLabel.TabIndex = 28;
            this.langFileLabel.Text = "Файл языка:";
            //
            // srcFilesSignButton
            //
            this.srcFilesSignButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.srcFilesSignButton.Location = new System.Drawing.Point(662, 554);
            this.srcFilesSignButton.Name = "srcFilesSignButton";
            this.srcFilesSignButton.Size = new System.Drawing.Size(147, 38);
            this.srcFilesSignButton.TabIndex = 27;
            this.srcFilesSignButton.Text = "Подписать";
            this.srcFilesSignButton.UseVisualStyleBackColor = true;
            this.srcFilesSignButton.Click += new System.EventHandler(this.button2_Click);
            //
            // dstFolderLabel
            //
            this.dstFolderLabel.AutoSize = true;
            this.dstFolderLabel.Location = new System.Drawing.Point(10, 148);
            this.dstFolderLabel.Name = "dstFolderLabel";
            this.dstFolderLabel.Size = new System.Drawing.Size(102, 13);
            this.dstFolderLabel.TabIndex = 23;
            this.dstFolderLabel.Text = "Папка для записи:";
            //
            // getAssemblyTabPage
            //
            this.getAssemblyTabPage.Controls.Add(this.MSSQLAuditorVersion);
            this.getAssemblyTabPage.Controls.Add(this.dnGuardExeLabel);
            this.getAssemblyTabPage.Controls.Add(this.dnGuardExeNameTextBox);
            this.getAssemblyTabPage.Controls.Add(this.resetDnGuardOptionsButton);
            this.getAssemblyTabPage.Controls.Add(this.dnGuardOptionsTextBox);
            this.getAssemblyTabPage.Controls.Add(this.dnGuardOptionsLabel);
            this.getAssemblyTabPage.Controls.Add(this.platformComboBox);
            this.getAssemblyTabPage.Controls.Add(this.chbDnGuardUse);
            this.getAssemblyTabPage.Controls.Add(this.dnGuardFolderLabel);
            this.getAssemblyTabPage.Controls.Add(this.txtNetVersion);
            this.getAssemblyTabPage.Controls.Add(this.frameworkVerLabel);
            this.getAssemblyTabPage.Controls.Add(this.frameworkFolderLabel);
            this.getAssemblyTabPage.Controls.Add(this.buildApplicationButton);
            this.getAssemblyTabPage.Controls.Add(this.txtDnGuardFolder);
            this.getAssemblyTabPage.Controls.Add(this.txtNetFolder);
            this.getAssemblyTabPage.Location = new System.Drawing.Point(4, 22);
            this.getAssemblyTabPage.Name = "getAssemblyTabPage";
            this.getAssemblyTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.getAssemblyTabPage.Size = new System.Drawing.Size(814, 598);
            this.getAssemblyTabPage.TabIndex = 2;
            this.getAssemblyTabPage.Text = "Сбор приложения";
            this.getAssemblyTabPage.UseVisualStyleBackColor = true;
            //
            // MSSQLAuditorVersion
            //
            this.MSSQLAuditorVersion.AutoSize = true;
            this.MSSQLAuditorVersion.Location = new System.Drawing.Point(12, 386);
            this.MSSQLAuditorVersion.Name = "MSSQLAuditorVersion";
            this.MSSQLAuditorVersion.Size = new System.Drawing.Size(0, 13);
            this.MSSQLAuditorVersion.TabIndex = 44;
            //
            // dnGuardExeLabel
            //
            this.dnGuardExeLabel.Location = new System.Drawing.Point(3, 121);
            this.dnGuardExeLabel.Name = "dnGuardExeLabel";
            this.dnGuardExeLabel.Size = new System.Drawing.Size(140, 17);
            this.dnGuardExeLabel.TabIndex = 43;
            this.dnGuardExeLabel.Text = "DNGuard exe name:";
            this.dnGuardExeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // dnGuardExeNameTextBox
            //
            this.dnGuardExeNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dnGuardExeNameTextBox.CausesValidation = false;
            this.dnGuardExeNameTextBox.Location = new System.Drawing.Point(159, 118);
            this.dnGuardExeNameTextBox.Name = "dnGuardExeNameTextBox";
            this.dnGuardExeNameTextBox.Size = new System.Drawing.Size(592, 20);
            this.dnGuardExeNameTextBox.TabIndex = 42;
            //
            // resetDnGuardOptionsButton
            //
            this.resetDnGuardOptionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resetDnGuardOptionsButton.Location = new System.Drawing.Point(570, 196);
            this.resetDnGuardOptionsButton.Name = "resetDnGuardOptionsButton";
            this.resetDnGuardOptionsButton.Size = new System.Drawing.Size(182, 22);
            this.resetDnGuardOptionsButton.TabIndex = 41;
            this.resetDnGuardOptionsButton.Text = "Параметры по-умолчанию";
            this.resetDnGuardOptionsButton.UseVisualStyleBackColor = true;
            this.resetDnGuardOptionsButton.Click += new System.EventHandler(this.resetDnGuardOptionsButton_Click);
            //
            // dnGuardOptionsTextBox
            //
            this.dnGuardOptionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dnGuardOptionsTextBox.CausesValidation = false;
            this.dnGuardOptionsTextBox.Location = new System.Drawing.Point(159, 166);
            this.dnGuardOptionsTextBox.Name = "dnGuardOptionsTextBox";
            this.dnGuardOptionsTextBox.Size = new System.Drawing.Size(592, 20);
            this.dnGuardOptionsTextBox.TabIndex = 40;
            //
            // dnGuardOptionsLabel
            //
            this.dnGuardOptionsLabel.Location = new System.Drawing.Point(12, 174);
            this.dnGuardOptionsLabel.Name = "dnGuardOptionsLabel";
            this.dnGuardOptionsLabel.Size = new System.Drawing.Size(131, 13);
            this.dnGuardOptionsLabel.TabIndex = 39;
            this.dnGuardOptionsLabel.Text = "параметры";
            this.dnGuardOptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // platformComboBox
            //
            this.platformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platformComboBox.FormattingEnabled = true;
            this.platformComboBox.Items.AddRange(new object[] {
            "x86",
            "x64 | /x64opt "});
            this.platformComboBox.Location = new System.Drawing.Point(159, 198);
            this.platformComboBox.Name = "platformComboBox";
            this.platformComboBox.Size = new System.Drawing.Size(106, 21);
            this.platformComboBox.TabIndex = 38;
            //
            // chbDnGuardUse
            //
            this.chbDnGuardUse.AutoSize = true;
            this.chbDnGuardUse.Location = new System.Drawing.Point(159, 144);
            this.chbDnGuardUse.Name = "chbDnGuardUse";
            this.chbDnGuardUse.Size = new System.Drawing.Size(99, 17);
            this.chbDnGuardUse.TabIndex = 37;
            this.chbDnGuardUse.Text = "Использовать";
            this.chbDnGuardUse.UseVisualStyleBackColor = true;
            this.chbDnGuardUse.CheckedChanged += new System.EventHandler(this.chbDnGuardUse_CheckedChanged);
            //
            // dnGuardFolderLabel
            //
            this.dnGuardFolderLabel.Location = new System.Drawing.Point(12, 98);
            this.dnGuardFolderLabel.Name = "dnGuardFolderLabel";
            this.dnGuardFolderLabel.Size = new System.Drawing.Size(134, 13);
            this.dnGuardFolderLabel.TabIndex = 35;
            this.dnGuardFolderLabel.Text = "Папка DNGuard:";
            this.dnGuardFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // txtNetVersion
            //
            this.txtNetVersion.Location = new System.Drawing.Point(159, 43);
            this.txtNetVersion.Name = "txtNetVersion";
            this.txtNetVersion.Size = new System.Drawing.Size(53, 20);
            this.txtNetVersion.TabIndex = 33;
            //
            // frameworkVerLabel
            //
            this.frameworkVerLabel.AutoSize = true;
            this.frameworkVerLabel.Location = new System.Drawing.Point(95, 46);
            this.frameworkVerLabel.Name = "frameworkVerLabel";
            this.frameworkVerLabel.Size = new System.Drawing.Size(47, 13);
            this.frameworkVerLabel.TabIndex = 32;
            this.frameworkVerLabel.Text = "Версия:";
            //
            // frameworkFolderLabel
            //
            this.frameworkFolderLabel.AutoSize = true;
            this.frameworkFolderLabel.Location = new System.Drawing.Point(9, 21);
            this.frameworkFolderLabel.Name = "frameworkFolderLabel";
            this.frameworkFolderLabel.Size = new System.Drawing.Size(132, 13);
            this.frameworkFolderLabel.TabIndex = 29;
            this.frameworkFolderLabel.Text = "Папка платформы .NET:";
            //
            // buildApplicationButton
            //
            this.buildApplicationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buildApplicationButton.Location = new System.Drawing.Point(660, 554);
            this.buildApplicationButton.Name = "buildApplicationButton";
            this.buildApplicationButton.Size = new System.Drawing.Size(147, 38);
            this.buildApplicationButton.TabIndex = 28;
            this.buildApplicationButton.Text = "Выполнить";
            this.buildApplicationButton.UseVisualStyleBackColor = true;
            this.buildApplicationButton.Click += new System.EventHandler(this.button3_Click);
            //
            // makeDistribTabPage
            //
            this.makeDistribTabPage.Controls.Add(this.installPackPutputFileLabel);
            this.makeDistribTabPage.Controls.Add(this.wixFolderLabel);
            this.makeDistribTabPage.Controls.Add(this.billboardLabel);
            this.makeDistribTabPage.Controls.Add(this.buildInstallPackButton);
            this.makeDistribTabPage.Controls.Add(this.wixFileLabel);
            this.makeDistribTabPage.Controls.Add(this.txtOutputMsi);
            this.makeDistribTabPage.Controls.Add(this.txtWixFolder);
            this.makeDistribTabPage.Controls.Add(this.txtWixBanner);
            this.makeDistribTabPage.Controls.Add(this.txtWixFileName);
            this.makeDistribTabPage.Location = new System.Drawing.Point(4, 22);
            this.makeDistribTabPage.Name = "makeDistribTabPage";
            this.makeDistribTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.makeDistribTabPage.Size = new System.Drawing.Size(814, 598);
            this.makeDistribTabPage.TabIndex = 3;
            this.makeDistribTabPage.Text = "Сбор дистрибутива";
            this.makeDistribTabPage.UseVisualStyleBackColor = true;
            //
            // installPackPutputFileLabel
            //
            this.installPackPutputFileLabel.AutoSize = true;
            this.installPackPutputFileLabel.Location = new System.Drawing.Point(6, 126);
            this.installPackPutputFileLabel.Name = "installPackPutputFileLabel";
            this.installPackPutputFileLabel.Size = new System.Drawing.Size(89, 13);
            this.installPackPutputFileLabel.TabIndex = 42;
            this.installPackPutputFileLabel.Text = "Выходной файл:";
            //
            // wixFolderLabel
            //
            this.wixFolderLabel.AutoSize = true;
            this.wixFolderLabel.Location = new System.Drawing.Point(6, 22);
            this.wixFolderLabel.Name = "wixFolderLabel";
            this.wixFolderLabel.Size = new System.Drawing.Size(63, 13);
            this.wixFolderLabel.TabIndex = 40;
            this.wixFolderLabel.Text = "Папка Wix:";
            //
            // billboardLabel
            //
            this.billboardLabel.AutoSize = true;
            this.billboardLabel.Location = new System.Drawing.Point(9, 93);
            this.billboardLabel.Name = "billboardLabel";
            this.billboardLabel.Size = new System.Drawing.Size(47, 13);
            this.billboardLabel.TabIndex = 38;
            this.billboardLabel.Text = "Баннер:";
            //
            // buildInstallPackButton
            //
            this.buildInstallPackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buildInstallPackButton.Location = new System.Drawing.Point(657, 554);
            this.buildInstallPackButton.Name = "buildInstallPackButton";
            this.buildInstallPackButton.Size = new System.Drawing.Size(147, 38);
            this.buildInstallPackButton.TabIndex = 37;
            this.buildInstallPackButton.Text = "Выполнить";
            this.buildInstallPackButton.UseVisualStyleBackColor = true;
            this.buildInstallPackButton.Click += new System.EventHandler(this.button4_Click);
            //
            // wixFileLabel
            //
            this.wixFileLabel.AutoSize = true;
            this.wixFileLabel.Location = new System.Drawing.Point(6, 54);
            this.wixFileLabel.Name = "wixFileLabel";
            this.wixFileLabel.Size = new System.Drawing.Size(60, 13);
            this.wixFileLabel.TabIndex = 35;
            this.wixFileLabel.Text = "Файл Wix:";
            //
            // splitter1
            //
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 624);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(822, 3);
            this.splitter1.TabIndex = 22;
            this.splitter1.TabStop = false;
            //
            // logTextBox
            //
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(0, 627);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTextBox.Size = new System.Drawing.Size(822, 70);
            this.logTextBox.TabIndex = 23;
            //
            // txtLoadConfFileName
            //
            this.txtLoadConfFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLoadConfFileName.Enabled = false;
            this.txtLoadConfFileName.Filter = "Файлы XML|*.xml";
            this.txtLoadConfFileName.IsCorrectFile = null;
            this.txtLoadConfFileName.Location = new System.Drawing.Point(115, 54);
            this.txtLoadConfFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtLoadConfFileName.Name = "txtLoadConfFileName";
            this.txtLoadConfFileName.Save = false;
            this.txtLoadConfFileName.Size = new System.Drawing.Size(690, 27);
            this.txtLoadConfFileName.TabIndex = 28;
            //
            // txtCreateConfFileName
            //
            this.txtCreateConfFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCreateConfFileName.Enabled = false;
            this.txtCreateConfFileName.Filter = "Файлы XML|*.xml";
            this.txtCreateConfFileName.IsCorrectFile = null;
            this.txtCreateConfFileName.Location = new System.Drawing.Point(115, 14);
            this.txtCreateConfFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtCreateConfFileName.Name = "txtCreateConfFileName";
            this.txtCreateConfFileName.Save = true;
            this.txtCreateConfFileName.Size = new System.Drawing.Size(690, 27);
            this.txtCreateConfFileName.TabIndex = 27;
            //
            // licenseToSignFileSelector
            //
            this.licenseToSignFileSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.licenseToSignFileSelector.Filter = null;
            this.licenseToSignFileSelector.IsCorrectFile = null;
            this.licenseToSignFileSelector.Location = new System.Drawing.Point(130, 14);
            this.licenseToSignFileSelector.Margin = new System.Windows.Forms.Padding(4);
            this.licenseToSignFileSelector.Name = "licenseToSignFileSelector";
            this.licenseToSignFileSelector.Save = false;
            this.licenseToSignFileSelector.Size = new System.Drawing.Size(675, 27);
            this.licenseToSignFileSelector.TabIndex = 45;
            //
            // signedLicenseOutFileSelector
            //
            this.signedLicenseOutFileSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.signedLicenseOutFileSelector.Filter = null;
            this.signedLicenseOutFileSelector.IsCorrectFile = null;
            this.signedLicenseOutFileSelector.Location = new System.Drawing.Point(130, 50);
            this.signedLicenseOutFileSelector.Margin = new System.Windows.Forms.Padding(4);
            this.signedLicenseOutFileSelector.Name = "signedLicenseOutFileSelector";
            this.signedLicenseOutFileSelector.Save = true;
            this.signedLicenseOutFileSelector.Size = new System.Drawing.Size(675, 27);
            this.signedLicenseOutFileSelector.TabIndex = 38;
            //
            // fsLicenseFolder
            //
            this.fsLicenseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fsLicenseFolder.IsRelative = false;
            this.fsLicenseFolder.Location = new System.Drawing.Point(135, 504);
            this.fsLicenseFolder.Margin = new System.Windows.Forms.Padding(4);
            this.fsLicenseFolder.Name = "fsLicenseFolder";
            this.fsLicenseFolder.Size = new System.Drawing.Size(666, 27);
            this.fsLicenseFolder.TabIndex = 69;
            //
            // fileAppPath
            //
            this.fileAppPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileAppPath.Filter = null;
            this.fileAppPath.IsCorrectFile = null;
            this.fileAppPath.Location = new System.Drawing.Point(135, 7);
            this.fileAppPath.Margin = new System.Windows.Forms.Padding(4);
            this.fileAppPath.Name = "fileAppPath";
            this.fileAppPath.Save = false;
            this.fileAppPath.Size = new System.Drawing.Size(669, 27);
            this.fileAppPath.TabIndex = 50;
            //
            // txtSrcFolder
            //
            this.txtSrcFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSrcFolder.IsRelative = false;
            this.txtSrcFolder.Location = new System.Drawing.Point(137, 172);
            this.txtSrcFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtSrcFolder.Name = "txtSrcFolder";
            this.txtSrcFolder.Size = new System.Drawing.Size(668, 27);
            this.txtSrcFolder.TabIndex = 41;
            //
            // txtUserSettingsFileName
            //
            this.txtUserSettingsFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserSettingsFileName.Filter = null;
            this.txtUserSettingsFileName.IsCorrectFile = null;
            this.txtUserSettingsFileName.Location = new System.Drawing.Point(137, 272);
            this.txtUserSettingsFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserSettingsFileName.Name = "txtUserSettingsFileName";
            this.txtUserSettingsFileName.Save = false;
            this.txtUserSettingsFileName.Size = new System.Drawing.Size(668, 27);
            this.txtUserSettingsFileName.TabIndex = 39;
            //
            // txtDstFolder
            //
            this.txtDstFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDstFolder.IsRelative = false;
            this.txtDstFolder.Location = new System.Drawing.Point(137, 140);
            this.txtDstFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtDstFolder.Name = "txtDstFolder";
            this.txtDstFolder.Size = new System.Drawing.Size(668, 27);
            this.txtDstFolder.TabIndex = 37;
            //
            // txtLangFileName
            //
            this.txtLangFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLangFileName.Filter = null;
            this.txtLangFileName.IsCorrectFile = null;
            this.txtLangFileName.Location = new System.Drawing.Point(137, 206);
            this.txtLangFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtLangFileName.Name = "txtLangFileName";
            this.txtLangFileName.Save = false;
            this.txtLangFileName.Size = new System.Drawing.Size(668, 27);
            this.txtLangFileName.TabIndex = 36;
            //
            // txtSysSettingsFileName
            //
            this.txtSysSettingsFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSysSettingsFileName.Filter = null;
            this.txtSysSettingsFileName.IsCorrectFile = null;
            this.txtSysSettingsFileName.Location = new System.Drawing.Point(137, 238);
            this.txtSysSettingsFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtSysSettingsFileName.Name = "txtSysSettingsFileName";
            this.txtSysSettingsFileName.Save = false;
            this.txtSysSettingsFileName.Size = new System.Drawing.Size(668, 27);
            this.txtSysSettingsFileName.TabIndex = 35;
            //
            // txtDnGuardFolder
            //
            this.txtDnGuardFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDnGuardFolder.IsRelative = false;
            this.txtDnGuardFolder.Location = new System.Drawing.Point(156, 89);
            this.txtDnGuardFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtDnGuardFolder.Name = "txtDnGuardFolder";
            this.txtDnGuardFolder.Size = new System.Drawing.Size(649, 27);
            this.txtDnGuardFolder.TabIndex = 36;
            //
            // txtNetFolder
            //
            this.txtNetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNetFolder.IsRelative = false;
            this.txtNetFolder.Location = new System.Drawing.Point(156, 14);
            this.txtNetFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtNetFolder.Name = "txtNetFolder";
            this.txtNetFolder.Size = new System.Drawing.Size(649, 27);
            this.txtNetFolder.TabIndex = 34;
            //
            // txtOutputMsi
            //
            this.txtOutputMsi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputMsi.Filter = "Файлы Windows Installer|*.msi";
            this.txtOutputMsi.IsCorrectFile = null;
            this.txtOutputMsi.Location = new System.Drawing.Point(129, 120);
            this.txtOutputMsi.Margin = new System.Windows.Forms.Padding(4);
            this.txtOutputMsi.Name = "txtOutputMsi";
            this.txtOutputMsi.Save = true;
            this.txtOutputMsi.Size = new System.Drawing.Size(675, 27);
            this.txtOutputMsi.TabIndex = 43;
            //
            // txtWixFolder
            //
            this.txtWixFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWixFolder.IsRelative = false;
            this.txtWixFolder.Location = new System.Drawing.Point(129, 14);
            this.txtWixFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtWixFolder.Name = "txtWixFolder";
            this.txtWixFolder.Size = new System.Drawing.Size(675, 27);
            this.txtWixFolder.TabIndex = 41;
            //
            // txtWixBanner
            //
            this.txtWixBanner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWixBanner.Filter = "Файлы BMP|*.bmp";
            this.txtWixBanner.IsCorrectFile = null;
            this.txtWixBanner.Location = new System.Drawing.Point(129, 86);
            this.txtWixBanner.Margin = new System.Windows.Forms.Padding(4);
            this.txtWixBanner.Name = "txtWixBanner";
            this.txtWixBanner.Save = false;
            this.txtWixBanner.Size = new System.Drawing.Size(675, 27);
            this.txtWixBanner.TabIndex = 39;
            //
            // txtWixFileName
            //
            this.txtWixFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWixFileName.Filter = "Файлы Wix|*.wxs";
            this.txtWixFileName.IsCorrectFile = null;
            this.txtWixFileName.Location = new System.Drawing.Point(129, 50);
            this.txtWixFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtWixFileName.Name = "txtWixFileName";
            this.txtWixFileName.Save = false;
            this.txtWixFileName.Size = new System.Drawing.Size(675, 27);
            this.txtWixFileName.TabIndex = 36;
            //
            // tbProgramType
            //
            this.tbProgramType.Location = new System.Drawing.Point(138, 564);
            this.tbProgramType.Name = "tbProgramType";
            this.tbProgramType.Size = new System.Drawing.Size(518, 20);
            this.tbProgramType.TabIndex = 73;
            //
            // frmMain
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(822, 697);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MSSQLServerAuditor Licenser";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.configTabPage.ResumeLayout(false);
            this.configTabPage.PerformLayout();
            this.keysTabPage.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.keysSignTab.ResumeLayout(false);
            this.keysSignTab.PerformLayout();
            this.keysCryptTab.ResumeLayout(false);
            this.keysCryptTab.PerformLayout();
            this.licensesTabPage.ResumeLayout(false);
            this.licensesTabPage.PerformLayout();
            this.srcTabPage.ResumeLayout(false);
            this.srcTabPage.PerformLayout();
            this.getAssemblyTabPage.ResumeLayout(false);
            this.getAssemblyTabPage.PerformLayout();
            this.makeDistribTabPage.ResumeLayout(false);
            this.makeDistribTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage configTabPage;
        private System.Windows.Forms.RadioButton loadCfgRb;
        private System.Windows.Forms.RadioButton createCfgRb;
        private System.Windows.Forms.TabPage srcTabPage;
        private System.Windows.Forms.Button doConfigurationButton;
        private System.Windows.Forms.Label dstFolderLabel;
        private System.Windows.Forms.Button srcFilesSignButton;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.TabPage getAssemblyTabPage;
        private System.Windows.Forms.Button buildApplicationButton;
        private System.Windows.Forms.Label frameworkFolderLabel;
        private System.Windows.Forms.TextBox txtNetVersion;
        private System.Windows.Forms.Label frameworkVerLabel;
        private System.Windows.Forms.Label sysSettingsFileLabel;
        private System.Windows.Forms.Label langFileLabel;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector txtCreateConfFileName;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector txtLoadConfFileName;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector txtLangFileName;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector txtSysSettingsFileName;
        private MSSQLServerAuditor.Licenser.Gui.FolderSelector txtDstFolder;
        private MSSQLServerAuditor.Licenser.Gui.FolderSelector txtNetFolder;
        private System.Windows.Forms.TabPage makeDistribTabPage;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector txtWixFileName;
        private System.Windows.Forms.Label wixFileLabel;
        private System.Windows.Forms.Button buildInstallPackButton;
        private System.Windows.Forms.Label billboardLabel;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector txtWixBanner;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector txtUserSettingsFileName;
        private System.Windows.Forms.Label userSettingsFileLabel;
        private MSSQLServerAuditor.Licenser.Gui.FolderSelector txtWixFolder;
        private System.Windows.Forms.Label wixFolderLabel;
        private System.Windows.Forms.Label sourceFolderLabel;
        private MSSQLServerAuditor.Licenser.Gui.FolderSelector txtSrcFolder;
        private System.Windows.Forms.TabPage keysTabPage;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage keysSignTab;
        private System.Windows.Forms.TextBox txtSignPriKey;
        private System.Windows.Forms.Label signPrivateKeyLabel;
        private System.Windows.Forms.TextBox txtSignPubKey;
        private System.Windows.Forms.Label signPublicKeyLabel;
        private System.Windows.Forms.TabPage keysCryptTab;
        private System.Windows.Forms.TextBox txtEncPriKey;
        private System.Windows.Forms.Label cryptPrivateKeyLabel;
        private System.Windows.Forms.TextBox txtEncPubKey;
        private System.Windows.Forms.Label cryptPublicKeyLabel;
        private System.Windows.Forms.Button keysSaveButton;
        private System.Windows.Forms.Label installPackPutputFileLabel;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector txtOutputMsi;
        private System.Windows.Forms.Button signNewButton;
        private System.Windows.Forms.Button cryptNewButton;
        private MSSQLServerAuditor.Licenser.Gui.FolderSelector txtDnGuardFolder;
        private System.Windows.Forms.Label dnGuardFolderLabel;
        private System.Windows.Forms.CheckBox chbDnGuardUse;
        private System.Windows.Forms.ComboBox platformComboBox;
        private System.Windows.Forms.Button resetDnGuardOptionsButton;
        private System.Windows.Forms.TextBox dnGuardOptionsTextBox;
        private System.Windows.Forms.Label dnGuardOptionsLabel;
        private System.Windows.Forms.Label dnGuardExeLabel;
        private System.Windows.Forms.TextBox dnGuardExeNameTextBox;
        private System.Windows.Forms.TabPage licensesTabPage;
        private System.Windows.Forms.Label licenseOutputLabel;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector signedLicenseOutFileSelector;
        private System.Windows.Forms.Button subscribeLicenseButton;
        private System.Windows.Forms.Label templatesLabel;
        private System.Windows.Forms.Label licenseToSignLabel;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector licenseToSignFileSelector;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListBox templeateFilesListBox;
        private System.Windows.Forms.Button removeTemplateButton;
        private System.Windows.Forms.Button addTemplateButton;
        private System.Windows.Forms.CheckBox addTemplateFileAsRelativePathCheckBox;
        private System.Windows.Forms.Label lbAdditionalSql;
        private System.Windows.Forms.TextBox tbAdditionalSql;
        private System.Windows.Forms.Label lblAppPath;
        private MSSQLServerAuditor.Licenser.Gui.FileSelector fileAppPath;
        private System.Windows.Forms.Label lblAdditionalTemplate;
        private System.Windows.Forms.Label MSSQLAuditorVersion;
        private System.Windows.Forms.Button btAdditionalTemplates;
        private System.Windows.Forms.TextBox tbAdditionalTemplates;
        private System.Windows.Forms.Label lbFileName;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Label lbDirName;
        private System.Windows.Forms.TextBox tbDirName;
        private System.Windows.Forms.TextBox tbShorcName;
        private System.Windows.Forms.Label lbShorcName;
        private System.Windows.Forms.Label lbProgramName;
        private System.Windows.Forms.TextBox tbProgramName;
        private MSSQLServerAuditor.Licenser.Gui.FolderSelector fsLicenseFolder;
        private System.Windows.Forms.Label еиДшсутыуАшдуТфьу;
        private System.Windows.Forms.Label lbRu;
        private System.Windows.Forms.TextBox tbLicenseFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbProgramType;
    }
}

