namespace MSSQLServerAuditor.Licenser.Model
{
    partial class SettingsForm
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
            this.uiLangCombo = new System.Windows.Forms.ComboBox();
            this.uiLanglabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.settingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.settingsBindingSource)).BeginInit();
            this.SuspendLayout();
            //
            // uiLangCombo
            //
            this.uiLangCombo.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.settingsBindingSource, "UiLanguage", true));
            this.uiLangCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiLangCombo.FormattingEnabled = true;
            this.uiLangCombo.Location = new System.Drawing.Point(135, 26);
            this.uiLangCombo.Name = "uiLangCombo";
            this.uiLangCombo.Size = new System.Drawing.Size(67, 21);
            this.uiLangCombo.TabIndex = 0;
            //
            // uiLanglabel
            //
            this.uiLanglabel.AutoSize = true;
            this.uiLanglabel.Location = new System.Drawing.Point(29, 29);
            this.uiLanglabel.Name = "uiLanglabel";
            this.uiLanglabel.Size = new System.Drawing.Size(68, 13);
            this.uiLanglabel.TabIndex = 1;
            this.uiLanglabel.Text = "UI language:";
            //
            // okButton
            //
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(355, 200);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            //
            // cancelButton
            //
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(436, 200);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            //
            // settingsBindingSource
            //
            this.settingsBindingSource.DataSource = typeof(MSSQLServerAuditor.Licenser.Model.Settings);
            //
            // SettingsForm
            //
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(523, 235);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.uiLanglabel);
            this.Controls.Add(this.uiLangCombo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.settingsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox uiLangCombo;
        private System.Windows.Forms.Label uiLanglabel;
        private System.Windows.Forms.BindingSource settingsBindingSource;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}