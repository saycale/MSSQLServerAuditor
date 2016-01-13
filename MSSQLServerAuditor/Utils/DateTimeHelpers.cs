using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSQLServerAuditor.Utils
{
	/// <summary>
	/// Convert DateTime in String
	/// </summary>
	public static class DateTimeHelpers
	{
		/// <summary>
		/// Convert DateTime to a String in the format 'yyyy-MM-dddTHH:mm:ss'.
		/// </summary>
		/// <param name="dateTime">Object DateTime.</param>
		/// <returns>String DateTime.</returns>
		public static string ToInternetString(this DateTime dateTime)
		{
			return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
		}

		public static DateTime PreviousMonday(this DateTime dt, bool preserveTime = true)
		{
			return dt.PreviousDay(DayOfWeek.Monday, preserveTime);
		}

		public static DateTime PreviousDay(this DateTime dt, DayOfWeek day, bool preserveTime = true)
		{
			int diff = dt.DayOfWeek - day;

			if (diff < 0)
			{
				diff += 7;
			}

			DateTime previousDay = dt.AddDays(-1 * diff);

			return preserveTime ? previousDay : previousDay.Date;
		}
	}
}
