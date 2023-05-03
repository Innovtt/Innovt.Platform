using Innovt.Core.Exceptions;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Tests
{
    [TestFixture]
    public class DefaultAWSConfigurationTests
    {
        [Test]
        public void GetCredentialWithoutProfileReturnDefaultProfile()
        {
            var configuration = new Configuration.DefaultAWSConfiguration();

            Assert.IsNotNull(configuration);

            var credentials = configuration.GetCredential();

            Assert.IsNotNull(credentials);
            Assert.IsNotNull(credentials.GetCredentials());
        }

        [Test]
        public void GetCredentialWithInvalidProfileThrowsException()
        {
            var configuration = new Configuration.DefaultAWSConfiguration("invalidProfile");

            Assert.IsNotNull(configuration);

            Assert.Throws<ConfigurationException>(() => configuration.GetCredential());
        }


        [Test]
        public void GetCredentialWithAccessKeyAnSecretReturnsValidCredential()
        {
            var configuration = new Configuration.DefaultAWSConfiguration("accessKey", "secret", "us-east-1");

            Assert.IsNotNull(configuration);

            var credentials = configuration.GetCredential();

            Assert.IsNotNull(credentials);
        }
    }
}