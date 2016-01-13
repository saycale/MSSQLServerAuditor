using System.Collections.Generic;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.BusinessLogic
{
	public class TemplateManager
	{
		private readonly CurrentStorage _storage;

		public TemplateManager(CurrentStorage storage)
		{
			this._storage = storage;
		}

		public List<TemplateRow> GetTemplates(
			string groupName,
			string instanceName,
			string connectionType
		)
		{
			string sqlQuery = string.Format(
				@"SELECT DISTINCT
					 {0}.[{20}]
					,{0}.{6}
					,{0}.{7}
					,{0}.{8}
				FROM
					{2}
					INNER JOIN {1} ON
						{2}.[{21}] = {1}.{9}
					INNER JOIN {3} ON
						{1}.[{22}] = {3}.{10}
					INNER JOIN {4} ON
						{4}.[{23}] = {3}.{11}
					INNER JOIN {5} ON
						{5}.[{24}] = {4}.{12}
					INNER JOIN {0} ON
						{0}.[{20}] = {5}.{13}
				WHERE
					{2}.{14} = '{17}'
					AND {1}.{15} = '{18}'
					AND {1}.{16} = '{19}'",
				TemplateDirectory.TableName,
				ServerInstanceDirectory.TableName,
				ConnectionGroupDirectory.TableName,
				QueryDirectory.TableName,
				TemplateNodeQueryDirectory.TableName,
				TemplateNodeDirectory.TableName,
				TemplateDirectory.NameFieldName,
				TemplateDirectory.IdFieldName,
				TemplateDirectory.DirFieldName,
				ConnectionGroupDirectory.TableName.AsFk(),
				ServerInstanceDirectory.TableName.AsFk(),
				TemplateNodeQueryDirectory.TableName.AsFk(),
				TemplateNodeDirectory.TableName.AsFk(),
				TemplateDirectory.TableName.AsFk(),
				ConnectionGroupDirectory.NameFn,
				ServerInstanceDirectory.ConnectionNameFn,
				ServerInstanceDirectory.DbTypeFn,
				groupName,
				instanceName,
				connectionType,

				TemplateDirectory.TableIdentityField,
				ConnectionGroupDirectory.TableIdentityField,
				ServerInstanceDirectory.TableIdentityField,
				TemplateNodeQueryDirectory.TableIdentityField,
				TemplateNodeDirectory.TableIdentityField
			);

			List<TemplateRow> result = new List<TemplateRow>();

			TableDefinition definition = TemplateDirectory.CreateTableDefinition();

			new SqlSelectCommand(
				this._storage.Connection,
				sqlQuery,
				reader =>
				{
					ITableRow row = TableRow.Read(definition, reader);
					result.Add(RowConverter.Convert<TemplateRow>(row));
				}
			).Execute(100);

			return result;
		}
	}
}
