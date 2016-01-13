using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Scheduling;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.SQLite.Tables.UserSettings;

namespace MSSQLServerAuditor.SQLite.Tables
{
	public class NodeInstanceTable : CurrentStorageTable
	{
		private static readonly ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public const string TableName                 = "d_NodeInstance";
		public const string TableIdentityField        = "d_NodeInstance_id";
		public const string ParentIdFn                = "d_NodeInstanceParent_Id";
		public const string NodeUIdFn                 = "NodeUId";
		public const string NodeUNameFn               = "NodeUName";
		public const string NodeUIconFn               = "NodeUIcon";
		public const string NodeEnabledFn             = "NodeEnabled";
		public const string NodeFontColorFn           = "NodeFontColor";
		public const string NodeFontStyleFn           = "NodeFontStyle";
		public const string ChildrenNotYetProcessedFn = "ChildrenNotYetProcessed";
		public const string NodeSequenceNumberFn      = "NodeSequenceNumber";
		public const string NodeScheduledUpdateFn     = "NodeScheduledUpdate";
		public const string NodeCounterValue          = "NodeCounterValue";
		public const string NodeLastUpdated           = "NodeLastUpdated";
		public const string NodeLastUpdateDuration    = "NodeLastUpdateDuration";

		public static readonly string ConnectionIdFn = ConnectionGroupDirectory.TableName.AsFk();
		public static readonly string TemplateNodeIdFn = TemplateNodeDirectory.TableName.AsFk();

		private readonly CurrentStorage _storage;

		public NodeInstanceTable(CurrentStorage storage)
			: base(storage, CreateTableDefinition())
		{
			this._storage = storage;
		}

