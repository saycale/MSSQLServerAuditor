using System;
using System.IO;
using System.Windows.Forms;
using log4net;
using MSSQLServerAuditor.Licenser.Utils;

namespace MSSQLServerAuditor.Licenser.Gui
{
    public delegate bool IsCorrectFileDelegate(string fileName);

    public partial class FileSelector : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool _save;
        private string _filter;
        private IsCorrectFileDelegate _isCorrectFile;

        public FileSelector()
        {
            InitializeComponent();
        }

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public bool WarnIfFileNotExists
        {
            get { return !Save; }
        }

        public IsCorrectFileDelegate IsCorrectFile
        {
            get { return _isCorrectFile; }
            set { _isCorrectFile = value; }
        }

        public bool Save
        {
            get { return _save; }
            set
            {
                _save = value;
                txtFileName_TextChanged(txtFileName, new EventArgs());
            }
        }

        public void ProcessValidate()
        {
            if (_isCorrectFile != null)
            {
                _isCorrectFile(this.Text);
            }
        }

        private void selectConnFileName_Click(object sender, EventArgs e)
        {
            FileDialog f;

            if (_save)
            {
                f = new SaveFileDialog();
            }
            else
            {
                f = new OpenFileDialog();
            }

            f.Filter = Filter;

            try
            {
                f.InitialDirectory = Path.GetDirectoryName(txtFileName.Text);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            if (f.ShowDialog() == DialogResult.OK)
            {
                string fileName = f.FileName.GetRelativePath();
                bool correct = true;
                if (_isCorrectFile != null)
                {
                    if (!_isCorrectFile(fileName))
                    {
                        MessageBox.Show("Выбран некорректный файл");
                        correct = false;
                    }
                }

                if (correct)
                {
                    txtFileName.Text = fileName;
                }
            }
        }

        public override string Text
        {
            get
            {
                return txtFileName.Text;
            }
            set
            {
                txtFileName.Text = value;
            }
        }

        private string GetAbsPath()
        {
            try
            {
                return Path.GetFullPath(txtFileName.Text);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return txtFileName.Text;
            }
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            if (txtFileName.Text == String.Empty)
            {
                return;
            }

            var designing = Site != null && Site.DesignMode;
            errorProvider.SetError(txtFileName,
                !string.IsNullOrEmpty(txtFileName.Text) &&
                !designing && WarnIfFileNotExists &&
                !File.Exists(GetAbsPath()) ? "File does not exist" : null);
        }
    }
}
