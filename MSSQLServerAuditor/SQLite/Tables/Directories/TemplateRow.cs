using MSSQLServerAuditor.SQLite.Common;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class TemplateRow : AutoincrementTableRow
	{
		public TemplateRow()
			: base(TemplateDirectory.CreateTableDefinition())
		{
		}

		internal string Name
		{
			get { return this.GetValue<string>(TemplateDirectory.NameFieldName); }
			set { this.SetValue(TemplateDirectory.NameFieldName, value); }
		}

		internal string Id
		{
			get { return this.GetValue<string>(TemplateDirectory.IdFieldName); }
			set { this.SetValue(TemplateDirectory.IdFieldName, value); }
		}

		internal string Directory
		{
			get { return this.GetValue<string>(TemplateDirectory.DirFieldName); }
			set { this.SetValue(TemplateDirectory.DirFieldName, value); }
		}
	}
}
