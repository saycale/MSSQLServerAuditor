using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using MSSQLServerAuditor.BusinessLogic.LocalStorage;
using MSSQLServerAuditor.Model.Groups;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Gui;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Managers
{
	public class TreeTaskManager
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly object                _schedulingLock;
		private readonly object                _runningTasksLock;
		private readonly ConnectionTabControl  _treeControl;
		private readonly NodeCounter           _nodeCounter;

		public TreeTaskProgressManager                        ProgressManager { get; private set; }
		public ObservableConcurrentDictionary<long, TreeTask> RunningTasks    { get; private set; }

		private TreeTaskManager()
		{
			this._schedulingLock   = new object();
			this._runningTasksLock = new object();
			this._treeControl      = null;
			this.ProgressManager   = null;
			this._nodeCounter      = null;
			this.RunningTasks      = new ObservableConcurrentDictionary<long, TreeTask>();
		}

		public TreeTaskManager(ConnectionTabControl treeControl) : this()
		{
			this.ProgressManager = new TreeTaskProgressManager(this);
			this._treeControl    = treeControl;
			this._nodeCounter    = new NodeCounter(treeControl._model);
		}

		public void StopTasks()
		{
			lock (this._runningTasksLock)
			{
				foreach (TreeTask task in this.RunningTasks.Values.ToArray())
				{
					task.Cancel();
				}
			}
		}

		public TreeTask BeginRefreshTask(
			TreeNode           treeNode,
			bool               hierarchically,
			NodeUpdatingSource mode,
			Action             continueWith = null
		)
		{
			if (treeNode == null)
			{
				return null;
			}

			ConcreteTemplateNodeDefinition templateDef =
				treeNode.Tag as ConcreteTemplateNodeDefinition;

			if (templateDef == null)
			{
				log.Debug("MSSQLServerAuditor.TreeTaskManager:BeginRefreshTask templateDef is not defined.");
				return null;
			}

			long nodeHandle = templateDef.ComputeHandle();

			if (this.RunningTasks.ContainsKey(nodeHandle))
			{
				return null;
			}

			ConnectionData connectionData = GetConnectionData(treeNode);

			TreeTaskInfo taskInfo = new TreeTaskInfo
			{
				Connection     = connectionData,
				Mode           = mode,
				Hierarchically = hierarchically,
				Note           = string.Format("{0}", DateTime.Now.ToString("mm:ss")),
				Handle         = nodeHandle
			};

			TreeTask treeTask = TreeTask.Create(taskInfo);

			if (this._treeControl != null)
			{
				treeTask.Progress.ProgressChanged +=
					(sender, args) => this._treeControl.SetProgressValue((int) args.NewValue);
			}

			treeTask.Completed += (sender, args) =>
			{
				Task.Factory.StartNew(() => RemoveClosedTask(treeTask));

				if (continueWith != null)
				{
					continueWith();
				}
			};

			lock (this._runningTasksLock)
			{
				this.RunningTasks.TryAdd(taskInfo.Handle, treeTask);
			}

			// don't change order of Add and Subscribe!
			treeTask.JobChanged += OnTaskJobChanged;

			if (this._treeControl != null)
			{
				this._treeControl.SetInProgress(taskInfo, true, true);
			}

			ProgressItem progress = new ProgressItem();

			RefreshNode(treeTask, null, treeNode, progress);

			return treeTask;
		}

		private CurrentStorage GetStorage(
			ConnectionGroupInfo connectionGroup
		)
		{
			return this._treeControl._model.GetVaultProcessor(connectionGroup).CurrentStorage;
		}

		private void RemoveClosedTask(
			TreeTask completeTask
		)
		{
			long taskHandle     = completeTask.Info.Handle;
			bool executingTasks = false;

			lock (this._runningTasksLock)
			{
				if (this.RunningTasks.ContainsKey(taskHandle))
				{
					TreeTask removedTask;

					this.RunningTasks.TryRemove(taskHandle, out removedTask);
				}

				completeTask.JobChanged -= OnTaskJobChanged;
			}

			lock (this._runningTasksLock)
			{
				executingTasks = this.RunningTasks.Any();
			}

			this._treeControl.SetInProgress(completeTask.Info, false, executingTasks);
		}

		private void RefreshNode(
			TreeTask     treeTask,
			TreeJob      parentJob,
			TreeNode     treeNode,
			ProgressItem progress
		)
		{
			ConcreteTemplateNodeDefinition nodeDefinition = treeNode.Tag as ConcreteTemplateNodeDefinition;

			if (treeTask.CancellationSource.IsCancellationRequested)
			{
				return;
			}

			if (nodeDefinition == null)
			{
				return;
			}

			TreeJob treeJob = new TreeJob(nodeDefinition);

			if (parentJob != null)
			{
				parentJob.AddChildJob(treeJob);
				treeJob.Parent = parentJob;
			}

			WeakReference treeNodeRef = new WeakReference(treeNode);

			treeTask.JobCompleted += (sender, args) =>
			{
				TreeTask task = sender as TreeTask;

				if (task != null && treeNodeRef.IsAlive)
				{
					if (!task.CancellationSource.IsCancellationRequested)
					{
						TreeNode targetNode = (TreeNode) treeNodeRef.Target;
						UpdateTreeCounts(targetNode, task.Info.Mode);
					}
				}
			};

			bool refreshChildren = treeTask.Info.Hierarchically;

			Action refreshNodeAction = () =>
			{
				try
				{
					TemplateNodeInfo templateNode = nodeDefinition.TemplateNode;

					if (templateNode == null || !nodeDefinition.NodeAvailable)
					{
						return;
					}

					templateNode.LastUpdateNode = DateTime.Now;

					CurrentStorage storage = GetStorage(templateNode.ConnectionGroup);

					templateNode.LoadUserParameters(storage);

					if (nodeDefinition.NodeActivated)
					{
						RefreshQueries(treeTask, nodeDefinition, progress);
					}

					if (templateNode.IsLeaf())
					{
						return;
					}

					AddLoadingNode(treeNode);

					templateNode.UpdateChildren(treeTask.Info.Mode, treeTask.CancellationSource);
					FillTreeNodes(templateNode, treeNode.Nodes);

					if (refreshChildren)
					{
						treeJob.PromisedChildCount = treeNode.Nodes.Count;

						foreach (TreeNode node in treeNode.Nodes)
						{
							if (node != null)
							{
								RefreshNode(treeTask, treeJob, node, progress);
							}
						}
					}
				}
				catch (Exception exc)
				{
					log.Error("MSSQLServerAuditor.Managers:RefreshNode:subTask (Exception)", exc);
				}
			};

			treeJob.Action = refreshNodeAction;

			lock (this._schedulingLock)
			{
				treeTask.Schedule(treeJob);
			}
		}

		private void UpdateTreeCounts(
			TreeNode           parentNode,
			NodeUpdatingSource mode
		)
		{
			StartUpdateNodeCounterTask(parentNode, mode).Wait();
		}

		private Task StartUpdateNodeCounterTask(
			TreeNode           treeNode,
			NodeUpdatingSource mode,
			Action             onFinished = null
		)
		{
			Task updateNodeTask = this._nodeCounter.StartUpdateTask(treeNode, mode)
				.ContinueWith(t =>
				{
					if (t.IsFaulted)
					{
						HandleTaskFailure(t);
					}
					else
					{
						if (t.IsCompleted)
						{
							try
							{
								int? count = t.Result;

								UpdateNodeTitle(treeNode, count);
							}
							catch (Exception exc)
							{
								log.Error("Failed to update node title with count.", exc);
							}
						}
					}

					if (onFinished != null)
					{
						onFinished();
					}
				});

			return updateNodeTask;
		}

		private void UpdateNodeTitle(
			TreeNode treeNode,
			int?     nodeCount
		)
		{
			ConcreteTemplateNodeDefinition nodeDefinition = treeNode.Tag as ConcreteTemplateNodeDefinition;
			string                         text           = string.Empty;

			if (nodeDefinition != null)
			{
				if (nodeDefinition.TemplateNode != null)
				{
					nodeDefinition.TemplateNode.CounterValue = nodeCount;
				}

				Action updateNodeTextAction = () =>
				{
					if (nodeDefinition.IsRoot)
					{
						TemplateNodeLocaleInfo liTreeTitle = nodeDefinition.TemplateNode.GetTemplateTreeTitleLocalized();

						string templateName = nodeDefinition.TemplateNode.Title;
						string connectionName = nodeDefinition.Connection.Name;

						text = GetTreeTitle(liTreeTitle, templateName, connectionName);
					}
					else
					{
						text = nodeDefinition.FormatNodeText(nodeCount);
					}

					if (!text.Equals(treeNode.Text))
					{
						treeNode.Text = text;
					}
				};

				this._treeControl.SafeInvoke(updateNodeTextAction);
			}
		}

		private string GetTreeTitle(
			TemplateNodeLocaleInfo liTreeTitle,
			string                 templateName,
			string                 connectionName
		)
		{
			string strTreeTitle         = String.Empty;
			string treeTitleMacroFrtStr = String.Empty;
			string macroStr             = String.Empty;

			if (liTreeTitle != null)
			{
				macroStr = liTreeTitle.Text;

				if (macroStr != null)
				{
					macroStr = macroStr.Replace("\r", String.Empty);
					macroStr = macroStr.Replace("\n", String.Empty);
					macroStr = macroStr.Replace("\t", String.Empty);
				}

				treeTitleMacroFrtStr = macroStr;
			}
			else
			{
				// default macro
				treeTitleMacroFrtStr = "$ModuleName$";
			}

			if (treeTitleMacroFrtStr != null)
			{
				treeTitleMacroFrtStr = treeTitleMacroFrtStr.Replace("$ModuleName$",     "{0}");
				treeTitleMacroFrtStr = treeTitleMacroFrtStr.Replace("$ConnectionName$", "{1}");

				strTreeTitle = string.Format(treeTitleMacroFrtStr,
					templateName   ?? "<-->",
					connectionName ?? "<-->"
				);
			}

			return strTreeTitle;
		}

		private void HandleTaskFailure(Task task)
		{
			if (task.Exception != null)
			{
				log.ErrorFormat("Task execution failed:{0}", task.Exception);
			}
		}

		private void OnTaskJobChanged(object sender, TreeJobChangedEventArgs args)
		{
			if (args.Event != TreeJobChangedEvent.RunningCompleted)
			{
				return;
			}

			lock (this._schedulingLock)
			{
				lock (this._runningTasksLock)
				{
					foreach (TreeTask treeTask in this.RunningTasks.Values)
					{
						if (treeTask.SubmitNextJob())
						{
							break;
						}
					}
				}
			}
		}

		private void RefreshQueries(
			TreeTask                       treeTask,
			ConcreteTemplateNodeDefinition nodeDefinition,
			ProgressItem                   progress
		)
		{
			log.DebugFormat("taskInfo.Connection:'{0}';nodeDefinition:'{1}'",
				treeTask.Info.Connection.ConnectionGroup.ToString() ?? "?",
				nodeDefinition.TemplateNode.Name ?? "?"
			);

			ErrorLog          errorLog      = new ErrorLog();
			MsSqlAuditorModel model         = this._treeControl._model;
			DateTime          startTime     = DateTime.Now;
			Stopwatch         durationWatch = new Stopwatch();

			durationWatch.Start();

			using (SqlProcessor sqlProcessor = model.GetNewSqlProcessor(treeTask.CancellationSource.Token))
			{
				if (!treeTask.Info.Connection.IsLiveConnection)
				{
					sqlProcessor.SetSkipMSSQLQueries();
				}

				progress.SetPromisedChildCount(1);

				TemplateNodeInfo     templateNode = nodeDefinition.TemplateNode;
				ConnectionGroupInfo  group        = nodeDefinition.Connection;
				MultyQueryResultInfo result;

				if (nodeDefinition.Group.Instance != null)
				{
					result = sqlProcessor.ExecuteMultyQuery(
						nodeDefinition.Group,
						templateNode.Queries,
						progress.GetChild()
					);
				}
				else
				{
					result = sqlProcessor.ExecuteMultyQuery(
						group,
						templateNode.Queries,
						progress.GetChild(),
						model.Settings.SystemSettings.MaximumDBRequestsThreadCount
					);
				}

				if (group != null && group.Connections != null)
				{
					group.Connections.ForEach(
						x => x.ConnectionGroup = x.ConnectionGroup ?? group
					);
				}

				errorLog.AppendErrorLog(result);

				IStorageManager storage = model.GetVaultProcessor(group);

				storage.SaveRequestedData(templateNode, result);

				durationWatch.Stop();

				DateTime duration = new DateTime(durationWatch.Elapsed.Ticks);

				storage.CurrentStorage.UpdateTreeNodeTimings(
					templateNode,
					startTime,
					duration
				);

				foreach (GroupDefinition database in result.ExtractDatabases())
				{
					ConcreteTemplateNodeDefinition nodeDef = new ConcreteTemplateNodeDefinition(
						templateNode,
						database,
						group
					);

					this._treeControl.VisualizeData(nodeDef);
				}
			}
		}

		private void FillTreeNodes(
			TemplateNodeInfo   parent,
			TreeNodeCollection treeNodes
		)
		{
			List<Tuple<TreeNode, ConcreteTemplateNodeDefinition>> pendingUpdateNodes =
				new List<Tuple<TreeNode, ConcreteTemplateNodeDefinition>>();

			Action action = () =>
			{
				treeNodes.Clear();

				treeNodes.AddRange(parent.Childs.Select(n =>
				{
					ConcreteTemplateNodeDefinition nodedef;
					TreeNode                       node = this._treeControl.CreateTreeViewNode(n, out nodedef);

					pendingUpdateNodes.Add(new Tuple<TreeNode, ConcreteTemplateNodeDefinition>(node, nodedef));

					return node;
				}).ToArray());

				if (parent.ChildrenAreLoadingNow)
				{
					treeNodes.Add(new TreeNode(this._treeControl._model.LocaleManager.GetLocalizedText(
						"common", "NodesQueryingTreeNodeText")
					)
					{
						ImageKey         = "NodesQuerying",
						SelectedImageKey = "NodesQuerying"
					});
				}
			};

			this._treeControl.SafeInvoke(action);

			foreach (Tuple<TreeNode, ConcreteTemplateNodeDefinition> pendingNode in pendingUpdateNodes)
			{
				TreeNode node                          = pendingNode.Item1;
				ConcreteTemplateNodeDefinition nodeDef = pendingNode.Item2;

				if (parent.IsDisabled)
				{
					nodeDef.TemplateNode.IsDisabled = true;
					nodeDef.NodeActivated           = false;
				}

				nodeDef.NodeAvailable = nodeDef.IsAvailableForDatabase(Program.Model) ?? true;

				if (!nodeDef.NodeAvailable)
				{
					this._treeControl.SetNotAvailableNode(node);
				}
				else
				{
					this._treeControl.SetNodeLoaded(node);

					List<TemplateNodeUpdateJob> refreshJobs = nodeDef.TemplateNode.GetRefreshJob(true);

					nodeDef.TemplateNode.HasActiveJobs = refreshJobs.Any(
						job =>
							job != null
							&& !job.IsEmpty()
							&& job.Enabled
					);

					UpdateTreeCounts(node, NodeUpdatingSource.LocallyOnly);
				}
			}
		}

		public void EnsureChildrenReady(TreeNode node)
		{
			TreeTask treeTask = BeginRefreshTask(node, false, NodeUpdatingSource.FromServerIfNotSavedLocally);

			if (treeTask != null)
			{
				EventWaitHandle alarm               = new EventWaitHandle(false, EventResetMode.ManualReset);
				EventHandler    taskCompleteHandler = null;

				taskCompleteHandler = (sender, args) =>
				{
					alarm.Set();
					treeTask.Completed -= taskCompleteHandler;
				};

				treeTask.Completed += taskCompleteHandler;

				alarm.WaitOne();
			}
		}

		private ConnectionData GetConnectionData(TreeNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			var tag = node.Tag as ConcreteTemplateNodeDefinition;

			return tag != null ? tag.TemplateNode.Connection : null;
		}

		private void AddLoadingNode(TreeNode treeNode)
		{
			Action action = () =>
			{
				treeNode.Nodes.Clear();

				treeNode.Nodes.Add(
					new TreeNode(this._treeControl._model.LocaleManager.GetLocalizedText("common", "NodesQueryingTreeNodeText"))
					{
						ImageKey         = "NodesQuerying",
						SelectedImageKey = "NodesQuerying"
					}
				);
			};

			this._treeControl.SafeInvoke(action);
		}
	}
}
