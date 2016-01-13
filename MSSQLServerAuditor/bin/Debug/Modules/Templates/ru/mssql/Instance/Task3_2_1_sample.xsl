<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Диаграмма для задачи #21" id="Ticket21Diagram.Graph.ru" columns="100" rows="50;50" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="" id="Ticket21Diagram.Graph.ru" column="1" row="1" colspan="1" rowspan="1">
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
						#,###
					</Format>
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='test' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<DateTag>JobRunDateTime</DateTag>
					<NameTag>JobName</NameTag>
					<ValueTag>JobDurationInSeconds</ValueTag>
				</GraphSource>

				<ItemsConfiguration>
					<NameSortType>
						NameAscending
					</NameSortType>
					<Unit>
						Minute
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
					NameDateStackedColumn
				</GraphType>

			</GraphConfiguration>
		</mssqlauditorpreprocessor>

		<mssqlauditorpreprocessor preprocessor="HtmlPreprocessorDialog" name="" id="jTicket21Diagram.Graph.ru" column="1" row="2" colspan="1" rowspan="1">
			<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
				<xsl:output method="html" encoding="utf-8" indent="yes" />

				<xsl:template match="@*|node()">
					<xsl:copy>
						<xsl:apply-templates select="@*|node()"/>
					</xsl:copy>
				</xsl:template>

				<xsl:template match="/MSSQLResults/MSSQLResult[@name='test' and @hierarchy='']/RecordSet[@id='1']">
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
									'JobDurationInSeconds'
								];

								<xsl:for-each select="//MSSQLResult[@name='test']/RecordSet/Row">
									chartData.push({
										name: '<xsl:value-of select="JobName"/>',

										values: [
											<xsl:value-of select="JobDurationInSeconds"/>
										]
									});
								</xsl:for-each>

								<xsl:text disable-output-escaping="yes">
									$(function()
									{
										stackedColumnBar.draw();

										$(window).resize($.proxy(stackedColumnBar.resize, stackedColumnBar));
									});

									var stackedColumnBar = (function ($, data)
									{
										/**
										 *
										 * @param {String} name
										 * @param {String} container
										 * @param {Object} data
										 * @returns {stacked-columns_L1.StackedColumnBar}
										 */
										var StackedColumnBar = function (name, container, data, labels)
										{
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
												this.plot = $.jqplot(this.chartName, charts,
												{
													//
													// "axes" options
													//
													axes:
													{
														xaxis:
														{
															renderer: $.jqplot.CategoryAxisRenderer,

															tickOptions:
															{
															},

															ticks: this.getTicks()
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
													},

													//
													// set "default options" for all graph series
													//
													seriesDefaults:
													{
														lineWidth: 0.75,

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

														renderer: $.jqplot.BarRenderer,

														rendererOptions:
														{
															highlightMouseOver: false,
															highlightMouseDown: false,
															highlightColor:     'rgb(0, 0, 0)',
															smooth:             true,
															showDataLabels:     true
														},

														pointLabels:
														{
															show: true
														}
													},

													//
													// "Cursor"
													// Options are passed to the cursor plugin through the "cursor" object at the top
													// level of the options object.
													//
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

													//
													// Series options are specified as an array of objects, one object
													// for each series.
													//
													series: this.getLabels(),

													stackSeries: true,

													highlighter:
													{
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
												this.$target.height(this.$container.height() * 0.98);

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
</root>
