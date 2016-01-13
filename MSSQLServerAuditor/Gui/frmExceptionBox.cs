using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MSSQLServerAuditor.Model.Internationalization;

namespace MSSQLServerAuditor.Gui
{
    /// <summary>
    /// Unhandled exception box
    /// </summary>
    public partial class frmExceptionBox : LocalizableForm
    {
        frmExceptionBox()
        {
            InitializeComponent();
            SetMaxSize();
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="ex">Unhandled exception</param>
        public frmExceptionBox(Exception ex):this()
        {
            lblMessage.Text = ex.Message;
            txtWholeText.Text = ex.ToString();
        }

        void SetMaxSize()
        {
            lblDescription.MaximumSize = new Size(ClientSize.Width, 0);
            lblMessage.MaximumSize = new Size(ClientSize.Width, 0);
            MinimumSize = new Size(0, lblMessage.ClientSize.Height + lblMessage.Padding.Size.Height + lblDescription.ClientSize.Height + lblDescription.Padding.Size.Height + panel1.ClientSize.Height + Size.Height - ClientSize.Height);
        }

        private void frmExceptionBox_Resize(object sender, EventArgs e)
        {
            SetMaxSize();
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            txtWholeText.Visible = !txtWholeText.Visible;
            Height = MinimumSize.Height + (txtWholeText.Visible ? 100 : 0);
        }

	/// <summary>
        /// Static handler for Domain.UnhandledException event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event args</param>
        public static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            frmExceptionBox box = new frmExceptionBox(e.ExceptionObject as Exception);
            box.ShowDialog();
        }

        /// <summary>
        /// Static handler for Application.ThreadException
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event args</param>
        internal static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            frmExceptionBox box = new frmExceptionBox(e.Exception);
            box.ShowDialog();
        }
    }
}
