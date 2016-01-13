using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MSSQLServerAuditor.Utils.Cryptography
{
	public class CryptoService : ICryptoService
	{
		private const string DefaultSaltKey = "S@LT&KEY";
		private const string DefaultViKey   = "@1B2c3D4e5F6g7H8";

		private readonly string _passwordHash;
		private readonly string _saltKey;
		private readonly string _viKey;

		public CryptoService(
			string passwordHash,
			string saltKey      = DefaultSaltKey,
			string viKey        = DefaultViKey)
		{
			this._passwordHash = passwordHash;
			this._saltKey      = saltKey;
			this._viKey        = viKey;
		}

		public string Encrypt(string plainText)
		{
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			byte[] keyBytes = new Rfc2898DeriveBytes(
				_passwordHash, Encoding.ASCII.GetBytes(_saltKey)).GetBytes(256/8);

			RijndaelManaged symmetricKey = new RijndaelManaged
			{
				Mode    = CipherMode.CBC,
				Padding = PaddingMode.Zeros
			};

			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes,
				Encoding.ASCII.GetBytes(_viKey));

			byte[] cipherTextBytes;

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
					cryptoStream.FlushFinalBlock();
					cipherTextBytes = memoryStream.ToArray();

					return Convert.ToBase64String(cipherTextBytes);
				}
			}
		}

		public string Decrypt(string encryptedText)
		{
			byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
			byte[] keyBytes = new Rfc2898DeriveBytes(
				_passwordHash, Encoding.ASCII.GetBytes(_saltKey)).GetBytes(256/8);

			RijndaelManaged symmetricKey = new RijndaelManaged
			{
				Mode    = CipherMode.CBC,
				Padding = PaddingMode.None
			};

			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes,
				Encoding.ASCII.GetBytes(_viKey));

			using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
				{
					byte[] plainTextBytes = new byte[cipherTextBytes.Length];

					int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

					return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
				}
			}
		}
	}
}
