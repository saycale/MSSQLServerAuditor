// -------------------------------------------------------------------------------------------------
// <copyright file="InstanceGroupQueryExecutor.cs" company="">
//
// </copyright>
// <summary>
//   The instance group query executor.
// </summary>
// -------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MSSQLServerAuditor.Model.Groups;
using MSSQLServerAuditor.Model.Queries.GroupResolvers;
using MSSQLServerAuditor.Model.Delegates;

namespace MSSQLServerAuditor.Model.Queries
{
	/// <summary>
	///     The instance group query executor.
	/// </summary>
	public class InstanceGroupScopeQueryExecutor : BaseQueryExecutor
	{
		/// <summary>
		///     The execute SQL function
		/// </summary>
		private readonly BaseResolverDelegate executeSqlFunction;

		/// <summary>
		/// Initializes a new instance of the <see cref="InstanceGroupScopeQueryExecutor"/> class.
		/// </summary>
		/// <param name="instance">
		/// The instance.
		/// </param>
		/// <param name="executeQueryItemFunction">
		/// The query item function.
		/// </param>
		/// <param name="executeSqlFunction">
		/// The execute SQL function.
		/// </param>
		public InstanceGroupScopeQueryExecutor(
			InstanceInfo                  instance,
			BaseResolverQueryItemDelegate executeQueryItemFunction,
			BaseResolverDelegate          executeSqlFunction
		) : base(
				instance,
				executeQueryItemFunction
			)
		{
			this.executeSqlFunction = executeSqlFunction;
		}

		/// <inheritdoc />
		protected override BaseGroupResolver GetGroupsResolver()
		{
			return new InstanceGroupGroupsResolver(this.Instance, this.executeSqlFunction);
		}

		/// <inheritdoc />
		protected override void ExecuteQuery(
			QueryInfo               query,
			QueryExecutionParams    parameters,
			QueryInstanceResultInfo result,
			QueryItemInfo           queryItem,
			GroupDefinition         group,
			ProgressItem            subProgress
		)
		{
			var tempValues = new List<ParameterValue>(parameters.Values);

			foreach (var parameter in group.GroupParameters)
			{
				tempValues.Add(new ParameterValue
					{
						Name        = "@" + parameter.Key,
						StringValue = parameter.Value
					}
				);
			}

			QueryDatabaseResultInfo databaseResult = this.ExecuteQueryItemFunction.Invoke(
				this.Instance,
				queryItem,
				null,
				null,
				query.Parameters,
				tempValues,
				subProgress
			);

			databaseResult.Database   = group.Name;
			databaseResult.DatabaseId = group.Id;

			result.AddDatabaseResult(databaseResult);
		}
	}
}
