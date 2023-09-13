// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Innovt.Core.Utilities;

/// <summary>
/// Provides methods for cryptographic operations, including hashing and encryption.
/// </summary>
public static class Cryptography
{
    /// <summary>
    /// Computes the SHA-256 hash of a given plaintext password with an optional salt.
    /// </summary>
    /// <param name="plainPassword">The plaintext password to be hashed.</param>
    /// <param name="salt">An optional salt value to add to the plaintext before hashing.</param>
    /// <returns>The SHA-256 hash of the plaintext password.</returns>
    public static string ShaHash(this string plainPassword, string salt = "")
    {
        using var sha256 = SHA256.Create();

        var passBytes = Encoding.UTF8.GetBytes(plainPassword + salt);

        var hashBytes = sha256.ComputeHash(passBytes);

        var hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        return hashedPassword;
    }
    /// <summary>
    /// Encrypts a plaintext string using the provided ICryptoTransform.
    /// </summary>
    /// <param name="cryptoTransform">The ICryptoTransform used for encryption.</param>
    /// <param name="plainText">The plaintext string to be encrypted.</param>
    /// <returns>The encrypted string.</returns>
    internal static string Encrypt(ICryptoTransform cryptoTransform, string plainText)
    {
        if (plainText == null) throw new ArgumentNullException(nameof(plainText));

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
    /// Decrypts an encrypted string using the provided ICryptoTransform.
    /// </summary>
    /// <param name="cryptoTransform">The ICryptoTransform used for decryption.</param>
    /// <param name="encrypted">The encrypted string to be decrypted.</param>
    /// <returns>The decrypted plaintext string.</returns>
    internal static string Decrypt(ICryptoTransform cryptoTransform, string encrypted)
    {
        if (encrypted == null) throw new ArgumentNullException(nameof(encrypted));

        var buffer = Convert.FromBase64String(encrypted);

        using var mStream = new MemoryStream(buffer);
        using var srDecrypt = new StreamReader(new CryptoStream(mStream, cryptoTransform, CryptoStreamMode.Read));
        return srDecrypt.ReadToEnd();
    }

    /// <summary>
    /// Encrypts a plaintext string using the AES algorithm with a specified key.
    /// </summary>
    /// <param name="plainText">The plaintext string to be encrypted.</param>
    /// <param name="keyString">The key for AES encryption.</param>
    /// <returns>The encrypted string.</returns>
    public static string AesEncrypt(string plainText, string keyString)
    {
        if (plainText == null) throw new ArgumentNullException(nameof(plainText));
        if (keyString == null) throw new ArgumentNullException(nameof(keyString));

        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(keyString);
        aesAlg.IV = new byte[16];

        using var crypto = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        return Encrypt(crypto, plainText);
    }

    /// <summary>
    /// Encrypts a plaintext string using the Rijndael algorithm with a specified key.
    /// </summary>
    /// <param name="plainText">The plaintext string to be encrypted.</param>
    /// <param name="keyString">The key for Rijndael encryption.</param>
    /// <returns>The encrypted string.</returns>
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

    /// <summary>
    /// Decrypts an encrypted string using the AES algorithm with a specified key.
    /// </summary>
    /// <param name="encryptedText">The encrypted string to be decrypted.</param>
    /// <param name="keyString">The key for AES decryption.</param>
    /// <returns>The decrypted plaintext string.</returns>
    public static string AesDecrypt(string encryptedText, string keyString)
    {
        if (encryptedText == null) throw new ArgumentNullException(nameof(encryptedText));
        if (keyString == null) throw new ArgumentNullException(nameof(keyString));


        using var aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(keyString);
        aesAlg.IV = new byte[16];

        using var cryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        return Decrypt(cryptoTransform, encryptedText);
    }
    /// <summary>
    /// Decrypts an encrypted string using the Rijndael algorithm with a specified key.
    /// </summary>
    /// <param name="encryptedText">The encrypted string to be decrypted.</param>
    /// <param name="keyString">The key for Rijndael decryption.</param>
    /// <returns>The decrypted plaintext string.</returns>

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
    /// <summary>
    /// Computes the MD5 hash of a given plaintext password.
    /// </summary>
    /// <param name="plainPassword">The plaintext password to be hashed.</param>
    /// <returns>The MD5 hash of the plaintext password.</returns>
    public static string Md5Hash(this string plainPassword)
    {
        using var sha256 = MD5.Create();
        var passBytes = Encoding.UTF8.GetBytes(plainPassword);

        var hashBytes = sha256.ComputeHash(passBytes);

        var hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        return hashedPassword;
    }
}