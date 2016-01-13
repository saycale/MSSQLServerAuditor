using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Common
{
	////////////////////////////////////////////////////////////////////////////////////////////////
	// TODO: connection used not correct. Replace this.Connection on wrapper.Connection.
	// See UpdateRowOrderByHash method for example
	////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Table in SQLite DN
	/// </summary>
	public class Table : TableBase
	{
		protected const string DefaultIdentityField = "rowid";

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="tableDefinition">Table definition</param>
		public Table(
			SQLiteConnection connection,
			TableDefinition  tableDefinition
		) : base(
				connection,
				tableDefinition
			)
		{
		}

		public static bool TableExists(
			SQLiteConnection connection,
			string           tableName
		)
		{
			bool   boolIsTableExists = false;
			string query             = null;

			if (tableName != null)
			{
				using (connection.OpenWrapper())
				{
					query = string.Format(
						"SELECT COUNT(*) FROM [sqlite_master] m WHERE m.[type] = 'table' AND m.[name] = '{0}'",
						tableName
					);

					SqlScalarCommand command = new SqlScalarCommand(
						connection,
						query
					);

					command.Execute(100);

					if ((long)command.Result > 0L)
					{
						boolIsTableExists = true;
					}
				}
			}

			return boolIsTableExists;
		}

		public static Table GetTable(SQLiteConnection connection, string name)
		{
			using (connection.OpenWrapper())
			{
				GetTableDefinitionCommand reader = new GetTableDefinitionCommand(connection, name);

				reader.Execute(100);

				TableDefinition def = reader.TableDefinition;

				return def != null
					? new Table(connection, def)
					: null;
			}
		}

		/// <summary>
		/// Add rows to table
		/// </summary>
		/// <param name="rows">Row collection</param>
		/// <returns></returns>
		public Int64? AddRows(IEnumerable<ITableRow> rows)
		{
			Int64?    lastRowIdentity = null;
			ITableRow lastTableRow    = null;

			using (this.Connection.OpenWrapper())
			{
				RowInsertCommand insertCommand = new RowInsertCommand(this.Connection, this.TableDefinition);

				foreach (ITableRow row in rows)
				{
					insertCommand.AddRowForInserting(row);

					lastTableRow = row;
				}

				insertCommand.Execute(100);

				if (lastTableRow != null)
				{
					lastRowIdentity = this.GetRow(lastTableRow);
				}

				return lastRowIdentity;
			}
		}

		/// <summary>
		/// Add row to table
		/// </summary>
		/// <param name="row">Row to add</param>
		/// <returns></returns>
		public Int64? AddRow(ITableRow row)
		{
			return this.AddRows(new[] { row });
		}

		/// <summary>
		/// Get rows from table
		/// </summary>
		/// <param name="clause">SQL clause</param>
		/// <param name="parameters">SQL parameters</param>
		/// <param name="limit">Max row count to read</param>
		/// <param name="sqLiteConnection"></param>
		/// <returns></returns>
		public List<ITableRow> GetRows(string clause, IEnumerable<SQLiteParameter> parameters = null, int? limit = null)
		{
			string sql = string.Format(
				"SELECT {0} FROM {1}{2}{3};",
				GetAllFieldsString(),
				this.TableDefinition.Name.AsDbName(),
				(clause == null)
					? String.Empty
					: " WHERE " + clause,
				(limit != null)
					? " LIMIT " + limit
					: String.Empty
			);

			using (this.Connection.OpenWrapper())
			{
				SelectTableCommand commandSelect = new SelectTableCommand(
					this.Connection,
					this.TableDefinition,
					sql,
					parameters
				);

				commandSelect.Execute(100);

				return commandSelect.Result;
			}
		}

		protected virtual string IdentityField
		{
			get { return DefaultIdentityField; }
		}

		/// <summary>
		/// Get rows from table
		/// </summary>
		/// <param name="clause">SQL clause</param>
		/// <param name="parameters">SQL parameters</param>
		/// <param name="limit">Max row count to read</param>
		/// <param name="sqLiteConnection"></param>
		/// <returns></returns>
		private Int64? GetRowIdentity(string strQueryWhere, IEnumerable<SQLiteParameter> parameters)
		{
			Int64? identity = null;

			string sql = string.Format(
				"SELECT [{0}] FROM {1} WHERE {2} LIMIT 1;",
				IdentityField,
				this.TableDefinition.Name.AsDbName(),
				strQueryWhere
			);

			using (this.Connection.OpenWrapper())
			{
				SelectTableCommand commandSelect = new SelectTableCommand(
					this.Connection,
					this.TableDefinition,
					sql,
					parameters
				);

				commandSelect.Execute(100);

				if (commandSelect.Result != null)
				{
					if (commandSelect.Result.Count > 0L)
					{
						identity = (long?) commandSelect.Result.First().Values[IdentityField];
					}
				}
			}

			return identity;
		}

		/// <summary>
		/// Get row id of row according values
		/// </summary>
		/// <param name="row">Row to search</param>
		/// <returns></returns>
		protected Int64? GetRow(ITableRow row)
		{
			string                clause     = null;
			List<SQLiteParameter> parameters = null;

			this.GetQueryDataForRow(row, out clause, out parameters);

			return this.GetRowIdentity(clause, parameters);
		}

		/// <summary>
		/// Get row by row id
		/// </summary>
		/// <param name="identity">Row identity</param>
		/// <returns></returns>
		public ITableRow GetRowByIdentity(Int64? identity)
		{
			string sql = string.Format(
				"SELECT {0} FROM {1} WHERE [{2}] = {3}",
				GetAllFieldsString(),
				this.TableDefinition.Name.AsDbName(),
				IdentityField,
				identity
			);

			using (this.Connection.OpenWrapper())
			{
				SelectTableCommand select = new SelectTableCommand(
					this.Connection,
					this.TableDefinition,
					sql,
					null
				);

				select.Execute(100);

				return select.Result.FirstOrDefault();
			}
		}

		public Int64? InsertOrUpdateRowForSure(ITableRow row)
		{
			//
			// without r => { } the records can't be updated
			//
			return this.InsertOrUpdateRow(row, r => { });
		}

		/// <summary>
		/// Find or create row according values
		/// </summary>
		/// <param name="row">Row to search or create</param>
		/// <param name="beforeRowUpdate">Code to be executed before row update. If null no update executes.</param>
		/// <param name="beforeRowAdd">Code to be executed before row insert</param>
		/// <returns></returns>
		public Int64? InsertOrUpdateRow(
			ITableRow         row,
			Action<ITableRow> beforeRowUpdate = null,
			Action<ITableRow> beforeRowAdd = null
		)
		{
			Int64?                identity   = null;
			string                clause     = null;
			List<SQLiteParameter> parameters = null;
			List<ITableRow>       result     = null;

			this.GetQueryDataForRow(row, out clause, out parameters);

			using (this.Connection.OpenWrapper())
			{
				result = this.GetRows(clause, parameters, 1);

				if (result != null && result.Count > 0)
				{
					// Log.DebugFormat("Record exists(1):Count:'{0}'",
					//    result.Count
					// );

					ITableRow existRow = result.First();

					if (beforeRowUpdate != null)
					{
						beforeRowUpdate(row);
					}

					if (row.CopyValues(existRow))
					{
						this.UpdateRow(existRow);
					}

					identity = (long?) existRow.Values[IdentityField];
				}
				else
				{
					// Log.Debug("Record is not exists");

					lock (this.GetLockObject())
					{
						//
						// try to get the record again as the record can be already added by the differect thread
						//
						result = this.GetRows(clause, parameters, 1);

						if (result != null && result.Count > 0)
						{
							// Log.DebugFormat("Record exists(2):Count:'{0}'",
							//    result.Count
							// );

							ITableRow existRow = result.First();

							if (beforeRowUpdate != null)
							{
								beforeRowUpdate(row);

								if (row.CopyValues(existRow))
								{
									this.UpdateRow(existRow);
								}
							}

							identity = (long?) existRow.Values[IdentityField];
						}
						else
						{
							if (beforeRowAdd != null)
							{
								beforeRowAdd(row);
							}

							identity = this.AddRow(row);
						}
					}
				}
			}

			return identity;
		}

		/// <summary>
		/// Update one row by row identity
		/// </summary>
		/// <param name="row">Row to update</param>
		public void UpdateRow(ITableRow row)
		{
			this.UpdateRows(new[] { row });
		}

		/// <summary>
		/// Update table: row by row ids
		/// </summary>
		/// <param name="rows">Rows to update</param>
		public void UpdateRows(IEnumerable<ITableRow> rows)
		{
			using (this.Connection.OpenWrapper())
			{
				RowUpdateCommand updateCommand = new RowUpdateCommand(
					this.Connection,
					this.TableDefinition,
					this.IdentityField
				);

				foreach (ITableRow row in rows)
				{
					updateCommand.AddRowForUpdating(row);
				}

				updateCommand.Execute(100);
			}
		}

		/// <summary>
		/// Replace rows
		/// </summary>
		/// <param name="rows">Rows to be replaces</param>
		public Int64 ReplaceRows(IEnumerable<ITableRow> rows)
		{
			Int64 result = 0L;

			using (this.Connection.OpenWrapper())
			{
				ReplaceCommand replaceCommand = new ReplaceCommand(
					this.Connection,
					this.TableDefinition
				);

				foreach (ITableRow row in rows)
				{
					replaceCommand.AddRowForReplacing(row);
				}

				result = replaceCommand.Execute(100);
			}

			return result;
		}

		/// <summary>
		/// Upgrade table accroding definition
		/// </summary>
		/// <param name="tableDefinition">New table definition</param>
		/// <returns></returns>
		public void UpdateScheme()
		{
			int intColumnIndex = 0;
			bool forceUpgrade = false;

			// var forceUpgrade = ServiceInfoTable.ReadDbVesion(this.Connection).MinorChangedOver(DatabaseVersion.Current);
			// Log.DebugFormat("forceUpgrade:'{0}'", forceUpgrade);

			lock (this._globalLock)
			{
				using (this.Connection.OpenWrapper())
				{
					// Log.DebugFormat(
					//    @"this.TableDefinition.Name:'{0}'",
					//    this.TableDefinition.Name
					// );

					Table existingTable = GetTable(
						this.Connection,
						this.TableDefinition.Name
					);

					// Log.DebugFormat(
					//    @"Connection:'{0}';ExistingTable:'{1}'",
					//    this.Connection.DataSource,
					//    existingTable.TableDefinition.Name
					// );

					if (existingTable == null)
					{
						this.CreateTable();

						return;
					}

					IEnumerable<FieldDefinition> fields = QueryDefinition.GetMinimumDefinitions(
						(definition, fieldDefinition) => definition.Equals(fieldDefinition),
						existingTable.TableDefinition,
						this.TableDefinition
					);

					// Log.DebugFormat(
					//    @"this.TableDefinition.Fields.Count:'{0}';Existing fields.Count:'{1}'",
					//    this.TableDefinition.Fields.Count,
					//    fields.Count()
					// );

					intColumnIndex = 0;

					foreach (FieldDefinition fieldInTableDefinition in this.TableDefinition.Fields.Values)
					{
						// Log.DebugFormat(
						//    @"index:{0};Column:'{1}'",
						//    intColumnIndex,
						//    fieldInTableDefinition.Name
						// );

						intColumnIndex++;
					}

					intColumnIndex = 0;

					foreach (FieldDefinition fieldInExistingTable in fields)
					{
						// Log.DebugFormat(
						//    @"index:{0};Column:'{1}'",
						//    intColumnIndex,
						//    fieldInExistingTable.Name
						// );

						intColumnIndex++;
					}

					if (forceUpgrade || fields.Count() < this.TableDefinition.Fields.Count)
					{
						string tmpName = existingTable.TableDefinition.Name + "_tmp";
						Table tmpExisting = GetTable(this.Connection, tmpName);

						if (tmpExisting != null)
						{
							tmpExisting.Drop();
						}

						existingTable.Rename(tmpName);

						this.CreateTable();

						existingTable.Copy(this);

						existingTable.Drop();
					}
				}
			}
		}

		private string GetAllFieldsString()
		{
			string fields = IdentityField.Equals(DefaultIdentityField, StringComparison.InvariantCultureIgnoreCase)
				? "*, " + DefaultIdentityField
				: "*";

			return fields;
		}

		/// <summary>
		/// Prepare the query to the table data
		/// </summary>
		/// <param name="row">table row</param>
		/// <returns></returns>
		private void GetQueryDataForRow(ITableRow row, out string clause, out List<SQLiteParameter> parameters)
		{
			List<string> clauses = new List<string>();

			parameters = new List<SQLiteParameter>();

			foreach (FieldDefinition field in this.TableDefinition.Fields.Values)
			{
				if (field.Unique)
				{
					if (row.Values[field.Name] != null)
					{
						clauses.Add(field.Name.AsSqlClausePair());

						SQLiteParameter param = new SQLiteParameter(field.Name.AsParamName(), field.SqlType.ToDbType())
						{
							Value = row.Values[field.Name] ?? DBNull.Value
						};

						parameters.Add(param);
					}
					else
					{
						clauses.Add(string.Format("{0} IS NULL", field.Name));
					}
				}
			}

			// the code bellow is needed to support the calculation of the "clause" based on unique
			// indexes composed from 2 or more fields
			if (clauses.Count == 0)
			{
				foreach (IndexDefinition indDef in this.TableDefinition.Indexes)
				{
					if (indDef.IsComposedUnique)
					{
						foreach (string fieldName in indDef.FieldNames)
						{
							FieldDefinition field = this.TableDefinition.Fields[fieldName];

							if (row.Values[field.Name] != null)
							{
								clauses.Add(field.Name.AsSqlClausePair());

								SQLiteParameter param = new SQLiteParameter(field.Name.AsParamName(), field.SqlType.ToDbType())
								{
									Value = row.Values[field.Name] ?? DBNull.Value
								};

								parameters.Add(param);
							}
							else
							{
								clauses.Add(string.Format("{0} IS NULL", field.Name));
							}
						}
					}
				}
			}

			clause = clauses.Join(" AND ");
		}
	}
}
