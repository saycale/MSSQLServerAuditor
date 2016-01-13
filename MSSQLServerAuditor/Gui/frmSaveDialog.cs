using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MSSQLServerAuditor.Model.Internationalization;

namespace MSSQLServerAuditor.Gui
{
    /// <summary>
    /// Form for save options
    /// </summary>
    public partial class frmSaveDialog : LocalizableForm
    {
        internal enum ScopeToSave
        {
            Current,
            All
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public frmSaveDialog()
        {
            InitializeComponent();
            CheckCanSave();
        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new FolderBrowserDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                txFolderName.Text = saveFileDialog.SelectedPath;
            CheckCanSave();
        }

        public void SetCurScopeName(string name)
        {
            lbCurScopeName.Text = name;
        }

        public void DisableCurrentScope()
        {
            lbCurScopeName.Text = "";
            rbCurrent.Enabled = false;
            rbAll.Checked = true;
        }

        private void chbCurrent_CheckedChanged(object sender, EventArgs e)
        {
            CheckCanSave();
        }

        void CheckCanSave()
        {
            btOk.Enabled = (chbCurrent.Checked || chbHistoric.Checked) && !String.IsNullOrEmpty(txFolderName.Text);
        }

        private void chbHistoric_CheckedChanged(object sender, EventArgs e)
        {
            CheckCanSave();
        }

        internal ScopeToSave Scope
        {
            get
            {
                if (rbAll.Checked)
                    return ScopeToSave.All;
                else
                    return ScopeToSave.Current;
            }
        }

        internal bool SaveCurrent
        {
            get { return chbCurrent.Checked; }
        }
        internal bool SaveHistoric
        {
            get { return chbHistoric.Checked; }
        }

        internal string Folder
        {
            get { return txFolderName.Text; }
        }
    }
}
