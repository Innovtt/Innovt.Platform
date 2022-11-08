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

public class DefaultAWSConfiguration : IAwsConfiguration
{
    private const string ConfigSection = "AWS";
    /// <summary>
    /// The default profile name
    /// </summary>
    /// <param name="profileName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public DefaultAWSConfiguration(string profileName)
    {
        Profile = profileName ?? throw new ArgumentNullException(nameof(profileName));
    }

    public DefaultAWSConfiguration()
    {
    }

    /// <summary>
    ///     This Constructor will use the Autobind from GetSection.
    /// </summary>
    /// <param name="configuration">IConfiguration from .Net Core</param>
    /// <param name="sectionName"> The default is AWS. </param>
    public DefaultAWSConfiguration(Microsoft.Extensions.Configuration.IConfiguration configuration, string sectionName = ConfigSection)
    {
        Check.NotNull(configuration, nameof(configuration));

        var section = configuration.GetSection(sectionName);

        if (section == null || !section.Exists()) throw new CriticalException($"Section {sectionName} not Found!");

        section.Bind(this);
    }

    public DefaultAWSConfiguration(string accessKey, string secretKey, string region, string accountNumber = null, string sessionToken = null)
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

    public string SessionToken { get; set; }
    public string AccountNumber { get; set; }
    public string SecretKey { get; set; }
    public string AccessKey { get; set; }
    public string Region { get; set; }
    public string Profile { get; set; }

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