namespace MSSQLServerAuditor.Licenser.Gui
{
    partial class AdditionalTemplateSelector
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btSave = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbLocales = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLocale = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.file = new MSSQLServerAuditor.Licenser.Gui.FileSelector();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 282);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(770, 36);
            this.panel1.TabIndex = 0;
            //
            // btSave
            //
            this.btSave.Location = new System.Drawing.Point(684, 8);
            this.btSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(77, 26);
            this.btSave.TabIndex = 4;
            this.btSave.Text = "OK";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            //
            // panel2
            //
            this.panel2.Controls.Add(this.lbLocales);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(770, 282);
            this.panel2.TabIndex = 1;
            //
            // lbLocales
            //
            this.lbLocales.ContextMenuStrip = this.contextMenuStrip1;
            this.lbLocales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLocales.FormattingEnabled = true;
            this.lbLocales.Location = new System.Drawing.Point(0, 0);
            this.lbLocales.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lbLocales.Name = "lbLocales";
            this.lbLocales.Size = new System.Drawing.Size(770, 245);
            this.lbLocales.TabIndex = 3;
            this.lbLocales.SelectedIndexChanged += new System.EventHandler(this.LbLocalesSelectedIndexChanged);
            //
            // contextMenuStrip1
            //
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRemove});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 26);
            //
            // menuRemove
            //
            this.menuRemove.Name = "menuRemove";
            this.menuRemove.Size = new System.Drawing.Size(118, 22);
            this.menuRemove.Text = "Удалить";
            this.menuRemove.Click += new System.EventHandler(this.MenuRemoveClick);
            //
            // panel4
            //
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.cbLocale);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.file);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 245);
            this.panel4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(770, 37);
            this.panel4.TabIndex = 4;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Шаблон";
            //
            // cbLocale
            //
            this.cbLocale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocale.FormattingEnabled = true;
            this.cbLocale.Items.AddRange(new object[] {
            "ru",
            "en",
            "de",
            "fr"});
            this.cbLocale.Location = new System.Drawing.Point(61, 5);
            this.cbLocale.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbLocale.Name = "cbLocale";
            this.cbLocale.Size = new System.Drawing.Size(98, 21);
            this.cbLocale.TabIndex = 2;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Локаль";
            //
            // file
            //
            this.file.Filter = null;
            this.file.IsCorrectFile = null;
            this.file.Location = new System.Drawing.Point(226, 2);
            this.file.Margin = new System.Windows.Forms.Padding(2);
            this.file.Name = "file";
            this.file.Save = false;
            this.file.Size = new System.Drawing.Size(534, 22);
            this.file.TabIndex = 0;
            //
            // AdditionalTemplateSelector
            //
            this.AcceptButton = this.btSave;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(770, 318);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "AdditionalTemplateSelector";
            this.Text = "Дополнительные HTML файлы для отчетов";
            this.Load += new System.EventHandler(this.AdditionalTemplateSelectorLoad);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.ListBox lbLocales;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbLocale;
        private System.Windows.Forms.Label label1;
        private FileSelector file;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuRemove;
    }
}