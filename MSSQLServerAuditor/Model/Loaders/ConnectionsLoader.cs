using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Loaders
{
    /// <summary>
    /// Loader for connections list
    /// </summary>
    public class ConnectionsLoader
    {
        /// <summary>
        /// Load wrapper.
        /// </summary>
        [XmlRoot(ElementName = "MSSQLServerAuditorConnections")]
        public class LoaderWrapper
        {
            private List<ConnectionGroupInfo> _infos;

            /// <summary>
            /// Information of connection.
            /// </summary>
            [XmlElement(ElementName = "connections")]
            public List<ConnectionGroupInfo> Infos
            {
                get { return _infos; }
                set { _infos = value; }
            }
        }

        /// <summary>
        /// Load connection groups from Xml-file
        /// </summary>
        /// <param name="fileName">Xml-file name</param>
        /// <returns>List of connection groups</returns>
        public static List<ConnectionGroupInfo> LoadFromXml(string fileName)
        {
            var serializer = new XmlSerializer(typeof(LoaderWrapper));

            //using (var includingReader = new XIncludingReader(fileName))

            using (var reader = XmlReader.Create(fileName, XmlUtils.GetXmlReaderSettings()))
            {
                var result = ((LoaderWrapper) serializer.Deserialize(reader)).Infos;

                foreach (ConnectionGroupInfo connectionGroup in result)
                {
                    connectionGroup.Init();
                }

                return result;
            }
        }

        /// <summary>
        /// Load one connection template from Xml-file
        /// </summary>
        /// <param name="fileName">Xml-file name</param>
        /// <returns>List of connection groups</returns>
        public static ConnectionGroupInfo LoadConnectionGroupFromXml(string fileName)
        {
            var serializer = new XmlSerializer(typeof(ConnectionGroupInfo));

            //using (var includingReader = new XIncludingReader(fileName))

            using (var reader = XmlReader.Create(fileName, XmlUtils.GetXmlReaderSettings()))
            {
                var result = ((ConnectionGroupInfo)serializer.Deserialize(reader));
                result.Init();
                return result;
            }
        }

        /// <summary>
        /// Save connection groups to file
        /// </summary>
        /// <param name="fileName">Xml-file name</param>
        /// <param name="connectionGroups">Collection of connection groups</param>
        public static void SaveToXml(string fileName, List<ConnectionGroupInfo> connectionGroups)
        {
            LoaderWrapper wrapper = new LoaderWrapper();
            wrapper.Infos = connectionGroups;

            XmlSerializer s = new XmlSerializer(typeof(LoaderWrapper));
            using (FileStream writer = new FileStream(fileName, FileMode.Create))
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, XmlUtils.GetXmlWriterSettings()))
            {
                s.Serialize(xmlWriter, wrapper);
            }
        }
    }
}
