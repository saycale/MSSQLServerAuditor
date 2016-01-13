using System.Collections.Generic;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.Utils.Cryptography;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class LoginRow : EncryptedTableRow
	{
		private static readonly List<string> EncryptedFields = new List<string>
		{
			LoginDirectory.PasswordFn
		};

		public LoginRow() : base(LoginDirectory.CreateTableDefinition())
		{
		}

		public LoginRow Decrypt(ITableRow row, ICryptoService cipher)
		{
			base.DecryptFrom(row, cipher);

			return this;
		}

		public LoginRow Encrypt(ITableRow row, ICryptoService cipher)
		{
			base.EncryptFrom(row, cipher);

			return this;
		}

		internal string Login
		{
			get
			{
				return this.GetValue<string>(LoginDirectory.LoginFn);
			}
			set
			{
				this.SetValue(LoginDirectory.LoginFn, value);
			}
		}

		internal string Password
		{
			get
			{
				return this.GetValue<string>(LoginDirectory.PasswordFn);
			}
			set
			{
				this.SetValue(LoginDirectory.LoginFn, value);
			}
		}

		internal bool IsWinAuth
		{
			get
			{
				return this.GetValue<bool>(LoginDirectory.IsWinAuthFn, false);
			}
			set
			{
				this.SetValue(LoginDirectory.IsWinAuthFn, value);
			}
		}

		public override List<string> EncryptedStringFields
		{
			get { return EncryptedFields; }
		}
	}
}
