using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Table
{
	/// <summary>
	/// Table configuration.
	/// </summary>
	[XmlRoot()]
	public class TableConfiguration
	{
		private TableSource _tableSource;

		/// <summary>
		/// Source data of the table. TableSource instance must have GetPlainData() method to provide data for the table
		/// </summary>
		[XmlElement]
		public TableSource TableSource
		{
			get { return this._tableSource; }
			set { this._tableSource = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether use auto size.
		/// </summary>
		[XmlAttribute]
		public bool UseAutoSize { get; set; }

		/// <summary>
		/// Loads XML file to new TableConfiguration instance
		/// </summary>
		/// <param name="fileName">FileName of the XML file to load</param>
		/// <returns>New GraphConfiguration loaded from file</returns>
		internal static TableConfiguration LoadFromXml(string fileName)
		{
			using (FileStream reader = new FileStream(fileName, FileMode.Open))
			{
				return LoadFromXml(reader);
			}
		}

		/// <summary>
		/// Loads XML from stream to new TableConfiguration instance
		/// </summary>
		/// <param name="stream">Xml configuration stream</param>
		/// <returns></returns>
		internal static TableConfiguration LoadFromXml(Stream stream)
		{
			XmlSerializer s = new XmlSerializer(typeof(TableConfiguration));

			using (XmlReader xmlReader = XmlReader.Create(stream, XmlUtils.GetXmlReaderSettings()))
			{
				return (TableConfiguration)s.Deserialize(xmlReader);
			}
		}

		/// <summary>
		/// Saves the current configuration to XML file.
		/// </summary>
		/// <param name="fileName">FileName of the XML file to save</param>
		public void SaveToXml(string fileName)
		{
			XmlSerializer s = new XmlSerializer(typeof(TableConfiguration));

			using (FileStream writer = new FileStream(fileName, FileMode.Create))
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(writer, XmlUtils.GetXmlWriterSettings()))
				{
					s.Serialize(xmlWriter, this);
				}
			}
		}

		/// <summary>
		/// Column definitions for Grid
		/// </summary>
		[XmlIgnore]
		public List<ColumnDefinition> ColumnDefinitions
		{
			get { return this._tableSource.ColumnDefinitions; }
		}
	}
}
