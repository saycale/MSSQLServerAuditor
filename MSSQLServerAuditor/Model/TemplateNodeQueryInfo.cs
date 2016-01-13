using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using MSSQLServerAuditor.Model.Settings;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Template node query definition
	/// </summary>
	[Serializable]
	public class TemplateNodeQueryInfo
	{
		private string                       _queryFileName;
		private string                       _queryName;
		private string                       _resultHierarchy;
		private List<ParameterValue>         _parameterValues;
		private string                       _id;
		private string                       _idsHierarchyCache;
		private List<TemplateNodeLocaleInfo> _locales;

		public TemplateNodeInfo              TemplateNode { get; set; }

		public TemplateNodeQueryInfo()
		{
			this._queryFileName     = null;
			this._queryName         = null;
			this._resultHierarchy   = null;
			this._parameterValues   = null;
			this._id                = null;
			this._idsHierarchyCache = null;
			this.TemplateNode       = null;
			this._locales           = null;
		}

		/// <summary>
		/// Localization of the node
		/// </summary>
		[XmlElement(ElementName = "i18n")]
		public List<TemplateNodeLocaleInfo> Locales
		{
			get
			{
				return this._locales;
			}
			set
			{
				this._locales = value;
			}
		}

		/// <summary>
		/// Query file name
		/// </summary>
		[XmlAttribute(AttributeName = "file")]
		public string QueryFileName
		{
			get { return this._queryFileName; }
			set { this._queryFileName = value; }
		}

		/// <summary>
		/// Query name in define filename
		/// </summary>
		[XmlAttribute(AttributeName = "name")]
		public string QueryName
		{
			get { return this._queryName; }
			set { this._queryName = value; }
		}

		/// <summary>
		/// Query result hierarchy as "path" (chain if parent nodes) to node, containing result data, inside xml file.
		/// </summary>
		[XmlAttribute(AttributeName = "hierarchy")]
		public string ResultHierarchy
		{
			get { return this._resultHierarchy; }
			set { this._resultHierarchy = value; }
		}

		[XmlAttribute(AttributeName = "defaultDataBaseField")]
		public string DatabaseForChildrenFieldName { get; set; }

		/// <summary>
		/// Parameter values
		/// </summary>
		[XmlElement(ElementName = "parameter")]
		public List<ParameterValue> ParameterValues
		{
			get { return this._parameterValues; }
			set { this._parameterValues = value; }
		}

		/// <summary>
		/// Id for query (using as a part of table name)
		/// </summary>
		[XmlAttribute(AttributeName = "id")]
		public string Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		[XmlAttribute(AttributeName = "connections-select-id")]
		public string ConnectionsSelectId { get; set; }

		[XmlAttribute(AttributeName = "IsHideTabs")]
		public bool IsHideTabs { get; set; }

		[XmlAttribute(AttributeName = "IsHideTypeColumn")]
		public bool IsHideTypeColumn { get; set; }

		/// <summary>
		/// Hierarchy representation of Ids
		/// </summary>
		public string IdsHierarchy
		{
			get
			{
				if (this._idsHierarchyCache == null)
				{
					this._idsHierarchyCache = TemplateNode == null ? Id : TemplateNode.IdsHierarchy + "_" + Id;
				}

				return this._idsHierarchyCache;
			}
		}

		/// <summary>
		/// Objects to string.
		/// </summary>
		/// <returns>String objects.</returns>
		public override string ToString()
		{
			return string.Format("QueryFileName={0} QueryName={1}", QueryFileName, QueryName)
				+ string.Join(" ", ParameterValues.Select(pv => pv.ToString()));
		}

		public TemplateNodeQueryInfo Clone()
		{
			var result = (TemplateNodeQueryInfo) MemberwiseClone();

			result._parameterValues = _parameterValues.Select(pv => pv.Clone()).ToList();

			return result;
		}

		public TemplateNodeQueryInfo GetParentConnectionSelectQuery()
		{
			if (string.IsNullOrWhiteSpace(ConnectionsSelectId))
			{
				return null;
			}

			if (TemplateNode.Parent == null)
			{
				throw new InvalidTemplateException(this + " has <connections-select-id> but has no parent node");
			}

			var result = TemplateNode.ConnectionQueries.FirstOrDefault(q => q.Id == ConnectionsSelectId);

			if (result == null)
			{
				throw new InvalidTemplateException(this + " has <connections-select-id> = " + ConnectionsSelectId + "  but there is no <connections-select> with such Id in parent node");
			}

			return result;
		}

		public void ReadParametersFrom(InstanceTemplate settings)
		{
			if (settings != null)
			{
				var querySettings = settings.Connection.Activity.Parameters
					.Where(i => i.Key == IdsHierarchy && i.Type == ParameterInfoType.Parameter && i.Value != null);

				foreach (var parameterInfo in querySettings)
				{
					var parameter = ParameterValues.FirstOrDefault(p => p.Name == parameterInfo.Parameter);

					if (parameter != null)
					{
						parameter.UserValue = parameterInfo.Value;
					}
				}
			}
		}
	}
}
