using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class QueryResultsDirectory : TableDirectory
	{
		public const           string TableName              = "d_Query_Results";
		public static readonly string TableIdentityField     = "d_Query_Results_id";
		public static readonly string QueryFk                = QueryDirectory.TableName.AsFk();
		public const           string RecordSetFn            = "RecordSet";
		public const           string QueryResultTableNameFn = "QueryResultTableName";

		public QueryResultsDirectory(
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
				.AddBigIntField(QueryFk,                  true,  true)
				.AddBigIntField(RecordSetFn,              true,  true)
				.AddNVarCharField(QueryResultTableNameFn, false, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public long? GetId(
			long   queryId,
			long   recordSet,
			string resultTableName
		)
		{
			return this.GetRecordIdByFields(
				this.CreateField(QueryFk,                queryId),
				this.CreateField(RecordSetFn,            recordSet),
				this.CreateField(QueryResultTableNameFn, resultTableName)
			);
		}
	}
}
