using MSSQLServerAuditor.Model.Loaders;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

namespace MSSQLServerAuditor.Model.Settings
{
    /// <summary>
    /// base template setting
    /// </summary>
    ///
    [XmlRoot]
    public class UserTemplateSettingcs: SettingsBase
    {
        private List<ActivityNodeXML> _connection;

        public UserTemplateSettingcs()
        {
            _connection = new List<ActivityNodeXML>();
        }

        public UserTemplateSettingcs(string parent_node, string node_name, bool node_activity)
        {
            ActivityNodeXML Connection = new ActivityNodeXML(parent_node, node_name, node_activity);
            _connection = new List<ActivityNodeXML>();
            _connection.Add(Connection);
        }

        /// <summary>
        /// Report language
        /// </summary>
        [XmlElement(ElementName="InstanceTemplateName___MSSQLServerAuditor.Template.xml")]
        public List<ActivityNodeXML> Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

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
        public UserTemplateSettingcs GetCopy()
        {
            return (UserTemplateSettingcs)MemberwiseClone();
        }

        protected override void SaveToFile(string fileName)
        {
            SettingsLoader.SaveTemplateToXml(fileName, this);
        }

    }


}
