using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSSQLServerAuditor.Model.Scheduling;

namespace AutomatedTestProject
{
	[TestClass]
	public class TestJobSchedulling
	{
		[TestMethod]
		public void TestMethod1()
		{
			AbstractJobProcessor jobProcessor = new SimpleJobProcessor();
			var ranCount = 0;

			var job = new Job();
			jobProcessor.JobStarted += delegate { ranCount++; };

			job.Settings = new ScheduleSettings();
			job.Settings.StartDate = new DateTime(1980, 2, 26);
			job.Settings.EndDate = new DateTime(2980, 2, 26);
			job.Settings.HasEndDate = true;
			job.Settings.SetFrequency(ReccurPeriodTimeUnit.Daily, 1);

			job.Settings.DailyFrequency.PeriodTimeUnit = DailyFrequency.TimeUnit.Hour;
			job.Settings.DailyFrequency.PeriodTimeUnitCount = 1;

			jobProcessor.RunIfTime(job, new DateTime(year: 1980, month: 2, day: 25, hour: 23, minute: 30, second: 0));
			Assert.AreEqual(0, ranCount);

			jobProcessor.RunIfTime(job, new DateTime(1980, 2, 26, 0, 30, 0));
			Assert.AreEqual(1, ranCount);
			jobProcessor.RunIfTime(job, new DateTime(1980, 2, 26, 0, 30, 0));
			Assert.AreEqual(1, ranCount);

			jobProcessor.RunIfTime(job, new DateTime(1980, 2, 26, 1, 0, 0));
			Assert.AreEqual(2, ranCount);

			jobProcessor.RunIfTime(job, new DateTime(1979, 2, 26, 0, 30, 0));
			Assert.AreEqual(2, ranCount);

			jobProcessor.RunIfTime(job, new DateTime(2000, 2, 26, 0, 30, 0));
			Assert.AreEqual(3, ranCount);

			DateTime nextDateTime;

			Assert.IsTrue(job.Settings.GetNextTime(new DateTime(1980, 2, 25), out nextDateTime));
			Assert.AreEqual(new DateTime(1980, 2, 26), nextDateTime);

			Assert.IsTrue(job.Settings.GetNextTime(new DateTime(1980, 2, 26), out nextDateTime));
			Assert.AreEqual(new DateTime(1980, 2, 26, 1, 0, 0), nextDateTime);

			Assert.IsFalse(job.Settings.GetNextTime(new DateTime(2980, 2, 27, 0, 0, 0), out nextDateTime));
		}

		[TestMethod]
		public void TestComplicated()
		{
			AbstractJobProcessor jobProcessor = new SimpleJobProcessor();
			var job = new Job();

			var runTimes = new List<DateTime>();
			jobProcessor.JobStarted += (object s, JobEventArgs a) => runTimes.Add(a.DateTime);

			job.Settings = new ScheduleSettings();
			job.Settings.StartDate = new DateTime(2014, 11, 20);
			job.Settings.SetFrequency(ReccurPeriodTimeUnit.Weekly, 1);
			job.Settings.ActiveWeekDays.Add(DayOfWeek.Monday);
			job.Settings.ActiveWeekDays.Add(DayOfWeek.Wednesday);

			job.Settings.DailyFrequency.PeriodTimeUnit = DailyFrequency.TimeUnit.Hour;
			job.Settings.DailyFrequency.PeriodTimeUnitCount = 7;
			job.Settings.DailyFrequency.EndingAt = new TimeSpan(hours: 20, minutes: 0, seconds: 0);

			job.Settings.EndDate = new DateTime(2015, 5, 10);
			job.Settings.HasEndDate = true;

			var allDates = new HashSet<DateTime>();

			for (var dt = new DateTime(2014, 10, 20).AddMinutes(-1);
				dt < new DateTime(2015, 10, 2); dt = dt.AddMinutes(15))
			{
				jobProcessor.RunIfTime(job, dt);

				if (dt < job.Settings.StartDate)
					continue;

				if (job.Settings.ActiveWeekDays.Contains(dt.DayOfWeek) && !allDates.Contains(dt.Date)
					&& dt <= job.Settings.EndDate
					&& dt.TimeOfDay <= job.Settings.DailyFrequency.EndingAt.Value)
				{
					allDates.Add(dt.Date);
				}
			}

			var runDates = runTimes.Select(dt => dt.Date).Distinct().ToList();

			Assert.IsTrue(allDates.SequenceEqual(runDates));
			Assert.AreEqual(runDates.Count * 3, runTimes.Count);
		}

