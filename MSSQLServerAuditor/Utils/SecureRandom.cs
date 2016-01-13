using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MSSQLServerAuditor.Utils
{
	public class SecureRandom
	{
		public const string AllowerChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		public static string GenerateKey(int length, string allowedChars = AllowerChars)
		{
			const int byteSize       = 0x100;
			char[]    allowedCharSet = new HashSet<char>(allowedChars).ToArray();

			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
			}

			if (string.IsNullOrEmpty(allowedChars))
			{
				throw new ArgumentException("allowedChars may not be empty.");
			}

			if (byteSize < allowedCharSet.Length)
			{
				throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));
			}

			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
			{
				StringBuilder result = new StringBuilder();
				byte[]        buf    = new byte[128];

				while (result.Length < length)
				{
					rng.GetBytes(buf);

					for (int i = 0; i < buf.Length && result.Length < length; ++i)
					{
						// Divide the byte into allowedCharSet-sized groups. If the
						// random value falls into the last group and the last group is
						// too small to choose from the entire allowedCharSet, ignore
						// the value in order to avoid biasing the result.
						int outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);

						if (outOfRangeStart <= buf[i])
						{
							continue;
						}

						result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
					}
				}

				return result.ToString();
			}
		}
	}
}
