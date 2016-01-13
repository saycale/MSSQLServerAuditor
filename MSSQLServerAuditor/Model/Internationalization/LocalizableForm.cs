using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Model.Internationalization
{
	/// <summary>
	/// Base autolocalization form
	/// </summary>
	public class LocalizableForm : Form
	{
		/// <summary>
		/// The set of values ​​ignoring the localization properties.
		/// </summary>
		public HashSet<string> IgnoreLocalizationProperties { get; private set; }

		/// <summary>
		/// Defaul constructor
		/// </summary>
		protected LocalizableForm()
		{
			Shown += LocalizableFormShown;
			IgnoreLocalizationProperties = new HashSet<string>();
		}

		/// <summary>
		/// Method for localized string.
		/// </summary>
		/// <param name="name">String for localized.</param>
		/// <returns>Localized string.</returns>
		protected string GetLocalizedText(string name)
		{
			if (Program.Model == null) return name;

			return Program.Model.LocaleManager != null
				? Program.Model.LocaleManager.GetLocalizedText(Name, name)
				: name;
		}

		private void LocalizableFormShown(object sender, EventArgs e)
		{
			if (!DesignMode)
			{
				if (Program.Model != null)
				{
					Program.Model.SettingsChanged += ModelSettingsChanged;

					LocaleManager localeManager = Program.Model.LocaleManager;

					Type baseForm = this.GetType().BaseType;

					if (baseForm != null && baseForm.IsSubclassOf(typeof(LocalizableForm)))
					{
						// localize base form
						localeManager.LocalizeForm(this, formAlias: baseForm.Name);
					}

					localeManager.LocalizeForm(this);
				}
			}

			Icon = Properties.Resources.appIcon;
		}

		void ModelSettingsChanged(object sender, SettingsChangedEventArgs e)
		{
			Program.Model.LocaleManager.LocalizeForm(this);
		}
	}
}
