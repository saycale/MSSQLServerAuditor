using System;
using System.Collections.Generic;
using System.Linq;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Tables
{
	public class MetaResultTable : CurrentStorageTable
	{
		public const string TableName               = "d_MetaResult";
		public const string TableIdentityField      = "d_MetaResult_id";
		public const string RequestIdFieldName      = "request_id";
		public const string RecordSetCountFieldName = "record_sets";
		public const string RowCountFieldName       = "rows";
		public const string ErrorIdFieldName        = "error_id";
		public const string ErrorCodeFieldName      = "error_code";
		public const string ErrorMessageFieldName   = "error_message";
		public const string SessionIdFieldName      = "session_id";

		private long?           maxRequestCache;
		private readonly object maxRequestLock;

		public MetaResultTable(CurrentStorage storage)
			: base(storage, CreateTableDefinition())
		{
			this.maxRequestCache = null;
			this.maxRequestLock  = new object();
		}

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddBigIntField(QueryDirectory.TableName.AsFk(),      true,  true)
				.AddBigIntField(QueryGroupDirectory.TableName.AsFk(), true,  false)
				.AddBigIntField(RequestIdFieldName,                   false, false)
				.AddBigIntField(SessionIdFieldName,                   true,  true)
				.AddBigIntField(RecordSetCountFieldName,              false, false)
				.AddBigIntField(RowCountFieldName,                    false, false)
				.AddNVarCharField(ErrorIdFieldName,                   false, false)
				.AddNVarCharField(ErrorCodeFieldName,                 false, false)
				.AddNVarCharField(ErrorMessageFieldName,              false, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public long GetMaxRequestId()
		{
			lock (this.maxRequestLock)
			{
				if (this.maxRequestCache == null)
				{
					string sql = string.Format(
						"SELECT MAX({0}) FROM [{1}]",
						RequestIdFieldName,
						TableName
					);

					using (this.Connection.OpenWrapper())
					{
						SqlScalarCommand command = new SqlScalarCommand(this.Connection, sql);

						command.Execute(100);

						object sqlResult = command.Result;

						if (sqlResult == DBNull.Value)
						{
							this.maxRequestCache = 0;
						}
						else
						{
							this.maxRequestCache = (Int64)sqlResult;
						}
					}
				}
				else
				{
					this.maxRequestCache++;
				}

				return this.maxRequestCache.Value;
			}
		}

		public ITableRow GetMetaRow(
			long                    requestId,
			long                    sessionId,
			DateTime                timestamp,
			QueryInstanceResultInfo queryInstanceResult,
			long?                   queryId,
			long?                   rowsSaved
		)
		{
			string    errorNumber    = null;
			string    errorCode      = null;
			string    errorMessage   = null;
			Int64     recordSetCount = 0L;
			ITableRow row            = this.NewRow();

			row.Values.Add(QueryDirectory.TableName.AsFk(),      queryId);
			row.Values.Add(QueryGroupDirectory.TableName.AsFk(), null);
			row.Values.Add(RequestIdFieldName,                   requestId);
			row.Values.Add(SessionIdFieldName,                   sessionId);
			row.Values.Add(TableDefinition.DateCreated,          timestamp);

			if (queryInstanceResult != null)
			{
				if (queryInstanceResult.ErrorInfo == null)
				{
					if (queryInstanceResult.DatabasesResult.ContainsKey("")) //group.Name))
					{
						QueryDatabaseResultInfo databaseResult = queryInstanceResult.DatabasesResult[""]; //group.Name];

						if (rowsSaved == null)
						{
							rowsSaved = databaseResult.DataTables != null
								? databaseResult.DataTables.Select(x => x.Rows).Sum(x => x.Count)
								: 0L;
						}

						recordSetCount = (databaseResult.DataTables == null)
							? 0L
							: databaseResult.DataTables.Length;

						if (databaseResult.ErrorInfo != null)
						{
							errorNumber  = databaseResult.ErrorInfo.Number;
							errorCode    = databaseResult.ErrorInfo.Code;
							errorMessage = databaseResult.ErrorInfo.Message;
						}
					}
				}
				else
				{
					errorNumber  = queryInstanceResult.ErrorInfo.Number;
					errorCode    = queryInstanceResult.ErrorInfo.Code;
					errorMessage = queryInstanceResult.ErrorInfo.Message;
					rowsSaved    = 0L;
				}
			}

			row.Values.Add(RowCountFieldName,       rowsSaved);
			row.Values.Add(RecordSetCountFieldName, recordSetCount);
			row.Values.Add(ErrorIdFieldName,        errorNumber);
			row.Values.Add(ErrorCodeFieldName,      errorCode);
			row.Values.Add(ErrorMessageFieldName,   errorMessage);

			return row;
		}

		public ITableRow GetLastForQuery(Int64 queryId)
		{
			Int64 sessionId = 1L;

			string clause = QueryDirectory.TableName.AsFk().AsSqlClausePair()
				+ " ORDER BY "
				+ "[" + TableDefinition.DateCreated + "]"
				+ " DESC";

			List<ITableRow> rows = this.GetRows(
				clause,
				new[]
				{
					SQLiteHelper.GetParameter(QueryDirectory.TableName.AsFk(), queryId),
					SQLiteHelper.GetParameter(SessionIdFieldName,              sessionId)
				},
				1
			);

			return rows.FirstOrDefault();
		}
	}
}
