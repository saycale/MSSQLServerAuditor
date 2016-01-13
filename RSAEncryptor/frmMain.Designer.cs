namespace RSAEncryptor
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
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tsKeys = new System.Windows.Forms.TabPage();
            this.btnWriteKey = new System.Windows.Forms.Button();
            this.btnReadKeys = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtConfFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPrivateKey = new System.Windows.Forms.TextBox();
            this.txtPublicKey = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.tsSQL = new System.Windows.Forms.TabPage();
            this.btnVerify = new System.Windows.Forms.Button();
            this.txtSign = new System.Windows.Forms.TextBox();
            this.btnSign = new System.Windows.Forms.Button();
            this.txtSQLDecrypted = new System.Windows.Forms.TextBox();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.txtSQLEnc = new System.Windows.Forms.TextBox();
            this.txtSQL = new System.Windows.Forms.TextBox();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.tsSQLFile = new System.Windows.Forms.TabPage();
            this.btnClearSignLog = new System.Windows.Forms.Button();
            this.btnSignAll = new System.Windows.Forms.Button();
            this.btnVerifyAll = new System.Windows.Forms.Button();
            this.txtSignLog = new System.Windows.Forms.TextBox();
            this.btnQueryBrowse = new System.Windows.Forms.Button();
            this.txtQueryFileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tsKeys.SuspendLayout();
            this.tsSQL.SuspendLayout();
            this.tsSQLFile.SuspendLayout();
            this.SuspendLayout();
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "SQL-запрос";
            //
            // tabControl1
            //
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tsKeys);
            this.tabControl1.Controls.Add(this.tsSQL);
            this.tabControl1.Controls.Add(this.tsSQLFile);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(612, 312);
            this.tabControl1.TabIndex = 14;
            //
            // tsKeys
            //
            this.tsKeys.Controls.Add(this.btnWriteKey);
            this.tsKeys.Controls.Add(this.btnReadKeys);
            this.tsKeys.Controls.Add(this.button1);
            this.tsKeys.Controls.Add(this.txtConfFileName);
            this.tsKeys.Controls.Add(this.label4);
            this.tsKeys.Controls.Add(this.label2);
            this.tsKeys.Controls.Add(this.label1);
            this.tsKeys.Controls.Add(this.txtPrivateKey);
            this.tsKeys.Controls.Add(this.txtPublicKey);
            this.tsKeys.Controls.Add(this.btnGenerate);
            this.tsKeys.Location = new System.Drawing.Point(4, 22);
            this.tsKeys.Name = "tsKeys";
            this.tsKeys.Padding = new System.Windows.Forms.Padding(3);
            this.tsKeys.Size = new System.Drawing.Size(604, 286);
            this.tsKeys.TabIndex = 0;
            this.tsKeys.Text = "Пара ключей";
            this.tsKeys.UseVisualStyleBackColor = true;
            //
            // btnWriteKey
            //
            this.btnWriteKey.Location = new System.Drawing.Point(338, 34);
            this.btnWriteKey.Name = "btnWriteKey";
            this.btnWriteKey.Size = new System.Drawing.Size(139, 23);
            this.btnWriteKey.TabIndex = 19;
            this.btnWriteKey.Tag = "0";
            this.btnWriteKey.Text = "Обновить конф. файл";
            this.btnWriteKey.UseVisualStyleBackColor = true;
            this.btnWriteKey.Click += new System.EventHandler(this.btnWriteKey_Click);
            //
            // btnReadKeys
            //
            this.btnReadKeys.Location = new System.Drawing.Point(12, 34);
            this.btnReadKeys.Name = "btnReadKeys";
            this.btnReadKeys.Size = new System.Drawing.Size(139, 23);
            this.btnReadKeys.TabIndex = 18;
            this.btnReadKeys.Tag = "0";
            this.btnReadKeys.Text = "Проверить конф. файл";
            this.btnReadKeys.UseVisualStyleBackColor = true;
            this.btnReadKeys.Click += new System.EventHandler(this.btnReadKeys_Click);
            //
            // button1
            //
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(564, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 21);
            this.button1.TabIndex = 17;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            //
            // txtConfFileName
            //
            this.txtConfFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConfFileName.Location = new System.Drawing.Point(83, 7);
            this.txtConfFileName.Name = "txtConfFileName";
            this.txtConfFileName.Size = new System.Drawing.Size(475, 20);
            this.txtConfFileName.TabIndex = 16;
            this.txtConfFileName.TextChanged += new System.EventHandler(this.txtConfFileName_TextChanged);
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Конф. файл";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Закрытый";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Открытый";
            //
            // txtPrivateKey
            //
            this.txtPrivateKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrivateKey.Location = new System.Drawing.Point(9, 181);
            this.txtPrivateKey.Multiline = true;
            this.txtPrivateKey.Name = "txtPrivateKey";
            this.txtPrivateKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPrivateKey.Size = new System.Drawing.Size(589, 101);
            this.txtPrivateKey.TabIndex = 12;
            //
            // txtPublicKey
            //
            this.txtPublicKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPublicKey.Location = new System.Drawing.Point(9, 80);
            this.txtPublicKey.Multiline = true;
            this.txtPublicKey.Name = "txtPublicKey";
            this.txtPublicKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPublicKey.Size = new System.Drawing.Size(589, 69);
            this.txtPublicKey.TabIndex = 11;
            //
            // btnGenerate
            //
            this.btnGenerate.Location = new System.Drawing.Point(157, 34);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(175, 23);
            this.btnGenerate.TabIndex = 10;
            this.btnGenerate.Text = "Создать новую пару ключей";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.button1_Click);
            //
            // tsSQL
            //
            this.tsSQL.Controls.Add(this.btnVerify);
            this.tsSQL.Controls.Add(this.txtSign);
            this.tsSQL.Controls.Add(this.btnSign);
            this.tsSQL.Controls.Add(this.txtSQLDecrypted);
            this.tsSQL.Controls.Add(this.btnDecrypt);
            this.tsSQL.Controls.Add(this.txtSQLEnc);
            this.tsSQL.Controls.Add(this.txtSQL);
            this.tsSQL.Controls.Add(this.btnEncrypt);
            this.tsSQL.Location = new System.Drawing.Point(4, 22);
            this.tsSQL.Name = "tsSQL";
            this.tsSQL.Padding = new System.Windows.Forms.Padding(3);
            this.tsSQL.Size = new System.Drawing.Size(604, 286);
            this.tsSQL.TabIndex = 1;
            this.tsSQL.Text = "SQL-запрос";
            this.tsSQL.UseVisualStyleBackColor = true;
            //
            // btnVerify
            //
            this.btnVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVerify.Location = new System.Drawing.Point(3, 251);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(135, 23);
            this.btnVerify.TabIndex = 21;
            this.btnVerify.Text = "Проверить подпись";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            //
            // txtSign
            //
            this.txtSign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSign.Location = new System.Drawing.Point(144, 226);
            this.txtSign.Multiline = true;
            this.txtSign.Name = "txtSign";
            this.txtSign.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSign.Size = new System.Drawing.Size(454, 49);
            this.txtSign.TabIndex = 20;
            //
            // btnSign
            //
            this.btnSign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSign.Location = new System.Drawing.Point(3, 224);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(135, 23);
            this.btnSign.TabIndex = 19;
            this.btnSign.Text = "Подписать";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            //
            // txtSQLDecrypted
            //
            this.txtSQLDecrypted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQLDecrypted.Location = new System.Drawing.Point(307, 149);
            this.txtSQLDecrypted.Multiline = true;
            this.txtSQLDecrypted.Name = "txtSQLDecrypted";
            this.txtSQLDecrypted.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSQLDecrypted.Size = new System.Drawing.Size(291, 69);
            this.txtSQLDecrypted.TabIndex = 18;
            //
            // btnDecrypt
            //
            this.btnDecrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecrypt.Location = new System.Drawing.Point(307, 120);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(110, 23);
            this.btnDecrypt.TabIndex = 17;
            this.btnDecrypt.Text = "Дешифровать";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            //
            // txtSQLEnc
            //
            this.txtSQLEnc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQLEnc.Location = new System.Drawing.Point(3, 149);
            this.txtSQLEnc.Multiline = true;
            this.txtSQLEnc.Name = "txtSQLEnc";
            this.txtSQLEnc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSQLEnc.Size = new System.Drawing.Size(289, 69);
            this.txtSQLEnc.TabIndex = 16;
            //
            // txtSQL
            //
            this.txtSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQL.Location = new System.Drawing.Point(6, 6);
            this.txtSQL.Multiline = true;
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSQL.Size = new System.Drawing.Size(592, 108);
            this.txtSQL.TabIndex = 15;
            //
            // btnEncrypt
            //
            this.btnEncrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEncrypt.Location = new System.Drawing.Point(3, 120);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(135, 23);
            this.btnEncrypt.TabIndex = 14;
            this.btnEncrypt.Text = "Зашифровать";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            //
            // tsSQLFile
            //
            this.tsSQLFile.Controls.Add(this.btnClearSignLog);
            this.tsSQLFile.Controls.Add(this.btnSignAll);
            this.tsSQLFile.Controls.Add(this.btnVerifyAll);
            this.tsSQLFile.Controls.Add(this.txtSignLog);
            this.tsSQLFile.Controls.Add(this.btnQueryBrowse);
            this.tsSQLFile.Controls.Add(this.txtQueryFileName);
            this.tsSQLFile.Controls.Add(this.label5);
            this.tsSQLFile.Location = new System.Drawing.Point(4, 22);
            this.tsSQLFile.Name = "tsSQLFile";
            this.tsSQLFile.Padding = new System.Windows.Forms.Padding(3);
            this.tsSQLFile.Size = new System.Drawing.Size(604, 286);
            this.tsSQLFile.TabIndex = 2;
            this.tsSQLFile.Text = "Файл запросов";
            this.tsSQLFile.UseVisualStyleBackColor = true;
            //
            // btnClearSignLog
            //
            this.btnClearSignLog.Location = new System.Drawing.Point(162, 34);
            this.btnClearSignLog.Name = "btnClearSignLog";
            this.btnClearSignLog.Size = new System.Drawing.Size(126, 23);
            this.btnClearSignLog.TabIndex = 24;
            this.btnClearSignLog.Text = "Очистить лог";
            this.btnClearSignLog.UseVisualStyleBackColor = true;
            this.btnClearSignLog.Click += new System.EventHandler(this.btnClearSignLog_Click);
            //
            // btnSignAll
            //
            this.btnSignAll.Location = new System.Drawing.Point(294, 33);
            this.btnSignAll.Name = "btnSignAll";
            this.btnSignAll.Size = new System.Drawing.Size(261, 23);
            this.btnSignAll.TabIndex = 23;
            this.btnSignAll.Text = "Подписать и обновить файл запросов";
            this.btnSignAll.UseVisualStyleBackColor = true;
            this.btnSignAll.Click += new System.EventHandler(this.btnSignAll_Click);
            //
            // btnVerifyAll
            //
            this.btnVerifyAll.Location = new System.Drawing.Point(12, 33);
            this.btnVerifyAll.Name = "btnVerifyAll";
            this.btnVerifyAll.Size = new System.Drawing.Size(143, 23);
            this.btnVerifyAll.TabIndex = 22;
            this.btnVerifyAll.Text = "Проверить подписи";
            this.btnVerifyAll.UseVisualStyleBackColor = true;
            this.btnVerifyAll.Click += new System.EventHandler(this.btnVerifyAll_Click);
            //
            // txtSignLog
            //
            this.txtSignLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSignLog.Location = new System.Drawing.Point(12, 67);
            this.txtSignLog.Multiline = true;
            this.txtSignLog.Name = "txtSignLog";
            this.txtSignLog.Size = new System.Drawing.Size(581, 213);
            this.txtSignLog.TabIndex = 21;
            //
            // btnQueryBrowse
            //
            this.btnQueryBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQueryBrowse.Location = new System.Drawing.Point(564, 6);
            this.btnQueryBrowse.Name = "btnQueryBrowse";
            this.btnQueryBrowse.Size = new System.Drawing.Size(29, 21);
            this.btnQueryBrowse.TabIndex = 20;
            this.btnQueryBrowse.Text = "...";
            this.btnQueryBrowse.UseVisualStyleBackColor = true;
            this.btnQueryBrowse.Click += new System.EventHandler(this.btnQueryBrowse_Click);
            //
            // txtQueryFileName
            //
            this.txtQueryFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQueryFileName.Location = new System.Drawing.Point(97, 6);
            this.txtQueryFileName.Name = "txtQueryFileName";
            this.txtQueryFileName.Size = new System.Drawing.Size(461, 20);
            this.txtQueryFileName.TabIndex = 19;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Файл запросов";
            //
            // frmMain
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 326);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label3);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RSAEncryptor";
            this.tabControl1.ResumeLayout(false);
            this.tsKeys.ResumeLayout(false);
            this.tsKeys.PerformLayout();
            this.tsSQL.ResumeLayout(false);
            this.tsSQL.PerformLayout();
            this.tsSQLFile.ResumeLayout(false);
            this.tsSQLFile.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tsKeys;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPrivateKey;
        private System.Windows.Forms.TextBox txtPublicKey;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TabPage tsSQL;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.TextBox txtSign;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.TextBox txtSQLDecrypted;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.TextBox txtSQLEnc;
        private System.Windows.Forms.TextBox txtSQL;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtConfFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tsSQLFile;
        private System.Windows.Forms.Button btnReadKeys;
        private System.Windows.Forms.Button btnQueryBrowse;
        private System.Windows.Forms.TextBox txtQueryFileName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnWriteKey;
        private System.Windows.Forms.Button btnSignAll;
        private System.Windows.Forms.Button btnVerifyAll;
        private System.Windows.Forms.TextBox txtSignLog;
        private System.Windows.Forms.Button btnClearSignLog;
    }
}

