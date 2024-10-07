using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.Exceptions;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Tests;

[TestFixture]
[Ignore("Only local tests")]
public class DefaultAWSConfigurationTests
{
    [Test]
    public void GetCredentialWithoutProfileReturnDefaultProfile()
    {
        var configuration = new DefaultAwsConfiguration();

        Assert.That(configuration, Is.Not.Null);

        var credentials = configuration.GetCredential();

        Assert.That(credentials, Is.Not.Null);
        Assert.That(credentials.GetCredentials(), Is.Not.Null);
    }

    [Test]
    public void GetCredentialWithInvalidProfileThrowsException()
    {
        var configuration = new DefaultAwsConfiguration("invalidProfile");

        Assert.That(configuration, Is.Not.Null);

        Assert.Throws<ConfigurationException>(() => configuration.GetCredential());
    }


    [Test]
    public void GetCredentialWithAccessKeyAnSecretReturnsValidCredential()
    {
        var configuration = new DefaultAwsConfiguration("accessKey", "secret", "us-east-1");

        Assert.That(configuration, Is.Not.Null);

        var credentials = configuration.GetCredential();

        Assert.That(credentials, Is.Not.Null);
    }
}