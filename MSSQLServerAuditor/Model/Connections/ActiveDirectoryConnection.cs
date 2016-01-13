namespace MSSQLServerAuditor.Model.Connections
{
	public class ActiveDirectoryConnection
	{
		public ActiveDirectoryConnection(string connectionPath)
		{
			this.ConnectionPath = connectionPath;
		}

		public string ConnectionPath { get; private set; }
	}
}
