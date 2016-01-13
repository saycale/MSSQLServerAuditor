using System;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using log4net;

namespace MSSQLServerAuditor.Utils
{
    /// <summary>
    /// Cryptographic processor.
    /// </summary>
    public class CryptoProcessor
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly RSACryptoServiceProvider _rsaKeySign;
        private readonly RSACryptoServiceProvider _rsaKeyDecrypt;

        /// <summary>
        /// Initializing object CryptoProcessor.
        /// </summary>
        public CryptoProcessor()
        {
            _rsaKeySign = new RSACryptoServiceProvider();
            _rsaKeyDecrypt = new RSACryptoServiceProvider();
        }

        /// <summary>
        /// Initializing object CryptoProcessor.
        /// </summary>
        /// <param name="keyXmlSign">XML encrypting key.</param>
        /// <param name="keyXmlDecrypt">XML dencrypting key.</param>
        public CryptoProcessor(string keyXmlSign, string keyXmlDecrypt) : this()
        {
            try
            {
                _rsaKeySign.FromXmlString(keyXmlSign);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("keyXmlSign:" + keyXmlSign);
            }

            try
            {
                _rsaKeyDecrypt.FromXmlString(keyXmlDecrypt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("keyXmlDecrypt:" + keyXmlDecrypt);
            }
        }

        /// <summary>
        /// Encrypt.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <returns></returns>
        public string Encrypt(string data)
        {
            return RijndaelSimple.Encrypt(data, _rsaKeyDecrypt.ToXmlString(false), "msSq1sa1t", "SHA1", 2, "ServerAuditorEIv", 256);
        }

        /// <summary>
        /// XML document encoding.
        /// </summary>
        /// <param name="xmlDoc">XML document.</param>
        public void EncryptXmlDocument(XmlDocument xmlDoc)
        {
            XmlEncryptor.Encrypt(xmlDoc, xmlDoc.DocumentElement.Name, _rsaKeyDecrypt, "rsaKey");
        }

        /// <summary>
        /// XML document dencoding.
        /// </summary>
        /// <param name="xmlDoc">XML document.</param>
        public void DecryptXmlDocument(XmlDocument xmlDoc)
        {
            XmlEncryptor.Decrypt(xmlDoc, _rsaKeyDecrypt, "rsaKey");
        }

        /// <summary>
        /// Decrypt.
        /// </summary>
        /// <param name="encdata">Data encoding.</param>
        /// <returns></returns>
        public string Decrypt(string encdata)
        {
            return RijndaelSimple.Decrypt(encdata, _rsaKeyDecrypt.ToXmlString(false), "msSq1sa1t", "SHA1", 2, "ServerAuditorEIv", 256);
        }

        /// <summary>
        /// Sign.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <returns></returns>
        public string Sign(string data)
        {
            return Convert.ToBase64String(_rsaKeySign.SignData(Encoding.UTF8.GetBytes(data.RemoveWhitespaces()), MD5.Create()));
        }

        /// <summary>
        /// Verify.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="sign">Sign.</param>
        /// <returns>Verify data.</returns>
        public bool Verify(string data, string sign)
        {
            try
            {
                return _rsaKeySign.VerifyData(Encoding.UTF8.GetBytes(data.RemoveWhitespaces()), MD5.Create(),
                                          Convert.FromBase64String(sign));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
    }

    /// <summary>
    /// This class uses a symmetric key algorithm (Rijndael/AES) to encrypt and
    /// decrypt data. As long as encryption and decryption routines use the same
    /// parameters to generate the keys, the keys are guaranteed to be the same.
    /// The class uses static functions with duplicate code to make it easier to
    /// demonstrate encryption and decryption logic. In a real-life application,
    /// this may not be the most efficient way of handling encryption, so - as
    /// soon as you feel comfortable with it - you may want to redesign this class.
    /// </summary>
    public class RijndaelSimple
    {
        /// <summary>
        /// Encrypts specified plaintext using Rijndael symmetric key algorithm
        /// and returns a base64-encoded result.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be encrypted.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this example we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Encrypted value formatted as a base64-encoded string.
        /// </returns>
        public static string Encrypt(string plainText,
                                     string passPhrase,
                                     string saltValue,
                                     string hashAlgorithm,
                                     int passwordIterations,
                                     string initVector,
                                     int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                passPhrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize/8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                keyBytes,
                initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="cipherText">
        /// Base64-formatted ciphertext value.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this example we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Decrypted string value.
        /// </returns>
        /// <remarks>
        /// Most of the logic in this function is similar to the Encrypt
        /// logic. In order for decryption to work, all parameters of this function
        /// - except cipherText value - must match the corresponding parameters of
        /// the Encrypt function which was called to generate the
        /// ciphertext.
        /// </remarks>
        public static string Decrypt(string cipherText,
                                     string passPhrase,
                                     string saltValue,
                                     string hashAlgorithm,
                                     int passwordIterations,
                                     string initVector,
                                     int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                passPhrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize/8);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                keyBytes,
                initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         decryptor,
                                                         CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length*2];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);
            return plainText;
        }
    }

