using System.Collections.Generic;
using System.Data;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Tables;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Databases
{
	public class ReportMemoryStorage : ReportStorage
	{
		private ReportMemoryStorage _memoryStorage;
		private readonly bool       _isMemoryStorage;

		public ReportMemoryStorage(
			string fileName,
			bool   readOnly = false
		) : base(
				fileName,
				readOnly
			)
		{
			this._memoryStorage   = null;
			this._isMemoryStorage = fileName.Equals("file::memory:");
		}

		protected override long? SaveQueryData(
			long          queryId,
			NormalizeInfo dbStructure,
			DataTable     queryData,
			long          sessionId
		)
		{
			long  rowsInserted        = 0L;
			long  rowsUpdated         = 0L;
			long  rowsDeleted         = 0L;
			long  rowsUpdatedRowOrder = 0L;
			long  rowsTotal           = 0L;
			long? rowsInMemory        = 0L;

			this.PrepareTables(dbStructure);

			if (this._memoryStorage == null)
			{
				this._memoryStorage = new ReportMemoryStorage("file::memory:");
			}

			DataTable currentData = this.ReadResult(dbStructure, queryId, true);

			this._memoryStorage.ClearData(queryId, dbStructure, sessionId);

			rowsInMemory = this._memoryStorage.SaveMemoryQueryData(queryId, dbStructure, queryData, sessionId, currentData);

			this.AttachDatabase("file:memdb1?mode=memory&cache=shared", "temp_report");

			string rowHashF    = (CommonRowFiller.RowHashFieldName).AsDbName();
			string rowHashOldF = (CommonRowFiller.RowHashFieldName + "Old").AsDbName();
			string queryIdF    = QueryDirectory.TableName.AsFk().AsDbName();
			string sessionIdF  = MetaResultTable.SessionIdFieldName.AsDbName();

			string masterFieldsList = this.GetFieldsString(dbStructure, string.Empty);

			// Log.DebugFormat("masterFieldsList:'{0}'",
			//    masterFieldsList
			// );

			string memoryFieldsList = this._memoryStorage.GetFieldsString(dbStructure, "tR");

			// Log.DebugFormat("memoryFieldsList:'{0}'",
			//    memoryFieldsList
			// );

			memoryFieldsList = memoryFieldsList.Replace(",tR." + rowHashOldF, string.Empty);

			// Log.DebugFormat("memoryFieldsList:'{0}'",
			//    memoryFieldsList
			// );

			string insertMemoryFieldsList = memoryFieldsList;

			// restore key fields
			memoryFieldsList = memoryFieldsList.Replace(",tR." + rowHashF,   ",dest." + rowHashF);
			memoryFieldsList = memoryFieldsList.Replace(",tR." + queryIdF,   ",dest." + queryIdF);
			memoryFieldsList = memoryFieldsList.Replace(",tR." + sessionIdF, ",dest." + sessionIdF);

			// remove date fields
			// memoryFieldsList = memoryFieldsList.Replace(",temp_report." + dbStructure.TableName + "." + TableDefinition.DateCreated.AsDbName(), "");
			// memoryFieldsList = memoryFieldsList.Replace(",temp_report." + dbStructure.TableName + "." + TableDefinition.DateUpdated.AsDbName(), "");

			// masterFieldsList = masterFieldsList.Replace("," + TableDefinition.DateCreated.AsDbName(), "");
			// masterFieldsList = masterFieldsList.Replace("," + TableDefinition.DateUpdated.AsDbName(), "");

			string deleteQuery = string.Format(
				  " DELETE FROM {0}"
				+ " WHERE"
				+ "    {1} NOT IN ("
				+ "       SELECT"
				+ "          tR.{2}"
				+ "       FROM"
				+ "          [temp_report].{0} tR"
				+ "       WHERE"
				+ "          tR.{3} = {4}"
				+ "          AND tR.{5} = {6}"
				+ "    )"
				+ "    AND {3} = {4}"
				+ "    AND {5} = {6}"
				+ ";",
				dbStructure.TableName.AsDbName(),
				rowHashF,
				rowHashOldF,
				queryIdF,
				queryId,
				sessionIdF,
				sessionId
			);

			// Log.DebugFormat("deleteStr:'{0}'",
			//    deleteStr
			// );

			string updateQuery = string.Format(
				  " REPLACE INTO {0}"
				+ " ("
				+ "    {1}"
				+ " )"
				+ " SELECT"
				+ "    {2}"
				+ " FROM"
				+ "    [temp_report].{0} tR"
				+ "    INNER JOIN {0} dest ON"
				+ "       tR.{3} = dest.{4}"
				+ "       AND tR.{4} != dest.{4}"
				+ " WHERE"
				+ "    tR.{5} = {6}"
				+ "    AND tR.{7} = {8}"
				+ " ;",
				dbStructure.TableName.AsDbName(),
				masterFieldsList,
				memoryFieldsList,
				rowHashOldF,
				rowHashF,
				queryIdF,
				queryId,
				sessionIdF,
				sessionId
			);

			// Log.DebugFormat("updateStr:'{0}'",
			//    updateStr
			// );

			string insertQuery = string.Format(
				  " INSERT INTO {0}"
				+ " ("
				+ "    {1}"
				+ " )"
				+ " SELECT"
				+ "    {2}"
				+ " FROM"
				+ "    [temp_report].{0} tR"
				+ " WHERE"
				+ "    tR.{3} = -1"
				+ "    AND tR.{4} = {5}"
				+ "    AND tR.{6} = {7}"
				+ "    AND tR.{8} NOT IN ("
				+ "       SELECT"
				+ "          {8}"
				+ "       FROM"
				+ "          {0}"
				+ "       WHERE"
				+ "          {4} = {5}"
				+ "          AND {6} = {7}"
				+ "    )"
				+ ";",
				dbStructure.TableName.AsDbName(),
				masterFieldsList,
				insertMemoryFieldsList,
				rowHashOldF,
				queryIdF,
				queryId,
				sessionIdF,
				sessionId,
				rowHashF
			);

			// Log.DebugFormat("insertStr:'{0}'",
			//    insertStr
			// );

			string updateRowOrderQuery = string.Format(
				  " UPDATE {0} SET"
				+ "    {1} = ("
				+ "       SELECT"
				+ "          tR.{1}"
				+ "       FROM"
				+ "          [temp_report].{0} tR"
				+ "       WHERE"
				+ "          tR.{2} = {3}"
				+ "          AND tR.{4} = {5}"
				+ "          AND tR.{6} = {0}.{6}"
				+ "          AND tR.{1} != {0}.{1}"
				+ "    )"
				+ " WHERE"
				+ "    EXISTS ("
				+ "       SELECT"
				+ "          1"
				+ "       FROM"
				+ "          [temp_report].{0} tR"
				+ "       WHERE"
				+ "          tR.{2} = {3}"
				+ "          AND tR.{4} = {5}"
				+ "          AND tR.{6} = {0}.{6}"
				+ "          AND tR.{1} != {0}.{1}"
				+ "    )"
				+ ";",
				dbStructure.TableName.AsDbName(),
				CommonRowFiller.RowOrderFieldName.AsDbName(),
				queryIdF,
				queryId,
				sessionIdF,
				sessionId,
				rowHashF
			);

			// Log.DebugFormat("strUpdateRowOrder:'{0}'",
			//    strUpdateRowOrder
			// );

			string countRowsQuery = string.Format(
				  " SELECT"
				+ "    COUNT(*)"
				+ " FROM"
				+ "    {0}"
				+ " WHERE"
				+ "    {1} = {2}"
				+ "    AND {3} = {4};",
				dbStructure.TableName.AsDbName(),
				queryIdF,
				queryId,
				sessionIdF,
				sessionId
			);

			// Log.DebugFormat("countRowsQuery:'{0}'",
			//    countRowsQuery
			// );

			// operations
			rowsDeleted         = this.ExecuteNonQuery(deleteQuery);
			rowsUpdated         = this.ExecuteNonQuery(updateQuery);
			rowsInserted        = this.ExecuteNonQuery(insertQuery);
			rowsUpdatedRowOrder = this.ExecuteNonQuery(updateRowOrderQuery);
			rowsTotal           = this.ExecuteScalar<long>(countRowsQuery);

			Log.DebugFormat(
				@"rowsInserted:{0};rowsUpdated:{1};rowsDeleted:{2};rowsUpdatedRowOrder:{3};totalRows:{4}",
				rowsInserted,
				rowsUpdated,
				rowsDeleted,
				rowsUpdatedRowOrder,
				rowsTotal
			);

			if (rowsTotal != rowsInMemory)
			{
				Log.ErrorFormat(
					@"Rows added to in-memory table:'{0}'. Rows formed by merge:'{1}'.",
					rowsInMemory,
					rowsTotal
				);
			}

			return rowsTotal;
		}

		private long? SaveMemoryQueryData(
			long          queryId,
			NormalizeInfo dbStructure,
			DataTable     queryData,
			long          sessionId,
			DataTable     currentTable
		)
		{
			if (!this._isMemoryStorage)
			{
				return 0L;
			}

			ReportTable               table                 = null;
			Dictionary<int, int>      rowOrderDict          = new Dictionary<int, int>();
			Dictionary<string, int>   currentDict           = new Dictionary<string, int>();
			Dictionary<string, int>   insertDict            = new Dictionary<string, int>();
			Dictionary<string, int>   updateDict            = new Dictionary<string, int>();
			HashSet<string>           duplicateRows         = new HashSet<string>();
			HashSet<string>           updatedRows           = new HashSet<string>();
			//List<RowUpdateInput>      rowsToUpdate          = new List<RowUpdateInput>();
			//List<RowUpdateOrderInput> rowsToUpdateOrder     = new List<RowUpdateOrderInput>();
			List<ITableRow>           rows                  = new List<ITableRow>();
			List<ITableRow>           rowsToUpdateList      = new List<ITableRow>();
			List<string>              rowsToDelete          = new List<string>();
			bool                      needInsertCommand     = false;
			int                       currentRowOrder       = 0;
			int                       queryRowOrder         = 0;
			//long                      rowsDeleted           = 0L;
			//long                      rowsDuplicatedDeleted = 0L;
			long                      rowsInserted          = 0L;
			//long                      rowsUnchanged         = 0L;
			long                      rowsUpdated           = 0L;

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

			foreach (DataRow queryRow in queryData.Rows)
			{
				string hash       = GetHashFromDataRow(queryRow);

				bool   hashExists = currentDict.ContainsKey(hash);

				if (hashExists && !duplicateRows.Contains(hash))
				{
					updateDict.Add(hash, currentRowOrder);
					updatedRows.Add(hash);
					currentDict.Remove(hash);
					rowOrderDict.Add(currentRowOrder, queryRowOrder);
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

					if (rowsToDelete.Count > 0L)
					{
						string hashOld = rowsToDelete[0];

						CommonRowFiller.ModifyTableRow(tableRow, queryId, sessionId, rowOrder, hash);

						tableRow.Values[CommonRowFiller.RowHashFieldName + "Old"] = hashOld;

						rowsToUpdateList.Add(tableRow);

						rowsToDelete.RemoveAt(0);
					}
					else
					{
						needInsertCommand = true;

						CommonRowFiller.ModifyTableRow(tableRow, queryId, sessionId, rowOrder, hash);

						tableRow.Values[CommonRowFiller.RowHashFieldName + "Old"] = -1;

						rows.Add(tableRow);
					}
				}

				foreach (KeyValuePair<string, int> keyPair in updateDict)
				{
					string    hash     = keyPair.Key;
					int       rowOrder = keyPair.Value;
					DataRow   row      = queryData.Rows[rowOrderDict[rowOrder]];
					ITableRow tableRow;

					this.ProcessRowForWrite(row, dbStructure, out tableRow, false);

					CommonRowFiller.ModifyTableRow(tableRow, queryId, sessionId, rowOrder, hash);

					tableRow.Values[CommonRowFiller.RowHashFieldName + "Old"] = hash;

					rowsToUpdateList.Add(tableRow);
				}

				rowsUpdated += table.ReplaceRowsTrans(rowsToUpdateList);

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

			Log.DebugFormat("rowsInserted:{0};rowsUpdated:{1}",
				rowsInserted,
				rowsUpdated
			);

			return rowsInserted + rowsUpdated;
		}
	}
}
