using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Licenser.Model
{
    using MSSQLServerAuditor.Licenser.Model.SignPreparators;

    /// <summary>
    /// Licenser settings
    /// </summary>
    [XmlRoot(ElementName = "LicenseSettings")]
    public class LicSettingsInfo
    {
        private string _publicKeySign;
        private string _privateKeySign;

        private string _publicKeyDecrypt;
        private string _privateKeyDecrypt;
        private string _srcFolder;

        private string _dstFolder;
        private string _netPlatform = "v4";
        private string _netFolder = @"c:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\";
        private string _languageFile;
        private string _systemSettingsFile;
        private string _userSettingsFile;
        private string _wixFileName;
        private string _wixBannerFileName;
        private string _wixFolder = @"c:\Program Files (x86)\WiX Toolset v3.7\bin\";
        private string _outputMsi;
        private string _dnGuardFolder = @"";
        private bool _useDnGuard = true;

        public LicSettingsInfo()
        {
            TemplateFiles = new List<string>();
            AdditionalTemplates = new List<AdditionalTemplate>();
        }

        /// <summary>
        /// Public key for sign verification (send to client)
        /// </summary>
        [XmlElement]
        public string PublicKeySign
        {
            get { return _publicKeySign; }
            set { _publicKeySign = value; }
        }

        /// <summary>
        /// Private key to sign (known for licenser only)
        /// </summary>
        [XmlElement]
        public string PrivateKeySign
        {
            get { return _privateKeySign; }
            set { _privateKeySign = value; }
        }

        /// <summary>
        /// Public key for Xml-file encrypt (known for licenser)
        /// </summary>
        [XmlElement]
        public string PublicKeyDecrypt
        {
            get { return _publicKeyDecrypt; }
            set { _publicKeyDecrypt = value; }
        }

        /// <summary>
        /// Private key for Xml-file decrypt (send to client)
        /// </summary>
        [XmlElement]
        public string PrivateKeyDecrypt
        {
            get { return _privateKeyDecrypt; }
            set { _privateKeyDecrypt = value; }
        }

        /// <summary>
        /// Template files to put into the distribution pack
        /// </summary>
        public List<string> TemplateFiles { get; set; }

        /// <summary>
        /// Destination folder for configuration
        /// </summary>
        [XmlElement]
        public string DstFolder
        {
            get { return _dstFolder; }
            set { _dstFolder = value; }
        }

        [XmlIgnore]
        public string DstFolderFull
        {
            get { return Path.GetFullPath(_dstFolder); }
        }

        /// <summary>
        /// Source folder of configuration
        /// </summary>
        [XmlElement]
        public string SrcFolder
        {
            get { return _srcFolder; }
            set { _srcFolder = value; }
        }

        /// <summary>
        /// .NET Folder
        /// </summary>
        [XmlElement]
        public string NetFolder
        {
            get { return _netFolder; }
            set { _netFolder = value; }
        }

        /// <summary>
        /// .NET Platform version ("v2", "v4")
        /// </summary>
        [XmlElement]
        public string NetPlatform
        {
            get { return _netPlatform; }
            set { _netPlatform = value; }
        }

        /// <summary>
        /// Language file for client
        /// </summary>
        [XmlElement]
        public string LanguageFile
        {
            get { return _languageFile; }
            set { _languageFile = value; }
        }

        /// <summary>
        /// System settings file for client
        /// </summary>
        [XmlElement]
        public string SystemSettingsFile
        {
            get { return _systemSettingsFile; }
            set { _systemSettingsFile = value; }
        }

        /// <summary>
        /// Default user settings file for client
        /// </summary>
        [XmlElement]
        public string UserSettingsFile
        {
            get { return _userSettingsFile; }
            set { _userSettingsFile = value; }
        }

        /// <summary>
        /// File name of Wix template
        /// </summary>
        [XmlElement]
        public string WixFileName
        {
            get { return _wixFileName; }
            set { _wixFileName = value; }
        }

        /// <summary>
        /// File name of Wix setup banner image
        /// </summary>
        [XmlElement]
        public string WixBannerFileName
        {
            get { return _wixBannerFileName; }
            set { _wixBannerFileName = value; }
        }

        public CryptoProcessor GetCryptoProcessor()
        {
            return new CryptoProcessor(_publicKeySign, PrivateKeyDecrypt);
        }

        /// <summary>
        /// Folder name of Wix bin
        /// </summary>
        [XmlElement]
        public string WixFolder
        {
            get { return _wixFolder; }
            set { _wixFolder = value; }
        }

        /// <summary>
        /// Destination MSI file name
        /// </summary>
        [XmlElement]
        public string OutputMsi
        {
            get { return _outputMsi; }
            set { _outputMsi = value; }
        }

        [XmlElement]
        public string DnGuardFolder
        {
            get { return _dnGuardFolder; }
            set { _dnGuardFolder = value; }
        }

        [XmlElement]
        public string DnGuardExeName { get; set; }

        [XmlElement]
        public bool UseDnGuard
        {
            get { return _useDnGuard; }
            set { _useDnGuard = value; }
        }

        [XmlElement]
        public bool DnGuardX64Opt { get; set; }

        [XmlElement]
        public string DnGuardOptions { get; set; }

        [XmlElement]
        public string AdditionalSql { get; set; }

        [XmlElement]
        public string AppPath { get; set; }

        [XmlElement]
        public string DirectoryName { get; set; }

        [XmlElement]
        public string ExeFileName { get; set; }

        [XmlElement]
        public string ShorcName { get; set; }

        [XmlElement]
        public string LicenseFolder { get; set; }

        [XmlElement]
        public string LicenseFileName { get; set; }

        [XmlElement]
        public string ProgramName { get; set; }

        [XmlElement]
        public string ProgramId { get; set; }


        [XmlElement(ElementName = "AdditionalTemplate")]
        public List<AdditionalTemplate> AdditionalTemplates { get; set; }
    }
}
