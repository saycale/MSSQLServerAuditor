using System.Data;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class QueryGroupParameterDirectory : QueryParameterDirectoryBase
	{
		public  static readonly string TableIdentityField = "d_Query_GroupParameter_id";
		private static readonly string _TableName         = "d_Query_GroupParameter";
		private static readonly string _FkQueryId         = QueryGroupDirectory.TableName.AsFk();
		private static readonly string _FkParameterId     = TemplateNodeQueryGroupParameterDirectory.TableName.AsFk();

		public QueryGroupParameterDirectory(
			CurrentStorage storage
		) : base(
				storage,
				CreateTableDefinition()
			)
		{
		}

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(_TableName, TableIdentityField)
				.AddBigIntField(_FkQueryId,            true,  false)
				.AddBigIntField(_FkParameterId,        true,  false)
				.AddField(ValueFn, SqlDbType.NVarChar, false, false, false, null)
				.AddField(IsEnabledFn, SqlDbType.Bit,  false, false, true,  null)
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
			return Storage.TemplateNodeQueryGroupParameterDirectory
				.GetId(connectionGroup, query, parameterName);
		}

		protected override string QueryIdFn
		{
			get { return _FkQueryId; }
		}

		protected override string ParameterIdFn
		{
			get { return _FkParameterId; }
		}

		protected override string GetTableName()
		{
			return _TableName;
		}
	}
}
