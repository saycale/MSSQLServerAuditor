using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using log4net;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.Preprocessor
{
	/// <summary>
	/// Xsl preprocessor to replace "mssqlauditorpreprocessor" tags accoring preprocessor attribute
	/// </summary>
	public class XslPreprocessManager
	{
		private static readonly ILog                log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		readonly                List<IPreprocessor> _availablePreprocessors;
		private readonly        DbFS                _dbFs;

		public XslPreprocessManager()
		{
			this._availablePreprocessors = new List<IPreprocessor>();
			this._dbFs                   = null;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dbFs">Virtual SQLite DB file system</param>
		public XslPreprocessManager(DbFS dbFs) : this()
		{
			this._dbFs = dbFs;
		}

		/// <summary>
		/// Preprocess XSL from string
		/// </summary>
		/// <param name="str">XSL string</param>
		/// <returns>XmlReader for preprocessed XSL</returns>
		public XmlReader PreprocessXslString(string str)
		{
			List<PreprocessorAreaData> preprocessors;
			XmlDocument document = new XmlDocument();

			try
			{
				document.LoadXml(str);
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}

			return ExecuteXslPreprocessing(document, out preprocessors);
		}

		/// <summary>
		/// Execute XSL preprocessing
		/// </summary>
		/// <param name="document">XML document</param>
		/// <param name="datas">List of result items</param>
		/// <returns></returns>
		public XmlReader ExecuteXslPreprocessing(XmlDocument document, out List<PreprocessorAreaData> datas)
		{
			datas = new List<PreprocessorAreaData>();

			PreprocessElement(document, document.DocumentElement, ref datas);

			XmlReader reader = new XmlNodeReader(document.DocumentElement);

			return reader;
		}

		private int ParseIntAttribute(XmlAttribute attribute, int defaultValue)
		{
			if (attribute != null)
			{
				int result = 0;

				if (int.TryParse(attribute.Value, out result))
				{
					return result;
				}
			}

			return defaultValue;
		}

		private void PreprocessElement(XmlDocument document, XmlElement element, ref List<PreprocessorAreaData> datas, PreprocessorAreaData parentArea = null)
		{
			List<XmlElement> nodes = element.ChildNodes.OfType<XmlElement>().ToList();

			foreach (XmlElement subElement in nodes)
			{
				if (subElement == null)
				{
					continue;
				}

				switch (subElement.Name.ToLower())
				{
					case "mssqlauditorpreprocessor":
						{
							string className           = subElement.Attributes["preprocessor"].Value;

							XmlAttribute idAttr        = subElement.Attributes["id"];
							XmlAttribute nameAttr      = subElement.Attributes["name"];
							XmlAttribute columnAttr    = subElement.Attributes["column"];
							XmlAttribute rowAttr       = subElement.Attributes["row"];
							XmlAttribute colSpanAttr   = subElement.Attributes["colspan"];
							XmlAttribute rowSpanAttr   = subElement.Attributes["rowspan"];
							XmlAttribute vertTestAlign = subElement.Attributes["text-vertical-align"];
							XmlAttribute textAlign     = subElement.Attributes["text-align"];

							string id                  = "";
							string preprocName         = "unnamed";

							if (nameAttr != null)
							{
								preprocName = nameAttr.Value;
							}

							if (idAttr != null)
							{
								id = idAttr.Value;
							}

							int col     = ParseIntAttribute(columnAttr,  1);
							int row     = ParseIntAttribute(rowAttr,     1);
							int colSpan = ParseIntAttribute(colSpanAttr, 1);
							int rowSpan = ParseIntAttribute(rowSpanAttr, 1);

							VerticalTextAlign? preprocVertTestAlign = null;

							if (vertTestAlign != null)
							{
								VerticalTextAlign tempVertTestAlign;

								if (Enum.TryParse(vertTestAlign.Value, out tempVertTestAlign))
								{
									preprocVertTestAlign = tempVertTestAlign;
								}
							}

							TextAlign preprocTestAlign = TextAlign.Left;

							if (textAlign != null)
							{
								if (!Enum.TryParse(textAlign.Value, out preprocTestAlign))
								{
									preprocTestAlign = TextAlign.Left;
								}
							}

							IPreprocessor preprocessor =
								(from proc in this._availablePreprocessors where proc.GetType().Name == className select proc)
									.FirstOrDefault();

							if (preprocessor != null)
							{
								string          configuration = subElement.InnerXml;
								IContentFactory factory       = preprocessor.CreateContentFactory(id, configuration);

								PreprocessorData data = new PreprocessorData
								{
									ContentFactory    = factory,
									Name              = preprocName,
									Column            = col,
									Row               = row,
									ColSpan           = colSpan,
									RowSpan           = rowSpan,
									VerticalTextAlign = preprocVertTestAlign,
									TextAlign         = preprocTestAlign
								};

								XmlElement newSubElement = document.CreateElement("div");

								newSubElement.SetAttribute("style", "margin:0; padding:0;");
								newSubElement.InnerXml = string.Empty;

								element.ReplaceChild(newSubElement, subElement);

								if (parentArea != null)
								{
									parentArea.Preprocessors.Add(data);
								}
								else
								{
									log.ErrorFormat(
										"Invalid configuration: <mssqlauditorpreprocessor> is not embedded in <mssqlauditorpreprocessors>. " +
										"Are you using old style configuration file?" + Environment.NewLine +
										"Silently ignoring '{0}' with id='{1}' and name='{2}'",
										className,
										id,
										preprocName
									);

									throw new ArgumentOutOfRangeException(
										"document",
										"Invalid configuration: <mssqlauditorpreprocessor> is not embedded in <mssqlauditorpreprocessors>!"
									);
								}
								continue;
							}
							break;
						}
					case "mssqlauditorpreprocessors":
						{
							XmlAttribute idAttr       = subElement.Attributes["id"];
							XmlAttribute nameAttr     = subElement.Attributes["name"];
							XmlAttribute rowsAttr     = subElement.Attributes["rows"];
							XmlAttribute columnsAttr  = subElement.Attributes["columns"];
							XmlAttribute splitterAttr = subElement.Attributes["splitter"];

							string id       = "";
							string name     = "unnamed";
							string rows     = "";
							string columns  = "";
							string splitter = "";

							if (nameAttr != null)
							{
								name = nameAttr.Value;
							}
							if (idAttr != null)
							{
								id = idAttr.Value;
							}
							if (columnsAttr != null)
							{
								columns = columnsAttr.Value;
							}
							if (rowsAttr != null)
							{
								rows = rowsAttr.Value;
							}
							if (splitterAttr != null)
							{
								splitter = splitterAttr.Value;
							}

							PreprocessorAreaData container = new PreprocessorAreaData(id, name, columns, rows);

							if (string.Equals(splitter, "no", StringComparison.InvariantCultureIgnoreCase))
							{
								container.NoSplitter = true;
							}

							datas.Add(container);

							PreprocessElement(document, subElement, ref datas, container);

							container.CheckPreprocessors();

							continue;
						}
				}

				PreprocessElement(document, subElement, ref datas);
			}
		}

		/// <summary>
		/// List of IPreprocessor instances which can be used to preprocess XSL files
		/// </summary>
		public List<IPreprocessor> AvailablePreprocessors
		{
			get { return this._availablePreprocessors; }
		}

		/// <summary>
		/// File system for results save
		/// </summary>
		public DbFS DbFs
		{
			get { return this._dbFs; }
		}
	}
}
