<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Круговая диаграмма" id="PieGraph.HTML.ru" columns="100" rows="50;50" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="" id="PieGraph.Graph.ru" column="1" row="1" colspan="1" rowspan="1">
			<GraphConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
				<FirstWeekDay>
					Monday
				</FirstWeekDay>

				<AxisXConfiguration>
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
						#.##
					</Format>
					<Name>
						%
					</Name>
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetPieGraph' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<DateTag>EventTime</DateTag>
					<NameTag>LineName</NameTag>
					<ValueTag>PieValue</ValueTag>
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
						true
					</GraphicOverlap>
				</LegendConfiguration>

				<GraphType>
					Pie
				</GraphType>

			</GraphConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="" id="jPie.Graph.ru" column="1" row="2" colspan="1" rowspan="1">
			<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
				<xsl:output method="html" encoding="utf-8" indent="yes" />

				<xsl:template match="@*|node()">
					<xsl:copy>
						<xsl:apply-templates select="@*|node()"/>
					</xsl:copy>
				</xsl:template>

				<xsl:template name="formatDate">
					<xsl:param name="date"/>
					<xsl:value-of select="concat(substring-before($date, 'T'), ' ', substring-after($date, 'T'))"/>
				</xsl:template>

				<xsl:template match="/MSSQLResults/MSSQLResult[@name='GetPieGraph' and @hierarchy='']/RecordSet[@id='1']">
					<xsl:text disable-output-escaping='yes'>&lt;!doctype html&gt;</xsl:text>

					<xsl:variable name="PieName" select="concat('Pie', RecordSet/@id)" />

					<html>
						<head>
							<meta content="text/html;charset=utf-8" http-equiv="Content-Type"/>
							<meta content="utf-8"                   http-equiv="encoding"/>

							<script type="text/javascript" src="$JS_FOLDER$/excanvas.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/jquery.jqplot.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.pieRenderer.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.enhancedLegendRenderer.min.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.highlighter.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.cursor.js"/>

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
								$(document).ready(function()
								{
									var pieData1 = [<xsl:apply-templates select="//Row" mode="pie" />
									];

									$('#chart').height($('#container').height() * 0.96);

									var plot = $.jqplot('chart', [pieData1], {
										grid: {
											drawBorder:    false,
											drawGridlines: false,
											background:    '#ffffff',
											shadow:        false
										},

										axesDefaults: {
										},

										seriesDefaults:{
											renderer: $.jqplot.PieRenderer,

											rendererOptions: {
												showDataLabels: true
											}
										},

										//
										// "legend" options
										//
										legend:
										{
											show:     true, // whether to show legend
											location: 'n',  // compass direction, nw, n, ne, e, se, s, sw, w
											xoffset:  12,   // pixel offset of the legend box from the x (or x2) axis.
											yoffset:  12,   // pixel offset of the legend box from the y (or y2) axis.

											renderer: $.jqplot.EnhancedLegendRenderer,

											rendererOptions:
											{
												numberRows: 1
											}
										}
									});

									plot.replot (
										{
											resetAxes: true
										}
									);

									$(window).resize(function()
									{
										$('#chart').height($('#container').height() * 0.96);

										plot.replot (
											{
												resetAxes: true
											}
										);
									});
								});
							</script>
						</body>
					</html>
				</xsl:template>

				<xsl:template match="//Row" mode="pie">
					[
						'<xsl:value-of select="LineName" />',

						<xsl:value-of select="PieValue" />
					]

					<xsl:if test="position() != last()">,</xsl:if>
				</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
