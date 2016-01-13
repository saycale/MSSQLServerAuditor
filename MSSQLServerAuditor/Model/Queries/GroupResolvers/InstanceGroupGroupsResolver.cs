// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceGroupGroupsResolver.cs" company="">
//
// </copyright>
// <summary>
//   The instance group groups resolver.
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
    /// The instance group groups resolver.
    /// </summary>
    public class InstanceGroupGroupsResolver : BaseGroupResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceGroupGroupsResolver" /> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="executeSqlFunction">The execute SQL function.</param>
        public InstanceGroupGroupsResolver(
            InstanceInfo instance,
            BaseResolverDelegate executeSqlFunction)
            : base(instance, executeSqlFunction)
        {
        }

        /// <inheritdoc />
        public override IEnumerable<ParameterValue> GetParametersFromDefinition(GroupDefinition definition)
        {
            return
                definition.GroupParameters.Select(
                    groupParameter =>
                    new ParameterValue()
                        {
                            Name = "@" + groupParameter.Key,
                            StringValue = groupParameter.Value
                        });
        }

        /// <inheritdoc />
        protected override void AddGroupsFromTable(DataTable groupTable, ICollection<GroupDefinition> groups)
        {
            if (groupTable.Columns.Count < 2)
            {
                throw new ArgumentException("Table with groups must contains minimum two columns. GroupName and GroupId.");
            }

            groupTable.Rows.Cast<DataRow>()
                      .ToList()
                      .ForEach(
                          row =>
                              {
                                  var group = new GroupDefinition(this.Instance, row[0].ToString(), row[1].ToString())
                                                  {
                                                      GroupType = QueryScope.InstanceGroup
                                                  };

                                  foreach (DataColumn column in groupTable.Columns)
                                  {
                                      group.GroupParameters.Add(column.ColumnName, row[column].ToString());
                                  }

                                  groups.Add(group);
                              });
        }

        /// <inheritdoc />
        protected override QueryItemInfo GetQueryForGroups(QueryInfo query, InstanceVersion version)
        {
            return query.GroupSelect.GetQueryItemForVersion(version);
        }
    }
}