<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Столбчатая диаграмма" id="DatabasesSize.ColumnDiagramm.ru" columns="100" rows="80;20" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Столбчатая диаграмма" id="DatabasesSize.ColumnDiagramm.ru" column="1" row="1" colspan="1" rowspan="1">
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
						МБайт
					</Name>
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetDatabaseSize' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<NameTag>DatabaseFileName</NameTag>
					<ValueTag>DatabaseFileSizeUsedMB</ValueTag>
					<ValueTag>DatabaseFileSizeFreeMB</ValueTag>
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
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Файлы для базы данных (МБайт)" id="DatabaseFilesSize.HTML.ru" column="1" row="2" colspan="1" rowspan="1">
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
							Зкземпляр
						</th>
						<th>
							Запрос
						</th>
						<th>
							Категория
						</th>
						<th>
							Наборов
						</th>
						<th>
							#
						</th>
						<th>
							Код
						</th>
						<th>
							Номер
						</th>
						<th>
							Сообщение
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
				<xsl:if test="MSSQLResult[@name='GetDatabaseSize' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/child::node()">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Экземпляр
						</th>
						<th>
							Файл
						</th>
						<th>
							Путь
						</th>
						<th>
							Тип
						</th>
						<th>
							Зарезервированное
						</th>
						<th>
							Используемое
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@name='GetDatabaseSize' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/Row">
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
								<xsl:when test="DatabaseFileName != ''">
									<xsl:value-of select="DatabaseFileName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="DatabaseFilePhysicalName != ''">
									<xsl:value-of select="DatabaseFilePhysicalName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="DatabaseFileStatusDesc != ''">
									<xsl:value-of select="DatabaseFileStatusDesc"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseFileSizeMB != ''">
									<xsl:value-of select="format-number(DatabaseFileSizeMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseFileSizeUsedMB != ''">
									<xsl:value-of select="format-number(DatabaseFileSizeUsedMB, '###,###,##0.00')"/>
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

	<mssqlauditorpreprocessors name="Круговая диаграмма" id="DatabasesSize.PieDiagramm.ru" columns="100" rows="80;20" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Круговая диаграмма" id="DatabasesSize.PieDiagramm.ru" column="1" row="1" colspan="1" rowspan="1">
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
						МБайт
					</Name>
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetDatabaseSize' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<NameTag>DatabaseFileName</NameTag>
					<ValueTag>DatabaseFileSizeMB</ValueTag>
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
						Bottom
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
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Файлы для базы данных (МБайт)" id="DatabaseFilesSize.HTML.ru" column="1" row="2" colspan="1" rowspan="1">
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
							Зкземпляр
						</th>
						<th>
							Запрос
						</th>
						<th>
							Категория
						</th>
						<th>
							Наборов
						</th>
						<th>
							#
						</th>
						<th>
							Код
						</th>
						<th>
							Номер
						</th>
						<th>
							Сообщение
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
				<xsl:if test="MSSQLResult[@name='GetDatabaseSize' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/child::node()">
				<table id="myTable">
				<thead>
					<tr>
						<th>
							Экземпляр
						</th>
						<th>
							Файл
						</th>
						<th>
							Путь
						</th>
						<th>
							Тип
						</th>
						<th>
							Зарезервированное
						</th>
						<th>
							Используемое
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResult[@name='GetDatabaseSize' and @SqlErrorNumber='0' and @hierarchy='']/RecordSet[@id='1']/Row">
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
								<xsl:when test="DatabaseFileName != ''">
									<xsl:value-of select="DatabaseFileName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="DatabaseFilePhysicalName != ''">
									<xsl:value-of select="DatabaseFilePhysicalName"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="DatabaseFileStatusDesc != ''">
									<xsl:value-of select="DatabaseFileStatusDesc"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseFileSizeMB != ''">
									<xsl:value-of select="format-number(DatabaseFileSizeMB, '###,###,##0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>&#160;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td align="right">
							<xsl:choose>
								<xsl:when test="DatabaseFileSizeUsedMB != ''">
									<xsl:value-of select="format-number(DatabaseFileSizeUsedMB, '###,###,##0.00')"/>
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
