using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.Managers
{
	public class NodeCounter
	{
		private readonly MsSqlAuditorModel      _model;
		private readonly object                 _lockObj;
		private readonly Dictionary<long, int?> _nodeCountMap;

		public NodeCounter(MsSqlAuditorModel model)
		{
			this._model        = model;
			this._lockObj      = new object();
			this._nodeCountMap = new Dictionary<long, int?>();
		}

		public Task<int?> StartUpdateTask(TreeNode treeNode, NodeUpdatingSource mode)
		{
			switch (mode)
			{
				case NodeUpdatingSource.ForcedFromServer:
					return Task.Factory.StartNew(
						t => UpdateNodeCounterValue((TreeNode)t),
						treeNode
					);

				default:
					return Task.Factory.StartNew(
						t => LoadNodeCounterValue((TreeNode)t),
						treeNode
					);
			}
		}

		private int? UpdateNodeCounterValue(TreeNode treeNode)
		{
			int? count = null;

			if (treeNode == null)
			{
				return null;
			}

			ConcreteTemplateNodeDefinition nodeDefinition = treeNode.Tag as ConcreteTemplateNodeDefinition;

			if (nodeDefinition == null)
			{
				return null;
			}

			if (treeNode.Nodes.Count == 0)
			{
				count = this._model.VisualizeProcessor.GetDataRowCount(nodeDefinition, nodeDefinition.Connection);

				if (nodeDefinition.IsDataseDetail && count == 0 && nodeDefinition.TemplateNode.HideEmptyResultDatabases)
				{
					count = null;
				}
				else
				{
					count = nodeDefinition.TemplateNode.Childs.Any() && count == 0 ? null : count;
				}
			}
			else
			{
				count = treeNode.Nodes
					.Cast<TreeNode>()
					.Select(LoadNodeCounterValue)
					.Where(add => add.HasValue && add != 0)
					.Aggregate<int?, int?>(0, (current, add) => current + add);

				if (count == 0)
				{
					count = null;
				}
			}

			long handle = nodeDefinition.ComputeHandle();

			lock (this._lockObj)
			{
				if (this._nodeCountMap.ContainsKey(handle))
				{
					int? currentCount = this._nodeCountMap[handle];

					if (Nullable.Equals(currentCount, count) || count == null)
					{
						return currentCount;
					}

					this._nodeCountMap[handle] = count;
				}
				else
				{
					this._nodeCountMap.Add(handle, count);
				}
			}

			CurrentStorage storage = GetStorage(nodeDefinition.TemplateNode.ConnectionGroup);
			storage.UpdateTreeNodeCounterValue(nodeDefinition.TemplateNode, count);

			return count;
		}

		private int? LoadNodeCounterValue(TreeNode treeNode)
		{
			if (treeNode == null)
			{
				return null;
			}

			ConcreteTemplateNodeDefinition nodeDefinition = treeNode.Tag as ConcreteTemplateNodeDefinition;

			if (nodeDefinition == null)
			{
				return null;
			}

			long handle = nodeDefinition.ComputeHandle();

			lock (this._lockObj)
			{
				if (this._nodeCountMap.ContainsKey(handle))
				{
					return this._nodeCountMap[handle];
				}
			}

			TemplateNodeInfo templateNode = nodeDefinition.TemplateNode;

			CurrentStorage storage = GetStorage(templateNode.ConnectionGroup);
			int?           count   = storage.GetTreeNodeCounterValue(templateNode);

			lock (this._lockObj)
			{
				if (this._nodeCountMap.ContainsKey(handle))
				{
					this._nodeCountMap[handle] = count;
				}
				else
				{
					this._nodeCountMap.Add(handle, count);
				}
			}

			return count;
		}

		private CurrentStorage GetStorage(ConnectionGroupInfo connectionGroup)
		{
			return this._model.GetVaultProcessor(connectionGroup).CurrentStorage;
		}
	}
}
