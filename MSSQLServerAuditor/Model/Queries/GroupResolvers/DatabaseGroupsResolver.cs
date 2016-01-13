// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseGroupsResolver.cs" company="">
//
// </copyright>
// <summary>
//   The database groups resolver.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using MSSQLServerAuditor.Model.Delegates;

namespace MSSQLServerAuditor.Model.Queries.GroupResolvers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using MSSQLServerAuditor.Model.Groups;
    using MSSQLServerAuditor.Utils;

    /// <summary>
    /// The database groups resolver.
    /// </summary>
    public class DatabaseGroupsResolver : BaseGroupResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseGroupsResolver"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="executeSqlFunction">
        /// The execute sql function.
        /// </param>
        public DatabaseGroupsResolver(
            InstanceInfo instance,
            BaseResolverDelegate executeSqlFunction)
            : base(instance, executeSqlFunction)
        {
        }

        /// <inheritdoc />
        public override IEnumerable<ParameterValue> GetParametersFromDefinition(GroupDefinition definition)
        {
            return new[] { new ParameterValue() { Name = definition.Name, StringValue = definition.Id } };
        }

        /// <inheritdoc />
        protected override void AddGroupsFromTable(DataTable groupTable, ICollection<GroupDefinition> groups)
        {
            groupTable.Rows.Cast<DataRow>()
                      .ToList()
                      .ForEach(
                          row =>
                          groups.Add(
                              new GroupDefinition(this.Instance, row[0].ToString(), row[1].ToString())
                                  {
                                      GroupType =
                                          QueryScope
                                          .Database
                                  }));
        }

        /// <inheritdoc />
        protected override QueryItemInfo GetQueryForGroups(QueryInfo query, InstanceVersion version)
        {
            if (query.GroupSelect != null && query.GroupSelect.Count > 0)
            {
                return query.GroupSelect.GetQueryItemForVersion(version);
            }

            return query.DatabaseSelect.GetQueryItemForVersion(version);
        }
    }
}