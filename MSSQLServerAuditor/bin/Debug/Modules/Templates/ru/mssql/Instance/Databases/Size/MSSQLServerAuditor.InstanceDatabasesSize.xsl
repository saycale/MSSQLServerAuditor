<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Распределение баз данных по размеру (МБайт)" id="InstanceDatabasesSize.HTML.ru" columns="100" rows="90;10" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="" id="InstanceDatabasesSize.HTML.ru" column="1" row="1" colspan="1" rowspan="1">
			<xsl:stylesheet version="1.0"
					xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
					xmlns:ms="urn:schemas-microsoft-com:xslt"
					xmlns:dt="urn:schemas-microsoft-com:datatypes">

			<xsl:template match="/MSSQLResults/MSSQLResult[not(@name='GetInstanceDatabasesSize' and @hierarchy='')]">
			</xsl:template>
			<xsl:template match="/MSSQLResults/MSSQLResult[@name='GetInstanceDatabasesSize' and @hierarchy='']">

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
						<th rowspan="2">
							Экземпляр
						</th>
						<th rowspan="2">
							База данных
						</th>
						<th rowspan="2">
							Суммарное
						</th>
						<th colspan="2">
							Данные
						</th>
						<th colspan="2">
							Журнал транзакций
						</th>
					</tr>
					<tr>
						<th>
							Зарезервированное
						</th>
						<th>
							Используемое
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

		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="Результаты выполнения" id="InstanceDatabasesSize.Results.HTML.ru" column="1" row="2" colspan="1" rowspan="1">
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

	<mssqlauditorpreprocessors name="Распределение баз данных по размеру (МБайт)" id="DatabasesSize.Table.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="DataGridPreprocessorDialog" name="Распределение баз данных по размеру (МБайт)" id="DatabasesSize.Table.ru" column="1" row="1" colspan="1" rowspan="1">
		<TableConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" UseAutoSize="true">
			<TableSource xsi:type="XmlFileTableSource">
				<FileName>$INPUT$</FileName>
				<PathToItems>MSSQLResults</PathToItems>
				<RecordSetNumber>1</RecordSetNumber>
				<ItemTag>MSSQLResult/RecordSet/Row</ItemTag>
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
	</mssqlauditorpreprocessors>

	<mssqlauditorpreprocessors name="Столбчатая диаграмма" id="DatabasesSize.ColumnDiagramm.ru" columns="100" rows="50;50" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="" id="DatabasesSize.ColumnDiagramm.ru" column="1" row="1" colspan="1" rowspan="1">
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
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetInstanceDatabasesSize' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
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
					<GraphicOverlap>
						true
					</GraphicOverlap>
				</LegendConfiguration>

				<GraphType>
					StackedColumn
				</GraphType>
			</GraphConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="" id="jDatabasesSize.ColumnDiagramm.ru" column="1" row="2" colspan="1" rowspan="1">
			<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
				<xsl:output method="html" encoding="utf-8" indent="yes" />

				<xsl:template match="@*|node()">
					<xsl:copy>
						<xsl:apply-templates select="@*|node()"/>
					</xsl:copy>
				</xsl:template>

				<xsl:template match="/MSSQLResults/MSSQLResult[@name='GetInstanceDatabasesSize' and @hierarchy='']/RecordSet[@id='1']">
					<xsl:text disable-output-escaping='yes'>&lt;!doctype html&gt;</xsl:text>

					<html>
						<head>
							<meta content="text/html;charset=utf-8" http-equiv="Content-Type"/>
							<meta content="utf-8"                   http-equiv="encoding"/>

							<script type="text/javascript" src="$JS_FOLDER$/excanvas.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/jquery.jqplot.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.barRenderer.js"></script>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.pieRenderer.js"></script>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.categoryAxisRenderer.js"></script>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.canvasTextRenderer.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.canvasAxisLabelRenderer.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.dateAxisRenderer.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.enhancedLegendRenderer.min.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.highlighter.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.cursor.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.pointLabels.js"></script>

							<link rel="stylesheet" type="text/css" href="$JS_FOLDER$/jqplot/jquery.jqplot.css" />
						</head>
						<body>
							<style type="text/css">
								html, body
								{
									height:   100%;
									overflow: hidden;
								}

								#container
								{
									width:  100%;
									height: 100%;
								}
							</style>

							<div id="container">
								<div id="chart" style="height:100%; width:100%"/>
							</div>

							<script type="text/javascript">
								var chartData = [],

								labels = [
									'DatabaseDataSizeMB',
									'DatabaseLogSizeMB',
								];

								<xsl:for-each select="//MSSQLResult[@name='GetInstanceDatabasesSize']/RecordSet/Row">
									chartData.push({
										name: '<xsl:value-of select="DatabaseName"/>',

										values: [
											<xsl:value-of select="DatabaseDataSizeMB"/>,
											<xsl:value-of select="DatabaseLogSizeMB"/>,
										]
									});
								</xsl:for-each>

								<xsl:text disable-output-escaping="yes">
									$(function(){
										stackedColumnBar.draw();
										$(window).resize($.proxy(stackedColumnBar.resize, stackedColumnBar));
									});

									var stackedColumnBar = (function ($, data) {

									/**
									 *
									 * @param {String} name
									 * @param {String} container
									 * @param {Object} data
									 * @returns {stacked-columns_L1.StackedColumnBar}
									 */
									var StackedColumnBar = function (name, container, data, labels) {
										this.chartName = name;
										this.data      = data;
										this.labels    = labels;

										this.$target    = $('#' + this.chartName);
										this.$container = $('#' + container);

										this.plot = null;

									};

									$.extend(StackedColumnBar.prototype, {
										/**
										 *
										 * @returns {undefined}
										 */
										draw: function () {
											var charts = this.getCharts();

											/**
											 * jqPlot settings
											 *
											 * read more
											 * @link http://www.jqplot.com/examples/barTest.php
											 */
											this.plot = $.jqplot(this.chartName, charts, {
												stackSeries: true,
												//
												// set "default options" for all graph series
												//
												seriesDefaults: {
													renderer: $.jqplot.BarRenderer,

													rendererOptions: {
													},

													pointLabels: {
														show: true
													}
												},

												legend: {
													show: true, // whether to show legend
													location: 'n', // compass direction, nw, n, ne, e, se, s, sw, w
													xoffset: 12, // pixel offset of the legend box from the x (or x2) axis.
													yoffset: 12, // pixel offset of the legend box from the y (or y2) axis.

													renderer: $.jqplot.EnhancedLegendRenderer,

													rendererOptions: {
														numberRows: 1
													}
												},

												series: this.getLabels(),

												axes: {
													xaxis: {
														renderer: $.jqplot.CategoryAxisRenderer,
														ticks: this.getTicks()
													}
												},

												highlighter: {
													sizeAdjust: 7.5
												}
											});

											this.resize();
										},

										getLabels: function () {
											var series = [];

											for (var i = 0; i &lt; this.labels.length; i++) {
												series.push({
													label: this.labels[i]
												});
											}

											return series;
										},

										getTicks: function () {
											var ticks = [];

											for (var i = 0; i &lt; this.data.length; i++) {
												ticks.push(this.data[i].name);
											}

											return ticks;
										},

										/**
										 * Resize plot
										 *
										 * @returns {undefined}
										 */
										resize: function () {
											this.$target.height(this.$container.height() * 0.9);

											if (this.plot) {
												this.plot.replot({
													resetAxes: true
												});
											}

										},
										/**
										 * Get charts data
										 * Extract chart data values
										 *
										 * @returns {Array}
										 */
										getCharts: function () {
											var charts = [];

											for (var i = 0; i &lt;  this.data.length; i++) {
												for (var k = 0; k &lt;  data[i].values.length; k++) {
													charts[k] = charts[k] || [];
													charts[k].push(data[i].values[k]);
												}

											}

											return charts;
										}
									});

									return new StackedColumnBar('chart', 'container', data, labels);

									})($, chartData);
								</xsl:text>
							</script>
						</body>
					</html>
				</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>

	<mssqlauditorpreprocessors name="Круговая диаграмма" id="DatabasesSize.PieDiagramm.ru" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Круговая диаграмма" id="DatabasesSize.PieDiagramm.ru" column="1" row="1" colspan="1" rowspan="1">
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
				<PathToItems>/MSSQLResults/MSSQLResult[@name='GetInstanceDatabasesSize' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
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
