using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Utils;
using System.Collections.Generic;

namespace MSSQLServerAuditor.Model.Loaders
{
    /// <summary>
    /// Loader for settings
    /// </summary>
    public class SettingsLoader
    {
        /// <summary>
        /// Loader root wrapper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [XmlRoot(ElementName = "root")]
        public class LoaderRootWrapper<T>// where T : SettingsBase
        {
            /// <summary>
            /// Settings wrapper.
            /// </summary>
            [XmlElement(ElementName = "settings")]
            public T Settings { get; set; }
        }

        /// <summary>
        /// Loader root wrapper activity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [XmlRoot(ElementName = "MSSQLServerAuditorUserTemplateSettings")]
        public class LoaderRootWrapperActivity<T>// where T : SettingsBaseTemplate
        {
            /// <summary>
            /// Settings root wrapper activity.
            /// </summary>
            public T Settings { get; set; }
        }

        /// <summary>
        /// Loads settings from Xml-file
        /// </summary>
        /// <typeparam name="T">Exact type of instance of Settings class to be created </typeparam>
        /// <param name="fileName">Xml file name to load settings from</param>
        /// <returns>New created instance with just loaded content</returns>
        public static T LoadAsBaseFromXml<T>(string fileName) where T : SettingsBase, new()
        {
            var serializer = new XmlSerializer(typeof(LoaderRootWrapper<T>), new[] { typeof(ExtendedSetting) });
            T settings = null;
            if (fileName != null)
            {
                using (var xmlReader = XmlReader.Create(fileName, XmlUtils.GetXmlReaderSettings()))
                {
                    settings = ((LoaderRootWrapper<T>) serializer.Deserialize(xmlReader)).Settings;
                }
            }

            if (settings == null)
            {
                settings = new T();
            }

           settings.OriginFile = fileName;
           return settings;
        }

        /// <summary>
        /// Load settings from Xml-file
        /// </summary>
        /// <param name="userFileName">Xml-file name in user folder</param>
        /// <param name="systemFileName">Xml-file name in program folder</param>
        /// <returns>List of settings</returns>
        public static SettingsInfo LoadFromXml(string userFileName, string systemFileName)
        {
            var settings = LoadAsBaseFromXml<SettingsInfo>(userFileName);

            settings.SystemSettings = systemFileName != null ?
                SystemSettingsInfo.LoadFrom(systemFileName)
                : new SystemSettingsInfo();

            if (String.IsNullOrEmpty(settings.SystemSettings.PublicKeyXmlSign))
            {
                settings.SystemSettings.PublicKeyXmlSign = Data.PredefinedProperties.PublicKeySign;
            }

            if (String.IsNullOrEmpty(settings.SystemSettings.PrivateKeyXmlDecrypt))
            {
                settings.SystemSettings.PrivateKeyXmlDecrypt = Data.PredefinedProperties.PrivateKeyDecrypt;
            }

            return settings;
        }

        /// <summary>
        /// Get standart system setting.
        /// </summary>
        /// <param name="systemFileName">Setting path.</param>
        /// <returns>System setting object.</returns>
        public static SystemSettingsInfo GetSystemSetting(string systemFileName)
        {
            return !string.IsNullOrEmpty(systemFileName) ?
                SystemSettingsInfo.LoadFrom(systemFileName)
                : new SystemSettingsInfo();
        }

        /// <summary>
        /// Load as template from XML.
        /// </summary>
        /// <typeparam name="T">Type objects.</typeparam>
        /// <param name="fileName">File name.</param>
        /// <returns>List of settings.</returns>
        public static T LoadAsTemplateFromXml<T>(string fileName) where T : List<InstanceTemplate>, new()
        {
            var serializer = new XmlSerializer(typeof(LoaderRootWrapperActivity<T>), new[] { (typeof(InstanceTemplate)) });
            T settings = null;
            if (fileName != null)
            {
                using (var xmlReader = XmlReader.Create(fileName, XmlUtils.GetXmlReaderSettings()))
                {
                    settings = ((LoaderRootWrapperActivity<T>)serializer.Deserialize(xmlReader)).Settings;
                }
            }

            if (settings == null)
            {
                settings = new T();
            }

            //settings.OriginFile = fileName;
            return settings;
        }

        private static void EnsureDirectoryExists(string fileName)
        {
            var folder = Path.GetDirectoryName(fileName);
            if (folder == null)
                return;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        /// <summary>
        /// Save settings to file
        /// </summary>
        /// <param name="fileName">Xml-file name</param>
        /// <param name="settings">Settings to be saved</param>
        public static void SaveToXml<T>(string fileName, T settings) where T : SettingsBase
        {
            EnsureDirectoryExists(fileName);

            var wrapper = new LoaderRootWrapper<T> {Settings = settings};
            var serializer = new XmlSerializer(typeof(LoaderRootWrapper<T>));
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                using (var xmlWriter = XmlWriter.Create(stream, XmlUtils.GetXmlWriterSettings()))
                {
                    serializer.Serialize(xmlWriter, wrapper);
                    settings.OriginFile = fileName;
                }
            }
        }

        /// <summary>
        /// Save layout to XML.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="settings">Settings.</param>
        public static void SaveLayoutToXml(string fileName, LayoutSettings settings)
        {
            var wrapper = new LoaderRootWrapper<LayoutSettings> { Settings = settings };

            var serializer = new XmlSerializer(typeof(LoaderRootWrapper<LayoutSettings>));
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                using (var xmlWriter = XmlWriter.Create(stream, XmlUtils.GetXmlWriterSettings()))
                {
                    serializer.Serialize(xmlWriter, wrapper);
                }
            }
        }

        /// <summary>
        /// Save template to XML.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="settings">Settings.</param>
        public static void SaveTemplateToXml (string fileName, ActivityLayoutSetting settings)
        {
            var wrapper = new LoaderRootWrapperActivity<ActivityLayoutSetting> { Settings = settings };

            var serializer = new XmlSerializer(typeof(LoaderRootWrapperActivity<ActivityLayoutSetting>));
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                using (var xmlWriter = XmlWriter.Create(stream, XmlUtils.GetXmlWriterSettings()))
                {
                    serializer.Serialize(xmlWriter, wrapper);
                }
            }
        }
    }
}
