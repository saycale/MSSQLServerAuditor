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
    [Serializable]
    public class InstanceTemplate
    {
        private string _templaName;

        /// <summary>
        /// Initializing object InstanceTemplate.
        /// </summary>
        public InstanceTemplate()
        {
            Connection = new ActivityNodeXML();
            _templaName = null;
        }

        /// <summary>
        /// Instance template.
        /// </summary>
        /// <param name="parentKey">Parent node.</param>
        /// <param name="nodeName">Node name.</param>
        /// <param name="nodeActivity">Note activity.</param>
        /// <param name="templateName">Template name.</param>
        //public InstanceTemplate(string parent_key, string node_name, bool node_activity, string template_name)
        //{
        //    ActivityNodeXML Connection = new ActivityNodeXML(parent_key, node_name, node_activity);
        //    this.Connection = new ActivityNodeXML();
        //    this.Connection = Connection;
        //    _templaName = template_name;
        //}

        public InstanceTemplate(string parentKey, string nodeName, bool nodeActivity, string templateName,
			string nodeLanguage, string nodeNewname, string nodeOldname,
			string nodeImage, String nodeColor, List<ParameterInfo> parameterInfos)
        {
            Connection = new ActivityNodeXML(parentKey, nodeName, nodeActivity, nodeLanguage, nodeNewname,
                nodeOldname, nodeImage, nodeColor, parameterInfos);
            _templaName = templateName;
        }

        /// <summary>
        /// activity tempate
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string TemplateName
        {
            get { return _templaName; }
            set { _templaName = value; }
        }

        /// <summary>
        /// activity tempate
        /// </summary>
        [XmlElement(ElementName="ConfigurationTemplate")]
        public ActivityNodeXML Connection { get; set; }
    }


}
