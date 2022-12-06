// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS

using System;
using Amazon.Runtime;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Configuration;

public class AssumeRoleAWSConfiguration : IAssumeRoleAWSConfiguration
{
    private readonly IAwsConfiguration configuration;

    public AssumeRoleAWSConfiguration(IAwsConfiguration configuration, string roleArn, string roleSessionName = null,
        string roleExternalId = null)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        RoleArn = roleArn ?? throw new ArgumentNullException(nameof(roleArn));
        ExternalId = roleExternalId;
        RoleSessionName = roleSessionName;
    }

    public string RoleArn { get; set; }
    public string ExternalId { get; set; }
    public string RoleSessionName { get; set; }
    public string AccountNumber { get; set; }
    public string SecretKey { get; set; }
    public string AccessKey { get; set; }
    public string Region { get; set; }
    public string Profile { get; set; }

    public AWSCredentials GetCredential()
    {
        var sourceCredential = configuration.GetCredential();

        if (sourceCredential == null)
            throw new ConfigurationException($"Invalid source credentials.");

        var options = new AssumeRoleAWSCredentialsOptions()
        {
            ExternalId = ExternalId
        };

        if (RoleSessionName.IsNotNullOrEmpty())
            RoleSessionName = "InnovtRoleSession";

        return new AssumeRoleAWSCredentials(sourceCredential, RoleArn, RoleSessionName, options);
    }
}