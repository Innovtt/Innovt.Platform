// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;
using Innovt.Core.Utilities;
using NUnit.Framework;

namespace Innovt.Core.Test;

[TestFixture]
public class CryptographyTests
{
    [Test]
    public void AesEncryptShouldThrowExceptionWhenTextIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Cryptography.AesEncrypt(null, "alfa"));

        Assert.Throws<ArgumentNullException>(() => Cryptography.AesEncrypt("michel borges", null));
    }


    [Test]
    [TestCase("michel borges")]
    public void AesEncryptCryptography(string plainText)
    {
        var key = "e37306c1755548f79bfac21185d5a6ef";

        var encrypted = Cryptography.AesEncrypt(plainText, key);

        Assert.IsNotNull(encrypted);

        Assert.AreEqual("+CSp39EM8HoEjSn4nOAbnw==", encrypted);
    }


    [Test]
    [TestCase("michel borges", "e37306c1755548f79bfac21185d5a6ef")]
    public void AesDecrypt(string plainText, string key)
    {
        var encrypted = Cryptography.AesEncrypt(plainText, key);

        Assert.IsNotNull(encrypted);

        var decrypted = Cryptography.AesDecrypt(encrypted, key);

        Assert.IsNotNull(decrypted);

        Assert.AreEqual(plainText, decrypted);
    }

    [Test]
    [TestCase("michel borges")]
    public void RijndaelEncrypt(string plainText)
    {
        var key = "e37306c1755548f79bfac21185d5a6ef";

        var encrypted = Cryptography.RijndaelEncrypt(plainText, key);

        Assert.IsNotNull(encrypted);

        Assert.AreEqual("+CSp39EM8HoEjSn4nOAbnw==", encrypted);
    }


    [Test]
    [TestCase("michel borges", "e37306c1755548f79bfac21185d5a6ef")]
    public void RijndaelDecrypt(string plainText, string key)
    {
        var encrypted = Cryptography.RijndaelEncrypt(plainText, key);

        Assert.IsNotNull(encrypted);

        var decrypted = Cryptography.RijndaelDecrypt(encrypted, key);

        Assert.IsNotNull(decrypted);

        Assert.AreEqual(plainText, decrypted);
    }
}