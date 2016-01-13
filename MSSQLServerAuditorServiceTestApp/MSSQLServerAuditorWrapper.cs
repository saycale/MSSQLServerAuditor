using System;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;
using log4net;
using MSSQLServerAuditor.BusinessLogic.LocalStorage;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.Gui;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables;
using MSSQLServerAuditor.BusinessLogic.UserSettings;
using MSSQLServerAuditor.SQLite.Tables.UserSettings;

namespace MSSQLServerAuditorServiceTestApp
{
    public class MSSQLServerAuditorWrapper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static MSSQLServerAuditorWrapper _instance;
        private static MsSqlAuditorModel _model;
        private static CurrentStorage _storage;
        private static ScheduleJobProcessor _scheduleJobProcessor;
        private static ConcurrentDictionary<long, NodeInstanceRow> _serviceNodeInstances;
        private static ConcurrentDictionary<long, NodeInstanceRow> _databaseNodeInstances;
        private static ConcurrentDictionary<long,ScheduleSettingsRow> _serviceSchedules;
        private static ConcurrentDictionary<long,ScheduleSettingsRow> _databaseSchedules;
        private static List<ConnectionGroupInfo> _connectionGroups;
        private static ConnectionsManager _connectionsManager;
        private static ScheduleSettingsManager _scheduleSettingsManager;
        private static ConcurrentDictionary<string, string> _runningTasks;
        private static readonly object _wrapperInstanceLock = new Object();
        private static readonly object _runningTasksLock = new Object();
        private static DateTime _serviceStarted;
        private static int _serviceDataUpdateTimeout = 0;
        private static int _serviceRunJobsTimeout = 0;
        private const int DEFAULT_DATA_UPDATE_TIMEOUT = 30;
        private const int DEFAULT_RUN_JOBS_TIMEOUT = 5;

        /// <summary>
        /// Private constructor
        /// </summary>
        private MSSQLServerAuditorWrapper()
        {
        }
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static MSSQLServerAuditorWrapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_wrapperInstanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MSSQLServerAuditorWrapper();

                            Initialize();
                            Thread.Sleep(TimeSpan.FromMilliseconds(100));
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Initialize wrapper
        /// </summary>
        public static void Initialize()
        {
            _model = new MsSqlAuditorModel();
            _model.Initialize();

            _storage = _model.DefaultVaultProcessor.CurrentStorage;

            _serviceDataUpdateTimeout = _model.Settings.SystemSettings.ServiceDataUpdateTimeout;
            _serviceRunJobsTimeout = _model.Settings.SystemSettings.ServiceRunJobsTimeout;
            _serviceStarted = DateTime.Now;

            _scheduleJobProcessor = new ScheduleJobProcessor(_storage);
            _connectionsManager = new ConnectionsManager(_model);
            _scheduleSettingsManager = new ScheduleSettingsManager(_storage);

            _runningTasks = new ConcurrentDictionary<string, string>();
            _databaseNodeInstances = new ConcurrentDictionary<long, NodeInstanceRow>();
            _serviceNodeInstances = new ConcurrentDictionary<long, NodeInstanceRow>();

            _serviceSchedules = new ConcurrentDictionary<long,ScheduleSettingsRow>();
            _databaseSchedules = new ConcurrentDictionary<long,ScheduleSettingsRow>();
            _connectionGroups = new List<ConnectionGroupInfo>();

            LoadFromDB(true);
            log.DebugFormat(
                    @"Active scheduled Instances:'{0}'  Active schedules: '{1}'",
                    _serviceNodeInstances.Count,
                    _serviceSchedules.Count
                );
        }

        /// <summary>
        /// Update instances timeout
        /// </summary>
        public int ServiceDataUpdateTimeout
        {
            get
            {
                return (_serviceDataUpdateTimeout != 0) ? _serviceDataUpdateTimeout : DEFAULT_DATA_UPDATE_TIMEOUT;
            }
        }

        /// <summary>
        /// Run scheduled jobs timeout
        /// </summary>
        public int ServiceRunJobsTimeout
        {
            get
            {
                return (_serviceRunJobsTimeout != 0) ? _serviceRunJobsTimeout : DEFAULT_RUN_JOBS_TIMEOUT;
            }
        }

