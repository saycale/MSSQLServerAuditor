using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model
{
	public class ParameterInfoLocalizable:ParameterInfo
	{
		private readonly IList<TemplateNodeLocaleInfo> _locales = new List<TemplateNodeLocaleInfo>();

		/// <summary>
		/// Localization of the node
		/// </summary>
		[XmlElement(ElementName = "i18n")]
		internal IList<TemplateNodeLocaleInfo> Locales
		{
			get
			{
				return this._locales;
			}
		}

		public string LocParameter
		{
			get
			{
				TemplateNodeLocaleInfo name = null;

				if (Locales != null && Locales.Any())
				{
					name = Locales.FirstOrDefault(
						it => it.Language == Program.Model.Settings.InterfaceLanguage);
				}

				return name != null ? name.Text : Parameter;
			}
		}

		public ParameterInfoLocalizable(
			string                              key,
			ParameterInfoType                   type,
			string                              parameter,
			string                              @default,
			string                              value,
			IEnumerable<TemplateNodeLocaleInfo> locales = null
		) : base(key, type, parameter, @default, value)
		{
			if (locales == null)
			{
				return;
			}

			foreach (var locale in locales)
			{
				this._locales.Add(locale);
			}
		}
	}
}
