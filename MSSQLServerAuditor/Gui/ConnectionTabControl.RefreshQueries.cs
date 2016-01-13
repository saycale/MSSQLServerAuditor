using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using log4net;
using MSSQLServerAuditor.Preprocessor;
using MSSQLServerAuditor.Managers;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	/// ConnectionTabControl.RefreshQueries.cs
	/// </summary>
	public partial class ConnectionTabControl
	{
		private static readonly ILog logConnectionTabControl = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private TreeTaskManager      _taskManager;
		public Action                ContinueWith = null;

		public TreeTaskManager TaskManager
		{
			get { return this._taskManager ?? (this._taskManager = new TreeTaskManager(this)); }
		}

		public void SetInProgress(TreeTaskInfo taskInfo, bool currentInProgress, bool existsInProgress)
		{
			Action action = () =>
			{
				btnStop.Visible = existsInProgress;

				//processesProgressBar.Value = existsInProgress ? 0 : 100;
				//processesProgressBar.Visible = existsInProgress;
				processWithLabel.Value        = existsInProgress ? 0 : 100;
				processWithLabel.ShowProgerss = existsInProgress;

				progressDetailsButton.Visible = existsInProgress;

				// updateNodeToolStripMenuItem.Enabled = !inProgress;
				// updateHierarciallyToolStripMenuItem.Enabled = !inProgress;
				// refreshTreeToolStripMenuItem.Enabled = !inProgress;
			};

			this.SafeInvoke(action);
		}

		public void SetProgressValue(int value)
		{
			//Action action = () => processesProgressBar.Value = value;
			Action action = () => processWithLabel.Value = value;
			this.SafeInvoke(action);
		}

		#region Events with refreshing

		private void BtnStopButtonClick(object sender, EventArgs e)
		{
			TaskManager.StopTasks();
		}

		private void UpdateMenuItemClick(object sender, EventArgs e)
		{
			TaskManager.BeginRefreshTask(treeTemplate.SelectedNode, false, NodeUpdatingSource.ForcedFromServer);
		}

		private void UpdateHierarciallyMenuItemClick(object sender, EventArgs e)
		{
			TaskManager.BeginRefreshTask(treeTemplate.SelectedNode, true, NodeUpdatingSource.ForcedFromServer);
		}

		public void F5RefreshView(TreeNode refreshnode = null)
		{
			var treeNode = refreshnode ?? treeTemplate.SelectedNode;

			if (treeNode != null)
			{
				var connectionData = GetConnectionDataForTreeNode(treeNode);

				if (connectionData.IsLiveConnection)
				{
					TaskManager.BeginRefreshTask(treeNode, false, NodeUpdatingSource.ForcedFromServer);
				}
			}
		}

		public void AppendConnectionData(ConnectionData connection)
		{
			this._connections.Add(connection);
			treeTemplate.ContextMenuStrip = null;
			NewFillTemplateTreeView(true);
		}

		#region Timer Scheduling Methods

		private TreeNode FindJobbedUiTreeNode(Job job)
		{
			var nodesList = new List<TreeNode>();

			foreach (TreeNode currentNode in treeTemplate.Nodes)
			{
				nodesList.AddRange(GetEnabledTreenodesForTimingTick(currentNode));
			}

			foreach (var treeNode in nodesList)
			{
				var node = treeNode.Tag as ConcreteTemplateNodeDefinition;

				if (node == null)
				{
					continue;
				}

				TemplateNodeUpdateJob tjob = node.TemplateNode.GetRefreshJob(true).FirstOrDefault(j => j == job);

				if (tjob != null && !tjob.IsEmpty())
				{
					return treeNode;
				}
			}

			return null;
		}

		private void tmSchedulerTimer_Tick(object sender, EventArgs e)
		{
			foreach (ConnectionData connection in Connections)
			{
				List<Tuple<TemplateNodeInfo, Job>> nodeJobs = new List<Tuple<TemplateNodeInfo, Job>>();

				connection.RootOfStaticTree.GetScheduledNodes(nodeJobs,true);

				foreach (Tuple<TemplateNodeInfo, Job> nodeJob in nodeJobs)
				{
					Job              job             = nodeJob.Item2;
					TemplateNodeInfo node            = nodeJob.Item1;
					ConnectionData   localConnection = connection;

					Action action = () =>
					{
						TreeNode uiTreeNode      = FindJobbedUiTreeNode(job) ?? CreateTreeViewNode(node);
						Action   sendEmailAction = null;

						if (job.Settings.IsSendMessage)
						{
							sendEmailAction = () =>
							{
								ConcreteTemplateNodeDefinition definition = new ConcreteTemplateNodeDefinition(
									node,
									localConnection.ConnectionGroup
								);

								AddEmailTask(definition, job);
							};
						}

						TaskManager.BeginRefreshTask(
							uiTreeNode,
							true,
							NodeUpdatingSource.ForcedFromServer,
							sendEmailAction
						);
					};

					this._jobProcessor.RunIfTime(
						job,
						action,
						connection.CreationDateTime
					);
				}
			}
		}

		public void AddEmailTask(ConcreteTemplateNodeDefinition definition, Job job)
		{
			Func<NodeDataProvider> getNodeDataProviderFunc =
				() => new NodeDataProvider(this._model, definition);

			Task<NodeDataProvider> queryReportTask = Task.Factory.StartNew(getNodeDataProviderFunc);

			EmailNotificationTask emlTask = new EmailNotificationTask(
				this._allEmailTask,
				job,
				this._model.VisualizeProcessor
			);

			queryReportTask.ContinueWith(emlTask.onQueryResult);
		}

		private List<TreeNode> GetEnabledTreenodesForTimingTick(TreeNode rootNode)
		{
			List<TreeNode> resultNodes = new List<TreeNode>();

			foreach (TreeNode currentnode in rootNode.Nodes)
			{
				ConcreteTemplateNodeDefinition currentNodeDefinition =
					(currentnode.Tag as ConcreteTemplateNodeDefinition);

				if (currentNodeDefinition != null &&
					currentNodeDefinition.NodeActivated &&
					currentNodeDefinition.NodeAvailable &&
					currentNodeDefinition.TemplateNode != null
				)
				{
					List<TemplateNodeUpdateJob> jobList = currentNodeDefinition.TemplateNode.GetRefreshJob(true);

					foreach (TemplateNodeUpdateJob job in jobList)
					{
						if (job != null && !job.IsEmpty() && job.Enabled)
						{
							resultNodes.Add(currentnode);
						}
					}
				}

				List<TreeNode> nodes = GetEnabledTreenodesForTimingTick(currentnode);

				if (nodes.Count > 0)
				{
					resultNodes.AddRange(nodes);
				}
			}

			return resultNodes;
		}
		#endregion

		#endregion

		private void NewFillTemplateTreeView(bool boolDoNotClearNodes)
		{
			string templateName = null;

			using (new WaitCursor())
			{
				treeTemplate.BeginUpdate();

				try
				{
					var wasState = SaveTreeState(treeTemplate);

					if (!boolDoNotClearNodes)
					{
						treeTemplate.Nodes.Clear();
					}

					foreach (var cnn in Connections)
					{
						if (cnn.RootInstance != null && boolDoNotClearNodes)
						{
							continue;
						}

						cnn.RootInstance      = cnn.RootOfTemplate.Instantiate(cnn, null, null);
						cnn.RootInstance.Name = cnn.Title;

						var rootNode = CreateConnectionNode(treeTemplate.Nodes, cnn, cnn.RootInstance);

						if (cnn.ConnectionGroup != null)
						{
							CurrentStorage db = this._model.GetVaultProcessor(cnn.ConnectionGroup).CurrentStorage;

							cnn.ConnectionGroup.ReadGroupIdFrom(db.ConnectionGroupDirectory);

							Int64? templateId = db.TemplateDirectory.GetId(cnn.ConnectionGroup);

							if (!db.NodeInstances.TryLoadRoodId(cnn.RootInstance, cnn.ConnectionGroup.Identity, templateId))
							{
								db.NodeInstances.Save(cnn.RootInstance);
							}

							ConnectionData connectionData = cnn;

							Action selectTask = () =>
							{
								if (!string.IsNullOrEmpty(connectionData.StartupTemplateId))
								{
									var selectFunction = new Action(() =>
									{
										rootNode.TreeView.SelectedNode = FindChildNode(
											rootNode.Nodes,
											connectionData.StartupTemplateId
										);
									});

									ExpandNodeToLoadTemplate(
										connectionData.StartupTemplateInfoIdStack,
										rootNode.Nodes,
										connectionData.StartupTemplateId,
										selectFunction
									);
								}
							};

							TaskManager.BeginRefreshTask(rootNode, false, NodeUpdatingSource.FromServerIfNotSavedLocally, () =>
								{
									this.SafeInvoke(rootNode.Expand);
									this.SafeInvoke(selectTask);
								}
							);
						}
					}

					ApplyTreeState(treeTemplate, wasState);
				}
				finally
				{
					treeTemplate.EndUpdate();
				}

				tmSchedulerTimer.Start();

				var connection = Connections.FirstOrDefault();

				if (connection != null)
				{
					var localeitem = connection.RootInstance.GetLocale(false);

					if (localeitem != null)
					{
						templateName = localeitem.Text;
					}
				}

				OnTreeSelectionChanged(templateName);

				if (TreeContainsOnlyOneReportNode(treeTemplate.Nodes))
				{
					treeTemplate.Hide();
				}
			}
		}

		/// <summary>
		/// Expand node.
		/// </summary>
		private void ExpandNodeToLoadTemplate(Stack<String> templatesToExpand, TreeNodeCollection nodes, string selectTemplateId, Action finalAction)
		{
			if (templatesToExpand != null)
			{
				if (templatesToExpand.Count > 0)
				{
					var currentId = templatesToExpand.Pop();

					var currentTemplateToExpand = nodes.OfType<TreeNode>().FirstOrDefault(x =>
						x.Tag is ConcreteTemplateNodeDefinition &&
						(x.Tag as ConcreteTemplateNodeDefinition).TemplateNode.Id == currentId);

					if (currentTemplateToExpand != null && !currentTemplateToExpand.IsExpanded)
					{
						if (currentId == selectTemplateId)
						{
							Invoke(finalAction);
						}
						else
						{
							this.ContinueWith = () =>
							{
								Invoke(
									new Action(
										() => ExpandNodeToLoadTemplate(
											templatesToExpand,
											currentTemplateToExpand.Nodes,
											selectTemplateId,
											finalAction
										)
									)
								);

								this.ContinueWith = null;
							};

							currentTemplateToExpand.Expand();
						}
					}
				}
			}
		}

		private TreeNode FindChildNode(TreeNodeCollection parentNodes, string templateId)
		{
			foreach (TreeNode currentNode in parentNodes)
			{
				ConcreteTemplateNodeDefinition nodeDefinition = currentNode.Tag as ConcreteTemplateNodeDefinition;

				if (nodeDefinition != null)
				{
					if (nodeDefinition.TemplateNode != null &&
						nodeDefinition.TemplateNode.Id == templateId
					)
					{
						return currentNode;
					}

					if (currentNode.Nodes.Count > 0)
					{
						var node = FindChildNode(currentNode.Nodes,templateId);

						if (node != null)
						{
							return node;
						}
					}
				}
			}

			return null;
		}

		private bool TreeContainsOnlyOneReportNode(TreeNodeCollection nodes)
		{
			if (nodes.Count > 1)
			{
				return false;
			}

			foreach (var n in nodes.OfType<TreeNode>())
			{
				if (n.Tag == _MakeExpandableNodeTag)
				{
					return false;
				}

				ConcreteTemplateNodeDefinition nodeDef = n.Tag as ConcreteTemplateNodeDefinition;

				if (nodeDef == null)
				{
					return false;
				}

				if (nodeDef.TemplateNode.GetParentQuery() != null)
				{
					return false;
				}

				var hasReport = !string.IsNullOrEmpty(nodeDef.TemplateNode.XslTemplateFileName);

				if (hasReport)
				{
					return n.Nodes.Count == 0;
				}

				return TreeContainsOnlyOneReportNode(n.Nodes);
			}

			return false;
		}
	}
}
