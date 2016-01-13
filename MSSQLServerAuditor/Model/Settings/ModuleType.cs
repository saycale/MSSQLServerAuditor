using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Settings
{
	[Serializable]
	public class ModuleType
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement(ElementName = "i18n")]
		public List<i18n> Locales { get; set; }
	}
}
