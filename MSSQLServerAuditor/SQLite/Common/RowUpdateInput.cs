namespace MSSQLServerAuditor.SQLite.Common
{
	public class RowUpdateInput
	{
		public ITableRow Row
		{
			get;
			private set;
		}

		public string OldHash
		{
			get;
			private set;
		}

		public RowUpdateInput(string oldHash, ITableRow row)
		{
			this.Row     = row;
			this.OldHash = oldHash;
		}
	}
}
