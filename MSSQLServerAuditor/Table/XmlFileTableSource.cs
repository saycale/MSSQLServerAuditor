using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.Table
{
	/// <summary>
	/// Default TableSource for extracting table data from XML file
	/// Data is stored in one of xml nodes. Path to the node must be specified with <c>PathToItems</c> property
	/// </summary>
	public class XmlFileTableSource : TableSource
	{
		private string                       _fileName;
		private string                       _pathToItems;
		private string                       _itemTag;
		private List<XmlFileTableSourceItem> _columns;
		private Int64?                       _recordSetNumber;

		public XmlFileTableSource()
		{
			this._fileName        = null;
			this._pathToItems     = null;
			this._itemTag         = null;
			this._columns         = new List<XmlFileTableSourceItem>();
			this._recordSetNumber = null;
		}

		private XmlDocument GetSourceDocument(object context)
		{
			var document = context as XmlDocument;

			if (document == null)
			{
				document = new XmlDocument();
				document.Load(this._fileName);
			}

			return document;
		}

		/// <summary>
		/// FileName of XML file
		/// </summary>
		[XmlElement]
		public string FileName
		{
			get { return this._fileName; }
			set { this._fileName = value; }
		}

		/// <summary>
		/// Tag for PlainItems
		/// </summary>
		[XmlElement]
		public string ItemTag
		{
			get { return this._itemTag; }
			set { this._itemTag = value; }
		}

		/// <summary>
		/// Path to PlainItems list from the root divided by '/' or '\' (eg. "Instance/Audit/Jobs")
		/// </summary>
		[XmlElement]
		public string PathToItems
		{
			get { return this._pathToItems; }
			set { this._pathToItems = value; }
		}

		/// <summary>
		/// Record set number. (Start index = 1)
		/// </summary>
		[XmlElement]
		public Int64? RecordSetNumber
		{
			get { return this._recordSetNumber; }
			set { this._recordSetNumber = value; }
		}

		/// <summary>
		/// Columns of table
		/// </summary>
		[XmlArray]
		public List<XmlFileTableSourceItem> Columns
		{
			get { return this._columns; }
			set { this._columns = value; }
		}

		internal override List<Dictionary<ColumnDefinition, object>> GetPlainData(object context)
		{
			var result = new List<Dictionary<ColumnDefinition, object>>();

			var document = GetSourceDocument(context);
			XPathNavigator currentElement = null;
			var navigator = document.CreateNavigator();
			var iterator = navigator.Select(XPathExpression.Compile(PathToItems));
			currentElement = iterator.OfType<XPathNavigator>().FirstOrDefault();

			if (currentElement != null)
			{
				var historyElements =
					currentElement.Select(ItemTag)
						.Cast<XPathNavigator>().ToList();

				foreach (XPathNavigator historyElementNew in historyElements)
				{
					XmlElement xmlElement = historyElementNew.UnderlyingObject as XmlElement;

					Dictionary<ColumnDefinition, object> row = new Dictionary<ColumnDefinition, object>();

					foreach (XmlFileTableSourceItem column in this._columns)
					{
						var elData = xmlElement[column.Tag];

						if (elData != null)
						{
							var value = ParameterValue.GetValue(elData.InnerXml, false, column.Type);

							if (value == DBNull.Value)
							{
								value = null;
							}

							row.Add(column, value);
						}
					}
					result.Add(row);
				}
			}
			return result;
		}

		internal override List<ColumnDefinition> ColumnDefinitions
		{
			get { return (from c in this._columns select (ColumnDefinition) c).ToList(); }
		}
	}
}
