using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using MSSQLServerAuditor.Cache;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Tables;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.SQLite.Tables.UserSettings;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.Utils.Cryptography;

namespace MSSQLServerAuditor.SQLite.Databases
{
	public class CurrentStorage : Database
	{
		private static readonly ILog                             Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly        ConcurrentDictionary<Int64, int> _nodesRowCountCache;

		public static CurrentStorage Create(
			string         fileName,
			ICryptoService cryptoService,
			bool           readOnly = false
		)
		{
			CurrentStorage storage = new CurrentStorage(fileName, readOnly)
			{
				CryptoService = cryptoService
			};

			storage.CreateTables();

			return storage;
		}

		private CurrentStorage(
			string fileName,
			bool   readOnly = false
		) : base(
				fileName,
				readOnly
			)
		{
			this._nodesRowCountCache = new ConcurrentDictionary<long, int>();
			this.MemoryCache         = new MemoryCache();
		}

		private void CreateTables()
		{
			this.ServiceInfoTable                         = new ServiceInfoTable(this);
			this.MetaResultTable                          = new MetaResultTable(this);
			this.NodeInstances                            = new NodeInstanceTable(this);
			this.NodeInstanceAttributes                   = new NodeInstanceAttributeTable(this);
			this.UserSettingsTable                        = new UserSettingsTable(this);
			this.ScheduleSettingsTable                    = new ScheduleSettingsTable(this);
			this.TemplateDirectory                        = new TemplateDirectory(this);
			this.TemplateNodeDirectory                    = new TemplateNodeDirectory(this);
			this.ConnectionGroupDirectory                 = new ConnectionGroupDirectory(this);
			this.ServerInstanceDirectory                  = new ServerInstanceDirectory(this);
			this.LoginDirectory                           = new LoginDirectory(this);
			this.QueryGroupDirectory                      = new QueryGroupDirectory(this);
			this.DynamicConnectionDirectory               = new DynamicConnectionDirectory(this);
			this.TemplateNodeQueryDirectory               = new TemplateNodeQueryDirectory(this);
			this.TemplateNodeQueryGroupDirectory          = new TemplateNodeQueryGroupDirectory(this);
			this.TemplateNodeQueryGroupParameterDirectory = new TemplateNodeQueryGroupParameterDirectory(this);
			this.QueryGroupParameterDirectory             = new QueryGroupParameterDirectory(this);
			this.TemplateNodeQueryParameterDirectory      = new TemplateNodeQueryParameterDirectory(this);
			this.QueryParameterDirectory                  = new QueryParameterDirectory(this);
			this.QueryDirectory                           = new QueryDirectory(this);
			this.QueryResultsDirectory                    = new QueryResultsDirectory(this);
			this.LastConnectionTable                      = new LastConnectionTable(this);
			this.LastConnectionProtocolTable              = new LastConnectionProtocolTable(this);
		}

		public LastConnectionTable                      LastConnectionTable         { get; private set; }
		public LastConnectionProtocolTable              LastConnectionProtocolTable { get; private set; }

		public QueryDirectoryBase                       QueryGroupDirectory          { get; private set; }
		public QueryDirectoryBase                       QueryDirectory               { get; private set; }
		public QueryResultsDirectory                    QueryResultsDirectory        { get; private set; }
		public QueryParameterDirectoryBase              QueryParameterDirectory      { get; private set; }
		public QueryParameterDirectoryBase              QueryGroupParameterDirectory { get; private set; }

		public TemplateDirectory                        TemplateDirectory                        { get; private set; }
		public TemplateNodeDirectory                    TemplateNodeDirectory                    { get; private set; }
		public TemplateNodeQueryParameterDirectory      TemplateNodeQueryParameterDirectory      { get; private set; }
		public TemplateNodeQueryGroupParameterDirectory TemplateNodeQueryGroupParameterDirectory { get; private set; }
		public TemplateNodeQueryGroupDirectory          TemplateNodeQueryGroupDirectory          { get; private set; }
		public TemplateNodeQueryDirectory               TemplateNodeQueryDirectory               { get; private set; }

		public DynamicConnectionDirectory               DynamicConnectionDirectory { get; private set; }

