using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Internationalization;

namespace MSSQLServerAuditor.Gui
{
    /// <summary>
    /// Error log form
    /// </summary>
    internal partial class frmErrorLog : Form
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        private frmErrorLog()
        {
            InitializeComponent();
        }

        private void lbErrorItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ErrorLogItem item = (ErrorLogItem)lbErrorItem.SelectedItem;
            if (item.QueryItem != null)
            txtSQL.Text = item.QueryItem.Text;
            else
            {
                txtSQL.Text = "-";
            }
            txtError.Text = item.Error.Message;
            lbInstance.Text = (item.Instance == null)?"":item.Instance.Name;
        }

        /// <summary>
        /// Set error log to be visualized
        /// </summary>
        /// <param name="errorLog">Error log</param>
        private void SetErrorLog(ErrorLog errorLog)
        {
            lbErrorItem.Items.AddRange(errorLog.ErrorItems.ToArray());

            if (lbErrorItem.Items.Count > 0)
            {
                lbErrorItem.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Static method decides show the modal form (error items > 0) or not (otherwise)
        /// </summary>
        /// <param name="errorLog">Error log</param>
        public static void VisualizeLog(ErrorLog errorLog)
        {
            if (errorLog.ErrorItems.Count > 0)
            {
                var frmErrorLog = new frmErrorLog();
                frmErrorLog.SetErrorLog(errorLog);
                frmErrorLog.ShowDialog();
            }
        }
    }
}
