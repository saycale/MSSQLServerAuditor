using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace MSSQLServerAuditor.Model.Settings
{
    /// <summary>
    /// Template base setting.
    /// </summary>
    [XmlRoot]
    public abstract class SettingBaseTemplate
    {
        /// <summary>
        /// Filename, the settings was loaded from or will be saved to
        /// </summary>
        ///


        [XmlIgnore]
        public string OriginFile { get; set; }

        private List<InstanceTemplate> _userSettings = new List<InstanceTemplate>();

        /// <summary>
        /// User settings.
        /// </summary>
        [XmlArray]
        public List<InstanceTemplate> UserSettings
        {
            get { return _userSettings; }
            set { _userSettings = value; }
        }

        /// <summary>
        /// Create document to XML.
        /// </summary>
        /// <param name="fileName">File name.</param>
        protected abstract void _createDocumentoXML(string fileName);

        ///// <summary>
        ///// Load documnr from XML.
        ///// </summary>
        ///// <param name="filename">File name.</param>
        ///// <returns>Return list instance template.</returns>
        //protected abstract List<InstanceTemplate> _loadDocumentFromXML(string filename);
    }
}


