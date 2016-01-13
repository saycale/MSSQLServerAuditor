using System;
using System.Collections.Generic;

namespace MSSQLServerAuditor.Graph
{
	class GanttGraphData
	{
		private readonly GraphConfiguration _configuration;
		private readonly List<GanttJobItem> _data;

		public IList<GanttJobItem> Jobs
		{
			get
			{
				return this._data.AsReadOnly();
			}
		}

		public GanttGraphData(GraphConfiguration configuration, object context)
		{
			this._configuration = configuration;
			this._data          = this._configuration.GraphSource.GetGanttData(context);
		}
	}

	class GraphData
	{
		private readonly GraphConfiguration                      _configuration;
		private readonly Dictionary<string, List<PlainItem>>     _plainData;
		private Dictionary<String, Dictionary<DateTime, double>> _internalDictionary;
		private List<string>                                     _names;
		private DateTime                                         _fromDate;
		private DateTime                                         _toDate;

		internal GraphData(GraphConfiguration configuration, object context)
		{
			this._configuration = configuration;
			this._plainData     = this._configuration.GraphSource.GetPlainData(context);

			if ((configuration.GraphType == GraphTypeEnum.NameDateStackedColumn) || (configuration.GraphType == GraphTypeEnum.NameDateLine) || (configuration.GraphType == GraphTypeEnum.NamedLine))
			{
				ParseData();
			}
		}

		private void ParseData()
		{
			this._internalDictionary = new Dictionary<string, Dictionary<DateTime, double>>();
			this._fromDate           = DateTime.MaxValue;
			this._toDate             = DateTime.MinValue;
			this._names              = new List<string>(_internalDictionary.Keys);

			foreach (KeyValuePair<string, List<PlainItem>> pair in _plainData)
			{
				foreach (PlainItem item in pair.Value)
				{
					DateTime date = item.DateTime;

					if (!this._internalDictionary.ContainsKey(item.Name))
					{
						this._internalDictionary.Add(item.Name, new Dictionary<DateTime, double>());
					}

					if (!this._names.Contains(item.Name))
					{
						this._names.Add(item.Name);
					}

					var dateDict = this._internalDictionary[item.Name];

					if (!dateDict.ContainsKey(date))
					{
						dateDict.Add(date, 0);
					}

					dateDict[date] += item.YValue;

					if (date > _toDate)
					{
						this._toDate = date;
					}

					if (date < _fromDate)
					{
						this._fromDate = date;
					}
				}

				if (this._configuration.ItemsConfiguration.NameSortType != NameSortEnum.None)
				{
					this._names.Sort((s, s1) =>
						{
							switch (_configuration.ItemsConfiguration.NameSortType.Value)
							{
								case NameSortEnum.NameAscending:
									return s1.CompareTo(s);
								case NameSortEnum.NameDescending:
									return s.CompareTo(s1);
								default:
									throw new ArgumentException(
										"Invalid NameSortType in ParseData",
										_configuration.ItemsConfiguration.NameSortType.ToString()
									);
							}
						}
					);
				}
			}
		}

		public DateTime FromDate
		{
			get { return this._fromDate; }
		}

		public DateTime ToDate
		{
			get { return this._toDate; }
		}

		public List<string> Names
		{
			get { return this._names; }
		}

		public GraphConfiguration Configuration
		{
			get { return this._configuration; }
		}

		public Dictionary<string, List<PlainItem>> PlainData
		{
			get { return this._plainData; }
		}

		public double? GetValueForNameAndDate(string name, DateTime date, UnitEnum unit)
		{
			var dateDict = this._internalDictionary[name];

			if (dateDict.ContainsKey(date))
			{
				return FormatNumericValue(unit, dateDict[date]);
			}

			return null;
		}

