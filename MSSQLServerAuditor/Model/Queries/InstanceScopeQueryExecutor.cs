// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceScopeQueryExecutor.cs" company="">
//
// </copyright>
// <summary>
//   The instance scope query executor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using MSSQLServerAuditor.Model.Delegates;

namespace MSSQLServerAuditor.Model.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using MSSQLServerAuditor.Model.Queries.GroupResolvers;

    /// <summary>
    /// The instance scope query executor.
    /// </summary>
    internal class InstanceScopeQueryExecutor : BaseQueryExecutor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceScopeQueryExecutor" /> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="executeQueryItemFunction">The query item function.</param>
        public InstanceScopeQueryExecutor(
            InstanceInfo instance,
            BaseResolverQueryItemDelegate executeQueryItemFunction)
            : base(instance, executeQueryItemFunction)
        {
        }

        /// <inheritdoc/>
        protected override BaseGroupResolver GetGroupsResolver()
        {
            return new EmptyGroupsResolver();
        }
    }
}