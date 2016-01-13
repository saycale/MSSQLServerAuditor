using System;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Settings
{
	[Serializable]
	public class LastDirectConnectionString
	{
		[XmlAttribute(AttributeName = "DataBaseType")]
		public string DataBaseType { get; set; }

		/// <summary>
		/// Connection string.
		/// </summary>
		[XmlElement(ElementName = "ConnectionString")]
		public string ConnectionString { get; set; }

		/// <summary>
		/// Is ODBC Datasource.
		/// </summary>
		[XmlElement(ElementName = "IsODBC")]
		public bool IsODBC { get; set; }

		/// <summary>
		/// Server name.
		/// </summary>
		[XmlElement(ElementName = "Name")]
		public string Name { get; set; }

		public LastDirectConnectionString()
		{
			this.DataBaseType     = null;
			this.ConnectionString = null;
			this.IsODBC           = false;
			this.Name             = null;
		}
	}
}
