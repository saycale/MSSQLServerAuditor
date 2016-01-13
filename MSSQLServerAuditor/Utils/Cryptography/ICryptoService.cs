namespace MSSQLServerAuditor.Utils.Cryptography
{
	public interface ICryptoService
	{
		string Encrypt(string plainText);
		string Decrypt(string encryptedText);
	}
}
