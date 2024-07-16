using Innovt.Core.Utilities;
using NUnit.Framework;

namespace Innovt.Core.Test
{
    [TestFixture]
    public class PasswordHelperTests
    {
        [Test]
        public void GeneratePassword()
        {
            string password;
            var passwordLength = 20;

            password = PasswordHelper.GeneratePassword(passwordLength);

            Assert.That(password, Is.Not.Null);
            Assert.That(password.Length, Is.EqualTo(passwordLength));
        }

        [Test]
        public void GeneratePasswordShouldGeneratePasswordOfLength6IfPasswordSizeIsLessThan6()
        {
            string password;
            var passwordLength = 4;

            password = PasswordHelper.GeneratePassword(passwordLength);

            Assert.That(password, Is.Not.Null);
            Assert.That(password.Length, Is.EqualTo(6));
        }
    }
}
