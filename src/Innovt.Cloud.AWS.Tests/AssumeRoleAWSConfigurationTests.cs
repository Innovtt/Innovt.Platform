using Innovt.Cloud.AWS.Configuration;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Tests;

[TestFixture]
public class AssumeRoleAWSConfigurationTests
{
    [Test]
    public void AssumeRoleAWSConfigurationShouldReturnProvidedRoleInfos()
    {
        var configuration = new DefaultAwsConfiguration();

        Assert.That(configuration, Is.Not.Null);

        var roleArn = "mockrolearn";
        var externalId = "mockexternalid";
        var sessionName = "mockrosession";

        var assumeRoleCredentials = new AssumeRoleAwsConfiguration(configuration, roleArn, sessionName, externalId);

        Assert.That(assumeRoleCredentials, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(assumeRoleCredentials.RoleArn, Is.EqualTo(roleArn));
            Assert.That(assumeRoleCredentials.RoleSessionName, Is.EqualTo(sessionName));
            Assert.That(assumeRoleCredentials.ExternalId, Is.EqualTo(externalId));
        });
    }

    [Test]
    [Ignore("Only for local tests")]
    public void GetCredentialWithoutProfileReturnDefaultProfile()
    {
        var configuration = new DefaultAwsConfiguration();

        Assert.That(configuration, Is.Not.Null);

        var assumeRoleCredentials = new AssumeRoleAwsConfiguration(configuration, "rolearn");

        Assert.That(assumeRoleCredentials, Is.Not.Null);
        Assert.That(assumeRoleCredentials.GetCredential(), Is.Not.Null);
    }
}