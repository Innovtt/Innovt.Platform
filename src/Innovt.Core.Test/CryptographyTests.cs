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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(encrypted, Is.Not.Null);
            // With dynamic IV, the encrypted value will be different each time
            // Format should be: IV:Ciphertext
            Assert.That(encrypted, Does.Contain(":"));
            var parts = encrypted.Split(':');
            Assert.That(parts.Length, Is.EqualTo(2));
        }
    }


    [Test]
    [TestCase("michel borges", "e37306c1755548f79bfac21185d5a6ef")]
    public void AesDecrypt(string plainText, string key)
    {
        var encrypted = Cryptography.AesEncrypt(plainText, key);

        Assert.That(encrypted, Is.Not.Null);

        var decrypted = Cryptography.AesDecrypt(encrypted, key);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decrypted, Is.Not.Null);
            Assert.That(plainText, Is.EqualTo(decrypted));
        }
    }
}