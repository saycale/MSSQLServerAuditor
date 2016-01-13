// -------------------------------------------------------------------------------------------------
// <copyright file="BaseQueryExecutor.cs" company="">
//
// </copyright>
// <summary>
//   The base query executor.
// </summary>
// -------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.Model.Delegates;
using MSSQLServerAuditor.Model.Groups;
using MSSQLServerAuditor.Model.Queries.GroupResolvers;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Queries
{
	/// <summary>
	/// The base query executor.
	/// </summary>
	public abstract class BaseQueryExecutor
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// The instance.
		/// </summary>
		protected readonly InstanceInfo Instance;

		/// <summary>
		/// The query item function.
		/// </summary>
		protected readonly BaseResolverQueryItemDelegate ExecuteQueryItemFunction;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseQueryExecutor" /> class.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="executeQueryItemFunction">The query item function.</param>
		protected BaseQueryExecutor(
			InstanceInfo                  instance,
			BaseResolverQueryItemDelegate executeQueryItemFunction
		)
		{
			this.Instance                 = instance;
			this.ExecuteQueryItemFunction = executeQueryItemFunction;
		}

		/// <summary>
		/// The execute query simple.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="parameterValues">The parameter values.</param>
		/// <param name="version">The version.</param>
		/// <param name="progress">The progress.</param>
		/// <param name="groups">The groups.</param>
		/// <returns>
		/// The <see cref="QueryInstanceResultInfo" />.
		/// </returns>
		public QueryInstanceResultInfo ExecuteQuerySimple(
			QueryInfo                query,
			QueryExecutionParams     parameters,
			InstanceVersion          version,
			ProgressItem             progress,
			params GroupDefinition[] groups
		)
		{
			// Log.InfoFormat("73:query:'{0}'",
			// 	query
			// );

			QueryInstanceResultInfo result = new QueryInstanceResultInfo(this.Instance);
			QueryItemInfo queryItem        = query.Items.GetQueryItemForVersion(version);

			this.PrepareProgress(progress, groups);

			this.ExecuteQuery(
				query,
				parameters,
				progress,
				groups,
				result,
				queryItem
			);

			return result;
		}

		/// <summary>
		/// The execute query.
		/// </summary>
		/// <param name="query">The query info.</param>
		/// <param name="parameterValues">The parameter values.</param>
		/// <param name="version">The version.</param>
		/// <param name="progress">The progress.</param>
		/// <returns>
		/// The <see cref="QueryInstanceResultInfo" />.
		/// </returns>
		public QueryInstanceResultInfo ExecuteQuery(
			QueryInfo            query,
			QueryExecutionParams parameters,
			InstanceVersion      version,
			ProgressItem         progress
		)
		{
			// Log.InfoFormat("107:query:'{0}'",
			// 	query
			// );

			var groups = this
				.GetGroupsResolver()
				.GetGroups(query, parameters.Values, version);

			return this.ExecuteQuerySimple(
				query,
				parameters,
				version,
				progress,
				groups.ToArray()
			);
		}

		/// <summary>
		/// The get groups resolver.
		/// </summary>
		/// <returns>
		/// The <see cref="BaseGroupResolver"/>.
		/// </returns>
		protected abstract BaseGroupResolver GetGroupsResolver();

		/// <summary>
		/// The process group.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="parameterValues">The parameter values.</param>
		/// <param name="result">The result.</param>
		/// <param name="queryItem">The query item.</param>
		/// <param name="group">The group.</param>
		/// <param name="subProgress">The sub progress.</param>
		protected virtual void ExecuteQuery(
			QueryInfo               query,
			QueryExecutionParams    parameters,
			QueryInstanceResultInfo result,
			QueryItemInfo           queryItem,
			GroupDefinition         group,
			ProgressItem            subProgress
		)
		{
			// Log.InfoFormat("146:query:'{0}'",
			// 	query
			// );

			var qr = this.ExecuteQueryItemFunction.Invoke(
				this.Instance,
				queryItem,
				string.IsNullOrWhiteSpace(group.Name) ? parameters.DefaultDatabase : group.Name,
				//group.Name,
				group.Id,
				query.Parameters,
				parameters.Values.ToList(),
				subProgress
			);

			// костыль. Наверное, прибъется вообще после убиения "групп"
			qr.Database = group.Name;

			result.AddDatabaseResult(qr);
		}

		/// <summary>
		/// The progress complete.
		/// </summary>
		/// <param name="progress">
		/// The progress.
		/// </param>
		private static void ProgressComplete(ProgressItem progress)
		{
			if (progress != null)
			{
				progress.SetProgress(100);
			}
		}

		/// <summary>
		/// The execute query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="parameterValues">The parameter values.</param>
		/// <param name="progress">The progress.</param>
		/// <param name="groups">The groups.</param>
		/// <param name="result">The result.</param>
		/// <param name="queryItem">The query item.</param>
		private void ExecuteQuery(
			QueryInfo               query,
			QueryExecutionParams    parameters,
			ProgressItem            progress,
			GroupDefinition[]       groups,
			QueryInstanceResultInfo result,
			QueryItemInfo           queryItem
		)
		{
			// Log.InfoFormat("query:'{0}'",
			// 	query
			// );

			Debug.Assert(groups.Length <= 1);

			if (groups.Length == 0)
			{
				this.ExecuteQuery(
					query,
					parameters,
					result,
					queryItem,
					new GroupDefinition(
						this.Instance,
						string.Empty,
						string.Empty
					),
					progress
				);

				ProgressComplete(progress);
			}
			else
			{
				foreach (var group in groups)
				{
					if (group.ChildGroups.Count > 0)
					{
						var tempParams = parameters.Clone();

						tempParams.AddValues(
							this.GetGroupsResolver().GetParametersFromDefinition(group)
						);

						this.ExecuteQuery(
							query,
							tempParams,
							progress,
							group.ChildGroups.ToArray(),
							result,
							queryItem
						);
					}

					ProgressItem subProgress = null;

					if (progress != null)
					{
						try
						{
							subProgress = progress.GetChild();
						}
						catch (Exception)
						{
						}
					}

					this.ExecuteQuery(
						query,
						parameters,
						result,
						queryItem,
						@group,
						subProgress
					);

					ProgressComplete(subProgress);
				}
			}
		}

		/// <summary>
		/// The prepare progress.
		/// </summary>
		/// <param name="progress">The progress.</param>
		/// <param name="groups">The groups.</param>
		private void PrepareProgress(ProgressItem progress, GroupDefinition[] groups)
		{
			if (progress != null)
			{
				if (groups.Length > 0)
				{
					progress.SetPromisedChildCount(groups.Length);
				}
				else
				{
					progress.SetProgress(100);
				}
			}
		}
	}
}
