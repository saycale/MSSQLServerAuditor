
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.Drawing;
using MSSQLServerAuditor.Model.Scheduling;

namespace MSSQLServerAuditor.Model
{
    /// <summary>
    /// The activity node XML.
    /// </summary>
    public class ActivityNodeXML
    {
        /// <summary>
        /// Initializing object ActivityNodeXML.
        /// </summary>
        public ActivityNodeXML()
        {
            ParentKey = null;
            Activity = new ActivitySetting();

        }

        /// <summary>
        /// Activity node XML.
        /// </summary>
        /// <param name="parentKey">Parent node.</param>
        /// <param name="node_name">Node name.</param>
        /// <param name="node_activity">Node activity.</param>
        //public ActivityNodeXML(string parentKey, string node_name, bool node_activity)
        //{
        //    ParentKey=parentKey;
        //    Activity = new ActivitySetting(node_name, node_activity);
        //}

        public ActivityNodeXML(string parentKey, string node_name, bool node_activity, string node_language, string node_newname,
            string node_oldname, string node_image, String node_color, List<ParameterInfo> parameterInfos)
        {
            ParentKey = parentKey;
            Activity = new ActivitySetting(node_name, node_activity, node_language, node_newname, node_oldname,
                node_image, node_color, parameterInfos);
        }


        /// <summary>
        /// Parent node name
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string ParentKey { get; set; }

        /// <summary>
        /// Node Activity
        /// </summary>
        [XmlElement(ElementName = "template")]
        public ActivitySetting Activity { get; set; }

        // [XmlElement(ElementName = "ReportRefreshJob")]
        [XmlIgnore]
        public TemplateNodeUpdateJob ReportRefreshJob { get; set; }
    }

    /// <summary>
    /// Activity setting.
    /// </summary>
    public class ActivitySetting
    {
        private string _nodeName;
        private bool   _activitySet;
        private string _oldNameNode;
        private string _colorFontNode;
        private String _imageNode;
        private string _newNameNode;
        private string _language;

        /// <summary>
        /// Initializing object ActivitySetting.
        /// </summary>
        public ActivitySetting()
        {
            this._nodeName    = String.Empty;
            this._activitySet = true;
            this.Parameters   = new List<ParameterInfo>();
        }

        /// <summary>
        /// Initializing object ActivitySetting.
        /// </summary>
        /// <param name="node_name">Name node.</param>
        /// <param name="node_activity">Node is activity.</param>
        public ActivitySetting(string node_name, bool node_activity) : this()
        {
            this._nodeName    = node_name;
            this._activitySet = node_activity;
        }

        public ActivitySetting(string node_name, bool node_activity, string _language, string _newNameNode, string _oldNodename,
            string image_node, string color_node, List<ParameterInfo> parameterInfos)
            : this()
        {
            this._nodeName      = node_name;
            this._activitySet   = node_activity;
            this._oldNameNode   = _oldNodename;
            this._newNameNode   = _newNameNode;
            this._colorFontNode = color_node;
            this._imageNode     = image_node;
            this._language      = _language;
            this.Parameters     = parameterInfos;
        }

        /// <summary>
        /// Node name
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string NodeName
        {
            get { return this._nodeName; }
            set { this._nodeName = value; }
        }

        [XmlElement(ElementName = "Language")]
        public string Language
        {
            get { return this._language; }
            set { this._language = value; }
        }

        [XmlElement(ElementName = "OldNameNode")]
        public String OldNameNode
        {
            get { return this._oldNameNode; }
            set { this._oldNameNode = value; }
        }

        [XmlElement(ElementName = "NewNameNode")]
        public String NewNameNode
        {
            get { return this._newNameNode; }
            set { this._newNameNode = value; }
        }

        [XmlElement(ElementName = "ColorFont")]
        public String ColorFontNode
        {
            get { return this._colorFontNode; }
            set { this._colorFontNode = value; }
        }

        [XmlElement(ElementName = "ImageNode")]
        public String ImageNode
        {
            get { return this._imageNode; }
            set { this._imageNode = value; }
        }

        [XmlElement(ElementName = "Parameters")]
        public List<ParameterInfo> Parameters { get; set; }

        /// <summary>
        /// Node Activity
        /// </summary>
        [XmlElement(ElementName = "Enabled")]
        public bool ActivitySet
        {
            get { return this._activitySet; }
            set { this._activitySet = value; }
        }
    }
}
