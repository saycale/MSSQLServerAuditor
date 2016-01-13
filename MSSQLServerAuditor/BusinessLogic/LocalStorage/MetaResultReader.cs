using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using MSSQLServerAuditor.Gui;
using MSSQLServerAuditor.Managers;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Connections;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Tables;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.BusinessLogic.LocalStorage
{
	public class MetaResultReader : NodeResultReader
	{
		private static readonly ILog                                  _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly List<InstanceInfo>                           _instances;
		private readonly Dictionary<TemplateNodeQueryInfo, DataTable> _histTable;

		public MetaResultReader(
			MsSqlAuditorModel              msSqlAuditor,
			StorageManager                 storageManager,
			ConnectionGroupInfo            connectionGroup,
			ConcreteTemplateNodeDefinition concreteTemplateNode
		) : base(
				msSqlAuditor,
				storageManager,
				concreteTemplateNode
			)
		{
			this._instances = new List<InstanceInfo>();

			if (base.GroupDefinition.Instance != null)
			{
				this._instances.Add(base.GroupDefinition.Instance);
			}
			else
			{
				this._instances.AddRange(connectionGroup.Connections.Where(info => info.IsEnabled));
			}

			this._histTable = ReadHistTable(connectionGroup, concreteTemplateNode);
		}

		public override void ReadTo(MultyQueryResultInfo result)
		{
			foreach (TemplateNodeResultItem templateNodeResultItem in base.TemplateNode.Queries.Select(
				templateNodeQueryInfo => ReadTemplateNodeResult(templateNodeQueryInfo, result))
			)
			{
				result.Add(templateNodeResultItem);
			}
		}

		private TemplateNodeResultItem ReadTemplateNodeResult(TemplateNodeQueryInfo templateNodeQueryInfo, MultyQueryResultInfo result)
		{
			var             queryResult = new QueryResultInfo();
			List<QueryInfo> queries     = MsSqlAuditor.GetQueryByTemplateNodeQueryInfo(templateNodeQueryInfo);

			// looking for user settings for parameter values
			templateNodeQueryInfo.ReadParametersFrom(Settings);

			string connectionsSelectId = templateNodeQueryInfo.ConnectionsSelectId;

			if (connectionsSelectId != null)
			{
				foreach (InstanceInfo instance in this._instances)
				{
					Int64? queryId = Storage.QueryDirectory.GetQueryId(
						templateNodeQueryInfo.TemplateNode,
						templateNodeQueryInfo,
						instance,
						DateTime.Now,
						false
					);

					if (queryId.HasValue)
					{
						List<DynamicConnection> connections = new List<DynamicConnection>(
							Storage.DynamicConnectionDirectory.ReadConnections(queryId.Value)
						);

						foreach (DynamicConnection connection in connections)
						{
							if (!connection.QueryId.HasValue)
							{
								continue;
							}

							Int64       dynamicQueryId = connection.QueryId.Value;
							QuerySource sourceType;

							if (!Enum.TryParse(connection.Type, true, out sourceType))
							{
								_log.ErrorFormat(
									@"Unknown ConnectionType:'{0}'",
									connection.Type ?? "<Null>"
								);

								sourceType = QuerySource.MSSQL;
							}

							InstanceInfo selectConnectionInstance = InstanceInfoResolver.ResolveDynamicInstance(
								connection.Name,
								sourceType,
								connection.IsOdbc
							);

							selectConnectionInstance.ConnectionGroup = instance.ConnectionGroup;

							selectConnectionInstance.LoadServerProperties(Storage);

							QueryInstanceResultInfo instanceResult = GetInstanceResult(
								result,
								selectConnectionInstance,
								dynamicQueryId,
								templateNodeQueryInfo,
								queries
							);

							if (instanceResult != null)
							{
								queryResult.AddInstanceResult(instanceResult);
							}
						}
					}
				}
			}
			else
			{
				foreach (InstanceInfo instance in this._instances)
				{
					Int64? queryId = Storage.QueryDirectory.GetQueryId(
						base.TemplateNode,
						templateNodeQueryInfo,
						instance,
						new DateTime(),
						true
					);

					if (queryId != null)
					{
						QueryInstanceResultInfo instanceResult = GetInstanceResult(
							result,
							instance,
							queryId.Value,
							templateNodeQueryInfo,
							queries
						);

						if (instanceResult != null)
						{
							queryResult.AddInstanceResult(instanceResult);
						}
					}
				}
			}

			Tuple<DateTime?, DateTime?> dateTimes =
				Storage.NodeInstances.GetTreeNodeLastUpdateAndDuration(base.TemplateNode);

			result.NodeLastUpdated        = dateTimes.Item1;
			result.NodeLastUpdateDuration = dateTimes.Item2;

			return new TemplateNodeResultItem(templateNodeQueryInfo, queryResult);
		}

		private QueryInstanceResultInfo GetInstanceResult(
			MultyQueryResultInfo  result,
			InstanceInfo          instance,
			Int64                 queryId,
			TemplateNodeQueryInfo templateNodeQueryInfo,
			List<QueryInfo>       queries
		)
		{
			QueryInstanceResultInfo instanceResult = null;
			Int64                   recordSetCount = 0L;
			ITableRow               meta           = Storage.ReadLastMeta(queryId);

			if (meta != null)
			{
				DateTime timestamp = (DateTime)meta.Values[TableDefinition.DateCreated];

				result.RefreshTimestamp(timestamp);

				if (!string.IsNullOrEmpty(meta.Values[MetaResultTable.ErrorMessageFieldName].ToString()))
				{
					instanceResult = new QueryInstanceResultInfo(
						new ErrorInfo(
							meta.Values[MetaResultTable.ErrorIdFieldName].ToString(),
							meta.Values[MetaResultTable.ErrorCodeFieldName].ToString(),
							meta.Values[MetaResultTable.ErrorMessageFieldName].ToString(),
							(DateTime)meta.Values[TableDefinition.DateCreated]
						),
						instance
					);
				}
				else
				{
					instanceResult = new QueryInstanceResultInfo(instance);
				}

				recordSetCount = (Int64) meta.Values[MetaResultTable.RecordSetCountFieldName];

				DataTable[] dataTables = GetDataTables(
					recordSetCount,
					queryId,
					instance,
					templateNodeQueryInfo
				);

				QueryInfo query = queries.FirstOrDefault(x => x.Source == instance.Type || x.Source == QuerySource.SQLite);

				if (query != null)
				{
					QueryDatabaseResultInfo databaseResult = new QueryDatabaseResultInfo(
						dataTables,
						query.Items.GetQueryItemForVersion(instance.GetServerPropertiesSafe().Version),
						base.GroupDefinition.Name,
						base.GroupDefinition.Id
					);

					instanceResult.AddDatabaseResult(databaseResult);
				}
			}

			return instanceResult;
		}

		private DataTable[] GetDataTables(
			Int64                 recordSetCount,
			Int64                 queryId,
			InstanceInfo          instance,
			TemplateNodeQueryInfo templateNodeQueryInfo
		)
		{
			var dataTables = new DataTable[recordSetCount];

			for (Int64 recordSet = 1L; recordSet <= recordSetCount; recordSet++)
			{
				NormalizeInfo db = StorageManager.GetDbStucture(templateNodeQueryInfo, recordSet, instance);

				if (db != null)
				{
					if (this._histTable.ContainsKey(templateNodeQueryInfo))
					{
						dataTables[recordSet - 1L] = this._histTable[templateNodeQueryInfo];
					}
					else
					{
						dataTables[recordSet - 1L] = base.ReportStorage.ReadResult(db, queryId);
					}
				}
			}

			return dataTables;
		}

		private Dictionary<TemplateNodeQueryInfo, DataTable> ReadHistTable(
			ConnectionGroupInfo            connectionGroup,
			ConcreteTemplateNodeDefinition concreteTemplateNode
		)
		{
			Dictionary<TemplateNodeQueryInfo, DataTable> histTable        = new Dictionary<TemplateNodeQueryInfo, DataTable>();
			TemplateNodeInfo                             templateNodeInfo = concreteTemplateNode.TemplateNode;
			ProgressItem                                 progress         = new ProgressItem();

			using (SqlProcessor sqlProcessor = MsSqlAuditor.GetNewSqlProcessor(new CancellationToken()))
			{
				progress.SetPromisedChildCount(1);

				MultyQueryResultInfo resultQuery = sqlProcessor.ExecuteMultyQuery(
					connectionGroup,
					templateNodeInfo.Queries,
					progress.GetChild(),
					Program.Model.Settings.SystemSettings.MaximumDBRequestsThreadCount,
					true
				);

				if (resultQuery != null)
				{
					if (resultQuery.List != null)
					{
						if (resultQuery.List.Count != 0)
						{
							long     requestId = this.Storage.MetaResultTable.GetMaxRequestId() + 1L;
							DateTime timestamp = DateTime.Now;

							histTable = PrepareHistoryData(resultQuery);

							this.Storage.SaveMeta(
								templateNodeInfo,
								resultQuery,
								requestId,
								timestamp
							);
						}
					}
				}
			}

			return histTable;
		}

		private Dictionary<TemplateNodeQueryInfo, DataTable> PrepareHistoryData(
			MultyQueryResultInfo results
		)
		{
			Dictionary<TemplateNodeQueryInfo, DataTable> result =
				new Dictionary<TemplateNodeQueryInfo, DataTable>();

			foreach (QueryDatabaseResult queryDatabaseResult in results.GetDatabaseResults())
			{
				QueryDatabaseResultInfo dbResult = queryDatabaseResult.Result;

				if (dbResult == null || dbResult.DataTables == null)
				{
					continue;
				}

				TemplateNodeQueryInfo templateNodeQuery = queryDatabaseResult.TemplateNodeQuery;

				foreach (DataTable table in dbResult.DataTables)
				{
					Debug.Assert(table != null);

					if (!result.ContainsKey(templateNodeQuery))
					{
						result.Add(templateNodeQuery, table);
					}
				}
			}

			return result;
		}
	}
}
