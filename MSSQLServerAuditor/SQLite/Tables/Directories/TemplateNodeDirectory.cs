using System;
using System.Data;
using System.Diagnostics;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	/// <summary>
	/// SQLite directory for template node
	/// </summary>
	public class TemplateNodeDirectory : TableDirectory
	{
		internal const string         TableName                    = "d_TemplateNode";
		public   const string         TableIdentityField           = "d_TemplateNode_id";
		private  const string         ParentIdFn                   = "d_TemplateNodeParent_id";
		public   const string         UserIdFieldName              = "TemplateNodeUserId";
		public   const string         ParentQueryGroupIdFn         = "d_TemplateNodeQueryGroupParent_Id";
		public   const string         NameFn                       = "TemplateNodeUserName";
		private  const string         IconFieldName                = "TemplateNodeIcon";
		private  const string         ShowIfEmptyFieldName         = "TemplateNodeShowIfEmpty";
		private  const string         ShowNumberOfRecordsFieldName = "TemplateNodeShowNumberOfRecords";
		public static readonly string TemplateIdFn                 = "d_Template_id";

		public TemplateNodeDirectory(
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
				.AddBigIntField(TemplateIdFn,                     true,  false)
				.AddBigIntField(ParentIdFn,                       true,  false)
				.AddNVarCharField(UserIdFieldName,                true,  false)
				.AddField(ParentQueryGroupIdFn, SqlDbType.BigInt, false, false, null, 2)
				.AddNVarCharField(NameFn,                         false, false)
				.AddNVarCharField(IconFieldName,                  false, false)
				.AddBitField(ShowIfEmptyFieldName,                false, false)
				.AddBitField(ShowNumberOfRecordsFieldName,        false, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		/// <summary>
		/// Get Id for data
		/// </summary>
		/// <param name="connectionGroup">Connection group</param>
		/// <param name="templateNode">Template node</param>
		/// <returns></returns>
		public Int64? GetId(ConnectionGroupInfo connectionGroup, TemplateNodeInfo templateNode)
		{
			Debug.Assert(!templateNode.IsInstance);

			Int64? parentId = null;

			if (templateNode.Parent != null)
			{
				parentId = this.GetId(connectionGroup, templateNode.Parent);
			}

			long?                 templateId = Storage.TemplateDirectory.GetId(connectionGroup);
			TemplateNodeQueryInfo pq         = templateNode.GetParentQuery();

			object parentQueryId = pq != null
				? (object) Storage.TemplateNodeQueryGroupDirectory.GetId(connectionGroup, pq)
				: null;

			return this.GetRecordIdByFields(
				this.CreateField(TemplateIdFn,                 templateId),
				this.CreateField(ParentIdFn,                   parentId),
				this.CreateField(UserIdFieldName,              templateNode.Id),
				this.CreateField(NameFn,                       templateNode.Name),
				this.CreateField(IconFieldName,                templateNode.IconImageReferenceName),
				this.CreateField(ShowIfEmptyFieldName,         !templateNode.HideEmptyResultDatabases),
				this.CreateField(ShowNumberOfRecordsFieldName, templateNode.ShowNumberOfRecords),
				this.CreateField(ParentQueryGroupIdFn,         parentQueryId)
			);
		}
	}
}
