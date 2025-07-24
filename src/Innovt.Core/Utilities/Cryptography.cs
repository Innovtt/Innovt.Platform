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

        return ComputeSha(text,salt).Replace("-", "", StringComparison.OrdinalIgnoreCase);
    }
    
    public static string Sha(this string text, string salt = "")
    {
        return ComputeSha(text,salt);
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
    /// </summary>
    /// <param name="plainText">The plaintext string to be encrypted.</param>
    /// <param name="keyString">The key for AES encryption.</param>
    /// <returns>The encrypted string.</returns>
    public static string AesEncrypt(string plainText, string keyString)
    {
        Check.NotNull(plainText);
        Check.NotNull(keyString);
        
        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(keyString);
        aesAlg.IV = new byte[16];

        using var crypto = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        return Encrypt(crypto, plainText);
    }

    /// <summary>
    ///     Encrypts a plaintext string using the Rijndael algorithm with a specified key.
    /// </summary>
    /// <param name="plainText">The plaintext string to be encrypted.</param>
    /// <param name="keyString">The key for Rijndael encryption.</param>
    /// <returns>The encrypted string.</returns>
    public static string RijndaelEncrypt(string plainText, string keyString)
    {
        Check.NotNull(plainText);
        Check.NotNull(keyString);

        using var rijndael = Rijndael.Create();
        rijndael.Key = Encoding.UTF8.GetBytes(keyString);
        rijndael.IV = new byte[16];

        using var crypto = rijndael.CreateEncryptor(Encoding.UTF8.GetBytes(keyString), rijndael.IV);

        return Encrypt(crypto, plainText);
    }

    /// <summary>
    ///     Decrypts an encrypted string using the AES algorithm with a specified key.
    /// </summary>
    /// <param name="encryptedText">The encrypted string to be decrypted.</param>
    /// <param name="keyString">The key for AES decryption.</param>
    /// <returns>The decrypted plaintext string.</returns>
    public static string AesDecrypt(string encryptedText, string keyString)
    {
        Check.NotNull(encryptedText);
        Check.NotNull(keyString);

        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(keyString);
        aesAlg.IV = new byte[16];

        using var cryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        return Decrypt(cryptoTransform, encryptedText);
    }

    /// <summary>
    ///     Decrypts an encrypted string using the Rijndael algorithm with a specified key.
    /// </summary>
    /// <param name="encryptedText">The encrypted string to be decrypted.</param>
    /// <param name="keyString">The key for Rijndael decryption.</param>
    /// <returns>The decrypted plaintext string.</returns>
    [Obsolete("Obsolete")]
    public static string RijndaelDecrypt(string encryptedText, string keyString)
    {
        Check.NotNull(encryptedText);
        Check.NotNull(keyString);

        using var rijndael = Rijndael.Create();
        rijndael.Key = Encoding.UTF8.GetBytes(keyString);
        rijndael.IV = new byte[16];

        using var cryptoTransform = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

        return Decrypt(cryptoTransform, encryptedText);
    }
}