using System;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils.Cryptography;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	/// <summary>
	/// SQLite directory for login
	/// </summary>
	public class LoginDirectory : TableDirectory
	{
		internal const string TableName          = "d_Login";
		internal const string TableIdentityField = "d_Login_id";
		internal const string LoginFn            = "Login";
		internal const string PasswordFn         = "Password";
		internal const string IsWinAuthFn        = "IsWinAuth";

		public LoginDirectory(
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
				.AddNVarCharField(LoginFn,     true,  false)
				.AddNVarCharField(PasswordFn,  true,  false)
				.AddBitField(IsWinAuthFn,      false, false)
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

		/// <summary>
		/// Get Row id for data
		/// </summary>
		/// <param name="instance">Instance</param>
		/// <returns></returns>
		public Int64? GetId(InstanceInfo instance)
		{
			AuthenticationInfo auth = instance.Authentication;

			ICryptoService cipher = this.Storage.CryptoService;

			return this.GetRecordIdByFields(
				this.CreateField(LoginFn,     auth.GetCurrentLogin()),
				this.CreateField(PasswordFn,  cipher.Encrypt(auth.Password)),
				this.CreateField(IsWinAuthFn, auth.IsWindows)
			);
		}
	}
}
