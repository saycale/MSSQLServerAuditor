using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Linq;
using MSSQLServerAuditor.Model.Loaders;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Settings
{
	/// <summary>
	/// System settings info
	/// </summary>
	[XmlRoot(ElementName = "root")]
	[Serializable]
	public class SystemSettingsInfo //: IXmlSerializable
	{
		private string _publicKeyXmlSign;
		private string _privateKeyXmlDecrypt;
		private string _supportEmail;
		private string _supportUrl;
		private string _supportSubject;
		private string _supportTemplateMail;

		/// <summary>
		/// Initializing object SystemSettingsInfo.
		/// </summary>
		public SystemSettingsInfo()
		{
			this._publicKeyXmlSign     = null;
			this._privateKeyXmlDecrypt = null;
			this._supportEmail         = string.Empty;
			this._supportUrl           = string.Empty;
			this._supportSubject       = string.Empty;
			this._supportTemplateMail  = string.Empty;
			this.AvailableUiLanguages  = new List<string>();
		}

		/// <summary>
		/// Load from system settings information.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <returns>Return system settings information.</returns>
		public static SystemSettingsInfo LoadFrom(string fileName)
		{
			using (var xmlReader = XmlReader.Create(fileName, XmlUtils.GetXmlReaderSettings()))
			{
				var serializer = new XmlSerializer(typeof(SettingsLoader.LoaderRootWrapper<SystemSettingsInfo>));

				return ((SettingsLoader.LoaderRootWrapper<SystemSettingsInfo>)serializer.Deserialize(xmlReader)).Settings;
			}
		}

		/// <summary>
		/// Public key for signatures
		/// </summary>
		[XmlElement(ElementName = "publickeysign")]
		public string PublicKeyXmlSign
		{
			get { return this._publicKeyXmlSign; }
			set { this._publicKeyXmlSign = value; }
		}

		/// <summary>
		/// Private key for decrypt
		/// </summary>
		[XmlElement(ElementName = "privatekeydecrypt")]
		[XmlText]
		public string PrivateKeyXmlDecrypt
		{
			get { return this._privateKeyXmlDecrypt; }
			set { this._privateKeyXmlDecrypt = value; }
		}

		/// <summary>
		/// Directory license.
		/// </summary>
		[XmlElement(ElementName = "LicenseDirectory")]
		public string LicenseDirectory { get; set; }

		/// <summary>
		/// Directory logs.
		/// </summary>
		[XmlElement(ElementName = "LogDirectory")]
		public string LogDirectory { get; set; }

		/// <summary>
		/// Serialization member
		/// </summary>
		/// <returns></returns>
		public XmlSchema GetSchema()
		{
			return (null);
		}

		/// <summary>
		/// Support e-mail for buy license.
		/// </summary>
		[XmlElement(ElementName = "SupportEmail")]
		public string SupportEmail
		{
			get { return this._supportEmail; }
			set { this._supportEmail = value; }
		}

		[XmlElement(ElementName = "SupportUrl")]
		public string SupportUrl
		{
			get { return this._supportUrl; }
			set { this._supportUrl = value; }
		}

		/// <summary>
		/// Subject e-mail for buy license.
		/// </summary>
		[XmlElement(ElementName = "SupportSubject")]
		public string SupportSubject
		{
			get { return this._supportSubject; }
			set { this._supportSubject = value; }
		}

		/// <summary>
		/// Template mail for buy license.
		/// </summary>
		[XmlElement(ElementName = "SupportTemplateMail")]
		public string SupportTemplateMail
		{
			get { return this._supportTemplateMail; }
			set { this._supportTemplateMail = value; }
		}

		/// <summary>
		/// Available UI language.
		/// </summary>
		[XmlElement("available_ui_lang")]
		public List<string> AvailableUiLanguages { get; set; }

		/// <summary>
		/// Available report language.
		/// </summary>
		[XmlElement("available_report_lang")]
		public List<string> AvailableReportLanguages { get; set; }

		/// <summary>
		/// Logging enabled.
		/// </summary>
		[XmlElement("LoggingEnabled")]
		public bool LoggingEnabled { get; set; }

		/// <summary>
		/// Show xml tab.
		/// </summary>
		[XmlElement("ShowXML")]
		public bool ShowXML { get; set; }

		/// <summary>
		/// User docs app folder.
		/// </summary>
		[XmlElement("UserDocsAppFolder")]
		public string UserDocsAppFolder { get; set; }

		/// <summary>
		/// The maximum number of simultaneous requests to the database.
		/// </summary>
		[XmlElement(ElementName = "MaximumDBRequestsThreadCount")]
		public int MaximumDBRequestsThreadCount { get; set; }

		/// <summary>
		/// Template directory folder.
		/// </summary>
		[XmlElement("TemplateDirectory")]
		public string TemplateDirectory { get; set; }

		[XmlElement("PostBuildCurrentDb")]
		public string PostBuildCurrentDb { get; set; }

		[XmlElement("PostBuildSqliteDb")]
		public List<PostBuildSqliteDb> PostBuildSQLiteDbs { get; set; }

		/// <summary>
		/// Main window title (constant string or template)
		/// </summary>
		[XmlElement(ElementName = "MainWindowTitle")]
		public FormWindowTitle MainFormWindowTitle { get; set; }

		/// <summary>
		/// Available Connections types.
		/// </summary>
		[XmlArray("ConnectionTypes")]
		public ConnectionType[] ConnectionTypes { get; set; }

		///// <summary>
		///// Serialization member
		///// </summary>
		///// <returns></returns>
		//public void ReadXml(XmlReader reader)
		//{
		//   reader.ReadToFollowing("publickeysign");
		//   PublicKeyXmlSign = reader.ReadInnerXml();
		//
		//   reader.ReadToFollowing("privatekeydecrypt");
		//   PrivateKeyXmlDecrypt = reader.ReadInnerXml();
		//}

		///// <summary>
		///// Serialization member
		///// </summary>
		///// <returns></returns>
		//public void WriteXml(XmlWriter writer)
		//{
		//   //writer.WriteString(PublicKey);
		//}

		/// <summary>
		/// Service data update timeout, seconds
		/// </summary>
		[XmlElement("ServiceDataUpdateTimeout")]
		public int ServiceDataUpdateTimeout { get; set; }

		/// <summary>
		/// Service run jobs for schedule timeout, seconds
		/// </summary>
		[XmlElement("ServiceRunJobsTimeout")]
		public int ServiceRunJobsTimeout { get; set; }
	}
}
