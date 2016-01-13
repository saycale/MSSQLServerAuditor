// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseScopeQueryExecutor.cs" company="">
//
// </copyright>
// <summary>
//   The database scope query.
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
    ///     The database scope query.
    /// </summary>
    public class DatabaseScopeQueryExecutor : BaseQueryExecutor
    {
        /// <summary>
        ///     The execute SQL function
        /// </summary>
        private readonly BaseResolverDelegate executeSqlFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseScopeQueryExecutor"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="executeQueryItemFunction">
        /// The query item function.
        /// </param>
        /// <param name="executeSqlFunction">
        /// The execute SQL Function.
        /// </param>
        public DatabaseScopeQueryExecutor(
            InstanceInfo instance,
            BaseResolverQueryItemDelegate executeQueryItemFunction,
            BaseResolverDelegate executeSqlFunction)
            : base(instance, executeQueryItemFunction)
        {
            this.executeSqlFunction = executeSqlFunction;
        }

        /// <inheritdoc />
        protected override BaseGroupResolver GetGroupsResolver()
        {
            return new DatabaseGroupsResolver(this.Instance, this.executeSqlFunction);
        }
    }
}