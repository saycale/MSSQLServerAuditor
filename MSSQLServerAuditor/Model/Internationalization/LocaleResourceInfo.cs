using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSQLServerAuditor.Model.Internationalization
{
	/// <summary>
	/// Localization for resource
	/// </summary>
	class LocaleResourceInfo
	{
		private readonly string                     _name;
		readonly Dictionary<string, LocaleItemInfo> _localeItems;

		public LocaleResourceInfo()
		{
			this._name        = null;
			this._localeItems = new Dictionary<string, LocaleItemInfo>();
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="name">Resource name</param>
		public LocaleResourceInfo(string name) : this()
		{
			this._name = name;
		}

		/// <summary>
		/// Add localization item
		/// </summary>
		/// <param name="localeItemInfo">Localization item</param>
		internal void AddLocaleItem(LocaleItemInfo localeItemInfo)
		{
			this._localeItems.Add(localeItemInfo.Language, localeItemInfo);
		}

		/// <summary>
		/// Resource name
		/// </summary>
		public string Name
		{
			get { return this._name; }
		}

		/// <summary>
		/// Get localized text for resource
		/// </summary>
		/// <param name="language">Language</param>
		/// <returns></returns>
		internal string GetLocalizedText(string language)
		{
			if (this._localeItems.ContainsKey(language))
			{
				return this._localeItems[language].Value;
			}

			return null;
		}
	}
}
