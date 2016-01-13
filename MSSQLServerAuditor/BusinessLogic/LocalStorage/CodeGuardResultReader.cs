using System;
using System.Collections.Generic;
using System.Linq;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Tables;

namespace MSSQLServerAuditor.BusinessLogic.LocalStorage
{
	public class CodeGuardResultReader : NodeResultReader
	{
		public CodeGuardResultReader(
			MsSqlAuditorModel              msSqlAuditor,
			StorageManager                 storageManager,
			ConcreteTemplateNodeDefinition concreteTemplateNode) :
			base(msSqlAuditor, storageManager, concreteTemplateNode)
		{
		}

		public override void ReadTo(MultyQueryResultInfo result)
		{
			foreach (var guardQueryInfo in base.TemplateNode.SqlCodeGuardQueries)
			{
				this.ProcessCodeGuardQuery(result, guardQueryInfo);
			}
		}

		private void ProcessCodeGuardQuery(
			MultyQueryResultInfo          result,
			TemplateNodeSqlGuardQueryInfo guardQueryInfo)
		{
			var queryResultInfo = result.List.First(item => item.TemplateNodeQuery.Id == guardQueryInfo.SqlQueryId);
			var templateNodeQueryInfo = queryResultInfo.TemplateNodeQuery;
			var userParams = new List<ParameterValue>();

			if (base.Settings != null)
			{
				var querySettings = base.Settings.Connection.Activity.Parameters
					.Where(i => i.Key == guardQueryInfo.IdsHierarchy && i.Value != null);

				foreach (var info in querySettings)
				{
					switch (info.Type)
					{
						case ParameterInfoType.Attribute:
							guardQueryInfo.GetType().GetProperty("User" + info.Parameter)
								.SetValue(guardQueryInfo, info.Value, null);
							break;

						case ParameterInfoType.Parameter:
							var parameter =
								templateNodeQueryInfo.ParameterValues.FirstOrDefault(p => p.Name == info.Parameter);
							if (parameter != null)
							{
								parameter.UserValue = info.Value;
							}
							break;

						case ParameterInfoType.EditableParameter:
							var editparameter = new ParameterValue
							{
								Name = info.Parameter,
								StringValue = info.Default,
								UserValue = info.Value
							};
							userParams.Add(editparameter);
							break;
					}
				}
			}

			var guardQueryResult = new QueryResultInfo();

			foreach (var instanceResult in queryResultInfo.QueryResult.InstancesResult)
			{
				var instance = instanceResult.Key;
				var queryTable = instanceResult.Value.DatabasesResult.First().Value.DataTables.First();

				if (!queryTable.Columns.Contains(guardQueryInfo.QueryCodeColumn))
				{
					continue;
				}

				//var meta = ReadMeta(connectionGroup, templateNode, instance, database, templateNodeQueryInfo).FirstOrDefault();
				var meta = Storage.ReadLastMeta(
					base.TemplateNode,
					instance,
					templateNodeQueryInfo
				);

				if (meta == null)
				{
					continue;
				}

				QueryInstanceResultInfo guardInstanceResult;
				var timestamp = (DateTime)meta.Values[TableDefinition.DateCreated];

				result.RefreshTimestamp(timestamp);

				if (!string.IsNullOrEmpty(meta.Values[MetaResultTable.ErrorMessageFieldName].ToString()))
				{
					guardInstanceResult = new QueryInstanceResultInfo(
						new ErrorInfo(
							meta.Values[MetaResultTable.ErrorIdFieldName].ToString(),
							meta.Values[MetaResultTable.ErrorCodeFieldName].ToString(),
							meta.Values[MetaResultTable.ErrorMessageFieldName].ToString(),
							timestamp
						),
						instance
					);
				}
				else
				{
					guardInstanceResult = new QueryInstanceResultInfo(instance);
				}

				var dataTables = Storage.ReadSqlCodeGuardResult(guardQueryInfo, queryTable, userParams);

				QueryItemInfo queryItemInfo = new QueryItemInfo
				{
					ParentQuery = new QueryInfo { Name = guardQueryInfo.QueryName }
				};

				QueryDatabaseResultInfo databaseResult = new QueryDatabaseResultInfo(
					dataTables,
					queryItemInfo,
					base.GroupDefinition.Name,
					base.GroupDefinition.Id
				);

				guardInstanceResult.AddDatabaseResult(databaseResult);
				guardQueryResult.AddInstanceResult(guardInstanceResult);
			}

			var templateNodeResultItem = new TemplateNodeResultItem(guardQueryInfo, guardQueryResult);

			result.Add(templateNodeResultItem);
		}
	}
}
