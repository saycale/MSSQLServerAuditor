namespace MSSQLServerAuditor.Model.Connections
{
	public class EventLogConnection
	{
		public EventLogConnection(string machineName)
		{
			this.MachineName = machineName;
		}

		public string MachineName { get; private set; }
	}
}
