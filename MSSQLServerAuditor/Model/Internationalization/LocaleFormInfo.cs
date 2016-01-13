using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSQLServerAuditor.Model.Internationalization
{
	/// <summary>
	/// Localization data for form
	/// </summary>
	class LocaleFormInfo
	{
		private string                                          _name;
		private readonly Dictionary<string, LocaleResourceInfo> _resources;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="name">Form name</param>
		public LocaleFormInfo(string name)
		{
			this._name      = name;
			this._resources = new Dictionary<string, LocaleResourceInfo>();
		}

		/// <summary>
		/// Form name
		/// </summary>
		public string Name
		{
			get { return this._name; }
		}

		/// <summary>
		/// Add localization resource
		/// </summary>
		/// <param name="resource"></param>
		internal void AddResource(LocaleResourceInfo resource)
		{
			this._resources.Add(resource.Name, resource);
		}

		/// <summary>
		/// Get localized text
		/// </summary>
		/// <param name="resource">Resource</param>
		/// <param name="language">Language</param>
		/// <returns></returns>
		internal string GetLocalizedText(string resource, string language)
		{
			if (this._resources.ContainsKey(resource))
			{
				return this._resources[resource].GetLocalizedText(language);
			}

			return null;
		}
	}
}
