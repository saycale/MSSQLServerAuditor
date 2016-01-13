using System.Collections.Generic;
using System.Data;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.Utils.Cryptography;

namespace MSSQLServerAuditor.SQLite.Common
{
	public abstract class EncryptedTableRow : TableRow
	{
		protected EncryptedTableRow(TableDefinition tableDefinition)
			: base(tableDefinition)
		{
		}

		public abstract List<string> EncryptedStringFields { get; }

		public void EncryptFrom(ITableRow row, ICryptoService cryptoService)
		{
			row.CopyValues(this);

			foreach (string encryptedField in EncryptedStringFields)
			{
				if (this.TableDefinition.Fields.ContainsKey(encryptedField))
				{
					FieldDefinition def = this.TableDefinition.Fields[encryptedField];

					if (def.SqlType == SqlDbType.NVarChar)
					{
						string encryptedValue = cryptoService.Encrypt(GetValue<string>(encryptedField));

						SetValue(encryptedField, encryptedValue);
					}
				}
			}
		}

		public void DecryptFrom(ITableRow encryptedRow, ICryptoService cryptoService)
		{
			encryptedRow.CopyValues(this);

			foreach (string encryptedField in EncryptedStringFields)
			{
				if (this.TableDefinition.Fields.ContainsKey(encryptedField))
				{
					FieldDefinition def = this.TableDefinition.Fields[encryptedField];

					if (def.SqlType == SqlDbType.NVarChar)
					{
						string decryptedValue = cryptoService.Decrypt(GetValue<string>(encryptedField));

						SetValue(encryptedField, decryptedValue);
					}
				}
			}
		}
	}
}
