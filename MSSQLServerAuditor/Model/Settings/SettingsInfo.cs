using System.Xml.Serialization;
using System.Collections.Generic;
using MSSQLServerAuditor.Model.Loaders;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Settings
{
	/// <summary>
	/// User settings
	/// </summary>
	[XmlRoot]
	public class SettingsInfo : SettingsBase
	{
		private string _interfaceLanguage;
		private string _reportLanguage;
		private bool   _showCloseTabConfirmation;
		private bool   _warnAboutUnsignedQuery;

		/// <summary>
		/// Initializing object SettingsInfo.
		/// </summary>
		public SettingsInfo()
		{
			this._interfaceLanguage        = "en";
			this._reportLanguage           = "en";
			this._showCloseTabConfirmation = true;
			this._warnAboutUnsignedQuery   = true;
			this.ShowMainMenu              = true;
			this.ShowConnectionTabIfSingle = true;

			// generate key at random
			this.EncryptionKey = SecureRandom.GenerateKey(32);
		}

		/// <summary>
		/// Interface language
		/// </summary>
		[XmlElement(ElementName = "interface_lang")]
		public string InterfaceLanguage
		{
			get { return this._interfaceLanguage; }
			set { this._interfaceLanguage = value; }
		}

		/// <summary>
		/// Report language
		/// </summary>
		[XmlElement(ElementName = "report_lang")]
		public string ReportLanguage
		{
			get { return this._reportLanguage; }
			set { this._reportLanguage = value; }
		}

		/// <summary>
		/// Sql timeout
		/// </summary>
		[XmlElement(ElementName = "sqltimeout")]
		public int SqlTimeout { get; set; }

		/// <summary>
		/// The maximum number of simultaneous requests to the database.
		/// </summary>
		[XmlElement(ElementName = "MaximumDBRequestsThreadCount")]
		public int MaximumDBRequestsThreadCount { get; set; }

		/// <summary>
		/// Show status panel
		/// </summary>
		[XmlElement(ElementName = "showstatuspanel")]
		public bool ShowStatusPanel { get; set; }

		/// <summary>
		/// Is application main menu visible
		/// </summary>
		[XmlElement(ElementName = "showmainmenu")]
		public bool ShowMainMenu { get; set; }

		/// <summary>
		/// Show address line
		/// </summary>
		[XmlElement(ElementName = "showaddressline")]
		public bool ShowAddressLine { get; set; }

		/// <summary>
		/// Will the connection tab be visible if it's only one (no other connection are opend)
		/// </summary>
		[XmlElement(ElementName = "showsinglecnntab")]
		public bool ShowConnectionTabIfSingle { get; set; }

		[XmlElement(ElementName = "HideSingleReportNode")]
		public bool HideSingleNodeInReportsTree { get; set; }

		/// <summary>
		/// Connections in tabs.
		/// </summary>
		[XmlElement(ElementName = "conectionsInTabs")]
		public bool ConnectionsInTabs { get; set; }

		/// <summary>
		/// Show close tab confirmation
		/// </summary>
		[XmlElement(ElementName = "showclosetabconfirm")]
		public bool ShowCloseTabConfirmation
		{
			get { return this._showCloseTabConfirmation; }
			set { this._showCloseTabConfirmation = value; }
		}

		[XmlElement(ElementName = "EmailNotificationSettings")]
		public EmailNotificationSettings EmailNotificationSettings { get; set; }

		//[XmlElement(ElementName = "TemplateNodesRefreshJobs")]
		//public List<TemplateNodeUpdateJob> TemplateNodesRefreshJobs { get; set; }

		/// <summary>
		/// Show xml tab.
		/// </summary>
		[XmlElement("ShowXML")]
		public bool ShowXML { get; set; }

		/// <summary>
		/// Encryption key
		/// </summary>
		[XmlElement("EncryptionKey")]
		public string EncryptionKey { get; set; }

		/// <summary>
		/// Template directory folder.
		/// </summary>
		[XmlElement("TemplateDirectory")]
		public string TemplateDirectory { get; set; }

		/// <summary>
		/// Warn about unsigned query
		/// </summary>
		public bool WarnAboutUnsignedQuery
		{
			get { return this._warnAboutUnsignedQuery; }
			set { this._warnAboutUnsignedQuery = value; }
		}

		/// <summary>
		/// Screen bounds and window state of the application main form
		/// </summary>
		public WindowStateAndBounds MainWindowState { get; set; }

		// !!! This property must be the last one in list of public properties in this class.
		// It's because of strange way of its reading in SystemSettingsInfo class
		/// <summary>
		/// System settings
		/// </summary>
		[XmlIgnore]
		public SystemSettingsInfo SystemSettings { get; set; }

		/// <summary>
		/// Copy settings
		/// </summary>
		/// <returns>New settings</returns>
		public SettingsInfo GetCopy()
		{
			return (SettingsInfo)MemberwiseClone();
		}

		protected void CopyNotReloadTreeParameters(SettingsInfo info)
		{
			info.ShowStatusPanel              = this.ShowStatusPanel;
			info.SqlTimeout                   = this.SqlTimeout;
			info.MaximumDBRequestsThreadCount = this.MaximumDBRequestsThreadCount;
			info.ShowCloseTabConfirmation     = this.ShowCloseTabConfirmation;
		}

		public bool IsNeedReloadTree(SettingsInfo info)
		{
			var oldSettingCopy = info.GetCopy();

			this.CopyNotReloadTreeParameters(oldSettingCopy);

			if (!this.EqualSetting(info) && !this.EqualSetting(oldSettingCopy))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//public override bool Equals(object obj)
		//{
		//    if (ReferenceEquals(null, obj)) return false;
		//    if (ReferenceEquals(this, obj)) return true;
		//    if (obj.GetType() != this.GetType()) return false;
		//    return Equals((SettingsInfo) obj);
		//}

		/// <summary>
		/// Save to file.
		/// </summary>
		/// <param name="fileName">File name.</param>
		protected override void SaveToFile(string strFileName)
		{
			SettingsLoader.SaveToXml(strFileName, this);
		}

		//protected bool Equals(SettingsInfo other)
		//{
		//    return string.Equals(_interfaceLanguage, other._interfaceLanguage)
		//        && string.Equals(_reportLanguage, other._reportLanguage)
		//        && _showCloseTabConfirmation.Equals(other._showCloseTabConfirmation)
		//        && _warnAboutUnsignedQuery.Equals(other._warnAboutUnsignedQuery)
		//        && SqlTimeout == other.SqlTimeout
		//        && MaximumDBRequestsThreadCount == other.MaximumDBRequestsThreadCount
		//        && ShowStatusPanel.Equals(other.ShowStatusPanel)
		//        && ShowMainMenu.Equals(other.ShowMainMenu)
		//        && ShowAddressLine.Equals(other.ShowAddressLine)
		//        && ShowConnectionTabIfSingle.Equals(other.ShowConnectionTabIfSingle)
		//        && HideSingleNodeInReportsTree.Equals(other.HideSingleNodeInReportsTree)
		//        && ConnectionsInTabs.Equals(other.ConnectionsInTabs)
		//        && Equals(LastDirectConnectionStrings, other.LastDirectConnectionStrings)
		//        && Equals(LastDirectConnectionStringsItems, other.LastDirectConnectionStringsItems)
		//        && Equals(LastExternalTemplatesItems, other.LastExternalTemplatesItems)
		//        && Equals(LastDirectConnectionStringsGroups, other.LastDirectConnectionStringsGroups)
		//        && string.Equals(LastSelectedTemplateId, other.LastSelectedTemplateId)
		//        && LastSelectedDataBaseType == other.LastSelectedDataBaseType
		//        && Equals(ScheduleNodeSettingList, other.ScheduleNodeSettingList)
		//        && ShowXML.Equals(other.ShowXML)
		//        && string.Equals(TemplateDirectory, other.TemplateDirectory)
		//        && MainWindowState.Equals(other.MainWindowState)
		//        && Equals(SystemSettings, other.SystemSettings);
		//}

		public bool EqualSetting(SettingsInfo secondInfo)
		{
			if (
				this.ConnectionsInTabs            != secondInfo.ConnectionsInTabs            ||
				this.HideSingleNodeInReportsTree  != secondInfo.HideSingleNodeInReportsTree  ||
				this.ShowAddressLine              != secondInfo.ShowAddressLine              ||
				this.ShowCloseTabConfirmation     != secondInfo.ShowCloseTabConfirmation     ||
				this.ShowConnectionTabIfSingle    != secondInfo.ShowConnectionTabIfSingle    ||
				this.ShowMainMenu                 != secondInfo.ShowMainMenu                 ||
				this.ShowStatusPanel              != secondInfo.ShowStatusPanel              ||
				this.ShowXML                      != secondInfo.ShowXML                      ||
				this.InterfaceLanguage            != secondInfo.InterfaceLanguage            ||
				this.MaximumDBRequestsThreadCount != secondInfo.MaximumDBRequestsThreadCount ||
				this.ReportLanguage               != secondInfo.ReportLanguage               ||
				this.SqlTimeout                   != secondInfo.SqlTimeout
			)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
