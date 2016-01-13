using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using MSSQLServerAuditor.Model.Connections;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils;

using Field = System.Tuple<string, object>;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class DynamicConnectionDirectory : TableDirectory
	{
		internal const string          TableName             = "d_DynamicConnection";
		public   const string          TableIdentityField    = "d_DynamicConnection_id";
		private static readonly string ParentQueryIdFn       = (QueryDirectory.TableName + "_parent").AsFk();
		private static readonly string QueryIdFn             = QueryDirectory.TableName.AsFk();
		private const string           ConnectionNameField   = "ConnectionName";
		private const string           ConnectionTypeField   = "Type";
		private const string           IsOdbcConnectionField = "IsOdbc";

		public DynamicConnectionDirectory(
			CurrentStorage storage
		) : base(
				storage,
				CreateTableDefinition()
			)
		{
		}

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddBigIntField(ParentQueryIdFn,       true,  false)
				.AddBigIntField(QueryIdFn,             true,  false)
				.AddNVarCharField(ConnectionNameField, true,  false)
				.AddNVarCharField(ConnectionTypeField, true,  false)
				.AddBitField(IsOdbcConnectionField,    false, false)
				.AddDateCreateField()
				.AddDateUpdatedField(TableIdentityField);
		}

		protected override string IdentityField
		{
			get
			{
				return TableIdentityField;
			}
		}

		public void RemoveConnections(Int64 parentQueryId)
		{
			const string parentQueryIdParamName = "@parentQueryId";
			string       strSQLClause           = null;

			if (!this.ReadOnly)
			{
				using (this.Connection.OpenWrapper())
				{
					var deleteCommand = new RowDeleteCommand(this.Connection, this.TableDefinition);

					strSQLClause = string.Format(
						"[{0}] = {1}",
						ParentQueryIdFn,
						parentQueryIdParamName
					);

					var parentQueryIdParam = new SQLiteParameter(parentQueryIdParamName, DbType.Int64)
					{
						Value = parentQueryId
					};

					var parameters = new List<SQLiteParameter> { parentQueryIdParam };

					deleteCommand.SetCommandConstraints(strSQLClause, parameters);

					deleteCommand.Execute(100);
				}
			}
		}

		public void UpdateConnections(
			Int64                   parentQueryId,
			List<DynamicConnection> connections
		)
		{
			foreach (DynamicConnection dynamicConnection in connections)
			{
				UpdateConnection(parentQueryId, dynamicConnection);
			}
		}

		public void UpdateConnection(
			Int64             parentQueryId,
			DynamicConnection connection
		)
		{
			if (!this.ReadOnly)
			{
				var fields = new List<Field> {
					this.CreateField(ParentQueryIdFn, parentQueryId),
					this.CreateField(QueryIdFn, connection.QueryId),
					this.CreateField(ConnectionNameField, connection.Name),
					this.CreateField(ConnectionTypeField, connection.Type),
					this.CreateField(IsOdbcConnectionField, connection.IsOdbc)
				};

				var row = this.NewRow();

				foreach (var field in fields)
				{
					row.Values.Add(field.Item1, field.Item2);
				}

				this.InsertOrUpdateRow(row, this.BeforeRowUpdate, this.BeforeRowAdd);
			}
		}

		public IEnumerable<DynamicConnection> ReadConnections(Int64 parentQueryId)
		{
			List<ITableRow> rows = this.GetRows(ParentQueryIdFn + " = " + parentQueryId);

			return rows.Select(tableRow => new DynamicConnection(
				tableRow.GetValue<string>(ConnectionNameField),
				tableRow.GetValue<string>(ConnectionTypeField),
				tableRow.GetValue<bool>(IsOdbcConnectionField),
				tableRow.GetValue<Int64>(QueryIdFn)
			));
		}
	}
}
