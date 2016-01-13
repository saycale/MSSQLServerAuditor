using System;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Tables.Directories;

namespace MSSQLServerAuditor.SQLite.Tables
{
	public class NodeInstanceRow : AutoincrementTableRow
	{
		public NodeInstanceRow() : base(NodeInstanceTable.CreateTableDefinition())
		{
		}

		public long ConnectionGroupId
		{
			get { return this.GetValue<long>(NodeInstanceTable.ConnectionIdFn); }
			set { this.SetValue(NodeInstanceTable.ConnectionIdFn, value); }
		}

		public long TemplateNodeId
		{
			get { return this.GetValue<long>(NodeInstanceTable.TemplateNodeIdFn); }
			set { this.SetValue(NodeInstanceTable.TemplateNodeIdFn, value); }
		}

		public DateTime DateUpdated
		{
			get { return this.GetValue<DateTime>("date_updated"); }
			set { this.SetValue("date_updated", value); }
		}

		public string TemplateNodeName
		{
			get { return this.GetValue<string>(TemplateNodeDirectory.NameFn); }
			set { this.SetValue(TemplateNodeDirectory.NameFn, value); }
		}

		public string TemplateFileName
		{
			get { return this.GetValue<string>(TemplateDirectory.IdFieldName); }
			set { this.SetValue(TemplateDirectory.IdFieldName, value); }
		}
	}
}
