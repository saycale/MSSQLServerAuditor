using System.Data.SQLite;
using System.Linq;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables
{
	public class LastConnectionTable : CurrentStorageTable
	{
		public const           string TableName          = "d_LastConnection";
		public const           string TableIdentityField = "d_LastConnection_id";
		public const           string MachineNameFn      = "MachineName";
		public static readonly string LastCnnProtocolFk  = LastConnectionProtocolTable.TableName.AsFk();

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddNVarCharField(MachineNameFn,   true,  false)
				.AddBigIntField(LastCnnProtocolFk, false, true);
		}

		public LastConnectionTable(CurrentStorage storage)
			: base(storage, CreateTableDefinition())
		{
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public LastConnectionRow GetLastConnection(string machineName)
		{
			string clause = MachineNameFn.AsSqlClausePair();

			LastConnectionRow row =
				GetRows(
					clause,
					Lists.Of(new SQLiteParameter(MachineNameFn, machineName))
				).Select(RowConverter.Convert<LastConnectionRow>).FirstOrDefault();

			return row;
		}

		public void SaveLastConnection(LastConnectionRow row)
		{
			this.InsertOrUpdateRow(row);
		}
	}
}
