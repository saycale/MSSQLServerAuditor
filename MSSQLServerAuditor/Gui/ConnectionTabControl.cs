using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using log4net;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.BusinessLogic.UserSettings;
using MSSQLServerAuditor.Gui.Base;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Model.UserSettingsParameters;
using MSSQLServerAuditor.Preprocessor;
using MSSQLServerAuditor.ReportViewer;
using MSSQLServerAuditor.SQLite.Tables.UserSettings;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	/// Control for operating with connection group
	/// </summary>
	public partial class ConnectionTabControl : UserControl
	{
		private static readonly ILog                        log1 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly object                      _MakeExpandableNodeTag = new object();
		private readonly        List<ConnectionData>        _connections;
		public readonly         MsSqlAuditorModel           _model;
		private readonly        bool                        _isDisableNavigationPanel;
		private                 Template                    _template;
		private                 DateTime                    _runningProcessFormClosedTime;
		private                 RunningTaskInfoForm         _runningTaskInfoForm;
		private                 CancellationTokenSource     _reportViewCanceler;
		private readonly        ScheduleJobProcessor        _jobProcessor;
		private readonly        List<EmailNotificationTask> _allEmailTask;
		private readonly        TabPageBuffer               _pageBuffer;

		#region Constructors

		private ConnectionTabControl()
		{
			this._connections                  = new List<ConnectionData>();
			this._model                        = null;
			this._isDisableNavigationPanel     = false;
			this._template                     = null;
			this._runningProcessFormClosedTime = DateTime.MinValue;
			this._runningTaskInfoForm          = null;
			this._reportViewCanceler           = null;
			this._jobProcessor                 = null;
			this._allEmailTask                 = new List<EmailNotificationTask>();
			this._pageBuffer                   = new TabPageBuffer();

			InitializeComponent();
			BuildImageList();

			txtXml.Padding = new Padding(0);

			if (!Program.Model.Settings.ShowXML)
			{
				tcBrowse.TabPages.Remove(tpXml);
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="model">Using model</param>
		/// <param name="connectionsAsNodes">All connection (even single one) will be represented
		/// as top level node of tree navigation view</param>
		public ConnectionTabControl(
			MsSqlAuditorModel model,
			bool              isDisableStatusLine,
			bool              isDisableNavigationPanel,
			bool              connectionsAsNodes = true
		) : this()
		{
			this._model                 = model;
			this._model.SettingsChanged += ModelSettingsChanged;

			this._jobProcessor    = new ScheduleJobProcessor(
				this._model.DefaultVaultProcessor.CurrentStorage
			);

			this.ConnectionsAsNodes = connectionsAsNodes;

			treeTemplate.DrawMode = TreeViewDrawMode.OwnerDrawText;

			this._isDisableNavigationPanel = isDisableNavigationPanel;

			SetSettings(isDisableStatusLine);
		}

		#endregion Constructors

		#region Properties

		public bool CanSaveRaw
		{
			get
			{
				var cnn = GetSelectedConnection();

				return cnn != null && cnn.IsLiveConnection;
			}
		}

		public IEnumerable<ConnectionData> Connections
		{
			get
			{
				return this._connections.AsReadOnly();
			}
		}

		public bool ConnectionsAsNodes { get; private set; }

		private ConcreteTemplateNodeDefinition CurrentConcreteTemplateNodeDefinition
		{
			get
			{
				return GetConcreteTemplateNodeDefinitions(treeTemplate.SelectedNode, true).FirstOrDefault();
			}
		}

		public Template SelectedTemplate
		{
			get { return this._template; }
			set { this._template = value; }
		}

		public static string GetCloseConnectionCommandText(LocaleManager localeManager)
		{
			return localeManager.GetLocalizedText("frmMain", "mnuCloseConnection");
		}

		#endregion Properties

		#region Methods

		public void CloseConnection(ConnectionData connection)
		{
			if (connection != null)
			{
				if (!this._connections.Remove(connection))
				{
					throw new ArgumentException();
				}

				this._model.DeassociateVaultProcessor(connection.ConnectionGroup);

				NewFillTemplateTreeView(false);
			}
		}

		/// <summary>
		/// Get size of Html window
		/// </summary>
		/// <returns></returns>
		public Size GetHtmlReportClientSize()
		{
			if (tcBrowse.DisplayRectangle.Width > 0 && tcBrowse.DisplayRectangle.Height > 0)
			{
				return new Size(
					tcBrowse.DisplayRectangle.Width - tcBrowse.Margin.Size.Width / 2,
					tcBrowse.DisplayRectangle.Height - tcBrowse.Margin.Size.Height / 2
				);
			}

			return new Size(
				tcBrowse.PreferredSize.Width - tcBrowse.Margin.Size.Width / 2,
				tcBrowse.PreferredSize.Height - tcBrowse.Margin.Size.Height / 2
			);
		}

		public ConnectionData GetSelectedConnection()
		{
			if (this._connections == null)
			{
				return null;
			}

			if (this._connections.Count == 0)
			{
				return null;
			}

			if (this._connections.Count == 1 || treeTemplate.SelectedNode == null)
			{
				return this._connections.Last();
			}

			return GetConnectionDataForTreeNode(treeTemplate.SelectedNode);
		}

		private ConnectionData GetConnectionDataForTreeNode(TreeNode node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			var tag = node.Tag as ConcreteTemplateNodeDefinition;

			if (tag != null)
			{
				return tag.TemplateNode.Connection;
			}

			return null;
		}

		public void OpenConnection(ConnectionData connectionData)
		{
			AppendConnectionData(connectionData);

			TreeIsVisible = !this._isDisableNavigationPanel;
		}

		public void OpenConnection(ConnectionGroupInfo connectionGroup)
		{
			OpenConnection(new ConnectionData(this._model, connectionGroup));
		}

		private static void ApplyTreeState(TreeView tree, object state)
		{
			var expanded = (HashSet<object>)state;

			tree.BeginUpdate();

			try
			{
				ForEachNode(tree,
					n =>
					{
						if (n.Tag != null)
						{
							if (expanded.Contains(n.Tag))
							{
								n.Expand();
							}
						}

						return n.IsExpanded;
					});
			}
			finally
			{
				tree.EndUpdate();
			}
		}

		private static string ComposeBlankDateTimeString()
		{
			var digits = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
			var s      = new DateTime().ToString();

			return digits.Aggregate(s, (current, digit) => current.Replace(digit, '-'));
		}

		private static TreeNode CreateConnectionNode(TreeNodeCollection nodes, ConnectionData connection, TemplateNodeInfo rootNode)
		{
			Debug.Assert(rootNode.Parent == null);
			Debug.Assert(rootNode.IsInstance);

			var iconName = connection.IsLiveConnection
				? ApplicationResources.ConnectionNodeImageRefName
				: ApplicationResources.RawDataNodeImageRefName;

			TreeNode newNode = new TreeNode
			{
				Tag              = new ConcreteTemplateNodeDefinition(rootNode, connection.ConnectionGroup),
				Text             = connection.Title,
				ImageKey         = iconName,
				SelectedImageKey = iconName,
			};

			nodes.Add(newNode);

			return newNode;
		}

		private static void ForEachNode(
			TreeView              tree,
			Func<TreeNode, bool>  action,
			IEnumerable<TreeNode> nodes = null
		)
		{
			foreach (var n in nodes ?? tree.Nodes.OfType<TreeNode>())
			{
				if (action(n))
				{
					ForEachNode(tree, action, n.Nodes.OfType<TreeNode>());
				}
			}
		}

		private static string GetIcon(ConcreteTemplateNodeDefinition node)
		{
			if (!string.IsNullOrWhiteSpace(node.TemplateNode.UIcon))
			{
				return node.TemplateNode.UIcon;
			}

			return node.TemplateNode.IconImageReferenceName;
		}

		private static FontStyle ParseFontStyle(string s)
		{
			FontStyle result = FontStyle.Regular;

			s = s.ToLower();

			if (s.Contains("italic"))
			{
				result = result | FontStyle.Italic;
			}

			if (s.Contains("bold"))
			{
				result = result | FontStyle.Bold;
			}

			if (s.Contains("underline"))
			{
				result = result | FontStyle.Underline;
			}

			if (s.Contains("strikeout"))
			{
				result = result | FontStyle.Strikeout;
			}

			return result;
		}

		private static object SaveTreeState(TreeView tree)
		{
			var expanded = new HashSet<object>();

			ForEachNode(tree, n =>
			{
				if (n.IsExpanded && n.Tag != null && !expanded.Contains(n.Tag))
				{
					expanded.Add(n.Tag);
				}

				return n.IsExpanded;
			});

			return expanded;
		}

		private void BuildImageList()
		{
			// empty image for nodes with no icon
			treeImageList.Images.Add(new Bitmap(treeImageList.ImageSize.Width, treeImageList.ImageSize.Height));

			var images = ApplicationResources.GetImages().Where(i => i.Picture.Size.Equals(treeImageList.ImageSize)).ToArray();

			foreach (var img in images)
			{
				treeImageList.Images.Add(img.Picture);

				treeImageList.Images.SetKeyName(treeImageList.Images.Count - 1, img.Name);
			}
		}

		private void cmsTree_Opening(object sender, CancelEventArgs e)
		{
			treeTemplate.ContextMenuStrip = null;

			cmsTree.SuspendLayout();

			ConcreteTemplateNodeDefinition nodeDef      = null;
			TreeNode                       selectedNode = treeTemplate.SelectedNode;
			bool                           isExpandable = false;

			if (selectedNode != null)
			{
				isExpandable = selectedNode.FirstNode != null;

				nodeDef = selectedNode.Tag as ConcreteTemplateNodeDefinition;

				mnuCollapseHierarchically.Enabled = selectedNode.IsExpanded;
				mnuExpandHierarchically.Enabled   = isExpandable;
			}

			ConnectionData connection = GetSelectedConnection();

			bool menuEnabled = connection != null && connection.ConnectionGroup != null;

			mnuUpdateHierarcially.Enabled = menuEnabled;
			mnuUpdateNode.Enabled         = menuEnabled;
			mnuRefreshTree.Enabled        = menuEnabled;

			// set default menu visibility
			mnuNodeSettings.Visible       = false;
			mnuSettingsParameters.Visible = false;
			mnuSeparator1.Visible         = false;
			mnuScheduleSettings.Visible   = false;

			if (connection != null)
			{
				// don't show active/deactive menu if it's not Online mode
				if (connection.IsLiveConnection)
				{
					if (nodeDef != null && !nodeDef.IsRoot)
					{
						bool updateEnabled = nodeDef.NodeActivated && nodeDef.NodeAvailable;
						bool dbExists      = string.IsNullOrEmpty(nodeDef.Group.Name);

						mnuUpdateHierarcially.Enabled = updateEnabled;
						mnuUpdateNode.Enabled         = updateEnabled;

						mnuNodeSettings.Visible       = true;
						mnuSettingsParameters.Visible = true;
						mnuSeparator1.Visible         = true;
						mnuNodeSettings.Enabled       = dbExists;

						// scheduling is not available for "dynamic" nodes
						mnuScheduleSettings.Visible =
							!nodeDef.TemplateNode.Template.NeedDataRowToBeInstantiated;
					}
				}
				else
				{
					mnuUpdateHierarcially.Enabled = false;
					mnuUpdateNode.Enabled         = false;

					Action updateMenuItems = () =>
					{
						bool hasMssqlQueries = HasMsSqlQueries(selectedNode, true);

						this.SafeInvoke(() =>
						{
							mnuUpdateHierarcially.Enabled &= !hasMssqlQueries;
							mnuUpdateNode.Enabled         &= !hasMssqlQueries;
						});
					};

					Task.Factory.StartNew(updateMenuItems).IgnoreExceptions();
				}
			}

			bool hasChildren = nodeDef != null && nodeDef.TemplateNode.Template.Childs.Any();
			bool isRoot      = nodeDef != null && nodeDef.IsRoot;

			mnuUpdateHierarcially.Visible     = hasChildren;
			mnuCollapseHierarchically.Visible = isExpandable;
			mnuExpandHierarchically.Visible   = isExpandable;
			mnuSeparator2.Visible             = isExpandable || hasChildren;

			mnuCloseConnection.Text = isRoot
				? GetCloseConnectionCommandText(this._model.LocaleManager)
				: string.Empty;

			mnuRefreshTree.Visible          = isRoot;
			mnuCloseConnection.Visible      = isRoot;
			mnuSeparator3.Visible           = isRoot;
			mnuConnectionProperties.Visible = isRoot
				&& menuEnabled
				&& connection.ConnectionGroup.IsDirectConnection;

			cmsTree.ResumeLayout(true);
		}

		private bool HasMsSqlQueries(TreeNode treeNode, bool hierarchically)
		{
			IEnumerable<ConcreteTemplateNodeDefinition> nodeDefinitions =
				GetConcreteTemplateNodeDefinitions(treeNode, !hierarchically);

			foreach (ConcreteTemplateNodeDefinition nodeDefinition in nodeDefinitions)
			{
				foreach (TemplateNodeQueryInfo queryInfo in nodeDefinition.TemplateNode.Queries)
				{
					List<QueryInfo> queries = this._model.GetQueryByTemplateNodeQueryInfo(queryInfo);

					return queries.Any(
						x => x.Source == QuerySource.MSSQL
						||
						x.Source == QuerySource.TDSQL
					);
				}
			}

			return false;
		}

		private void collapseHierarciallyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			treeTemplate.SelectedNode.Collapse(false);
		}

		private TreeNode CreateTreeViewNode(TemplateNodeInfo node)
		{
			ConcreteTemplateNodeDefinition nodeDef;
			TreeNode                       result = CreateTreeViewNode(node, out nodeDef);

			nodeDef.NodeAvailable = nodeDef.IsAvailableForDatabase(Program.Model) ?? true;

			if (!nodeDef.NodeAvailable)
			{
				SetNotAvailableNode(result);
			}
			else
			{
				SetNodeLoaded(result);
			}

			return result;
		}

		public TreeNode CreateTreeViewNode(TemplateNodeInfo node, out ConcreteTemplateNodeDefinition nodeDef)
		{
			Debug.Assert(node.IsInstance);

			ConnectionData connection = node.Connection;

			nodeDef = new ConcreteTemplateNodeDefinition(node, connection.ConnectionGroup);

			UserSettingsManager settingsManager = new UserSettingsManager(
				Program.Model.Settings.InterfaceLanguage
			);

			UserSettingsRow nodeSettings = settingsManager.LoadUserSettings(
				node.TemplateNodeId.GetValueOrDefault()
			);

			if (nodeSettings != null)
			{
				if (!string.IsNullOrEmpty(nodeSettings.NodeUIName))
				{
					node.Title = nodeSettings.NodeUIName;
				}

				node.UIcon      = nodeSettings.NodeUIIcon;
				node.FontColor  = nodeSettings.NodeFontColor;
				node.IsDisabled = !nodeSettings.NodeEnabled;
			}

			string iconName = GetIcon(nodeDef);

			TreeNode treeNode = new TreeNode
			{
				Tag              = nodeDef,
				Text             = nodeDef.FormatNodeText(),
				ImageKey         = iconName,
				SelectedImageKey = iconName,
			};

			if (node.FontColor != null)
			{
				Color color = Colors.FromString(node.FontColor);

				treeNode.ForeColor = color;
			}

			if (node.FontStyle != null)
			{
				treeNode.NodeFont = new Font(treeTemplate.Font, ParseFontStyle(node.FontStyle));
			}

			if (node.Template.Childs.Any())
			{
				if (treeNode.Nodes.Count == 0)
				{
					// to make it expandable
					treeNode.Nodes.Add(
						new TreeNode { Tag = _MakeExpandableNodeTag }
					);
				}
			}

			nodeDef.NodeActivated = !node.IsDisabled;

			SetNodeOnLoading(treeNode);

			return treeNode;
		}

		private void SetNodeOnLoading(TreeNode treeNode)
		{
			treeNode.ForeColor = Color.DarkGray;

			treeNode.ToolTipText = this._model.LocaleManager.GetLocalizedText(
				"common",
				"nodeLoading",
				this._model.Settings.InterfaceLanguage
			);

			var nodeDef = treeNode.Tag as ConcreteTemplateNodeDefinition;

			if (nodeDef != null)
			{
				nodeDef.NodeAvailable = false;
			}

			foreach (TreeNode childNode in treeNode.Nodes)
			{
				SetNodeOnLoading(childNode);
			}
		}

		internal void SetNodeLoaded(TreeNode treeNode)
		{
			Color nodeColor      = Color.Empty;
			treeNode.ToolTipText = String.Empty;

			ConcreteTemplateNodeDefinition nodeDef = treeNode.Tag as ConcreteTemplateNodeDefinition;

			if (nodeDef != null)
			{
				if (nodeDef.NodeActivated)
				{
					nodeDef.NodeAvailable = true;

					TemplateNodeInfo nodeInfo = nodeDef.TemplateNode;

					if (nodeInfo != null && !string.IsNullOrEmpty(nodeInfo.FontColor))
					{
						nodeColor = Colors.FromString(nodeInfo.FontColor);
					}
				}
			}

			treeNode.ForeColor = nodeColor;

			foreach (TreeNode childNode in treeNode.Nodes)
			{
				SetNodeLoaded(childNode);
			}
		}

		private void EnsureChildrenReady(TreeNode node)
		{
			// EXPERIMENTAL!
			if (node.Nodes.Count == 1 && node.Nodes[0].Tag == _MakeExpandableNodeTag)
			{
				TaskManager.EnsureChildrenReady(node);
			}
		}

		private void expandHierarciallyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			treeTemplate.SelectedNode.ExpandAll();
		}

		internal IEnumerable<ConcreteTemplateNodeDefinition> GetCurrentTree(out ConnectionData connectionData)
		{
			connectionData = GetSelectedConnection();
			var selectedConnectionData = connectionData;

			var selectedConnectionNode = treeTemplate.Nodes.OfType<TreeNode>().FirstOrDefault(
				n => (n.Tag is ConcreteTemplateNodeDefinition) &&
					((ConcreteTemplateNodeDefinition)n.Tag).IsRoot &&
					GetConnectionDataForTreeNode(n) == selectedConnectionData);

			if (selectedConnectionNode != null)
			{
				return GetConcreteTemplateNodeDefinitions(selectedConnectionNode);
			}

			return null;
		}

		private IEnumerable<ConcreteTemplateNodeDefinition> GetConcreteTemplateNodeDefinitions(
			TreeNode treeNode,
			bool     justFirst = false,
			bool     expandChilds = true
		)
		{
			var resultList = new List<ConcreteTemplateNodeDefinition>();

			if (treeNode != null)
			{
				var nodeDef = treeNode.Tag as ConcreteTemplateNodeDefinition;

				if (nodeDef != null)
				{
					resultList.Add(nodeDef);
				}

				if (!justFirst)
				{
					if (expandChilds)
					{
						EnsureChildrenReady(treeNode);
					}

					foreach (TreeNode subNode in treeNode.Nodes)
					{
						resultList.AddRange(GetConcreteTemplateNodeDefinitions(subNode, false, expandChilds));
					}
				}
			}

			return resultList;
		}

		private void mnuTreeContextCloseConnection_Click(object sender, EventArgs e)
		{
			// SettingsLoader.SaveTemplateToXml(this._model.FilesProvider.UserTemplateSettingsFileName, this._model.TemplateSettings);
			this._model.TemplateSettings.CreateDocumentoXML(this._model.FilesProvider.UserTemplateSettingsFileName);

			CloseConnection(GetSelectedConnection());
		}

		private void ModelSettingsChanged(object sender, SettingsChangedEventArgs e)
		{
			SetSettings(null);

			if (e.NewSetting.IsNeedReloadTree(e.OldSetting))
			{
				VisualizeData(CurrentConcreteTemplateNodeDefinition);
				//NewFillTemplateTreeView(false);
			}
		}

		private void OnTreeSelectionChanged(string templateName = null)
		{
			UpdateConnectionUiControls();

			ShowReport(treeTemplate.SelectedNode);

			if (ConntectionUpdated != null)
			{
				ConntectionUpdated(this, new ConntectionUpdatedArgs
					{
						ConnectionName = processWithLabel.Text,
						TemplateName   = templateName
					}
				);
			}
		}

		public class ConntectionUpdatedArgs : EventArgs
		{
			public string ConnectionName;
			public string TemplateName;
		}

		public event EventHandler<ConntectionUpdatedArgs> ConntectionUpdated;

		private void RefreshTreeToolStripMenuItemClick(object sender, EventArgs e)
		{
			foreach (var connection in Connections.Where(cnn => cnn.IsLiveConnection))
			{
				connection.ReloadTemplate();
			}

			var oldNodeTags = new List<string>();
			var treeNode    = treeTemplate.SelectedNode;

			while (treeNode != null)
			{
				oldNodeTags.Insert(0, treeNode.Text);
				treeNode = treeNode.Parent;
			}

			// OpenConnection(_connectionGroup);
			NewFillTemplateTreeView(false);

			var      nodes       = treeTemplate.Nodes;
			TreeNode currentNode = null;

			foreach (string tag in oldNodeTags)
			{
				foreach (TreeNode node in nodes)
				{
					if (node.Text == tag)
					{
						currentNode = node;
						nodes       = currentNode.Nodes;
						break;
					}
				}
			}

			if (currentNode != null)
			{
				treeTemplate.SelectedNode = currentNode;
			}
		}

		internal void SetNotAvailableNode(TreeNode treeNode)
		{
			treeNode.ForeColor   = Color.DarkGray;

			treeNode.ToolTipText = this._model.LocaleManager.GetLocalizedText(
				"common",
				"notAvailablenodeDescription",
				this._model.Settings.InterfaceLanguage
			);

			ConcreteTemplateNodeDefinition nodeDef = treeNode.Tag as ConcreteTemplateNodeDefinition;

			if (nodeDef != null)
			{
				nodeDef.NodeAvailable = false;
			}

			foreach (TreeNode childNode in treeNode.Nodes)
			{
				SetNotAvailableNode(childNode);
			}
		}

		private void SetSettings(bool? isDisableStatusLine)
		{
			if (isDisableStatusLine != null && isDisableStatusLine == true)
			{
				statusStrip.Visible = false;
			}
			else
			{
				statusStrip.Visible = this._model.Settings.ShowStatusPanel;
			}

			if (this._isDisableNavigationPanel)
			{
				this.TreeIsVisible = false;
			}
		}

		private void settingsNodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmUserSettingsNode userSetNode = new frmUserSettingsNode(
				this._model,
				treeTemplate.SelectedNode
			);

			userSetNode.ShowDialog();
		}

		private void settingsParametersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var nodes = GetConcreteTemplateNodeDefinitions(treeTemplate.SelectedNode, false, false);

			if (nodes == null)
			{
				return;
			}

			var concreteTemplateNodeDefinitions = nodes as ConcreteTemplateNodeDefinition[] ?? nodes.ToArray();

			if (!concreteTemplateNodeDefinitions.Any())
			{
				return;
			}

			var model = new QueryParameters(
				concreteTemplateNodeDefinitions.First(),
				treeTemplate.SelectedNode.Tag as ConcreteTemplateNodeDefinition,
				() => TaskManager.BeginRefreshTask(
					treeTemplate.SelectedNode,
					false,
					NodeUpdatingSource.ForcedFromServer
				)
			);

			var settingsParameters = new frmUserSettingsParameters(model);

			settingsParameters.ShowDialog();
		}

		private void settingScheduleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool                           hasActiveJobs  = false;
			TreeNode                       selectedNode   = treeTemplate.SelectedNode;
			ConcreteTemplateNodeDefinition nodeDefinition = GetConcreteTemplateNodeDefinitions(selectedNode, false, false).First();
			TemplateNodeInfo               nodeInfo       = nodeDefinition.TemplateNode;

			frmUserSettingSchedule sheduleSettingsForm = new frmUserSettingSchedule(nodeDefinition);

			if (sheduleSettingsForm.ShowDialog() == DialogResult.OK)
			{
				List<TemplateNodeUpdateJob> updateJobs = sheduleSettingsForm.GetUpdateJobs();
				foreach (TemplateNodeUpdateJob updateJob in updateJobs)
				{
					if (updateJob.Enabled)
					{
						hasActiveJobs = true;
					}
				}

				nodeInfo.HasActiveJobs = hasActiveJobs;

				SaveSchedulesAsync(nodeInfo, updateJobs)
					.ContinueOnCompleted(t => UpdateScheduledDate(nodeInfo));
			}
		}

		private Task SaveSchedulesAsync(
			TemplateNodeInfo            node,
			List<TemplateNodeUpdateJob> updateJobs)
		{
			tmSchedulerTimer.Stop();

			Action saveJobsAction = () =>
			{
				foreach (TemplateNodeUpdateJob updateJob in updateJobs)
				{
					node.AssignRefreshJob(updateJob);
					node.SaveJob(updateJob);
				}

				this._model.SaveSettings();
			};

			TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

			return Task.Factory.StartNew(saveJobsAction)
				.ContinueWith(t => tmSchedulerTimer.Start(), uiScheduler);
		}

		private class PreprocessorDataDescriptor
		{
			public PreprocessorAreaData AreaData { get; private set; }
			public PreprocessorData     Data     { get; private set; }

			private PreprocessorDataDescriptor()
			{
				this.AreaData = null;
				this.Data     = null;
			}

			public PreprocessorDataDescriptor(PreprocessorAreaData areaData) : this()
			{
				this.AreaData = areaData;
			}

			public PreprocessorDataDescriptor(PreprocessorAreaData areaData, PreprocessorData data) : this()
			{
				this.AreaData = areaData;
				this.Data     = data;
			}
		}

		private void InitializeTabs(ConcreteTemplateNodeDefinition definition)
		{
			int currentTabIndex = 0;
			tcBrowse.Visible    = definition != null;

			txtXml.Clear();

			bool showXmlTab      = Program.Model.Settings.ShowXML;
			bool isXmlTabVisible = tcBrowse.TabPages.Contains(tpXml);
			if (showXmlTab)
			{
				if (!isXmlTabVisible)
				{
					tcBrowse.TabPages.Insert(0, tpXml);
				}

				currentTabIndex = 1;
			}
			else
			{
				if (isXmlTabVisible)
				{
					tcBrowse.TabPages.Remove(tpXml);
				}
			}

			lock (tcBrowse)
			{
				// Dispose all tabs (except XML tab)
				for (int i = tcBrowse.TabPages.Count - 1; i >= currentTabIndex; i--)
				{
					TabPage tabPage = tcBrowse.TabPages[i];
					tcBrowse.TabPages.Remove(tabPage);
				}
			}
		}

		private void ShowReport(TreeNode treeNode)
		{
			ConcreteTemplateNodeDefinition definition   = GetConcreteTemplateNodeDefinitions(treeNode, true).FirstOrDefault();
			int                            lastTabIndex = tcBrowse.SelectedIndex;

			InitializeTabs(definition);

			if (definition == null)
			{
				return;
			}

			if (definition.NodeAvailable)
			{
				ShowReportAsync(definition, lastTabIndex);
			}
			else
			{
				ClearReportInfo();
			}
		}

		private GraphicsInfo GetGraphicsInfo()
		{
			Size  clientSize = (Size)Invoke((Func<Size>)GetHtmlReportClientSize);
			float dpiX       = 0.0F;
			float dpiY       = 0.0F;

			using (Graphics graphics = CreateGraphics())
			{
				dpiX = graphics.DpiX;
				dpiY = graphics.DpiY;
			}

			return new GraphicsInfo(clientSize, dpiX, dpiY);
		}

		private void ShowReportAsync(ConcreteTemplateNodeDefinition definition, int tabSelectIndex)
		{
			if (this._reportViewCanceler != null)
			{
				this._reportViewCanceler.Cancel();
			}

			this._reportViewCanceler = new CancellationTokenSource();

			CancellationToken reportViewToken = this._reportViewCanceler.Token;

			GraphicsInfo graphicsInfo = GetGraphicsInfo();

			Func<NodeDataProvider> getQueryResultFunc =
				() => new NodeDataProvider(this._model, definition);

			// log1.DebugFormat("getQueryResultFunc:{0}",
			//     getQueryResultFunc
			// );

			Task<NodeDataProvider> queryReportTask = Task.Factory.StartNew(getQueryResultFunc);

			// log1.DebugFormat("queryReportTask:{0}",
			//     queryReportTask
			// );

			// If task succeeded.
			queryReportTask
				.ContinueWith(t =>
					{
						NodeDataProvider resultDataProvider = t.Result;

						// log1.DebugFormat("queriesResult:{0}",
						// 	queriesResult
						// );

						// log1.DebugFormat("(1067):definition:{0};definition.Connection:{1};queriesResult:{2}",
						// 	definition,
						// 	definition.Connection,
						// 	queriesResult
						// );

						reportViewToken.ThrowIfCancellationRequested();

						VisualizeData visualizeData = this._model.VisualizeProcessor.GetVisualizeData(
							resultDataProvider,
							graphicsInfo
						);

						reportViewToken.ThrowIfCancellationRequested();

						Action showReportAction = () =>
						{
							try
							{
								// log1.DebugFormat("(1082)visualizeData:{0}",
								// 	visualizeData
								// );

								reportViewToken.ThrowIfCancellationRequested();

								if (visualizeData != null)
								{
									RenderVisualizeData(definition, visualizeData, tabSelectIndex);
								}
								else
								{
									ClearReportInfo();
								}
							}
							catch (OperationCanceledException exc)
							{
								// operation has been cancelled
								log1.ErrorFormat("Show report operation cancelled. Exception:{0}",
									exc
								);
							}
						};

						this.SafeInvoke(showReportAction);
					},
					reportViewToken,
					TaskContinuationOptions.OnlyOnRanToCompletion,
					TaskScheduler.Current
				)
				.ContinueOnException(HandleTaskFailure);

			// If task failed.
			queryReportTask.ContinueOnException(t => HandleTaskFailure(t));
		}

		private void HandleTaskFailure(Task task)
		{
			if (task != null)
			{
				if (task.Exception != null)
				{
					log1.ErrorFormat("Task execution failed. Details:{0}",
						task.Exception
					);
				}
			}
		}

		private void ClearReportInfo()
		{
			txtXml.Text      = string.Empty;
			txtXml.ReadOnly  = true;
			txtXml.BackColor = Color.DarkGray;
		}

		private void RenderVisualizeData(ConcreteTemplateNodeDefinition definition, VisualizeData visualizeData, int selectTabIndex)
		{
			string additionalXml = string.Empty;
			int    tabIndex      = tcBrowse.TabCount - 1;

			TemplateNodeInfo nodeInfo = definition.TemplateNode;
			tcBrowse.ShowSingleTab    = !nodeInfo.IsHideTabs;

			if (visualizeData.PreprocessorAreas != null)
			{
				List<PreprocessorDataDescriptor> dialogObjects =
					visualizeData.PreprocessorAreas.SelectMany(
						a => a.NoSplitter || a.Preprocessors.Count == 0 || (a.Columns.Length == 1 && a.Rows.Length == 1)
							? a.Preprocessors.SelectMany(
								data => new[]
								{
									new PreprocessorDataDescriptor(a, data)
								}
							)
							: new[]
							{
								new PreprocessorDataDescriptor(a)
							}
					).ToList();

				// create tab pages with empty content first
				// to avoid flickering and extra reports loading
				List<TabPage> newPages    = new List<TabPage>();
				int           bufferIndex = 0;

				foreach (PreprocessorDataDescriptor dialogObj in dialogObjects)
				{
					TabPage page = this._pageBuffer.GetPage(bufferIndex++);
					PreprocessorAreaData dialogArea = dialogObj.AreaData;
					page.Text = dialogArea != null
						? dialogArea.Name
						: string.Empty;

					page.DisposeChildControls();
					newPages.Add(page);
					tcBrowse.TabPages.Add(page);
					tabIndex++;

					if (selectTabIndex == tabIndex)
					{
						tcBrowse.SelectedIndex = selectTabIndex;
					}
				}

				// release extra pages in buffer
				this._pageBuffer.Resize(bufferIndex);

				// fill tab pages with report controls
				IEnumerator<TabPage> pageEnum = newPages.GetEnumerator();

				foreach (PreprocessorDataDescriptor dialogObj in dialogObjects)
				{
					pageEnum.MoveNext();
					TabPage page = pageEnum.Current;

					PreprocessorAreaData dialogArea = dialogObj.AreaData;

					if (dialogArea != null)
					{
						ConnectionTabArea ctrl = new ConnectionTabArea
						{
							Dock = DockStyle.Fill
						};

						ctrl.Init(this, dialogArea, definition, this._model);

						page.Controls.Add(ctrl);
					}
					else
					{
						PreprocessorData dialogPreproc = dialogObj.Data;

						if (dialogPreproc != null)
						{
							Control control = dialogPreproc.ContentFactory.CreateControl();

							if (control is WebBrowser || control is ReportViewrControl)
							{
								control.PreviewKeyDown += (s, e) =>
								{
									if (e.KeyCode == Keys.F5)
									{
										F5RefreshView();
									}
								};

								if (control is ReportViewrControl && tcBrowse.Visible)
								{
									if ((control as ReportViewrControl).ReportXML != string.Empty)
									{
										using (MemoryStream xmlWriterStream = new MemoryStream())
										{
											XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
											{
												CheckCharacters     = false,
												ConformanceLevel    = ConformanceLevel.Auto,
												Encoding            = Encoding.UTF8,
												Indent              = true,
												IndentChars         = " ",
												NewLineChars        = Environment.NewLine,
												NewLineHandling     = NewLineHandling.Replace,
												NewLineOnAttributes = false,
												OmitXmlDeclaration  = false
											};

											using (var xmlWriter = XmlWriter.Create(xmlWriterStream, xmlWriterSettings))
											{
												XmlDocument doc = new XmlDocument();
												doc.LoadXml((control as ReportViewrControl).ReportXML);
												doc.Save(xmlWriter);
												xmlWriter.Flush();
											}

											additionalXml = Encoding.UTF8.GetString(xmlWriterStream.ToArray());
										}
									}
								}
							}

							if (dialogPreproc.VerticalTextAlign != null)
							{
								TitleFrame titleFrame = new TitleFrame(
									dialogPreproc.VerticalTextAlign,
									dialogPreproc.TextAlign,
									control
								);

								titleFrame.Title = dialogPreproc.Name;

								control = titleFrame;
							}

							control.Dock = DockStyle.Fill;
							page.Controls.Add(control);
						}
					}
				}
			}

			txtXml.Text = visualizeData.SourceXml + Environment.NewLine + additionalXml;

			txtXml.SelectionStart  = 0;
			txtXml.SelectionLength = 0;
			txtXml.ReadOnly        = true;
			txtXml.BackColor       = Color.White;

			lblLastRefreshDate.Text = !visualizeData.NodeLastUpdated.HasValue
				? ComposeBlankDateTimeString()
				: visualizeData.NodeLastUpdated.Value.ToString();

			lblSpendTime.Text = visualizeData.NodeLastUpdateDuration.HasValue
				? String.Format(
					"   {0:00}:{1:00}:{2:00}",
					visualizeData.NodeLastUpdateDuration.Value.Hour,
					visualizeData.NodeLastUpdateDuration.Value.Minute,
					visualizeData.NodeLastUpdateDuration.Value.Second
				)
				: String.Format(
					"   {0:00}:{1:00}:{2:00}",
					0,
					0,
					0
				);

			UpdateScheduledDate(nodeInfo);
		}

		private void UpdateScheduledDate(TemplateNodeInfo nodeInfo)
		{
			string sNextUpdateDateTime = ComposeBlankDateTimeString();

			if (nodeInfo != null)
			{
				IEnumerable<TemplateNodeUpdateJob> updateJobs = nodeInfo.GetRefreshJob(true)
					.Where(job => job != null && !job.IsEmpty());

				DateTime? nearestUpdateDate = null;

				foreach (TemplateNodeUpdateJob updateJob in updateJobs)
				{
					DateTime nextDate;

					if (updateJob.Settings.GetNextTime(DateTime.Now, out nextDate))
					{
						if (nearestUpdateDate != null)
						{
							if (nextDate.CompareTo(nearestUpdateDate) < 0)
							{
								nearestUpdateDate = nextDate;
							}
						}
						else
						{
							nearestUpdateDate = nextDate;
						}
					}
				}

				if (nearestUpdateDate != null)
				{
					sNextUpdateDateTime = nearestUpdateDate.Value.ToString();
				}
			}

			this.SafeInvoke(() =>
			{
				lbNextUpdateDateTime.Text = sNextUpdateDateTime;
			});
		}

		private void treeTemplate_AfterSelect(object sender, TreeViewEventArgs e)
		{
			ConnectionData connection   = Connections.FirstOrDefault();
			string         templateName = null;

			if (connection != null)
			{
				var localeitem = connection.RootInstance.GetLocale(false);

				if (localeitem != null)
				{
					templateName = localeitem.Text;
				}
			}

			OnTreeSelectionChanged(templateName);
		}

		private void treeTemplate_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			treeTemplate.SelectedNode = e.Node;

			treeTemplate.MouseClick += (s, ev) =>
			{
				if (ev.Button == MouseButtons.Right)
				{
					treeTemplate.ContextMenuStrip = treeTemplate.ContextMenuStrip ?? cmsTree;
				}
			};
		}

		private void UpdateConnectionUiControls()
		{
			string connectionName = String.Empty;
			var    cnn            = GetSelectedConnection();

			if (cnn != null)
			{
				// otherwise long names does not fit into the status section (later they are trimmed when string is drawing)
				connectionName = cnn.SourceConnectionName + string.Concat(Enumerable.Repeat(" ", cnn.SourceConnectionName.Length / 10));

				if (cnn.ConnectionGroup != null && cnn.ConnectionGroup.Connections != null)
				{
					lblInstancesCount.Text = cnn.ConnectionGroup.Connections.Count(
						item => item.IsEnabled
					).ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					lblInstancesCount.Text = "--";
				}

				lblUserName.Text = cnn.UserNameAsDisplayed;
			}
			else
			{
				connectionName         = String.Empty;
				lblInstancesCount.Text = "--";
				lblUserName.Text       = String.Empty;
			}

			processWithLabel.Text = connectionName;
		}

		public void VisualizeData(ConcreteTemplateNodeDefinition changedNode)
		{
			ConcreteTemplateNodeDefinition definition   = null;
			int                            lastTabIndex = -1;

			Action action = () =>
			{
				ConcreteTemplateNodeDefinition activeNode = CurrentConcreteTemplateNodeDefinition;

				if (activeNode == null)
				{
					return;
				}

				if (changedNode.Equals(activeNode))
				{
					definition   = GetConcreteTemplateNodeDefinitions(treeTemplate.SelectedNode, true, false).FirstOrDefault();
					lastTabIndex = tcBrowse.SelectedIndex;

					InitializeTabs(definition);
				}
			};

			this.SafeInvoke(action);

			if (definition == null)
			{
				return;
			}

			if (definition.NodeAvailable)
			{
				ShowReportAsync(definition, lastTabIndex);
			}
			else
			{
				this.SafeInvoke(ClearReportInfo);
			}
		}

		#endregion Methods

		#region Running Processes Form

		private void ShowRunningProcessesForm()
		{
			if (this._runningTaskInfoForm != null)
			{
				if (this._runningTaskInfoForm.IsDisposed)
				{
					this._runningTaskInfoForm = null;
				}
			}

			if (this._runningTaskInfoForm != null)
			{
				if(DateTime.Now - this._runningProcessFormClosedTime > new TimeSpan(0, 0, 0, 0, 200))
				{
					this._runningTaskInfoForm.Visible = true;
				}
			}
			else
			{
				this._runningTaskInfoForm = new RunningTaskInfoForm(TaskManager.ProgressManager, this._model);

				this._runningTaskInfoForm.VisibleChanged += (s, a) =>
				{
					if (!this._runningTaskInfoForm.Visible)
					{
						this._runningProcessFormClosedTime = DateTime.Now;
					}
				};

				var pt = statusStrip.PointToScreen(processWithLabel.Bounds.Location);

				this._runningTaskInfoForm.Show(this, pt);
			}
		}

		private void progressDetailsButton_Click(object sender, EventArgs e)
		{
			ShowRunningProcessesForm();
		}

		#endregion

		public bool TreeIsVisible
		{
			get { return treeTemplate.Visible; }

			set
			{
				if (value)
				{
					treeTemplate.Show();
				}
				else
				{
					treeTemplate.Hide();
				}
			}
		}

		public bool HideStatusPanel
		{
			get
			{
				return !statusStrip.Visible;
			}

			set
			{
				SetSettings(value);
			}
		}

		private void splitter1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && !this.TreeIsVisible)
			{
				this.TreeIsVisible = true;
			}
		}

		private void treeTemplate_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Tag == _MakeExpandableNodeTag)
			{
				TaskManager.BeginRefreshTask(
					e.Node,
					false,
					NodeUpdatingSource.FromServerIfNotSavedLocally,
					ContinueWith
				);
			}
		}

		private void treeTemplate_AfterExpand(object sender, TreeViewEventArgs e)
		{
			/*
			if (ContinueWith != null)
			{
				ContinueWith();
				ContinueWith = null;
			}*/
		}

		private void processWithLabel_Click(object sender, EventArgs e)
		{
			ShowRunningProcessesForm();
		}

		private void mnuConnectionProperties_Click(object sender, EventArgs e)
		{
			ConnectionData             selectedConnection         = GetSelectedConnection();
			EditDirectConnectionDialog editDirectConnectionDialog = new EditDirectConnectionDialog(
				this._model,
				selectedConnection.ConnectionGroup
			);

			if (editDirectConnectionDialog.ShowDialog() == DialogResult.OK)
			{
				treeTemplate.SelectedNode.Collapse(false);

				TaskManager.BeginRefreshTask(
					treeTemplate.SelectedNode,
					false,
					NodeUpdatingSource.ForcedFromServer
				);
			}
		}
	}
}
