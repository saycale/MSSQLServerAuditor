namespace MSSQLServerAuditor.SQLite.Common
{
	public class RowUpdateOrderInput
	{
		public long Id
		{
			get;
			private set;
		}

		public string Hash
		{
			get;
			private set;
		}

		public RowUpdateOrderInput(long id, string hash)
		{
			this.Id   = id;
			this.Hash = hash;
		}
	}
}
