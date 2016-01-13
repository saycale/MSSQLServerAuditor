using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using log4net;
using MSSQLServerAuditor.Model.Groups;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	///     Information on connection.
	/// </summary>
	[Serializable]
	public class ConnectionInfo
	{
		/// <summary>
		///     Initializing object ConnectionInfo.
		/// </summary>
		public ConnectionInfo()
		{
		}

		/// <summary>
		///     Initializing object ConnectionInfo.
		/// </summary>
		/// <param name="connectionGroup">Group connection.</param>
		public ConnectionInfo(ConnectionGroupInfo connectionGroup) : this()
		{
			this.ConnectionGroupName = connectionGroup.Name;
			this.UserLoginName       = connectionGroup.UserLoginName;
		}

		/// <summary>
		///     Group name connection.
		/// </summary>
		public string ConnectionGroupName { get; set; }

		/// <summary>
		///     User name.
		/// </summary>
		public string UserLoginName { get; set; }
	}

	/// <summary>
	///     Definition for concrete node in template tree (default or for database)
	/// </summary>
	[Serializable]
	public class ConcreteTemplateNodeDefinition : IEquatable<ConcreteTemplateNodeDefinition>
	{
		[NonSerialized] private static readonly log4net.ILog        log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly        TemplateNodeInfo    _templateNode;
		private readonly        GroupDefinition     _group;
		[NonSerialized] private ConnectionGroupInfo _connection;

		/// <summary>
		///     Information group on connection.
		/// </summary>
		public ConnectionGroupInfo Connection
		{
			get { return this._connection; }
			private set { this._connection = value; }
		}

		//offline default active  mode (not use)
		//[OnDeserialized()]
		//public void Init(StreamingContext context)
		//{
		//    this.NodeActivated = true;
		//}

		/// <summary>
		///     Check if the node is active
		/// </summary>
		public bool NodeActivated { get; set; }

		/// <summary>
		///     Available node for the database
		/// </summary>
		public bool NodeAvailable { get; set; }

		/// <summary>
		///     Constructor for serialization
		/// </summary>
		public ConcreteTemplateNodeDefinition()
		{
			this.NodeActivated = true;
			this.NodeAvailable = true;
		}

		/// <summary>
		///     Constructor (without database).
		/// </summary>
		/// <param name="templateNode">Template node.</param>
		/// <param name="connection">Information group on connection.</param>
		public ConcreteTemplateNodeDefinition(TemplateNodeInfo templateNode, ConnectionGroupInfo connection)
		{
			this._templateNode = templateNode;
			this._group        = GroupDefinition.NullGroup;
			this.Connection    = connection;
			this.NodeActivated = true;
			this.NodeAvailable = true;
		}

		/// <summary>
		///     Constructor (with database).
		/// </summary>
		/// <param name="templateNode">Template node.</param>
		/// <param name="group">Database.</param>
		/// ///
		/// <param name="connection">Connection group information.</param>
		public ConcreteTemplateNodeDefinition(TemplateNodeInfo templateNode, GroupDefinition @group, ConnectionGroupInfo connection)
		{
			Debug.Assert(templateNode.IsInstance);

			this._templateNode = templateNode;
			this._group        = @group;
			this.Connection    = connection;
			this.NodeActivated = true;
			this.NodeAvailable = true;
		}

		/// <summary>
		///     Template node.
		/// </summary>
		[XmlElement]
		public TemplateNodeInfo TemplateNode
		{
			get { return this._templateNode; }
		}

		/// <summary>
		///     Database (null if empty).
		/// </summary>
		[XmlElement]
		public GroupDefinition Group
		{
			get { return this._group; }
		}

		/// <summary>
		///     Defines there are details about a database or not.
		/// </summary>
		public bool IsDataseDetail
		{
			get { return this.Group != null && !string.IsNullOrEmpty(this.Group.Name); }
		}

		public bool IsRoot
		{
			get { return TemplateNode.Parent == null; }
		}

		/// <summary>
		///     Equals two objects.
		/// </summary>
		/// <param name="other">Specific determination of a node of a template.</param>
		/// <returns>True if are identical.</returns>
		public bool Equals(ConcreteTemplateNodeDefinition other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Equals(this._templateNode, other._templateNode) && Equals(this._group, other._group);
		}

		/// <summary>
		///     Equals two objects.
		/// </summary>
		/// <param name="obj">Object for equals.</param>
		/// <returns>True if are identical.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((ConcreteTemplateNodeDefinition) obj);
		}

		/// <summary>
		///     Get the hash for the object code
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return ((this._templateNode != null ? this._templateNode.GetHashCode() : 0) * 397) ^
						(this._group        != null ? this._group.GetHashCode() : 0);
			}
		}

		/// <summary>
		///     Сreate the object clone
		/// </summary>
		/// <returns>The created object.</returns>
		public ConcreteTemplateNodeDefinition Clone()
		{
			return (ConcreteTemplateNodeDefinition) MemberwiseClone();
		}

		public bool? IsAvailableForDatabase(MsSqlAuditorModel sqlAuditor)
		{
			if (TemplateNode.ConnectionQueries.Count > 0)
			{
				return true;
			}

			CurrentStorage storage = sqlAuditor.GetVaultProcessor(Connection).CurrentStorage;
			int            timeout = sqlAuditor.Settings.SqlTimeout;

			List<InstanceVersion> versions =
				Connection.Connections.Select(cnn =>
					{
						try
						{
							return cnn.InitServerProperties(storage, timeout).Version;
						}
						catch (Exception ex)
						{
							log.Error("Error in get version.", ex);

							return new InstanceVersion();
						}
					}
				).ToList();

			List<QuerySource> queriesTypes = Connection.Connections.Select(x => x.Type).ToList();

			if (TemplateNode.Queries.Count == 0)
			{
				return true;
			}

			return TemplateNode.Queries.Any(x =>
			{
				List<QueryInfo> queries =
					sqlAuditor.GetQueryByTemplateNodeQueryInfo(x)
						.Where(y => queriesTypes.Contains(y.Source) || y.Source == QuerySource.SQLite)
						.ToList();

				return versions.Distinct().Any(
					version => queries.Any(
						queryInfo => queryInfo.Items.GetQueryItemForVersion(version) != null));
			});
		}

		public string FormatNodeText(int? resultRowCount = null)
		{
			string text = String.Empty;

			if (this.IsDataseDetail)
			{
				text = Group.Name;
				Debug.Fail("");
			}
			else
			{
				text = TemplateNode.Title;
			}

			if (resultRowCount != null && TemplateNode.ShowNumberOfRecords)
			{
				text = text + " (" + resultRowCount.Value + ")";
			}

			return text;
		}

		public long ComputeHandle()
		{
			unchecked
			{
				long handle = 0L;

				if (TemplateNode.TemplateNodeId.HasValue)
				{
					handle |= TemplateNode.TemplateNodeId.Value;
				}

				if (Connection.Identity.HasValue)
				{
					handle |= Connection.Identity.Value << 32;
				}

				return handle;
			}
		}
	}
}
