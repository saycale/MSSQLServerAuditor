// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryExecutorFactory.cs" company="">
//
// </copyright>
// <summary>
//   The query executor factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using MSSQLServerAuditor.Model.Delegates;

namespace MSSQLServerAuditor.Model.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// The query executor factory.
    /// </summary>
    public class QueryExecutorFactory
    {
        /// <summary>
        /// The instance.
        /// </summary>
        private readonly InstanceInfo instance;

        /// <summary>
        /// The execute sql function.
        /// </summary>
        private readonly BaseResolverDelegate executeSqlFunction;

        /// <summary>
        /// The query item function.
        /// </summary>
        private readonly BaseResolverQueryItemDelegate executeQueryItemFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExecutorFactory"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="executeQueryItemFunction">
        /// The query item function.
        /// </param>
        /// <param name="executeSqlFunction">
        /// The execute sql function.
        /// </param>
        public QueryExecutorFactory(
            InstanceInfo instance,
            BaseResolverQueryItemDelegate executeQueryItemFunction,
            BaseResolverDelegate executeSqlFunction)
        {
            this.instance = instance;
            this.executeQueryItemFunction = executeQueryItemFunction;
            this.executeSqlFunction = executeSqlFunction;
        }

        /// <summary>
        /// The get executor.
        /// </summary>
        /// <param name="queryScope">
        /// The query scope.
        /// </param>
        /// <returns>
        /// The <see cref="BaseQueryExecutor"/>.
        /// </returns>
        public BaseQueryExecutor GetExecutor(QueryScope queryScope)
        {
            switch (queryScope)
            {
                case QueryScope.Instance:
                    return new InstanceScopeQueryExecutor(
                        this.instance, this.executeQueryItemFunction);
                case QueryScope.Database:
                    return new DatabaseScopeQueryExecutor(
                        this.instance, this.executeQueryItemFunction, this.executeSqlFunction);
                case QueryScope.InstanceGroup:
                    return new InstanceGroupScopeQueryExecutor(
                        this.instance, this.executeQueryItemFunction, this.executeSqlFunction);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}