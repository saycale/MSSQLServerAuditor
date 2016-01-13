using System.Data;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class QueryParameterDirectory : QueryParameterDirectoryBase
	{
		private static readonly string TableName          = "d_Query_Parameter";
		public  static readonly string TableIdentityField = "d_Query_Parameter_id";
		private static readonly string FkQueryId          = QueryDirectory.TableName.AsFk();
		private static readonly string FkParameterId      = TemplateNodeQueryParameterDirectory.TableName.AsFk();

		public QueryParameterDirectory(
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
				.AddBigIntField(FkQueryId,            true,  false)
				.AddBigIntField(FkParameterId,        true,  false)
				.AddNVarCharField(ValueFn,            false, false)
				.AddField(IsEnabledFn, SqlDbType.Bit, false, true, true, null)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		protected override long? GetParameterId(
			ConnectionGroupInfo   connectionGroup,
			TemplateNodeQueryInfo query,
			string                parameterName
		)
		{
			return Storage.TemplateNodeQueryParameterDirectory
				.GetId(connectionGroup, query, parameterName);
		}

		protected override string QueryIdFn
		{
			get { return FkQueryId; }
		}

		protected override string ParameterIdFn
		{
			get { return FkParameterId; }
		}

		protected override string GetTableName()
		{
			return TableName;
		}
	}
}