		[TestMethod]
		public void TestDailyHourly()
		{
			AbstractJobProcessor jobProcessor = new SimpleJobProcessor();

			var runTimes = new List<DateTime>();
			var job = new Job();
			jobProcessor.JobStarted += (object s, JobEventArgs a) => runTimes.Add(a.DateTime);

			job.Settings = new ScheduleSettings();
			job.Settings.StartDate = new DateTime(2014, 11, 20);
			job.Settings.ReccurPeriod = new TimeUnitBasedPeriod(ReccurPeriodTimeUnit.Daily, 1);

			job.Settings.DailyFrequency.PeriodTimeUnit = DailyFrequency.TimeUnit.Hour;
			job.Settings.DailyFrequency.PeriodTimeUnitCount = 1;

			for (var dt = new DateTime(2014, 11, 20); dt < new DateTime(2014, 11, 23); dt = dt.AddMinutes(0.5))
			{
				jobProcessor.RunIfTime(job, dt);
			}

			Assert.AreEqual(3 * 24, runTimes.Count);
		}

		[TestMethod]
		public void TestMonthly()
		{
			AbstractJobProcessor jobProcessor = new SimpleJobProcessor();

			var runs = new List<DateTime>();
			var job = new Job();
			jobProcessor.JobStarted += (object s, JobEventArgs a) => runs.Add(a.DateTime);

			job.Settings = new ScheduleSettings();
			job.Settings.StartDate = new DateTime(2014, 11, 20);
			job.Settings.ReccurPeriod = new TimeUnitBasedPeriod(ReccurPeriodTimeUnit.Monthly, 2);
			job.Settings.DayOfMonth.DayNumber = 15;

			job.Settings.DailyFrequency.OccursOnceTime = new TimeSpan(hours: 8, minutes: 0, seconds: 0);
			job.Settings.DailyFrequency.OccursOnce = true;

			jobProcessor.RunIfTime(job, new DateTime(2014, 11, 20));
			Assert.AreEqual(0, runs.Count);
			jobProcessor.RunIfTime(job, new DateTime(2015, 1, 15, 7, 0, 0));
			Assert.AreEqual(0, runs.Count);

			DateTime nextRun;
			Assert.IsTrue(job.Settings.GetNextTime(new DateTime(2015, 1, 15, 7, 0, 0), out nextRun));
			Assert.AreEqual(new DateTime(2015, 1, 15, 8, 0, 0), nextRun);

			Assert.IsTrue(job.Settings.GetNextTime(new DateTime(2015, 1, 14, 8, 0, 0), out nextRun));
			Assert.AreEqual(new DateTime(2015, 1, 15, 8, 0, 0), nextRun);

			Assert.IsTrue(job.Settings.GetNextTime(new DateTime(2015, 1, 14, 9, 0, 0), out nextRun));
			Assert.AreEqual(new DateTime(2015, 1, 15, 8, 0, 0), nextRun);

			jobProcessor.RunIfTime(job, new DateTime(2015, 1, 15, 9, 0, 0));
			Assert.AreEqual(1, runs.Count);
			jobProcessor.RunIfTime(job, new DateTime(2015, 1, 20, 0, 0, 0));
			Assert.AreEqual(1, runs.Count);
			jobProcessor.RunIfTime(job, new DateTime(2015, 1, 20, 10, 0, 0));
			Assert.AreEqual(1, runs.Count);
		}

