namespace MSSQLServerAuditor.SQLite.Commands
{
	interface IDbCommand
	{
		long Execute(int intCommandAttempts);
	}
}
