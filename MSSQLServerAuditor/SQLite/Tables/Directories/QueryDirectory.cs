using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

using Field = System.Tuple<string, object>;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class QueryDirectory : QueryDirectoryBase //NodeInstance, TemplateNodeQuery, Login, ServerInstance
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public  static readonly string TableName              = "d_Query";
		public  static readonly string TableIdentityField     = "d_Query_id";
		private static readonly string NodeInstanceIdFn       = NodeInstanceTable.TableName.AsFk();
		private static readonly string TemplateNodeQueryIdFn  = TemplateNodeQueryDirectory.TableName.AsFk();
		private static readonly string ServerInstanceIdFn     = ServerInstanceDirectory.TableName.AsFk();
		private static readonly string DefaultDatabaseNameFn  = "DefaultDatabaseName";

		public QueryDirectory(
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
				.AddBigIntField(NodeInstanceIdFn,         true,  true)
				.AddBigIntField(TemplateNodeQueryIdFn,    true,  false)
				.AddBigIntField(ServerInstanceIdFn,       true,  false)
				.AddNVarCharField(DefaultDatabaseNameFn,  false, false)
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
			Int64? queryId = 0L;

			Debug.Assert(node.IsInstance);

			if (node.TemplateNodeId == null)
			{
				throw new InvalidOperationException();
			}

			Int64? templateNodeQueryId = Storage.TemplateNodeQueryDirectory
				.GetId(node.ConnectionGroup, node.Template, templateNodeQuery);

			Int64? serverId = Storage.ServerInstanceDirectory
				.GetId(node.ConnectionGroup, instance);

			if (onlyFind)
			{
				Tuple<long, long, long> key = new Tuple<Int64, Int64, Int64>(
					(long) node.TemplateNodeId.Value,
					(long) templateNodeQueryId,
					(long) serverId
				);

				return this.Cache.GetOrAdd(
					key, () =>
					{
						ITableRow row = this.NewRow();

						row.Values.Add(NodeInstanceIdFn,      node.TemplateNodeId.Value);
						row.Values.Add(TemplateNodeQueryIdFn, templateNodeQueryId);
						row.Values.Add(ServerInstanceIdFn,    serverId);

						queryId = this.GetRow(row);

						Log.InfoFormat("ServerId:'{0}';NodeInstanceId:'{1}';TemplateNodeQueryId:'{2}';PrimaryKey:'{3}'",
							serverId,
							node.TemplateNodeId.Value,
							templateNodeQueryId,
							queryId
						);

						return queryId;
					}
				);
			}

			List<Field> customFields = new List<Field>
			{
				this.CreateField(DefaultDatabaseNameFn,       node.GetDefaultDatabase()),
				this.CreateField(TableDefinition.DateCreated, dateCreated),
				this.CreateField(NodeInstanceIdFn,            node.TemplateNodeId.GetValueOrDefault()),
				this.CreateField(TemplateNodeQueryIdFn,       templateNodeQueryId),
				this.CreateField(ServerInstanceIdFn,          serverId)
			};

			queryId = this.GetRecordIdByFields(customFields.ToArray());

			Log.InfoFormat("ServerId:'{0}';NodeInstanceId:'{1}';TemplateNodeQueryId:'{2}';id:'{3}'",
				serverId,
				node.TemplateNodeId.GetValueOrDefault(),
				templateNodeQueryId,
				queryId
			);

			return queryId;
		}

		public override string DirectoryTableName
		{
			get { return TableName; }
		}
	}
}
