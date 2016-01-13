using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Xml;
using log4net;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Groups;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Preprocessor
{
	public class QueryResultXmlTransformer : IQueryResultXmlTransformer
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private const string TimestampAttributeName = "timestamp";
		private const string QueryResultNodeName    = "MSSQLResult";

		public XmlDocument Transform(QueryResultDataSource dataSource)
		{
			GroupDefinition      database      = dataSource.NodeDefinition.Group;
			MultyQueryResultInfo queriesResult = dataSource.QueriesResult;
			MsSqlAuditorModel    model         = dataSource.Model;

			if (queriesResult == null)
			{
				return null;
			}

			try
			{
				DateTime    timestamp = queriesResult.Timestamp;
				XmlDocument xml       = new XmlDocument();

				xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", String.Empty));

				XmlElement rootNode = xml.CreateElement(Consts.ResultsTag);
				rootNode.SetAttribute(TimestampAttributeName, timestamp.ToInternetString());
				xml.AppendChild(rootNode);

				foreach (TemplateNodeResultItem tuple in queriesResult.List)
				{
					TemplateNodeQueryInfo templateNodeQueryInfo = tuple.TemplateNodeQuery;
					QueryResultInfo       queryResult           = tuple.QueryResult;

					foreach (
						KeyValuePair<InstanceInfo, QueryInstanceResultInfo> instancePair in queryResult.InstancesResult)
					{
						InstanceInfo instance = instancePair.Key;

						QueryInstanceResultInfo queryInstanceResult = instancePair.Value;

						if (queryInstanceResult.ErrorInfo == null)
						{
							try
							{
								QueryDatabaseResultInfo databaseResult = queryInstanceResult.DatabasesResult[database.Name];
								int                     rowCount       = 0;

								if (databaseResult != null)
								{
									rowCount = databaseResult.DataTables != null
										? databaseResult.DataTables.Where(x => x != null).Select(x => x.Rows).Sum(x => x.Count)
										: 0;
								}

								GenerateResultDefinition(
									rootNode,
									templateNodeQueryInfo,
									databaseResult.ErrorInfo,
									instance,
									rowCount,
									(databaseResult.DataTables != null ? databaseResult.DataTables.Length : 0)
								);
							}
							catch (Exception ex)
							{
								log.Error("Error in 'Extracts data from queries and saves it to Xml-files'", ex);
							}
						}
						else
						{
							GenerateResultDefinition(
								rootNode,
								templateNodeQueryInfo,
								queryInstanceResult.ErrorInfo,
								instance,
								0,
								0
							);
						}
					}
				}

				foreach (TemplateNodeResultItem tuple in queriesResult.List)
				{
					TemplateNodeQueryInfo templateNodeQueryInfo = tuple.TemplateNodeQuery;

					if (templateNodeQueryInfo.GetType() != typeof(TemplateNodeSqlGuardQueryInfo))
					{
						model.GetQueryByTemplateNodeQueryInfo(templateNodeQueryInfo);
					}

					QueryResultInfo queryResult = tuple.QueryResult;

					foreach (
						KeyValuePair<InstanceInfo, QueryInstanceResultInfo> instancePair in queryResult.InstancesResult)
					{
						InstanceInfo            instance            = instancePair.Key;
						QueryInstanceResultInfo queryInstanceResult = instancePair.Value;

						if (queryInstanceResult.ErrorInfo == null)
						{
							foreach (KeyValuePair<string, QueryDatabaseResultInfo> namedResult in queryInstanceResult.DatabasesResult)
							{
								if (namedResult.Key == database.Name)
								{
									XmlNode parent = rootNode.ChildNodes.OfType<XmlNode>().FirstOrDefault(
										x =>
										(
											x.Attributes["instance"]        != null &&
											x.Attributes["instance"].Value  == instance.Name
										)
										&&
										(
											x.Attributes["name"]       != null &&
											x.Attributes["name"].Value == templateNodeQueryInfo.QueryName
										)
										&&
										(
											x.Attributes["hierarchy"]      != null &&
											x.Attributes["hierarchy"].Value == (templateNodeQueryInfo.ResultHierarchy ?? string.Empty)
										)
									);

									QueryDatabaseResultInfo databaseResult = namedResult.Value;

									if (databaseResult.DataTables != null)
									{
										Int64 recordSet = 1L;

										foreach (DataTable curTable in databaseResult.DataTables)
										{
											if (parent != null)
											{
												parent.InnerXml = parent.InnerXml +
													ProcessDataTableAsStringXml(curTable, recordSet);
											}

											recordSet++;
										}
									}

									if (databaseResult.QueryItem.ParentQuery.Scope == QueryScope.Database)
									{
										if (!string.IsNullOrEmpty(databaseResult.Database))
										{
											XmlAttribute attr = xml.CreateAttribute("database");
											attr.Value = databaseResult.Database;
											parent.Attributes.Append(attr);
										}

										if (!string.IsNullOrEmpty(databaseResult.DatabaseId))
										{
											XmlAttribute attr = xml.CreateAttribute("databaseId");
											attr.Value = databaseResult.DatabaseId;
											parent.Attributes.Append(attr);
										}
									}
									else if (databaseResult.QueryItem.ParentQuery.Scope == QueryScope.InstanceGroup)
									{
										if (!string.IsNullOrEmpty(databaseResult.Database))
										{
											XmlAttribute attr = xml.CreateAttribute("InstanceGroupName");
											attr.Value = databaseResult.Database;
											parent.Attributes.Append(attr);
										}
									}
								}
							}
						}
					}
				}

				return xml;
			}
			catch (Exception ex)
			{
				log.Error("Error in 'Extracts data from queries and saves it to Xml-files'", ex);
			}

			return null;
		}

		private static void GenerateResultDefinition(
			XmlElement            rootNode,
			TemplateNodeQueryInfo templateNodeQueryInfo,
			ErrorInfo             errorInfo,
			InstanceInfo          instance,
			long                  rowCount,
			Int64                 recordSets
		)
		{
			string     errorCode   = string.Empty;
			string     errorNumber = "0";
			XmlElement node        = rootNode.OwnerDocument.CreateElement(QueryResultNodeName);

			node.SetAttribute("instance",   instance.Name);
			node.SetAttribute("name",       templateNodeQueryInfo.QueryName);
			node.SetAttribute("RecordSets", recordSets.ToString());
			node.SetAttribute("RowCount",   rowCount.ToString());

			if (errorInfo != null)
			{
				XmlElement errorNode = rootNode.OwnerDocument.CreateElement("SqlErrorMessage");

				errorNode.InnerText = errorInfo.Message;
				node.AppendChild(errorNode);
				errorCode = errorInfo.Code;
				errorNumber = errorInfo.Number;
			}

			node.SetAttribute("SqlErrorCode",   errorCode);
			node.SetAttribute("SqlErrorNumber", errorNumber);
			node.SetAttribute("hierarchy",      templateNodeQueryInfo.ResultHierarchy);

			rootNode.AppendChild(node);
		}

		private static string ProcessDataTableAsStringXml(
			DataTable dataTable,
			Int64?    recordSetNumber
		)
		{
			StringBuilder sb = new StringBuilder();

			if (dataTable != null)
			{
				sb.AppendLine(
					string.Format(
						Environment.NewLine + "  <RecordSet id=\"{0}\" RowCount=\"{1}\">",
						recordSetNumber,
						dataTable.Rows.Count
					)
				);

				int rowNumber = 1;

				foreach (DataRow r in dataTable.Rows)
				{
					sb.AppendLine(string.Format("    <Row  id=\"{0}\">", rowNumber));

					foreach (DataColumn c in dataTable.Columns)
					{
						if (c.ColumnName != null)
						{
							sb.Append("       <" + c.ColumnName.DeleteSpecChars() + ">");
							sb.Append(FormatResultItemAsString(r, c));
							sb.AppendLine("</" + c.ColumnName.DeleteSpecChars() + ">");
						}
					}

					rowNumber++;

					sb.AppendLine("    </Row>");
				}

				sb.AppendLine("  </RecordSet>");
			}

			return sb.ToString();
		}

		private static string FormatResultItemAsString(
			DataRow    row,
			DataColumn col
		)
		{
			object        value = row[col];
			StringBuilder sb    = new StringBuilder();

			if (value is DBNull)
			{
				sb.Append(String.Empty);
			}
			else if (col.DataType == typeof(DateTime))
			{
				sb.Append(((DateTime)value).ToInternetString());
			}
			else if (col.DataType == typeof(byte[]))
			{
				sb.Append("0x" + BitConverter.ToString((byte[])value));
			}
			else
			{
				sb.Append(Convert.ToString(value, CultureInfo.InvariantCulture.NumberFormat));
			}

			return EscapeNonXmlCharacters(sb);
		}

		private static string EscapeNonXmlCharacters(StringBuilder sb)
		{
			for (int i = 0; i < sb.Length; i++)
			{
				if (!XmlConvert.IsXmlChar(sb[i]))
				{
					var oldString = sb.ToString(i, 1);
					var newString = "\\" + XmlConvert.EncodeName(oldString).Trim('_');

					sb.Replace(oldString, newString);
				}
			}

			return SecurityElement.Escape(sb.ToString());
		}
	}
}
