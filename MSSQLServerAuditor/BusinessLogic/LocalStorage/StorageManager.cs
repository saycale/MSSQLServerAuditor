using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Connections;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.BusinessLogic.LocalStorage
{
	public class StorageManager : IStorageManager
	{
		private static readonly ILog                       Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private const string                               _QueryIdParameterName = "d_Query_id";
		private readonly Dictionary<string, NormalizeInfo> _dbStructures;
		private readonly bool                              _updateHistory;

		public CurrentStorage                              CurrentStorage { get; private set; }
		public ReportStorage                               ReportStorage  { get; private set; }
		public List<HistoryStorage>                        HistoryStorage { get; private set; }

		private StorageManager()
		{
			this._dbStructures  = new Dictionary<string, NormalizeInfo>();
			this._updateHistory = false;
			this.CurrentStorage = null;
			this.ReportStorage  = null;
			this.HistoryStorage = new List<HistoryStorage>();
		}

		public StorageManager(
			MsSqlAuditorModel model,
			bool              updateHistory,
			string            currentStorageFileName,
			string            historyStorageFileNamePattern,
			string            reportStorageFileName
		) : this()
		{
			this.Model = model;

			this._updateHistory = updateHistory;

			this.CurrentStorage = CurrentStorage.Create(
				currentStorageFileName,
				model.CryptoService
			);

			// this.ReportStorage = new ReportStorage(
			//    reportStorageFileName
			// );
			this.ReportStorage = new ReportMemoryStorage(
				reportStorageFileName
			);

			this.CurrentStorage.AttachDatabase(
				reportStorageFileName,
				"report"
			);

			if (
				this._updateHistory
				&& Model.Settings != null
				&& Model.Settings.SystemSettings != null
				&& !string.IsNullOrEmpty(historyStorageFileNamePattern)
			)
			{
				var histStorageSQLiteDbs = Model.Settings.SystemSettings.PostBuildSQLiteDbs;

				foreach (var historyStorage in histStorageSQLiteDbs)
				{
					if (!string.IsNullOrEmpty(historyStorage.FileName) && !string.IsNullOrEmpty(historyStorage.Alias))
					{
						string historyStorageFileName = string.Format(
							historyStorageFileNamePattern,
							historyStorage.FileName
						);

						this.HistoryStorage.Add(
							new HistoryStorage(
								historyStorageFileName,
								historyStorage.Alias,
								historyStorage.PostBuildScript
							)
						);

						this.CurrentStorage.AttachDatabase(
							historyStorageFileName,
							historyStorage.Alias
						);
					}
				}
			}
		}

		public StorageManager(
			MsSqlAuditorModel model,
			FilesProvider     filesProvider,
			bool              updateHistory
		) : this(
				model,
				updateHistory,
				filesProvider.GetCurrentDbFileName(),
				filesProvider.GetHistoricDbFileName(),
				filesProvider.GetReportDbFileName()
			)
		{
		}

		public void InitializeDataBases()
		{
			this.CurrentStorage.Initialize();

			this.ReportStorage.Initialize();

			if (this.HistoryStorage != null)
			{
				this.HistoryStorage.ForEach(x => x.Initialize());
			}
		}

		protected MsSqlAuditorModel Model { get; private set; }

		public void SaveRequestedData(
			TemplateNodeInfo     templateNodeInfo,
			MultyQueryResultInfo results
		)
		{
			Debug.Assert(templateNodeInfo.IsInstance);

			long            requestId = this.CurrentStorage.MetaResultTable.GetMaxRequestId() + 1L;
			DateTime        timestamp = DateTime.Now;
			const long      sessionId = 1L;
			List<ITableRow> metaRows  = new List<ITableRow>();

			foreach (TemplateNodeResultItem nodeResult in results.List)
			{
				TemplateNodeQueryInfo templateNodeQuery = nodeResult.TemplateNodeQuery;
				QueryResultInfo       queryResult       = nodeResult.QueryResult;

				foreach (KeyValuePair<InstanceInfo, QueryInstanceResultInfo> instancePair in queryResult.InstancesResult)
				{
					long                    totalRowsSaved      = 0L;
					InstanceInfo            instance            = instancePair.Key;
					QueryInstanceResultInfo queryInstanceResult = instancePair.Value;

					Int64? queryId = CurrentStorage.QueryDirectory.GetQueryId(
						templateNodeInfo,
						templateNodeQuery,
						instance,
						timestamp,
						false
					);

					Log.InfoFormat("Instance:'{0}';QueryId:'{1}'",
						instance,
						queryId
					);

					if (queryInstanceResult.ErrorInfo == null)
					{
						IEnumerable<QueryDatabaseResultInfo> notEmptyResults = queryInstanceResult.DatabasesResult.Values
							.Where(d => d != null && d.DataTables != null);

						foreach (QueryDatabaseResultInfo databaseResultInfo in notEmptyResults)
						{
							totalRowsSaved += SaveDatabaseResult(
								templateNodeQuery,
								instance,
								databaseResultInfo,
								queryId
							);
						}
					}

					ITableRow metaRow = this.CurrentStorage.MetaResultTable.GetMetaRow(
						requestId,
						sessionId,
						timestamp,
						queryInstanceResult,
						queryId,
						totalRowsSaved
					);

					metaRows.Add(metaRow);
				}
			}

			this.CurrentStorage.MetaResultTable.ReplaceRows(metaRows);
			this.CurrentStorage.ResetRowCountCache(templateNodeInfo);
		}

		private long SaveDatabaseResult(
			TemplateNodeQueryInfo   templateNodeQuery,
			InstanceInfo            instance,
			QueryDatabaseResultInfo dbResult,
			Int64?                  queryId
		)
		{
			long  totalRows = 0L;
			Int64 recordSet = 1L;

			foreach (DataTable table in dbResult.DataTables)
			{
				Debug.Assert(table != null);

				long? savedRows = this.SaveResults(
					instance,
					templateNodeQuery,
					recordSet,
					table,
					queryId
				);

				if (savedRows.HasValue)
				{
					totalRows += savedRows.Value;
				}

				recordSet++;
			}

			this.UpdateHistory(
				instance,
				templateNodeQuery,
				queryId.Value
			);

			return totalRows;
		}

		public void SerializeData(
			TemplateNodeInfo     templateNodeInfo,
			MultyQueryResultInfo queriesResult
		)
		{
			SaveDynamicConnections(templateNodeInfo, queriesResult);

			SaveRequestedData(templateNodeInfo, queriesResult);

			this.CurrentStorage.UpdateTreeNodeTimings(
				templateNodeInfo,
				queriesResult
			);
		}

		public void FlushDbStructureCache()
		{
			this._dbStructures.Clear();
		}

		public int? GetDataRowCount(
			ConnectionGroupInfo            connectionGroup,
			ConcreteTemplateNodeDefinition concreteTemplateNode
		)
		{
			return this.CurrentStorage.GetRowCount(concreteTemplateNode.TemplateNode);
		}

		public MultyQueryResultInfo ReadCurrentResult(
			ConnectionGroupInfo            connectionGroup,
			ConcreteTemplateNodeDefinition concreteTemplateNode
		)
		{
			MultiQueryResultReader multiQueryResultReader = new MultiQueryResultReader(Model, this);

			return multiQueryResultReader.Read(connectionGroup, concreteTemplateNode);
		}

		private Int64? SaveResults(
			InstanceInfo          instance,
			TemplateNodeQueryInfo templateNodeQuery,
			Int64                 recordSet,
			DataTable             dataTable,
			Int64?                queryId
		)
		{
			Int64? lSavedRows = 0L;

			NormalizeInfo dbStructure = this.GetDbStucture(
				templateNodeQuery,
				recordSet,
				instance,
				dataTable
			);

			// Log.InfoFormat("Instance:'{0}';templateNode:'{1}';templateNodeQuery:'{2}';Table:'{3}';PrimaryKey:'{4}'",
			//    instance.Name,
			//    templateNode,
			//    templateNodeQuery,
			//    dbStructure.TableName,
			//    queryId
			// );

			if (queryId != null)
			{
				this.CurrentStorage.QueryResultsDirectory.GetId(
					queryId.Value,
					recordSet,
					dbStructure.TableName
				);

				lSavedRows = this.ReportStorage.SaveResults(
					queryId.Value,
					dbStructure,
					dataTable
				);
			}

			return lSavedRows;
		}

		private void UpdateHistory(
			InstanceInfo          instance,
			TemplateNodeQueryInfo templateNodeQuery,
			long                  queryId
		)
		{
			if (this.HistoryStorage != null && this._updateHistory)
			{
				List<QueryInfo> queries = Model.GetQueryByTemplateNodeQueryInfo(templateNodeQuery);
				QueryInfo       query   = queries.FirstOrDefault(
					x =>
						x.Source == instance.Type
						||
						x.Source == QuerySource.SQLite
				);

				try
				{
					this.SaveHistoryData(
						query,
						instance,
						queryId,
						templateNodeQuery
					);
				}
				catch (Exception ex)
				{
					Log.ErrorFormat("Update History exception:{0};query:{1}",
						ex,
						query
					);

					if (!ex.Data.Contains("IgnoreLog"))
					{
						Log.ErrorFormat("Update History exception:{0};query:{1}",
							ex,
							query
						);
					}
				}
			}
		}

		private void SaveHistoryData(
			QueryInfo             query,
			InstanceInfo          instance,
			long                  queryId,
			TemplateNodeQueryInfo templateNodeQuery
		)
		{
			QueryInfo queryInfo            = new QueryInfo { Source = QuerySource.SQLite };
			Regex     regex                = new Regex(@"\[\$\{(?<QueryName>[\w]+)\}\$_\$\{(?<RecordSetNumber>[\w]+)\}\$\]");
			string    strQueryName         = String.Empty;
			Int64     intRecordSetNumber   = 0L;
			string    strReplacedTableName = String.Empty;

			if (query.FillStatementList == null)
			{
				return;
			}

			List<HistoryFillStatement> list = query.FillStatementList.GetSortedStatements();

			List<QueryParameterInfo> parameters = new List<QueryParameterInfo>
			{
				new QueryParameterInfo
				{
					Name = _QueryIdParameterName,
					Type = SqlDbType.BigInt
				}
			};

			List<ParameterValue> paramterValues = new List<ParameterValue>
			{
				new ParameterValue
				{
					Name        = _QueryIdParameterName,
					StringValue = queryId.ToString()
				}
			};

			// string newTableName = String.Format("[{0}]",
			//    this.GetTableName(templateNodeQuery, recordSet) ?? "_unknown_table_"
			// );

			// string oldTableName = "[${" + query.Name + "}$_${" + recordSet + "}$]";

			// string oldTableName = String.Format("[${{{0}}}$_${{{1}}}$]",
			//   query.Name ?? "_unknown_table_",
			//   recordSet
			//);

			// Log.DebugFormat("oldTableName:'{0}',newTableName:'{1}'",
			//    oldTableName,
			//    newTableName
			//);

			foreach (HistoryFillStatement statement in list)
			{
				QueryItemInfo queryItem = new QueryItemInfo();

				queryItem.ParentQuery = queryInfo;
				queryItem.Text        = statement.Text;

				// queryItem.Text        = statement.Text.Replace(oldTableName, newTableName);

				// Regex regex = new Regex("[\$\{([\w]+)\}\$_\$\{([\w]+)\}\$]");

				// Log.InfoFormat("regex:'{0}'",
				//    regex
				// );

				var results = regex.Matches(statement.Text);

				foreach (Match match in results)
				{
					// Log.InfoFormat("match:'{0}';match.Value:'{1}'",
					// 	match,
					// 	match.Value
					// );

					strQueryName       = match.Groups["QueryName"].Value;
					intRecordSetNumber = Int64.Parse(match.Groups["RecordSetNumber"].Value);

					// Log.InfoFormat("strQueryName:'{0}';intRecordSetNumber:'{1}'",
					//    strQueryName,
					//    intRecordSetNumber
					// );

					if (String.Equals(strQueryName, query.Name, StringComparison.OrdinalIgnoreCase))
					{
						// Log.InfoFormat("matches:strQueryName:'{0}';query.Name:'{1}'",
						//    strQueryName,
						//    query.Name
						// );

						strReplacedTableName = string.Format(
							"[{0}]",
							this.GetTableName(
								templateNodeQuery,
								intRecordSetNumber
							)
						);

						// Log.InfoFormat("strReplacedTableName:'{0}'",
						//    strReplacedTableName
						// );

						// Log.InfoFormat("queryItem.Text:'{0}'",
						//    queryItem.Text
						// );

						queryItem.Text = queryItem.Text.Replace(
							match.Value,
							strReplacedTableName
						);

						// Log.InfoFormat("queryItem.Text:'{0}'",
						//    queryItem.Text
						// );
					}
				}

				this.ExecuteSql(
					instance,
					queryItem,
					parameters,
					paramterValues
				);
			}
		}

		internal DataTable[] ExecuteSql(
			InstanceInfo                    instance,
			QueryItemInfo                   sql,
			IEnumerable<QueryParameterInfo> parameters = null,
			IEnumerable<ParameterValue>     parameterValues = null
		)
		{
			bool shouldExecute = true;

			if (sql == null)
			{
				throw new Exception("There is no sql statement to execute (QueryItemInfo == null).");
			}

			// Log.InfoFormat("instance:'{0}';sql.Text:'{1}'",
			// 	instance,
			// 	sql.Text
			// );

			if (sql.ExecuteIfSqlText != null)
			{
				var clone = sql.Clone();

				clone.ExecuteIfSqlText = null;
				clone.Text             = sql.ExecuteIfSqlText;

				DataTable[] tt = ExecuteSql(
					instance,
					clone,
					parameters,
					parameterValues
				);

				if (tt.Length > 0 && tt[0].Rows.Count > 0)
				{
					shouldExecute = (int)(tt[0].Rows[0][0]) == 1;
				}
			}

			if (shouldExecute)
			{
				return this.CurrentStorage.ExecuteSql(
					sql.Text,
					parameters,
					parameterValues,
					instance
				);
			}

			return new DataTable[0];
		}

		public NormalizeInfo GetDbStucture(
			TemplateNodeQueryInfo templateNodeQueryInfo,
			Int64                 recordSet,
			InstanceInfo          instance,
			DataTable             dataTable = null
		)
		{
			List<QueryInfo> queries      = Model.GetQueryByTemplateNodeQueryInfo(templateNodeQueryInfo);
			QueryInfo       query        = queries.FirstOrDefault(x => x.Source == instance.Type || x.Source == QuerySource.SQLite);
			string          tableName    = this.GetTableName(templateNodeQueryInfo, recordSet);
			string          tableIndexes = string.Empty;

			if (query != null)
			{
				tableIndexes = query.QueryIndexFileds;
			}

			lock (this._dbStructures)
			{
				if (!this._dbStructures.ContainsKey(tableName))
				{
					NormalizeInfo definedRecordSet = query.GetDbStructureForRecordSet(recordSet);

					if (definedRecordSet != null)
					{
						var clone = definedRecordSet.Clone();
						clone.TableName = tableName;
						this._dbStructures.Add(tableName, clone);
					}
					else
					{
						if (dataTable == null)
						{
							NormalizeInfo result     = new NormalizeInfo();
							NormalizeFieldInfo field = new NormalizeFieldInfo();

							result.TableName = tableName;
							result.Fields    = new List<NormalizeFieldInfo>();
							field.Name       = "*";
							field.Type       = SqlDbType.NVarChar;

							result.Fields.Add(field);

							result.TableIndexFileds = tableIndexes;

							return result;
						}

						NormalizeInfo dbStructure = new NormalizeInfo
						{
							TableName        = tableName,
							IsAutoGenerated  = true,
							Fields           = new List<NormalizeFieldInfo>(),
							ChildDirectories = new List<NormalizeInfo>(),
							RecordSet        = recordSet,
							QueryName        = query.Name,
							TableIndexFileds = tableIndexes
						};

						foreach (DataColumn column in dataTable.Columns)
						{
							NormalizeFieldInfo field = new NormalizeFieldInfo();

							field.Name     = column.ColumnName;
							field.Type     = column.DataType.ToSqlDbType();
							field.IsUnique = true;

							dbStructure.Fields.Add(field);
						}

						this._dbStructures.Add(tableName, dbStructure);
					}
				}
			}

			NormalizeInfo structure = this._dbStructures[tableName];

			return structure;
		}

		public string GetTableName(
			TemplateNodeQueryInfo templateNodeQueryInfo,
			Int64                 recordSet
		)
		{
			TemplateNodeInfo templateNode = templateNodeQueryInfo.TemplateNode;

			Debug.Assert(templateNodeQueryInfo.TemplateNode.IsInstance);

			long? id = this.CurrentStorage.TemplateNodeDirectory.GetId(
				templateNode.ConnectionGroup,
				templateNode.Template
			);

			string tableName = String.Format(
				"t_{0}_{1}_{2}_q{3}_r{4}",
				templateNode.TemplateId ?? String.Empty,
				id,
				templateNodeQueryInfo.QueryName,
				templateNodeQueryInfo.Id ?? "0",
				recordSet
			);

			// Log.InfoFormat("tableName:'{0}'",
			// 	tableName
			// );

			return tableName;
		}

		private void SaveDynamicConnections(
			TemplateNodeInfo     templateNode,
			MultyQueryResultInfo queriesResult
		)
		{
			foreach (TemplateNodeResultItem templateNodeResultItem in queriesResult.List)
			{
				TemplateNodeQueryInfo queryInfo         = templateNodeResultItem.TemplateNodeQuery;
				TemplateNodeInfo      templateNodeClone = templateNode;

				if (queryInfo.ConnectionsSelectId != null)
				{
					QueryResultInfo queryResult = templateNodeResultItem.QueryResult;

					foreach (InstanceInfo instanceInfo in templateNodeClone.ConnectionGroup.Connections)
					{
						Int64? destParentQueryId = this.CurrentStorage.QueryDirectory.GetQueryId(
							templateNode,
							queryInfo,
							instanceInfo,
							DateTime.Now,
							false
						);

						foreach (KeyValuePair<InstanceInfo, QueryInstanceResultInfo> queryInstanceResultInfo in queryResult.InstancesResult)
						{
							InstanceInfo selectConnectionInstance = queryInstanceResultInfo.Key;

							Int64? dynamicQueryId = this.CurrentStorage.QueryDirectory.GetQueryId(
								templateNodeClone,
								queryInfo,
								selectConnectionInstance,
								DateTime.Now,
								false
							);

							if (destParentQueryId == null)
							{
								continue;
							}

							DynamicConnection dynamicConnection = new DynamicConnection(
								selectConnectionInstance.Name,
								selectConnectionInstance.Type.ToString(),
								selectConnectionInstance.IsODBC,
								dynamicQueryId
							);

							this.CurrentStorage.DynamicConnectionDirectory.UpdateConnection(
								destParentQueryId.Value,
								dynamicConnection
							);
						}
					}
				}
			}
		}
	}
}
