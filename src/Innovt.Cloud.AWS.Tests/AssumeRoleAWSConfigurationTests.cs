using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.Exceptions;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Tests
{
    [TestFixture]
    public class AssumeRoleAWSConfigurationTests
    {
        [Test]
        public void AssumeRoleAWSConfigurationShouldReturnProvidedRoleInfos()
        {
            var configuration = new Configuration.DefaultAWSConfiguration();

            Assert.IsNotNull(configuration);

            var roleArn = "mockrolearn"; 
            var externalId = "mockexternalid"; 
            var sessionName = "mockrosession"; 

            var assumeRoleCredentials = new AssumeRoleAWSConfiguration(configuration, roleArn,sessionName,externalId);

            Assert.IsNotNull(assumeRoleCredentials);
            Assert.AreEqual(roleArn, assumeRoleCredentials.RoleArn);
            Assert.AreEqual(sessionName, assumeRoleCredentials.RoleSessionName);
            Assert.AreEqual(externalId, assumeRoleCredentials.ExternalId);
        }


        [Test]
        public void GetCredentialWithoutProfileReturnDefaultProfile()
        {
            var configuration = new Configuration.DefaultAWSConfiguration();

            Assert.IsNotNull(configuration);

            var assumeRoleCredentials = new AssumeRoleAWSConfiguration(configuration, "rolearn");

            Assert.IsNotNull(assumeRoleCredentials);
            Assert.IsNotNull(assumeRoleCredentials.GetCredential());
        }

       
    }
}