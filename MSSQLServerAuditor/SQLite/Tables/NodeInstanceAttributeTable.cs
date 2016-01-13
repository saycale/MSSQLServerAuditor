using System.Data;
using System.Data.SQLite;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables
{
	public class NodeInstanceAttributeTable : CurrentStorageTable
	{
		public static readonly string TableName          = "d_NodeInstance_Attribute";
		public           const string TableIdentityField = "d_NodeInstance_Attribute_id";
		public static readonly string NodeInstaceIdFn    = NodeInstanceTable.TableName.AsFk();
		public static readonly string NameFn             = "AttributeName";
		public static readonly string ValueFn            = "AttributeValue";

		public NodeInstanceAttributeTable(CurrentStorage storage)
			: base(storage, CreateTableDefinition())
		{
		}

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddBigIntField(NodeInstaceIdFn, true,  false)
				.AddNVarCharField(NameFn,        true,  false)
				.AddNVarCharField(ValueFn,       false, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public void ReadTo(long nodeInstanceAttrId, TemplateNodeInfo.NodeAttirbutes attributes, SQLiteConnection cnn)
		{
			string sql = string.Format(
				"SELECT * FROM {0} WHERE {1} = {2}",
				TableName,
				NodeInstaceIdFn,
				nodeInstanceAttrId
			);

			using (SQLiteCommand cmd = new SQLiteCommand(sql, cnn))
			{
				using (SQLiteDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						attributes.Values[(string)reader[NameFn]] = (string)reader[ValueFn];
						//attributes.Initialized = true;
						//TODO KupVadim [20.12.2014 23:27]: This flag broken getter for UIid, NodeAttribute.GetUid()
					}
				}
			}
		}
	}
}
