using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables
{
	public class LastConnectionProtocolRow : AutoincrementTableRow
	{
		public LastConnectionProtocolRow()
			: base(LastConnectionProtocolTable.CreateTableDefinition())
		{
		}

		public string DbType
		{
			get { return this.GetValue<string>(LastConnectionProtocolTable.DbTypeFn); }
			set { this.SetValue(LastConnectionProtocolTable.DbTypeFn, value); }
		}

		public string MachineName
		{
			get { return this.GetValue<string>(LastConnectionProtocolTable.MachineNameFn); }
			set { this.SetValue(LastConnectionProtocolTable.MachineNameFn, value); }
		}

		public long GroupId
		{
			get { return this.GetValue<long>(LastConnectionProtocolTable.GroupFk); }
			set { this.SetValue(LastConnectionProtocolTable.GroupFk, value); }
		}

		public long TemplateId
		{
			get { return this.GetValue<long>(LastConnectionProtocolTable.TemplateFk); }
			set { this.SetValue(LastConnectionProtocolTable.TemplateFk, value); }
		}
	}

	public class LastConnectionProtocolTable : CurrentStorageTable
	{
		public const           string TableName          = "d_LastConnection_Protocol";
		public const           string TableIdentityField = "d_LastConnection_Protocol_id";
		public const           string MachineNameFn      = "MachineName";
		public const           string DbTypeFn           = "DbTypeFn";
		public static readonly string GroupFk            = ConnectionGroupDirectory.TableName.AsFk();
		public static readonly string TemplateFk         = TemplateDirectory.TableName.AsFk();

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddNVarCharField(MachineNameFn, true,  false)
				.AddNVarCharField(DbTypeFn,      true,  true)
				.AddBigIntField(GroupFk,         false, false)
				.AddBigIntField(TemplateFk,      false, false);
		}

		public LastConnectionProtocolTable(CurrentStorage storage)
			: base(storage, CreateTableDefinition())
		{
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public LastConnectionProtocolRow GetRow(long lastConnectionId)
		{
			ITableRow row = GetRowByIdentity(lastConnectionId);

			if (row == null)
			{
				return null;
			}

			return RowConverter.Convert<LastConnectionProtocolRow>(row);
		}

		public List<LastConnectionProtocolRow> GetLastConnections(
			string machineName
		)
		{
			string clause = MachineNameFn.AsSqlClausePair();

			List<SQLiteParameter> parameters = Lists.Of(
				new SQLiteParameter(MachineNameFn, machineName)
			);

			List<LastConnectionProtocolRow> rows =
				GetRows(
					clause,
					parameters)
				.Select(RowConverter.Convert<LastConnectionProtocolRow>)
				.ToList();

			return rows;
		}

		public LastConnectionProtocolRow GetLastConnection(
			string machineName,
			string protocolType
		)
		{
			string clause = string.Join(" AND ", Lists.Of(
				DbTypeFn.AsSqlClausePair(),
				MachineNameFn.AsSqlClausePair()
			));

			List<SQLiteParameter> parameters = Lists.Of(
				new SQLiteParameter(DbTypeFn,      protocolType),
				new SQLiteParameter(MachineNameFn, machineName)
			);

			LastConnectionProtocolRow row =
				GetRows(
					clause,
					parameters
				)
				.Select(RowConverter.Convert<LastConnectionProtocolRow>)
				.FirstOrDefault();

			return row;
		}

		public long? SaveLastConnection(LastConnectionProtocolRow row)
		{
			return this.InsertOrUpdateRow(row);
		}
	}
}
