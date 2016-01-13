using MSSQLServerAuditor.Graph;
using MSSQLServerAuditor.Gui;
using MSSQLServerAuditor.Table;
using System;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Settings
{
    /// <summary>
    /// Class for extended settings (for table control etc...)
    /// </summary>
    [Serializable]
    public class ExtendedSetting
    {
        /// <summary>
        /// Id of extended settings
        /// </summary>
        [XmlAttribute(AttributeName = "setid")]
        public string SettingId { get; set; }

        /// <summary>
        /// Preprocessor name.
        /// </summary>
        [XmlAttribute(AttributeName = "preprocessor")]
        public string PreprocessorName { get; set; }

        /// <summary>
        /// Report language.
        /// </summary>
        [XmlAttribute(AttributeName = "report_lang")]
        public string ReportLanguage { get; set; }

        /// <summary>
        /// Settings graphics.
        /// </summary>
        [XmlElement(typeof(GraphStateSettings)), XmlElement(typeof(TableStateSettings)), XmlElement(typeof(frmConnectionSelectionSettings)), XmlElement(typeof(frmUserSettingsParametersLayout)), XmlElement(typeof(SplitterSettings))]
        public object Settings { get; set; }
    }
}