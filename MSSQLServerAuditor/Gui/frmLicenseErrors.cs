using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Internationalization;

namespace MSSQLServerAuditor.Gui
{
    /// <summary>
    /// Dialog of problem with the license.
    /// </summary>
    public partial class frmLicenseErrors : LocalizableForm
    {
        /// <summary>
        /// Initializing object frmLicenseErrors.
        /// </summary>
        public frmLicenseErrors()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set problems license.
        /// </summary>
        /// <param name="licenseProblems">List problems license.</param>
        public void SetLicenseProblems(List<LicenseState> licenseProblems)
        {
            licenseProblemBindingSource.DataSource = licenseProblems;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
