using System.Xml;

namespace MSSQLServerAuditor.Utils
{
    /// <summary>
    /// Object for working with XML.
    /// </summary>
    public static class XmlUtils
    {
        private static XmlWriterSettings _xmlWriterSettings;

        /// <summary>
        /// Get the object for write xml settings.
        /// </summary>
        /// <returns>object XmlWriterSettings.</returns>
        public static XmlWriterSettings GetXmlWriterSettings()
        {
            if (_xmlWriterSettings == null)
            {
                _xmlWriterSettings = new XmlWriterSettings();
                _xmlWriterSettings.Indent = true;
                _xmlWriterSettings.IndentChars = "\t";
            }
            return _xmlWriterSettings;
        }

        private static XmlReaderSettings _xmlReaderSettings;

        /// <summary>
        /// Get the object for read xml settings.
        /// </summary>
        /// <returns>object XmlReaderSettings.</returns>
        public static XmlReaderSettings GetXmlReaderSettings()
        {
            if (_xmlReaderSettings == null)
            {
                _xmlReaderSettings = new XmlReaderSettings();
                _xmlReaderSettings.IgnoreWhitespace = true;
            }
            return _xmlReaderSettings;
        }
    }
}
