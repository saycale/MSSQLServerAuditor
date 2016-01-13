<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Databases Size (MB)" id="DatabasesSize.HTML.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Databases Size (MB)" id="DatabasesSize.HTML.en" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/MSSQLResults">

			<html>
			<head>
				<link rel="stylesheet" href="$JS_FOLDER$/tablesorter/css/theme.mssqlserverauditor.css" type="text/css"/>

				<script src="$JS_FOLDER$/json-js/json2.js"></script>
				<script src="$JS_FOLDER$/jquery-1.11.1.js"></script>
				<script src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.js"></script>
				<script src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.widgets.js"></script>

				<script type="text/javascript">
					$(document).ready(function()
						{
							$("#myErrorTable").tablesorter({
								theme : 'MSSQLServerAuditorError',

								widgets: [ "zebra", "resizable" ],

								widgetOptions : {
									zebra : ["even", "odd"]
								}
							});

							$("#myTable").tablesorter({
								theme : 'MSSQLServerAuditor',

								widgets: [ "zebra", "resizable" ],

								widgetOptions : {
									zebra : ["even", "odd"]
								}
							});
						}
					);
				</script>
			</head>
			<body>
				<style>
					body { overflow: auto; padding:0; margin:0; }
				</style>
				<xsl:if test="MSSQLResult[@SqlErrorNumber!='0' or @SqlErrorCode!='']/child::node()">
				<table id="myErrorTable">
				<thead>
					<tr>
						<th>
							Instance
						</th>
						<th>
							Query
						</th>
						<th>
							Hierarchy
						</th>
						<th>
							RecordSets
						</th>
						<th>
							#
						</th>
						<th>
							Code
						</th>
						<th>
							Number
						</th>
						<th>
							Message
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@SqlErrorNumber!='0' or @SqlErrorCode!='']">
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="@instance != ''">
									<xsl:value-of select="@instance"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="@name != ''">
									<xsl:value-of select="@name"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="@hierarchy != ''">
									<xsl:value-of select="@hierarchy"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@RecordSets != ''">
									<xsl:value-of select="@RecordSets"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@RowCount != ''">
									<xsl:value-of select="@RowCount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@SqlErrorCode != ''">
									<xsl:value-of select="@SqlErrorCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="@SqlErrorNumber != ''">
									<xsl:value-of select="@SqlErrorNumber"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="SqlErrorMessage != ''">
									<xsl:value-of select="SqlErrorMessage"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>
					</xsl:for-each>
				</tbody>
				</table>
				</xsl:if>
				<xsl:if test="MSSQLResult[@name='GetInstanceDatabasesSize' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/child::node()">
				<table id="myTable">
				<thead>
					<tr>
						<th rowspan="2">
							Instance
						</th>
						<th rowspan="2">
							Database
						</th>
						<th rowspan="2">
							Summary
						</th>
						<th colspan="2">
							Data
						</th>
						<th colspan="2">
							Log
						</th>
					</tr>
					<tr>
						<th>
							Reserved
						</th>
						<th>
							Used
						</th>
						<th>
							Reserved
						</th>
						<th>
							Used
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@name='GetInstanceDatabasesSize' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/Row">
					<tr>
						<td>
							<xsl:choose>
								<xsl:when test="../../@instance != ''">
									<xsl:value-of select="../../@instance"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="DatabaseName != ''">
									<xsl:value-of select="DatabaseName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseSizeMB != ''">
									<xsl:value-of select="format-number(DatabaseSizeMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseDataSizeMB != ''">
									<xsl:value-of select="format-number(DatabaseDataSizeMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseDataSizeUsedMB != ''">
									<xsl:value-of select="format-number(DatabaseDataSizeUsedMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseLogSizeMB != ''">
									<xsl:value-of select="format-number(DatabaseLogSizeMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseLogSizeUsedMB != ''">
									<xsl:value-of select="format-number(DatabaseLogSizeUsedMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>
					</xsl:for-each>
				</tbody>
				</table>
				</xsl:if>
			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>

	<mssqlauditorpreprocessors name="Databases Size (MB)" id="DatabasesSize.Table.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="DataGridPreprocessorDialog" name="Databases Size (MB)" id="DatabasesSize.Table.en" column="1" row="1" colspan="1" rowspan="1">
			<TableConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" UseAutoSize="true">
				<TableSource xsi:type="XmlFileTableSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetInstanceDatabasesSize' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<Columns>
						<XmlFileTableSourceItem Tag="DatabaseName"           ColumnName="Name"        HeaderAlign="Center" Type="NText"                                 />
						<XmlFileTableSourceItem Tag="DatabaseSizeMB"         ColumnName="Summary"     HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeMB"     ColumnName="Data"        HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeUsedMB" ColumnName="Data (used)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeMB"      ColumnName="Log"         HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeUsedMB"  ColumnName="Log (used)"  HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
					</Columns>
				</TableSource>
			</TableConfiguration>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>

	<mssqlauditorpreprocessors name="Bar" id="DatabasesSize.ColumnDiagramm.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Bar" id="DatabasesSize.ColumnDiagramm.en" column="1" row="1" colspan="1" rowspan="1">
			<GraphConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
				<AxisXConfiguration>
					<ShowLabels>
						true
					</ShowLabels>
					<MajorGrid>
						<Enabled>
							true
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
						<LineWidth>
							1
						</LineWidth>
					</MajorGrid>
					<MinorGrid>
						<Enabled>
							false
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
						<LineWidth>
							1
						</LineWidth>
					</MinorGrid>
				</AxisXConfiguration>

				<AxisYConfiguration>
					<ShowLabels>
						true
					</ShowLabels>
					<MajorGrid>
						<LineWidth>
							1
						</LineWidth>
						<Enabled>
							true
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MajorGrid>
					<MinorGrid>
						<LineWidth>
							1
						</LineWidth>
						<Enabled>
							true
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MinorGrid>

					<!-- http://msdn.microsoft.com/en-us/library/0c899ak8.aspx -->
					<Format>
						#,###
					</Format>

					<Name>
						MB
					</Name>
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetInstanceDatabasesSize' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<NameTag>DatabaseName</NameTag>
					<ValueTag>DatabaseDataSizeMB</ValueTag>
					<ValueTag>DatabaseLogSizeMB</ValueTag>
					<ValueGroup>Day</ValueGroup>
				</GraphSource>

				<ItemsConfiguration>
					<NameSortType>
						NameAscending
					</NameSortType>
					<Unit>
						Number
					</Unit>
				</ItemsConfiguration>

				<LegendConfiguration>
					<Enabled>
						true
					</Enabled>
					<Docking>
						Top
					</Docking>
					<Alignment>
						Center
					</Alignment>
					<GraphicOverlap>
						true
					</GraphicOverlap>
				</LegendConfiguration>

				<GraphType>
					StackedColumn
				</GraphType>
			</GraphConfiguration>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>

	<mssqlauditorpreprocessors name="Pie" id="DatabasesSize.PieDiagramm.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Pie" id="DatabasesSize.PieDiagramm.en" column="1" row="1" colspan="1" rowspan="1">
			<GraphConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
				<AxisXConfiguration>
					<ShowLabels>
						true
					</ShowLabels>
					<MajorGrid>
						<Enabled>
							false
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
						<LineWidth>
							1
						</LineWidth>
					</MajorGrid>
					<MinorGrid>
						<Enabled>
							false
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
						<LineWidth>
							1
						</LineWidth>
					</MinorGrid>
				</AxisXConfiguration>

				<AxisYConfiguration>
					<ShowLabels>
						true
					</ShowLabels>
					<MajorGrid>
						<LineWidth>
							1
						</LineWidth>
						<Enabled>
							false
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MajorGrid>
					<MinorGrid>
						<LineWidth>
							1
						</LineWidth>
						<Enabled>
							false
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MinorGrid>
					<Format>
						#,###
					</Format>
					<Name>
						MB
					</Name>
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetInstanceDatabasesSize' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<NameTag>DatabaseName</NameTag>
					<ValueTag>DatabaseDataSizeMB</ValueTag>
				</GraphSource>

				<ItemsConfiguration>
					<NameSortType>
						NameAscending
					</NameSortType>
					<Unit>
						Byte
					</Unit>
				</ItemsConfiguration>

				<LegendConfiguration>
					<Enabled>
						true
					</Enabled>
					<Docking>
						Top
					</Docking>
					<Alignment>
						Center
					</Alignment>
					<GraphicOverlap>
						false
					</GraphicOverlap>
				</LegendConfiguration>

				<GraphType>
					Pie
				</GraphType>
			</GraphConfiguration>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
