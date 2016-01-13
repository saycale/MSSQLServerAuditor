using System;
using MSSQLServerAuditor.SQLite.Common;

namespace MSSQLServerAuditor.SQLite.Tables.UserSettings
{
	public class ScheduleSettingsRow : AutoincrementTableRow
	{
		public ScheduleSettingsRow() : base(ScheduleSettingsTable.GetTableDefinition())
		{
		}

		public long TemplateNodeId
		{
			get
			{
				return this.GetValue<long>(ScheduleSettingsTable.TemplateNodeFk);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.TemplateNodeFk, value);
			}
		}

		public DateTime? LastRan
		{
			get
			{
				return this.GetValue<DateTime?>("LastRan");
			}
			set
			{
				this.SetValue("LastRan", value);
			}
		}

		public bool Enabled
		{
			get
			{
				return this.GetValue<bool> ("Enabled");
			}
			set
			{
				this.SetValue("Enabled", value);
			}
		}

		public string TemplateNodesScheduleUserId
		{
			get
			{
				return this.GetValue<string>(ScheduleSettingsTable.UserIdFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.UserIdFn, value);
			}
		}

		public string TemplateNodesScheduleUserName
		{
			get
			{
				return this.GetValue<string>(ScheduleSettingsTable.UserNameFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.UserNameFn, value);
			}
		}

		public bool IsSendMessage
		{
			get
			{
				return this.GetValue<bool>(ScheduleSettingsTable.SendMessageFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.SendMessageFn, value);
			}
		}

		public string MessageAddress
		{
			get
			{
				return this.GetValue<string>(ScheduleSettingsTable.MessageAddressFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.MessageAddressFn, value);
			}
		}

		public DateTime OccursOnceDateTime
		{
			get
			{
				return this.GetValue<DateTime>(ScheduleSettingsTable.OccursOnceDateTimeFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.OccursOnceDateTimeFn, value);
			}
		}

		public bool OccursOnceDateTimeEnabled
		{
			get
			{
				return this.GetValue<bool>(ScheduleSettingsTable.OccursOnceDateTimeEnabledFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.OccursOnceDateTimeEnabledFn, value);
			}
		}

		public string ActiveWeekDays
		{
			get
			{
				return this.GetValue<string>(ScheduleSettingsTable.ActiveWeekDaysFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.ActiveWeekDaysFn, value);
			}
		}

		public long DayNumber
		{
			get
			{
				return this.GetValue<long>(ScheduleSettingsTable.DayNumberFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.DayNumberFn, value);
			}
		}

		public DateTime StartDate
		{
			get
			{
				return this.GetValue<DateTime>(ScheduleSettingsTable.StartDateFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.StartDateFn, value);
			}
		}

		public DateTime? EndDate
		{
			get
			{
				return this.GetValue<DateTime?>(ScheduleSettingsTable.EndDateFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.EndDateFn, value);
			}
		}

		public long TimeUnit
		{
			get
			{
				return this.GetValue<long>(ScheduleSettingsTable.TimeUnitFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.TimeUnitFn, value);
			}
		}

		public long TimeUnitCount
		{
			get
			{
				return this.GetValue<long>(ScheduleSettingsTable.TimeUnitCountFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.TimeUnitCountFn, value);
			}
		}

		public long OccursOnceTime
		{
			get
			{
				return this.GetValue<long>(ScheduleSettingsTable.OccursOnceTimeFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.OccursOnceTimeFn, value);
			}
		}

		public long? PeriodTimeUnit
		{
			get
			{
				return this.GetValue<long?>(ScheduleSettingsTable.PeriodTimeUnitFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.PeriodTimeUnitFn, value);
			}
		}

		public long? PeriodTimeUnitCount
		{
			get
			{
				return this.GetValue<long?>(ScheduleSettingsTable.PeriodTimeUnitCountFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.PeriodTimeUnitCountFn, value);
			}
		}

		public long? StartingAt
		{
			get
			{
				return this.GetValue<long?>(ScheduleSettingsTable.StartingAtFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.StartingAtFn, value);
			}
		}

		public long? EndingAt
		{
			get
			{
				return this.GetValue<long?>(ScheduleSettingsTable.EndingAtFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.EndingAtFn, value);
			}
		}

		public bool OccursOnce
		{
			get
			{
				return this.GetValue<bool>(ScheduleSettingsTable.OccursOnceFn);
			}
			set
			{
				this.SetValue(ScheduleSettingsTable.OccursOnceFn, value);
			}
		}

		public DateTime DateUpdated
		{
			get
			{
				return this.GetValue<DateTime>("date_updated");
			}
			set
			{
				this.SetValue("date_updated", value);
			}
		}
	}
}
