using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using log4net;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Graph
{
	/// <summary>
	/// Graph control
	/// </summary>
	public partial class GraphControl : UserControl
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private const string LegendShow     = "legendShow";
		private const string MarginSettings = "marginSettings";
		private const string MenuAll        = "menuAll";
		private const string MenuHide       = "menuHide";
		private const string MenuHideAll    = "menuHideAll";
		private const string MenuRestore    = "menuRestore";
		private const string MenuSelect     = "menuSelect";

		private GraphTypeEnum      _graphType;
		private string             _id;
		private Color              _oldColor;
		private ChartDashStyle     _oldDashStyle;
		private int                _oldWidth;
		private string             _preprocessorTypeName;
		private DataPoint          _selectedPoint;
		private Series             _selectedSeries;
		private GraphStateSettings _stateSettings;
		private ToolTip            _toolTipMousePosition;

		/// <summary>
		/// Initializing object GraphControl.
		/// </summary>
		public GraphControl()
		{
			this._stateSettings        = new GraphStateSettings();
			this._toolTipMousePosition = new ToolTip();

			this._toolTipMousePosition.ShowAlways = false;

			this.InitializeComponent();
		}

		/// <summary>
		/// Save graph to image file
		/// </summary>
		/// <param name="fileName">Image filename</param>
		/// <param name="format">Image format</param>
		public void SaveImage(string fileName, ChartImageFormat format)
		{
			this.chart.SaveImage(fileName, format);
		}

		public Chart ChartInstance
		{
			get { return this.chart; }
		}

		/// <summary>
		/// Load configuration from XML file and draw graph
		/// </summary>
		/// <param name="fileName">XML configuration filename</param>
		/// <param name="ownerSize">Owner size.</param>
		/// <param name="context">Configuration context.</param>
		public void SetConfiguration(string fileName, Size ownerSize, object context)
		{
			this.SetConfiguration(GraphConfiguration.LoadFromXml(fileName), ownerSize, context);
		}

		/// <summary>
		/// Load configuration from XML in stream and draw graph
		/// </summary>
		/// <param name="stream">Xml configuration stream</param>
		/// <param name="ownerSize">Owner size.</param>
		/// <param name="context">Configuration context.</param>
		public void SetConfiguration(Stream stream, Size ownerSize, object context)
		{
			this.SetConfiguration(GraphConfiguration.LoadFromXml(stream), ownerSize, context);
		}

		/// <summary>
		/// Draw graph according passed configuration
		/// </summary>
		/// <param name="configuration">Graph configuration</param>
		/// <param name="ownerSize">Owner size.</param>
		/// <param name="context">Configuration context.</param>
		public void SetConfiguration(GraphConfiguration configuration, Size ownerSize, object context)
		{
			this.chart.Series.Clear();

			this.SetupGraph(configuration);

			switch (configuration.GraphType.Value)
			{
				case GraphTypeEnum.NameDateStackedColumn:
				case GraphTypeEnum.NameDateLine:
					this.DrawNameDateGraph(new GraphData(configuration, context), configuration);
					break;

				case GraphTypeEnum.NamedLine:
					this.DrawNamedGraph(new GraphData(configuration, context), configuration);
					break;

				case GraphTypeEnum.GanttDiagram:
					this.DrawGantt(new GanttGraphData(configuration, context), configuration);
					break;

				default:
					this.DrawSimpleGraph(new GraphData(configuration, context), configuration);
					break;
			}

			foreach (Series series in this.chart.Series)
			{
				GraphSeriesSettings seriesState = this._stateSettings.GetSeriesSettings(series.Name);

				series.Enabled = seriesState.Visible;

				if (!seriesState.Visible && configuration.GraphType.Value == GraphTypeEnum.GanttDiagram)
				{
					this.RevalueAxisY(series);
				}
			}

			if (configuration.GraphType == GraphTypeEnum.NameDateStackedColumn ||
				configuration.GraphType == GraphTypeEnum.NameDateLine ||
				configuration.GraphType == GraphTypeEnum.NamedLine
			)
			{
				this.HideUnnecessaryXAxisZeroLabel();
			}
		}

		/// <summary>
		/// Get configuration from string and draw graph.
		/// </summary>
		/// <param name="text">XML configuration represented as string.</param>
		/// <param name="ownerSize">Owner size.</param>
		/// <param name="context">Configuration context.</param>
		public void SetConfigurationFromText(string text, Size ownerSize, object context)
		{
			using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text)))
			{
				stream.Seek(0, SeekOrigin.Begin);

				this.SetConfiguration(
					GraphConfiguration.LoadFromXml(stream),
					ownerSize,
					context
				);
			}
		}

		internal void SetId(string id, string preprocessorTypeName)
		{
			this._id                   = "G." + id;
			this._preprocessorTypeName = preprocessorTypeName;

			this._stateSettings = Program.Model.LayoutSettings.GetExtendedSettings<GraphStateSettings>(
				this._id,
				preprocessorTypeName,
				Program.Model.Settings.ReportLanguage
			);

			if (this._stateSettings == null)
			{
				this._stateSettings = new GraphStateSettings();
			}

			this.chart.Legends[0].Enabled = this._stateSettings.LegendEnabled;

			// set background color for the legend as transparent
			this.chart.Legends[0].BackColor = Color.Transparent;
		}

		private static void chart_FormatNumber(object sender, FormatNumberEventArgs e)
		{
			if (e.ElementType == ChartElementType.AxisLabels && e.ValueType == ChartValueType.DateTime)
			{
				var value = DateTime.FromOADate(e.Value);

				if (value == value.Date)
				{
					e.LocalizedValue = value.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
				}
				else
				{
					if (value.Second != 0 || Math.Abs(value.Millisecond - 0.0) > 1e-12)
					{
						e.LocalizedValue = value.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern);
					}
					else
					{
						e.LocalizedValue = value.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);
					}
				}
			}
		}

		private static string EnsureNameIsValue(string strDesired, SeriesCollection existing)
		{
			int    intSeriaNumber = 0;
			string strName        = strDesired;

			while (existing.Any(series => series.Name == strName))
			{
				intSeriaNumber++;

				strName = string.Format(
					"{0}_{1}",
					strDesired,
					intSeriaNumber
				);
			}

			return strName;
		}

		private static void SetupAxis(Axis axis, AxisConfiguration configuration)
		{
			SetupGrid(axis.MajorGrid, configuration.MajorGrid);
			SetupGrid(axis.MinorGrid, configuration.MinorGrid);

			axis.LabelStyle.Enabled = configuration.ShowLabels ?? false;

			if (axis.LabelStyle.Enabled)
			{
				axis.Enabled        = AxisEnabled.True;
				axis.Title          = configuration.AxisTitleName ?? String.Empty;
				axis.TitleAlignment = StringAlignment.Far;
			}
			else
			{
				axis.Enabled        = AxisEnabled.False;
			}
		}

		private static void SetupAxisX(Axis axis, GraphConfiguration configuration)
		{
			SetupAxis(axis, configuration.AxisXConfiguration);

			axis.Interval = configuration.AxisXConfiguration.Interval ?? 1;

			switch (configuration.AxisXConfiguration.AxisXLabelFilter.Value)
			{
				case AxisXLabelFilter.All:
					break;

				case AxisXLabelFilter.AllDays:
					axis.IntervalType = DateTimeIntervalType.Days;
					break;

				case AxisXLabelFilter.StartOfMonth:
					axis.IntervalType = DateTimeIntervalType.Months;
					break;

				case AxisXLabelFilter.StartOfWeek:
					axis.IntervalType       = DateTimeIntervalType.Weeks;
					axis.IntervalOffsetType = DateTimeIntervalType.Days;
					axis.IntervalOffset     = configuration.FirstWeekDay - DayOfWeek.Sunday;
					break;

				default:
					throw new ArgumentException(
						"Invalid AxisXLabelFilter value",
						configuration.AxisXConfiguration.AxisXLabelFilter.ToString()
					);
			}
		}

		private void SetupAxisY(AxisYConfiguration configuration)
		{
			// log.DebugFormat("SetupAxisY: configuration.Format: '{0}'", configuration.Format);

			SetupAxis(this.chart.ChartAreas[0].AxisY, configuration);

			this.chart.ChartAreas[0].AxisY.LabelStyle.Format = configuration.Format;
			this.chart.ChartAreas[0].AxisY.LabelStyle.Angle  = -90;

			// this.chart.ChartAreas[0].AxisY.IsLabelAutoFit    = true;
			// this.chart.ChartAreas[0].AxisY.LabelAutoFitStyle = this.chart.ChartAreas[0].AxisY.LabelAutoFitStyle & (~LabelAutoFitStyles.WordWrap) & LabelAutoFitStyles.DecreaseFont;
		}

		private static void SetupGrid(Grid grid, GraphGridConfiguration configuration)
		{
			grid.Enabled   = configuration.Enabled;
			grid.LineWidth = configuration.LineWidth;
			grid.LineColor = configuration.LineColor;
		}

		private void chart_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this._graphType == GraphTypeEnum.Pie)
			{
			}
			else
			{
				this.DoubleClickDefault(e);
			}
		}

		private void ChartMouseDown(object sender, MouseEventArgs e)
		{
			if (this._graphType == GraphTypeEnum.Pie)
			{
				this.MouseDownPie(e);
			}
			else
			{
				this.MouseDownDefault(e);
			}
		}

		private void DoubleClickDefault(MouseEventArgs e)
		{
			if (this.chart.Series.Count > 1)
			{
				var hitTest = this.chart.HitTest(e.X, e.Y);

				if (hitTest.ChartElementType == ChartElementType.LegendItem)
				{
					this.SetSeriesEnabled(hitTest.Series, !hitTest.Series.Enabled);

					hitTest.Series.IsVisibleInLegend = true;
				}
			}
		}

		private string GetLocalizedText(string name)
		{
			if (Program.Model != null)
			{
				if (Program.Model.LocaleManager != null)
				{
					return Program.Model.LocaleManager.GetLocalizedText(GetType().Name, name);
				}
			}

			return name;
		}

		private void HideUnnecessaryXAxisZeroLabel()
		{
			// Looks like it's not needed any more.
			// Perhapse there was a sort of "feature" in Chart class and a trick with assigning 1e-12
			// to AxisX.Minimum worked as a workaround.
			// Now now zero label apperas for X axis in case of ticket #21

			// Ticket #21
			// Otherwise, in case of points with same X value, zero X label appears.
			// It's looks especcialy strange when X axis is of DateTime type.

			//actually .Minimum is zero exactly. This conditions was added for #188
			if (
				Math.Abs(this.chart.ChartAreas[0].AxisX.Minimum) < double.Epsilon &&
				chart.Series.Where(s => s.Enabled).SelectMany(s => s.Points.Select(pt => pt.XValue)).Distinct().Count() == 1
			)
			{
				this.chart.ChartAreas[0].AxisX.Minimum = 1e-12;
			}
		}

		#region Draw graphics

		private static void DrawGantt(Chart myChart, IEnumerable<GanttJobItem> jobs)
		{
			int i       = 0;
			var jobList = jobs as IList<GanttJobItem> ?? jobs.ToList();

			if (jobList.Count == 0)
			{
				return;
			}

			var area = myChart.ChartAreas[0];

			if (string.IsNullOrWhiteSpace(area.Name))
			{
				area.Name = "Main";
			}

			area.AxisX.MajorGrid.Enabled = true;

			var dates = jobList.SelectMany(j => new[] {j.Start, j.End}).ToList();

			area.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

			var min = dates.Min();
			var max = dates.Max();

			area.AxisY.Minimum = min.ToOADate();
			area.AxisY.Maximum = max.ToOADate();

			var namedGroups = jobList.GroupBy(j => j.Name).ToList();

			i = 0;

			foreach (var namedGroup in namedGroups)
			{
				var series = new Series
				{
					Name             = EnsureNameIsValue(namedGroup.Key, myChart.Series),
					ChartType        = SeriesChartType.RangeBar,
					ChartArea        = area.Name,
					YValuesPerPoint  = 2,
					CustomProperties = "DrawSideBySide=false"
				};

				myChart.Series.Add(series);

				foreach (var job in namedGroup)
				{
					var ptIndex = series.Points.AddXY(namedGroups.Count - i, job.Start, job.End);
					var pt      = series.Points[ptIndex];

					pt.AxisLabel = job.Name;
					//this will place text label right inside the colored bar
					//pt.Label = job.Name;
				}

				i++;
			}
		}

		private void DrawGantt(GanttGraphData data, GraphConfiguration configuration)
		{
			this.chart.Series.Clear();

			DrawGantt(this.chart, data.Jobs);
		}

		private void DrawNameDateGraph(GraphData data, GraphConfiguration configuration)
		{
			int  i           = 0;
			int  countPoints = -1;
			bool flag        = false;
			var  sortGroup   = ((XmlFileGraphSource) configuration.GraphSource).ValueGroup;

			foreach (string name in data.Names)
			{
				var series = new Series(name) { IsXValueIndexed = true };

				SetupSeries(series, configuration, i);

				switch (configuration.GraphType.Value)
				{
					case GraphTypeEnum.NameDateStackedColumn:
						series.ChartType = SeriesChartType.StackedColumn;
						break;

					case GraphTypeEnum.NameDateLine:
						series.ChartType = SeriesChartType.Line;
						break;
				}

				this.chart.Series.Add(series);

				switch (sortGroup)
				{
					case ValGroup.Day:
					case ValGroup.Week:
					case ValGroup.Month:
					default: MonthWeekDaySort(data, configuration, name, series, sortGroup);
						break;

					case ValGroup.Hour: HourSort(data, configuration, sortGroup, name, series);
						break;

					case ValGroup.Minute:
					case ValGroup.Second:
						MinuteSecondSort(data, configuration, sortGroup, name, series);
						break;
				}

				countPoints = countPoints == -1 ? series.Points.Count : -1;

				flag = flag || (countPoints != series.Points.Count);

				i++;
			}

			if (flag)
			{
				foreach (var chartItems in this.chart.Series)
				{
					chartItems.IsXValueIndexed = false;
				}
			}
		}

		private void DrawNamedGraph(GraphData data, GraphConfiguration configuration)
		{
			int      seriesIndex = 0;
			DateTime minDate     = DateTime.MaxValue;
			DateTime maxDate     = DateTime.MinValue;
			bool     setupAxis   = false;

			foreach (string name in data.Names)
			{
				Series series = new Series(name)
				{
					IsXValueIndexed = true
				};

				SetupSeries(series, configuration, seriesIndex++);

				series.ChartType = SeriesChartType.Line;

				this.chart.Series.Add(series);

				foreach (KeyValuePair<string, List<PlainItem>> items in data.PlainData)
				{
					List<PlainItem> namedItems = items.Value.Where(el => el.Name == name).ToList();

					if (namedItems.Any())
					{
						List<DateTime> d                   = namedItems.SelectMany(it => new[] { it.DateTime }).ToList();
						DateTime       minCurrentNamedItem = d.Min();
						DateTime       maxCurrentNamedItem = d.Max();

						// minDate = new DateTime(Math.Min(minCurrentNamedItem.Ticks, minDate.Ticks));
						// maxDate = new DateTime(Math.Max(maxCurrentNamedItem.Ticks, maxDate.Ticks));

						if (minDate > minCurrentNamedItem)
						{
							minDate = minCurrentNamedItem;
						}

						if (maxDate < maxCurrentNamedItem)
						{
							maxDate = maxCurrentNamedItem;
						}

						if (!setupAxis)
						{
							setupAxis = true;
						}

						this.chart.FormatNumber += chart_FormatNumber;

						series.IsXValueIndexed = false;

						foreach (PlainItem item in namedItems)
						{
							double y = GraphData.FormatNumericValue(
								configuration.ItemsConfiguration.Unit,
								item.YValue
							);

							series.Points.AddXY(item.DateTime, y);
						}
					}
				}
			}

			ChartArea chartArea = this.chart.ChartAreas[0];

			if (setupAxis)
			{
				chartArea.AxisX.Minimum           = minDate.ToOADate();
				chartArea.AxisX.Maximum           = maxDate.ToOADate();
				chartArea.AxisX.IntervalAutoMode  = IntervalAutoMode.VariableCount;
				chartArea.AxisX.IntervalType      = DateTimeIntervalType.Auto;
				chartArea.AxisX.Interval          = 0.0d;
				chartArea.AxisX.IsStartedFromZero = false;
			}
		}

		private void DrawSimpleGraph(GraphData data, GraphConfiguration configuration)
		{
			var dt = data.PlainData.FirstOrDefault(d => d.Value.Where(p => p.XValue.HasValue).Count() > 0);

			if (dt.Value != null)
			{
				DrawSimpleXYGraph(dt.Value, configuration);
			}
			else
			{
				DrawSimpleDateTimeGraph(data, configuration);
			}
		}

		private void DrawSimpleDateTimeGraph(GraphData data, GraphConfiguration configuration)
		{
			int i = 0;

			this.chart.FormatNumber -= chart_FormatNumber;

			foreach (var items in data.PlainData)
			{
				var series = new Series();

				SetupSeries(series, configuration, i++);

				series.IsXValueIndexed = true;

				switch (configuration.GraphType.Value)
				{
					case GraphTypeEnum.Line:
						series.ChartType = SeriesChartType.Line;
						break;

					case GraphTypeEnum.Pie:
						series.ChartType = SeriesChartType.Pie;
						break;

					case GraphTypeEnum.Column:
						series.ChartType = SeriesChartType.Column;
						break;

					case GraphTypeEnum.StackedColumn:
						series.ChartType = SeriesChartType.StackedColumn;
						break;
				}

				series.Name = items.Key;

				this.chart.Series.Add(series);

				foreach (var item in items.Value)
				{
					if (String.IsNullOrEmpty(item.Name) && configuration.GraphType.Value != GraphTypeEnum.Pie
						&& items.Value[items.Value.Count - 1].DateTime - items.Value[0].DateTime <= new TimeSpan(2, 0, 0, 0)
					)
					{
						var d = items.Value.SelectMany(it => new[] { it.DateTime }).ToList();

						var min = d.Min();
						var max = d.Max();

						this.chart.ChartAreas[0].AxisX.Minimum = min.ToOADate();
						this.chart.ChartAreas[0].AxisX.Maximum = max.ToOADate();

						if (item.Equals(items.Value[0]))
						{
							this.chart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
							this.chart.ChartAreas[0].AxisX.IntervalType     = DateTimeIntervalType.Auto;
							this.chart.ChartAreas[0].AxisX.Interval         = 0.0;

							this.chart.FormatNumber += chart_FormatNumber;

							series.IsXValueIndexed = false;
						}

						var y = GraphData.FormatNumericValue(configuration.ItemsConfiguration.Unit, item.YValue);

						series.Points.AddXY(item.DateTime, y);
					}
					else
					{
						var x     = item.Name != null ? (object)item.Name : item.DateTime;
						var y     = GraphData.FormatNumericValue(configuration.ItemsConfiguration.Unit, item.YValue);
						var index = series.Points.AddXY(x, y);
						var point = series.Points[index];

						if (configuration.GraphType.Value == GraphTypeEnum.Pie)
						{
							SetupPoint(point, configuration, index);
						}
					}
				}
			}

			this.chart.ChartAreas[0].AxisX.IsStartedFromZero = false;
		}

		private void DrawSimpleXYGraph(List<PlainItem> values, GraphConfiguration configuration)
		{
			var linesData = values.GroupBy(v => v.Name);
			int i         = 0;

			foreach (var items in linesData)
			{
				var series = new Series();

				SetupSeries(series, configuration, i++);

				series.IsXValueIndexed = false;
				series.ChartType       = SeriesChartType.Line;
				series.Name            = items.Key;

				this.chart.Series.Add(series);

				this.chart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;

				if (configuration.AxisXConfiguration.Interval.HasValue)
				{
					this.chart.ChartAreas[0].AxisX.Interval = configuration.AxisXConfiguration.Interval.Value;
				}

				this.chart.ChartAreas[0].AxisX.LabelStyle.Format = configuration.AxisXConfiguration.Format;

				this.chart.FormatNumber += chart_FormatNumber;

				var orderedItems = items.OrderBy(itm => itm.XValue);

				foreach (var item in orderedItems)
				{
					var    x  = item.Name != null ? (object)item.Name : item.DateTime;
					var    y  = GraphData.FormatNumericValue(configuration.ItemsConfiguration.Unit, item.YValue);
					double fx = GraphData.FormatNumericValue(configuration.ItemsConfiguration.Unit, item.XValue.Value);

					series.Points.AddXY(fx, y);
				}
			}
		}

		#region Private methods Sorting group

		private static void HourSort(
			GraphData          data,
			GraphConfiguration configuration,
			ValGroup           sortGroup,
			string             name,
			Series             series
		)
		{
			int     i     = 0;
			int     point = 0;
			double? value = null;

			for (DateTime date = data.FromDate; date <= data.ToDate; date = date.AddHours(1))
			{
				value = data.GetValueForNameAndDate(name, date, data.Configuration.ItemsConfiguration.Unit, sortGroup);

				if (value != null && value >= 1.0d)
				{
					point = series.Points.AddXY(i, value.Value);

					series.Points[point].AxisLabel = date.ToString("HH") + "h " + date.ToString("dd.MM.yy");
				}
				else if (value != null && value < 1.0d)
				{
				}
				else if (configuration.GraphType.Value == GraphTypeEnum.NameDateStackedColumn)
				{
					point = series.Points.AddXY(i, 0.0d);

					series.Points[point].AxisLabel = date.ToString("HH") + "h " + date.ToString("dd.MM.yy");
				}

				i++;
			}
		}

		private static void MinuteSecondSort(
			GraphData          data,
			GraphConfiguration configuration,
			ValGroup           sortGroup,
			string             name,
			Series             series
		)
		{
			int     i     = 0;
			int     point = 0;
			double? value = null;

			for (DateTime date = data.FromDate; date <= data.ToDate; date = sortGroup == ValGroup.Minute ? date.AddMinutes(1) : date.AddSeconds(1))
			{
				value = data.GetValueForNameAndDate(name, date, data.Configuration.ItemsConfiguration.Unit, sortGroup);

				if (value != null && value >= 1.0d)
				{
					point = series.Points.AddXY(i, value.Value);

					series.Points[point].AxisLabel = date.ToString("HH:mm" + (sortGroup == ValGroup.Minute ? "" : ":ss") + " dd.MM.yy");
				}
				else if (value != null && value < 1.0d)
				{
				}
				else if (configuration.GraphType.Value == GraphTypeEnum.NameDateStackedColumn)
				{
					point = series.Points.AddXY(i, 0.0d);

					series.Points[point].AxisLabel = date.ToString("HH:mm" + (sortGroup == ValGroup.Minute ? "" : ":ss") + " dd.MM.yy");
				}

				i++;
			}
		}

		private static void MonthWeekDaySort(
			GraphData          data,
			GraphConfiguration configuration,
			string             name,
			Series             series,
			ValGroup           sort
		)
		{
			int     i     = 0;
			int     point = 0;
			double? value = null;

			for (DateTime date = data.FromDate.Date; date <= data.ToDate.Date; date = date.Date.AddDays(1))
			{
				DateTime labelDate = date;

				if (sort == ValGroup.Week)
				{
					labelDate = labelDate.PreviousMonday();
				}

				value = data.GetValueForNameAndDate(name, date, data.Configuration.ItemsConfiguration.Unit, sort);

				if (value != null && value >= 1.0d)
				{
					point = 0;

					switch (sort)
					{
						case ValGroup.Month:
							point = series.Points.AddXY(i, value.Value);
							series.Points[point].AxisLabel = date.Date.ToString("MM.yyyy");
							break;

						case ValGroup.Day:
							point = series.Points.AddXY(date, value.Value);
							series.Points[point].AxisLabel = date.Date.ToString("dd.MM.yyyy");
							break;

						case ValGroup.Week:
							point = series.Points.AddXY(labelDate, value.Value);
							series.Points[point].AxisLabel = labelDate.Date.ToString("dd.MM.yyyy");
							break;
					}
				}
				else if (value != null && value < 1.0d)
				{
				}
				else if (configuration.GraphType.Value == GraphTypeEnum.NameDateStackedColumn)
				{
					switch (sort)
					{
						case ValGroup.Month:
							int t = series.Points.AddXY(i, 0.0d);

							series.Points[t].AxisLabel = date.Date.ToString("MM.yyyy");

							while (date.AddDays(1).Day != 1)
							{
								date = date.AddDays(1);
							}
							break;

						case ValGroup.Day: t = series.Points.AddXY(date, 0.0d);
							series.Points[t].AxisLabel = date.Date.ToString("dd.MM.yyyy");
							break;

						case ValGroup.Week:
							if (date.DayOfWeek == DayOfWeek.Monday)
							{
								t = series.Points.AddXY(labelDate, 0.0d);
								series.Points[t].AxisLabel = labelDate.Date.ToString("dd.MM.yyyy");
							}
							break;
					}
				}

				i++;
			}
		}

		#endregion Private methods Sorting group

		#endregion Draw graphics

		private void ShowMouseCoordinates(MouseEventArgs e)
		{
			string strCursorSeriesName = string.Empty;
			string strToolTipMessage   = string.Empty;

			if (this.chart != null && this.chart.ChartAreas != null && this.chart.ChartAreas.Count > 0)
			{
				var hitTest = this.chart.HitTest(e.X, e.Y);

				if (hitTest.Series != null)
				{
					strCursorSeriesName = hitTest.Series.Name;
				}
				else
				{
					strCursorSeriesName = string.Empty;
				}

				ChartArea ca = this.chart.ChartAreas[0];

				ca.CursorX.LineWidth = 0;
				ca.CursorY.LineWidth = 0;

				ca.CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);
				ca.CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);

				if (!string.IsNullOrEmpty(strCursorSeriesName))
				{
					strToolTipMessage = string.Format(
						"{0} ({1}, {2})",
						strCursorSeriesName,
						ca.CursorX.Position,
						ca.CursorY.Position
					);
				}
				else
				{
					strToolTipMessage = string.Format(
						"({0}, {1})",
						ca.CursorX.Position,
						ca.CursorY.Position
					);
				}

				this._toolTipMousePosition.Show(
					strToolTipMessage,
					this,
					e.X,
					e.Y - 20
				);
			}
		}

		private void chart_MouseUp(object sender, MouseEventArgs e)
		{
			this._toolTipMousePosition.Hide(this);
		}

		private void MouseDownDefault(MouseEventArgs e)
		{
			if (this.chart.Series.Count > 0)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.ShowMouseCoordinates(e);

					var hitTest = this.chart.HitTest(e.X, e.Y);

					if (hitTest.ChartElementType == ChartElementType.LegendItem)
					{
						this.SelectSeries(hitTest.Series);
					}
					else if (hitTest.ChartElementType == ChartElementType.DataPoint)
					{
						this.SelectSeries(hitTest.Series);
					}
					else
					{
						this.SelectSeries(null);
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					ContextMenuStrip contextMenu = new ContextMenuStrip();

					var hideSeries = (from series in this.chart.Series
						where !series.Enabled
						select series
					).ToList();

					var hitTest = this.chart.HitTest(e.X, e.Y);

					if (hitTest.Series != null)
					{
						ToolStripMenuItem nameItem = new ToolStripMenuItem(hitTest.Series.Name);

						nameItem.Enabled = false;

						contextMenu.Items.Add(nameItem);

						contextMenu.Items.Add(GetLocalizedText(MenuSelect)).Click +=
							(o, args) => SelectSeries(hitTest.Series);

						if (contextMenu.Items.Count > 0)
						{
							contextMenu.Items.Add(new ToolStripSeparator());
						}
					}

					if (hitTest.Series != null)
					{
						contextMenu.Items.Add(GetLocalizedText(MenuHide)).Click +=
							(o, args) => SetSeriesEnabled(hitTest.Series, false);
					}

					if (hideSeries.Count != this.chart.Series.Count)
					{
						ToolStripMenuItem hideAllItems =
							new ToolStripMenuItem(GetLocalizedText(MenuHideAll));

						hideAllItems.Click += (sender, args) =>
						{
							foreach (var s in this.chart.Series)
							{
								SetSeriesEnabled(s, false);
							}

							Scale();
						};

						contextMenu.Items.Add(hideAllItems);
					}

					if (hideSeries.Count > 0)
					{
						if (contextMenu.Items.Count > 0)
						{
							contextMenu.Items.Add(new ToolStripSeparator());
						}

						ToolStripMenuItem restoreItems =
							new ToolStripMenuItem(GetLocalizedText(MenuRestore));

						contextMenu.Items.Add(restoreItems);

						foreach (var series in hideSeries)
						{
							var item = restoreItems.DropDownItems.Add(series.Name);

							item.Click += (o, args) =>
							{
								SetSeriesEnabled(series, true);
							};
						}

						if (hideSeries.Count > 1)
						{
							restoreItems.DropDownItems.Add(new ToolStripSeparator());

							restoreItems.DropDownItems.Add(GetLocalizedText(MenuAll)).Click +=
								(o, args) =>
								{
									foreach (Series s in hideSeries)
									{
										SetSeriesEnabled(s, true);
									}
								};
						}
					}

					if (this.chart.Legends.Any())
					{
						if (contextMenu.Items.Count > 0)
						{
							contextMenu.Items.Add(new ToolStripSeparator());
						}

						ToolStripMenuItem showLegendItem = (ToolStripMenuItem)contextMenu.Items.Add(GetLocalizedText(LegendShow));

						showLegendItem.Checked = this.chart.Legends[0].Enabled;

						showLegendItem.Click +=
							(o, args) =>
							{
								ToolStripMenuItem item = (ToolStripMenuItem)o;

								item.Checked = !item.Checked;

								this.chart.Legends[0].Enabled = item.Checked;

								this._stateSettings.LegendEnabled = item.Checked;

								Program.Model.LayoutSettings.SetExtendedSettings(
									this._id,
									this._preprocessorTypeName,
									Program.Model.Settings.ReportLanguage,
									this._stateSettings
								);
							};
					}

					this.chart.ContextMenuStrip = contextMenu;
				}
			}
		}

		private void MouseDownPie(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				var hitTest = this.chart.HitTest(e.X, e.Y);

				if (hitTest.ChartElementType == ChartElementType.LegendItem)
				{
					this.SelectPoint(hitTest.Series.Points[hitTest.PointIndex]);
				}
				else if (hitTest.ChartElementType == ChartElementType.DataPoint)
				{
					this.SelectPoint(hitTest.Series.Points[hitTest.PointIndex]);
				}
				else
				{
					this.SelectSeries(null);
				}
			}
		}

		private void RevalueAxisY(Series series)
		{
			double min = 0.0d;
			double max = 0.0d;

			foreach (var item in series.Points)
			{
				if (min == 0.0d || item.YValues[0] < min)
				{
					min = item.YValues[0];
				}

				if (max == 0.0d || item.YValues[1] > max)
				{
					max = item.YValues[1];
				}
			}

			var area = this.chart.ChartAreas[0];

			if (min <= area.AxisY.Minimum || max >= area.AxisY.Maximum)
			{
				min = 0.0d;
				max = 0.0d;

				foreach (var item in this.chart.Series)
				{
					foreach (var point in item.Points)
					{
						if (item.Enabled && (min == 0.0d || point.YValues[0] < min))
						{
							min = point.YValues[0];
						}

						if (item.Enabled && (max == 0.0d || point.YValues[1] > max))
						{
							max = point.YValues[1];
						}
					}
				}

				area.AxisY.Minimum = min;
				area.AxisY.Maximum = max;
			}
		}

		private void Scale()
		{
			this.chart.ChartAreas[0].RecalculateAxesScale();
		}

		private void SelectPoint(DataPoint point)
		{
			if (this._selectedPoint != null)
			{
				this._selectedPoint.BorderWidth     = this._oldWidth;
				this._selectedPoint.BorderDashStyle = this._oldDashStyle;
				this._selectedPoint.BorderColor     = this._oldColor;
			}

			this._selectedPoint = point;

			if (this._selectedPoint != null)
			{
				this._oldWidth                      = this._selectedPoint.BorderWidth;
				this._oldDashStyle                  = this._selectedPoint.BorderDashStyle;
				this._oldColor                      = this._selectedPoint.BorderColor;
				this._selectedPoint.BorderWidth     = 5;
				this._selectedPoint.BorderColor     = Color.Red;
				this._selectedPoint.BorderDashStyle = ChartDashStyle.Dash;
			}
		}

		private void SelectSeries(Series series)
		{
			if (this._selectedSeries != null)
			{
				this._selectedSeries.BorderWidth     = this._oldWidth;
				this._selectedSeries.BorderDashStyle = this._oldDashStyle;
				this._selectedSeries.BorderColor     = this._oldColor;
			}

			this._selectedSeries = series;

			if (this._selectedSeries != null)
			{
				this._oldWidth                       = this._selectedSeries.BorderWidth;
				this._oldDashStyle                   = this._selectedSeries.BorderDashStyle;
				this._oldColor                       = this._selectedSeries.BorderColor;
				this._selectedSeries.BorderWidth     = 5;
				this._selectedSeries.BorderColor     = Color.Red;
				this._selectedSeries.BorderDashStyle = ChartDashStyle.Dash;
			}
		}

		private void SetSeriesEnabled(Series series, bool enabled)
		{
			series.Enabled = enabled;

			//if (series.Points.Count > 0 && series.Points[0].XValue.GetType() == typeof(Double))
			//{
			//    series.XValueType = ChartValueType.DateTime;
			//}
			this.Scale();

			this._stateSettings.GetSeriesSettings(series.Name).Visible = enabled;

			Program.Model.LayoutSettings.SetExtendedSettings(
				this._id,
				this._preprocessorTypeName,
				Program.Model.Settings.ReportLanguage,
				this._stateSettings
			);

			if (this._graphType == GraphTypeEnum.GanttDiagram)
			{
				this.RevalueAxisY(series);
			}
		}

		private void SetupChartMargin(GraphConfiguration configuration)
		{
			//
			// AxisX.IsMarginVisible
			//
			if (this.chart.ChartAreas[0].AxisX.LabelStyle.Enabled)
			{
				this.chart.ChartAreas[0].AxisX.IsMarginVisible = true;

				// top
				this.chart.ChartAreas[0].InnerPlotPosition.Y = configuration.GraphMargins.GetTop();
				// bottom
				this.chart.ChartAreas[0].InnerPlotPosition.Height = configuration.GraphMargins.GetBottom();
			}
			else
			{
				this.chart.ChartAreas[0].AxisX.IsMarginVisible = false;

				// top
				this.chart.ChartAreas[0].InnerPlotPosition.Y = 0.0f;
				// bottom
				this.chart.ChartAreas[0].InnerPlotPosition.Height = 100.0f;
			}

			//
			// AxisY.IsMarginVisible
			//
			if (this.chart.ChartAreas[0].AxisY.LabelStyle.Enabled)
			{
				this.chart.ChartAreas[0].AxisY.IsMarginVisible = true;

				// left
				this.chart.ChartAreas[0].InnerPlotPosition.X = configuration.GraphMargins.GetLeft();
				// right
				this.chart.ChartAreas[0].InnerPlotPosition.Width = configuration.GraphMargins.GetRight();
			}
			else
			{
				this.chart.ChartAreas[0].AxisY.IsMarginVisible = false;

				// left
				this.chart.ChartAreas[0].InnerPlotPosition.X = 0.0f;
				// right
				this.chart.ChartAreas[0].InnerPlotPosition.Width = 100.0f;
			}

			/*
			// left
			this.chart.ChartAreas[0].InnerPlotPosition.X = configuration.GraphMargins.GetLeft();

			// right
			this.chart.ChartAreas[0].InnerPlotPosition.Width = configuration.GraphMargins.GetRight();

			// top
			this.chart.ChartAreas[0].InnerPlotPosition.Y = configuration.GraphMargins.GetTop();

			// bottom
			this.chart.ChartAreas[0].InnerPlotPosition.Height = configuration.GraphMargins.GetBottom();
			*/

			this.chart.ChartAreas[0].Position.X = this.chart.ChartAreas[0].InnerPlotPosition.X;

			if (this.chart.ChartAreas[0].AxisY.LabelStyle.Enabled)
			{
				this.chart.ChartAreas[0].Position.Y = 1;
			}
			else
			{
				this.chart.ChartAreas[0].Position.Y = 0;
			}

			this.chart.ChartAreas[0].Position.Width  = this.chart.ChartAreas[0].InnerPlotPosition.Width;

			if (this.chart.ChartAreas[0].AxisY.LabelStyle.Enabled)
			{
				this.chart.ChartAreas[0].Position.Height = 98;
			}
			else
			{
				this.chart.ChartAreas[0].Position.Height = 100;
			}

			if (!configuration.LegendConfiguration.GraphicOverlap)
			{
				this.chart.ChartAreas[0].Position.Auto = true;
			}
			else
			{
				this.chart.ChartAreas[0].Position.Auto = false;
			}

			this.chart.Padding = new Padding(0, 0, 0, 0);

			/*
			var chartArea = this.chart.ChartAreas[0];

			chartArea.AxisX.IsMarginVisible = true;
			chartArea.AxisY.IsMarginVisible = true;

			chartArea.InnerPlotPosition = new ElementPosition(
				-0.5f,
				-0.5f,
				configuration.GraphMargins.GetRight() + 3,
				configuration.GraphMargins.GetBottom() + 3
			);
			*/
		}

		private void SetupGraph(GraphConfiguration configuration)
		{
			this._graphType = configuration.GraphType;

			// set background color for the graph as transparent
			this.chart.BackColor = Color.Transparent;

			this.chart.ChartAreas[0].BorderWidth = configuration.AxisXConfiguration.MajorGrid.Enabled
				? configuration.AxisXConfiguration.MajorGrid.LineWidth
				: 0;

			this.chart.ChartAreas[0].BorderColor = configuration.AxisXConfiguration.MajorGrid.LineColor;

			this.chart.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;

			switch (configuration.GraphType.Value)
			{
				case GraphTypeEnum.GanttDiagram:
					this.chart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
					SetupAxisX(this.chart.ChartAreas[0].AxisY, configuration);
					this.chart.FormatNumber += chart_FormatNumber;
					break;

				default:
					SetupAxisX(this.chart.ChartAreas[0].AxisX, configuration);
					SetupAxisY(configuration.AxisYConfiguration);
					break;
			}

			this.chart.Legends[0].Enabled   = configuration.LegendConfiguration.Enabled;
			this.chart.Legends[0].Docking   = configuration.LegendConfiguration.Docking;
			this.chart.Legends[0].Alignment = configuration.LegendConfiguration.Alignment;

			if (configuration.LegendConfiguration.GraphicOverlap)
			{
				// this.chart.Legends[0].Alignment = StringAlignment.Center;
				// this.chart.Legends[0].Docking = Docking.Top;
				this.chart.Legends[0].DockedToChartArea = this.chart.ChartAreas[0].Name;
			}

			SetupChartMargin(configuration);
		}

		private static void SetupPoint(DataPoint point, GraphConfiguration configuration, int index)
		{
			if (configuration.Palette.Count > 0)
			{
				PaletteItem item = configuration.Palette[index % configuration.Palette.Count];

				point.BackHatchStyle = item.HatchStyle;
				point.Color          = item.Color;
			}
		}

		private static void SetupSeries(Series series, GraphConfiguration configuration, int index)
		{
			if (configuration.Palette.Count > 0)
			{
				PaletteItem item = configuration.Palette[index % configuration.Palette.Count];

				series.BackHatchStyle = item.HatchStyle;
				series.Color          = item.Color;
			}
		}

	}
}
