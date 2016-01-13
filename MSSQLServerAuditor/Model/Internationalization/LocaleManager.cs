using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Internationalization
{
	/// <summary>
	/// Localization manager
	/// </summary>
	public class LocaleManager
	{
		/// <summary>
		/// Constant of exception.
		/// </summary>
		public const string                         Exceptions = "exceptions";
		private readonly Func<string>               _getLanguage;
		readonly Dictionary<string, LocaleFormInfo> _resources;

		/// <summary>
		/// Form. If form omited.
		/// </summary>
		public Form FormIfOmited { get; set; }

		public LocaleManager()
		{
			this._getLanguage = null;
			this._resources   = new Dictionary<string, LocaleFormInfo>();
		}

		/// <summary>
		/// Default construtor
		/// </summary>
		/// <param name="getLanguage">Must return language name for localized values the localizer will provide</param>
		/// <param name="filename">Localization filename</param>
		public LocaleManager(Func<string> getLanguage, string filename) : this()
		{
			if (getLanguage == null)
			{
				throw new ArgumentNullException("getLanguage");
			}

			this._getLanguage = getLanguage;

			var document = new XmlDocument();

			document.Load(filename);

			foreach (XmlNode childNode in document.DocumentElement.ChildNodes)
			{
				foreach (XmlNode formNode in childNode.ChildNodes)
				{
					var formInfo = new LocaleFormInfo(formNode.Name);

					foreach (XmlNode resourceNode in formNode.ChildNodes)
					{
						var resourceInfo = new LocaleResourceInfo(resourceNode.Name);

						foreach (XmlNode i18nNode in resourceNode.ChildNodes)
						{
							var localeItem = new LocaleItemInfo(
								i18nNode.Attributes["name"].Value,
								i18nNode.InnerText.RemoveWhitespaces()
							);

							resourceInfo.AddLocaleItem(localeItem);
						}

						formInfo.AddResource(resourceInfo);
					}

					this._resources.Add(formInfo.Name, formInfo);
				}
			}
		}

		/// <summary>
		/// Get localized text
		/// </summary>
		/// <param name="form">Form name</param>
		/// <param name="resource">Resource name</param>
		/// <param name="language">Language (null to get from model settings)</param>
		/// <returns></returns>
		public string GetLocalizedText(string form, string resource, string language = null)
		{
			string lng = language ?? _getLanguage();

			if (string.IsNullOrEmpty(lng))
			{
				lng = "en";
			}

			return this._resources.ContainsKey(form)
				? this._resources[form].GetLocalizedText(resource, lng)
				: null;
		}

		/// <summary>
		/// Get text.
		/// </summary>
		/// <param name="resource">Resource.</param>
		/// <returns>Return localized text.</returns>
		public string GetText(string resource)
		{
			if (FormIfOmited == null)
			{
				throw new InvalidOperationException();
			}

			return GetLocalizedText(FormIfOmited.Name, resource);
		}

		/// <summary>
		/// Localize form
		/// </summary>
		/// <param name="form">Form</param>
		/// <param name="language">Language (null to get from model settings)</param>
		/// /// <param name="language">Form alias</param>
		public void LocalizeForm(LocalizableForm form, string language = null, string formAlias = null)
		{
			string name = formAlias ?? form.Name;

			LocalizeDeep(name, form.Controls, language);

			string caption = GetLocalizedText(name, "Caption", language);

			if (caption != null && !form.IgnoreLocalizationProperties.Contains("Text"))
			{
				form.Text = caption;
			}
		}

		/// <summary>
		/// Localize control collection
		/// </summary>
		/// <param name="form">Form</param>
		/// <param name="controls">Control collection</param>
		/// <param name="language">Language (null to get from model settings)</param>
		public void LocalizeDeep(Form form, Control.ControlCollection controls, string language = null)
		{
			LocalizeDeep(form.Name, controls, language);
		}

		/// <summary>
		/// Localize control collection
		/// </summary>
		/// <param name="formName">Form name</param>
		/// <param name="controls">Control collection</param>
		/// <param name="language">Language (null to get from model settings)</param>
		public void LocalizeDeep(string formName, Control.ControlCollection controls, string language = null)
		{
			foreach (Control control in controls)
			{
				if (control is Button || control is Label || control is CheckBox || control is RadioButton || control is GroupBox)
				{
					var caption = GetLocalizedText(formName, control.Name, language);

					if (caption != null)
					{
						control.Text = caption;
					}
				}

				var grid = control as DataGridView;

				if (grid != null)
				{
					foreach (DataGridViewColumn column in grid.Columns)
					{
						column.HeaderText = GetLocalizedText(formName, column.Name, language);
					}
				}

				var menu = control as MenuStrip;

				if (menu != null)
				{
					LocalizeMenuItem(formName, menu.Items, language);
				}

				if (control.ContextMenuStrip != null)
				{
					LocalizeMenuItem(formName, control.ContextMenuStrip.Items, language);
				}

				if (control is TabControl)
				{
					foreach (var tab in ((TabControl)control).TabPages.OfType<TabPage>())
					{
						var caption = GetLocalizedText(formName, tab.Name, language);

						if (caption != null)
						{
							tab.Text = caption;
						}
					}
				}

				LocalizeDeep(formName, control.Controls, language);
			}
		}

		private void LocalizeMenuItem(string formName, ToolStripItemCollection items, string language)
		{
			foreach (var itm in items)
			{
				ToolStripMenuItem item = itm as ToolStripMenuItem;

				if (item != null)
				{
					item.Text = GetLocalizedText(formName, item.Name, language);

					LocalizeMenuItem(formName, item.DropDownItems, language);
				}
			}
		}
	}
}
