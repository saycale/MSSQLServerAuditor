using System;
using System.Collections.Generic;
using System.Linq;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;

using Field = System.Tuple<string, object>;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public abstract class QueryParameterDirectoryBase : TableDirectory
	{
		public static readonly string ValueFn     = "ParameterValue";
		public static readonly string IsEnabledFn = "IsParameterEnabled";

		protected abstract string QueryIdFn     { get; }
		protected abstract string ParameterIdFn { get; }

		protected abstract string GetTableName();

		protected QueryParameterDirectoryBase(
			CurrentStorage   storage,
			TableDefinition  tableDefinition
		) : base(storage, tableDefinition)
		{
		}

		public void Update(
			ConnectionGroupInfo   connectionGroup,
			Int64                 queryId,
			TemplateNodeQueryInfo query,
			List<ParameterValue>  newValues
		)
		{
			Dictionary<long, ITableRow> existingRows = new Dictionary<Int64, ITableRow>();

			foreach (ITableRow row in this.GetRows(QueryIdFn + " = " + queryId))
			{
				existingRows[(Int64) row.Values[ParameterIdFn]] = row;
			}

			foreach (var value in newValues)
			{
				Int64? parameterId = GetParameterId(connectionGroup, query, value.Name);

				if (parameterId != null)
				{
					existingRows.Remove((Int64)parameterId);
				}

				if (!this.ReadOnly)
				{
					List<Field> fields = new List<Field> {
						this.CreateField(ValueFn,       value.UserValue),
						this.CreateField(IsEnabledFn,   true),
						this.CreateField(QueryIdFn,     queryId),
						this.CreateField(ParameterIdFn, parameterId)
					};

					ITableRow row = this.NewRow();

					foreach (Field field in fields)
					{
						if (field != null)
						{
							row.Values.Add(field.Item1, field.Item2);
						}
					}

					this.InsertOrUpdateRow(row, this.BeforeRowUpdate, this.BeforeRowAdd);
				}
			}

			foreach (KeyValuePair<long, ITableRow> absent in existingRows)
			{
				string sqlQuery = string.Format(
					"UPDATE {0} SET {1} = {2} WHERE [{4}] = {3} AND {1} != {2};",
					GetTableName(),
					IsEnabledFn,
					SqLiteBool.ToBit(false),
					absent.Key,
					IdentityField
				);

				new SqlCustomCommand(this.Connection, sqlQuery)
					.Execute(100);
			}
		}

		public void ReadParameter(
			ConnectionGroupInfo   connectionGroup,
			Int64                 queryId,
			TemplateNodeQueryInfo query,
			ParameterValue        paramValue
		)
		{
			Int64? parameterId = GetParameterId(connectionGroup, query, paramValue.Name);

			if (parameterId != null)
			{
				List<ITableRow> rows = this.GetRows(
					QueryIdFn + " = " + queryId + " AND " + ParameterIdFn + " = " + parameterId
				);

				if (rows != null)
				{
					if (rows.Count != 0)
					{
						ITableRow paramRow = rows.ElementAt(0);

						paramValue.UserValue = paramRow.GetValue<string>(ValueFn);
					}
				}
			}
		}

		protected abstract Int64? GetParameterId(
			ConnectionGroupInfo   connectionGroup,
			TemplateNodeQueryInfo query,
			string                parameterName
		);
	}
}