		public LoginDirectory                           LoginDirectory           { get; private set; }
		public ServerInstanceDirectory                  ServerInstanceDirectory  { get; private set; }
		public ConnectionGroupDirectory                 ConnectionGroupDirectory { get; private set; }

		public ScheduleSettingsTable                    ScheduleSettingsTable { get; private set; }
		public UserSettingsTable                        UserSettingsTable     { get; private set; }

		public NodeInstanceAttributeTable               NodeInstanceAttributes { get; private set; }
		public NodeInstanceTable                        NodeInstances          { get; private set; }

		public MetaResultTable                          MetaResultTable  { get; private set; }
		public ServiceInfoTable                         ServiceInfoTable { get; private set; }

		internal MemoryCache    MemoryCache   { get; private set; }
		internal ICryptoService CryptoService { get; private set; }

		public override void Initialize()
		{
			var onDiskVersion = this.ServiceInfoTable.ReadDbVesion();

			Debug.Assert(onDiskVersion.IsOlderOrSameAs(DatabaseVersion.Current));

			if (DatabaseVersion.Current.MajorChangedOver(onDiskVersion))
			{
				this.DropAllTables();
				this.ServiceInfoTable.UpdateScheme();
			}

			this.MetaResultTable.UpdateScheme();
			this.NodeInstances.UpdateScheme();
			this.NodeInstanceAttributes.UpdateScheme();
			this.UserSettingsTable.UpdateScheme();
			this.ScheduleSettingsTable.UpdateScheme();
			this.TemplateDirectory.UpdateScheme();
			this.TemplateNodeDirectory.UpdateScheme();
			this.ConnectionGroupDirectory.UpdateScheme();
			this.ServerInstanceDirectory.UpdateScheme();
			this.LoginDirectory.UpdateScheme();
			this.QueryGroupDirectory.UpdateScheme();
			this.DynamicConnectionDirectory.UpdateScheme();
			this.TemplateNodeQueryDirectory.UpdateScheme();
			this.TemplateNodeQueryGroupDirectory.UpdateScheme();
			this.TemplateNodeQueryGroupParameterDirectory.UpdateScheme();
			this.QueryGroupParameterDirectory.UpdateScheme();
			this.TemplateNodeQueryParameterDirectory.UpdateScheme();
			this.QueryParameterDirectory.UpdateScheme();
			this.QueryDirectory.UpdateScheme();
			this.QueryResultsDirectory.UpdateScheme();
			this.LastConnectionTable.UpdateScheme();
			this.LastConnectionProtocolTable.UpdateScheme();

			this.ServiceInfoTable.WriteVersion(DatabaseVersion.Current);

			this.RunBuildScripts();
		}

		private void RunBuildScripts()
		{
			//
			// ticket #390: service requirenments
			// Program.Model is NULL for a service
			//
			if (Program.Model != null)
			{
				if (Program.Model.Settings != null)
				{
					if (Program.Model.Settings.SystemSettings != null)
					{
						string postBuildScript = Program.Model.Settings.SystemSettings.PostBuildCurrentDb;

						if (!String.IsNullOrEmpty(postBuildScript))
						{
							this.RunBuildScripts(postBuildScript);
						}
					}
				}
			}
		}

		internal void SaveMeta(
			TemplateNodeInfo     templateNodeInfo,
			MultyQueryResultInfo results,
			long                 requestId,
			DateTime             timestamp
		)
		{
			Debug.Assert(templateNodeInfo.IsInstance);

			Int64 sessionId          = 1L;
			List<ITableRow> metaRows = new List<ITableRow>();

			foreach (TemplateNodeResultItem tuple in results.List)
			{
				TemplateNodeQueryInfo templateNodeQuery = tuple.TemplateNodeQuery;
				QueryResultInfo       queryResult       = tuple.QueryResult;

				foreach (QueryInstanceResultInfo queryInstanceResult in queryResult.InstancesResult.Values)
				{
					InstanceInfo instance = queryInstanceResult.Instance;

					Int64? queryId = this.QueryDirectory.GetQueryId(
						templateNodeInfo,
						templateNodeQuery,
						instance,
						timestamp,
						false);

					Log.InfoFormat("Instance:'{0}';QueryId:'{1}'",
						instance,
						queryId
					);

					ITableRow row = this.MetaResultTable.GetMetaRow(
						requestId,
						sessionId,
						timestamp,
						queryInstanceResult,
						queryId,
						null
					);

					metaRows.Add(row);
				}
			}

			this.MetaResultTable.ReplaceRows(metaRows);

			this.ResetRowCountCache(templateNodeInfo);
		}

