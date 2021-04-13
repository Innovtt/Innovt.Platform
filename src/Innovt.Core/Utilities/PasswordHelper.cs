// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Security.Cryptography;

namespace Innovt.Core.Utilities
{
    public static class PasswordHelper
    {
        private const int SaltSize = 128 / 8; // 128 bits

        public static bool IsValid(string decodedPassword, string hashedPassword, string salt)
        {
            var encodedPassword = HashPassword(decodedPassword, salt);

            return encodedPassword == hashedPassword;
        }

        private static byte[] InternalRandomSalt()
        {
            var salt = new byte[SaltSize];
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
            Check.NotEmpty(plainPassword, nameof(plainPassword));
            Check.NotEmpty(salt, nameof(salt));

            return plainPassword.ShaHash(salt);
        }

        public static (string password, string salt) HashPassword(string plainPassword)
        {
            Check.NotEmpty(plainPassword, nameof(plainPassword));

            var salt = RandomSalt();

            // Aspnet core sample
            var hashedPassword = plainPassword.ShaHash(salt);

            return (hashedPassword, salt);
        }
    }
}