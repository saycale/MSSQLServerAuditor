using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using MSSQLServerAuditor.Model.Loaders;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.Graph;
using MSSQLServerAuditor.Table;

namespace MSSQLServerAuditor.Model.Settings
{
    /// <summary>
    /// Supports "extended settings"
    /// </summary>
    [XmlRoot]
    public abstract class SettingsBase
    {
        /// <summary>
        /// Filename, the settings was loaded from or will be saved to
        /// </summary>
        [XmlIgnore]
        public string OriginFile { get; set; }

        private List<ExtendedSetting> _extendedSettings = new List<ExtendedSetting>();

        /// <summary>
        /// Extended settings.
        /// </summary>
        [XmlArray(ElementName = "extsettings")]
        public List<ExtendedSetting> ExtendedSettings
        {
            get { return _extendedSettings; }
            set { _extendedSettings = value; }
        }

        /// <summary>
        /// List of extended settings
        /// </summary>
        Dictionary<string, ExtendedSetting> _cachedSettings = new Dictionary<string, ExtendedSetting>();

        // All of these can be refactored a way like extracting generic key. For this case it's going to be a struct with three fields.

        private object GetExtendedSettings(string id, string preprocessor, string reportLanguage)
        {
            if (_cachedSettings.ContainsKey(id))
                return _cachedSettings[id].Settings;

            var settings = (from s in ExtendedSettings
                            where s.SettingId == id &&
                                  s.PreprocessorName == preprocessor &&
                                  s.ReportLanguage == reportLanguage
                            select s).FirstOrDefault();

            if (settings == null)
                return null;

            _cachedSettings.Add(id, settings);

            return settings.Settings;
        }

        private void SetExtendedSettings(string id, string preprocessor, string reportLanguage, string xml, object setting)
        {
            var settings = (from s in ExtendedSettings
                            where s.SettingId == id &&
                                  s.PreprocessorName == preprocessor &&
                                  s.ReportLanguage == reportLanguage
                            select s).FirstOrDefault();
            if (settings == null)
            {
                settings = new ExtendedSetting
                    {
                        SettingId = id,
                        PreprocessorName = preprocessor,
                        ReportLanguage = reportLanguage
                    };
                ExtendedSettings.Add(settings);
            }

            settings.Settings = setting;
        }

        /// <summary>
        /// Get extended settings.
        /// </summary>
        /// <typeparam name="T">Type objects.</typeparam>
        /// <param name="id">ID.</param>
        /// <param name="preprocessor">Preprocessor name.</param>
        /// <param name="reportLanguage">Report language.</param>
        /// <returns></returns>
        public T GetExtendedSettings<T>(string id, string preprocessor, string reportLanguage)
        {
            var settingsStr = GetExtendedSettings(id, preprocessor, reportLanguage);

            if (settingsStr == null)
            {
                return default(T);
            }

            return (T)settingsStr;
        }

        /// <summary>
        /// Set extended settings.
        /// </summary>
        /// <typeparam name="T">Type objects.</typeparam>
        /// <param name="id">ID.</param>
        /// <param name="preprocessor">Preprocessor name.</param>
        /// <param name="reportLanguage">Report language.</param>
        /// <param name="settings">Settings.</param>
        public virtual void SetExtendedSettings<T>(string id, string preprocessor, string reportLanguage, T settings)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var writer = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(writer, XmlUtils.GetXmlWriterSettings()))
                {
                    serializer.Serialize(xmlWriter, settings);
                    // writer.Position = 0;
                }

                writer.Position = 0;

                //
                // #248 - fix memory leaks during XML files processing
                //
                // var reader = new StreamReader(writer);
                // SetExtendedSettings(id, preprocessor, reportLanguage, reader.ReadToEnd(), settings);
                using (var reader = new StreamReader(writer))
                {
                    SetExtendedSettings(id, preprocessor, reportLanguage, reader.ReadToEnd(), settings);
                }
            }

            if (!string.IsNullOrEmpty(OriginFile))
            {
                SaveToFile(OriginFile);
            }
        }

        /// <summary>
        /// Save to file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        protected abstract void SaveToFile(string fileName);
    }
}