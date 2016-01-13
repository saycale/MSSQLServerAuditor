<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Диаграмма Ганта" id="Gantt.Graph.ru" columns="100" rows="34;33;33" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessor" name="" id="Gantt.Graph.ru" column="1" row="1" colspan="1" rowspan="1">
			<GraphConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
				<!--This tag is new added. It applies to "Interval" property of .net Charting.Axis object. Zero means the value will be adjusted automaticaly.
					If this tag is omited the property value will be assigned to 1.0 (as was implemented in SQLAuditor charting subsystem) -->
				<Interval>0.0</Interval>

				<FirstWeekDay>
					Monday
				</FirstWeekDay>

				<AxisXConfiguration>
					<Interval>0.0</Interval>
					<ShowLabels>
						true
					</ShowLabels>
					<AxisXLabelFilter>
						All
					</AxisXLabelFilter>
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
							true
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
						<Enabled>
							true
						</Enabled>
						<LineWidth>
							1
						</LineWidth>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MajorGrid>
					<MinorGrid>
						<Enabled>
							true
						</Enabled>
						<LineWidth>
							1
						</LineWidth>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MinorGrid>
					<Format>
					</Format>
					<Name>
					</Name>
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetGanttGraph' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<RecordSetNumber>1</RecordSetNumber>
					<ItemTag>Row</ItemTag>
					<NameTag>LineName</NameTag>
					<DateTagBegin>StartDateTime</DateTagBegin>
					<DateTagEnd>EndDateTime</DateTagEnd>
				</GraphSource>

				<ItemsConfiguration>
					<NameSortType>
						NameAscending
					</NameSortType>
					<Unit>
						Hour
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
					GanttDiagram
				</GraphType>

			</GraphConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="" id="jD3.Gantt.Graph.ru" column="1" row="2" colspan="1" rowspan="1">
			<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
				<xsl:output method="html" encoding="utf-8" indent="yes" />

				<xsl:key name="LineName" match="Row" use="LineName" />

				<xsl:template match="@*|node()">
					<xsl:copy>
						<xsl:apply-templates select="@*|node()"/>
					</xsl:copy>
				</xsl:template>

				<xsl:template name="formatDate">
					<xsl:param name="date"/>
					<xsl:value-of select="concat(substring-before($date, 'T'), ' ', substring-after($date, 'T'))"/>
				</xsl:template>

				<xsl:template match="/MSSQLResults/MSSQLResult[@name='GetGanttGraph' and @hierarchy='']/RecordSet[@id='1']">
					<xsl:text disable-output-escaping='yes'>&lt;!doctype html&gt;</xsl:text>

					<html>
						<head>
							<meta content="text/html;charset=utf-8" http-equiv="Content-Type"/>
							<meta content="utf-8"                   http-equiv="encoding"/>

							<script type="text/javascript" src="$JS_FOLDER$/excanvas.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/d3/d3.min.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/d3/gantt-chart-d3.js"/>

							<link rel='stylesheet' type='text/css' href='js/d3/d3.css' />
						</head>
						<body>
							<style>
								html,body {
										width:  100%;
										height: 100%;
										margin: 0px;
									}

									.chart {
										width:       100%;
										font-family: Arial, sans-serif;
										font-size:   11px;
									}

									.axis path,

									.axis line {
										fill:            none;
										stroke:          #000;
										shape-rendering: crispEdges;
									}

									.bar {
										fill: #33b5e5;
									}

									.bar-green {
										fill: #a8f0a8;
									}

									.bar-grey {
										fill: #d0d0d0;
									}

									.bar-red {
										fill: #e14a4a;
									}

									.bar-yellow {
										fill: #e1d34a;
									}
							</style>

							<script type="text/javascript">
								var chartData = [], taskNames = [];

								<xsl:for-each select="//MSSQLResult[@name='GetGanttGraph']/RecordSet/Row">
									chartData.push({
										startDate: new Date(
											<!-- '<xsl:call-template name="formatDate">
												<xsl:with-param name="date" select="StartDateTime"/>
											</xsl:call-template>', -->

											'<xsl:value-of select="StartDateTime"/>'
										),

										endDate: new Date(

											<!-- '<xsl:call-template name="formatDate">
												<xsl:with-param name="date" select="EndDateTime"/>
											</xsl:call-template>', -->

											'<xsl:value-of select="EndDateTime"/>'
										),

										taskName: '<xsl:value-of select="LineName"/>',

										status: '<xsl:value-of select="LineName"/>'
									});

									if($.inArray('<xsl:value-of select="LineName"/>', taskNames) === -1){
										taskNames.push('<xsl:value-of select="LineName"/>')
									}
								</xsl:for-each>

								(function (window, $, d3, taskNames, chartData) {
									var taskColors = {}, colors

									colors = ['bar-green', 'bar-grey', 'bar-red', 'bar-yellow'];

									for (var i = 0; i &lt; taskNames.length; i++) {
										taskColors[taskNames[i]] = colors[0];

										colors.push(colors.shift());
									}

									format = '%m/%d/%Y';

									var gantt = d3.gantt().taskTypes(taskNames).taskStatus(taskColors).tickFormat(format);

									gantt(chartData);

									$(window).resize(function () {
										$('svg.chart').remove();
										gantt(chartData);
									});
								})(window, $, d3, taskNames, chartData);
							</script>
						</body>
					</html>
				</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="" id="jqPlot.Gantt.Graph.ru" column="1" row="3" colspan="1" rowspan="1">
			<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
				<xsl:output method="html" encoding="utf-8" indent="yes" />

				<xsl:key name="LineName" match="RecordSet/Row" use="LineName" />

				<xsl:template match="@*|node()">
					<xsl:copy>
						<xsl:apply-templates select="@*|node()"/>
					</xsl:copy>
				</xsl:template>

				<xsl:template name="formatDate">
					<xsl:param name="date"/>
					<xsl:value-of select="concat(substring-before($date, 'T'), ' ', substring-after($date,'T'))"/>
				</xsl:template>

				<xsl:template match="/MSSQLResults/MSSQLResult[@name='GetGanttGraph' and @hierarchy='']/RecordSet[@id='1']">
					<xsl:text disable-output-escaping='yes'>&lt;!doctype html&gt;</xsl:text>

					<xsl:variable name="GanttName" select="concat('Gantt', RecordSet/@id)" />

					<html>
						<head>
							<meta content="text/html;charset=utf-8" http-equiv="Content-Type"/>
							<meta content="utf-8"                   http-equiv="encoding"/>

							<script type="text/javascript" src="$JS_FOLDER$/excanvas.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/jquery.jqplot.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.dateAxisRenderer.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.canvasTextRenderer.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.enhancedLegendRenderer.js"/>

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

								<xsl:apply-templates select="//Row" mode="ganttCss" />

								#chart td.jqplot-table-legend-hide {
									display: none;
								}
							</style>

							<div id="container">
								<div id="chart" style="height:100%; width:100%"/>
							</div>

							<script type="text/javascript">
								$(document).ready(function()
								{
									var color_1 = 'rgb(5,   100, 146)';
									var color_2 = 'rgb(224,  64,  10)';
									var color_3 = 'rgb(252, 180,  65)';
									var color_4 = 'rgb(65,  140, 240)';

									var ganttData1 = [
										<xsl:apply-templates select="//ancestor::RecordSet/Row" mode="gantt" />
									];

									var seriesConf = [
										<xsl:apply-templates select="//ancestor::RecordSet/Row" mode="ganttConf" />
									];

									plot = $.jqplot('chart', ganttData1, {
										series: seriesConf,

										seriesColors: ["#888888"],

										seriesDefaults: {
											showMarker: false
										},

										legend:
										{
											show:     true,
											location: 'n',
											renderer: $.jqplot.EnhancedLegendRenderer,

											rendererOptions: {
												numberRows:   1,
												seriesToggle: false
											},

											placement: 'inside'
										},

										axes:
										{
											xaxis:
											{
												renderer: $.jqplot.DateAxisRenderer,

												tickOptions:
												{
												},

												pad: 0
											},

											yaxis: {
												min: <xsl:value-of select="1 + count(
													//ancestor::RecordSet/Row
													[generate-id(.) = generate-id(key('LineName', LineName)[1])]
												)" />,

												max: 0,

												tickInterval: 1,

												tickOptions: {
													show:         false,
													showLabel:    false,
													formatString: '%i'
												}
											}
										},

										grid:
										{
											background:    '#ffffff',
											drawGridlines: true,
											drawBorder:    true,
											shadow:        false,
											shadowAngle:   -90
										}
									});

									plot.replot (
										{
											resetAxes: false
										}
									);

									$(window).resize(function()
									{
										$('#chart').height($('#container').height() * 0.96);

										plot.replot (
											{
												resetAxes: false
											}
										);
									});
								});
							</script>
						</body>
					</html>
				</xsl:template>

				<xsl:template match="//Row" mode="gantt">
					[
						[
							new Date(
								'<xsl:value-of select="StartDateTime" />Z'
							).getTime(),

							<xsl:call-template name="jobOrder" />
						],

						[
							new Date(
								'<xsl:value-of select="EndDateTime" />Z'
							).getTime(),

							<xsl:call-template name="jobOrder" />
						]
					]

					<xsl:if test="position() != last()">,</xsl:if>
				</xsl:template>

				<xsl:template match="//Row" mode="ganttConf">
					{
						color:     color_<xsl:call-template name="jobOrder" />,
						label:     '<xsl:value-of select="LineName" />',
						lineWidth: 24
					}

					<xsl:if test="position() != last()">,</xsl:if>
				</xsl:template>

				<xsl:template match="//Row" mode="ganttLegend">
					<xsl:text>'</xsl:text>
						<xsl:value-of select="LineName" />
					<xsl:text>'</xsl:text>

					<xsl:if test="position() != last()">,</xsl:if>
				</xsl:template>

				<xsl:template match="//Row" mode="ganttCss">
					<xsl:if test="LineName[text() = ../preceding-sibling::Row/LineName/text()]">
						<xsl:text>#chart td.jqplot-table-legend:nth-of-type(</xsl:text>
						<xsl:value-of select="position() * 2 - 1" />
						<xsl:text>),&#xa;</xsl:text>
						<xsl:text></xsl:text>
						<xsl:text>#chart td.jqplot-table-legend:nth-of-type(
							</xsl:text>
							<xsl:value-of select="position() * 2" />
							<xsl:text>
						),&#xa;</xsl:text>
						<xsl:text></xsl:text>
					</xsl:if>
				</xsl:template>

				<xsl:template name="jobOrder">
					<xsl:variable name="firstRowWithJobName" select="ancestor::RecordSet
						/Row[LineName/text() = current()/LineName/text()][1]"/>
					<xsl:value-of select="1 +
						count($firstRowWithJobName/preceding-sibling::Row[not(LineName = preceding-sibling::Row/LineName)])
					" />
				</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