		public static TableDefinition CreateTableDefinition()
		{
			TableDefinition nodeInstanceTableDefinition = TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddBigIntField(ConnectionIdFn,           true,  false)
				.AddBigIntField(TemplateNodeIdFn,         true,  false)
				.AddBigIntField(ParentIdFn,               true,  false)
				.AddNVarCharField(NodeUIdFn,              true,  false)
				.AddNVarCharField(NodeUNameFn,            false, false)
				.AddNVarCharField(NodeUIconFn,            false, false)
				.AddBitField(NodeEnabledFn,               false, false, SqLiteBool.ToBit(true))
				.AddNVarCharField(NodeFontColorFn,        false, false)
				.AddNVarCharField(NodeFontStyleFn,        false, false)
				.AddBigIntField(NodeSequenceNumberFn,     true,  false)
				.AddBitField(ChildrenNotYetProcessedFn,   false, false, SqLiteBool.ToBit(true))
				.AddDateTimeField(NodeScheduledUpdateFn,  false, false)
				.AddIntField(NodeCounterValue,            false, false)
				.AddDateTimeField(NodeLastUpdated,        false, false)
				.AddDateTimeField(NodeLastUpdateDuration, false, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);

			nodeInstanceTableDefinition.Indexes.Add(
				new IndexDefinition(
					nodeInstanceTableDefinition,
					"Parent_idx",
					false,
					ParentIdFn
				)
			);

			return nodeInstanceTableDefinition;
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		private bool? GetChildrenNotYetProcessed(Int64 nodeInstanceId)
		{
			ITableRow row = this.GetRowByIdentity(nodeInstanceId);

			return row != null
				? (bool)row.Values[ChildrenNotYetProcessedFn]
				: (bool?)null;
		}

		private void SetChildrenNotProcessedYet(TemplateNodeInfo node, bool value)
		{
			Debug.Assert(node.IsInstance);
			Debug.Assert(node.TemplateNodeId != null);

			if (node.TemplateNodeId == null)
			{
				this.Save(node);
			}

			if (node.TemplateNodeId == null)
			{
				throw new InvalidOperationException(node + " is not saved (has no PrimaryKey)");
			}

			using (this.Connection.OpenWrapper())
			{
				String query = string.Format(
					"UPDATE [{0}] SET [{1}] = {2} WHERE [{3}] = {4} AND ([{1}] IS NULL OR [{1}] != {2});",
					TableName,
					ChildrenNotYetProcessedFn,
					SqLiteBool.ToBit(value),
					IdentityField,
					node.TemplateNodeId.Value
				);

				new SqlCustomCommand(this.Connection, query).Execute(1);
			}
		}

		public void SaveScheduledUpdateTime(TemplateNodeInfo node, DateTime value)
		{
			Debug.Assert(node.IsInstance);
			Debug.Assert(node.TemplateNodeId != null);

			if (node.TemplateNodeId == null)
			{
				this.Save(node);
			}

			if (node.TemplateNodeId == null)
			{
				throw new InvalidOperationException(node + " is not saved (has no PrimaryKey)");
			}

			using (this.Connection.OpenWrapper())
			{
				string query = string.Format(
					"UPDATE [{0}] SET [{1}] = @dateTimeValue WHERE [{2}] = {3} AND ([{1}] IS NULL OR [{1}] != @dateTimeValue);",
					TableName,
					NodeScheduledUpdateFn,
					IdentityField,
					node.TemplateNodeId.Value
				);

				new SqlCustomCommand(this.Connection, query, new SQLiteParameter("@dateTimeValue", value))
					.Execute(0);
			}
		}

		public void DisableNode(Int64 nodeId)
		{
			using (this.Connection.OpenWrapper())
			{
				String query = string.Format(
					"UPDATE [{0}] SET [{1}] = {2} WHERE [{3}] = {4} AND ([{1}] IS NULL OR [{1}] != {2});",
					TableName,
					NodeEnabledFn,
					SqLiteBool.ToBit(false),
					IdentityField,
					nodeId
				);

				new SqlCustomCommand(this.Connection, query)
					.Execute(100);
			}
		}

		public void DisableChildren(TemplateNodeInfo node)
		{
			Debug.Assert(node.IsInstance);

			if (node.TemplateNodeId == null)
			{
				return;
			}

			using (this.Connection.OpenWrapper())
			{
				String query = string.Format(
					"UPDATE [{0}] SET [{1}] = {2} WHERE {3} = {4} AND ([{1}] IS NULL OR [{1}] != {2});",
					TableName,
					NodeEnabledFn,
					SqLiteBool.ToBit(false),
					ParentIdFn,
					node.TemplateNodeId.Value
				);

				new SqlCustomCommand(this.Connection, query)
					.Execute(100);
			}
		}

		public bool TryLoadRoodId(TemplateNodeInfo node, Int64? connectionGroupId, Int64? templatedId)
		{
			String sql = string.Format(
				"SELECT ni.[{8}] FROM [{0}] ni " +
				"JOIN [{4}] tn ON ni.[{5}] = tn.[{8}] " +
				"WHERE ni.[{1}] IS NULL AND ni.[{2}] = {3} " +
				"AND tn.[{6}] = {7}",
				TableName,
				ParentIdFn,
				ConnectionIdFn,
				connectionGroupId,
				TemplateNodeDirectory.TableName,
				TemplateNodeIdFn,
				TemplateNodeDirectory.TemplateIdFn,
				templatedId,
				TemplateNodeDirectory.TableIdentityField
			);

			using (this.Connection.OpenWrapper())
			{
				bool result = false;

				new SqlSelectCommand(
					this.Connection,
					sql,
					reader =>
					{
						node.AssignTemplateId((Int64)reader[IdentityField]);
						result = true;
					})
					.Execute(100);

				if (result)
				{
					return true;
				}
			}

			return false;
		}

		public bool TryLoadChildren(TemplateNodeInfo node)
		{
			if (node.TemplateNodeId == null)
			{
				return false;
			}

			bool? notProcessedYet = this.GetChildrenNotYetProcessed(node.TemplateNodeId.Value);

			if (notProcessedYet ?? true)
			{
				return false;
			}

			String sql = string.Format(
				"SELECT I.{0}, I.*, PQ.{1}, T.{9} AS [TemplateUserId] " +
				"FROM {2} AS I " +
				"LEFT JOIN {3} AS T ON I.{7} = T.{13} " +
				"LEFT JOIN {4} AS PQ ON T.{8} = PQ.{14} " +
				"WHERE I.{5} = {6} AND I.{10} = {11} " +
				"ORDER BY I.{12}",
				IdentityField,
				TemplateNodeQueryGroupDirectory.DefaultDatabaseFieldFn,
				TableName,
				TemplateNodeDirectory.TableName,
				TemplateNodeQueryGroupDirectory.TableName,
				ParentIdFn,
				node.TemplateNodeId,
				TemplateNodeIdFn,
				TemplateNodeDirectory.ParentQueryGroupIdFn,
				TemplateNodeDirectory.UserIdFieldName,
				NodeEnabledFn,
				SqLiteBool.ToBit(true),
				NodeSequenceNumberFn,
				TemplateNodeDirectory.TableIdentityField,
				TemplateNodeQueryGroupDirectory.TableIdentityField
			);

			node.Childs.Clear();

			using (this.Connection.OpenWrapper())
			{
				new SqlSelectCommand(
					this.Connection,
					sql,
					reader =>
					{
						string templateUserId = reader["TemplateUserId"].ToString();

						TemplateNodeInfo template = node.Template.Childs.FirstOrDefault(
							n => n.Id == templateUserId.ToString()
						);

						if (template == null)
						{
							this.DisableNode((Int64)reader[IdentityField]);
							return;
						}

						string defaultDbFieldnName =
							reader[TemplateNodeQueryGroupDirectory.DefaultDatabaseFieldFn].ToString();

						TemplateNodeInfo newNode = template.Instantiate(node.Connection, defaultDbFieldnName, node);

						long newNodeId = (Int64)reader[IdentityField];

						newNode.AssignTemplateId(newNodeId);

						List<TemplateNodeUpdateJob> refreshJobs = newNode.GetRefreshJob(true);

						foreach (TemplateNodeUpdateJob refreshJob in refreshJobs)
						{
							if (refreshJob != null && !refreshJob.IsEmpty())
							{
								if (!(reader[NodeScheduledUpdateFn] is DBNull))
								{
									refreshJob.LastRan = (DateTime)reader[NodeScheduledUpdateFn];
								}
							}
						}

						this._storage.NodeInstanceAttributes.ReadTo(
							newNodeId,
							newNode.Attributes,
							this.Connection
						);

						newNode.OnAttributesChanged();

						node.Childs.Add(newNode);
					})
				.Execute(100);
			}

			Dictionary<string, TemplateNodeInfo> absent = new Dictionary<string, TemplateNodeInfo>();

			foreach (TemplateNodeInfo nodeTemplate in node.Template.Childs.Where(n => !n.NeedDataRowToBeInstantiated))
			{
				absent[nodeTemplate.Id] = nodeTemplate;
			}

			foreach (TemplateNodeInfo loaded in node.Childs)
			{
				absent.Remove(loaded.Template.Id);
			}

			if (absent.Any())
			{
				Dictionary<TemplateNodeInfo, int> templateIndexes = new Dictionary<TemplateNodeInfo, int>();

				for (int i = 0; i < node.Template.Childs.Count; i++)
				{
					templateIndexes[node.Template.Childs[i]] = i;
				}

				List<TemplateNodeInfo> newStatics = absent.Values.Select(t => t.Instantiate(node.Connection, null, node)).ToList();

				IOrderedEnumerable<TemplateNodeInfo> newList =
					new List<TemplateNodeInfo>(node.Childs).Union(newStatics).OrderBy(n => templateIndexes[n.Template]);

				node.Childs.Clear();

				node.Childs.AddRange(newList);

				this.SaveChildren(node);
			}

			return true;
		}

		public void SaveChildren(TemplateNodeInfo node)
		{
			List<TemplateNodeInfo> children = node.Childs;

			Debug.Assert(children.All(n => n.IsInstance));
			Debug.Assert(children.All(n => n.Parent == node));

			if (children.Count == 0)
			{
				return;
			}

			this.DisableChildren(node);

			this.Save(children);

			this.SetChildrenNotProcessedYet(node, false);
		}

		public void Save(TemplateNodeInfo node)
		{
			this.Save(new[] { node });
		}

		private void Save(IEnumerable<TemplateNodeInfo> node)
		{
			IList<TemplateNodeInfo> nodeList = node as IList<TemplateNodeInfo> ?? node.ToList();

			Debug.Assert(nodeList.All(n => n.IsInstance));

			foreach (var n in nodeList)
			{
				ITableRow row;
				List<ITableRow> attributesRows;

				this.ComposeTableRow(n, out row, out attributesRows);

				if (n.TemplateNodeId == null)
				{
					Int64? id = this.InsertOrUpdateRowForSure(row);
					n.AssignTemplateId(id);
				}

				foreach (ITableRow attr in attributesRows)
				{
					attr.Values.Add(NodeInstanceAttributeTable.NodeInstaceIdFn, n.TemplateNodeId);

					this._storage.NodeInstanceAttributes.InsertOrUpdateRowForSure(attr);
				}
			}
		}

		public void SaveTree(TemplateNodeInfo node)
		{
			this.Save(node);

			foreach (TemplateNodeInfo ch in node.Childs)
			{
				this.SaveTree(ch);
			}

			this.SetChildrenNotProcessedYet(node, false);
		}

		public int? GetTreeNodeCounterValue(TemplateNodeInfo node)
		{
			Debug.Assert(node.IsInstance);

			if (node.TemplateNodeId == null)
			{
				return null;
			}

			using (this.Connection.OpenWrapper())
			{
				String query = string.Format(
					"SELECT [{0}] FROM [{1}] WHERE [{2}] = {3}",
					NodeCounterValue,
					TableName,
					IdentityField,
					node.TemplateNodeId.Value
				);

				using (SQLiteCommand cmd = new SQLiteCommand(query, this.Connection))
				{
					using (SQLiteDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							string counterVal = reader[NodeCounterValue].ToString();

							if (string.IsNullOrEmpty(counterVal))
							{
								return null;
							}

							return int.Parse(counterVal);
						}
					}
				}
			}

			return null;
		}