		public double? GetValueForNameAndDate(string name, DateTime date, UnitEnum unit, ValGroup sort)
		{
			var dateDict     = this._internalDictionary[name];
			var IsDateInKeys = dateDict.ContainsKey(date);

			foreach (var item in dateDict)
			{
				if (sort == ValGroup.Day ||
					sort == ValGroup.Month ||
					sort == ValGroup.Week
				)
				{
					IsDateInKeys = item.Key.Date == date.Date;
				}

				if (IsDateInKeys)
				{
					break;
				}
			}

			DateTime existDate = DateTime.MinValue;

			foreach (var i in dateDict.Keys)
			{
				existDate = i;

				switch (sort)
				{
					case ValGroup.Month:
						if (existDate.Date.ToString("yyyy.MM") == date.Date.ToString("yyyy.MM"))
						{
							if (IsDateInKeys)
							{
								return FormatNumericValue(unit, dateDict[existDate]);
							}
							else
							{
								return -1.0;
							}
						}
						break;

					case ValGroup.Day:
						if (existDate.Date.ToString("yyyy.MM.dd") == date.Date.ToString("yyyy.MM.dd"))
						{
							if (IsDateInKeys)
							{
								return FormatNumericValue(unit, dateDict[existDate]);
							}
						}
						break;

					case ValGroup.Week:
						if (CheckOfWeek(date, existDate))
						{
							if (IsDateInKeys)
							{
								return FormatNumericValue(unit, dateDict[existDate]);
							}
							else
							{
								return -1.0;
							}
						}
						break;

					case ValGroup.Hour:
						if (existDate.ToString("yyyy.MM.dd HH") == date.ToString("yyyy.MM.dd HH"))
						{
							return FormatNumericValue(unit, dateDict[existDate]);
						}
						break;

					case ValGroup.Minute:
						if (existDate.ToString("yyyy.MM.dd HH:mm") == date.ToString("yyyy.MM.dd HH:mm"))
						{
							return FormatNumericValue(unit, dateDict[existDate]);
						}
						break;

					case ValGroup.Second:
						if (existDate.ToString("yyyy.MM.dd HH:mm:ss") == date.ToString("yyyy.MM.dd HH:mm:ss"))
						{
							return FormatNumericValue(unit, dateDict[existDate]);
						}
						break;
				}
			}

			return null;
		}

		#region Work with sort week

		private static bool CheckOfWeek(DateTime date, DateTime existDate)
		{
			if ((existDate.Date > date.Date ?
				 existDate.Date - date.Date :
				 date.Date - existDate.Date) < new TimeSpan(7, 0, 0, 0)
			)
			{
				if (existDate.Date > date.Date)
				{
					return DayOfWeekNumber(existDate) - DayOfWeekNumber(date) >= 0;
				}
				else
				{
					return DayOfWeekNumber(date) - DayOfWeekNumber(existDate) >= 0;
				}
			}

			return false;
		}

		private static int DayOfWeekNumber(DateTime existDate)
		{
			int iExDaOfWeek = -1;

			switch (existDate.DayOfWeek)
			{
				case DayOfWeek.Monday: iExDaOfWeek = 0;
					break;
				case DayOfWeek.Tuesday: iExDaOfWeek = 1;
					break;
				case DayOfWeek.Wednesday: iExDaOfWeek = 2;
					break;
				case DayOfWeek.Thursday: iExDaOfWeek = 3;
					break;
				case DayOfWeek.Friday: iExDaOfWeek = 4;
					break;
				case DayOfWeek.Saturday: iExDaOfWeek = 5;
					break;
				case DayOfWeek.Sunday: iExDaOfWeek = 6;
					break;
			}

			return iExDaOfWeek;
		}

		#endregion

		public double? GetValueForNameAndDateInterpolated(string name, DateTime date, UnitEnum unit)
		{
			date = date.Date;

			DateTime prevDate  = date;
			DateTime nextDate  = date;
			double?  prevValue = null;
			double?  nextValue = null;

			do
			{
				prevDate  = prevDate.AddDays(-1);
				prevValue = GetValueForNameAndDate(name, prevDate, unit);
			} while ((prevValue == null) && (prevDate >= FromDate));

			do
			{
				nextDate = nextDate.AddDays(+1);
				nextValue = GetValueForNameAndDate(name, nextDate, unit);
			} while ((nextValue == null) && (nextDate <= ToDate));

			if ((nextValue != null) && (prevValue != null))
			{
				return prevValue + (nextValue - prevValue)/((nextDate - prevDate).TotalDays*(date - prevDate).TotalDays);
			}

			return null;
		}

		public static double FormatNumericValue(UnitEnum unit, double value)
		{
			switch (unit)
			{
				case UnitEnum.Number:
					return value;

				case UnitEnum.Byte:
					return value;
				case UnitEnum.Kilobyte:
					return value / 1024.0;
				case UnitEnum.Megabyte:
					return value / (1024.0 * 1024.0);
				case UnitEnum.Gigabyte:
					return value / (1024.0 * 1024.0 * 1024.0);

				case UnitEnum.Second:
					return value;
				case UnitEnum.Minute:
					return value / 60.0;
				case UnitEnum.Hour:
					return value / 3600.0;

				default:
					throw new ArgumentException("Invalid Unit in GetValueForNameAndDate", unit.ToString());
			}
		}
	}
}
