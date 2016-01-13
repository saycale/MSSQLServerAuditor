using System;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model
{
    /// <summary>
    /// Localized info for template node
    /// </summary>
    [Serializable]
    public class TemplateNodeLocaleInfo
    {
        private string _language;
        private string _templateFile;
        private string _text;

        /// <summary>
        /// Language
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// Template filename
        /// </summary>
        [XmlAttribute(AttributeName = "file")]
        public string TemplateFile
        {
            get { return _templateFile; }
            set { _templateFile = value; }
        }

        /// <summary>
        /// Template name
        /// </summary>
        [XmlText]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}