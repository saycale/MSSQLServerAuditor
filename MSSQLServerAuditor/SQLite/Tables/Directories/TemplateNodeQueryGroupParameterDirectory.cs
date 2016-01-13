using System;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class TemplateNodeQueryGroupParameterDirectory : TableDirectory
	{
		public const string  TableName              = "d_TemplateNodeQueryGroupParameter";
		public const string  TableIdentityField     = "d_TemplateNodeQueryGroupParameter_id";
		private const string ParameterNameFieldName = "ParameterName";

		public TemplateNodeQueryGroupParameterDirectory(
			CurrentStorage storage
		) : base(
				storage,
				CreateTableDefinition()
			)
		{
		}

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddBigIntField(TemplateNodeQueryGroupDirectory.TableName.AsFk(), true,  false)
				.AddNVarCharField(ParameterNameFieldName,                         true,  false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public Int64? GetId(ConnectionGroupInfo connectionGroup, TemplateNodeQueryInfo groupQuery, string parameterName)
		{
			long? queryId = Storage.TemplateNodeQueryGroupDirectory
				.GetId(connectionGroup, groupQuery);

			return this.GetRecordIdByFields(
				this.CreateField(TemplateNodeQueryGroupDirectory.TableName.AsFk(), queryId),
				this.CreateField(ParameterNameFieldName, parameterName)
			);
		}
	}
}
