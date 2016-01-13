using MSSQLServerAuditor.SQLite.Common;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class ConnectionGroupRow : AutoincrementTableRow
	{
		public ConnectionGroupRow()
			: base(ConnectionGroupDirectory.CreateTableDefinition())
		{

		}

		public string Name
		{
			get
			{
				return this.GetValue<string>(ConnectionGroupDirectory.NameFn);
			}
			set
			{
				this.SetValue(ConnectionGroupDirectory.NameFn, value);
			}
		}

		public bool IsDirect
		{
			get
			{
				return this.GetValue<bool>(ConnectionGroupDirectory.IsDirectConnectionFn);
			}
			set
			{
				this.SetValue(ConnectionGroupDirectory.IsDirectConnectionFn, value);
			}
		}
	}
}
