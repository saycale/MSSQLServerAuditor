using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSSQLServerAuditor.Licenser.Utils;

namespace MSSQLServerAuditor.Licenser.Gui
{
    public partial class FolderSelector : UserControl
    {
        private bool _isRelative;

        public FolderSelector()
        {
            InitializeComponent();
        }

        public bool IsRelative
        {
            get { return _isRelative; }
            set { _isRelative = value; }
        }

        private string GetAbsPath()
        {
            try
            {
                return Path.GetFullPath(txtDstFolder.Text);
            }
            catch
            {
                return txtDstFolder.Text;
            }
        }

        private void selectDstFolder_Click(object sender, EventArgs e)
        {
            var browserDialog = new FolderBrowserDialog();

            var initialPath = GetAbsPath();

            if (Directory.Exists(initialPath))
                browserDialog.SelectedPath = initialPath;

            if (browserDialog.ShowDialog() != DialogResult.OK)
                return;

            txtDstFolder.Text = IsRelative ? txtDstFolder.Text.GetRelativePath() : browserDialog.SelectedPath;
        }

        public override string Text
        {
            get
            {
                return txtDstFolder.Text;
            }
            set
            {
                txtDstFolder.Text = value;
            }
        }

        private void FolderSelector_Load(object sender, EventArgs e)
        {

        }

        private void txtDstFolder_TextChanged(object sender, EventArgs e)
        {
            errorProvider.SetError(txtDstFolder,
                !string.IsNullOrEmpty(txtDstFolder.Text) &&
                !Directory.Exists(GetAbsPath()) ? "Directory does not exists" : null);
        }
    }
}
