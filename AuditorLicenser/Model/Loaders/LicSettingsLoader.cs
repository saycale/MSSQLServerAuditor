using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Licenser.Model.Loaders
{
    /// <summary>
    /// Loader for settings
    /// </summary>
    public class LicSettingsLoader
    {
        /// <summary>
        /// Load settings from Xml-file
        /// </summary>
        /// <param name="fileName">Xml-file name in user folder</param>
        /// <returns>List of settings</returns>
        public static LicSettingsInfo LoadFromXml(string fileName)
        {
            XmlSerializer s = new XmlSerializer(typeof(LicSettingsInfo));
            LicSettingsInfo settings = null;
            using (XmlReader xmlReader = XmlReader.Create(fileName, XmlUtils.GetXmlReaderSettings()))
            {
                settings = ((LicSettingsInfo)s.Deserialize(xmlReader));
            }
            return settings;
        }

        /// <summary>
        /// Save settings to file
        /// </summary>
        /// <param name="fileName">Xml-file name</param>
        /// <param name="settings">Settings</param>
        public static void SaveToXml(string fileName, LicSettingsInfo settings)
        {
            XmlSerializer s = new XmlSerializer(typeof(LicSettingsInfo));
            using (FileStream writer = new FileStream(fileName, FileMode.Create))
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, XmlUtils.GetXmlWriterSettings()))
            {
                s.Serialize(xmlWriter, settings);
            }
        }
    }
}
