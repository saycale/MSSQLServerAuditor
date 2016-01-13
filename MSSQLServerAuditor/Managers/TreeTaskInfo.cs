using MSSQLServerAuditor.Gui;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.Managers
{
	public class TreeTaskInfo
	{
		public ConnectionData     Connection     { get; set; }
		public bool               Hierarchically { get; set; }
		public NodeUpdatingSource Mode           { get; set; }
		public long               Handle         { get; set; }
		public string             Note           { get; set; }
	}
}