		internal void ResetRowCountCache(TemplateNodeInfo node)
		{
			int oldValue = 0;

			if (node != null)
			{
				if (node.TemplateNodeId != null)
				{
					this._nodesRowCountCache.TryRemove(
						node.TemplateNodeId.Value,
						out oldValue
					);
				}
			}
		}

		internal int? GetRowCount(TemplateNodeInfo node)
		{
			Debug.Assert(node.IsInstance);

			int  cachedValue = 0;
			int? rowsTotal   = null;

			if (node.TemplateNodeId == null)
			{
				return null;
			}

			if (this._nodesRowCountCache.TryGetValue(node.TemplateNodeId.Value, out cachedValue))
			{
				return cachedValue;
			}

			foreach (InstanceInfo serverInstance in node.ConnectionGroup.Connections)
			{
				foreach (TemplateNodeQueryInfo tqi in node.Queries)
				{
					Int64? queryId = this.QueryDirectory.GetQueryId(
						node,
						tqi,
						serverInstance,
						new DateTime(),
						true
					);

					if (queryId == null)
					{
						continue;
					}

					if (rowsTotal == null)
					{
						rowsTotal = 0;
					}

					ITableRow lastMetaRow = this.ReadLastMeta(queryId.Value);

					rowsTotal += lastMetaRow != null
						? (int)(Int64)lastMetaRow.Values[MetaResultTable.RowCountFieldName]
						: 0;
				}
			}

			if (rowsTotal == null)
			{
				return null;
			}

			this._nodesRowCountCache.AddOrUpdate(node.TemplateNodeId.Value, id => rowsTotal.Value, (id, old) => rowsTotal.Value);

			//NodeInstances.SetReportsRowCountTotal(node.PrimaryKey.Value, rowsTotal.Value);

			return rowsTotal.Value;
		}

		public ITableRow ReadLastMeta(Int64 queryId)
		{
			return this.MetaResultTable.GetLastForQuery(queryId);
		}

		public ITableRow ReadLastMeta(
			TemplateNodeInfo      node,
			InstanceInfo          instance,
			TemplateNodeQueryInfo templateNodeQuery
		)
		{
			Int64? queryId = this.QueryDirectory.GetQueryId(
				node,
				templateNodeQuery,
				instance,
				new DateTime(),
				true
			);

			return queryId != null
				? this.ReadLastMeta(queryId.Value)
				: null;
		}

