using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Reporting.WinForms;
using log4net;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Groups;
using MSSQLServerAuditor.ReportViewer;

namespace MSSQLServerAuditor.Preprocessor
{
	internal class RDLPreprocessorDialog : Preprocessor
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private class RDLContentFactory : ContentFactory
		{
			private readonly XmlDocument                                          _rdlReport;
			private readonly List<Tuple<TemplateNodeQueryInfo, Int64, DataTable>> _dataTablesInfoResult;

			public RDLContentFactory(
				IPreprocessor                                        preprocessor,
				string                                               id,
				string                                               configuration,
				List<Tuple<TemplateNodeQueryInfo, Int64, DataTable>> dataTablesInfoResult
			) : base(
					preprocessor,
					id,
					configuration
				)
			{
				this._dataTablesInfoResult = dataTablesInfoResult;

				this._rdlReport = CreateRdlReport(
					configuration,
					dataTablesInfoResult
				);
			}

			public override Control CreateControl()
			{
				try
				{
					if (this._rdlReport != null)
					{
						ReportViewrControl control = new ReportViewrControl();

						using (MemoryStream msStream = new MemoryStream(Encoding.UTF8.GetBytes(this._rdlReport.OuterXml)))
						{
							control.LoadReportDefinition(msStream, this._rdlReport.OuterXml);
						}

						List<ReportDataSource> dataSources =
							this._dataTablesInfoResult.Select(x => new ReportDataSource(x.Item3.TableName, x.Item3)).ToList();

						control.LoadReportDataSources(dataSources);

						control.ShowReport();

						return control;
					}
				}
				catch (Exception ex)
				{
					log.Error("RdlcPreprocessorDialog.GetPreprocessedContent(...) got an exception", ex);
					throw;
				}

				return new ReportViewrControl();
			}

			private XmlDocument CreateRdlReport(
				string                                               configuration,
				List<Tuple<TemplateNodeQueryInfo, Int64, DataTable>> dataTablesInfoResult
			)
			{
				XmlDocument rdlcDocument = new XmlDocument
				{
					InnerXml = configuration
				};

				XmlNode report = rdlcDocument.ChildNodes.Cast<XmlNode>().FirstOrDefault(
					x => x.Name.ToLower() == "report"
				);

				if (report == null)
				{
					log.Error("RdlcPreprocessorDialog.GetPreprocessedContent(...).In report data not found report tag.");
					return null;
				}

				// Set document size.
				if (this.Preprocessor.GraphicsInfo != null)
				{
					Tuple<float, float> smSize = GetSizeInMillimetrs();

					// Document width.
					XmlNode widthTag = report.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name.ToLower() == "width");

					if (widthTag != null)
					{
						widthTag.InnerText = smSize.Item1.ToString().Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, ".") + "mm";
					}
					else
					{
						XmlNode newNode = rdlcDocument.CreateNode(XmlNodeType.Element, "Width", string.Empty);
						newNode.InnerText = smSize.Item1.ToString().Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, ".") + "mm";
						report.AppendChild(newNode);
					}

