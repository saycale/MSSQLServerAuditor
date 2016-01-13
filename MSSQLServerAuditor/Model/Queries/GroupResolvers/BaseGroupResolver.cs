using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using log4net;
using MSSQLServerAuditor.Model.Delegates;
using MSSQLServerAuditor.Model.Groups;
using MSSQLServerAuditor.Utils;

// -------------------------------------------------------------------------------------------------
// <copyright file="BaseGroupResolver.cs" company="">
//
// </copyright>
// <summary>
//   The base group resolver.
// </summary>
// -------------------------------------------------------------------------------------------------
namespace MSSQLServerAuditor.Model.Queries.GroupResolvers
{
	/// <summary>
	///     The base group resolver.
	/// </summary>
	public abstract class BaseGroupResolver
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// The executed SQL function.
		/// </summary>
		protected readonly BaseResolverDelegate ExecuteSqlFunction;

		/// <summary>
		/// The instance.
		/// </summary>
		protected readonly InstanceInfo Instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseGroupResolver"/> class.
		/// </summary>
		/// <param name="instance">
		/// The instance.
		/// </param>
		/// <param name="executeSqlFunction">
		/// The execute SQL function.
		/// </param>
		protected BaseGroupResolver(
			InstanceInfo         instance,
			BaseResolverDelegate executeSqlFunction
		)
		{
			this.Instance           = instance;
			this.ExecuteSqlFunction = executeSqlFunction;
		}

		/// <summary>
		/// The get groups.
		/// </summary>
		/// <param name="query">
		/// The query information.
		/// </param>
		/// <param name="parameterValues">
		/// The parameter values.
		/// </param>
		/// <param name="version">
		/// The version.
		/// </param>
		/// <returns>
		/// The <see cref="Dictionary{TKey,TValue}"/>.
		/// </returns>
		public virtual ICollection<GroupDefinition> GetGroups(
			QueryInfo                   query,
			IEnumerable<ParameterValue> parameterValues,
			InstanceVersion             version
		)
		{
			var groups = new List<GroupDefinition>();

			// TODO: Create GroupDefinition with childrens
			QueryItemInfo selectGroupsSql = this.GetQueryForGroups(query, version);

			if (selectGroupsSql != null)
			{
				try
				{
					var paramsArray = parameterValues as ParameterValue[] ?? parameterValues.ToArray();

					DataTable[] groupsTabels = this.ExecuteSqlFunction(
						this.Instance,
						selectGroupsSql,
						null,
						query.Parameters,
						paramsArray,
						null,
						true
					);

					foreach (DataTable groupTable in groupsTabels)
					{
						this.AddGroupsFromTable(groupTable, groups);
					}

					if (selectGroupsSql.ChildGroups.IsNullOrEmpty())
					{
						return groups;
					}

					foreach (GroupDefinition groupDefinition in groups)
					{
						this.GetChildGroupsDefinitions(selectGroupsSql, groupDefinition, paramsArray, version);
					}
				}
				catch (OperationCanceledException ex)
				{
					log.Error(ex);
					throw;
				}
				catch (Exception ex)
				{
					log.Error("instance=" + this.Instance.Name + " query=" + query, ex);
				}
			}

			return groups;
		}

		/// <summary>
		/// The get parameters from definition.
		/// </summary>
		/// <param name="definition">
		/// The parent definition.
		/// </param>
		/// <returns>
		/// The <see cref="IEnumerable{T}"/>.
		/// </returns>
		public abstract IEnumerable<ParameterValue> GetParametersFromDefinition(GroupDefinition definition);

		/// <summary>
		/// The process group table.
		/// </summary>
		/// <param name="groupTable">
		/// The cur table.
		/// </param>
		/// <param name="groups">
		/// The groups.
		/// </param>
		protected abstract void AddGroupsFromTable(DataTable groupTable, ICollection<GroupDefinition> groups);

		/// <summary>
		/// The get query item for version.
		/// </summary>
		/// <param name="query">
		/// The query.
		/// </param>
		/// <param name="version">
		/// The version.
		/// </param>
		/// <returns>
		/// The <see cref="QueryItemInfo"/>.
		/// </returns>
		protected abstract QueryItemInfo GetQueryForGroups(QueryInfo query, InstanceVersion version);

		/// <summary>
		/// The get child groups definitions.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="parentDefinition">The parent definition.</param>
		/// <param name="parameterValues">The parameter values.</param>
		/// <param name="version">The version.</param>
		private void GetChildGroupsDefinitions(
			QueryItemInfo               parent,
			GroupDefinition             parentDefinition,
			IEnumerable<ParameterValue> parameterValues,
			InstanceVersion             version
		)
		{
			QueryItemInfo childGroupSql = parent.ChildGroups.GetQueryItemForVersion(version);
			var           tempParams    = parameterValues.ToList();

			tempParams.AddRange(this.GetParametersFromDefinition(parentDefinition));

			try
			{
				DataTable[] groupsTabels = this.ExecuteSqlFunction(
					this.Instance,
					childGroupSql,
					null,
					childGroupSql.Parameters,
					tempParams,
					null,
					true
				);

				foreach (DataTable groupTable in groupsTabels)
				{
					this.AddGroupsFromTable(groupTable, parentDefinition.ChildGroups);
				}

				if (childGroupSql.ChildGroups == null || childGroupSql.ChildGroups.Count <= 0)
				{
					return;
				}

				foreach (GroupDefinition childGroup in parentDefinition.ChildGroups)
				{
					this.GetChildGroupsDefinitions(childGroupSql, childGroup, tempParams, version);
				}
			}
			catch (OperationCanceledException ex)
			{
				log.Error(ex);
				throw;
			}
			catch (Exception ex)
			{
				log.Error("instance=" + this.Instance.Name + " query=" + childGroupSql, ex);
			}
		}
	}
}
