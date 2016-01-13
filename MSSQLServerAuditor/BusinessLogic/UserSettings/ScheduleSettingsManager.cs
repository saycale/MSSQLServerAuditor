using System;
using System.Collections.Generic;
using System.Linq;
using MSSQLServerAuditor.Cache;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.UserSettings;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.BusinessLogic.UserSettings
{
	public class ScheduleSettingsManager
	{
		private const string                ScheduleSettingsCacheKey = "ScheduleSettings";
		private readonly CurrentStorage     _currentStorage;
		private static readonly MemoryCache Cache;

		static ScheduleSettingsManager()
		{
			Cache = new MemoryCache();
		}

		public ScheduleSettingsManager(CurrentStorage currentStorage)
		{
			this._currentStorage = currentStorage;
		}

		public void SaveScheduleSettings(long templateNodeId, Job job)
		{
			ScheduleSettingsRow row = this.CreateRow(templateNodeId, job);

			this._currentStorage.ScheduleSettingsTable.InsertOrUpdateRowForSure(row);

			Cache.Remove(GetCacheKey(templateNodeId));
		}

		public List<TemplateNodeUpdateJob> LoadScheduleSettings(TemplateNodeInfo node, bool fromCache)
		{
			long                        templateNodeId = node.TemplateNodeId.GetValueOrDefault();
			List<TemplateNodeUpdateJob> result;

			if (fromCache)
			{
				result = Cache.GetOrAdd(
					GetCacheKey(templateNodeId),
					() => GetJobs(node)
				);
			}
			else
			{
				result = GetJobs(node);
			}

			return result;
		}

		public List<TemplateNodeUpdateJob> GetJobs(List<ScheduleSettingsRow> rows, TemplateNodeInfo node)
		{
			List<TemplateNodeUpdateJob> jobs = new List<TemplateNodeUpdateJob>();

			foreach (ScheduleSettingsRow row in rows)
			{
				if (row != null)
				{
					TemplateNodeUpdateJob job = this.CreateJob(row, node);

					jobs.Add(job);
				}
				else
				{
					jobs.Add(TemplateNodeUpdateJob.Empty);
				}
			}

			return jobs;
		}

		public List<TemplateNodeUpdateJob> GetJobs(TemplateNodeInfo node)
		{
			long templateNodeId = node.TemplateNodeId.GetValueOrDefault();

			List<ScheduleSettingsRow> rows = this._currentStorage.ScheduleSettingsTable
				.GetAllRowsByTemplateNodeId(templateNodeId);

			List<TemplateNodeUpdateJob> jobs = new List<TemplateNodeUpdateJob>();

			foreach (ScheduleSettingsRow row in rows)
			{
				if (row != null)
				{
					TemplateNodeUpdateJob job = this.CreateJob(row, node);

					jobs.Add(job);
				}
				else
				{
					jobs.Add(TemplateNodeUpdateJob.Empty);
				}
			}

			return jobs;
		}

		private static string GetCacheKey(long templateNodeId)
		{
			return ScheduleSettingsCacheKey + templateNodeId;
		}

		private TemplateNodeUpdateJob CreateJob(ScheduleSettingsRow row, TemplateNodeInfo node)
		{
			var scheduleSettings = new ScheduleSettings
			{
				ActiveWeekDays = row.ActiveWeekDays
					.Split(',')
					.Select(
						s =>
							{
								DayOfWeek result;

								if (Enum.TryParse(s, out result))
								{
									return result as DayOfWeek?;
								}

								return null;
							})
					.Where(d => d != null)
					.Select(d => d.Value).ToList(),
				Enabled = row.Enabled,
				Id                        = row.TemplateNodesScheduleUserId,
				Name                      = row.TemplateNodesScheduleUserName,
				IsSendMessage             = row.IsSendMessage,
				EndDate                   = row.EndDate.GetValueOrDefault(),
				OccursOnceDateTime        = row.OccursOnceDateTime,
				OccursOnceDateTimeEnabled = row.OccursOnceDateTimeEnabled,
				StartDate                 = row.StartDate,
				DayOfMonth = new DayOfMonthSettings
					{
						DayNumber = (int)row.DayNumber
					},
				ReccurPeriod = new TimeUnitBasedPeriod(
					(ReccurPeriodTimeUnit)row.TimeUnit,
					(int)row.TimeUnitCount),
				DailyFrequency =
					new DailyFrequency
						{
							EndingAt            = row.EndingAt.FromTicks(),
							OccursOnceTime      = row.OccursOnceTime.FromTicks(),
							PeriodTimeUnit      = row.PeriodTimeUnit.HasValue
								? (DailyFrequency.TimeUnit?)row.PeriodTimeUnit.Value
								: null,
							PeriodTimeUnitCount = (int?)row.PeriodTimeUnitCount,
							StartingAt          = row.StartingAt.FromTicks(),
							OccursOnce          = row.OccursOnce
						},
				HasEndDate = row.EndDate.HasValue
			};

			return new TemplateNodeUpdateJob
			{
				LastRan    = row.LastRan,
				Settings   = scheduleSettings,
				NodeInfo   = node,
				SettingsId = row.Identity
			};
		}

		private ScheduleSettingsRow CreateRow(long templateNodeId, Job job)
		{
			return new ScheduleSettingsRow
			{
				TemplateNodeId                = templateNodeId,
				TemplateNodesScheduleUserId   = job.Settings.Id,
				TemplateNodesScheduleUserName = job.Settings.Name,
				IsSendMessage                 = job.Settings.IsSendMessage,
				Enabled                       = job.Settings.Enabled,
				ActiveWeekDays                = string.Join(",", job.Settings.ActiveWeekDays),
				DayNumber                     = job.Settings.DayOfMonth.DayNumber,
				EndDate                       = job.Settings.HasEndDate ? (DateTime?)job.Settings.EndDate : null,
				EndingAt                      = job.Settings.DailyFrequency.EndingAt.ToTicks(),
				LastRan                       = job.LastRan,
				OccursOnceDateTime            = job.Settings.OccursOnceDateTime,
				OccursOnceDateTimeEnabled     = job.Settings.OccursOnceDateTimeEnabled,
				OccursOnceTime                = job.Settings.DailyFrequency.OccursOnceTime.ToTicks(),
				PeriodTimeUnit                = job.Settings.DailyFrequency.PeriodTimeUnit.HasValue ? (int?)job.Settings.DailyFrequency.PeriodTimeUnit.Value : null,
				PeriodTimeUnitCount           = job.Settings.DailyFrequency.PeriodTimeUnitCount,
				StartDate                     = job.Settings.StartDate,
				StartingAt                    = job.Settings.DailyFrequency.StartingAt.ToTicks(),
				TimeUnit                      = job.Settings.ReccurPeriod.With(r => (int)r.TimeUnit),
				TimeUnitCount                 = job.Settings.ReccurPeriod.With(r => r.TimeUnitCount),
				OccursOnce                    = job.Settings.DailyFrequency.OccursOnce
			};
		}
	}
}
