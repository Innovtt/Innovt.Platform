// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Security.Cryptography;
using System.Text;

namespace Innovt.Core.Utilities;

/// <summary>
///     Provides utility methods for handling password hashing and validation.
/// </summary>
public static class PasswordHelper
{
    private const int SaltSize = 128 / 8; // 128 bits

    private static readonly char[] passwordChars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()".ToCharArray();

    /// <summary>
    ///     Checks if a decoded password matches its hashed representation using the provided salt.
    /// </summary>
    /// <param name="decodedPassword">The plain text password to validate.</param>
    /// <param name="hashedPassword">The hashed password to compare against.</param>
    /// <param name="salt">The salt used for hashing the password.</param>
    /// <returns>True if the decoded password matches the hashed password; otherwise, false.</returns>
    public static bool IsValid(string decodedPassword, string hashedPassword, string salt)
    {
        var encodedPassword = HashPassword(decodedPassword, salt);

        return encodedPassword == hashedPassword;
    }

    /// <summary>
    ///     Generates a cryptographically secure random salt of the specified size in bytes.
    /// </summary>
    /// <returns>An array of bytes representing the random salt.</returns>
    private static byte[] InternalRandomSalt()
    {
        var salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        return salt;
    }

    /// <summary>
    ///     Generates a random salt and returns it as a Base64-encoded string.
    /// </summary>
    /// <returns>A Base64-encoded salt.</returns>
    public static string RandomSalt()
    {
        return Convert.ToBase64String(InternalRandomSalt());
    }

    /// <summary>
    ///     Hashes a plain text password using the provided salt.
    /// </summary>
    /// <param name="plainPassword">The plain text password to hash.</param>
    /// <param name="salt">The salt used for hashing the password.</param>
    /// <returns>The hashed password as a string.</returns>
    public static string HashPassword(string plainPassword, string salt)
    {
        Check.NotEmpty(plainPassword, nameof(plainPassword));
        Check.NotEmpty(salt, nameof(salt));

        return plainPassword.ShaHash(salt);
    }

    /// <summary>
    ///     Hashes a plain text password using a randomly generated salt and returns both the hashed password and the salt.
    /// </summary>
    /// <param name="plainPassword">The plain text password to hash.</param>
    /// <returns>A tuple containing the hashed password and the salt used.</returns>
    public static (string password, string salt) HashPassword(string plainPassword)
    {
        Check.NotEmpty(plainPassword, nameof(plainPassword));

        var salt = RandomSalt();

        // Aspnet core sample
        var hashedPassword = plainPassword.ShaHash(salt);

        return (hashedPassword, salt);
    }

    /// <summary>
    ///     Generates a random password of specified length using cryptographic randomness to ensure security.
    /// </summary>
    /// <param name="passwordLength">
    ///     (int): The desired length of the generated password. If the specified length is less than
    ///     6, the method defaults to a minimum length of 6 to ensure a reasonable level of security.
    /// </param>
    /// <returns>(string) A randomly generated password consisting of characters chosen from a predefined set.</returns>
    public static string GeneratePassword(int passwordLength)
    {
        if (passwordLength < 6) passwordLength = 6;

        var passwordBuilder = new StringBuilder(passwordLength);
        using (var rng = RandomNumberGenerator.Create())
        {
            var randomBytes = new byte[passwordLength];

            rng.GetBytes(randomBytes);

            for (var i = 0; i < passwordLength; i++)
            {
                var randomChar = passwordChars[randomBytes[i] % passwordChars.Length];
                passwordBuilder.Append(randomChar);
            }
        }

        return passwordBuilder.ToString();
    }
}