		public DataTable[] ReadSqlCodeGuardResult(
			TemplateNodeSqlGuardQueryInfo guardQueryInfo,
			DataTable                     queryTable,
			List<ParameterValue>          userParams
		)
		{
			var analyzer     = new CodeGuardAnalyzer();
			var summaryTable = new DataTable();

			summaryTable.Columns.Add("SCGObject");
			summaryTable.Columns.Add("SCGErrorRows");

			foreach (var parameterValue in guardQueryInfo.ParameterValues)
			{
				summaryTable.Columns.Add(parameterValue.Name);
			}

			foreach (var parameterValue in userParams)
			{
				summaryTable.Columns.Add(parameterValue.Name);
			}

			var table = new DataTable();

			// 25/08/2014 Aleksey A. Saychenko - object code is not required
			// table.Columns.Add("SCGObject");

			table.Columns.Add("SCGErrorCode");
			table.Columns.Add("SCGRow");
			table.Columns.Add("SCGColumn");
			table.Columns.Add("SCGMessage");

			foreach (var parameterValue in guardQueryInfo.ParameterValues)
			{
				table.Columns.Add(parameterValue.Name);
			}

			foreach (var parameterValue in userParams)
			{
				table.Columns.Add(parameterValue.Name);
			}

			foreach (DataRow row in queryTable.Rows)
			{
				var sqlcode = row[guardQueryInfo.GetQueryCodeColumn()].ToString();
				var result  = analyzer.ProcessSqlQuery(
					sqlcode,
					guardQueryInfo.GetIncludedIssue(),
					guardQueryInfo.GetExcludedIssue()
				);
				var errorRows = new StringBuilder();
				var statements = result.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

				foreach (var statement in statements)
				{
					var newRow = table.NewRow();

					// 25/08/2014 Aleksey A. Saychenko
					// newRow["SCGObject"] = row[guardQueryInfo.GetQueryCodeColumn()];

					var start = statement.IndexOf("(", StringComparison.Ordinal);
					var end = start < 0 ? 0 : statement.IndexOf(")", start, StringComparison.Ordinal);

					if (start > -1 && end > 0)
					{
						newRow["SCGErrorCode"] = statement.Substring(start + 1, end - 1);
					}

					newRow["SCGMessage"] = statement.Substring(end + 1);
					var position = Regex.Match(statement, "([0-9])+:([0-9])+").Value.Split(':');
					newRow["SCGRow"] = position[1];
					newRow["SCGColumn"] = position[0];
					errorRows.AppendFormat(",{0}", position[1]);

					foreach (var parameterValue in guardQueryInfo.ParameterValues)
					{
						newRow[parameterValue.Name] = row[parameterValue.StringValue ?? parameterValue.UserValue];
					}

					foreach (var parameterValue in userParams)
					{
						newRow[parameterValue.Name] = row[parameterValue.StringValue ?? parameterValue.UserValue];
					}

					table.Rows.Add(newRow);
				}

				if (!guardQueryInfo.GetAddSummary())
				{
					continue;
				}

				var newSummaryRow = summaryTable.NewRow();

				newSummaryRow["SCGObject"]    = row[guardQueryInfo.GetObjectColumn()];
				newSummaryRow["SCGErrorRows"] = errorRows.Length > 0 ? errorRows.Remove(0, 1).ToString() : String.Empty;

				summaryTable.Rows.Add(newSummaryRow);
			}

			if (!guardQueryInfo.GetAddSummary())
			{
				return new[] { table };
			}

			return new[] { table, summaryTable };
		}

		public void SaveMeta(
			Int64?           groupGueryId,
			ErrorInfo        error,
			IList<DataTable> tables
		)
		{
			Int64 rowCount       = 0L;
			Int64 recordSetCount = 0L;
			string errorNumber   = null;
			string errorCode     = null;
			string errorMessage  = null;
			ITableRow row        = new TableRow(this.MetaResultTable.TableDefinition);

			row.Values.Add(QueryDirectory.DirectoryTableName.AsFk(),        null);
			row.Values.Add(QueryGroupDirectory.DirectoryTableName.AsFk(),   groupGueryId);
			row.Values.Add(MetaResultTable.RequestIdFieldName,              this.MetaResultTable.GetMaxRequestId() + 1);
			row.Values.Add(MetaResultTable.SessionIdFieldName,              1);
			row.Values.Add(TableDefinition.DateCreated,                     DateTime.Now);

			if (error == null)
			{
				rowCount = tables != null
					? tables.Select(x => x.Rows).Sum(x => x.Count)
					: 0L;

				recordSetCount = tables != null
					? tables.Count
					: 0L;
			}
			else
			{
				errorNumber  = error.Number;
				errorCode    = error.Code;
				errorMessage = error.Message;
			}

			row.Values.Add(MetaResultTable.RowCountFieldName,       rowCount);
			row.Values.Add(MetaResultTable.RecordSetCountFieldName, recordSetCount);
			row.Values.Add(MetaResultTable.ErrorIdFieldName,        errorNumber);
			row.Values.Add(MetaResultTable.ErrorCodeFieldName,      errorCode);
			row.Values.Add(MetaResultTable.ErrorMessageFieldName,   errorMessage);

			this.MetaResultTable.ReplaceRows(new[] { row });
		}

