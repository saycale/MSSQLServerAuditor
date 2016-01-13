using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using log4net;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.BusinessLogic.LocalStorage;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Gui;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.BusinessLogic.UserSettings;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Template node definition
	/// </summary>
	[Serializable]
	public partial class TemplateNodeInfo
	{
		private static readonly log4net.ILog        log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private string                              _templateId;
		private List<TemplateNodeLocaleInfo>        _locales;
		private List<TemplateNodeInfo>              _childs;
		private List<TemplateNodeQueryInfo>         _queries;
		private List<TemplateNodeQueryInfo>         _groupQueries;
		private List<TemplateNodeQueryInfo>         _connectionQueries;
		private List<TemplateNodeSqlGuardQueryInfo> _sqlCodeGuardQueries;
		private string                              _idsHierarchyCache;
		private Int64?                              _templateNodeId;
		private string                              _defaultDatabaseAttributeName;

		/// <summary>
		/// Main window title (constant string or template)
		/// </summary>
		private FormWindowTitle                     _mainFormWindowTitle;
		private TemplateTreeTitle                   _treeTitle;

		[NonSerialized]
		private MsSqlAuditorModel                   _model;

		/// <summary>
		/// Template node information.
		/// </summary>
		public TemplateNodeInfo()
		{
			this._templateId                   = null;
			this._locales                      = null;
			this._childs                       = null;
			this._queries                      = null;
			this._groupQueries                 = null;
			this._connectionQueries            = null;
			this._sqlCodeGuardQueries          = null;
			this._idsHierarchyCache            = null;
			this._templateNodeId               = null;
			this._defaultDatabaseAttributeName = null;
			this._mainFormWindowTitle          = null;
			this._treeTitle                    = null;
			this._model                        = null;
			this.ShowNumberOfRecords           = true;
			this.HideEmptyResultDatabases      = false;
		}

		/// <summary>
		/// Node name (uses in result file hierarchy)
		/// </summary>
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Node id (uses in tables hierarchy)
		/// </summary>
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Rule "Show single Tab"
		/// </summary>
		[XmlAttribute(AttributeName = "IsHideTabs")]
		public bool IsHideTabs { get; set; }

		[XmlAttribute(AttributeName = "group-select-id")]
		public string GroupSelectId { get; set; }

		[XmlAttribute(AttributeName = "file")]
		public string XslTemplateFileName { get; set; }

		/// <summary>
		/// Reference to an icon. The icon will be displayed in tree view next to the node
		/// </summary>
		[XmlAttribute(AttributeName = "icon")]
		public string IconImageReferenceName { get; set; }

		/// <summary>
		/// Hide empty result databases.
		/// </summary>
		[XmlAttribute(AttributeName = "hideEmptyResultDatabases")]
		public bool HideEmptyResultDatabases { get; set; }

		/// <summary>
		/// Show number of records.
		/// </summary>
		[XmlAttribute(AttributeName = "showNumberOfRecords")]
		public bool ShowNumberOfRecords { get; set; }

		/// <summary>
		/// Localization of the node
		/// </summary>
		[XmlElement(ElementName = "i18n")]
		public List<TemplateNodeLocaleInfo> Locales
		{
			get
			{
				return this._locales;
			}

			set
			{
				this._locales = value;
			}
		}

		[XmlElement(ElementName = "MainWindowTitle")]
		public FormWindowTitle MainFormWindowTitle
		{
			get
			{
				return this._mainFormWindowTitle;
			}

			set
			{
				this._mainFormWindowTitle = value;
			}
		}

		public TemplateTreeTitle TreeTitle
		{
			get
			{
				return this._treeTitle;
			}

			set
			{
				this._treeTitle = value;
			}
		}

		/// <summary>
		/// Child nodes
		/// </summary>
		[XmlElement(ElementName = "template")]
		public List<TemplateNodeInfo> Childs
		{
			get
			{
				return this._childs;
			}

			set
			{
				this._childs = value;
			}
		}

		[XmlIgnore]
		public bool ChildrenAreLoadingNow { get; private set; }

		[XmlIgnore]
		public DateTime? LastUpdateNode { get; set; }

		[XmlIgnore]
		public int? CounterValue { get; set; }

		[XmlIgnore]
		public ConnectionData Connection { get; private set; }

		[XmlIgnore]
		public TemplateNodeInfo Template { get; private set; }

		/// <summary>
		/// Parent node
		/// </summary>
		[XmlIgnore]
		public TemplateNodeInfo Parent { get; private set; }

		/// <summary>
		/// One node becomes an owner of another if other one is its child
		/// </summary>
		[XmlIgnore]
		public TemplateNodeInfo Owner { get; private set; }

		public List<RefreshSchedule> RefreshSchedules { get; set; }

		public IEnumerable<TemplateNodeInfo> GetSubnodesFor(TemplateNodeQueryInfo groupQuery)
		{
			Debug.Assert(IsInstance);
			Debug.Assert(GroupQueries.Contains(groupQuery));

			return Template.Childs.Where(t => t.GroupSelectId == groupQuery.Id);
		}

		public TemplateNodeQueryInfo GetParentQuery()
		{
			if (IsInstance)
			{
				return Template.GetParentQuery();
			}

			if (string.IsNullOrWhiteSpace(GroupSelectId))
			{
				return null;
			}

			if (Parent == null)
			{
				throw new InvalidTemplateException(this + " has <group-select-id> but has no parent node");
			}

			var result = Parent.GroupQueries.FirstOrDefault(q => q.Id == GroupSelectId);

			if (result == null)
			{
				throw new InvalidTemplateException(this + " has <group-select-id> = " + GroupSelectId +  "  but there is no <group-select> with such Id in parent node");
			}

			return result;
		}

		/// <summary>
		/// Queries for node
		/// </summary>
		[XmlElement(ElementName = "sql-select")]
		public List<TemplateNodeQueryInfo> Queries
		{
			get
			{
				if (this._queries != null)
				{
					this._queries.ForEach(x => x.TemplateNode = this);
				}

				return this._queries;
			}

			set
			{
				this._queries = value;
			}
		}

		[XmlElement(ElementName = "group-select")]
		public List<TemplateNodeQueryInfo> GroupQueries
		{
			get
			{
				if (this._groupQueries != null)
				{
					this._groupQueries.ForEach(x => x.TemplateNode = this);
				}

				return this._groupQueries;
			}

			set
			{
				this._groupQueries = value;
			}
		}

		[XmlElement(ElementName = "connections-select")]
		public List<TemplateNodeQueryInfo> ConnectionQueries
		{
			get
			{
				if (this._connectionQueries != null)
				{
					this._connectionQueries.ForEach(x => x.TemplateNode = this);
				}

				return this._connectionQueries;
			}

			set
			{
				this._connectionQueries = value;
			}
		}

		[XmlElement(ElementName = "sqlcodeguard-select")]
		public List<TemplateNodeSqlGuardQueryInfo> SqlCodeGuardQueries
		{
			get
			{
				return this._sqlCodeGuardQueries;
			}
			set
			{
				this._sqlCodeGuardQueries = value;
			}
		}

		private InstanceTemplate GetUserSettings(bool createIfNotExists = false)
		{
			var existing = Program.Model.TemplateSettings.UserSettings.FirstOrDefault(
				i => i.TemplateName == ConnectionGroup.TemplateFileName
					&& i.Connection.ParentKey == IdsHierarchy
			);

			if (existing == null && createIfNotExists)
			{
				existing = new InstanceTemplate
				{
					TemplateName = ConnectionGroup.TemplateFileName,
					Connection = {ParentKey = IdsHierarchy}
				};

				Program.Model.TemplateSettings.UserSettings.Add(existing);
			}

			return existing;
		}

		public void GetScheduledNodes(List<Tuple<TemplateNodeInfo, Job>> listToPlaceJobbedNodesTo, bool fromCache)
		{
			Debug.Assert(this.IsInstance);

			List<TemplateNodeUpdateJob> thisNodeJobs = GetRefreshJob(fromCache);

			foreach (TemplateNodeUpdateJob thisNodeJob in thisNodeJobs)
			{
				if (thisNodeJob != null && !thisNodeJob.IsEmpty())
				{
					listToPlaceJobbedNodesTo.Add(new Tuple<TemplateNodeInfo, Job>(this, thisNodeJob));
				}
			}

			foreach (var child in this.Childs)
			{
				child.GetScheduledNodes(listToPlaceJobbedNodesTo, fromCache);
			}
		}

		public DataTable[] GetGroupSelectResultsFromServer(
			SqlProcessor          sqlProcessor,
			InstanceInfo          serverInstance,
			QueryInfo             queryInfo,
			TemplateNodeQueryInfo query
		)
		{
			DataTable[] tables = null;
			ErrorInfo   error  = null;

			Debug.Assert(IsInstance);
			Debug.Assert(GroupQueries.Contains(query));

			var db = this._model.GetVaultProcessor(ConnectionGroup).CurrentStorage;

			InstanceTemplate settings = GetUserSettings();

			//looking for user settings for parameter values
			query.ReadParametersFrom(settings);

			try
			{
				tables = sqlProcessor.ExecuteSql(
					serverInstance,
					queryInfo,
					GetDefaultDatabase(),
					queryInfo.Parameters,
					query.ParameterValues,
					fromGroupSelect: true
				);
			}
			catch (Exception ex)
			{
				error  = new ErrorInfo(ex);
				tables = null;
			}

			Int64? queryId = db.QueryGroupDirectory.GetQueryId(
				query.TemplateNode,
				query,
				serverInstance,
				DateTime.Now,
				false
			);

			db.SaveMeta(queryId, error, tables);

			if (tables != null && tables.Length < 0)
			{
				throw new InvalidTemplateException(query + " returned no recordsets.");
			}

			return tables;
		}

		/// <summary>
		/// Template id obtained from template file
		/// </summary>
		public string TemplateId
		{
			get { return this._templateId; }
			set { this._templateId = value; }
		}

		public List<TemplateNodeUpdateJob> GetRefreshJob(bool fromCache)
		{
			ScheduleSettingsManager scheduleManager = new ScheduleSettingsManager(
				this._model.DefaultVaultProcessor.CurrentStorage
			);

			List<TemplateNodeUpdateJob> resultList = scheduleManager.LoadScheduleSettings(this, fromCache);

			if (resultList.IsNullOrEmpty() && !RefreshSchedules.IsNullOrEmpty())
			{
				foreach (RefreshSchedule sset in RefreshSchedules)
				{
					TemplateNodeUpdateJob result = new TemplateNodeUpdateJob();

					if (sset != null)
					{
						result.Settings = sset.Clone();

						AssignRefreshJob(result);

						resultList.Add(result);
					}
				}
			}

			return resultList;
		}

		public void AssignRefreshJob(TemplateNodeUpdateJob job)
		{
			InstanceTemplate nodeSetting = GetUserSettings(true);

			nodeSetting.Connection.ReportRefreshJob = job;
		}

		public void SaveJob(TemplateNodeUpdateJob job)
		{
			ScheduleSettingsManager scheduleManager = new ScheduleSettingsManager(
				this._model.DefaultVaultProcessor.CurrentStorage
			);

			scheduleManager.SaveScheduleSettings(this.TemplateNodeId.GetValueOrDefault(), job);

			if (this.Connection.RootOfStaticTree.Search(this) == null)
			{
				this.Connection.ResetStaticTreeRoot();
			}
		}

		/// <summary>
		/// Get localized item
		/// </summary>
		/// <param name="locale">Language (null for default)</param>
		/// <returns></returns>
		public TemplateNodeLocaleInfo GetLocale(bool throwExceptionIfNotFound, string locale = null)
		{
			var targetLanguage = locale ?? this._model.Settings.InterfaceLanguage;
			var localized      = Locales.FirstOrDefault(l => l.Language == targetLanguage);

			if (localized == null && throwExceptionIfNotFound)
			{
				log.ErrorFormat(
					"Localized item not found. TemplateNode.Name:'{0}';Language:'{1}'",
					PathAsString,
					targetLanguage
				);

				throw new InvalidOperationException(
					"Localized item not found. TemplateNode.Name=" + PathAsString + " Language="+targetLanguage
				);
			}

			return localized;
		}

		public TemplateNodeLocaleInfo GetMainFormWindowTitleLocalized()
		{
			if (this.MainFormWindowTitle != null && this.MainFormWindowTitle.Locales != null)
			{
				return this.getLocalizedItem(this.MainFormWindowTitle.Locales);
			}
			else
			{
				return null;
			}
		}

		public TemplateNodeLocaleInfo GetTemplateTreeTitleLocalized()
		{
			if (this.TreeTitle != null && this.TreeTitle.Locales != null)
			{
				return this.getLocalizedItem(this.TreeTitle.Locales);
			}
			else
			{
				return null;
			}
		}

		private TemplateNodeLocaleInfo getLocalizedItem(List<TemplateNodeLocaleInfo> locales)
		{
			string                 targetLanguage = this._model.Settings.InterfaceLanguage;
			TemplateNodeLocaleInfo localized      = this.TreeTitle.Locales.FirstOrDefault(l => l.Language == targetLanguage);

			if (localized == null)
			{
				log.ErrorFormat(
					"Localized item not found. TemplateNode.Name:'{0}';Language:'{1}'",
					PathAsString,
					targetLanguage
				);
			}

			return localized;
		}

		/// <summary>
		/// Path as string.
		/// </summary>
		public string PathAsString
		{
			get
			{
				return Parent == null ? Name : Parent.PathAsString + "->" + Name;
			}
		}

		/// <summary>
		/// Hierarchy representation of Ids
		/// </summary>
		public string IdsHierarchy
		{
			get
			{
				if (this._idsHierarchyCache == null)
				{
					// Debug.Assert((Id == null) == (Parent == null));

					var result = Id;

					if (Parent != null && Parent.IdsHierarchy != null)
					{
						result = Parent.IdsHierarchy + "_" + result;
					}

					this._idsHierarchyCache = result;
				}

				return this._idsHierarchyCache;
			}
		}

		/// <summary>
		/// Init node after load
		/// </summary>
		/// <param name="templateId"></param>
		/// <param name="model">Model</param>
		public void Init(string templateId, MsSqlAuditorModel model)
		{
			this._model      = model;
			this._templateId = templateId;

			foreach (var child in Childs)
			{
				child.Parent = this;
				child.Owner = this;
				child.Init(templateId, model);
			}
		}

		/// <summary>
		/// Gets a value indicating whether has groups.
		/// </summary>
		public bool MayHaveDynamicChildren
		{
			get
			{
				Debug.Assert(IsInstance);

				return Template.Childs.Any(ch => !string.IsNullOrWhiteSpace(ch.GroupSelectId));

				// if (this._model == null)
				// throw new InvalidOperationException();
				// return Queries.Any(q => this._model.GetQueryByTemplateNodeQueryInfo(q).GroupSelect.Count > 0)
				//    || GroupQueries.Any();
			}
		}

		/// <summary>
		/// Gets a value indicating whether has groups.
		/// </summary>
		public bool HasGroups(List<QuerySource> dataBaseTypeList)
		{
			if (this._model == null)
			{
				throw new InvalidOperationException();
			}

			return Queries.Any(q =>
			{
				Debug.Assert(IsInstance);
				var queries = this._model.GetQueryByTemplateNodeQueryInfo(q);
				return queries.Any(x => (dataBaseTypeList.Contains(x.Source) || x.Source == QuerySource.SQLite) && x.GroupSelect.Count > 0);
			});
		}

		public bool NeedDataRowToBeInstantiated
		{
			get { return !string.IsNullOrWhiteSpace(GroupSelectId); }
		}

		[XmlIgnore]
		public NodeAttirbutes Attributes { get; private set; }

		[XmlIgnore]
		public TemplateNodeQueryInfo ReplicationSourceQuery { get; private set; }

		public TemplateNodeInfo Instantiate(ConnectionData connection, DataRow row, TemplateNodeQueryInfo replicationSourceQuery, TemplateNodeInfo parent)
		{
			Debug.Assert(!IsInstance);
			Debug.Assert(NeedDataRowToBeInstantiated);

			var result = (TemplateNodeInfo)MemberwiseClone();
			result.Childs = new List<TemplateNodeInfo>();

			result.Template = this;

			result.Attributes = new NodeAttirbutes(result, row);
			result.Connection = connection;

			result.ReplicationSourceQuery = replicationSourceQuery;

			result.Queries = Queries.Select(q => q.Clone()).ToList();
			result.ConnectionQueries = ConnectionQueries.Select(q => q.Clone()).ToList();
			result.GroupQueries = GroupQueries.Select(q => q.Clone()).ToList();

			result.Parent = parent;
			result.Id = null;
			result.Name = Name + result.Id;

			result.OnAttributesChanged();

			return result;
		}

		public TemplateNodeInfo Instantiate(ConnectionData connection, string defaultDatabaseAttribute, TemplateNodeInfo parent)
		{
			Debug.Assert(!IsInstance);

			TemplateNodeInfo result = (TemplateNodeInfo)MemberwiseClone();

			result.Template                      = this;
			result.Attributes                    = new NodeAttirbutes(result);
			result.Childs                        = new List<TemplateNodeInfo>();
			result.Connection                    = connection;
			result.ReplicationSourceQuery        = null;
			result._defaultDatabaseAttributeName = defaultDatabaseAttribute;

			result.Queries = Queries.Select(q => q.Clone()).ToList();
			result.ConnectionQueries = ConnectionQueries.Select(q => q.Clone()).ToList();
			result.GroupQueries = GroupQueries.Select(q => q.Clone()).ToList();

			result.Parent = parent;

			return result;
		}

		public TemplateNodeInfo InstatiateStaticTree(ConnectionData connection, TemplateNodeInfo parent, Int64? id)
		{
			//Debug.Assert(parent == null || parent.IsInstance);
			//Debug.Assert(!this.IsInstance);

			var nodeInstance = this.IsInstance ? this : this.Instantiate(connection, null, parent);

			if (id != null)
			{
				nodeInstance.AssignTemplateId(id.Value);
			}

			nodeInstance.UpdateChildren(NodeUpdatingSource.LocallyOnly, null);

			foreach (var child in nodeInstance.Childs)
			{
				child.InstatiateStaticTree(connection, nodeInstance, null);
			}

			return nodeInstance;
		}

		public void OnAttributesChanged()
		{
			Queries.ForEach(q => q.ParameterValues.TakeValuesFrom(Attributes.Values));
			GroupQueries.ForEach(q => q.ParameterValues.TakeValuesFrom(Attributes.Values));
		}

		public string GetDefaultDatabase()
		{
			Debug.Assert(IsInstance);

			string attrName = this._defaultDatabaseAttributeName;

			if (attrName == null &&
				ReplicationSourceQuery != null
				&& !string.IsNullOrEmpty(ReplicationSourceQuery.DatabaseForChildrenFieldName)
			)
			{
				attrName = ReplicationSourceQuery.DatabaseForChildrenFieldName;
			}

			if (!string.IsNullOrWhiteSpace(attrName))
			{
				string result = String.Empty;

				if (!Attributes.Values.TryGetValue(attrName, out result))
				{
					throw new InvalidTemplateException("No column " + attrName + " in " + ReplicationSourceQuery);
				}

				return result;
			}

			return Parent != null ? Parent.GetDefaultDatabase() : null;
		}

		[XmlIgnore]
		public bool IsInstance
		{
			get { return this.Connection != null; }
		}

		[XmlIgnore]
		public string UId
		{
			get { return this.Attributes.GetUId(); }
		}

		[XmlIgnore]
		public string UName { get { return Attributes.GetUName(); } }

		[XmlIgnore]
		public string FontColor
		{
			get { return this.Attributes.GetValue("NodeFontColor"); }
			set { this.SetAttributeValue("NodeFontColor", value); }
		}

		[XmlIgnore]
		public string FontStyle
		{
			get { return this.Attributes.GetValue("NodeFontStyle"); }
			set { this.SetAttributeValue("NodeFontStyle", value); }
		}

		[XmlIgnore]
		public string UIcon
		{
			get { return this.Attributes.GetUIcon(); }
			set { this.SetAttributeValue("NodeUIcon", value); }
		}

		[XmlIgnore]
		public bool IsDisabled
		{
			get
			{
				bool result;

				if (bool.TryParse(this.Attributes.GetValue("NodeIsDisabled"), out result))
				{
					return result;
				}

				return false;
			}

			set { this.SetAttributeValue("NodeIsDisabled", value.ToString()); }
		}

		public Int64? TemplateNodeId
		{
			get
			{
				return this._templateNodeId;
			}
		}

		public ConnectionGroupInfo ConnectionGroup
		{
			get { return this.Connection.ConnectionGroup; }
		}

		public string Title
		{
			get
			{
				if (!string.IsNullOrEmpty(this.Attributes.GetValue("NodeUName")))
				{
					return this.Attributes.GetValue("NodeUName");
				}

				var locale = GetLocale(false);

				return locale != null
					? locale.Text
					: (Name ?? "<no name> (id = " + this.Id + ")");
			}

			set
			{
				this.SetAttributeValue("NodeUName", value);
			}
		}

		public bool HasActiveJobs { get; set; }

		public void AssignTemplateId(Int64? value)
		{
			string strException = string.Empty;

			if (this._templateNodeId != null && value != this._templateNodeId)
			{
				strException = string.Format("Template node id is already set to '{0}' and new value different:'{1}'",
					this._templateNodeId ?? -1L,
					value          ?? -1L
				);

				throw new InvalidOperationException(strException);
			}

			this._templateNodeId = value;
		}

		private static IEnumerable<TemplateNodeInfo> InstantiateStaticNodes(
			IEnumerable<TemplateNodeInfo> templateNodes,
			ConnectionData                connection,
			TemplateNodeInfo              parent
		)
		{
			Debug.Assert(templateNodes.All(t => !t.IsInstance));
			Debug.Assert(parent.IsInstance);

			foreach (TemplateNodeInfo templateNode in templateNodes.Where(n => !n.NeedDataRowToBeInstantiated))
			{
				yield return templateNode.Instantiate(connection, null, parent);
			}
		}

		public bool IsLeaf()
		{
			return Template.Childs.Count == 0 && Childs.Count == 0;
		}

		public void UpdateChildren(NodeUpdatingSource mode, CancellationTokenSource cancellationSource)
		{
			if (IsDisabled)
			{
				mode = NodeUpdatingSource.LocallyOnly;
			}

			Debug.Assert(this.IsInstance);

			IStorageManager vault   = this._model.GetVaultProcessor(Connection.ConnectionGroup);
			bool            updated = false;

			if (mode == NodeUpdatingSource.FromServerIfNotSavedLocally || mode == NodeUpdatingSource.LocallyOnly)
			{
				updated = vault.CurrentStorage.NodeInstances.TryLoadChildren(this);
			}

			if (updated || mode == NodeUpdatingSource.LocallyOnly)
			{
				return;
			}

			Childs.Clear();
			Childs.AddRange(InstantiateStaticNodes(this.Template.Childs, Connection, this));

			vault.CurrentStorage.NodeInstances.SaveChildren(this);

			if (MayHaveDynamicChildren && Connection.IsLiveConnection)
			{
				UpdateDynamicChildren(cancellationSource.Token);
			}
		}

		private void UpdateDynamicChildren(CancellationToken cancellationToken)
		{
			ChildrenAreLoadingNow = true;

			try
			{
				List<TemplateNodeInfo> newNodes = new List<TemplateNodeInfo>();

				using (SqlProcessor sqlProcessor = this._model.GetNewSqlProcessor(cancellationToken))
				{
					foreach (TemplateNodeQueryInfo query in GroupQueries)
					{
						List<TemplateNodeInfo> templateNodesToBeReplicated = GetSubnodesFor(query).ToList();
						List<QueryInfo>        queryInfos                  = this._model.GetQueryByTemplateNodeQueryInfo(query);

						foreach (InstanceInfo serverInstance in Connection.ConnectionGroup.Connections.Where(c => c.IsEnabled))
						{
							cancellationToken.ThrowIfCancellationRequested();

							QueryInfo queryInfo = queryInfos.FirstOrDefault(x =>
								x.Source == serverInstance.Type || x.Source == QuerySource.SQLite
							);

							DataTable[] tables = GetGroupSelectResultsFromServer(sqlProcessor, serverInstance, queryInfo, query);

							cancellationToken.ThrowIfCancellationRequested();

							foreach (DataTable t in tables)
							{
								for (int rowIndex = 0; rowIndex < t.Rows.Count; rowIndex++)
								{
									List<TemplateNodeInfo> replicants =
										templateNodesToBeReplicated.Select(
											tn => tn.Instantiate(
												Connection,
												t.Rows[rowIndex],
												query,
												this
											)
										).ToList();
									newNodes.AddRange(replicants);
								}
							}
						}
					}
				}

				cancellationToken.ThrowIfCancellationRequested();

				if (newNodes.Count == 0)
				{
					return;
				}

				lock (Childs)
				{
					Childs.AddRange(newNodes);

					this._model.GetVaultProcessor(Connection.ConnectionGroup).CurrentStorage.NodeInstances.SaveChildren(this);
				}
			}
			catch (Exception ex)
			{
				log.Error("private Task CreateAddDynamicChildrenTask(Action done)", ex);
			}
			finally
			{
				ChildrenAreLoadingNow = false;
			}
		}

		public TemplateNodeInfo CloneNotSavedTree(
			Dictionary<TemplateNodeInfo, TemplateNodeInfo> originalsAndClones,
			ConnectionData                                 connectionData,
			TemplateNodeInfo                               parent = null
		)
		{
			TemplateNodeInfo clone = (TemplateNodeInfo) MemberwiseClone();

			clone._templateNodeId     = null;
			clone.Parent     = parent;
			clone.Connection = connectionData;
			clone.Childs     = new List<TemplateNodeInfo>(
				Childs.Select(
					ch => ch.CloneNotSavedTree(
						originalsAndClones,
						connectionData,
						clone
					)
				)
			);

			originalsAndClones.Add(this, clone);

			return clone;
		}

		public void SaveUserParameters(CurrentStorage storage)
		{
			QueryDirectories queryDirectories = QueryDirectories.GetInstance(storage, false);
			SaveUserParameters(Queries, queryDirectories);

			QueryDirectories groupQueryDirectories = QueryDirectories.GetInstance(storage, true);
			SaveUserParameters(GroupQueries, groupQueryDirectories);
		}

		public void LoadUserParameters(CurrentStorage storage)
		{
			QueryDirectories queryDirectories = QueryDirectories.GetInstance(storage, false);
			LoadUserParameters(Queries, queryDirectories);

			QueryDirectories groupQueryDirectories = QueryDirectories.GetInstance(storage, true);
			LoadUserParameters(GroupQueries, groupQueryDirectories);
		}

		private void SaveUserParameters(IEnumerable<TemplateNodeQueryInfo> queries, QueryDirectories queryDirectories)
		{
			QueryDirectoryBase          queryDirectory          = queryDirectories.QueryDirectory;
			QueryParameterDirectoryBase queryParameterDirectory = queryDirectories.QueryParameterDirectory;

			foreach (TemplateNodeQueryInfo queryInfo in queries)
			{
				foreach (InstanceInfo instanceInfo in ConnectionGroup.Connections)
				{
					Int64? queryId = queryDirectory.GetQueryId(
						this,
						queryInfo,
						instanceInfo,
						DateTime.Now,
						false
					);

					if (queryId.HasValue)
					{
						queryParameterDirectory.Update(
							ConnectionGroup,
							queryId.Value,
							queryInfo,
							queryInfo.ParameterValues
						);
					}
				}
			}
		}

		private void LoadUserParameters(IEnumerable<TemplateNodeQueryInfo> queries, QueryDirectories queryDirectories)
		{
			QueryDirectoryBase          queryDirectory          = queryDirectories.QueryDirectory;
			QueryParameterDirectoryBase queryParameterDirectory = queryDirectories.QueryParameterDirectory;

			foreach (TemplateNodeQueryInfo queryInfo in queries)
			{
				foreach (ParameterValue paramInfo in queryInfo.ParameterValues)
				{
					foreach (InstanceInfo instanceInfo in ConnectionGroup.Connections)
					{
						Int64? queryId = queryDirectory.GetQueryId(
							this,
							queryInfo,
							instanceInfo,
							DateTime.Now,
							false
						);

						if (queryId.HasValue)
						{
							queryParameterDirectory.ReadParameter(
								ConnectionGroup,
								queryId.Value,
								queryInfo,
								paramInfo
							);

							break;
						}
					}
				}
			}
		}

		public TemplateNodeInfo GetRootPatent()
		{
			TemplateNodeInfo result = this;

			while (result.Parent != null)
			{
				result = result.Parent;
			}

			return result;
		}

		public void SetAttributeValue(string key, string value)
		{
			if (this.Attributes.Values.ContainsKey(key))
			{
				this.Attributes.Values[key] = value;
			}
			else
			{
				this.Attributes.Values.Add(key, value);
			}
		}

		private TemplateNodeInfo Search(TemplateNodeInfo searchable)
		{
			if (this.TemplateNodeId == searchable.TemplateNodeId)
			{
				return this;
			}

			foreach (var templateNodeInfo in this.Childs)
			{
				if (templateNodeInfo.TemplateNodeId == searchable.TemplateNodeId)
				{
					return templateNodeInfo;
				}

				var founded = templateNodeInfo.Search(searchable);

				if (founded != null)
				{
					return founded;
				}
			}

			return null;
		}

		public void SetConnectionData(ConnectionData connection)
		{
			this.Connection = connection;
		}
	}
}