        /// <summary>
        /// Is initialized storage and model
        /// </summary>
        /// <returns></returns>
        public bool IsInitialized()
        {
            if (_storage == null || _model == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Is scheduled instances is empty
        /// </summary>
        /// <returns></returns>
        private static bool IsSchedulesEmpty()
        {
            return _serviceNodeInstances.Count > 0 ? false : true;
        }

        /// <summary>
        /// Load schedules for instances
        /// </summary>
        private static void LoadSchedules(bool initialize)
        {
            if (!IsSchedulesEmpty())
            {
                _databaseSchedules.Clear();
                foreach (ScheduleSettingsRow row in _storage.ScheduleSettingsTable.GetAllRows())
                {
                    _databaseSchedules.TryAdd(row.Identity, row);
                    if (initialize)
                    {
                        _serviceSchedules.TryAdd(row.Identity, row);
                    }
                }
            }
        }

        private static void LoadInstances(bool initialize)
        {
            _databaseNodeInstances.Clear();
            foreach (NodeInstanceRow row in _storage.NodeInstances.GetScheduledInstances())
            {
                _databaseNodeInstances.TryAdd(row.Identity, row);
                if (initialize)
                {
                    _serviceNodeInstances.TryAdd(row.Identity, row);
                    
                }

            }
        }

        /// <summary>
        /// Get connection groups from database
        /// </summary>
        private static void LoadConnectionGroups()
        {
            if (!IsSchedulesEmpty())
            {
                _connectionGroups.Clear();
                _connectionGroups = _connectionsManager.GetAllGroups(String.Empty);
                foreach (var connInfo in _connectionGroups)
                {
                    NodeInstanceRow row = _serviceNodeInstances.Values.FirstOrDefault(inst => inst.ConnectionGroupId == connInfo.Identity);
                    if (row != null)
                    {
                        connInfo.TemplateFileName = row.TemplateFileName;
                        connInfo.TemplateDir = _model.Settings.SystemSettings.TemplateDirectory;

                        foreach (var connection in connInfo.Connections)
                        {
                            connection.ConnectionGroup = connInfo;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Load values from database
        /// </summary>
        /// <param name="initialize">If initializing load instances to service</param>
        private static void LoadFromDB(bool initialize)
        {
            LoadInstances(initialize);
            LoadSchedules(initialize);

            if (initialize)
            {
                LoadConnectionGroups();
            }

            bool changes = LoadUpdates(_serviceNodeInstances, _databaseNodeInstances);
            if (changes)
            {
                LoadConnectionGroups();
            }
            
            LoadUpdates(_serviceSchedules, _databaseSchedules);
        }

        /// <summary>
        /// Load instances updates from database to service
        /// </summary>
        /// <typeparam name="T">Concurrent dictionary value type</typeparam>
        /// <param name="service">service values</param>
        /// <param name="database">database values</param>
        private static bool LoadUpdates<T>(ConcurrentDictionary<long, T> service, ConcurrentDictionary<long, T> database)
        {
            bool dbChanges = false;
            if (service.Count > database.Count)
            {
                foreach (long key in service.Keys)
                {
                    T outRow = default(T);
                    if (!database.Keys.Contains(key))
                    {
                        service.TryRemove(key, out outRow);
                    }
                }
                dbChanges = true;
            }
            if (service.Count < database.Count)
            {
                foreach (long key in database.Keys)
                {
                    if (!service.Keys.Contains(key))
                    {
                        service.TryAdd(key, database[key]);
                    }
                }
                dbChanges = true;
            }
            foreach (long key in database.Keys)
            {
                service[key] = database[key];
            }
            return dbChanges;
        }

        /// <summary>
        /// Check instances updates from sqlite
        /// </summary>
        public void CheckUpdates()
        {
            string strReturnMessage = string.Empty;

            if (!IsInitialized())
            {
                log.ErrorFormat(
                    @"Storage is not initialized"
                );
            }
            else
            {
                LoadFromDB(false);
            }
            log.DebugFormat(
                    @"Active scheduled Instances:'{0}'  Active schedules: '{1}'",
                    _serviceNodeInstances.Count,
                    _serviceSchedules.Count
                );
        }

        /// <summary>
        /// Check and run scheduled jobs
        /// </summary>
        public void CheckScheduledJobs()
        {
            if (!IsInitialized())
            {
                log.ErrorFormat(
                    @"Storage is not initialized"
                );

                return;
            }

            if (!IsSchedulesEmpty())
            {
                Action runNodeJobs = () =>
                {
                    RunNodeJobs();
                };
                Task.Factory.StartNew(runNodeJobs);
            }
        }

        /// <summary>
        /// Run scheduled job
        /// </summary>
        private void RunNodeJobs()
        {
            foreach (var nodeInstance in _serviceNodeInstances)
            {
                
                ConnectionGroupInfo connInfo = _connectionGroups.FirstOrDefault(cgi => cgi.Identity == nodeInstance.Value.ConnectionGroupId);
                connInfo.TemplateFileName = nodeInstance.Value.TemplateFileName;
                connInfo.TemplateDir = _model.Settings.TemplateDirectory;
                log.DebugFormat(
                        @"Connection:'{0}'; id: {1}",
                        connInfo.Name, connInfo.Identity
                    );
                if (connInfo != null)
                {
                    ConnectionData connectionData = new ConnectionData(_model, connInfo);

                    var allNodesInTemplate = ToPlainList(connectionData.RootOfTemplate);
                    TemplateNodeInfo node = allNodesInTemplate.FirstOrDefault(n => n.Name == nodeInstance.Value.TemplateNodeName);
                    node.AssignTemplateId(nodeInstance.Value.TemplateNodeId);

                    
                    // List<ScheduleSettingsRow> nodeSchedules = _serviceSchedules.Where(r => r.TemplateNodeId == nodeInstance.Value.TemplateNodeId).ToList();
                    List<ScheduleSettingsRow> nodeSchedules = _serviceSchedules.Values.Where(r => r.TemplateNodeId == nodeInstance.Value.Identity).ToList();

                    log.DebugFormat(
                        @"Node schedules count:'{0}'",
                        nodeSchedules.Count
                    );
                    if (nodeSchedules != null)
                    {
                        List<TemplateNodeUpdateJob> nodeJobs = _scheduleSettingsManager.GetJobs(nodeSchedules, node);

                        log.DebugFormat(
                                 @"Node jobs count:'{0}'",
                                nodeJobs.Count
                         );
                        foreach (var job in nodeJobs)
                        {
                            string runningKey = nodeInstance.Value.TemplateNodeId.ToString() + connectionData.ConnectionGroup + job.SettingsId.ToString() + job.NodeInfo.Name;
                            if (_runningTasks.ContainsKey(runningKey))
                            {
                                log.DebugFormat(
                                    @"Runned: Task:'{0}'",
                                    runningKey
                                );
                                return;
                            }
                            
                            log.DebugFormat(
                                @"Run if time job node :'{1}'; id: {0}; last run: {2}",
                                    job.SettingsId, job.NodeInfo.Name, job.LastRan
                            );
                            Action action = () =>
                            {
                                RunQueries(node, connectionData, nodeInstance.Value.TemplateNodeId, job.SettingsId, runningKey);
                            };
                            _scheduleJobProcessor.RunIfTime(job, action, _serviceStarted);
                        }
                    }
                }
                else
                {
                    log.DebugFormat(
                        @"ConnectionGroupInfo is null"
                    );
                }
            }
        }

        /// <summary>
        /// Run queries for the node
        /// </summary>
        /// <param name="nodeInfo">TemplateNodeInfo</param>
        /// <param name="connData">Connection data</param>
        /// <param name="templateRowId">Template Id</param>
        private void RunQueries(TemplateNodeInfo nodeInfo, ConnectionData connData, long templateRowId, long scheduleId, string runningKey)
        {
            MultyQueryResultInfo result;
            ProgressItem item = new ProgressItem();
            CancellationToken token = new CancellationToken();

            log.DebugFormat(
                @"Started: Connection:'{0}';templateRowId:'{1}';Query:'{2}'",
                connData.Title,
                templateRowId,
                nodeInfo.Name
            );

            lock (_runningTasksLock)
            {
                _runningTasks.TryAdd(runningKey, nodeInfo.Name);
            }

            using (SqlProcessor sqlProcessor = _model.GetNewSqlProcessor(token))
            {
                nodeInfo.SetConnectionData(null);
                TemplateNodeInfo node = nodeInfo.Instantiate(connData, null, null);
                node.AssignTemplateId(templateRowId);
                node.LoadUserParameters(_storage);

                Stopwatch durationWatch = new Stopwatch();
                DateTime startTime = DateTime.Now;

                durationWatch.Start();

                result = sqlProcessor.ExecuteMultyQuery(
                    connData.ConnectionGroup,
                    node.Queries,
                    item,
                    _model.Settings.SystemSettings.MaximumDBRequestsThreadCount
                );

                IStorageManager istorage = _model.GetVaultProcessor(connData.ConnectionGroup);

                istorage.SaveRequestedData(node, result);

                durationWatch.Stop();

                DateTime duration = new DateTime(durationWatch.Elapsed.Ticks);

                istorage.CurrentStorage.UpdateTreeNodeTimings(
                    node,
                    startTime,
                    duration
                );

                //_serviceSchedules.FirstOrDefault(sch => sch.TemplateNodeId == nodeInstanceRowId).LastRan = startTime;
                _serviceSchedules[scheduleId].LastRan = startTime;
            }

            nodeInfo.SetConnectionData(connData);

            lock (_runningTasksLock)
            {
                string outTask;
                _runningTasks.TryRemove(runningKey, out outTask);
            }
        }


        /// <summary>
        /// Convert TemplateNodeInfo to the plain list
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static IEnumerable<TemplateNodeInfo> ToPlainList(TemplateNodeInfo node)
        {
            var result = new List<TemplateNodeInfo> { node };

            foreach (var child in node.Childs)
            {
                result.AddRange(ToPlainList(child));
            }

            return result;
        }
    }
}