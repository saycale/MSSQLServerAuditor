using MSSQLServerAuditor.SQLite.Common;

namespace MSSQLServerAuditor.SQLite.Tables
{
	public class LastConnectionRow : AutoincrementTableRow
	{
		public LastConnectionRow() : base(LastConnectionTable.CreateTableDefinition())
		{
		}

		public string MachineName
		{
			get { return this.GetValue<string>(LastConnectionTable.MachineNameFn); }
			set { this.SetValue(LastConnectionTable.MachineNameFn, value); }
		}

		public long LastConnectionProtocolId
		{
			get { return this.GetValue<long>(LastConnectionTable.LastCnnProtocolFk); }
			set { this.SetValue(LastConnectionTable.LastCnnProtocolFk, value); }
		}
	}
}
