using System.Xml.Serialization;
using MSSQLServerAuditor.Model.Loaders;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Settings
{
    /// <summary>
    /// The activity layout of settings.
    /// </summary>
    [XmlRoot]
    public class ActivityLayoutSetting: SettingBaseTemplate
    {
        /// <summary>
        /// Method of saving settings to a file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        protected override void _createDocumentoXML(string fileName)
        {
            var wrapper = new SettingsLoader.LoaderRootWrapperActivity<List<InstanceTemplate>>
            {
                Settings = UserSettings
            };

            var serializer = new XmlSerializer(typeof(MSSQLServerAuditor.Model.Loaders.SettingsLoader.LoaderRootWrapperActivity<List<InstanceTemplate>>));
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                using (var xmlWriter = XmlWriter.Create(stream, XmlUtils.GetXmlWriterSettings()))
                {
                    serializer.Serialize(xmlWriter, wrapper);
                }
            }
        }

        /// <summary>
        /// Create documnet to XML.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public void CreateDocumentoXML(string fileName)
        {
            _createDocumentoXML(fileName);
        }

        ///// <summary>
        ///// Load document from XML.
        ///// </summary>
        ///// <param name="filename">File name.</param>
        ///// <returns></returns>
        //protected override List<InstanceTemplate> _loadDocumentFromXML(string filename)
        //{

        //    List<InstanceTemplate> userTemplateSettings = new List<InstanceTemplate>();

        //    XmlTextReader reader = new  XmlTextReader(filename);

        //    while (reader.Read())
        //    {
        //        if (reader.NodeType == XmlNodeType.Element)
        //        {
        //            string InstTemplateName = "";
        //            while (reader.NodeType != XmlNodeType.None)
        //            {
        //                string fullPath = "";
        //                string nodeName = "";
        //                bool activ = true;
        //                bool IsTemplateReed = false;
        //                while (!IsTemplateReed && reader.NodeType != XmlNodeType.None)
        //                {
        //                    reader.Read();
        //                    if (reader.NodeType == XmlNodeType.Element)
        //                    {
        //                        if (reader.Name == "template")
        //                        {
        //                            fullPath += reader.GetAttribute("name").ToString() + ";";
        //                        }
        //                        else if (reader.Name == "InstanceTemplate")
        //                        {

        //                            InstTemplateName = reader.GetAttribute("name").ToString();
        //                        }

        //                        else if (reader.Name == "Enabled")
        //                        {
        //                            activ = reader.ReadElementContentAsBoolean();
        //                            IsTemplateReed = true;
        //                        }

        //                    }
        //                }
        //                if (reader.NodeType != XmlNodeType.None)
        //                {
        //                    string[] str = fullPath.Split(';');
        //                    nodeName = str[str.Length - 2];
        //                    if (str.Length > 2)
        //                    {
        //                        //- name -';'
        //                        fullPath = fullPath.Substring(0, fullPath.Length - nodeName.Length - 1);
        //                        fullPath = fullPath.TrimEnd(';');
        //                    }
        //                    else
        //                    {
        //                        fullPath = null;
        //                    }
        //                    userTemplateSettings.Add(new InstanceTemplate(fullPath, nodeName, activ, InstTemplateName));
        //                }
        //            }

        //        }
        //    }

        //    reader.Close();
        //    return userTemplateSettings;
        //}

        ///// <summary>
        ///// Load document from XML.
        ///// </summary>
        ///// <param name="filename">File name.</param>
        ///// <returns>List instance template.</returns>
        //public List<InstanceTemplate> LoadDocumentFromXML(string filename)
        //{
        //    return _loadDocumentFromXML(filename);
        //}

    }
}
