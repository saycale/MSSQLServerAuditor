using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Tables;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Common
{
	public class CommonRowFiller
	{
		public const string RowOrderFieldName = "row_order";
		public const string RowHashFieldName  = "row_hash";

		public static void ModifyTableRow(ITableRow row, Int64 queryId, Int64 sessionId, Int64 rowOrder, string rowhash)
		{
			row.Values.Add(QueryDirectory.TableName.AsFk(),    queryId);
			row.Values.Add(MetaResultTable.SessionIdFieldName, sessionId);
			row.Values.Add(RowOrderFieldName,                  rowOrder);

			row.Values[RowHashFieldName] = rowhash;
		}

		public static void ModifyTableDefinition(TableDefinition tableDefinition)
		{
			tableDefinition
				.AddBigIntField(QueryDirectory.TableName.AsFk(),    false, true)
				.AddBigIntField(MetaResultTable.SessionIdFieldName, false, true)
				.AddNVarCharField(RowHashFieldName,                 false, true)
				.AddBigIntField(RowOrderFieldName,                  false, false)
				.AddDateCreateField()                                            // datetime the record is created
				.AddDateUpdatedField()                                           // datetime the record is updated
				.SetCompoundPrimaryKey(new List<string>
				{
					QueryDirectory.TableName.AsFk(),
					MetaResultTable.SessionIdFieldName,
					RowHashFieldName
				});

			// _tableDefinition.Indexes.Add(new IndexDefinition(
			//    _tableDefinition,
			//    "idx_" + _tableDefinition.Name + "_Query_Session",
			//    false,
			//    QueryDirectory.TableName.AsFk(),
			//    MetaResultTable.SessionIdFieldName
			// ));
		}

		public static void ModifyTableDefinitionAddOptionField(
			TableDefinition tableDefinition,
			string          optFieldName,
			SqlDbType       sqlType,
			bool            addFieldIndex
		)
		{
			tableDefinition.AddField(
				new FieldDefinition(
					optFieldName,
					sqlType,
					false,
					false
				)
			);

			if (addFieldIndex)
			{
				tableDefinition.Indexes.Add(
					new IndexDefinition(
						tableDefinition,
						optFieldName + "_indx",
						false,
						optFieldName
					)
				);
			}
		}

		public static string GetIdentClause()
		{
			string clause = String.Format(
				"{0} AND {1}",
				MetaResultTable.SessionIdFieldName.AsSqlClausePair(),
				QueryDirectory.TableName.AsFk().AsSqlClausePair()
			);

			return clause;
		}

		public static List<SQLiteParameter> GetIdentParameters(Int64 queryId, long sessionId)
		{
			List<SQLiteParameter> parameters = new List<SQLiteParameter>();
			SQLiteParameter       parameter  = null;

			// add Fk
			parameter = new SQLiteParameter(QueryDirectory.TableName.AsFk().AsParamName(), DbType.Int64);
			parameter.Value = queryId;
			parameters.Add(parameter);

			// add SessionId
			parameter = new SQLiteParameter(MetaResultTable.SessionIdFieldName.AsParamName(), DbType.Int64);
			parameter.Value = sessionId;
			parameters.Add(parameter);

			return parameters;
		}

		public static void GetQueryModifier(
			out string                       modSql,
			out IEnumerable<SQLiteParameter> parameters,
			Int64                            queryId,
			Int64                            sessionId
		)
		{
			modSql     = GetIdentClause();
			parameters = GetIdentParameters(queryId, sessionId);
		}

		public static void RemoveServiceColumns(DataTable dataTable)
		{
			List<string> listColumnsToRemove = new List<string>()
			{
				// ticket #400
				QueryDirectory.TableName.AsFk(),
				MetaResultTable.SessionIdFieldName,
				TableDefinition.DateCreated,
				TableDefinition.DateUpdated,
				RowOrderFieldName,
				RowHashFieldName,
				RowHashFieldName + "Old"
			};

			RemovePrimaryKey(dataTable);

			foreach (string strColumnName in listColumnsToRemove)
			{
				if (dataTable.Columns.Contains(strColumnName))
				{
					dataTable.Columns.Remove(strColumnName);
				}
			}
		}

		private static void RemovePrimaryKey(DataTable dataTable)
		{
			dataTable.PrimaryKey = null;

			foreach (DataColumn column in dataTable.Columns)
			{
				column.Unique = false;
			}
		}
	}
}
