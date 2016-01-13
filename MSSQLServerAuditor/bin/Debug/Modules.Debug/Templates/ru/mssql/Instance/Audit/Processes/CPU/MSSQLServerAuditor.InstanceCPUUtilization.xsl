<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Использование процессорного времени" id="InstanceCPUUtilization.HTML.ru" columns="100" rows="50;50" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="" id="InstanceCPUUtilization.Graph.ru" column="1" row="1" colspan="1" rowspan="1">
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
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetInstanceCPUUtilization' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<DateTag>EventTime</DateTag>
					<NameTag></NameTag>
					<ValueTag>SQLProcessCPUUtilization</ValueTag>
					<ValueTag>OtherProcessesCPUUtilization</ValueTag>
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
					Line
				</GraphType>

			</GraphConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="" id="jInstanceCPUUtilization.Graph.ru" column="1" row="2" colspan="1" rowspan="1">
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

				<xsl:template match="/MSSQLResults/MSSQLResult[@name='GetInstanceCPUUtilization' and @hierarchy='']/RecordSet[@id='1']">
					<xsl:text disable-output-escaping='yes'>&lt;!doctype html&gt;</xsl:text>

					<html>
						<head>
							<meta content="text/html;charset=utf-8" http-equiv="Content-Type"/>
							<meta content="utf-8"                   http-equiv="encoding"/>

							<script type="text/javascript" src="$JS_FOLDER$/excanvas.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jquery-1.11.1.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/jquery.jqplot.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.canvasTextRenderer.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.canvasAxisLabelRenderer.js"/>
							<script type="text/javascript" src="$JS_FOLDER$/jqplot/plugins/jqplot.dateAxisRenderer.js"/>
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
									var color_1 = 'rgb(  5, 100, 146)';
									var color_2 = 'rgb(224,  64,  10)';
									var color_3 = 'rgb(252, 180,  65)';
									var color_4 = 'rgb( 65, 140, 240)';

									var charts = [];

									<xsl:apply-templates select="//Row[SQLProcessCPUUtilization][1]/SQLProcessCPUUtilization"         mode="plots" />
									<xsl:apply-templates select="//Row[OtherProcessesCPUUtilization][1]/OtherProcessesCPUUtilization" mode="plots" />

									$('#chart').height($('#container').height() * 0.96);

									var plot = $.jqplot('chart', charts,
									{
										stackSeries: true,

										/**
										 *  "axes" options
										 */
										axes:
										{
											xaxis:
											{
												renderer: $.jqplot.DateAxisRenderer,

												tickOptions:
												{
												}
											}
										},

										/**
										 *  "legend" options
										 */
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
										},

										/**
										 * set "default options" for all graph series
										 */
										seriesDefaults:
										{
											lineWidth: 0.75,

											fill: true,

											markerOptions:
											{
												show:         false,          // whether to show data point markers
												style:        'filledCircle', // circle, diamond, square, filledCircle, filledDiamond or filledSquare.
												lineWidth:    2,              // width of the stroke drawing the marker.
												size:         9,              // size (diameter, edge length, etc.) of the marker.
												color:        '#666666',      // color of marker, set to color of line by default.
												shadow:       true,           // whether to draw shadow on marker or not.
												shadowAngle:  45,             // angle of the shadow.  Clockwise from x axis.
												shadowOffset: 1,              // offset from the line of the shadow,
												shadowDepth:  3,              // Number of strokes to make when drawing shadow. Each stroke offset by shadowOffset from the last.
												shadowAlpha:  0.07            // Opacity of the shadow
											},

											rendererOptions:
											{
												highlightMouseOver: false,
												highlightMouseDown: false,
												highlightColor:     'rgb(0, 0, 0)',
												smooth:             true
											}
										},

										/**
										 * "Cursor"
										 * Options are passed to the cursor plugin through the "cursor" object at the top
										 * level of the options object.
										 */
										cursor:
										{
											style:                   'crosshair', // A CSS spec for the cursor type to change the cursor to when over plot.
											show:                    true,        // whether to show cursor
											showTooltip:             true,        // show a tooltip showing cursor position.
											followMouse:             false,       // whether tooltip should follow the mouse or be stationary.
											tooltipLocation:         's',         // location of the tooltip either relative to the mouse (followMouse=true) or relative to the plot.  One of the compass directions, n, ne, e, se, etc.
											tooltipOffset:           6,           // pixel offset of the tooltip from the mouse or the axes.
											showTooltipGridPosition: false,       // show the grid pixel coordinates of the mouse in the tooltip.
											showTooltipUnitPosition: true,        // show the coordinates in data units of the mouse in the tooltip.
											tooltipFormatString:     '%.4P',      // sprintf style format string for tooltip values.
											useAxesFormatters:       true,        // whether to use the same formatter and formatStrings as used by the axes, or to use the formatString specified on the cursor with sprintf.
											tooltipAxesGroups:       [],          // show only specified axes groups in tooltip.  Would specify like: [['xaxis', 'yaxis'], ['xaxis', 'y2axis']].  By default, all axes combinations with for the series in the plot are shown.
											zoom:                    true
										},

										/**
										 * Series options are specified as an array of objects, one object
										 * for each series.
										 */
										series:
										[
											<xsl:apply-templates select="//Row[SQLProcessCPUUtilization][1]/SQLProcessCPUUtilization"         mode="lines" />,
											<xsl:apply-templates select="//Row[OtherProcessesCPUUtilization][1]/OtherProcessesCPUUtilization" mode="lines" />
										]
									}
									);

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

				<xsl:template match="//Row/SQLProcessCPUUtilization | //Row/OtherProcessesCPUUtilization" mode="plots">
					<xsl:variable name="LineName">
						<xsl:value-of select="local-name()" />
					</xsl:variable>

					<xsl:variable name="var_name">
						<xsl:value-of select="concat('chart_', $LineName)"/>
					</xsl:variable>

					var <xsl:value-of select="$var_name"/>=[];

					<xsl:for-each select="ancestor::RecordSet/Row/child::*[local-name() = $LineName]">
						<xsl:value-of select="$var_name"/>.push(
							[
								'<xsl:call-template name="formatDate">
									<xsl:with-param name="date" select="../EventTime"/>
								</xsl:call-template>',

								<xsl:value-of select="."/>
							]
						);
					</xsl:for-each>

					charts.push(<xsl:value-of select="$var_name"/>);
				</xsl:template>

				<xsl:template match="//Row/SQLProcessCPUUtilization | //Row/OtherProcessesCPUUtilization" mode="lines">
					<xsl:variable name="LineName">
						<xsl:value-of select="local-name()" />
					</xsl:variable>

					{
						label: '<xsl:value-of select="$LineName"/>',

						color: color_<xsl:choose>
							<xsl:when test="$LineName = 'SQLProcessCPUUtilization'">
								<xsl:value-of select="1"/>
							</xsl:when>
							<xsl:when test="$LineName = 'OtherProcessesCPUUtilization'">
								<xsl:value-of select="2"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="3"/>
							</xsl:otherwise>
						</xsl:choose>
					}
				</xsl:template>
			</xsl:stylesheet>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>
