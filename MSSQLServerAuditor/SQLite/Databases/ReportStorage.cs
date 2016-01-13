using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using log4net;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Tables;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Databases
{
	public class ReportStorage : Database
	{
		protected static readonly ILog                            Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		protected readonly Dictionary<NormalizeInfo, ReportTable> _tables;
		protected readonly object                                 _tablesLock;
		protected readonly ThreadLocal<HashAlgorithm>             _hashLocal;

		public ReportStorage(
			string fileName,
			bool   readOnly = false
		) : base(
				fileName,
				readOnly
			)
		{
			this._tables     = new Dictionary<NormalizeInfo, ReportTable>();
			this._tablesLock = new object();
			this._hashLocal  = new ThreadLocal<HashAlgorithm>(SHA1.Create);
		}

		internal Int64? SaveResults(long queryId, NormalizeInfo dbStructure, DataTable queryData)
		{
			const Int64 sessionId    = 1L;
			Int64?      affectedRows = null;

			if (queryData != null && queryData.Columns.Count > 0)
			{
				affectedRows = SaveQueryData(queryId, dbStructure, queryData, sessionId);
			}
			else
			{
				ClearData(queryId, dbStructure, sessionId);
			}

			return affectedRows;
		}

		protected void ClearData(long queryId, NormalizeInfo dbStructure, long sessionId)
		{
			string strQuery = string.Empty;

			using (this.Connection.OpenWrapper())
			{
				if (Common.Table.TableExists(this.Connection, dbStructure.TableName))
				{
					strQuery = string.Format(
						  " DELETE FROM"
						+ "    {0}"
						+ " WHERE"
						+ "    {1} = {2}"
						+ "    AND {3} = {4};",
						dbStructure.TableName.AsDbName(),
						QueryDirectory.TableName.AsFk(),
						queryId,
						MetaResultTable.SessionIdFieldName,
						sessionId
					);

					new SqlCustomCommand(this.Connection, strQuery)
						.Execute(100);
				}
			}
		}

		protected virtual long? SaveQueryData(
			long          queryId,
			NormalizeInfo dbStructure,
			DataTable     queryData,
			long          sessionId
		)
		{
			DataTable                 currentTable          = this.ReadResult(dbStructure, queryId, true);
			Dictionary<string, int>   currentDict           = new Dictionary<string, int>();
			Dictionary<string, int>   insertDict            = new Dictionary<string, int>();
			Dictionary<int, int>      rowOrderDict          = new Dictionary<int, int>();
			HashSet<string>           duplicateRows         = new HashSet<string>();
			HashSet<string>           updatedRows           = new HashSet<string>();
			List<ITableRow>           rows                  = new List<ITableRow>();
			int                       currentRowOrder       = 0;
			List<string>              rowsToDelete          = new List<string>();
			bool                      needInsertCommand     = false;
			ReportTable               table                 = null;
			long                      rowsUnchanged         = 0L;
			long                      rowsInserted          = 0L;
			long                      rowsUpdated           = 0L;
			long                      rowsDeleted           = 0L;
			long                      rowsDuplicatedDeleted = 0L;
			List<RowUpdateOrderInput> rowsToUpdateOrder     = new List<RowUpdateOrderInput>();
			List<RowUpdateInput>      rowsToUpdate          = new List<RowUpdateInput>();

			if (currentTable != null && currentTable.Rows != null)
			{
				for (int i = 0; i < currentTable.Rows.Count; i++)
				{
					DataRow row  = currentTable.Rows[i];
					string  hash = row[CommonRowFiller.RowHashFieldName].ToString();

					if (!currentDict.ContainsKey(hash))
					{
						currentDict.Add(hash, i);
					}
					else
					{
						duplicateRows.Add(hash);
					}
				}
			}

			this.PrepareTables(dbStructure);

			// Log.DebugFormat(
			//    @"SaveQueryData:BeginLock:QueryId:'{0}';sessionId:'{1}'",
			//    queryId,
			//    sessionId
			// );

			lock (this._tablesLock)
			{
				if (!this._tables.TryGetValue(dbStructure, out table))
				{
					Log.DebugFormat("SaveQueryData:EndLock(TryGetValue):QueryId:'{0}';sessionId:'{1}'",
						queryId,
						sessionId
					);

					return null;
				}
			}

			// Log.DebugFormat(
			//    @"SaveQueryData:EndLock:QueryId:'{0}';sessionId:'{1}'",
			//    queryId,
			//    sessionId
			// );

			int queryRowOrder = 0;

			foreach (DataRow queryRow in queryData.Rows)
			{
				string hash       = GetHashFromDataRow(queryRow);
				bool   hashExists = currentDict.ContainsKey(hash);

				if (hashExists && !duplicateRows.Contains(hash))
				{
					int storedOrder = currentDict[hash];

					if (storedOrder == queryRowOrder)
					{
						rowsUnchanged++;
					}
					else
					{
						rowsToUpdateOrder.Add(new RowUpdateOrderInput(currentRowOrder, hash));
					}

					updatedRows.Add(hash);
					currentDict.Remove(hash);

					currentRowOrder++;
				}
				else
				{
					bool needInsert = !insertDict.ContainsKey(hash) && !updatedRows.Contains(hash);

					if (needInsert)
					{
						insertDict.Add(hash, currentRowOrder);
						rowOrderDict.Add(currentRowOrder, queryRowOrder);
						currentRowOrder++;
					}
				}

				queryRowOrder++;
			}

			foreach (string hash in currentDict.Keys)
			{
				if (!updatedRows.Contains(hash) && !duplicateRows.Contains(hash))
				{
					rowsToDelete.Add(hash);
				}
			}

			// Log.DebugFormat(
			//    @"SaveQueryData:BeginLock:QueryId:'{0}';sessionId:'{1}'",
			//    queryId,
			//    sessionId
			// );

			lock (this._tablesLock)
			{
				foreach (KeyValuePair<string, int> keyPair in insertDict)
				{
					string    hash     = keyPair.Key;
					int       rowOrder = keyPair.Value;
					DataRow   row      = queryData.Rows[rowOrderDict[rowOrder]];
					ITableRow tableRow;

					this.ProcessRowForWrite(row, dbStructure, out tableRow, false);

					if (rowsToDelete.Count > 0)
					{
						string hashOld = rowsToDelete[0];

						CommonRowFiller.ModifyTableRow(tableRow, queryId, sessionId, rowOrder, hash);
						rowsToUpdate.Add(new RowUpdateInput(hashOld, tableRow));
						rowsToDelete.RemoveAt(0);
					}
					else
					{
						needInsertCommand = true;
						CommonRowFiller.ModifyTableRow(tableRow, queryId, sessionId, rowOrder, hash);
						rows.Add(tableRow);
					}
				}

				if (duplicateRows.Any())
				{
					Log.ErrorFormat("SaveQueryData. Duplicated rows found. QueryId:'{0}';sessionId:'{1}';count:{2}",
						queryId,
						sessionId,
						duplicateRows.Count
					);

					rowsDuplicatedDeleted += table.DeleteRows(queryId, sessionId, duplicateRows.ToList());
				}

				rowsUpdated += table.UpdateRowsOrderByHash(queryId, sessionId, rowsToUpdateOrder);
				rowsUpdated += table.UpdateRowsByHash(queryId, sessionId, rowsToUpdate);

				if (rowsToDelete.Count > 0)
				{
					rowsDeleted += table.DeleteRows(queryId, sessionId, rowsToDelete);
				}

				if (needInsertCommand)
				{
					rowsInserted += table.ReplaceRowsTrans(rows);
				}
			}

			// Log.DebugFormat(
			//    @"SaveQueryData:EndLock:QueryId:'{0}';sessionId:'{1}'",
			//    queryId,
			//    sessionId
			// );

			Log.DebugFormat("rowsUnchanged:{0};rowsInserted:{1};rowsUpdated:{2};rowsDeleted:{3};rowsDuplicatedDeleted:{4}",
				rowsUnchanged,
				rowsInserted,
				rowsUpdated,
				rowsDeleted,
				rowsDuplicatedDeleted
			);

			return rowsInserted + rowsUnchanged + rowsUpdated + rowsDuplicatedDeleted;
		}

		public DataTable ReadResult(NormalizeInfo dbStructure, long queryId, bool readServiceColumns = false)
		{
			const long                   sessionId       = 1L;
			string                       modSql          = null;
			string                       strSQL          = null;
			IEnumerable<SQLiteParameter> parameters      = null;
			DataTable                    dataTableResult = null;

			if (dbStructure != null)
			{
				Common.Table table = Common.Table.GetTable(
					this.Connection,
					dbStructure.TableName
				);

				if (table != null)
				{
					CommonRowFiller.GetQueryModifier(
						out modSql,
						out parameters,
						queryId,
						sessionId
					);

					strSQL = dbStructure.GetSelectQuery(modSql, readServiceColumns)
						+ " ORDER BY " + CommonRowFiller.RowOrderFieldName;

					ReadTableCommand command = new ReadTableCommand(
						this.Connection,
						strSQL,
						parameters.ToArray()
					);

					command.Execute(100);

					if (!readServiceColumns)
					{
						CommonRowFiller.RemoveServiceColumns(command.Result);
					}

					dataTableResult = command.Result;
				}
			}

			return dataTableResult;
		}

		protected Int64? ProcessRowForWrite(
			DataRow        row,
			NormalizeInfo  dbStructure,
			out ITableRow  tableRow,
			bool           writeToDb
		)
		{
			Int64?       id    = null;
			Common.Table table = this._tables[dbStructure];

			tableRow = new TableRow(table.TableDefinition);

			foreach (DataColumn column in row.Table.Columns)
			{
				string columnName = column.ColumnName.DeleteSpecChars();

				if (dbStructure.Fields.Any(field => field.Name == columnName))
				{
					tableRow.Values.Add(columnName, row[column.ColumnName]);
				}
			}

			foreach (NormalizeInfo childDirectory in dbStructure.ChildDirectories)
			{
				ITableRow tmpTableRow = null;
				long?     childId     = this.ProcessRowForWrite(row, childDirectory, out tmpTableRow, true);

				tableRow.Values.Add(childDirectory.GetAsFk(), childId);
			}

			if (writeToDb)
			{
				id = table.InsertOrUpdateRow(tableRow);
			}
			else
			{
				id = -1L;
			}

			return id;
		}

		public void PrepareTables(NormalizeInfo normalizeInfo, bool mainTable = true)
		{
			if (normalizeInfo != null)
			{
				TableDefinition tableDefinition = normalizeInfo.GetTableDefinition();

				// Log.DebugFormat("PrepareTables:BeginLock");

				lock (this._tablesLock)
				{
					if (!this._tables.ContainsKey(normalizeInfo))
					{
						if (mainTable)
						{
							CommonRowFiller.ModifyTableDefinition(tableDefinition);

							if (this.FileName.Equals("file::memory:"))
							{
								CommonRowFiller.ModifyTableDefinitionAddOptionField(
									tableDefinition,
									CommonRowFiller.RowHashFieldName + "Old",
									SqlDbType.NVarChar,
									true
								);
							}
						}

						if (tableDefinition.Fields.Any())
						{
							ReportTable table = new ReportTable(this.Connection, tableDefinition);

							table.UpdateScheme();

							this._tables.Add(normalizeInfo, table);
						}

						foreach (NormalizeInfo childDirectory in normalizeInfo.ChildDirectories)
						{
							this.PrepareTables(childDirectory, false);
						}
					}
				}

				// Log.DebugFormat("PrepareTables:EndLock");
			}
		}

		private byte[] GetHash(string inputString)
		{
			HashAlgorithm algorithm = this._hashLocal.Value;

			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
		}

		private string GetHashString(string inputString)
		{
			StringBuilder sb = new StringBuilder();

			foreach (byte b in GetHash(inputString))
			{
				sb.Append(b.ToString("X2"));
			}

			return sb.ToString();
		}

		protected String GetHashFromDataRow(DataRow row)
		{
			StringBuilder result = new StringBuilder();

			if (row != null)
			{
				foreach (var colValue in row.ItemArray)
				{
					result.Append(colValue).Append("_");
				}
			}

			return GetHashString(result.ToString());
		}

		public string GetFieldsString(NormalizeInfo tableStructure, string strAlias = "")
		{
			List<string> fields = new List<string>();

			if (this._tables[tableStructure].TableDefinition.Fields.Any())
			{
				if (!String.IsNullOrEmpty(strAlias))
				{
					strAlias = strAlias + ".";
				}

				foreach (var f in this._tables[tableStructure].TableDefinition.Fields)
				{
					//
					// "DateCreated" column value is populated with a "default" value
					//
					if (f.Key.CompareTo(TableDefinition.DateCreated) == 0)
					{
						continue;
					}

					//
					// "DateUpdated" column value is populated by after update trigger
					//
					if (f.Key.CompareTo(TableDefinition.DateUpdated) == 0)
					{
						continue;
					}

					fields.Add(strAlias + f.Key.AsDbName());
				}

				return fields.Join(",");
			}

			return String.Empty;
		}

		protected T ExecuteScalar<T>(
			string                          sql,
			IEnumerable<QueryParameterInfo> parameters = null,
			IEnumerable<ParameterValue>     parameterValues = null
		)
		{
			T commandResult;

			SqlScalarCommand scalarCommand = new SqlScalarCommand(
				this.Connection,
				sql,
				this.GetParameters(parameters, parameterValues)
			);

			lock (this._tablesLock)
			{
				scalarCommand.Execute(100);

				object sqlResult = scalarCommand.Result;

				commandResult = sqlResult == DBNull.Value
					? default(T)
					: (T) sqlResult;
			}

			return commandResult;
		}

		protected long ExecuteNonQuery(
			string                          sql,
			IEnumerable<QueryParameterInfo> parameters      = null,
			IEnumerable<ParameterValue>     parameterValues = null
		)
		{
			long commandResult;

			SqlCustomCommand customCommand = new SqlCustomCommand(
				this.Connection,
				sql,
				this.GetParameters(parameters, parameterValues)
			);

			// Log.DebugFormat("ExecuteNonQuery:BeginLock");

			lock (this._tablesLock)
			{
				commandResult = customCommand.Execute(100);
			}

			// Log.DebugFormat("ExecuteNonQuery:EndLock");

			return commandResult;
		}

		public SQLiteParameter[] GetParameters(
			IEnumerable<QueryParameterInfo> parameters,
			IEnumerable<ParameterValue>     parameterValues
		)
		{
			List<SQLiteParameter> result = new List<SQLiteParameter>();

			if (parameters != null)
			{
				foreach (QueryParameterInfo parameter in parameters)
				{
					result.Add(
						new SQLiteParameter
						{
							ParameterName = parameter.Name,
							IsNullable    = true,
							DbType        = parameter.Type.ToDbType(),
							Value         = parameter.GetDefaultValue()
						}
					);
				}
			}

			if (parameterValues != null)
			{
				foreach (ParameterValue value in parameterValues)
				{
					SQLiteParameter parameter =
						(from SQLiteParameter p in result
							where p.ParameterName == value.Name
							select p
						).FirstOrDefault();

					if (parameter != null)
					{
						parameter.Value = value.GetValue(parameter.DbType.ToSqlDbType());
					}
				}
			}

			return result.ToArray();
		}
	}
}
