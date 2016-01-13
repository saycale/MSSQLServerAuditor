using System.Data;
using System.Data.SQLite;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables
{
	public class FileTable : Common.Table
	{
		public const string TableIdentityField = "rowid";

		private const string    FieldFileName = "FileName";
		private const string    FieldContent  = "Content";
		private readonly string _folderTableName;

		public FileTable(SQLiteConnection connection, string tableName, string folderTableName)
			: base(connection, CreateTableDefinition(tableName, folderTableName))
		{
			this._folderTableName = folderTableName;
		}

		public static TableDefinition CreateTableDefinition(string tableName, string folderTableName)
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(tableName, TableIdentityField)
				.AddBigIntField(folderTableName.AsFk(),                          true,  false)
				.AddNVarCharField(FieldFileName,                                 true,  true)
				.AddField(new FieldDefinition(FieldContent, SqlDbType.VarBinary, false, false));
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public void WriteFile(byte[] raw, long? folderId, string file)
		{
			var row = this.NewRow();

			row.Values.Add(this._folderTableName.AsFk(), folderId);
			row.Values.Add(FieldFileName,                file);
			row.Values.Add(FieldContent,                 raw);

			this.InsertOrUpdateRow(row, tableRow => { });
		}

		public byte[] ReadFile(long? folderId, string file)
		{
			byte[] result = null;

			string sql = string.Format(
				"SELECT {0} FROM {1} WHERE {2} = {3} AND {4} = {5}",
				FieldContent,
				this.TableDefinition.Name,
				this._folderTableName.AsFk(),
				this._folderTableName.AsFk().AsParamName(),
				FieldFileName,
				FieldFileName.AsParamName()
			);

			var folderParameter =
				new SQLiteParameter(this._folderTableName.AsFk().AsParamName(), DbType.Int64)
				{
					Value = folderId
				};

			var fileParameter =
				new SQLiteParameter(FieldFileName.AsParamName(), DbType.String)
				{
					Value = file
				};

			using (this.Connection.OpenWrapper())
			{
				new SqlSelectCommand(
					this.Connection,
					sql,
					reader =>
					{
						result = reader.GetBytes();
					},
					folderParameter,
					fileParameter
				).Execute(100);
			}

			return result;
		}
	}
}