		public void UpdateTreeNodeCounterValue(TemplateNodeInfo node, int? counterValue)
		{
			Debug.Assert(node.IsInstance);

			if (node.TemplateNodeId == null)
			{
				this.Save(node);
			}

			if (node.TemplateNodeId == null)
			{
				return;
			}

			using (this.Connection.OpenWrapper())
			{
				const string paramName = "@intCounterValue";

				string update = string.Format("UPDATE [{0}] SET [{1}] = {2}", TableName, NodeCounterValue, paramName);
				string whereClause = string.Format("[{0}] = {1}", IdentityField, node.TemplateNodeId.Value);

				SQLiteParameter parameter;

				if (counterValue != null)
				{
					whereClause += string.Format(" AND ([{0}] IS NULL OR [{0}] != {1})", NodeCounterValue, paramName);
					parameter = new SQLiteParameter(paramName, counterValue.Value);
				}
				else
				{
					whereClause += string.Format(" AND [{0}] IS NOT NULL", NodeCounterValue);
					parameter = new SQLiteParameter(paramName, DBNull.Value);
				}

				string query = string.Format(
					"{0} WHERE {1};",
					update,
					whereClause
				);

				new SqlCustomCommand(this.Connection, query, parameter)
					.Execute(100);
			}
		}

		public void UpdateTreeNodeLastUpdateAndDuration(
			TemplateNodeInfo node,
			DateTime?        lastUpdateDateTime,
			DateTime?        lastUpdateDuration
		)
		{
			Debug.Assert(node.IsInstance);

			if (node.TemplateNodeId != null)
			{
				using (this.Connection.OpenWrapper())
				{
					String query = string.Format(
						  "UPDATE [{0}] SET"
						+ "     [{1}] = @NodeLastUpdated"
						+ "    ,[{2}] = @NodeLastUpdateDuration"
						+ " WHERE"
						+ "    [{3}] = @{3}"
						+ "    AND ("
						+ "       ([{1}] IS NULL OR [{1}] != @NodeLastUpdated)"
						+ "       OR"
						+ "       ([{2}] IS NULL OR [{2}] != @NodeLastUpdateDuration)"
						+ "    );",
						TableName,
						NodeLastUpdated,
						NodeLastUpdateDuration,
						IdentityField
					);

					SQLiteParameter[] sqliteParameters = {
						new SQLiteParameter("@NodeLastUpdated",        lastUpdateDateTime),
						new SQLiteParameter("@NodeLastUpdateDuration", lastUpdateDuration),
						new SQLiteParameter("@" + IdentityField,       node.TemplateNodeId.Value)
					};

					new SqlCustomCommand(this.Connection, query, sqliteParameters)
						.Execute(0);
				}
			}
		}

