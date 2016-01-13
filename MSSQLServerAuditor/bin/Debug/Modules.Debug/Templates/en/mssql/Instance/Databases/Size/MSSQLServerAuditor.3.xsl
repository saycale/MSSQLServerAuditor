<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Тестирование нескольких отчётов с разделителями" id="3" columns="50;50" rows="40;40;20" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Файлы для базы данных (МБайт)" id="DatabaseFilesSize.HTML.en" column="1" row="1" colspan="2" rowspan="1">
			<xsl:stylesheet version="1.0"
				  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				  xmlns:ms="urn:schemas-microsoft-com:xslt"
				  xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/">

			<xsl:variable name="SQLErrorCode" select="MSSQLResults/MSSQLResult/@SqlErrorCode"/>

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

				<xsl:if test="$SQLErrorCode != ''">
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
					<xsl:for-each select="MSSQLResults/MSSQLResult">
					<xsl:if test="@SqlErrorNumber != '0'">
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
									<xsl:variable name="MyError" select="1" />
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
					</xsl:if>
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
							БД
						</th>
						<th>
							Размер (МБ)
						</th>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="MSSQLResults/GetInstanceDatabasesSize">
						<tr>
							<td>
								<xsl:choose>
									<xsl:when test="Instance != ''">
										<xsl:value-of select="Instance"/>
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

		<mssqlauditorpreprocessor preprocessor="DataGridPreprocessorDialog" name="Распределение баз данных по размеру (МБайт)" id="DatabasesSize.Table.en" column="1" row="2" colspan="2" rowspan="1">
			<TableConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" UseAutoSize="true">
				<TableSource xsi:type="XmlFileTableSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>MSSQLResults</PathToItems>
					<ItemTag>GetInstanceDatabasesSize</ItemTag>
					<Columns>
						<XmlFileTableSourceItem Tag="DatabaseName"           ColumnName="Имя"                   HeaderAlign="Center" Type="NText"                                 />
						<XmlFileTableSourceItem Tag="DatabaseSizeMB"         ColumnName="Суммарное"             HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeMB"     ColumnName="Данные"                HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseDataSizeUsedMB" ColumnName="Данные (используется)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeMB"      ColumnName="Журнал"                HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
						<XmlFileTableSourceItem Tag="DatabaseLogSizeUsedMB"  ColumnName="Журнал (используется)" HeaderAlign="Center" Type="Float" Align="Right" Format="###,###.0"/>
					</Columns>
				</TableSource>
			</TableConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Столбчатая диаграмма" id="DatabaseFilesSize.Graph.StackedColumn.en" column="1" row="3" colspan="1" rowspan="1">
			<GraphConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
				<AxisXConfiguration>
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
					<PathToItems>MSSQLResults</PathToItems>
					<RecordSetNumber>1</RecordSetNumber>
					<ItemTag>GetInstanceDatabasesSize</ItemTag>
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
						Bottom
					</Docking>
					<GraphicOverlap>
						true
					</GraphicOverlap>
				</LegendConfiguration>

				<GraphType>
					StackedColumn
				</GraphType>
			</GraphConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Круговая диаграмма" id="DatabaseFilesSize.Graph.Pie.en" column="2" row="3" colspan="1" rowspan="1">
			<GraphConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
				<AxisXConfiguration>
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
					<PathToItems>MSSQLResults</PathToItems>
					<RecordSetNumber>1</RecordSetNumber>
					<ItemTag>GetInstanceDatabasesSize</ItemTag>
					<NameTag>DatabaseName</NameTag>
					<ValueTag>DatabaseSizeMB</ValueTag>
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
