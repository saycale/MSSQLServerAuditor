using System;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Connections
{
	[Serializable]
	public class DynamicConnection
	{
		public DynamicConnection()
		{
		}

		public DynamicConnection(string name, string type, bool isOdbc, Int64? queryId) : this()
		{
			this.Name    = name;
			this.Type    = type;
			this.IsOdbc  = isOdbc;
			this.QueryId = queryId;
		}

		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		[XmlAttribute(AttributeName = "isOdbc")]
		public bool IsOdbc { get; set; }

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }

		[XmlAttribute(AttributeName = "queryId")]
		public Int64? QueryId { get; set; }
	}
}
