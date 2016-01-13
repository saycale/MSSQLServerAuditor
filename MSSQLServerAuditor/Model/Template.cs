using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Template.
	/// </summary>
	[XmlRoot(ElementName = "MSSQLServerAuditorTemplate")]
	public class Template
	{
		private string            _type;
		private FormWindowTitle   _mainFormWindowTitle;
		private TemplateTreeTitle _treeTitle;

		public Template()
		{
			this._type                = null;
			this._mainFormWindowTitle = null;
			this._treeTitle           = null;
		}

		/// <summary>
		/// Informations.
		/// </summary>
		[XmlElement(ElementName = "template")]
		public List<TemplateNodeInfo> Infos { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "startuptemplateid")]
		public string StartupTemplateId { get; set; }

		[XmlElement(ElementName = "group-select")]
		public List<TemplateNodeQueryInfo> GroupQueries { get; set; }

		/// <summary>
		/// Template type string.
		/// </summary>
		[XmlAttribute(AttributeName = "type")]
		public string Type
		{
			get { return this._type; }
			set { this._type = value; }
		}

		/// <summary>
		/// TEmplate database type.
		/// </summary>
		public QuerySource DBType
		{
			get
			{
				QuerySource type;

				if (Enum.TryParse(Type, true, out type))
				{
					return type;
				}

				return QuerySource.MSSQL;
			}
		}

		/// <summary>
		/// Localization of the node
		/// </summary>
		[XmlElement(ElementName = "i18n")]
		public List<TemplateNodeLocaleInfo> Locales { get; set; }

		[XmlElement(ElementName = "MainWindowTitle")]
		public FormWindowTitle MainFormWindowTitle
		{
			get
			{
				return this._mainFormWindowTitle;
			}
			set
			{
				this._mainFormWindowTitle = value;
			}
		}

		[XmlElement(ElementName = "TreeTitle")]
		public TemplateTreeTitle TreeTitle
		{
			get
			{
				return this._treeTitle;
			}
			set
			{
				this._treeTitle = value;
			}
		}

		[XmlIgnore]
		public string Display
		{
			get
			{
				return (Locales.FirstOrDefault(l => l.Language == Program.Model.Settings.InterfaceLanguage)
					?? Locales.First()).Text.Trim();
			}
		}
	}
}
