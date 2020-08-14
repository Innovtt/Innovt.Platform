using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Innovt.Core.Utilities
{
    public static  class Cryptography
    {
        public static string ShaHash(this string plainPassword,string salt="")
        {
            using var sha256 = SHA256.Create();

            var passBytes = Encoding.UTF8.GetBytes(plainPassword + salt);
                
            var hashBytes = sha256.ComputeHash(passBytes);

            var hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); 

            return hashedPassword;
        }

        internal static string Encrypt(ICryptoTransform cryptoTransform, string plainText)
        {
            if (plainText == null) throw new ArgumentNullException(nameof(plainText));

            using var resultStream = new MemoryStream();
            using (var sWriter = new StreamWriter(new CryptoStream(resultStream, cryptoTransform, CryptoStreamMode.Write)))
            {
                sWriter.Write(plainText);
            }

            var encrypted = resultStream.ToArray();

            return Convert.ToBase64String(encrypted);  
        }

     
        internal static string Decrypt(ICryptoTransform cryptoTransform,string  encrypted)
        {
            if (encrypted == null) throw new ArgumentNullException(nameof(encrypted));
   
            var buffer = Convert.FromBase64String(encrypted);

            using var mStream = new MemoryStream(buffer);
            using var srDecrypt = new StreamReader(new CryptoStream(mStream, cryptoTransform, CryptoStreamMode.Read));
            return srDecrypt.ReadToEnd();
        }


        public static string AesEncrypt(string plainText,string keyString)
        {
            if (plainText == null) throw new ArgumentNullException(nameof(plainText));
            if (keyString == null) throw new ArgumentNullException(nameof(keyString));

            using var aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(keyString);
            aesAlg.IV = new byte[16];

            using var crypto = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            return Encrypt(crypto, plainText);
        }

        
        public static string RijndaelEncrypt(string plainText, string keyString)
        {
            if (plainText == null) throw new ArgumentNullException(nameof(plainText));
            if (keyString == null) throw new ArgumentNullException(nameof(keyString));

            using var rijndael = Rijndael.Create();
            rijndael.Key = Encoding.UTF8.GetBytes(keyString);
            rijndael.IV = new byte[16];

            using var crypto = rijndael.CreateEncryptor(Encoding.UTF8.GetBytes(keyString), rijndael.IV);

            return Encrypt(crypto, plainText);
        }



        public static string AesDecrypt(string encryptedText, string keyString)
        {
            if (encryptedText == null) throw new ArgumentNullException(nameof(encryptedText));
            if (keyString == null) throw new ArgumentNullException(nameof(keyString));


            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(keyString);
                aesAlg.IV = new byte[16];

                using var cryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                return Decrypt(cryptoTransform, encryptedText);
            }

        }

        public static string RijndaelDecrypt(string encryptedText, string keyString)
        {
            if (encryptedText == null) throw new ArgumentNullException(nameof(encryptedText));
            if (keyString == null) throw new ArgumentNullException(nameof(keyString));

            using var rijndael = Rijndael.Create();
            rijndael.Key = Encoding.UTF8.GetBytes(keyString);
            rijndael.IV = new byte[16];

            using var cryptoTransform = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

            return Decrypt(cryptoTransform, encryptedText);
        }

        public static string Md5Hash(this string plainPassword)
        {
            using var sha256 = MD5.Create();
            var passBytes = Encoding.UTF8.GetBytes(plainPassword);
                
            var hashBytes = sha256.ComputeHash(passBytes);

            var hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); 

            return hashedPassword;
        }
    }
}