		public Tuple<DateTime?, DateTime?> GetTreeNodeLastUpdateAndDuration(TemplateNodeInfo node)
		{
			DateTime? lastUpdate         = null;
			DateTime? lastUpdateDuration = null;

			Debug.Assert(node.IsInstance);

			if (node.TemplateNodeId == null)
			{
				return new Tuple<DateTime?, DateTime?>(null, null);
			}

			using (this.Connection.OpenWrapper())
			{
				String query = string.Format(
					"SELECT {0}, {1} FROM [{2}] WHERE {3} = {4}",
					NodeLastUpdated,
					NodeLastUpdateDuration,
					TableName,
					IdentityField,
					node.TemplateNodeId.Value
				);

				using (SQLiteCommand cmd = new SQLiteCommand(query, this.Connection))
				{
					using (SQLiteDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							int ordinalDateUpdate         = reader.GetOrdinal(NodeLastUpdated);
							int ordinalDateUpdateDuration = reader.GetOrdinal(NodeLastUpdateDuration);

							if (!reader.IsDBNull(ordinalDateUpdate))
							{
								lastUpdate = reader.GetDateTime(ordinalDateUpdate);
							}

							if (!reader.IsDBNull(ordinalDateUpdateDuration))
							{
								lastUpdateDuration = reader.GetDateTime(ordinalDateUpdateDuration);
							}
						}
					}
				}
			}

