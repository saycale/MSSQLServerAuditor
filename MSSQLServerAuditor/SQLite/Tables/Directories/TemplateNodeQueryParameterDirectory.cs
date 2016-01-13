using System;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class TemplateNodeQueryParameterDirectory : TableDirectory
	{
		public const string  TableName              = "d_TemplateNodeQueryParameter";
		public const string  TableIdentityField     = "d_TemplateNodeQueryParameter_id";
		private const string ParameterNameFieldName = "ParameterName";

		public TemplateNodeQueryParameterDirectory(
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
				.AddBigIntField(TemplateNodeQueryDirectory.TableName.AsFk(), true,  false)
				.AddNVarCharField(ParameterNameFieldName,                    true,  false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public Int64? GetId(ConnectionGroupInfo connectionGroup, TemplateNodeQueryInfo query, string parameterName)
		{
			TemplateNodeInfo templateNode = query.TemplateNode.IsInstance
				? query.TemplateNode.Template
				: query.TemplateNode;

			long? queryId = Storage.TemplateNodeQueryDirectory
				.GetId(connectionGroup, templateNode, query);

			return this.GetRecordIdByFields(
				this.CreateField(TemplateNodeQueryDirectory.TableName.AsFk(), queryId),
				this.CreateField(ParameterNameFieldName, parameterName)
			);
		}
	}
}
