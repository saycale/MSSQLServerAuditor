
namespace MSSQLServerAuditor.Model.Groups
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    ///     Definition for group
    /// </summary>
    [Serializable]
    public class GroupDefinition : IEquatable<GroupDefinition>
    {
        /// <summary>
        ///     Initializes static members of the <see cref="GroupDefinition" /> class.
        /// </summary>
        static GroupDefinition()
        {
            NullGroup = new GroupDefinition(null, string.Empty, string.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupDefinition"/> class.
        ///     Default constructor
        /// </summary>
        /// <param name="instance">
        /// MS SQL server instance
        /// </param>
        /// <param name="name">
        /// Name of database
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        public GroupDefinition(InstanceInfo instance, string name, string id, bool alwaysSetInstance = false)
        {
            this.Name = name;
            this.Id = id;
            if (!string.IsNullOrEmpty(this.Name) || alwaysSetInstance)
            {
                this.Instance = instance;
            }

            this.GroupParameters = new Dictionary<string, string>();
            ChildGroups = new List<GroupDefinition>();
        }

        /// <summary>
        ///     Gets the null group.
        /// </summary>
        public static GroupDefinition NullGroup { get; private set; }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        [XmlElement]
        public InstanceInfo Instance { get; private set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        [XmlElement]
        public string Name { get; private set; }

        /// <summary>
        ///     Gets the id.
        /// </summary>
        [XmlIgnore]
        public string Id { get; private set; }

        /// <summary>
        ///     Gets or sets the group parameters.
        /// </summary>
        [XmlElement]
        public Dictionary<string, string> GroupParameters { get; set; }

        /// <summary>
        /// Gets or sets the group type.
        /// </summary>
        [XmlElement]
        public QueryScope GroupType { get; set; }

        /// <summary>
        /// Gets or sets the child groups.
        /// </summary>
        [XmlElement]
        public ICollection<GroupDefinition> ChildGroups { get; set; }

        /// <summary>
        /// Equals objects.
        /// </summary>
        /// <param name="other">
        /// Definition database.
        /// </param>
        /// <returns>
        /// Equals database.
        /// </returns>
        public bool Equals(GroupDefinition other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.Instance, other.Instance) && Equals(this.Name, other.Name);
        }

        /// <inheritdoc />
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

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((GroupDefinition)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Instance != null ? this.Instance.GetHashCode() : 0) * 397)
                       ^ (this.Name != null ? this.Name.GetHashCode() : 0);
            }
        }

        /// <summary>
        ///     The ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator ==(GroupDefinition left, GroupDefinition right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     The !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator !=(GroupDefinition left, GroupDefinition right)
        {
            return !Equals(left, right);
        }
    }
}