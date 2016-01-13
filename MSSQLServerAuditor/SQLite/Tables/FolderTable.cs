using System.Data.SQLite;
using System.IO;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;

namespace MSSQLServerAuditor.SQLite.Tables
{
	public class FolderTable : Common.Table
	{
		public const string TableIdentityField   = "rowid";
		private const string FieldFolderName     = "FolderName";
		private const string FieldParentFolderId = "ParentFolderId";

		public FolderTable(SQLiteConnection connection, string tableName)
			: base(connection, CreateTableDefinition(tableName))
		{
		}

		public static TableDefinition CreateTableDefinition(string tableName)
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(tableName, TableIdentityField)
				.AddNVarCharField(FieldFolderName,   true,  false)
				.AddBigIntField(FieldParentFolderId, true,  false);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public long? GetFolderId(string path)
		{
			string[]  folders = path.Split(Path.DirectorySeparatorChar);
			long?     result  = null;
			ITableRow row     = this.NewRow();

			row.Values.Add(FieldParentFolderId, null);
			row.Values.Add(FieldFolderName,     null);

			foreach (var folder in folders)
			{
				row.Values[FieldParentFolderId] = result;
				row.Values[FieldFolderName]     = folder;

				result = this.InsertOrUpdateRow(row);
			}

			return result;
		}
	}
}
