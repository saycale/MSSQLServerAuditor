using System;
using System.Diagnostics;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class TemplateNodeQueryGroupDirectory : TableDirectory
	{
		internal const string TableName                           = "d_TemplateNodeQueryGroup";
		internal const string TableIdentityField                  = "d_TemplateNodeQueryGroup_id";
		internal const string TemplateNodeQueryGroupIdFieldName   = "TemplateNodeQueryGroupId";
		internal const string TemplateNodeQueryGroupNameFieldName = "TemplateNodeQueryGroupName";
		internal const string DefaultDatabaseFieldFn              = "DefaultDatabaseField";

		public TemplateNodeQueryGroupDirectory(
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
				.AddBigIntField(TemplateNodeDirectory.TableName.AsFk(), true,  false)
				.AddNVarCharField(TemplateNodeQueryGroupIdFieldName,    true,  false)
				.AddNVarCharField(TemplateNodeQueryGroupNameFieldName,  false, false)
				.AddNVarCharField(DefaultDatabaseFieldFn,               false, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public Int64? GetId(ConnectionGroupInfo connectionGroup, TemplateNodeQueryInfo query)
		{
			Debug.Assert(query.TemplateNode.GroupQueries.Contains(query));

			TemplateNodeInfo templateNode = query.TemplateNode.IsInstance
				? query.TemplateNode.Template
				: query.TemplateNode;

			long? templateNodeId = Storage.TemplateNodeDirectory.GetId(connectionGroup, templateNode);

			return this.GetRecordIdByFields(
				this.CreateField(TemplateNodeDirectory.TableName.AsFk(), templateNodeId),
				this.CreateField(TemplateNodeQueryGroupIdFieldName,      query.Id),
				this.CreateField(TemplateNodeQueryGroupNameFieldName,    query.QueryName),
				this.CreateField(DefaultDatabaseFieldFn,                 query.DatabaseForChildrenFieldName)
			);
		}
	}
}
