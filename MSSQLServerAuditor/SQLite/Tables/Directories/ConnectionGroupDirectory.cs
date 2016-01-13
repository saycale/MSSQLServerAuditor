using System;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	/// <summary>
	/// SQLite directory for connection group
	/// </summary>
	public class ConnectionGroupDirectory : TableDirectory
	{
		internal const string TableName            = "d_ConnectionGroup";
		public   const string TableIdentityField   = "d_ConnectionGroup_id";
		internal const string NameFn               = "ConnectionGroupName";
		internal const string IsDirectConnectionFn = "IsDirectConnection";

		public ConnectionGroupDirectory(
			CurrentStorage storage
		) : base (
				storage,
				CreateTableDefinition()
			)
		{
		}

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddNVarCharField(NameFn,          true,  false)
				.AddBitField(IsDirectConnectionFn, true,  false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get
			{
				return TableIdentityField;
			}
		}

		/// <summary>
		/// Get Id for data
		/// </summary>
		/// <param name="connectionGroup"></param>
		/// <returns></returns>
		public Int64? GetId(ConnectionGroupInfo connectionGroup)
		{
			Int64? connectionGroupId = null;

			if (connectionGroup != null)
			{
				connectionGroupId = this.GetRecordIdByFields(
					this.CreateField(NameFn, connectionGroup.Name),
					this.CreateField(IsDirectConnectionFn, connectionGroup.IsDirectConnection)
				);
			}

			return connectionGroupId;
		}
	}
}
