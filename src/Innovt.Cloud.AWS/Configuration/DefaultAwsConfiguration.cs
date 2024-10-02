// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS

using System;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Microsoft.Extensions.Configuration;

namespace Innovt.Cloud.AWS.Configuration;

/// <summary>
///     Represents the default AWS configuration used for AWS authentication and credentials.
/// </summary>
[CLSCompliant(false)]
public class DefaultAwsConfiguration : IAwsConfiguration
{
    private const string ConfigSection = "AWS";

    /// <summary>
    ///     The default profile name
    /// </summary>
    /// <param name="profileName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public DefaultAwsConfiguration(string profileName)
    {
        Profile = profileName ?? throw new ArgumentNullException(nameof(profileName));
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DefaultAwsConfiguration" /> class.
    /// </summary>
    public DefaultAwsConfiguration()
    {
    }

    /// <summary>
    ///     This Constructor will use the Autobind from GetSection.
    /// </summary>
    /// <param name="configuration">IConfiguration from .Net Core</param>
    /// <param name="sectionName"> The default is AWS. </param>
    public DefaultAwsConfiguration(Microsoft.Extensions.Configuration.IConfiguration configuration,
        string sectionName = ConfigSection)
    {
        Check.NotNull(configuration, nameof(configuration));

        var section = configuration.GetSection(sectionName);

        if (section == null || !section.Exists()) 
            throw new CriticalException($"Section {sectionName} not Found!");

        section.Bind(this);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DefaultAwsConfiguration" /> class with provided credentials.
    /// </summary>
    /// <param name="accessKey">The AWS access key.</param>
    /// <param name="secretKey">The AWS secret key.</param>
    /// <param name="region">The AWS region.</param>
    /// <param name="accountNumber">The AWS account number.</param>
    /// <param name="sessionToken">The AWS session token (optional).</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="accessKey" />, <paramref name="secretKey" />, or
    ///     <paramref name="region" /> is null.
    /// </exception>
    public DefaultAwsConfiguration(string accessKey, string secretKey, string region, string accountNumber = null,
        string sessionToken = null)
    {
        Check.NotNull(accessKey, nameof(accessKey));
        Check.NotNull(secretKey, nameof(secretKey));
        Check.NotNull(region, nameof(region));

        AccountNumber = accountNumber;
        AccessKey = accessKey;
        SecretKey = secretKey;
        Region = region;
        SessionToken = sessionToken;
    }

    /// <summary>
    ///     Gets or sets the AWS session token for temporary credentials.
    /// </summary>
    public string SessionToken { get; set; }

    /// <summary>
    ///     Gets or sets the AWS account number associated with the AWS credentials.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    ///     Gets or sets the AWS secret key for authentication.
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    ///     Gets or sets the AWS access key for authentication.
    /// </summary>
    public string AccessKey { get; set; }

    /// <summary>
    ///     Gets or sets the AWS region for AWS service requests.
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    ///     Gets or sets the AWS named profile to be used for AWS credentials. If set, other credentials will be ignored.
    /// </summary>
    public string Profile { get; set; }

    /// <summary>
    ///     Gets the AWS credentials based on the configured profile or provided access and secret keys.
    /// </summary>
    /// <returns>The AWS credentials.</returns>
    public AWSCredentials GetCredential()
    {
        AWSCredentials credentials = null;

        if (Profile.IsNotNullOrEmpty())
        {
            credentials = GetCredentialsFromProfile();
        }
        else
        {
            if (AccessKey.IsNotNullOrEmpty() || SecretKey.IsNotNullOrEmpty())
            {
                if (SessionToken.IsNullOrEmpty())
                    credentials = new BasicAWSCredentials(AccessKey, SecretKey);
                else
                    credentials = new SessionAWSCredentials(AccessKey, SecretKey, SessionToken);
            }
        }

        if (credentials is null)
            credentials = FallbackCredentialsFactory.GetCredentials();

        return credentials;
    }

    /// <summary>
    ///     Gets AWS credentials using the named AWS profile specified in the <see cref="Profile" /> property.
    /// </summary>
    /// <returns>AWS credentials obtained from the named profile.</returns>
    /// <exception cref="ConfigurationException">Thrown when the specified profile is not found.</exception>
    private AWSCredentials GetCredentialsFromProfile()
    {
        var sharedProfile = new SharedCredentialsFile();

        var profile = sharedProfile.ListProfiles()
            .Find(p => p.Name.Equals(Profile, StringComparison.OrdinalIgnoreCase));

        if (profile == null)
            throw new ConfigurationException($"Profile {Profile} not found.");

        if (Region == null && profile.Region != null)
            Region = profile.Region.SystemName;

        return AWSCredentialsFactory.GetAWSCredentials(profile, sharedProfile);
    }
}