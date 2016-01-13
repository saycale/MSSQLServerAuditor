using System;

namespace MSSQLServerAuditor.Model.Scheduling
{
	[Serializable]
	public class TemplateNodeUpdateJob : Job
	{
		public static readonly TemplateNodeUpdateJob Empty = new TemplateNodeUpdateJob();

		public bool IsEmpty()
		{
			return this == Empty;
		}
	}
}
