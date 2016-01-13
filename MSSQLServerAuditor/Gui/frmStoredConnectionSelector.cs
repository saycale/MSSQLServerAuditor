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
    /// Form for stored connections
    /// </summary>
    public partial class frmStoredConnectionSelector : LocalizableForm
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public frmStoredConnectionSelector()
        {
            InitializeComponent();
        }
    }
}
