using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace RSAEncryptor
{
    public partial class frmMain : Form
    {
        Crypto cr = new Crypto();
        string dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\\RSAEncryptor";
        string keyfilename;
        public frmMain()
        {
            InitializeComponent();
            Text += " - " + Application.ProductVersion;
            Directory.CreateDirectory(dir);
            keyfilename = dir + "\\keys.conf";


            ShowContent();
        }
        string TrimEx(string S)
        {
            if (S.Trim() == "")
            {
                return "";
            }
            char[] sep = { Convert.ToChar(13), Convert.ToChar(10), Convert.ToChar(9) };
            string s = S;
            int start = -1;
            int end = -1;
            // ищем первый и последний НЕ разделитель, определяем позиции
            for (int i = 0; i < s.Length; i++)
            {
                if (!sep.Contains(s[i]))
                {
                    start = i;
                    break;
                }
            }
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (!sep.Contains(s[i]))
                {
                    end = i;
                    break;
                }
            }
            return S.Substring(start, end - start + 1);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Создание пары ключей", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                txtPublicKey.Text = cr.CreatePublicKey();
                txtPrivateKey.Text = cr.CreatePrivateKey();
                SaveContent();
                MessageBox.Show("Пара ключей успешно создана.");
            }

        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            txtSQLEnc.Text = cr.Encrypt(txtPublicKey.Text, txtSQL.Text);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            txtSQLDecrypted.Text = cr.Decrypt(txtPrivateKey.Text, txtSQLEnc.Text);
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            txtSign.Text = cr.Sign(txtPrivateKey.Text, txtSQL.Text);
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            bool res = cr.Verify(txtPublicKey.Text, txtSQL.Text, txtSign.Text);
            if (res)
            {
                MessageBox.Show("OK.");
            }
            else
            {
                MessageBox.Show("FAIL!!!");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog {Filter = "Файлы XML|*.xml"};

            if (!string.IsNullOrWhiteSpace(txtConfFileName.Text))
            {
                var initialPath = Path.GetDirectoryName(txtConfFileName.Text);
                if (initialPath != null)
                    dialog.InitialDirectory = initialPath;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtConfFileName.Text = dialog.FileName;
                SaveContent();
            }
        }

        private void btnReadKeys_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtConfFileName.Text))
            {
                MessageBox.Show("Файл не существует");
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(txtConfFileName.Text);
            string pub = "";
            foreach (XmlNode n in doc.SelectNodes(".//publickey"))
            {
                pub = n.InnerXml;
                break;
            }
            if (pub == txtPublicKey.Text)
            {
                MessageBox.Show("Открытые ключи совпадают.");
            }
            else
            {
                MessageBox.Show("Ошибка! Открытый ключ в файле не совпадает с текущим ключом. Обновите конф. файл.");

            }

        }

        void CreateEmptyKeyFile()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "UTF-8", String.Empty);
            doc.AppendChild(decl);
            XmlNode keys = doc.AppendChild(doc.CreateElement("keys"));
            keys.AppendChild(doc.CreateElement("privatekey"));
            keys.AppendChild(doc.CreateElement("publickey"));
            keys.AppendChild(doc.CreateElement("conffilename"));
            keys.AppendChild(doc.CreateElement("queryfilename"));
            doc.Save(keyfilename);
        }
        /// <summary>
        /// read from keys.xml
        /// </summary>
        void ShowContent()
        {
            if (!File.Exists(keyfilename)) CreateEmptyKeyFile();
            XmlDocument doc = new XmlDocument();
            doc.Load(keyfilename);
            foreach (XmlNode n in doc.SelectNodes(".//publickey"))
            {
                txtPublicKey.Text = n.InnerXml;
                break;
            }
            foreach (XmlNode n in doc.SelectNodes(".//privatekey"))
            {
                txtPrivateKey.Text = n.InnerXml;
                break;
            }
            foreach (XmlNode n in doc.SelectNodes(".//conffilename"))
            {
                txtConfFileName.Text = n.InnerText;
                break;
            }
            foreach (XmlNode n in doc.SelectNodes(".//queryfilename"))
            {
                txtQueryFileName.Text = n.InnerText;
                break;
            }
        }
        /// <summary>
        /// save to keys.xml
        /// </summary>
        void SaveContent()
        {
            if (!File.Exists(keyfilename)) CreateEmptyKeyFile();
            XmlDocument doc = new XmlDocument();
            doc.Load(keyfilename);
            foreach (XmlNode n in doc.SelectNodes(".//publickey"))
            {
                n.InnerXml = txtPublicKey.Text;
                break;
            }
            foreach (XmlNode n in doc.SelectNodes(".//privatekey"))
            {
                n.InnerXml = txtPrivateKey.Text;
                break;
            }
            foreach (XmlNode n in doc.SelectNodes(".//conffilename"))
            {
                n.InnerText = txtConfFileName.Text;
                break;
            }
            foreach (XmlNode n in doc.SelectNodes(".//queryfilename"))
            {
                n.InnerText = txtQueryFileName.Text;
                break;
            }
            doc.Save(keyfilename);
        }

        private void btnQueryBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "Файлы XML|*.xml";
            f.InitialDirectory = Path.GetDirectoryName(txtQueryFileName.Text);
            if (f.ShowDialog() == DialogResult.OK)
            {
                txtQueryFileName.Text = f.FileName;
                SaveContent();
            }
        }

        private void btnWriteKey_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtConfFileName.Text))
            {
                MessageBox.Show("Файл не существует");
                return;
            }
            if (MessageBox.Show("Вы уверены?", "Обновление конф. файла", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(txtConfFileName.Text);
                foreach (XmlNode n in doc.SelectNodes(".//publickey"))
                {
                    n.InnerXml = txtPublicKey.Text;
                    break;
                }
                doc.Save(txtConfFileName.Text);

                MessageBox.Show("Конф. файл обновлен");
            }
        }

        private void btnVerifyAll_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtQueryFileName.Text))
            {
                MessageBox.Show("Файл запросов не существует");
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(txtQueryFileName.Text);
            string sign = "";
            string msg = "";
            int i = 1;
            string sql="";
            foreach (XmlNode n in doc.SelectNodes(".//sql-select-text | .//database-select-text"))
            {
                msg = "Запрос #" + i + ": ";
                sign = "";
                sql =  TrimEx(n.InnerText);
                foreach (XmlAttribute a in n.Attributes)
                {
                    switch (a.Name)
                    {
                        case "signature": sign = a.Value; break;
                    }
                }
                if (sign == "")
                {
                    msg += "нет подписи.";
                }
                else
                {
                    if (cr.Verify(txtPublicKey.Text, sql, sign))
                    {
                        msg += "ОК.";

                    }
                    else
                    {
                        msg += "FAIL!!!";
                    }
                }
                txtSignLog.AppendText(msg + Convert.ToChar(13) + Convert.ToChar(10));
                i++;
            }


        }

        private void btnClearSignLog_Click(object sender, EventArgs e)
        {
            txtSignLog.Text = "";
        }

        private void btnSignAll_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtQueryFileName.Text))
            {
                MessageBox.Show("Файл запросов не существует");
                return;
            }
            if (MessageBox.Show("Вы уверены?", "Подписывание файла запросов", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(txtQueryFileName.Text);
                string sign = "";
                string msg = "";
                int i = 1;
                string sql = "";
                XmlAttribute signature = null;
                foreach (XmlNode n in doc.SelectNodes(".//sql-select-text | .//database-select-text"))
                {
                    msg = "Запрос #" + i + " подписан.";
                    sql = TrimEx(n.InnerText);
                    sign = cr.Sign(txtPrivateKey.Text, sql);
                    signature = null;
                    foreach (XmlAttribute a in n.Attributes)
                    {
                        switch (a.Name)
                        {
                            case "signature": signature = a; break;
                        }
                    }
                    if (signature == null)
                    {
                        signature = n.Attributes.Append(doc.CreateAttribute("signature"));
                    }
                    signature.Value = sign;
                    txtSignLog.AppendText(msg + Convert.ToChar(13) + Convert.ToChar(10));
                    i++;
                }
                doc.Save(txtQueryFileName.Text);
            }
        }

        private void txtConfFileName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
