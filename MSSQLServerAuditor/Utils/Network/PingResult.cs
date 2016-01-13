namespace MSSQLServerAuditor.Utils.Network
{
	public class PingResult
	{
		public static PingResult Succeeded(
			string status,
			string ipAddress,
			long   elapsedMillis
		)
		{
			PingResult result = new PingResult
			{
				Status        = status,
				IpAddress     = ipAddress,
				ElapsedMillis = elapsedMillis
			};

			return result;
		}

		public static PingResult Failed(
			string failureStatus,
			string failureDescription,
			long   elapsedMillis
		)
		{
			PingResult result = new PingResult
			{
				Status        = failureStatus,
				Description   = failureDescription,
				ElapsedMillis = elapsedMillis
			};

			return result;
		}

		private PingResult()
		{
			this.ElapsedMillis = -1;
		}

		public string Status        { get; private set; }
		public string IpAddress     { get; private set; }
		public long   ElapsedMillis { get; private set; }
		public string Description   { get; private set; }
	}
}
