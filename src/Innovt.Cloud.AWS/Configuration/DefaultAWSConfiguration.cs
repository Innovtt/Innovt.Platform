using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Microsoft.Extensions.Configuration;
using System;

namespace Innovt.Cloud.AWS.Configuration
{
    public class DefaultAWSConfiguration : IAWSConfiguration
    {
        private const string configSection = "AWS";
        public string AccountNumber { get; set; }
        public string SecretKey { get; set; }
        public string AccessKey { get; set; }
        public string RoleArn { get; set; }
        public string SessionToken { get; set; }
        public string Region { get; set; }
        public string Profile { get; set; }


        /// <summary>
        /// Using custom Profile
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="roleArn"></param>
        public DefaultAWSConfiguration(string profileName, string roleArn = null)
        {
            Profile = profileName ?? throw new ArgumentNullException(nameof(profileName));
            RoleArn = roleArn;
        }

        public DefaultAWSConfiguration()
        {
        }

        /// <summary>
        /// This Constructor will use the Autobind from GetSection. 
        /// </summary>
        /// <param name="configuration">IConfiguration from .Net Core</param>
        /// <param name="sectionName"> The default is AWS. </param>
        public DefaultAWSConfiguration(Microsoft.Extensions.Configuration.IConfiguration configuration,
            string sectionName = configSection)
        {
            Check.NotNull(configuration, nameof(configuration));

            var section = configuration.GetSection(sectionName);

            if (section == null || !section.Exists()) throw new CriticalException($"Section {sectionName} not Found!");

            section.Bind(this);
        }

        public DefaultAWSConfiguration(string accessKey, string secretKey, string region, string accountNumber = null,
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

        private AWSCredentials GetCredentialsFromProfile()
        {
            var sharedProfile = new SharedCredentialsFile();

            var profile = sharedProfile.ListProfiles()
                .Find(p => p.Name.Equals(Profile, StringComparison.InvariantCultureIgnoreCase));

            if (profile == null)
                throw new ConfigurationException($"Profile {Profile} not found.");

            if (Region == null && profile?.Region != null)
                Region = profile.Region.SystemName;

            return AWSCredentialsFactory.GetAWSCredentials(profile, sharedProfile);
        }

        private AWSCredentials AssumeRole(AWSCredentials credentials)
        {
            if (credentials is null || RoleArn.IsNullOrEmpty())
                return credentials;

            return new AssumeRoleAWSCredentials(credentials, RoleArn, $"InnovtRoleSession");
        }

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

            if (RoleArn.IsNotNullOrEmpty()) credentials = AssumeRole(credentials);

            return credentials;
        }
    }
}