		public DataTable[] ExecuteSql(
			string                          sql,
			IEnumerable<QueryParameterInfo> parameters,
			IEnumerable<ParameterValue>     parameterValues,
			InstanceInfo                    instance
		)
		{
			var result = new List<DataTable>();

			var selectCommand = new SqlSelectCommand(
				this.Connection,
				sql,
				reader =>
				{
					var table = new DataTable();

					table.Load(reader, LoadOption.OverwriteChanges);

					result.Add(table);
				},
				this.GetParameters(parameters, parameterValues, instance)
			);

			selectCommand.Execute(100);

			return result.ToArray();
		}

		public void Save(TemplateNodeInfo templateNodeInfo)
		{
			NodeInstances.Save(templateNodeInfo);

			templateNodeInfo.SaveUserParameters(this);
		}

		public SQLiteParameter[] GetParameters(
			IEnumerable<QueryParameterInfo> parameters,
			IEnumerable<ParameterValue>     parameterValues,
			InstanceInfo                    instance
		)
		{
			var result = new List<SQLiteParameter>();

			if (parameters != null)
			{
				foreach (var parameter in parameters)
				{
					result.Add(
						new SQLiteParameter
						{
							ParameterName = parameter.Name,
							IsNullable    = true,
							DbType        = parameter.Type.ToDbType(),
							Value         = parameter.GetDefaultValue()
						}
					);
				}
			}

			if (parameterValues != null)
			{
				foreach (var value in parameterValues)
				{
					var parameter = (
							from
								SQLiteParameter p
							in
								result
							where
								p.ParameterName == value.Name
							select
								p
						).FirstOrDefault();

					if (parameter != null)
					{
						parameter.Value = value.GetValue(parameter.DbType.ToSqlDbType());
					}
				}
			}

			result.AddRange(this.GetDefaultParameters(instance));

			return result.ToArray();
		}

		private IEnumerable<SQLiteParameter> GetDefaultParameters(InstanceInfo instance)
		{
			List<SQLiteParameter> result = new List<SQLiteParameter>();

			ConnectionGroupInfo connectionGroup = instance.ConnectionGroup;
			connectionGroup.ReadGroupIdFrom(this.ConnectionGroupDirectory);

			long? instanceId = this.ServerInstanceDirectory.GetId(connectionGroup, instance);
			long? loginId    = this.LoginDirectory.GetId(instance);
			long? templateId = this.TemplateDirectory.GetId(connectionGroup);

			result.Add(
				SQLiteHelper.GetParameter(ConnectionGroupDirectory.TableName.AsFk(), connectionGroup.Identity)
			);
			result.Add(
				SQLiteHelper.GetParameter(ServerInstanceDirectory.TableName.AsFk(), instanceId)
			);
			result.Add(
				SQLiteHelper.GetParameter(LoginDirectory.TableName.AsFk(), loginId)
			);
			result.Add(
				SQLiteHelper.GetParameter(TemplateDirectory.TableName.AsFk(), templateId)
			);

			return result;
		}

		public void UpdateTreeNodeTimings(
			TemplateNodeInfo     node,
			MultyQueryResultInfo results
		)
		{
			UpdateTreeNodeTimings(
				node,
				results.NodeLastUpdated,
				results.NodeLastUpdateDuration
			);
		}

		public void UpdateTreeNodeTimings(
			TemplateNodeInfo node,
			DateTime?        lastUpdateDateTime,
			DateTime?        lastUpdateDuration
		)
		{
			NodeInstances.UpdateTreeNodeLastUpdateAndDuration(
				node,
				lastUpdateDateTime,
				lastUpdateDuration
			);
		}

		public void UpdateTreeNodeCounterValue(
			TemplateNodeInfo node,
			int?             counterValue
		)
		{
			int? currentValue = GetTreeNodeCounterValue(node);

			if (!Nullable.Equals(counterValue, currentValue))
			{
				NodeInstances.UpdateTreeNodeCounterValue(node, counterValue);
			}
		}

		public int? GetTreeNodeCounterValue(TemplateNodeInfo node)
		{
			return NodeInstances.GetTreeNodeCounterValue(node);
		}
	}
}
