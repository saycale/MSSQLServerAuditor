namespace MSSQLServerAuditor.SQLite.Common
{
	public static class RowConverter
	{
		public static TRow Convert<TRow>(ITableRow sourceRow) where TRow : ITableRow, new()
		{
			TRow row = new TRow();

			sourceRow.CopyValues(row);

			return row;
		}
	}
}
