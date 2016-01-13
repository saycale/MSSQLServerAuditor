using System.Collections.Generic;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.Utils.Cryptography;

namespace MSSQLServerAuditor.BusinessLogic
{
	public class LoginManager
	{
		private readonly CurrentStorage _storage;
		private readonly ICryptoService _cryptoService;

		public LoginManager(CurrentStorage storage)
		{
			this._storage       = storage;
			this._cryptoService = storage.CryptoService;
		}

		public LoginRow GetLogin(long loginId)
		{
			ITableRow row = this._storage.LoginDirectory.GetRowByIdentity(loginId);

			return new LoginRow().Decrypt(row, this._cryptoService);
		}

		public List<LoginRow> GetLogins(
			string templateName,
			string connectionGroupName,
			string connectionName,
			string connectionType
		)
		{
			string sqlQuery = string.Format(@"
				SELECT DISTINCT
					 {6}.[{24}]
					,{6}.{7}
					,{6}.{8}
					,{6}.{9}
				FROM
					{0}
					INNER JOIN {1} ON
						{0}.[{25}] = {1}.{10}
					INNER JOIN {2} ON
						{1}.[{26}] = {2}.{11}
					INNER JOIN {3} ON
						{3}.[{27}] = {2}.{12}
					INNER JOIN {4} ON
						{4}.[{28}] = {3}.{13}
					INNER JOIN {5} ON
						{5}.[{29}] = {4}.{14}
					INNER JOIN {6} ON
						{6}.[{24}] = {1}.{15}
				WHERE
					{0}.{16} = '{17}'
					AND {1}.{18} = '{19}'
					AND {1}.{20} = '{21}'
					AND {5}.{22} = '{23}'",
				ConnectionGroupDirectory.TableName,
				ServerInstanceDirectory.TableName,
				QueryDirectory.TableName,
				TemplateNodeQueryDirectory.TableName,
				TemplateNodeDirectory.TableName,
				TemplateDirectory.TableName,
				LoginDirectory.TableName,

				LoginDirectory.LoginFn,
				LoginDirectory.IsWinAuthFn,
				LoginDirectory.PasswordFn,

				ConnectionGroupDirectory.TableName.AsFk(),
				ServerInstanceDirectory.TableName.AsFk(),
				TemplateNodeQueryDirectory.TableName.AsFk(),
				TemplateNodeDirectory.TableName.AsFk(),
				TemplateDirectory.TableName.AsFk(),
				LoginDirectory.TableName.AsFk(),

				ConnectionGroupDirectory.NameFn,
				connectionGroupName,

				ServerInstanceDirectory.ConnectionNameFn,
				connectionName,

				ServerInstanceDirectory.DbTypeFn,
				connectionType,

				TemplateDirectory.NameFieldName,
				templateName,

				LoginDirectory.TableIdentityField,
				ConnectionGroupDirectory.TableIdentityField,
				ServerInstanceDirectory.TableIdentityField,
				TemplateNodeQueryDirectory.TableIdentityField,
				TemplateNodeDirectory.TableIdentityField,
				TemplateDirectory.TableIdentityField
			);

			List<LoginRow> result = new List<LoginRow>();

			TableDefinition definition = LoginDirectory.CreateTableDefinition();
			new SqlSelectCommand(
				this._storage.Connection,
				sqlQuery,
				reader =>
					{
						ITableRow row = TableRow.Read(definition, reader);
						result.Add(new LoginRow().Decrypt(row, this._cryptoService));
					}
			).Execute(100);

			return result;
		}
	}
}
