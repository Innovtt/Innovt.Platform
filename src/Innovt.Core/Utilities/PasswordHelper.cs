// Innovt.Common

using System;
using System.Security.Cryptography;
using System.Text;


namespace Innovt.Core.Utilities
{
    public class PasswordHelper
    {
        const int SaltSize = 128 / 8; // 128 bits

        private static string InternalShaHash(string plainPassword,string salt)
        {
            using var sha256 = SHA256.Create();
            var passBytes = Encoding.UTF8.GetBytes(plainPassword + salt);
                
            var hashBytes = sha256.ComputeHash(passBytes);

            var hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); 

            return hashedPassword;
        }


        private static string InternalMd5Hash(string plainPassword)
        {
            using var sha256 = MD5.Create();
            var passBytes = Encoding.UTF8.GetBytes(plainPassword);
                
            var hashBytes = sha256.ComputeHash(passBytes);

            var hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); 

            return hashedPassword;
        }

        public static bool IsValid(string decodedPassword, string hashedPassword, string salt)
        {     
            string encodedPassword = HashPassword(decodedPassword, salt);

            return encodedPassword == hashedPassword;
        }

        private static byte[] InternalRandomSalt()
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string RandomSalt()
        {
            return Convert.ToBase64String(InternalRandomSalt());
        }

        public static string HashPassword(string plainPassword, string salt)
        {
            Check.NotEmpty(plainPassword,nameof(plainPassword));
            Check.NotEmpty(salt,nameof(salt));

            return InternalShaHash(plainPassword, salt);
        }

        public static string Md5Hash(string plainPassword)
        {
            Check.NotEmpty(plainPassword,nameof(plainPassword));

            return InternalMd5Hash(plainPassword);
        }

        public static (string password, string salt) HashPassword(string plainPassword)
        {
            Check.NotEmpty(plainPassword,nameof(plainPassword));

            string salt     = RandomSalt();
         
            // Aspnet core sample
            string hashedPassword = InternalShaHash(plainPassword, salt);

            return (hashedPassword, salt);
        }
    }
}