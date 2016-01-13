using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace MSSQLServerAuditor.Utils
{
	internal static class StringHelpers
	{
		public static string AsParamName(this string name)
		{
			return "@" + name;
		}

		public static string AsDbName(this string name)
		{
			if (name == "*")
			{
				return name;
			}

			return string.Format("[{0}]", name);
		}

		public static string ToSqlOnOff(this Boolean value)
		{
			return value ? "ON" : "OFF";
		}

		public static string AsFk(this string name)
		{
			return name + "_id";
		}

		public static string AsSqlClausePair(this string name)
		{
			return string.Format("{0} = {1}", name.AsDbName(), name.AsParamName());
		}

		public static string AsValidSqlName(this string name)
		{
			Regex rgSpecChar = new Regex(@"[\~\!\@\#\$\%\^\&\*\(\)\{\}\?\/\=\-\.]");
			return rgSpecChar.Replace(name.Replace(" ", String.Empty), String.Empty);
		}

		public static string RemoveWhitespaces(this string st)
		{
			return st.Trim(' ', '\n', '\t', '\r');
		}

		public static string TrimmedOrEmpty(this string st)
		{
			return TrimmedOrDefault(st, string.Empty);
		}

		public static string TrimmedOrDefault(this string st, string def)
		{
			return string.IsNullOrEmpty(st)
				? def
				: st.RemoveWhitespaces();
		}

		public static string AsIndexName(this string st)
		{
			return st + "_idx";
		}

		public static string GetValidFileName(this string st)
		{
			return Path.GetInvalidFileNameChars()
				.Aggregate(st, (current, c) => current.Replace(c, '_'))
				.Replace(' ', '_');
		}

		public static string Join(this IEnumerable<string> strings, string separator)
		{
			return string.Join(separator, strings.ToArray());
		}

		private static readonly Regex _rgSpecChar = new Regex(@"[\~\!\@\#\$\%\^\&\*\(\)\{\}\?\/\=]", RegexOptions.Compiled);

		/// <summary>
		/// Delete spec chars for XML node name.
		/// </summary>
		/// <param name="currentString">Innser string.</param>
		/// <returns></returns>
		public static string DeleteSpecChars(this string currentString)
		{
			//Regex rgSpecChar = new Regex(@"[\~\!\@\#\$\%\^\&\*\(\)\{\}\?\/\=]", RegexOptions.Compiled);
			return _rgSpecChar.Replace(currentString.Replace(" ", String.Empty), String.Empty);
		}

		static public string FormatXml(this XmlDocument doc)
		{
			StringBuilder sb = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "  ";
			settings.NewLineChars = "\r\n";
			settings.NewLineHandling = NewLineHandling.Replace;
			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				doc.Save(writer);
			}
			return sb.ToString();
		}

		public static long ToSettingDate(this DateTime currentDate)
		{
			long retValue = 0L;

			retValue = currentDate.Year * 10000L + currentDate.Month * 100L + currentDate.Day;

			return retValue;
		}

		public static long ToSettingTime(this DateTime currentTime)
		{
			long retValue = 0L;

			retValue = currentTime.Hour * 10000 + currentTime.Minute * 100 + currentTime.Second;

			return retValue;
		}

		public static long ToSettingTime(this TimeSpan currentTime)
		{
			long retValue = 0L;

			retValue = currentTime.Hours * 10000L + currentTime.Minutes * 100L + currentTime.Seconds;

			return retValue;
		}

		public static long ToTicks(this TimeSpan currentTime)
		{
			return currentTime.Ticks;
		}

		public static TimeSpan FromTicks(this long ticks)
		{
			return new TimeSpan(ticks);
		}

		public static long? ToTicks(this TimeSpan? currentTime)
		{
			return !currentTime.HasValue ? (long?)null : currentTime.Value.Ticks;
		}

		public static TimeSpan? FromTicks(this long? ticks)
		{
			return ticks == null ? (TimeSpan?)null : new TimeSpan(ticks.Value);
		}

		public static long? ToSettingTime(this TimeSpan? currentTime)
		{
			if (!currentTime.HasValue)
			{
				return null;
			}

			long retValue = 0L;

			retValue = currentTime.Value.Hours * 10000L + currentTime.Value.Minutes * 100L + currentTime.Value.Seconds;

			return retValue;
		}

		public static DateTime FromSettingDate(this long currentTime)
		{
			DateTime tmpDateTime;

			DateTime.TryParseExact(currentTime.ToString(), "yyyyMMdd", CultureInfo.CurrentCulture,
				DateTimeStyles.None, out tmpDateTime);

			return tmpDateTime;
		}

		public static DateTime FromSettingTime(this long currentTime)
		{
			DateTime tmpDateTime;
			int      legnth     = currentTime.ToString().Length;
			int      zeroNumber = 6 - legnth;
			String   prefix     = String.Join("",Enumerable.Repeat("0", zeroNumber));

			DateTime.TryParseExact(prefix + currentTime, "HHmmss", CultureInfo.CurrentCulture,
				DateTimeStyles.None, out tmpDateTime);

			return new DateTime(tmpDateTime.TimeOfDay.Ticks);
		}
	}
}