    /// <summary>
    /// Illustrates the use of RijndaelSimple class to encrypt and decrypt data.
    /// </summary>
    public class RijndaelSimpleTest
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main1(string[] args)
        {
            string plainText = "Hello, World!"; // original plaintext

            string passPhrase = "Pas5pr@se"; // can be any string
            string saltValue = "s@1tValue"; // can be any string
            string hashAlgorithm = "SHA1"; // can be "MD5"
            int passwordIterations = 2; // can be any number
            string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
            int keySize = 256; // can be 192 or 128

            Console.WriteLine(String.Format("Plaintext : {0}", plainText));

            string cipherText = RijndaelSimple.Encrypt(plainText,
                                                       passPhrase,
                                                       saltValue,
                                                       hashAlgorithm,
                                                       passwordIterations,
                                                       initVector,
                                                       keySize);

            Console.WriteLine(String.Format("Encrypted : {0}", cipherText));

            plainText = RijndaelSimple.Decrypt(cipherText,
                                               passPhrase,
                                               saltValue,
                                               hashAlgorithm,
                                               passwordIterations,
                                               initVector,
                                               keySize);

            Console.WriteLine(String.Format("Decrypted : {0}", plainText));
        }
    }

    /// <summary>
    /// XML encryptor.
    /// </summary>
    public class XmlEncryptor
    {
        /// <summary>
        /// Encrypt.
        /// </summary>
        /// <param name="Doc">XML document.</param>
        /// <param name="ElementToEncrypt">Element to encrypt.</param>
        /// <param name="Alg">Algoritm.</param>
        /// <param name="KeyName">Key name.</param>
        public static void Encrypt(XmlDocument Doc, string ElementToEncrypt, RSA Alg, string KeyName)
        {
            // Check the arguments.
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (ElementToEncrypt == null)
                throw new ArgumentNullException("ElementToEncrypt");
            if (Alg == null)
                throw new ArgumentNullException("Alg");

            ////////////////////////////////////////////////
            // Find the specified element in the XmlDocument
            // object and create a new XmlElemnt object.
            ////////////////////////////////////////////////

            XmlElement elementToEncrypt = Doc.GetElementsByTagName(ElementToEncrypt)[0] as XmlElement;

            // Throw an XmlException if the element was not found.
            if (elementToEncrypt == null)
            {
                throw new XmlException("The specified element was not found");

            }

            //////////////////////////////////////////////////
            // Create a new instance of the EncryptedXml class
            // and use it to encrypt the XmlElement with the
            // a new random symmetric key.
            //////////////////////////////////////////////////

            // Create a 256 bit Rijndael key.
            RijndaelManaged sessionKey = new RijndaelManaged();
            sessionKey.KeySize = 256;

            EncryptedXml eXml = new EncryptedXml();

            byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, sessionKey, false);

            ////////////////////////////////////////////////
            // Construct an EncryptedData object and populate
            // it with the desired encryption information.
            ////////////////////////////////////////////////

            EncryptedData edElement = new EncryptedData();
            edElement.Type = EncryptedXml.XmlEncElementUrl;

            // Create an EncryptionMethod element so that the
            // receiver knows which algorithm to use for decryption.

            edElement.EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncAES256Url);

            // Encrypt the session key and add it to an EncryptedKey element.
            EncryptedKey ek = new EncryptedKey();

            byte[] encryptedKey = EncryptedXml.EncryptKey(sessionKey.Key, Alg, false);

            ek.CipherData = new CipherData(encryptedKey);

            ek.EncryptionMethod = new EncryptionMethod(EncryptedXml.XmlEncRSA15Url);

            // Set the KeyInfo element to specify the
            // name of the RSA key.

            // Create a new KeyInfo element.
            edElement.KeyInfo = new KeyInfo();

            // Create a new KeyInfoName element.
            KeyInfoName kin = new KeyInfoName();

            // Specify a name for the key.
            kin.Value = KeyName;

            // Add the KeyInfoName element to the
            // EncryptedKey object.
            ek.KeyInfo.AddClause(kin);

            // Add the encrypted key to the
            // EncryptedData object.

            edElement.KeyInfo.AddClause(new KeyInfoEncryptedKey(ek));

            // Add the encrypted element data to the
            // EncryptedData object.
            edElement.CipherData.CipherValue = encryptedElement;

            ////////////////////////////////////////////////////
            // Replace the element from the original XmlDocument
            // object with the EncryptedData element.
            ////////////////////////////////////////////////////

            EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);

        }

        /// <summary>
        /// Decrypt.
        /// </summary>
        /// <param name="Doc">XML document.</param>
        /// <param name="Alg">Algoritm.</param>
        /// <param name="KeyName">Key name.</param>
        public static void Decrypt(XmlDocument Doc, RSA Alg, string KeyName)
        {
            // Check the arguments.
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (Alg == null)
                throw new ArgumentNullException("Alg");
            if (KeyName == null)
                throw new ArgumentNullException("KeyName");

            // Create a new EncryptedXml object.
            EncryptedXml exml = new EncryptedXml(Doc);

            // Add a key-name mapping.
            // This method can only decrypt documents
            // that present the specified key name.
            exml.AddKeyNameMapping(KeyName, Alg);

            // Decrypt the element.
            exml.DecryptDocument();

        }
    }
}
