using System;

namespace MSSQLServerAuditor.Model.Scheduling
{
	public class Job
	{
		public DateTime?        LastRan    { get; set; }
		public ScheduleSettings Settings   { get; set; }
		public TemplateNodeInfo NodeInfo   { get; set; }
		public long             SettingsId { get; set; }
		public bool             Enabled    { get { return this.Settings.Enabled; } }
	}
}