		[TestMethod]
		public void TestRunOnce()
		{
			var job = new Job();

			job.Settings = new ScheduleSettings();
			job.Settings.OccursOnceDateTime = new DateTime(2015, 1, 15, 9, 0, 0);
			job.Settings.OccursOnceDateTimeEnabled = true;

			DateTime next;
			Assert.IsTrue(job.Settings.GetNextTime(new DateTime(2015, 1, 15, 8, 0, 0), out next));
			Assert.AreEqual(new DateTime(2015, 1, 15, 9, 0, 0), next);
			Assert.IsFalse(job.Settings.GetNextTime(new DateTime(2015, 1, 15, 10, 0, 0), out next));
		}

		[TestMethod]
		public void TestOvernight()
		{
			AbstractJobProcessor jobProcessor = new SimpleJobProcessor();

			var runs = new List<DateTime>();
			var job = new Job();
			jobProcessor.JobStarted += (object s, JobEventArgs a) => runs.Add(a.DateTime);

			job.Settings = new ScheduleSettings();
			job.Settings.StartDate = new DateTime(2014, 11, 20);
			job.Settings.ReccurPeriod = new TimeUnitBasedPeriod(ReccurPeriodTimeUnit.Daily, 1);

			job.Settings.DailyFrequency.PeriodTimeUnit = DailyFrequency.TimeUnit.Minute;
			job.Settings.DailyFrequency.PeriodTimeUnitCount = 1;

			DateTime next;
			Assert.IsTrue(job.Settings.GetNextTime(new DateTime(2014, 11, 21, 23, 58, 30), out next));
			Assert.AreEqual(new DateTime(2014, 11, 21, 23, 59, 0), next);

			Assert.IsTrue(job.Settings.GetNextTime(new DateTime(2014, 11, 21, 23, 59, 30), out next));
			Assert.AreEqual(new DateTime(2014, 11, 22, 0, 0, 0), next);

			job.Settings.DailyFrequency.StartingAt = TimeSpan.FromSeconds(5);
			job.Settings.DailyFrequency.EndingAt = new TimeSpan(23, 59, 59, 59, 9999);

			Assert.IsTrue(job.Settings.GetNextTime(new DateTime(2014, 11, 21, 23, 58, 30), out next));
			Assert.AreEqual(new DateTime(2014, 11, 21, 23, 59, 05), next);

			Assert.IsTrue(job.Settings.GetNextTime(new DateTime(2014, 11, 21, 23, 59, 30), out next));
			Assert.AreEqual(new DateTime(2014, 11, 22, 0, 0, 05), next);

			for (var dt = new DateTime(2014, 11, 21, 23, 57, 30);
				dt < new DateTime(2014, 11, 22, 0, 1, 30);
				dt = dt.AddSeconds(1))
			{
				jobProcessor.RunIfTime(job, dt);
			}

			Assert.AreEqual(5, runs.Count);
			Assert.AreEqual(runs.Last().Date, new DateTime(2014, 11, 22));

			runs.Clear();

			// every seccond for several days
			for (var dt = new DateTime(2014, 11, 22, 23, 57, 30);
				dt < new DateTime(2014, 12, 15, 9, 10, 30);
				dt = dt.AddSeconds(1))
			{
				jobProcessor.RunIfTime(job, dt);
			}

			var runDated = runs.Select(t => t.Date).Distinct().ToList();

			//Assert.AreEqual(`);

			Assert.AreEqual(runs.Last().Date, new DateTime(2014, 12, 15));

			for (var i = 0; i < runs.Count - 1; i++)
			{
				Assert.IsTrue(runs[i + 1] - runs[i] < new TimeSpan(0, 1, 1));
			}
		}
	}
}
