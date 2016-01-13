using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Graph
{
	/// <summary>
	/// Smart size for parsing strings like "70" - 70px, "70%" - 70% of original size, "70% width" - 70% of self size
	/// </summary>
	public class SmartSize
	{
		struct SmartDimension
		{
			public readonly double Value;
			public readonly bool   Percent;
			public readonly bool   DependsOn;

			public SmartDimension(string fromString)
			{
				string[] parsed = fromString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				this.DependsOn = parsed.Length == 2;
				this.Percent   = (parsed[0].EndsWith("%"));
				this.Value     = Double.Parse(parsed[0].Replace("%", String.Empty));
			}
		}

		/// <summary>
		/// Smart height string
		/// </summary>
		[XmlElement]
		public string Height { get; set; }

		/// <summary>
		/// Smart width string
		/// </summary>
		[XmlElement]
		public string Width { get; set; }

		/// <summary>
		/// Get size in px for current smart height and width and owner control size
		/// </summary>
		/// <param name="owner"></param>
		/// <returns></returns>
		public Size GetSize(Size owner)
		{
			SmartDimension w = new SmartDimension(Width);
			SmartDimension h = new SmartDimension(Height);

			if ((w.DependsOn) && (h.DependsOn))
			{
				throw new ArgumentException("Circular dimension reference");
			}

			double resultW = 0.0d;
			double resultH = 0.0d;

			if (!w.DependsOn)
			{
				if (w.Percent)
				{
					resultW = owner.Width * w.Value / 100.0d;
				}
				else
				{
					resultW = w.Value;
				}
			}
			if (!h.DependsOn)
			{
				if (h.Percent)
				{
					resultH = owner.Height * h.Value / 100.0d;
				}
				else
				{
					resultH = h.Value;
				}
			}
			if (w.DependsOn)
			{
				resultW = resultH * w.Value / 100.0d;
			}

			if (h.DependsOn)
			{
				resultH = resultW * h.Value / 100.0d;
			}

			double verticalScrollBarWidth    = SystemInformation.VerticalScrollBarWidth;
			double horizontalScrollBarHeight = 0.0d; // SystemInformation.HorizontalScrollBarHeight;

			if ((resultH > (owner.Height - horizontalScrollBarHeight)) && (h.Percent))
			{
				resultH = owner.Height - horizontalScrollBarHeight;
			}

			if ((resultW > (owner.Width - verticalScrollBarWidth)) && (w.Percent))
			{
				resultW = owner.Width - verticalScrollBarWidth;
			}

			return new Size((int)resultW, (int)resultH);
		}
	}

	/// <summary>
	/// Color class for XML configuration representation
	/// </summary>
	public class ConfigurationColor
	{
		private Color _color;

		/// <summary>
		/// .NET color instance
		/// </summary>
		[XmlIgnore]
		public Color Color
		{
			get { return this._color; }
			set { this._color = value; }
		}

		/// <summary>
		/// String for color representation. It can be "Black" or "#ff1234"
		/// </summary>
		[XmlElement]
		public string Name
		{
			get
			{
				if ((this._color.Name.Length == 8) && (this._color.Name.ToLower().StartsWith("ff")))
				{
					return "#" + this._color.Name.Substring(2);
				}

				return this._color.Name;
			}
			set
			{
				if (value.StartsWith("#"))
				{
					this._color = Color.FromArgb(Int32.Parse(value.Replace("#", "ff"), System.Globalization.NumberStyles.HexNumber));
				}
				else
				{
					this._color = Color.FromName(value);
				}
			}
		}

		/// <summary>
		/// Color.
		/// </summary>
		/// <param name="d">Configuretion color.</param>
		/// <returns>Return color configuration.</returns>
		public static implicit operator Color(ConfigurationColor d)
		{
			return d._color;
		}

		/// <summary>
		/// Configuration color.
		/// </summary>
		/// <param name="c">Color.</param>
		/// <returns>Return configuration color, wher color equal parameter.</returns>
		public static implicit operator ConfigurationColor(Color c)
		{
			return new ConfigurationColor { Color = c };
		}
	}

	/// <summary>
	/// Wrapper for Enums to gently reading from XML
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
	[XmlInclude(typeof(Color))]
	[XmlInclude(typeof(ChartHatchStyle))]
	[XmlInclude(typeof(DayOfWeek))]
	[XmlInclude(typeof(Docking))]
	[XmlInclude(typeof(StringAlignment))]

	public struct EnumWrapper<T>
	{
		private T _enumValue;

		/// <summary>
		/// Enum original value
		/// </summary>
		[XmlIgnore]
		public T Value
		{
			get { return this._enumValue; }
			set { this._enumValue = value; }
		}

		/// <summary>
		/// Enum text representation. It can contain unsignificant Tabs, Spaces and NewLine symbols
		/// </summary>
		[XmlText]
		public string Text
		{
			get { return this._enumValue.ToString(); }
			set { this._enumValue = (T)Enum.Parse(typeof (T), value.Replace("\n", String.Empty).Replace("\t","".Replace(" ", String.Empty))); }
		}

		/// <summary>
		/// Type object.
		/// </summary>
		/// <param name="w">Enum wrapper.</param>
		/// <returns>Return enum value.</returns>
		public static implicit operator T(EnumWrapper<T> w)
		{
			return w._enumValue;
		}

		/// <summary>
		/// Type object.
		/// </summary>
		/// <param name="v"></param>
		/// <returns>Return new enum wrapper is parameter.</returns>
		public static implicit operator EnumWrapper<T>(T v)
		{
			return new EnumWrapper<T>(){Value = v};
		}
	}

	/// <summary>
	/// Palette item for Graph
	/// </summary>
	public class PaletteItem
	{
		/// <summary>
		/// Color of palette element
		/// </summary>
		[XmlElement]
		public ConfigurationColor Color { get; set; }

		/// <summary>
		/// Hatch style of palette element <see href="http://msdn.microsoft.com/en-us/library/vstudio/system.windows.forms.datavisualization.charting.charthatchstyle(v=vs.100).aspx"/>
		/// </summary>
		[XmlElement]
		public EnumWrapper<ChartHatchStyle> HatchStyle { get; set; }
	}

	/// <summary>
	/// Default GraphSource for extracting data from XML file
	/// </summary>
	public class XmlFileGraphSource : GraphSource
	{
		private string       _fileName;
		private string       _pathToItems;
		private string       _itemTag;
		private string       _dateTag;
		private string       _nameTag;
		private List<string> _valueTag;

		/// <summary>
		/// FileName of XML file
		/// </summary>
		[XmlElement]
		public string FileName
		{
			get { return this._fileName; }
			set { this._fileName = value; }
		}

		/// <summary>
		/// Tag for PlainItem (eg. "GetJobsExecutionHistory")
		/// </summary>
		[XmlElement]
		public string ItemTag
		{
			get { return this._itemTag; }
			set { this._itemTag = value; }
		}

		/// <summary>
		/// Path to PlainItems list from the root divided by '/' or '\' (eg. "Instance/Audit/Jobs")
		/// </summary>
		[XmlElement]
		public string PathToItems
		{
			get { return this._pathToItems; }
			set { this._pathToItems = value; }
		}

		/// <summary>
		/// Tag for date (eg. "JobRunDateTime"). Mandatory for StackedColumn graph.
		/// </summary>
		[XmlElement]
		public string DateTag
		{
			get { return this._dateTag; }
			set { this._dateTag = value; }
		}

		/// <summary>
		/// Tag for name (eg. "JobName"). Mandatory for StackedColumn graph.
		/// </summary>
		[XmlElement]
		public string NameTag
		{
			get { return this._nameTag; }
			set { this._nameTag = value; }
		}

		/// <summary>
		/// Tag for value (eg. "JobDurationInSeconds"). Mandatory.
		/// </summary>
		[XmlElement]
		public List<string> ValueTag
		{
			get { return this._valueTag; }
			set { this._valueTag = value; }
		}

		[XmlElement]
		public string BaseTag
		{
			get;
			set;
		}

		private ValGroup _valuegroup = ValGroup.Day;

		/// <summary>
		/// Value group for sort.
		/// </summary>
		[XmlElement]
		public ValGroup ValueGroup
		{
			get { return this._valuegroup; }
			set { this._valuegroup = value; }
		}

		/// <summary>
		/// Begin date tag.
		/// </summary>
		[XmlElement("DateTagBegin")]
		public string DateTagBegin { get; set; }

		/// <summary>
		/// End date tag.
		/// </summary>
		[XmlElement("DateTagEnd")]
		public string DateTagEnd { get; set; }

		internal override List<GanttJobItem> GetGanttData(object context)
		{
			var result   = new List<GanttJobItem>();
			var document = context as XmlDocument;

			if (document == null)
			{
				document = new XmlDocument();

				document.Load(this._fileName);
			}

			XPathNavigator currentElement = null;
			var            navigator      = document.CreateNavigator();
			var            iterator       = navigator.Select(XPathExpression.Compile(PathToItems));

			currentElement = iterator.OfType<XPathNavigator>().FirstOrDefault();

			if (currentElement != null)
			{
				var historyElements =
					currentElement.Select(ItemTag)
						.Cast<XPathNavigator>().ToList();

				foreach (XPathNavigator historyElementNew in historyElements)
				{
					XmlElement historyElement = historyElementNew.UnderlyingObject as XmlElement;

					var startElement = historyElement[DateTagBegin];
					var start        = startElement != null ? DateTime.Parse(startElement.InnerText) : DateTime.MinValue;
					var endElement   = historyElement[DateTagEnd];
					var end          = endElement != null ? DateTime.Parse(endElement.InnerText) : DateTime.MinValue;
					var nameElement  = historyElement[NameTag];
					var name         = nameElement != null ? nameElement.InnerText : null;

					result.Add(new GanttJobItem(name, start, end));
				}
			}

			return result;
		}

		private readonly NumberFormatInfo _decimalCommaNumberFormat = new NumberFormatInfo
		{
			NumberDecimalSeparator = ","
		};

		internal override Dictionary<string, List<PlainItem>> GetPlainData(object context)
		{
			Dictionary<string, List<PlainItem>> result   = new Dictionary<string, List<PlainItem>>();
			var                                 document = context as XmlDocument;

			if (document == null)
			{
				document = new XmlDocument();

				document.Load(this._fileName);
			}

			XPathNavigator currentElement = null;

			var navigator = document.CreateNavigator();
			var iterator = navigator.Select(XPathExpression.Compile(PathToItems));

			currentElement = iterator.OfType<XPathNavigator>().FirstOrDefault();

			if (currentElement != null)
			{
				var historyElements =
					currentElement.Select(ItemTag)
						.Cast<XPathNavigator>().ToList();

				foreach (XPathNavigator historyElementNew in historyElements)
				{
					XmlElement historyElement = historyElementNew.UnderlyingObject as XmlElement;
					DateTime   runTime        = DateTime.MinValue;
					Double?    baseValue      = null;
					string     name           = null;
					XmlElement xmlElement     = historyElement[DateTag];

					if (xmlElement != null)
					{
						runTime = DateTime.Parse(xmlElement.InnerText);
					}

					xmlElement = historyElement[BaseTag];

					if (xmlElement != null)
					{
						baseValue = double.Parse(xmlElement.InnerText.Replace(".", _decimalCommaNumberFormat.NumberDecimalSeparator), NumberStyles.Any, _decimalCommaNumberFormat);
					}

					xmlElement = historyElement[NameTag];

					if (xmlElement != null)
					{
						name = xmlElement.InnerText;
					}

					int itemIndex = -1;

					foreach (string tag in ValueTag)
					{
						itemIndex++;

						xmlElement = historyElement[tag];

						double yValue = 0.0d;

						if (xmlElement == null ||
							!double.TryParse(
								xmlElement.InnerText.Replace(".", _decimalCommaNumberFormat.NumberDecimalSeparator),
								NumberStyles.Any, _decimalCommaNumberFormat, out yValue
							)
						)
						{
							yValue = 0.0d;
						}

						if (!result.ContainsKey(tag))
						{
							result.Add(tag, new List<PlainItem>());
						}

						if (!baseValue.HasValue)
						{
							result[tag].Add(new PlainItem
								{
									YValue   = yValue,
									Name     = name,
									DateTime = runTime
								}
							);
						}
						else
						{
							result[tag].Add(new PlainItem
								{
									YValue = yValue,
									Name   = name,
									XValue = baseValue
								}
							);
						}
					}
				}
			}

			return result;
		}
	}

	/// <summary>
	/// Base data source class for PlainItems
	/// </summary>
	[XmlInclude(typeof (XmlFileGraphSource))]
	public abstract class GraphSource
	{
		/// <summary>
		/// Extract PlainItems for the Graph according settings
		/// </summary>
		/// <returns>List of PlainItems as a source data for Graph</returns>
		internal abstract Dictionary<string, List<PlainItem>> GetPlainData(object context);

		internal abstract List<GanttJobItem> GetGanttData(object context);
	}

	/// <summary>
	/// Unit types
	/// </summary>
	public enum UnitEnum
	{
		/// <summary>
		/// Second.
		/// </summary>
		[EnumMember] Second,
		/// <summary>
		/// Minute.
		/// </summary>
		[EnumMember] Minute,
		/// <summary>
		/// Hour.
		/// </summary>
		[EnumMember] Hour,
		/// <summary>
		/// Byte.
		/// </summary>
		[EnumMember] Byte,
		/// <summary>
		/// Kilobyte.
		/// </summary>
		[EnumMember] Kilobyte,
		/// <summary>
		/// Megabyte.
		/// </summary>
		[EnumMember] Megabyte,
		/// <summary>
		/// Gigabyte.
		/// </summary>
		[EnumMember] Gigabyte,
		/// <summary>
		/// Number for data of different type
		/// </summary>
		[EnumMember] Number,
	}

	/// <summary>
	/// Enumeration for sort.
	/// </summary>
	public enum ValGroup
	{
		/// <summary>
		/// Second
		/// </summary>
		[EnumMember] Second,
		/// <summary>
		/// Minute
		/// </summary>
		[EnumMember] Minute,
		/// <summary>
		/// Hour
		/// </summary>
		[EnumMember] Hour,
		/// <summary>
		/// Day
		/// </summary>
		[EnumMember] Day,
		/// <summary>
		/// Week
		/// </summary>
		[EnumMember] Week,
		/// <summary>
		/// Month
		/// </summary>
		[EnumMember] Month
	}

	/// <summary>
	/// Axis X label filter
	/// </summary>
	public enum AxisXLabelFilter
	{
		/// <summary>
		/// Show all axis values
		/// </summary>
		[EnumMember] All,
		/// <summary>
		/// Show all axis values as days
		/// </summary>
		[EnumMember] AllDays,
		/// <summary>
		/// Show only first day of week's values
		/// </summary>
		[EnumMember] StartOfWeek,
		/// <summary>
		/// Show only
		/// </summary>
		[EnumMember] StartOfMonth
	}

	/// <summary>
	/// Axis Y configuration class
	/// </summary>
	public class AxisYConfiguration : AxisConfiguration
	{
	}

	/// <summary>
	/// Axis X configuration class
	/// </summary>
	public class AxisXConfiguration : AxisConfiguration
	{
		private EnumWrapper<AxisXLabelFilter> _axisXLabelFilter;

		/// <summary>
		/// Axis X label filter
		/// </summary>
		[XmlElement]
		public EnumWrapper<AxisXLabelFilter> AxisXLabelFilter
		{
			get { return this._axisXLabelFilter; }
			set { this._axisXLabelFilter = value; }
		}
	}

	/// <summary>
	/// Base axis configuration class
	/// </summary>
	public class AxisConfiguration
	{
		private string                 _Format;
		private string                 _Name;
		private GraphGridConfiguration _majorGrid;
		private GraphGridConfiguration _minorGrid;

		/// <summary>
		/// Initializing object AxisConfiguration.
		/// </summary>
		public AxisConfiguration()
		{
			this._Format    = "#,###";
			this._Name      = String.Empty;
			this._majorGrid = new GraphGridConfiguration();
			this._minorGrid = new GraphGridConfiguration();
		}

		/// <summary>
		/// Major grid configuration
		/// </summary>
		[XmlElement]
		public GraphGridConfiguration MajorGrid
		{
			get { return this._majorGrid; }
			set { this._majorGrid = value; }
		}

		/// <summary>
		/// Minor grid configuration
		/// </summary>
		[XmlElement]
		public GraphGridConfiguration MinorGrid
		{
			get { return this._minorGrid; }
			set { this._minorGrid = value; }
		}

		/// <summary>
		/// String format of axis values
		/// </summary>
		[XmlElement]
		public string Format
		{
			get
			{
				return this._Format;
			}

			set
			{
				if (value != null)
				{
					this._Format = value.Trim();
				}
				else
				{
					this._Format = null;
				}
			}
		}

		/// <summary>
		/// Name of axis value.
		/// </summary>
		[XmlElement]
		public string AxisTitleName
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (value != null)
				{
					this._Name = value.Trim();
				}
				else
				{
					this._Name = null;
				}
			}
		}

		/// <summary>
		/// Applys to Interval property of Charting Axis object
		/// </summary>
		[XmlElement]
		public double? Interval { get; set; }

		[XmlElement]
		public bool? ShowLabels { get; set; }
	}

	/// <summary>
	/// Grid configuration class
	/// </summary>
	public class GraphGridConfiguration
	{
		private int                _lineWidth;
		private bool               _enabled;
		private ConfigurationColor _lineColor;

		public GraphGridConfiguration()
		{
			this._lineWidth = 1;
			this._enabled   = true;
			this._lineColor = Color.Black;
		}

		/// <summary>
		/// Grid line width
		/// </summary>
		[XmlElement]
		public int LineWidth
		{
			get { return this._lineWidth; }
			set { this._lineWidth = value; }
		}

		/// <summary>
		/// Grid enable status
		/// </summary>
		[XmlElement]
		public bool Enabled
		{
			get { return this._enabled; }
			set { this._enabled = value; }
		}

		/// <summary>
		/// Grid line color
		/// </summary>
		[XmlElement]
		public ConfigurationColor LineColor
		{
			get { return this._lineColor; }
			set { this._lineColor = value; }
		}
	}

	/// <summary>
	/// Method for sorting names of items
	/// </summary>
	public enum NameSortEnum
	{
		/// <summary>
		/// Sorting is disabled
		/// </summary>
		[EnumMember] None,

		/// <summary>
		/// Sorting by name ascending
		/// </summary>
		[EnumMember] NameAscending,

		/// <summary>
		/// Sorting by name descending
		/// </summary>
		[EnumMember] NameDescending
	}

	/// <summary>
	/// Items configuration class
	/// </summary>
	public class ItemsConfiguration
	{
		private EnumWrapper<UnitEnum>     _unit;
		private EnumWrapper<NameSortEnum> _nameSortType;

		public ItemsConfiguration()
		{
			this._unit         = UnitEnum.Second;
			this._nameSortType = NameSortEnum.NameAscending;
		}

		/// <summary>
		/// Unit of the Y values
		/// </summary>
		[XmlElement]
		public EnumWrapper<UnitEnum> Unit
		{
			get { return this._unit; }
			set { this._unit = value; }
		}

		/// <summary>
		/// Legend sorting type
		/// </summary>
		[XmlElement]
		public EnumWrapper<NameSortEnum> NameSortType
		{
			get { return this._nameSortType; }
			set { this._nameSortType = value; }
		}
	}

	/// <summary>
	/// Legend configuration class
	/// </summary>
	public class LegendConfiguration
	{
		private bool                         _enabled;
		private bool                         _graphicOverlap;
		private EnumWrapper<Docking>         _docking;
		private EnumWrapper<StringAlignment> _alignment;

		public LegendConfiguration()
		{
			this._enabled        = true;
			this._graphicOverlap = true;
			this._docking        = System.Windows.Forms.DataVisualization.Charting.Docking.Right;
			this._alignment      = StringAlignment.Center;
		}

		/// <summary>
		/// Enabled of legend.
		/// </summary>
		[XmlElement]
		public Boolean Enabled
		{
			get { return this._enabled; }
			set { this._enabled = value; }
		}

		/// <summary>
		/// Display on top of the chart legend or not.
		/// </summary>
		[XmlElement]
		public Boolean GraphicOverlap
		{
			get { return this._graphicOverlap; }
			set { this._graphicOverlap = value; }
		}

		/// <summary>
		/// Docking of the legend. Values are: Bottom, Left, Right, Top
		/// </summary>
		[XmlElement]
		public EnumWrapper<Docking> Docking
		{
			get { return this._docking; }
			set { this._docking = value; }
		}

		/// <summary>
		/// StringAlignment for the legend names. Values are: Center, Far, Near
		/// </summary>
		[XmlElement]
		public EnumWrapper<StringAlignment> Alignment
		{
			get { return this._alignment; }
			set { this._alignment = value; }
		}
	}

	/// <summary>
	/// Graph type
	/// </summary>
	public enum GraphTypeEnum
	{
		/// <summary>
		/// Column graph with stacked columns
		/// </summary>
		StackedColumn,
		/// <summary>
		/// Smart stacked column graph aggregating data by days and series names.
		/// </summary>
		NameDateStackedColumn,
		/// <summary>
		/// Ordniary column graph
		/// </summary>
		Column,
		/// <summary>
		/// Ordinary pie graph. Can draw several series in one pie.
		/// </summary>
		Pie,
		/// <summary>
		/// Smart line graph aggregating data by days and series names.
		/// </summary>
		NameDateLine,
		/// <summary>
		/// Ordinary line graph aggregating series names.
		/// </summary>
		NamedLine,
		/// <summary>
		/// Ordinary line graph
		/// </summary>
		Line,
		/// <summary>
		/// Gantt Diagram (implemented as Rangebar)
		/// </summary>
		GanttDiagram
	}

	/// <summary>
	/// parsing strings % InnerPlotPossition
	/// </summary>
	public class GraphMargins
	{
		private string _left;
		private string _top;
		private string _right;
		private string _bottom;

		public GraphMargins()
		{
			this._left   = "0%";
			this._top    = "0%";
			this._right  = "0%";
			this._bottom = "0%";
		}

		/// <summary>
		/// Smart X string (like left margin)
		/// </summary>
		[XmlElement]
		public string Left
		{
			get { return this._left; }
			set { this._left = value; }
		}

		/// <summary>
		/// Smart Y string (like top margin)
		/// </summary>
		[XmlElement]
		public string Top
		{
			get { return this._top; }
			set { this._top = value; }
		}

		/// <summary>
		/// Smart height string (like bottom margin)
		/// </summary>
		[XmlElement]
		public string Right
		{
			get { return this._right; }
			set { this._right = value; }
		}

		/// <summary>
		/// Smart width string (like right margin)
		/// </summary>
		[XmlElement]
		public string Bottom
		{
			get { return this._bottom; }
			set { this._bottom = value; }
		}

		public float GetLeft()
		{
			float Value = float.Parse(Left.Replace("%", String.Empty)) + 2;

			return Value;
		}

		public float GetRight()
		{
			float Value = 98.0f - float.Parse(Left.Replace("%", String.Empty)) - float.Parse(Right.Replace("%", String.Empty));

			return Value;
		}

		public float GetTop()
		{
			float Value = float.Parse(Top.Replace("%", String.Empty));

			return Value;
		}

		public float GetBottom()
		{
			float Value = 97.0f - float.Parse(Top.Replace("%", String.Empty)) - float.Parse(Bottom.Replace("%", String.Empty));

			return Value;
		}

		public void SetMargin(int top, int bottom, int left, int right)
		{
			Top    = top.ToString()    + "%";
			Bottom = bottom.ToString() + "%";
			Left   = left.ToString()   + "%";
			Right  = right.ToString()  + "%";
		}
	}

	/// <summary>
	/// Serializable configuration for the Graph.
	/// </summary>
	[XmlRoot]
	public class GraphConfiguration
	{
		private GraphSource                _graphSource;
		private ItemsConfiguration         _itemsConfiguration;
		private AxisXConfiguration         _xConfiguration;
		private AxisYConfiguration         _yConfiguration;
		private LegendConfiguration        _legendConfiguration;
		private EnumWrapper<GraphTypeEnum> _graphType;
		private List<PaletteItem>          _palette;
		private EnumWrapper<DayOfWeek>     _firstWeekDay;
		private GraphMargins               _margin;

		/// <summary>
		/// Initializing object GraphConfiguration.
		/// </summary>
		public GraphConfiguration()
		{
			this._itemsConfiguration  = new ItemsConfiguration();
			this._xConfiguration      = new AxisXConfiguration();
			this._yConfiguration      = new AxisYConfiguration();
			this._legendConfiguration = new LegendConfiguration();
			this._graphType           = GraphTypeEnum.NameDateStackedColumn;
			this._palette             = new List<PaletteItem>();
			this._firstWeekDay        = DayOfWeek.Sunday;
			this._margin              = new GraphMargins();
		}

		/// <summary>
		/// Loads XML file to new GraphConfiguration instance
		/// </summary>
		/// <param name="fileName">FileName of the XML file to load</param>
		/// <returns>New GraphConfiguration loaded from file</returns>
		internal static GraphConfiguration LoadFromXml(string fileName)
		{
			using (FileStream reader = new FileStream(fileName, FileMode.Open))
			{
				return LoadFromXml(reader);
			}
		}

		/// <summary>
		/// Loads XML from stream to new GraphConfiguration instance
		/// </summary>
		/// <param name="stream">Xml configuration stream</param>
		/// <returns></returns>
		internal static GraphConfiguration LoadFromXml(Stream stream)
		{
			XmlSerializer s = new XmlSerializer(typeof (GraphConfiguration));

			using (XmlReader xmlReader = XmlReader.Create(stream, XmlUtils.GetXmlReaderSettings()))
			{
				return (GraphConfiguration) s.Deserialize(xmlReader);
			}
		}

		/// <summary>
		/// Saves the current configuration to XML file.
		/// </summary>
		/// <param name="fileName">FileName of the XML file to save</param>
		public void SaveToXml(string fileName)
		{

			XmlSerializer s = new XmlSerializer(typeof(GraphConfiguration));

			using (FileStream writer = new FileStream(fileName, FileMode.Create))
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(writer, XmlUtils.GetXmlWriterSettings()))
				{
					s.Serialize(xmlWriter, this);
				}
			}
		}

		/// <summary>
		/// Source data of the graph. GraphSource instance must have GetPlainData() method to provide PlainData for the graph
		/// </summary>
		[XmlElement]
		public GraphSource GraphSource
		{
			get { return this._graphSource; }
			set { this._graphSource = value; }
		}

		/// <summary>
		/// Axis X configuration
		/// </summary>
		[XmlElement]
		public AxisXConfiguration AxisXConfiguration
		{
			get { return this._xConfiguration; }
			set { this._xConfiguration = value; }
		}

		/// <summary>
		/// Axis Y configuration
		/// </summary>
		[XmlElement]
		public AxisYConfiguration AxisYConfiguration
		{
			get { return this._yConfiguration; }
			set { this._yConfiguration = value; }
		}

		/// <summary>
		/// Items configurations
		/// </summary>
		[XmlElement]
		public ItemsConfiguration ItemsConfiguration
		{
			get { return this._itemsConfiguration; }
			set { this._itemsConfiguration = value; }
		}

		/// <summary>
		/// Legend configuration
		/// </summary>
		[XmlElement]
		public LegendConfiguration LegendConfiguration
		{
			get { return this._legendConfiguration; }
			set { this._legendConfiguration = value; }
		}

		/// <summary>
		/// Type of the graph. For StackedColumn all three PlainItem properties should be filled properly.
		/// For other kinds of GraphTypes Value and (DateTime or Name) should be filled.
		/// </summary>
		[XmlElement]
		public EnumWrapper<GraphTypeEnum> GraphType
		{
			get { return this._graphType; }
			set { this._graphType = value; }
		}

		/// <summary>
		/// Palette for graph. Palette items is used cyclically
		/// </summary>
		[XmlArray]
		public List<PaletteItem> Palette
		{
			get { return this._palette; }
			set { this._palette = value; }
		}

		/// <summary>
		/// First day of the week
		/// </summary>
		[XmlElement]
		public EnumWrapper<DayOfWeek> FirstWeekDay
		{
			get { return this._firstWeekDay; }
			set { this._firstWeekDay = value; }
		}

		/// <summary>
		/// Margins for graph
		/// </summary>
		[XmlElement]
		public GraphMargins GraphMargins
		{
			get { return this._margin; }
			set { this._margin = value; }
		}
	}
}
