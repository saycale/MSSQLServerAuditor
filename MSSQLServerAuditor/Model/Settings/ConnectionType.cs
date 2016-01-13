using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Settings
{
	[Serializable]
	public class ConnectionType
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement(ElementName = "i18n")]
		public List<i18n> Locales { get; set; }

		/// <summary>
		/// Available module types.
		/// </summary>
		[XmlArray("ModuleTypes")]
		public ModuleType[] ModuleTypes { get; set; }
	}
}
