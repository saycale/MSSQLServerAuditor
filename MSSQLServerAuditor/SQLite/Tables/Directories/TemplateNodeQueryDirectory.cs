using System;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	/// <summary>
	/// SQLite directory for template node queries
	/// </summary>
	public class TemplateNodeQueryDirectory : TableDirectory
	{
		internal const string TableName               = "d_TemplateNodeQuery";
		internal const string TableIdentityField      = "d_TemplateNodeQuery_id";
		internal const string TemplateNodeIdFieldName = "d_TemplateNode_id";
		internal const string UserIdFieldName         = "TemplateNodeQueryUserId";
		internal const string NameFieldName           = "TemplateNodeQueryUserName";
		internal const string HierarchyFieldName      = "TemplateNodeQueryUserHierarchy";

		public TemplateNodeQueryDirectory(
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
				.AddBigIntField(TemplateNodeIdFieldName, true, false)
				.AddNVarCharField(UserIdFieldName,       true, false)
				.AddNVarCharField(NameFieldName,         true, false)
				.AddNVarCharField(HierarchyFieldName,    true, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		/// <summary>
		/// Get ID for data
		/// </summary>
		/// <param name="connectionGroup">Connection group</param>
		/// <param name="templateNode">Template node</param>
		/// <param name="templateNodeQuery">Template node query</param>
		/// <returns></returns>
		public Int64? GetId(
			ConnectionGroupInfo   connectionGroup,
			TemplateNodeInfo      templateNode,
			TemplateNodeQueryInfo templateNodeQuery
		)
		{
			long? templateNodeId = Storage.TemplateNodeDirectory
				.GetId(connectionGroup, templateNode);

			string id        = templateNodeQuery.Id;
			string name      = templateNodeQuery.QueryName;
			string hierarchy = templateNodeQuery.ResultHierarchy;

			return this.GetRecordIdByFields(
				this.CreateField(TemplateNodeIdFieldName, templateNodeId),
				this.CreateField(UserIdFieldName,         id),
				this.CreateField(NameFieldName,           name),
				this.CreateField(HierarchyFieldName,      hierarchy)
			);
		}
	}
}
