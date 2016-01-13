using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MSSQLServerAuditor.SQLite.Tables.Directories;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Connection group info
	/// </summary>
	[Serializable]
	public class ConnectionGroupInfo
	{
		private List<InstanceInfo> _connections;
		private string             _templateId;
		private Int64?             _groupId;
		private readonly object    _syncLock;

		public ConnectionGroupInfo()
		{
			this._connections       = new List<InstanceInfo>();
			this._templateId        = null;
			this._groupId           = null;
			this._syncLock          = new object();
			this.IsDirectConnection = false;
		}

		/// <summary>
		/// The create trial connection group.
		/// </summary>
		/// <param name="propertiesList">The properties list.</param>
		/// <param name="templateFile">Path to temlate file</param>
		/// <param name="dbType">Type DB</param>
		/// <param name="connectionGroupName">Name connection group</param>
		/// <param name="isExternalTemplate">Is opened from user file template </param>
		/// <returns>
		/// The <see cref="ConnectionGroupInfo" />.
		/// </returns>
		public static ConnectionGroupInfo CreateTrialConnectionGroup(
			List<Tuple<DbConnectionStringBuilder, bool>> propertiesList,
			string                                       templateFile,
			QuerySource                                  dbType,
			string                                       connectionGroupName,
			bool                                         isExternalTemplate
		)
		{
			ConnectionGroupInfo connectionGroup = new ConnectionGroupInfo
			{
				Connections      = InstanceInfoResolver.ResolveInstances(propertiesList, dbType),
				IsExternal       = isExternalTemplate,
				TemplateDir      = Path.GetDirectoryName(templateFile),
				TemplateFileName = Path.GetFileName(templateFile),
				Name             = connectionGroupName
			};

			return connectionGroup;
		}

		/// <summary>
		/// Type of connection
		/// we do have two types of connection dialogs: direct and non direct connections
		/// </summary>
		[XmlElement(ElementName = "isDirect")]
		public bool IsDirectConnection { get; set; }

		/// <summary>
		/// List of connections
		/// </summary>
		[XmlElement(ElementName = "connection")]
		public List<InstanceInfo> Connections
		{
			get { return this._connections; }
			set { this._connections = value; }
		}

		/// <summary>
		/// Template filename for connections
		/// </summary>
		[XmlAttribute(AttributeName = "template")]
		public string TemplateFileName { get; set; }

		/// <summary>
		/// Template filename for connections
		/// </summary>
		[XmlAttribute(AttributeName = "templateDir")]
		public string TemplateDir { get; set; }

		/// <summary>
		/// Text name for connections
		/// </summary>
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Is opened from user file template
		/// </summary>
		[XmlAttribute(AttributeName = "IsExternal")]
		public bool IsExternal { get; set; }

		/// <summary>
		/// Instances count
		/// </summary>
		public int InstancesCount
		{
			get { return this.Connections.Count; }
		}

		/// <summary>
		/// Connection group initialization
		/// </summary>
		public void Init()
		{
			foreach (var instance in Connections)
			{
				instance.ConnectionGroup = this;
			}
		}

		public ConnectionGroupInfo CopyXmlContent()
		{
			var serializer = new XmlSerializer(typeof(ConnectionGroupInfo));

			using (MemoryStream memoryStream = new MemoryStream())
			{
				serializer.Serialize(memoryStream, this);

				memoryStream.Position = 0;

				ConnectionGroupInfo copy = (ConnectionGroupInfo)serializer.Deserialize(memoryStream);

				copy.Identity = null;
				copy.Init();

				return copy;
			}
		}

		/// <summary>
		/// Template Id obtained from template file
		/// </summary>
		public string TemplateId
		{
			get { return this._templateId; }
			set { this._templateId = value; }
		}

		/// <summary>
		/// Row Id of record in database
		/// </summary>
		public Int64? Identity
		{
			get
			{
				lock (this._syncLock)
				{
					return this._groupId;
				}
			}

			set
			{
				lock (this._syncLock)
				{
					this._groupId = value;
				}
			}
		}

		private void AssignGroupId(Int64? value)
		{
			lock (this._syncLock)
			{
				if (this._groupId != null && value != this._groupId)
				{
					String strException = string.Format(
						"Group id is already set to '{0}' and new value different:'{1}'",
						this.Identity ?? -1L,
						value         ?? -1L
					);

					throw new InvalidOperationException(strException);
				}

				this._groupId = value;
			}
		}

		/// <summary>
		/// Get connections with enabled instances only
		/// </summary>
		/// <returns>New connections</returns>
		public ConnectionGroupInfo ExtractSelectedGroup()
		{
			ConnectionGroupInfo result = new ConnectionGroupInfo
			{
				Name             = Name,
				TemplateFileName = TemplateFileName,
				TemplateDir      = TemplateDir,
				IsExternal       = IsExternal,
			};

			foreach (InstanceInfo cnn in Connections)
			{
				if (cnn.IsEnabled)
				{
					result.Connections.Add(cnn);
					cnn.ConnectionGroup = result;
				}
			}

			return result;
		}

		/// <summary>
		/// Login user name.
		/// </summary>
		public string UserLoginName
		{
			get
			{
				string result = "...";

				if (this.InstancesCount > 0)
				{
					if (this.Connections.All(
						c => c.Authentication.Equals(this.Connections[0].Authentication))
					)
					{
						result = Connections[0].Authentication.GetCurrentLogin();
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Objects to string.
		/// </summary>
		/// <returns>String to objects.</returns>
		public override string ToString()
		{
			return this.Name;
		}

		public string DisplayName
		{
			get
			{
				return string.Format("{0} ({1})",
					this.Name,
					this.TemplateId
				);
			}
		}

		public void ReadGroupIdFrom(ConnectionGroupDirectory directory)
		{
			Int64? groupId = null;

			if (directory != null)
			{
				groupId = directory.GetId(this);
			}

			AssignGroupId(groupId);
		}
	}
}
