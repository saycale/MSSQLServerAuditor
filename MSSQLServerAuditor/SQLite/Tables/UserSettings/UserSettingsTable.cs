using System.Linq;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.SQLite.Tables.UserSettings
{
	public class UserSettingsTable : CurrentStorageTable
	{
		public const string TableName          = "d_TemplateNode_UserSettings";
		public const string TableIdentityField = "d_TemplateNode_UserSettings_id";
		public const string TemplateNodeId     = "d_TemplateNode_Id";
		public const string Language           = "Language";
		public const string NodeUIcon          = "NodeUIcon";
		public const string NodeEnabled        = "NodeEnabled";
		public const string NodeFontStyle      = "NodeFontStyle";
		public const string NodeFontColor      = "NodeFontColor";
		public const string NodeUName          = "NodeUName";

		public UserSettingsTable(
			CurrentStorage storage
		) : base(storage, CreateTableDefinition())
		{
		}

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddBigIntField(TemplateNodeId,  true,  false)
				.AddNVarCharField(Language,      true,  false)
				.AddNVarCharField(NodeUName,     false, false)
				.AddNVarCharField(NodeUIcon,     false, false)
				.AddBitField(NodeEnabled,        false, false)
				.AddNVarCharField(NodeFontStyle, false, false)
				.AddNVarCharField(NodeFontColor, false, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public UserSettingsRow GetByTemplateNodeAndLanguage(long templateNodeId, string language)
		{
			return this
				.GetRows(
					string.Format(
						"{0}={1} AND {2}='{3}'",
						TemplateNodeId,
						templateNodeId,
						Language,
						language
					)
				)
				.Select(RowConverter.Convert<UserSettingsRow>)
				.FirstOrDefault();
		}
	}
}
