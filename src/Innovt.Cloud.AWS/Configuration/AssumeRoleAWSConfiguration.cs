// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS

using System;
using Amazon.Runtime;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Configuration;

/// <summary>
///     Represents the configuration for assuming a role in AWS.
/// </summary>
public class AssumeRoleAWSConfiguration : IAssumeRoleAWSConfiguration
{
    private readonly IAwsConfiguration configuration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AssumeRoleAWSConfiguration" /> class.
    /// </summary>
    /// <param name="configuration">The AWS configuration.</param>
    /// <param name="roleArn">The Amazon Resource Name (ARN) of the role to assume.</param>
    /// <param name="roleSessionName">The name to use for the assumed role session.</param>
    /// <param name="roleExternalId">An external ID to use when assuming the role.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="configuration" /> or <paramref name="roleArn" /> is
    ///     null.
    /// </exception>
    public AssumeRoleAWSConfiguration(IAwsConfiguration configuration, string roleArn, string roleSessionName = null,
        string roleExternalId = null)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        RoleArn = roleArn ?? throw new ArgumentNullException(nameof(roleArn));
        ExternalId = roleExternalId;
        RoleSessionName = roleSessionName;
    }

    /// <summary>
    ///     Gets or sets the Amazon Resource Name (ARN) of the role to assume.
    /// </summary>
    public string RoleArn { get; set; }

    /// <summary>
    ///     Gets or sets the external ID to use when assuming the role.
    /// </summary>
    public string ExternalId { get; set; }

    /// <summary>
    ///     Gets or sets the name to use for the assumed role session.
    /// </summary>
    public string RoleSessionName { get; set; }

    /// <summary>
    ///     Gets or sets the AWS account number.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    ///     Gets or sets the AWS secret key.
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    ///     Gets or sets the AWS access key.
    /// </summary>
    public string AccessKey { get; set; }

    /// <summary>
    ///     Gets or sets the AWS region.
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    ///     Gets or sets the AWS profile.
    /// </summary>
    public string Profile { get; set; }

    /// <summary>
    ///     Gets the AWS credentials for assuming the specified role.
    /// </summary>
    /// <returns>The AWS credentials for assuming the role.</returns>
    /// <exception cref="ConfigurationException">Thrown when the source credentials are invalid.</exception>
    public AWSCredentials GetCredential()
    {
        var sourceCredential = configuration.GetCredential();

        if (sourceCredential == null)
            throw new ConfigurationException("Invalid source credentials.");

        var options = new AssumeRoleAWSCredentialsOptions
        {
            ExternalId = ExternalId
        };

        if (RoleSessionName.IsNotNullOrEmpty())
            RoleSessionName = "InnovtRoleSession";

        return new AssumeRoleAWSCredentials(sourceCredential, RoleArn, RoleSessionName, options);
    }
}