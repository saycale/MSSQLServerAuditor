using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace RSAEncryptor
{
    class Crypto
    {
        RSACryptoServiceProvider rsakey;
        public Crypto()
        {
            rsakey = new RSACryptoServiceProvider();
        }
        public string CreatePublicKey()
        {
            return rsakey.ToXmlString(false);
        }
        public string CreatePrivateKey()
        {
            return rsakey.ToXmlString(true);
        }
        public string Encrypt(string publickeyxml, string data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickeyxml);
            byte[] b =  Encoding.UTF8.GetBytes(data);
            byte[] bb = rsa.Encrypt(b, false);
            string s = Convert.ToBase64String(bb);
            return s;
        }
        public string Decrypt(string privatekeyxml, string encdata)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privatekeyxml);
            byte[] cc = Convert.FromBase64String(encdata);
            byte[] b = rsa.Decrypt(cc, false);
            return Encoding.UTF8.GetString(b);
        }

        public string Sign(string privatekeyxml, string data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privatekeyxml);
            return Convert.ToBase64String(rsa.SignData(Encoding.UTF8.GetBytes(data), MD5.Create()));

        }
        public bool Verify(string publickeyxml, string data, string sign)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickeyxml);
            return rsa.VerifyData(Encoding.UTF8.GetBytes(data), MD5.Create(), Convert.FromBase64String(sign));
        }
    }
}