			return new Tuple<DateTime?, DateTime?>(lastUpdate, lastUpdateDuration);
		}

		public void ComposeTableRow(TemplateNodeInfo node, out ITableRow nodeRow, out List<ITableRow> attributesRows)
		{
			Debug.Assert(node.IsInstance);

			Int64? parentNodeId = null;

			if (node.Parent != null)
			{
				if (node.Parent.TemplateNodeId == null)
				{
					throw new InvalidOperationException(node + ": it's parent is not saved (has no id)");
				}

				parentNodeId = node.Parent.TemplateNodeId;
			}

			nodeRow = new TableRow(this.TableDefinition);
			node.ConnectionGroup.ReadGroupIdFrom(this._storage.ConnectionGroupDirectory);

			nodeRow.Values.Add(
				ConnectionIdFn,
				node.ConnectionGroup.Identity
			);

			nodeRow.Values.Add(
				TemplateNodeIdFn,
				this._storage.TemplateNodeDirectory.GetId(node.ConnectionGroup, node.Template)
			);

			nodeRow.Values.Add(ParentIdFn,      parentNodeId);
			nodeRow.Values.Add(NodeUIdFn,       node.UId);
			nodeRow.Values.Add(NodeUNameFn,     node.UName);
			nodeRow.Values.Add(NodeUIconFn,     node.IconImageReferenceName);
			nodeRow.Values.Add(NodeEnabledFn,  !node.IsDisabled);
			nodeRow.Values.Add(NodeFontColorFn, node.FontColor);
			nodeRow.Values.Add(NodeFontStyleFn, node.FontStyle);

			if (node.CounterValue != null)
			{
				nodeRow.Values.Add(NodeCounterValue, node.CounterValue);
			}

			nodeRow.Values.Add(
				NodeSequenceNumberFn,
				node.Parent != null ? node.Parent.Childs.IndexOf(node) : 0
			);

			List<TemplateNodeUpdateJob> jobs = node.GetRefreshJob(true);

			foreach (TemplateNodeUpdateJob job in jobs)
			{
				if (job != null && job.LastRan != null)
				{
					nodeRow.Values.Add(NodeScheduledUpdateFn, job.LastRan);
				}
			}

			attributesRows = new List<ITableRow>();

			foreach (KeyValuePair<string, string> pair in node.Attributes.Values)
			{
				ITableRow row = new TableRow(NodeInstanceAttributeTable.CreateTableDefinition());

				row.Values.Add(NodeInstanceAttributeTable.NameFn, pair.Key);
				row.Values.Add(NodeInstanceAttributeTable.ValueFn, pair.Value);

				attributesRows.Add(row);
			}
		}

