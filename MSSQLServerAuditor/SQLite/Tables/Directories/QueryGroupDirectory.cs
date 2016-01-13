using System;
using System.Diagnostics;
using System.Collections.Generic;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

using Field = System.Tuple<string, object>;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class QueryGroupDirectory : QueryDirectoryBase
	{
		private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		internal const string         TableName                  = "d_Query_Group";
		public static readonly string TableIdentityField         = "d_Query_Group_id";
		public static readonly string NodeInstanceIdFn           = NodeInstanceTable.TableName.AsFk();
		public static readonly string TemplateNodeQueryGroupIdFn = TemplateNodeQueryGroupDirectory.TableName.AsFk();
		public static readonly string LoginIdFn                  = LoginDirectory.TableName.AsFk();
		public static readonly string ServerInstanceIdFn         = ServerInstanceDirectory.TableName.AsFk();
		public static readonly string DefaultDatabaseNameFn      = "DefaultDatabaseName";

		public QueryGroupDirectory(
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
				.AddBigIntField(NodeInstanceIdFn,           true,  false)
				.AddBigIntField(TemplateNodeQueryGroupIdFn, true,  false)
				.AddBigIntField(LoginIdFn,                  true,  false)
				.AddBigIntField(ServerInstanceIdFn,         true,  false)
				.AddNVarCharField(DefaultDatabaseNameFn,    false, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public override long? GetQueryId(
			TemplateNodeInfo      node,
			TemplateNodeQueryInfo templateNodeQuery,
			InstanceInfo          instance,
			DateTime              dateCreated,
			bool                  onlyFind
		)
		{
			Int64? queryGroupId = 0L;

			Debug.Assert(node.IsInstance);

			if (node.TemplateNodeId == null)
			{
				throw new InvalidOperationException();
			}

			Int64? templateNodeQueryGroupId = Storage.TemplateNodeQueryGroupDirectory
				.GetId(node.ConnectionGroup, templateNodeQuery);

			Int64? loginId = Storage.LoginDirectory.GetId(instance);

			Int64? serverId = Storage.ServerInstanceDirectory.GetId(
				node.ConnectionGroup,
				instance
			);

			if (onlyFind)
			{
				var key = new Tuple<Int64, Int64, Int64, Int64>(
					(Int64) node.TemplateNodeId.Value,
					(Int64) templateNodeQueryGroupId,
					(Int64) loginId,
					(Int64) serverId
				);

				return base.Cache.GetOrAdd(
					key,
					() =>
					{
						ITableRow row = this.NewRow();

						row.Values.Add(NodeInstanceIdFn,           node.TemplateNodeId.Value);
						row.Values.Add(TemplateNodeQueryGroupIdFn, templateNodeQueryGroupId);
						row.Values.Add(ServerInstanceIdFn,         serverId);
						row.Values.Add(LoginIdFn,                  loginId);

						queryGroupId = this.GetRow(row);

						Log.InfoFormat("ServerId:'{0}';NodeInstanceId:'{1}';TemplateNodeQueryGroupId:'{2}';PrimaryKey:'{3}'",
							serverId,
							node.TemplateNodeId.Value,
							templateNodeQueryGroupId,
							queryGroupId
						);

						return queryGroupId;
					}
				);
			}

			List<Field> customFields = new List<Field>
			{
				this.CreateField(DefaultDatabaseNameFn,       node.GetDefaultDatabase()),
				this.CreateField(TableDefinition.DateCreated, dateCreated),
				this.CreateField(NodeInstanceIdFn,            node.TemplateNodeId.GetValueOrDefault()),
				this.CreateField(TemplateNodeQueryGroupIdFn,  templateNodeQueryGroupId),
				this.CreateField(LoginIdFn,                   loginId),
				this.CreateField(ServerInstanceIdFn,          serverId)
			};

			queryGroupId = this.GetRecordIdByFields(customFields.ToArray());

			Log.InfoFormat("ServerId:'{0}';NodeInstanceId:'{1}';TemplateNodeQueryGroupId:'{2}';PrimaryKey:'{3}'",
				serverId,
				node.TemplateNodeId.GetValueOrDefault(),
				templateNodeQueryGroupId,
				queryGroupId
			);

			return queryGroupId;
		}

		public override string DirectoryTableName
		{
			get { return TableName; }
		}
	}
}
