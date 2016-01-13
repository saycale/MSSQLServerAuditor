using System.Xml.Serialization;
using MSSQLServerAuditor.Model.Loaders;

namespace MSSQLServerAuditor.Model.Settings
{
    /// <summary>
    /// Layout settings to save.
    /// </summary>
    [XmlRoot]
    public class LayoutSettings : SettingsBase
    {
        /// <summary>
        /// Method of saving settings to a file.
        /// </summary>
        /// <param name="fileName">Filename.</param>
        protected override void SaveToFile(string fileName)
        {
            SettingsLoader.SaveToXml(fileName, this);
        }
    }
}