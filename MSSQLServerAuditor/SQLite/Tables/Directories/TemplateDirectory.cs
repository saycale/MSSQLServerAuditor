using System;
using System.Collections.Generic;
using System.IO;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	/// <summary>
	/// SQLite directory for template
	/// </summary>
	public class TemplateDirectory : TableDirectory
	{
		internal const string TableName          = "d_Template";
		internal const string TableIdentityField = "d_Template_id";
		internal const string NameFieldName      = "TemplateUserName";
		internal const string IdFieldName        = "TemplateUserId";
		internal const string DirFieldName       = "TemplateUserDir";

		public TemplateDirectory(
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
				.AddNVarCharField(NameFieldName, true, false)
				.AddNVarCharField(IdFieldName,   true, false)
				.AddNVarCharField(DirFieldName,  true, false)
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
		/// <returns></returns>
		public Int64? GetId(ConnectionGroupInfo connectionGroup)
		{
			return this.GetRecordIdByFields(
				this.CreateField(IdFieldName,   connectionGroup.TemplateFileName),
				this.CreateField(NameFieldName, connectionGroup.TemplateId),
				this.CreateField(DirFieldName,  connectionGroup.TemplateDir ?? string.Empty)
			);
		}

		public List<TemplateRow> GetExternalTemplates()
		{
			return GetTemplates().FindAll(r =>
			{
				string dir = r.Directory;

				if (!string.IsNullOrEmpty(dir) && Path.IsPathRooted(dir))
				{
					return true;
				}

				return false;
			});
		}

		private List<TemplateRow> GetTemplates()
		{
			List<TemplateRow> templateRows = new List<TemplateRow>();
			List<ITableRow>   rows         = GetRows(null);

			foreach (ITableRow row in rows)
			{
				TemplateRow templateRow = RowConverter.Convert<TemplateRow>(row);

				templateRows.Add(templateRow);
			}

			return templateRows;
		}

		public TemplateRow GetTemplate(long templateId)
		{
			ITableRow row = GetRowByIdentity(templateId);

			if (row != null)
			{
				return RowConverter.Convert<TemplateRow>(row);
			}

			return null;
		}
	}
}
