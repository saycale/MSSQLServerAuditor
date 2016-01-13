// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyGroupsResolver.cs" company="">
//
// </copyright>
// <summary>
//   The empty groups resolver.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MSSQLServerAuditor.Model.Queries.GroupResolvers
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using MSSQLServerAuditor.Model.Groups;

    /// <summary>
    /// The empty groups resolver.
    /// </summary>
    public class EmptyGroupsResolver : BaseGroupResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyGroupsResolver" /> class.
        /// </summary>
        public EmptyGroupsResolver()
            : base(null, null)
        {
        }

        /// <inheritdoc />
        public override ICollection<GroupDefinition> GetGroups(QueryInfo query, IEnumerable<ParameterValue> values, InstanceVersion i)
        {
            return new List<GroupDefinition>()
                       {
                           new GroupDefinition(this.Instance, string.Empty, string.Empty)
                               {
                                   GroupType
                                       =
                                       QueryScope
                                       .Instance
                               }
                       };
        }

        /// <inheritdoc />
        public override IEnumerable<ParameterValue> GetParametersFromDefinition(GroupDefinition definition)
        {
            return new[] { new ParameterValue() { Name = string.Empty, StringValue = string.Empty } };
        }

        /// <inheritdoc />
        protected override void AddGroupsFromTable(DataTable groupTable, ICollection<GroupDefinition> groups)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override QueryItemInfo GetQueryForGroups(QueryInfo query, InstanceVersion version)
        {
            throw new NotImplementedException();
        }
    }
}