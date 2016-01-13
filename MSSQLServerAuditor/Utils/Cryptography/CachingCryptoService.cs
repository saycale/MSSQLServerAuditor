using System.Collections.Concurrent;

namespace MSSQLServerAuditor.Utils.Cryptography
{
	public class CachingCryptoService : ICryptoService
	{
		private readonly ICryptoService _cryptoService;

		private readonly ConcurrentDictionary<string, string> _encCache;
		private readonly ConcurrentDictionary<string, string> _decCache;

		private readonly object _lockObj;

		public CachingCryptoService(string encryptionKey)
		{
			this._cryptoService = new CryptoService(encryptionKey);
			this._lockObj       = new object();
			this._encCache      = new ConcurrentDictionary<string, string>();
			this._decCache      = new ConcurrentDictionary<string, string>();
		}

		public string Encrypt(string plainText)
		{
			lock (this._lockObj)
			{
				string encryptedValue;

				if (this._encCache.TryGetValue(plainText, out encryptedValue))
				{
					return encryptedValue;
				}

				encryptedValue = this._cryptoService.Encrypt(plainText);

				this._encCache.TryAdd(plainText, encryptedValue);

				return encryptedValue;
			}
		}

		public string Decrypt(string encryptedText)
		{
			lock (this._lockObj)
			{
				string decryptedValue;

				if (this._decCache.TryGetValue(encryptedText, out decryptedValue))
				{
					return decryptedValue;
				}

				decryptedValue = this._cryptoService.Decrypt(encryptedText);

				this._decCache.TryAdd(encryptedText, decryptedValue);

				return decryptedValue;
			}
		}
	}
}