		public List<NodeInstanceRow> GetScheduledInstances()
		{
			List<NodeInstanceRow> scheduledInstances = new List<NodeInstanceRow>();

			using (this.Connection.OpenWrapper())
			{
				String query = string.Format(
					  "SELECT"
					+ "     node.{0}"
					+ "    ,node.{1}"
					+ "    ,tmnode.{6}"
					+ "    ,tm.{8}"
					+ "    ,node.[date_updated]"
					+ "    ,node.[{11}]"
					+ " FROM"
					+ "    [{2}] node"
					+ "    INNER JOIN {7} tmnode ON"
					+ "       tmnode.[{12}] = node.{1}"
					+ "    INNER JOIN {9} tm ON"
					+ "       tm.[{13}] = tmnode.{10}"
					+ "    INNER JOIN {4} tS ON"
					+ "       tS.[{3}] = node.[{11}]"
					+ " WHERE"
					+ "    node.{5} = 1;",
					ConnectionIdFn,
					TemplateNodeIdFn,
					TableName,
					ScheduleSettingsTable.TemplateNodeFk,
					ScheduleSettingsTable.TableName,
					NodeEnabledFn,
					TemplateNodeDirectory.NameFn,
					TemplateNodeDirectory.TableName,
					TemplateDirectory.IdFieldName,
					TemplateDirectory.TableName,
					TemplateNodeDirectory.TemplateIdFn,
					IdentityField,
					TemplateNodeDirectory.TableIdentityField,
					TemplateDirectory.TableIdentityField
				);

				_Log.InfoFormat(@"query:'{0}'",
					query
				);

				TableDefinition definition = CreateTableDefinition();

				using (this.Connection.OpenWrapper())
				{
					using (SQLiteCommand cmd = new SQLiteCommand(query, this.Connection))
					{
						using (SQLiteDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								ITableRow row = TableRow.Read(definition, reader);

								row.Values.Add(TemplateNodeDirectory.NameFn, reader[2]);
								row.Values.Add(TemplateDirectory.IdFieldName, reader[3]);

								scheduledInstances.Add(RowConverter.Convert<NodeInstanceRow>(row));
							}
						}
					}
				}

				return scheduledInstances;
			}
		}
	}
}
