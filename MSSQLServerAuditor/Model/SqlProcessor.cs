//#define PROXY_CONNECTION
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MSSQLServerAuditor.BusinessLogic.LocalStorage;
using MSSQLServerAuditor.Model.Commands;
using MSSQLServerAuditor.Model.Connections;
using MSSQLServerAuditor.Model.Connections.Factories;
using MSSQLServerAuditor.Model.Connections.Query;
using MSSQLServerAuditor.Model.Groups;
using MSSQLServerAuditor.Model.Queries;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Sql processor
	/// </summary>
	public sealed class SqlProcessor : IDisposable
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly MsSqlAuditorModel       _model;
		private readonly IQueryConnectionFactory _queryConnectionFactory;
		private          CancellationToken       _cancellationToken;
		private bool                             _skipMsSqlQueries;
		private bool                             _disposed; // to detect redundant calls

		/// <summary>
		/// Default constructor
		/// </summary>
		private SqlProcessor()
		{
			this._model             = null;
			this._skipMsSqlQueries  = false;
			this._disposed          = false;
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="model">Model</param>
		/// <param name="cancellationToken">Cancellation token</param>
		public SqlProcessor(
			MsSqlAuditorModel model,
			CancellationToken cancellationToken
		) : this()
		{
			this._model             = model;
			this._cancellationToken = cancellationToken;

#if (PROXY_CONNECTION)
			this._queryConnectionFactory =
				new CachingQueryConnectionFactory(
					new QueryConnectionFactory(this._model)
			);
#else
			this._queryConnectionFactory = new QueryConnectionFactory(this._model);
#endif
		}

		/// <summary>
		/// Executes multyquery
		/// </summary>
		/// <param name="connectionGroup">Connection group</param>
		/// <param name="templateNodeQueryInfos">Template nodes</param>
		/// <param name="progress">Progress item</param>
		/// <param name="maxthreadCount">Maximum thread</param>
		/// <returns>Multyquery result</returns>
		public MultyQueryResultInfo ExecuteMultyQuery(
			ConnectionGroupInfo         connectionGroup,
			List<TemplateNodeQueryInfo> templateNodeQueryInfos,
			ProgressItem                progress,
			int                         maxthreadCount,
			bool                        checkHist = false
		)
		{
			Settings.InstanceTemplate settings = null;

			////////////////////////////////////////////////////////////////////////////////////////
			// string strLogMessage = "DEBUG:MSSQLServerAuditor.Model.ExecuteMultyQuery(1)";
			// strLogMessage = strLogMessage + ";GD:" + connectionGroup.ToString();
			// log.Debug(strLogMessage);
			////////////////////////////////////////////////////////////////////////////////////////

			var result = new MultyQueryResultInfo();

			if (templateNodeQueryInfos.Count > 0)
			{
				progress.SetPromisedChildCount(templateNodeQueryInfos.Count);

				foreach (var qi in templateNodeQueryInfos)
				{
					TemplateNodeQueryInfo queryInfo   = qi;
					QueryResultInfo       queryResult = null;

					try
					{
						var queries = this._model.GetQueryByTemplateNodeQueryInfo(queryInfo);

						if (this._skipMsSqlQueries)
						{
							queries.RemoveAll(
								x => (
									x.Source == QuerySource.MSSQL
									|| x.Source == QuerySource.TDSQL
								)
							);
						}

						if (checkHist)
						{
							queries.RemoveAll(x => (x.Source != QuerySource.SQLite));
						}
						else
						{
							queries.RemoveAll(x => (x.Source == QuerySource.SQLite));
						}

						if (queryInfo.ConnectionsSelectId == null)
						{
							if (!queries.Any(
								x => connectionGroup.Connections.Select(y => y.Type).Contains(x.Source)
								|| x.Source == QuerySource.SQLite
							))
							{
								continue;
							}
						}

						// var settings = Program.Model.TemplateSettings.UserSettings.FirstOrDefault(i =>
						//    i.TemplateName == connectionGroup.TemplateFileName
						//    && i.Connection.ParentKey == queryInfo.TemplateNode.IdsHierarchy
						// );

						if (Program.Model != null)
						{
							settings = Program.Model.TemplateSettings.UserSettings.FirstOrDefault(i =>
								i.TemplateName == connectionGroup.TemplateFileName
								&& i.Connection.ParentKey == queryInfo.TemplateNode.IdsHierarchy
							);
						}
						else
						{
							settings = this._model.TemplateSettings.UserSettings.FirstOrDefault(i =>
								i.TemplateName == connectionGroup.TemplateFileName
								&& i.Connection.ParentKey == queryInfo.TemplateNode.IdsHierarchy
							);
						}

						queryInfo.ReadParametersFrom(settings);

						string connectionsSelectId = queryInfo.ConnectionsSelectId;

						if (connectionsSelectId != null)
						{
							queryResult = ExecuteConnectionsSelectQuery(
								queryInfo,
								connectionGroup,
								queries,
								maxthreadCount
							);
						}
						else
						{
							queryResult = ExecuteQuery(
								connectionGroup,
								queries,
								QueryExecutionParams.CreateFrom(queryInfo),
								maxthreadCount, progress.GetChild()
							);
						}
					}
					catch (OperationCanceledException ex)
					{
						log.Error(queryInfo.ToString(), ex);

						throw;
					}
					catch (AggregateException ex)
					{
						if (ex.InnerExceptions.All(e => e is OperationCanceledException))
						{
							throw;
						}

						queryResult = new QueryResultInfo(
							new ErrorInfo(
								ex.InnerExceptions.FirstOrDefault(
									e => !(e is OperationCanceledException)
								)
							)
						);

						progress.GetChild().SetProgress(100);

						log.Error(queryInfo.ToString(), ex);
					}
					catch (Exception ex)
					{
						queryResult = new QueryResultInfo(new ErrorInfo(ex));

						progress.GetChild().SetProgress(100);

						log.Error(queryInfo.ToString(), ex);
					}

					result.Add(new TemplateNodeResultItem(queryInfo, queryResult));
				}
			}
			else
			{
				progress.SetProgress(100);
			}

			return result;
		}

		public QueryResultInfo ExecuteConnectionsSelectQuery(
			TemplateNodeQueryInfo queryInfo,
			ConnectionGroupInfo   connectionGroup,
			List<QueryInfo>       queries,
			int                   maxthreadCount
		)
		{
			const string colConnectionName   = "ConnectionName";
			const string colConnectionType   = "ConnectionType";
			const string colConnectionIsOdbc = "IsOdbcConnection";

			IStorageManager storageManager = this._model.GetVaultProcessor(connectionGroup);
			CurrentStorage storage = storageManager.CurrentStorage;

			TemplateNodeQueryInfo connectionsQueryInfo = queryInfo.GetParentConnectionSelectQuery();

			List<QueryInfo> connectionsQueries = this._model.GetQueryByTemplateNodeQueryInfo(connectionsQueryInfo);

			QueryResultInfo connectionsQueryResult = ExecuteQuery(
				connectionGroup,
				connectionsQueries,
				QueryExecutionParams.CreateFrom(connectionsQueryInfo),
				maxthreadCount
			);

			QueryResultInfo queryResult = new QueryResultInfo();

			foreach (KeyValuePair<InstanceInfo, QueryInstanceResultInfo> resultInfo in connectionsQueryResult.InstancesResult)
			{
				QueryInstanceResultInfo instanceResult = resultInfo.Value;

				if (instanceResult.ErrorInfo == null)
				{
					InstanceInfo mainConnection = resultInfo.Key;

					Int64? parentQueryId  = storage.QueryDirectory.GetQueryId(
						queryInfo.TemplateNode,
						queryInfo,
						mainConnection,
						DateTime.Now,
						false
					);

					List<DynamicConnection> dynamicConnections = new List<DynamicConnection>();

					foreach (KeyValuePair<string, QueryDatabaseResultInfo> databaseResultInfo in instanceResult.DatabasesResult)
					{
						QueryDatabaseResultInfo queryDatabaseResultInfo = databaseResultInfo.Value;
						DataTable[]             dataTables              = queryDatabaseResultInfo.DataTables;

						if (dataTables != null)
						{
							foreach (DataTable dataTable in dataTables)
							{
								bool colConnectionNameExists = dataTable.Columns.Contains(colConnectionName);
								bool colConnectionTypeExists = dataTable.Columns.Contains(colConnectionType);
								bool colConnectionOdbcExists = dataTable.Columns.Contains(colConnectionIsOdbc);

								foreach (DataRow row in dataTable.Rows)
								{
									string connectionName   = colConnectionNameExists ? row[colConnectionName].ToString() : string.Empty;
									string connectionType   = colConnectionTypeExists ? row[colConnectionType].ToString() : QuerySource.MSSQL.ToString();
									bool   isOdbcConnection = false;

									if (colConnectionOdbcExists)
									{
										bool.TryParse(row[colConnectionIsOdbc].ToString(), out isOdbcConnection);
									}

									QuerySource sourceType;

									if (!Enum.TryParse(connectionType, true, out sourceType))
									{
										sourceType = QuerySource.MSSQL;
									}

									InstanceInfo selectConnectionInstance = InstanceInfoResolver.ResolveDynamicInstance(
										connectionName,
										sourceType,
										isOdbcConnection
									);

									selectConnectionInstance.ConnectionGroup = connectionGroup;

									if (selectConnectionInstance.IsEnabled)
									{
										var query = queries.FirstOrDefault(
											x => x.Source == (selectConnectionInstance.Type) || x.Source == QuerySource.SQLite
										);

										if (query == null)
										{
											continue;
										}

										InstanceVersion ver = selectConnectionInstance.InitServerProperties(
											storage,
											this._model.Settings.SqlTimeout
										).Version;

										QueryItemInfo queryItem = query.Items.GetQueryItemForVersion(ver);

										QueryInstanceResultInfo instanceResultInfo = GetConnectionSelectResults(
											selectConnectionInstance,
											query,
											queryItem,
											queryInfo
										);

										queryResult.AddInstanceResult(instanceResultInfo);

										Int64? dynamicQueryId = storage.QueryDirectory.GetQueryId(
											queryInfo.TemplateNode,
											queryInfo,
											selectConnectionInstance,
											DateTime.Now,
											false
										);

										if (parentQueryId.HasValue && dynamicQueryId.HasValue)
										{
											DynamicConnection dynamicConnection = new DynamicConnection(
												connectionName,
												connectionType,
												isOdbcConnection,
												dynamicQueryId
											);

											dynamicConnections.Add(dynamicConnection);
										}
									}
								}
							}
						}
					}

					if (dynamicConnections.Count > 0)
					{
						Int64                      pqId                       = parentQueryId.Value;
						DynamicConnectionDirectory dynamicConnectionDirectory = storage.DynamicConnectionDirectory;

						dynamicConnectionDirectory.RemoveConnections(pqId);
						dynamicConnectionDirectory.UpdateConnections(pqId, dynamicConnections);
					}
				}
			}

			return queryResult;
		}

		public QueryInstanceResultInfo GetConnectionSelectResults(
			InstanceInfo          selectConnectionInstance,
			QueryInfo             queryInfo,
			QueryItemInfo         queryItem,
			TemplateNodeQueryInfo query
		)
		{
			QueryDatabaseResultInfo dbResultInfo;

			try
			{
				DataTable[] tables = ExecuteSql(
					selectConnectionInstance,
					queryItem,
					query.TemplateNode.GetDefaultDatabase(),
					queryInfo.Parameters,
					query.ParameterValues
				);

				if (tables != null && tables.Length < 0)
				{
					throw new InvalidTemplateException(query + " returned no recordsets.");
				}

				dbResultInfo = new QueryDatabaseResultInfo(tables, queryItem);
			}
			catch (Exception exc)
			{
				ErrorInfo error = new ErrorInfo(exc);

				dbResultInfo = new QueryDatabaseResultInfo(error, queryItem);
			}

			QueryInstanceResultInfo instanceResultInfo = new QueryInstanceResultInfo(selectConnectionInstance);

			instanceResultInfo.AddDatabaseResult(dbResultInfo);

			return instanceResultInfo;
		}

		/// <summary>
		/// Executes multyquery for database
		/// </summary>
		/// <param name="group">Database definition</param>
		/// <param name="templateNodeQueryInfos">Template nodes</param>
		/// <param name="progress">Progress item</param>
		/// <returns>Multyquery result</returns>
		public MultyQueryResultInfo ExecuteMultyQuery(
			GroupDefinition             @group,
			List<TemplateNodeQueryInfo> templateNodeQueryInfos,
			ProgressItem                progress
		)
		{
			////////////////////////////////////////////////////////////////////////////////////////
			// string strLogMessage = "DEBUG:MSSQLServerAuditor.Model.ExecuteMultyQuery(2)";
			// strLogMessage = strLogMessage + ";GD:" + @group.ToString();
			// log.Debug(strLogMessage);
			////////////////////////////////////////////////////////////////////////////////////////

			MultyQueryResultInfo result = new MultyQueryResultInfo();

			progress.SetPromisedChildCount(templateNodeQueryInfos.Count);

			foreach (TemplateNodeQueryInfo queryInfo in templateNodeQueryInfos)
			{
				List<QueryInfo> queries = this._model.GetQueryByTemplateNodeQueryInfo(queryInfo);

				QueryInfo query = queries.FirstOrDefault(x =>
					x.Source == (@group.Instance.Type) || x.Source == QuerySource.SQLite);

				if ((query.Source == QuerySource.MSSQL || query.Source == QuerySource.TDSQL) && this._skipMsSqlQueries)
				{
					continue;
				}

				QueryExecutionParams parameters = QueryExecutionParams.CreateFrom(queryInfo);

				QueryResultInfo queryResult = ExecuteQuery(
					@group,
					query,
					parameters,
					progress.GetChild()
				);

				result.Add(new TemplateNodeResultItem(queryInfo, queryResult));

				if (this._cancellationToken.IsCancellationRequested)
				{
					break;
				}
			}

			return result;
		}

		private QueryResultInfo ExecuteQuery(
			GroupDefinition      groupDefinition,
			QueryInfo            query,
			QueryExecutionParams parameters,
			ProgressItem         progress = null
		)
		{
			QueryResultInfo queryResult = new QueryResultInfo();
			InstanceInfo instance       = groupDefinition.Instance;

			QueryExecutorFactory factory = new QueryExecutorFactory(
				instance,
				this.ExecuteQueryItem,
				this.ExecuteSql
			);

			BaseQueryExecutor executor = factory.GetExecutor(
				query.Scope
			);

			CurrentStorage storage = this._model.GetVaultProcessor(
				instance.ConnectionGroup).CurrentStorage;

			ServerProperties props = instance.InitServerProperties(storage);
			QueryInstanceResultInfo instanceResult = executor.ExecuteQuerySimple(
				query,
				parameters,
				props.Version,
				progress,
				groupDefinition
			);

			queryResult.AddInstanceResult(instanceResult);

			return queryResult;
		}

		private QueryResultInfo ExecuteQuery(
			ConnectionGroupInfo   connectionGroupInfo,
			List<QueryInfo>       queries,
			QueryExecutionParams  parameters,
			int                   maxthreadCount,
			ProgressItem          progress = null
		)
		{
			var                result      = new QueryResultInfo();
			List<InstanceInfo> connections = connectionGroupInfo.Connections;

			if (progress != null)
			{
				progress.SetPromisedChildCount(connections.Count);
			}

			ParallelOptions op = new ParallelOptions { MaxDegreeOfParallelism = (maxthreadCount == 0 ? Int32.MaxValue : maxthreadCount) };

			Parallel.ForEach(connections, op, (connection) =>
			{
				var query = queries.FirstOrDefault(x => x.Source == (connection.Type) || x.Source == QuerySource.SQLite);

				if (query != null)
				{
					ProgressItem subProgress = null;

					if (progress != null)
					{
						subProgress = progress.GetChild();
					}

					result.AddInstanceResult(ExecuteQuery(connection, query, parameters, subProgress));
				}
			});

			return result;
		}

		/// <summary>
		/// The execute query.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="query">The query.</param>
		/// <param name="parameterValues">The parameter values.</param>
		/// <param name="progress">The progress.</param>
		/// <returns>
		/// The <see cref="QueryInstanceResultInfo" />.
		/// </returns>
		private QueryInstanceResultInfo ExecuteQuery(
			InstanceInfo         instance,
			QueryInfo            query,
			QueryExecutionParams parameters,
			ProgressItem         progress = null
		)
		{
			try
			{
				var factory = new QueryExecutorFactory(instance, this.ExecuteQueryItem, this.ExecuteSql);

				CurrentStorage storage = this._model.GetVaultProcessor(
					instance.ConnectionGroup).CurrentStorage;

				ServerProperties props = instance.InitServerProperties(storage);
				return factory
					.GetExecutor(query.Scope)
					.ExecuteQuery(query, parameters, props.Version, progress);
			}
			catch (OperationCanceledException ex)
			{
				return new QueryInstanceResultInfo(new ErrorInfo(ex), instance);
			}
			catch (Exception ex)
			{
				if (instance.IsEnabled && !ex.Data.Contains("IgnoreLog"))
				{
					log.Error(query.ToString(), ex);
				}

				if (progress != null)
				{
					progress.SetProgressCanceled();
				}

				return new QueryInstanceResultInfo(new ErrorInfo(ex), instance);
			}
		}

		private QueryDatabaseResultInfo ExecuteQueryItem(
			InstanceInfo                    connection,
			QueryItemInfo                   queryItem,
			string                          database,
			string                          databaseId,
			IEnumerable<QueryParameterInfo> parameters,
			IEnumerable<ParameterValue>     parameterValues,
			ProgressItem                    progress = null
		)
		{
			try
			{
				var table = ExecuteSql(
					connection,
					queryItem,
					database,
					parameters,
					parameterValues,
					progress
				);

				return new QueryDatabaseResultInfo(table, queryItem, database, databaseId);
			}
			catch (OperationCanceledException ex)
			{
				return new QueryDatabaseResultInfo(new ErrorInfo(ex), queryItem, database, databaseId);
			}
			catch (Exception ex)
			{
				log.Error(ex);

				if (!ex.Data.Contains("IgnoreLog"))
				{
					log.Error(queryItem.ToString() + Environment.NewLine
						+ "connection.Instance=" + connection.Instance +
						Environment.NewLine
						+ " database=" + database + Environment.NewLine
						+ " connection.Authentication.Username=" +
						connection.Authentication.Username, ex);
				}

				if (progress != null)
				{
					progress.SetProgress(100);
				}

				return new QueryDatabaseResultInfo(new ErrorInfo(ex), queryItem, database, databaseId);
			}
		}

		internal DataTable[] ExecuteSql(
			InstanceInfo                    instance,
			QueryInfo                       query,
			string                          database = null,
			IEnumerable<QueryParameterInfo> parameters = null,
			IEnumerable<ParameterValue>     parameterValues = null,
			ProgressItem                    progress = null,
			bool                            fromGroupSelect = false
		)
		{
			InstanceVersion ver           = instance.ServerProperties.Version;
			QueryItemInfo exactSqlVersion = query.Items.GetQueryItemForVersion(ver);

			if (exactSqlVersion == null)
			{
				throw new InvalidOperationException("No <sql-select-text> found for version " + ver + " in <sql-select name=" + query.Name);
			}

			return ExecuteSql(
				instance,
				exactSqlVersion,
				database,
				parameters,
				parameterValues,
				progress,
				fromGroupSelect
			);
		}

		internal DataTable[] ExecuteSql(
			InstanceInfo                    instance,
			QueryItemInfo                   sql,
			string                          database = null,
			IEnumerable<QueryParameterInfo> parameters = null,
			IEnumerable<ParameterValue>     parameterValues = null,
			ProgressItem                    progress = null,
			bool                            fromGroupSelect = false
		)
		{
			Exception       gotException    = null;
			List<DataTable> tables          = new List<DataTable>();

			this._cancellationToken.ThrowIfCancellationRequested();

			try
			{
				if (sql == null)
				{
					throw new Exception("There is no sql statement to execute (QueryItemInfo == null).");
				}

				List<Tuple<int, string>> parametersQueueForODBC;

				using (IQueryConnection connection = _queryConnectionFactory.CreateQueryConnection(sql.ParentQuery.Source, instance))
				{
					using (IQueryCommand sqlCommand = connection.GetCommand(sql.Text, this._model.Settings.SqlTimeout, parameters, out parametersQueueForODBC))
					{
						using (new TryFinally(connection.Open, connection.Close))
						{
							connection.ChangeDatabase(database);

							var shouldExecute = true;

							if (sql.ExecuteIfSqlText != null)
							{
								var clone = sql.Clone();

								clone.ExecuteIfSqlText = null;
								clone.Text             = sql.ExecuteIfSqlText;

								DataTable[] tt = ExecuteSql(
									instance,
									clone,
									database,
									parameters,
									parameterValues,
									progress,
									fromGroupSelect
								);

								if (tt.Length > 0 && tt[0].Rows.Count > 0)
								{
									shouldExecute = (int)(tt[0].Rows[0][0]) == 1;
								}
							}

							if (shouldExecute)
							{
								var executionFinishedEvent = new AutoResetEvent(false);

								IQueryCommand command = null;

								Action<IAsyncResult> handleCallback = result =>
								{
									command = (IQueryCommand)result.AsyncState;

									try
									{
										using (var reader = command.EndExecuteReader(result))
										{
											while (!reader.IsClosed)
											{
												DataTable table = new DataTable();

												table.Load(reader, LoadOption.OverwriteChanges, ExecuteSqlFillErrorHandler);
												tables.Add(table);
											}
										}
									}
									catch (Exception ex)
									{
										log.Error(ex);

										gotException = ex;

										if (fromGroupSelect)
										{
											log.ErrorFormat(
												"Instance:'{0}';Authentication:'{1}';SQL:'{2}';Exception:'{3}'",
												instance.Instance,
												instance.Authentication,
												sql,
												ex
											);
										}
									}
									finally
									{
										if (command != null)
										{
											command.Cancel();
										}

										executionFinishedEvent.Set();
									}
								};

								sqlCommand.AssignParameters(parameters, parameterValues, parametersQueueForODBC);
								var callback = new AsyncCallback(handleCallback);
								var asyncResult = sqlCommand.BeginExecuteReader(callback);

								if (WaitHandle.WaitAny(new[] { asyncResult.AsyncWaitHandle, this._cancellationToken.WaitHandle }) == 1)
								{
									if (command != null)
									{
										command.Cancel();
									}

									this._cancellationToken.ThrowIfCancellationRequested();
								}

								executionFinishedEvent.WaitOne();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);

				if (gotException == null)
				{
					gotException = ex;

					if (fromGroupSelect)
					{
						log.ErrorFormat(
							"Instance:'{0}';Authentication:'{1}';Exception:'{2}'",
							instance.Instance,
							instance.Authentication,
							ex
						);
					}
				}
			}
			finally
			{
				if (progress != null)
				{
					progress.SetProgressDone();
				}
			}

			if (gotException != null)
			{
				if (!fromGroupSelect)
				{
					gotException.Data.Add("IgnoreLog", true);
				}

				throw gotException;
			}

			return tables.ToArray();
		}

		private void ExecuteSqlFillErrorHandler(object sender, FillErrorEventArgs e)
		{
			log.Error("ExecuteSqlFillErrorHandler", e.Errors);

			// Setting e.Continue to True tells the Load method to continue trying. Setting it to False
			// indicates that an error has occurred, and the Load method raises the exception that got
			// you here.
			e.Continue = true;
		}

		/// <summary>
		/// Notify to skip real MS SQL Queries (for imported SQLite DB)
		/// </summary>
		public void SetSkipMSSQLQueries()
		{
			this._skipMsSqlQueries = true;
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					if (this._queryConnectionFactory != null)
					{
						this._queryConnectionFactory.Dispose();
					}
				}

				_disposed = true;
			}
		}

		public void Close()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose()
		{
			this.Close();
		}
	}

	/// <summary>
	/// Error info
	/// </summary>
	public class ErrorInfo
	{
		private readonly DateTime _dateTime;
		private readonly string   _errorNumber;
		private readonly string   _errorCode;
		private readonly string   _errorMessage;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="ex">Exception</param>
		public ErrorInfo(Exception ex)
		{
			this._dateTime = DateTime.Now;
			Exception = ex;

			if (ex != null)
			{
				this._errorMessage = ex.Message;

				if (ex is SqlException)
				{
					SqlException sqlException = ex as SqlException;

					if (sqlException != null)
					{
						this._errorCode   = sqlException.ErrorCode.ToString();
						this._errorNumber = sqlException.Number.ToString();
					}
					else
					{
						this._errorCode   = "-1";
						this._errorNumber = "-1";
					}
				}
				else
				{
					this._errorCode   = "-1";
					this._errorNumber = "-1";
				}
			}
			else
			{
				this._errorMessage = String.Empty;
				this._errorCode    = "-1";
				this._errorNumber  = "-1";
			}
		}

		public ErrorInfo(string number, string code, string message, DateTime dateTime)
		{
			this._errorNumber  = string.IsNullOrEmpty(number) ? "-1" : number;
			this._errorCode    = string.IsNullOrEmpty(code) ? "-1"   : code;
			this._errorMessage = message;
			this._dateTime     = dateTime;

			Exception = new Exception(message);
		}

		/// <summary>
		/// Exception datetime
		/// </summary>
		public DateTime DateTime
		{
			get { return this._dateTime; }
		}

		/// <summary>
		/// Message.
		/// </summary>
		public string Message
		{
			get { return this._errorMessage; }
		}

		/// <summary>
		/// Code.
		/// </summary>
		public string Code
		{
			get
			{
				return this._errorCode;
			}
		}

		/// <summary>
		/// Number.
		/// </summary>
		public string Number
		{
			get
			{
				return this._errorNumber;
			}
		}

		/// <summary>
		/// Catched exception
		/// </summary>
		public Exception Exception { get; private set; }
	}

	/// <summary>
	/// Query result for database
	/// </summary>
	public class QueryDatabaseResultInfo
	{
		private string               _database;
		private string               _databaseId;
		private readonly DataTable[] _dataTables;
		private readonly ErrorInfo   _errorInfo;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="dataTable">Result datatable</param>
		/// <param name="queryItem">Query item</param>
		/// <param name="database">Database name (null for instance scope)</param>
		/// <param name="databaseId">Database Id</param>
		public QueryDatabaseResultInfo(DataTable[] dataTable, QueryItemInfo queryItem, string database = "", string databaseId = "")
		{
			this.QueryItem   = queryItem;
			this._dataTables = dataTable;
			this._database   = database;
			this._databaseId = databaseId;
		}

		/// <summary>
		/// Constructor for thrown exception
		/// </summary>
		/// <param name="errorInfo">Error info</param>
		/// <param name="queryItem">Query item</param>
		/// <param name="databaseId">Database name (null for instance scope)</param>
		public QueryDatabaseResultInfo(ErrorInfo errorInfo, QueryItemInfo queryItem, string database = "", string databaseId = "")
		{
			this.QueryItem   = queryItem;
			this._errorInfo  = errorInfo;
			this._database   = database;
			this._databaseId = databaseId;
		}

		/// <summary>
		/// Database name (null for instance scope)
		/// </summary>
		public string Database
		{
			get
			{
				return this._database;
			}
			set
			{
				this._database = value;
			}
		}

		/// <summary>
		/// Database Id (null for instance scope)
		/// </summary>
		public string DatabaseId
		{
			get
			{
				return this._databaseId;
			}
			set
			{
				this._databaseId = value;
			}
		}

		/// <summary>
		/// Result datatable
		/// </summary>
		public DataTable[] DataTables
		{
			get
			{
				return this._dataTables;
			}
		}

		/// <summary>
		/// Error info if exception was thrown
		/// </summary>
		public ErrorInfo ErrorInfo
		{
			get
			{
				return this._errorInfo;
			}
		}

		/// <summary>
		/// Query item
		/// </summary>
		public QueryItemInfo QueryItem { get; private set; }
	}

	/// <summary>
	/// Query result for server instance
	/// </summary>
	public class QueryInstanceResultInfo
	{
		private readonly InstanceInfo                                          _instance;
		private readonly ConcurrentDictionary<string, QueryDatabaseResultInfo> _databasesResult;
		private readonly ErrorInfo                                             _errorInfo;

		public QueryInstanceResultInfo()
		{
			this._instance        = null;
			this._databasesResult = new ConcurrentDictionary<string, QueryDatabaseResultInfo>();
			this._errorInfo       = null;
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="instance">Server instance</param>
		public QueryInstanceResultInfo(InstanceInfo instance) : this()
		{
			this._instance = instance;
		}

		/// <summary>
		/// Constructor with exception thrown
		/// </summary>
		/// <param name="errorInfo">Error info</param>
		/// <param name="instance">Server instance</param>
		public QueryInstanceResultInfo(ErrorInfo errorInfo, InstanceInfo instance) : this()
		{
			this._instance  = instance;
			this._errorInfo = errorInfo;
		}

		/// <summary>
		/// Server instance
		/// </summary>
		public InstanceInfo Instance
		{
			get
			{
				return this._instance;
			}
		}

		/// <summary>
		/// Result for databases (one item for instance result)
		/// </summary>
		public ConcurrentDictionary<string, QueryDatabaseResultInfo> DatabasesResult
		{
			get
			{
				return this._databasesResult;
			}
		}

		/// <summary>
		/// Error info if exception was thrown
		/// </summary>
		public ErrorInfo ErrorInfo
		{
			get
			{
				return this._errorInfo;
			}
		}

		/// <summary>
		/// Add database result
		/// </summary>
		/// <param name="queryDatabaseResult"></param>
		public void AddDatabaseResult(QueryDatabaseResultInfo queryDatabaseResult)
		{
			Debug.Assert(queryDatabaseResult.Database == String.Empty);

			this._databasesResult.TryAdd(queryDatabaseResult.Database, queryDatabaseResult);
		}
	}

	/// <summary>
	/// Result for query
	/// </summary>
	public class QueryResultInfo
	{
		private readonly ConcurrentDictionary<InstanceInfo, QueryInstanceResultInfo> _instancesResult;
		private readonly ErrorInfo                                                   _errorInfo;

		/// <summary>
		/// Default constructor
		/// </summary>
		public QueryResultInfo()
		{
			this._instancesResult = new ConcurrentDictionary<InstanceInfo, QueryInstanceResultInfo>();
			this._errorInfo       = null;
		}

		/// <summary>
		/// Constructor with exception thrown
		/// </summary>
		/// <param name="errorInfo">Error info</param>
		public QueryResultInfo(ErrorInfo errorInfo) : this()
		{
			this._errorInfo = errorInfo;
		}

		/// <summary>
		/// Dictionary for instance results
		/// </summary>
		public ConcurrentDictionary<InstanceInfo, QueryInstanceResultInfo> InstancesResult
		{
			get
			{
				return this._instancesResult;
			}
		}

		/// <summary>
		/// Add instance result
		/// </summary>
		/// <param name="instanceResult">Instance result</param>
		public void AddInstanceResult(QueryInstanceResultInfo instanceResult)
		{
			this._instancesResult.TryAdd(instanceResult.Instance, instanceResult);
		}

		/// <summary>
		/// Error info if exception was thrown
		/// </summary>
		public ErrorInfo ErrorInfo
		{
			get
			{
				return this._errorInfo;
			}
		}
	}

	/// <summary>
	/// Database results of node query
	/// </summary>
	internal class QueryDatabaseResult
	{
		public QueryDatabaseResult(
			TemplateNodeQueryInfo   templateNodeQueryInfo,
			InstanceInfo            instanceInfo,
			QueryDatabaseResultInfo databaseResult)
		{
			this.TemplateNodeQuery = templateNodeQueryInfo;
			this.Instance          = instanceInfo;
			this.Result            = databaseResult;
		}

		public TemplateNodeQueryInfo TemplateNodeQuery { get; private set; }
		public InstanceInfo Instance                   { get; private set; }
		public QueryDatabaseResultInfo Result          { get; private set; }
	}

	/// <summary>
	/// Results for template node
	/// </summary>
	public struct TemplateNodeResultItem
	{
		private readonly TemplateNodeQueryInfo _templateNodeInfo;
		private readonly QueryResultInfo       _queryResult;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="templateNodeInfo">Template node info</param>
		/// <param name="queryResult">Query result</param>
		public TemplateNodeResultItem(TemplateNodeQueryInfo templateNodeInfo, QueryResultInfo queryResult)
		{
			this._templateNodeInfo = templateNodeInfo;
			this._queryResult      = queryResult;
		}

		/// <summary>
		/// Template node info
		/// </summary>
		public TemplateNodeQueryInfo TemplateNodeQuery
		{
			get
			{
				return this._templateNodeInfo;
			}
		}

		/// <summary>
		/// Query result
		/// </summary>
		public QueryResultInfo QueryResult
		{
			get
			{
				return this._queryResult;
			}
		}
	}

	/// <summary>
	/// Multy query result info
	/// </summary>
	public class MultyQueryResultInfo
	{
		private readonly object                       _listLock;
		private readonly List<TemplateNodeResultItem> _list;
		private          DateTime                     _timestamp;

		/// <summary>
		/// Default constructor
		/// </summary>
		public MultyQueryResultInfo()
		{
			this._listLock  = new object();
			this._list      = new List<TemplateNodeResultItem>();
			this._timestamp = DateTime.Now;

			this.NodeLastUpdated        = null;
			this.NodeLastUpdateDuration = new DateTime(0);
		}

		/// <summary>
		/// List of results
		/// </summary>
		public ReadOnlyCollection<TemplateNodeResultItem> List
		{
			get { return this._list.AsReadOnly(); }
		}

		/// <summary>
		/// Results date-time
		/// </summary>
		public DateTime Timestamp
		{
			get { return this._timestamp; }
		}

		internal void RefreshTimestamp(DateTime timestamp)
		{
			if (this._timestamp > timestamp)
			{
				this._timestamp = timestamp;
			}
		}

		/// <summary>
		/// Last update node datetime.
		/// </summary>
		public DateTime? NodeLastUpdated { get; set; }

		/// <summary>
		/// Last update node duration.
		/// </summary>
		public DateTime? NodeLastUpdateDuration { get; set; }

		/// <summary>
		/// Add template node item.
		/// </summary>
		/// <param name="item">template node item.</param>
		public void Add(TemplateNodeResultItem item)
		{
			lock (this._listLock)
			{
				this._list.Add(item);
			}
		}

		internal IEnumerable<GroupDefinition> ExtractDatabases()
		{
			List<GroupDefinition> databases = new List<GroupDefinition>();

			foreach (TemplateNodeResultItem resultItem in List)
			{
				foreach (var instancePair in resultItem.QueryResult.InstancesResult)
				{
					if ((instancePair.Value.ErrorInfo != null) && (!databases.Contains(GroupDefinition.NullGroup)))
					{
						databases.Add(GroupDefinition.NullGroup);
					}

					foreach (var databasePair in instancePair.Value.DatabasesResult)
					{
						GroupDefinition database = new GroupDefinition(
							instancePair.Value.Instance,
							databasePair.Value.Database,
							databasePair.Value.DatabaseId
						);

						if (!databases.Contains(database))
						{
							databases.Add(database);
						}
					}
				}
			}

			if (databases.Count == 0)
			{
				databases.Add(GroupDefinition.NullGroup);
			}

			return databases;
		}

		internal IEnumerable<QueryDatabaseResult> GetDatabaseResults()
		{
			foreach (TemplateNodeResultItem nodeResult in this.List)
			{
				TemplateNodeQueryInfo templateNodeQuery = nodeResult.TemplateNodeQuery;
				QueryResultInfo       queryResult       = nodeResult.QueryResult;

				foreach (KeyValuePair<InstanceInfo, QueryInstanceResultInfo> instancePair in queryResult.InstancesResult)
				{
					InstanceInfo            instance            = instancePair.Key;
					QueryInstanceResultInfo queryInstanceResult = instancePair.Value;

					if (queryInstanceResult.ErrorInfo != null)
					{
						continue;
					}

					foreach (KeyValuePair<string, QueryDatabaseResultInfo> namedResult in queryInstanceResult.DatabasesResult)
					{
						yield return new QueryDatabaseResult(
							templateNodeQuery,
							instance,
							namedResult.Value
						);
					}
				}
			}
		}
	}
}
