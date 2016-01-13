using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Licenser.Gui
{
    using System.IO;

    using MSSQLServerAuditor.Licenser.Model;
    using MSSQLServerAuditor.Licenser.Model.SignPreparators;

    public partial class AdditionalTemplateSelector : Form
    {
        public LicSettingsInfo Settings { get; set; }

        public AdditionalTemplateSelector()
        {
            InitializeComponent();

            this.file.IsCorrectFile += name =>
                {
                    if (File.Exists(name) && !string.IsNullOrEmpty(this.cbLocale.Text))
                    {
                        var locale = this.cbLocale.Text;
                        var template = this.FindTemplate(locale);

                        if (template != null)
                        {
                            template.FileName = name;
                        }
                        else
                        {
                            this.Settings.AdditionalTemplates.Add(new AdditionalTemplate
                                                                      {
                                                                          Locale = locale,
                                                                          FileName = name
                                                                      });
                        }

                        this.BindList();

                        return true;
                    }

                    return false;
                };
        }

        private AdditionalTemplate FindTemplate(string locale)
        {
            if (this.Settings == null)
            {
                return null;
            }

            return this.Settings.AdditionalTemplates.FirstOrDefault(t => t.Locale == locale);
        }

        private void LbLocalesSelectedIndexChanged(object sender, EventArgs e)
        {
            var current = this.GetCurrentSelectedLocale();
            if (current != null)
            {
                var template = this.FindTemplate(current);

                if (template != null)
                {
                    this.cbLocale.Text = template.Locale;
                    this.file.Text = template.FileName;
                }
                else
                {
                    this.cbLocale.Text = current;
                    this.file.Text = string.Empty;
                }
            }
        }

        private string GetCurrentSelectedLocale()
        {
            var locale = this.lbLocales.SelectedItem as string;
            if (string.IsNullOrEmpty(locale))
            {
                return null;
            }

            return locale.Split(':')[0];
        }

        private void AdditionalTemplateSelectorLoad(object sender, EventArgs e)
        {
            this.BindList();
        }

        private void BindList()
        {
            string format = "{0}: {1}";
            this.lbLocales.Items.Clear();
            foreach (var template in this.Settings.AdditionalTemplates)
            {
                this.lbLocales.Items.Add(string.Format(format, template.Locale, template.FileName));
            }
        }

        private void MenuRemoveClick(object sender, EventArgs e)
        {
            var template = this.FindTemplate(this.GetCurrentSelectedLocale());

            if (template != null)
            {
                this.Settings.AdditionalTemplates.Remove(template);
                this.BindList();
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
