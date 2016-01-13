<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Тестирование запросов с несколькими рекорд сетами" id="TestMultiTableSelect.HTML.en" columns="25;50;25" rows="70;20;10" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Круговая диаграмма1" id="TestMultiTableSelect.Pie.1.1" column="1" row="1" colspan="1" rowspan="1">
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
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='TestMultiTableSelect' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<NameTag>intValue1ForRecordSet1</NameTag>
					<ValueTag>intValue1ForRecordSet1</ValueTag>
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
					Pie
				</GraphType>
			</GraphConfiguration>
		</mssqlauditorpreprocessor>
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Круговая диаграмма2" id="TestMultiTableSelect.Pie.2" column="2" row="1" colspan="1" rowspan="1">
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
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='TestMultiTableSelect' and @hierarchy='']/RecordSet[@id='2']</PathToItems>
					<ItemTag>Row</ItemTag>
					<NameTag>strName1ForRecordSet2</NameTag>
					<ValueTag>intValue1ForRecordSet2</ValueTag>
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
					Column
				</GraphType>
			</GraphConfiguration>
		</mssqlauditorpreprocessor>
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Круговая диаграмма3" id="TestMultiTableSelect.Pie.3" column="3" row="1" colspan="1" rowspan="1">
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
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='TestMultiTableSelect' and @hierarchy='']/RecordSet[@id='3']</PathToItems>
					<ItemTag>Row</ItemTag>
					<NameTag>intValue2ForRecordSet3</NameTag>
					<ValueTag>intValue1ForRecordSet3</ValueTag>
					<ValueTag>intValue2ForRecordSet3</ValueTag>
					<ValueTag>intValue3ForRecordSet3</ValueTag>
					<ValueTag>intValue4ForRecordSet3</ValueTag>
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
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="TestMultiTableSelect" id="TestMultiTableSelect.HTML.1.2" column="1" row="2" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/MSSQLResults/MSSQLResult[not(@name='TestMultiTableSelect' and @hierarchy='')]">
			</xsl:template>
			<xsl:template match="/MSSQLResults/MSSQLResult[@name='TestMultiTableSelect' and @hierarchy='']">

			<xsl:variable name="SQLErrorCode" select="@SqlErrorCode"/>

			<html>
			<head>
				<link rel="stylesheet" href="$JS_FOLDER$/tablesorter/css/theme.mssqlserverauditor.css" type="text/css"/>

				<script type="text/javascript" src="$JS_FOLDER$/json-js/json2.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.widgets.js"/>

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
				<xsl:if test="MSSQLResult[@SqlErrorNumber!='0']/child::node()">
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
					<xsl:for-each select="MSSQLResult[@SqlErrorNumber!='0']">
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

				<xsl:if test="$SQLErrorCode = ''">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Экземпляр
						</th>
						<th>
							Имя
						</th>
						<th>
							Значение
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="RecordSet[@id='1']/Row">
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
								<xsl:when test="intValue1ForRecordSet1 != ''">
									<xsl:value-of select="intValue1ForRecordSet1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="intValue1ForRecordSet1 != ''">
									<xsl:value-of select="intValue1ForRecordSet1"/>
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
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="TestMultiTableSelect" id="TestMultiTableSelect.HTML.2.2" column="2" row="2" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/MSSQLResults/MSSQLResult[not(@name='TestMultiTableSelect' and @hierarchy='')]">
			</xsl:template>
			<xsl:template match="/MSSQLResults/MSSQLResult[@name='TestMultiTableSelect' and @hierarchy='']">

			<xsl:variable name="SQLErrorCode" select="@SqlErrorCode"/>

			<html>
			<head>
				<link rel="stylesheet" href="$JS_FOLDER$/tablesorter/css/theme.mssqlserverauditor.css" type="text/css"/>

				<script type="text/javascript" src="$JS_FOLDER$/json-js/json2.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.widgets.js"/>

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
				<xsl:if test="MSSQLResult[@SqlErrorNumber!='0']/child::node()">
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
					<xsl:for-each select="MSSQLResult[@SqlErrorNumber!='0']">
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

				<xsl:if test="$SQLErrorCode = ''">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Экземпляр
						</th>
						<th>
							Имя
						</th>
						<th>
							Значение
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="RecordSet[@id='2']/Row">
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
								<xsl:when test="strName1ForRecordSet2 != ''">
									<xsl:value-of select="strName1ForRecordSet2"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="intValue1ForRecordSet2 != ''">
									<xsl:value-of select="intValue1ForRecordSet2"/>
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
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="TestMultiTableSelect" id="TestMultiTableSelect.HTML.3.2" column="3" row="2" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/MSSQLResults/MSSQLResult[not(@name='TestMultiTableSelect' and @hierarchy='')]">
			</xsl:template>
			<xsl:template match="/MSSQLResults/MSSQLResult[@name='TestMultiTableSelect' and @hierarchy='']">

			<xsl:variable name="SQLErrorCode" select="@SqlErrorCode"/>

			<html>
			<head>
				<link rel="stylesheet" href="$JS_FOLDER$/tablesorter/css/theme.mssqlserverauditor.css" type="text/css"/>

				<script type="text/javascript" src="$JS_FOLDER$/json-js/json2.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.widgets.js"/>

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
				<xsl:if test="MSSQLResult[@SqlErrorNumber!='0']/child::node()">
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
					<xsl:for-each select="MSSQLResult[@SqlErrorNumber!='0']">
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

				<xsl:if test="$SQLErrorCode = ''">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Экземпляр
						</th>
						<th>
							Имя
						</th>
						<th>
							Значение
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="RecordSet[@id='3']/Row">
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
								<xsl:when test="intValue2ForRecordSet3 != ''">
									<xsl:value-of select="intValue2ForRecordSet3"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="intValue2ForRecordSet3 != ''">
									<xsl:value-of select="intValue2ForRecordSet3"/>
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
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Результаты выполнения" id="TestMultiTableSelect.HTML.Results.en" column="1" row="3" colspan="3" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/MSSQLResults">

			<html>
			<head>
				<link rel="stylesheet" href="$JS_FOLDER$/tablesorter/css/theme.mssqlserverauditor.css" type="text/css"/>

				<script type="text/javascript" src="$JS_FOLDER$/json-js/json2.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.js"/>
				<script type="text/javascript" src="$JS_FOLDER$/tablesorter/js/jquery.tablesorter.widgets.js"/>

				<script type="text/javascript">
					$(document).ready(function()
						{
							$("#myResultTable").tablesorter({
								theme : 'MSSQLServerAuditorResult',

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
				<xsl:if test="MSSQLResult/child::node()">
				<table id="myResultTable">
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
					<xsl:for-each select="MSSQLResult">
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
			</body>
			</html>
			</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
