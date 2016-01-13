using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables.UserSettings
{
	public class ScheduleSettingsTable : CurrentStorageTable
	{
		private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public const string TableName                   = "d_TemplateNodeSchedule_UserSettings";
		public const string TableIdentityField          = "d_TemplateNodeSchedule_UserSettings_id";
		public static readonly string TemplateNodeFk    = TemplateNodeDirectory.TableName.AsFk();
		public const string UserIdFn                    = "UserId";
		public const string UserNameFn                  = "UserName";
		public const string SendMessageFn               = "SendMessage";
		public const string MessageAddressFn            = "MessageAddress";
		public const string EnabledFn                   = "Enabled";
		public const string OccursOnceFn                = "OccursOnce";
		public const string OccursOnceDateTimeFn        = "OccursOnceDateTime";
		public const string OccursOnceDateTimeEnabledFn = "OccursOnceDateTimeEnabled";
		public const string ActiveWeekDaysFn            = "ActiveWeekDays";
		public const string DayNumberFn                 = "DayNumber";
		public const string StartDateFn                 = "StartDate";
		public const string EndDateFn                   = "EndDate";
		public const string TimeUnitFn                  = "TimeUnit";
		public const string TimeUnitCountFn             = "TimeUnitCount";
		public const string OccursOnceTimeFn            = "OccursOnceTime";
		public const string PeriodTimeUnitFn            = "PeriodTimeUnit";
		public const string PeriodTimeUnitCountFn       = "PeriodTimeUnitCount";
		public const string StartingAtFn                = "StartingAt";
		public const string EndingAtFn                  = "EndingAt";
		public const string LastRanFn                   = "LastRan";

		public ScheduleSettingsTable(CurrentStorage storage)
			: base(
				storage,
				GetTableDefinition()
			)
		{
		}

		public static TableDefinition GetTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddBigIntField(  TemplateNodeFk,              true,  true)
				.AddNVarCharField(UserIdFn,                    true,  true)
				.AddNVarCharField(UserNameFn,                  false, false)
				.AddBitField(     SendMessageFn,               false, false)
				.AddNVarCharField(MessageAddressFn,            false, false)
				.AddBitField(     EnabledFn,                   false, false)
				.AddBitField(     OccursOnceFn,                false, false)
				.AddDateTimeField(OccursOnceDateTimeFn,        false, false)
				.AddBitField(     OccursOnceDateTimeEnabledFn, false, false)
				.AddNVarCharField(ActiveWeekDaysFn,            false, false)
				.AddIntField(     DayNumberFn,                 false, false)
				.AddDateTimeField(StartDateFn,                 false, false)
				.AddDateTimeField(EndDateFn,                   false, false)
				.AddIntField(     TimeUnitFn,                  false, false)
				.AddIntField(     TimeUnitCountFn,             false, false)
				.AddBigIntField(  OccursOnceTimeFn,            false, false)
				.AddIntField(     PeriodTimeUnitFn,            false, false)
				.AddIntField(     PeriodTimeUnitCountFn,       false, false)
				.AddBigIntField(  StartingAtFn,                false, false)
				.AddBigIntField(  EndingAtFn,                  false, false)
				.AddDateTimeField(LastRanFn,                   false, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public List<ScheduleSettingsRow> GetAllRowsByTemplateNodeId(long templateNodeId)
		{
			List<ScheduleSettingsRow> rv =
				this.GetRows(
					string.Format(
						"{0}={1}",
						TemplateNodeFk,
						templateNodeId
					)
				)
				.Select(RowConverter.Convert<ScheduleSettingsRow>).ToList();

			return rv;
		}

		public List<ScheduleSettingsRow> GetAllRows()
		{
			List<ScheduleSettingsRow> rv =
				this.GetRows(
					string.Format(
						"[{0}]=1",
						EnabledFn
					)
				)
				.Select(RowConverter.Convert<ScheduleSettingsRow>).ToList();

			return rv;
		}

		public ScheduleSettingsRow GetByTemplateNodeId(long templateNodeId, string strTemplateNodesScheduleUserId)
		{
			return this
				.GetRows(
					string.Format(
						"[{0}]={1} AND [{2}]='{3}'",
						TemplateNodeFk,
						templateNodeId,
						UserIdFn,
						strTemplateNodesScheduleUserId
					)
				)
				.Select(RowConverter.Convert<ScheduleSettingsRow>)
				.FirstOrDefault();
		}

		public void SaveLastRun(long settingsId, DateTime lastRun)
		{
			string sqlQuery = string.Format(
				"UPDATE [{0}] " +
				"SET {1} = {2} " +
				"WHERE {3} AND ({1} IS NULL OR {1} != {2});",
				TableName,
				LastRanFn.AsDbName(),
				LastRanFn.AsParamName(),
				TableIdentityField.AsSqlClausePair()
			);

			using (SQLiteCommand cmd = new SQLiteCommand(
				sqlQuery,
				this.Connection
			))
			{
				cmd.Parameters.AddRange(new[]
				{
					new SQLiteParameter(LastRanFn.AsParamName(),          lastRun),
					new SQLiteParameter(TableIdentityField.AsParamName(), settingsId),
				});

				try
				{
					cmd.ExecuteNonQuery();
				}
				catch (SQLiteException ex)
				{
					_Log.ErrorFormat("SQLite exception:'{0}'",
						ex
					);
				}
			}
		}
	}
}
