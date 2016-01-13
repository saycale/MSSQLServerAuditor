using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Serialization;
using MSSQLServerAuditor.Entities;

namespace MSSQLServerAuditor.Model.Scheduling
{
	[Serializable]
	public class ScheduleSettings
	{
		[XmlElement("Enabled")]
		public bool Enabled { get; set; }

		[XmlElement("Id")]
		public string Id { get; set; }

		[XmlElement("Name")]
		public string Name { get; set; }

		[XmlElement("IsSendMessage")]
		public bool IsSendMessage { get; set; }

		public DateTime OccursOnceDateTime { get; set; }

		public bool OccursOnceDateTimeEnabled { get; set; }

		public TimeUnitBasedPeriod ReccurPeriod { get; set; }

		public DailyFrequency DailyFrequency { get; set; }

		public List<DayOfWeek> ActiveWeekDays { get; set; }

		public DayOfMonthSettings DayOfMonth { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public bool HasEndDate { get; set; }

		public ScheduleSettings()
		{
			this.Enabled        = true;
			this.ActiveWeekDays = new List<DayOfWeek>();
			this.ReccurPeriod   = null;
			this.DailyFrequency = new DailyFrequency();
			this.DayOfMonth     = new DayOfMonthSettings();
		}

		public bool Recuring
		{
			get { return !this.OccursOnceDateTimeEnabled; }
		}

		public void SetFrequency(ReccurPeriodTimeUnit reccurPeriodTimeUnit, int timeUnitCount)
		{
			this.OccursOnceDateTimeEnabled = false;
			this.ReccurPeriod              = new TimeUnitBasedPeriod(reccurPeriodTimeUnit, timeUnitCount);
		}

		internal bool GetNextRecurDate(DateTime originDate, out DateTime nextDate)
		{
			if (!this.Recuring)
			{
				throw new InvalidOperationException();
			}

			if (this.ReccurPeriod == null)
			{
				throw new InvalidOperationException();
			}

			switch (ReccurPeriod.TimeUnit)
			{
				case ReccurPeriodTimeUnit.Daily:
					nextDate = originDate.Date.AddDays(ReccurPeriod.TimeUnitCount);
					return !HasEndDate || nextDate <= EndDate;

				case ReccurPeriodTimeUnit.Weekly:
					nextDate = new DateTime();

					if (ActiveWeekDays.Count == 0)
					{
						return false;
					}

					// this week
					var day = originDate.Date;

					while (day.Date.AddDays(1).DayOfWeek > day.DayOfWeek)
					{
						day = day.AddDays(1);

						if (ActiveWeekDays.Contains(day.DayOfWeek))
						{
							nextDate = day;
							return !HasEndDate || nextDate <= EndDate;
						}
					}

					const int daysInWeek = 7;
					//First day of next active week
					day = day.AddDays(1 + daysInWeek*(ReccurPeriod.TimeUnitCount - 1));

					do
					{
						if (ActiveWeekDays.Contains(day.DayOfWeek))
						{
							nextDate = day;
							return !HasEndDate || nextDate <= EndDate;
						}

						day = day.AddDays(1);
					} while (day.DayOfWeek > day.AddDays(-1).DayOfWeek);

					Debug.Fail("");
					return false;

				case ReccurPeriodTimeUnit.Monthly:
					nextDate = DayOfMonth.GetDateOfThisMonth(originDate);

					if (nextDate <= originDate)
					{
						var nextMonth = DayOfMonthSettings.BeginingOfMonth(nextDate)
							.AddMonths(ReccurPeriod.TimeUnitCount);

						nextDate = DayOfMonth.GetDateOfThisMonth(nextMonth);
					}
					return !HasEndDate || nextDate <= EndDate;

				default:
					throw new Exception("Unforseen enum value");
			}
		}

		public bool GetNextTime(DateTime dateTime, out DateTime nextDateTime)
		{
			nextDateTime = new DateTime();

			if (!this.Enabled)
			{
				return false;
			}

			if (!Recuring && OccursOnceDateTimeEnabled)
			{
				if (!(dateTime <= OccursOnceDateTime))
				{
					return false;
				}

				nextDateTime = OccursOnceDateTime;

				return true;
			}

			var searchOriginDateTime = dateTime > StartDate ? dateTime : StartDate;
			var newDay               = !(dateTime > StartDate);

			// Checking if "date" is sutable to run

			DateTime nextDayAfterYestarday;

			// if there is no suitable day after search origin at all
			if (!GetNextRecurDate(searchOriginDateTime.AddDays(-1), out nextDayAfterYestarday))
			{
				return false;
			}

			var candidateDateTime = nextDayAfterYestarday > searchOriginDateTime
				? nextDayAfterYestarday.Date
				: searchOriginDateTime;

			do
			{
				TimeSpan candidateTime;

				while (!DailyFrequency.GetNextTime(candidateDateTime.TimeOfDay, newDay, out candidateTime))
				{
					if (!GetNextRecurDate(candidateDateTime.Date, out candidateDateTime))
					{
						return false;
					}

					newDay = true;
				}

				candidateDateTime = candidateDateTime.Date + candidateTime;

				if (HasEndDate && candidateDateTime > EndDate.Date.AddDays(1))
				{
					// task should be able to run all last day
					return false;
				}

				newDay = false; // candidateTime == DailyFrequency.DayLength;
			} while (candidateDateTime <= dateTime);

			nextDateTime = candidateDateTime;

			return true;
		}

		public ScheduleSettings Clone()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream     stream    = new MemoryStream();

			using (stream)
			{
				formatter.Serialize(stream, this);
				stream.Seek(0, SeekOrigin.Begin);
				return (ScheduleSettings) formatter.Deserialize(stream);
			}
		}
	}

	/// <summary>
	/// Class RefreshSchedule is required to solve naming incomatibility between class name and xml node name
	/// (xml node corresponding to class ScheduleSettings named as "RefreshSchedule")
	/// </summary>
	[Serializable]
	public class RefreshSchedule:ScheduleSettings
	{
		public RefreshSchedule():base()
		{
		}
	}
}
