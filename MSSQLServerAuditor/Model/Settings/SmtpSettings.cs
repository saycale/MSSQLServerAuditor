using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MSSQLServerAuditor.Model.Settings
{
	[Serializable]
	public class SmtpSettings
	{
		[XmlElement(ElementName = "ServerAddress")]
		public string ServerAddress { get; set; }

		[XmlElement(ElementName = "Port")]
		public int port { get; set; }

		[XmlElement(ElementName = "AuthenticationRequired")]
		public bool AuthenticationRequired { get; set; }

		[XmlElement(ElementName = "SSL")]
		public bool SSL { get; set; }

		[XmlElement(ElementName = "SmtpCredentials")]
		public SmtpCredentials SMTPCredentials { get; set; }

		[XmlElement(ElementName = "SenderEmailAddress")]
		public string SenderEmailAddress { get; set; }

		[XmlElement(ElementName = "EmailNotificationSettings")]
		public EmailNotificationSettings EmailNotificationSettings { get; set; }

		[XmlIgnore]
		public string UserName
		{
			get
			{
				if (this.SMTPCredentials == null)
				{
					return String.Empty;
				}

				return this.SMTPCredentials.UserName;
			}

			set
			{
				if (this.SMTPCredentials == null)
				{
					this.SMTPCredentials = new SmtpCredentials();
				}

				this.SMTPCredentials.UserName = value;
			}
		}

		[XmlIgnore]
		public string Password
		{
			get
			{
				if (this.SMTPCredentials == null)
				{
					return String.Empty;
				}

				return this.SMTPCredentials.Password;
			}

			set
			{
				if (this.SMTPCredentials == null)
				{
					this.SMTPCredentials = new SmtpCredentials();
				}

				this.SMTPCredentials.Password = value;
			}
		}
	}

	public class SmtpCredentials
	{
		[XmlAttribute(AttributeName = "UserName")]
		public string UserName { get; set; }

		[XmlAttribute(AttributeName = "Password")]
		public string Password { get; set; }
	}

	public class ChartSettings
	{
		[XmlAttribute(AttributeName = "ImageWidth")]
		public string ImageWidth { get; set; }

		[XmlAttribute(AttributeName = "ImageHeight")]
		public string ImageHeight { get; set; }

		[XmlAttribute(AttributeName = "ImageResolution")]
		public string ImageResolution { get; set; }

		[XmlIgnore]
		public System.Drawing.Size ClientSize
		{
 			get
			{
				int intOut    = 0;
				int intWidth  = 800;
				int intHeight = 600;

				if (!string.IsNullOrEmpty(this.ImageWidth))
				{
					if (int.TryParse(this.ImageWidth, out intOut))
					{
						intWidth = intOut;
					}
				}

				if (!string.IsNullOrEmpty(this.ImageHeight))
				{
					if (int.TryParse(this.ImageHeight, out intOut))
					{
						intHeight = intOut;
					}
				}

				return new System.Drawing.Size(intWidth, intHeight);
			}
		}
	}
}
