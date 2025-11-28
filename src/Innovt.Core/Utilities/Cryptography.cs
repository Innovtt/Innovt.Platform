// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Innovt.Core.Utilities;

/// <summary>
///     Provides methods for cryptographic operations, including hashing and encryption.
/// </summary>
public static class Cryptography
{
    /// <summary>
    ///     Computes the SHA-256 hash of a given plaintext password with an optional salt.
    /// </summary>
    /// <param name="text">The plaintext password to be hashed.</param>
    /// <param name="salt">An optional salt value to add to the plaintext before hashing.</param>
    /// <returns>The SHA-256 hash of the plaintext password.</returns>
    public static string ShaHash(this string text, string salt = "")
    {
        Check.NotNull(text, nameof(text));

        return ComputeSha(text, salt).Replace("-", "", StringComparison.OrdinalIgnoreCase);
    }

    public static string Sha(this string text, string salt = "")
    {
        return ComputeSha(text, salt);
    }

    private static string ComputeSha(this string plainPassword, string salt = "")
    {
        var passBytes = Encoding.UTF8.GetBytes(plainPassword + salt);

        var hashBytes = SHA256.HashData(passBytes);

        var hashedPassword = BitConverter.ToString(hashBytes);

        return hashedPassword;
    }

    /// <summary>
    ///     Encrypts a plaintext string using the provided ICryptoTransform.
    /// </summary>
    /// <param name="cryptoTransform">The ICryptoTransform used for encryption.</param>
    /// <param name="plainText">The plaintext string to be encrypted.</param>
    /// <returns>The encrypted string.</returns>
    internal static string Encrypt(ICryptoTransform cryptoTransform, string plainText)
    {
        Check.NotNull(plainText);

        using var resultStream = new MemoryStream();
        using (var sWriter =
               new StreamWriter(new CryptoStream(resultStream, cryptoTransform, CryptoStreamMode.Write)))
        {
            sWriter.Write(plainText);
        }

        var encrypted = resultStream.ToArray();

        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    ///     Decrypts an encrypted string using the provided ICryptoTransform.
    /// </summary>
    /// <param name="cryptoTransform">The ICryptoTransform used for decryption.</param>
    /// <param name="encrypted">The encrypted string to be decrypted.</param>
    /// <returns>The decrypted plaintext string.</returns>
    internal static string Decrypt(ICryptoTransform cryptoTransform, string encrypted)
    {
        Check.NotNull(encrypted);

        var buffer = Convert.FromBase64String(encrypted);

        using var mStream = new MemoryStream(buffer);
        using var srDecrypt = new StreamReader(new CryptoStream(mStream, cryptoTransform, CryptoStreamMode.Read));
        return srDecrypt.ReadToEnd();
    }

    /// <summary>
    ///     Encrypts a plaintext string using the AES algorithm with a specified key.
    ///     The IV (Initialization Vector) is randomly generated and prepended to the ciphertext.
    /// </summary>
    /// <param name="plainText">The plaintext string to be encrypted.</param>
    /// <param name="keyString">The key for AES encryption.</param>
    /// <returns>The encrypted string with the IV prepended.</returns>
    public static string AesEncrypt(string plainText, string keyString)
    {
        Check.NotNull(plainText);
        Check.NotNull(keyString);

        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(keyString);
        aesAlg.GenerateIV(); // Generate a cryptographically random IV

        using var crypto = aesAlg.CreateEncryptor();
        var encryptedBytes = Encrypt(crypto, plainText);

        // Prepend IV to the encrypted data so it can be used for decryption
        var ivAndEncrypted = Convert.ToBase64String(aesAlg.IV) + ":" + encryptedBytes;
        return ivAndEncrypted;
    }

    /// <summary>
    ///     Encrypts a plaintext string using the Rijndael algorithm with a specified key.
    ///     The IV (Initialization Vector) is randomly generated and prepended to the ciphertext.
    /// </summary>
    /// <param name="plainText">The plaintext string to be encrypted.</param>
    /// <param name="keyString">The key for Rijndael encryption.</param>
    /// <returns>The encrypted string with the IV prepended.</returns>
    public static string RijndaelEncrypt(string plainText, string keyString)
    {
        Check.NotNull(plainText);
        Check.NotNull(keyString);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(keyString);
        aes.GenerateIV(); // Generate a cryptographically random IV

        using var crypto = aes.CreateEncryptor();
        var encryptedBytes = Encrypt(crypto, plainText);

        // Prepend IV to the encrypted data so it can be used for decryption
        var ivAndEncrypted = Convert.ToBase64String(aes.IV) + ":" + encryptedBytes;
        return ivAndEncrypted;
    }

    /// <summary>
    ///     Decrypts an encrypted string using the AES algorithm with a specified key.
    ///     Expects the IV to be prepended to the ciphertext (separated by colon).
    /// </summary>
    /// <param name="encryptedText">The encrypted string with IV prepended.</param>
    /// <param name="keyString">The key for AES decryption.</param>
    /// <returns>The decrypted plaintext string.</returns>
    public static string AesDecrypt(string encryptedText, string keyString)
    {
        Check.NotNull(encryptedText);
        Check.NotNull(keyString);

        // Split IV and ciphertext
        var parts = encryptedText.Split(':');
        if (parts.Length != 2)
            throw new ArgumentException("Invalid encrypted text format. Expected format: IV:Ciphertext");

        var iv = Convert.FromBase64String(parts[0]);
        var ciphertext = parts[1];

        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(keyString);
        aesAlg.IV = iv;

        using var cryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        return Decrypt(cryptoTransform, ciphertext);
    }

    /// <summary>
    ///     Decrypts an encrypted string using the Rijndael algorithm with a specified key.
    ///     This method uses a static IV for backward compatibility with legacy encrypted data.
    ///     WARNING: This method is obsolete and insecure. Use AesDecrypt instead.
    /// </summary>
    /// <param name="encryptedText">The encrypted string to be decrypted.</param>
    /// <param name="keyString">The key for Rijndael decryption.</param>
    /// <returns>The decrypted plaintext string.</returns>
    [Obsolete("This method uses insecure static IV. Use AesDecrypt instead.")]
    public static string RijndaelDecrypt(string encryptedText, string keyString)
    {
        Check.NotNull(encryptedText);
        Check.NotNull(keyString);

        // Check if the encrypted text contains an IV (new format)
        var parts = encryptedText.Split(':');
        if (parts.Length == 2)
        {
            // New format with IV prepended - extract and use it
            var iv = Convert.FromBase64String(parts[0]);
            var ciphertext = parts[1];

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(keyString);
            aes.IV = iv;

            using var cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
            return Decrypt(cryptoTransform, ciphertext);
        }
        else
        {
            // Legacy format with static IV (insecure, but needed for backward compatibility)
            using var rijndael = Rijndael.Create();
            rijndael.Key = Encoding.UTF8.GetBytes(keyString);
            rijndael.IV = new byte[16];

            using var cryptoTransform = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);
            return Decrypt(cryptoTransform, encryptedText);
        }
    }
}