					// Page width.
					XmlNode pageTag = report.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name.ToLower() == "page");

					if (pageTag != null)
					{
						XmlNode pagewidthTag =
							pageTag.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name.ToLower() == "pagewidth");

						if (pagewidthTag != null)
						{
							pagewidthTag.InnerText =
								(smSize.Item1 / 10).ToString()
									.Replace(
										System.Globalization.CultureInfo.CurrentCulture.NumberFormat
											.CurrencyDecimalSeparator, ".") + "cm";
						}
						else
						{
							XmlNode newNode = rdlcDocument.CreateNode(XmlNodeType.Element, "pagewidth", string.Empty);

							newNode.InnerText =
								(smSize.Item1 / 10).ToString()
									.Replace(
										System.Globalization.CultureInfo.CurrentCulture.NumberFormat
											.CurrencyDecimalSeparator, ".") + "cm";

							pageTag.AppendChild(newNode);
						}
					}
				}
				else
				{
					log.Error("RdlcPreprocessorDialog.GetPreprocessedContent(...).Dpi is a null.");
				}

				XmlNode dataSets = report.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name.ToLower() == "datasets");

				if (dataSets != null)
				{
					List<XmlNode> dataSetsList =
						dataSets.ChildNodes.Cast<XmlNode>().Where(x => x.Name.ToLower() == "dataset").ToList();

					foreach (var currentDataSet in dataSetsList)
					{
						// DataSetId attribute.
						int dataSetId = 1;

						XmlAttribute datasetIdAttr =
							currentDataSet.Attributes.Cast<XmlAttribute>()
								.FirstOrDefault(x => x.Name.ToLower() == "datasetnumber");

						int tmp = 0;

						if (datasetIdAttr != null && int.TryParse(datasetIdAttr.Value, out tmp))
						{
							dataSetId = tmp;
						}

						// DataSetName attribute.
						XmlAttribute nameAttr =
							currentDataSet.Attributes.Cast<XmlAttribute>()
								.FirstOrDefault(x => x.Name.ToLower() == "name");

						if (nameAttr != null && !string.IsNullOrEmpty(nameAttr.Value))
						{
							Tuple<TemplateNodeQueryInfo, long, DataTable> currentDataSetTuple =
								dataTablesInfoResult.FirstOrDefault(
									x => x.Item1.QueryName == nameAttr.Value && x.Item2 == dataSetId);

							if (currentDataSetTuple != null)
							{
								string oldValue = nameAttr.Value;

								nameAttr.Value = currentDataSetTuple.Item3.TableName;

								SetDataSet(report, oldValue, currentDataSetTuple.Item3.TableName);

								// DataSetTableName attribute.
								XmlNode dataSetInfo = currentDataSet.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name.ToLower() == "rd:datasetinfo");

								if (dataSetInfo != null)
								{
									XmlNode dataTableName = dataSetInfo.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name.ToLower() == "rd:tablename");

									if (dataTableName != null)
									{
										dataTableName.InnerText = currentDataSetTuple.Item3.TableName;
									}

									XmlNode dataSetName = dataSetInfo.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name.ToLower() == "rd:datasetname");

									if (dataSetName != null)
									{
										dataSetName.InnerText = currentDataSetTuple.Item3.TableName;
									}
								}
							}
							else
							{
								log.ErrorFormat(
									"RdlcPreprocessorDialog.GetPreprocessedContent(...). Not exists dataset table for query='{0}' and datasetId={1}",
									nameAttr.Value,
									dataSetId
								);
							}
						}
					}
				}

				return rdlcDocument;
			}

			/// <summary>
			/// Set datasources.
			/// </summary>
			/// <param name="parentNode">Parent node for search.</param>
			/// <param name="oldName">Old name datasource.</param>
			/// <param name="newName">New name datasource.</param>
			private void SetDataSet(XmlNode parentNode, string oldName, string newName)
			{
				IEnumerable<XmlNode> childNodes = parentNode.ChildNodes.Cast<XmlNode>();

				foreach (XmlNode currentChildNode in childNodes)
				{
					if (currentChildNode.Name.ToLower() == "datasetname" && currentChildNode.InnerText.ToLower() == oldName.ToLower())
					{
						currentChildNode.InnerText = newName;
					}
					else
					{
						SetDataSet(currentChildNode, oldName, newName);
					}
				}
			}

			/// <summary>
			/// Get size in milimetrs for report size.
			/// </summary>
			/// <returns></returns>
			private Tuple<float, float> GetSizeInMillimetrs()
			{
				int          scrollWidth  = SystemInformation.VerticalScrollBarWidth;
				GraphicsInfo gi           = Preprocessor.GraphicsInfo;
				Size         ownerSize    = gi.Size;
				var          currentTuple = new Tuple<float, float>(
					((float)(((ownerSize.Width - scrollWidth - 4) / gi.DpiX) * 25.4)),
					((float)((ownerSize.Height / gi.DpiY) * 25.4))
				);

				return currentTuple;
			}
		}

		/// <summary>
		/// Rdlc preprocessor dialog.
		/// </summary>
		public RDLPreprocessorDialog(
			NodeDataProvider dataProvider,
			GraphicsInfo     graphics
		) : base(
				dataProvider,
				graphics
			)
		{
		}

		private List<Tuple<TemplateNodeQueryInfo, Int64, DataTable>> GetResultTables(
			ConcreteTemplateNodeDefinition nodeDefinition,
			MultyQueryResultInfo           queriesResult
		)
		{
			List<Tuple<TemplateNodeQueryInfo, Int64, DataTable>> resultTables =
				new List<Tuple<TemplateNodeQueryInfo, Int64, DataTable>>();

			try
			{
				GroupDefinition database = nodeDefinition.Group;

				if (database != null)
				{
					foreach (QueryDatabaseResult queryDatabaseResult in queriesResult.GetDatabaseResults())
					{
						QueryDatabaseResultInfo dbResult = queryDatabaseResult.Result;

						if (dbResult == null || dbResult.DataTables == null)
						{
							continue;
						}

						if (database.Name == dbResult.Database)
						{
							Int64 recordSet = 1L;

							foreach (DataTable curTable in dbResult.DataTables)
							{
								resultTables.Add(
									new Tuple<TemplateNodeQueryInfo, Int64, DataTable>(
										queryDatabaseResult.TemplateNodeQuery,
										recordSet,
										curTable
									)
								);

								recordSet++;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				log.Error("Error in 'Extracts data from queries and saves it to table for RDLC.'", ex);
				throw;
			}

			return resultTables;
		}

		public override IContentFactory CreateContentFactory(string id, string configuration)
		{
			RDLContentFactory factory = new RDLContentFactory(
				this,
				id,
				configuration,
				GetResultTables(
					this.DataProvider.NodeDefinition,
					this.DataProvider.QueryResult
				)
			);

			return factory;
		}
	}
}
