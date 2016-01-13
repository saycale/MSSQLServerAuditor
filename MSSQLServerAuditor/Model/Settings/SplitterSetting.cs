using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Settings
{
	[Serializable]
	public class SplitterSettings
	{
		/// <summary>
		/// Gets or sets splitter settings for nodes.
		/// </summary>
		[XmlArray(ElementName = "SplitterSettings")]
		public List<SplitterSetting> SplitterNodeSettingList { get; set; }
	}

	[Serializable]
	public class SplitterSetting
	{
		[XmlAttribute(AttributeName = "nodeid")]
		public string NodeId { get; set; }

		[XmlAttribute(AttributeName = "areaid")]
		public string AreaId { get; set; }

		[XmlAttribute(AttributeName = "columns")]
		public string Columns { get; set; }

		[XmlAttribute(AttributeName = "rows")]
		public string Rows { get; set; }
	}
}