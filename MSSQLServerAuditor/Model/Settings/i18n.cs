using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Settings
{
	[Serializable]
	public class i18n
	{
		private string _language;
		private string _text;

		/// <summary>
		/// Language
		/// </summary>
		[XmlAttribute(AttributeName = "name")]
		public string Language
		{
			get { return this._language; }
			set { this._language = value; }
		}

		/// <summary>
		/// Template name
		/// </summary>
		[XmlText]
		public string Text
		{
			get { return this._text; }
			set { this._text = value; }
		}
	}
}
