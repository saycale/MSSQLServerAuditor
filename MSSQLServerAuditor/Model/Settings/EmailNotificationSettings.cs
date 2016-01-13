using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Settings
{
	[Serializable]
	public class EmailNotificationSettings
	{
		[XmlElement(ElementName = "RecipientEmailAddress")]
		public string RecipientEmailAddress { get; set; }

		[XmlElement(ElementName = "ChartSettings")]
		public ChartSettings chartSettings { get; set; }

		[XmlElement(ElementName = "SmtpSettings")]
		public SmtpSettings SmtpSettings { get; set; }
	}
}
