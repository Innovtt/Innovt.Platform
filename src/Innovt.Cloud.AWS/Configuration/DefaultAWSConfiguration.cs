using Innovt.Core.Utilities;
using Microsoft.Extensions.Configuration;
using Innovt.Core.Exceptions;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using System;

namespace Innovt.Cloud.AWS.Configuration
{
    public class DefaultAWSConfiguration: IAWSConfiguration
    {
        private const string configSection = "AWS";
        public string AccountNumber { get; set; }
        public string SecretKey { get; set; }
        public string AccessKey { get; set; }
        public string Region { get; set; }
        public string Profile { get; set; }
      
        /// <summary>
        /// Using custom Profile
        /// </summary>
        /// <param name="profileName"></param>
        public DefaultAWSConfiguration(string profileName)
        {
            Profile = profileName ?? throw new System.ArgumentNullException(nameof(profileName));
        }

        public DefaultAWSConfiguration()
        {
        }

        /// <summary>
        /// This Constructor will use the Autobind from GetSection. 
        /// </summary>
        /// <param name="configuration">IConfiguration from .Net Core</param>
        /// <param name="sectionName"> The default is AWS. </param>
        public DefaultAWSConfiguration(Microsoft.Extensions.Configuration.IConfiguration configuration, string sectionName = configSection)
        {
            Check.NotNull(configuration, nameof(configuration));

            var section = configuration.GetSection(sectionName);

            if (section == null || !section.Exists())
            {
                throw new CriticalException($"Section {sectionName} not Found!");
            }

            section.Bind(this);
        }


        public DefaultAWSConfiguration(string accessKey,string secretKey,string region, string accountNumber = null)
        {   
            Check.NotNull(accessKey, nameof(accessKey));
            Check.NotNull(secretKey, nameof(secretKey));
            Check.NotNull(region, nameof(region));
            
            this.AccountNumber = accountNumber;
            this.AccessKey = accessKey;
            this.SecretKey = secretKey;
            this.Region = region;
        }

        internal AWSCredentials GetCredentialsFromProfile()
        {
            var sharedProfile = new SharedCredentialsFile();

            var profile = sharedProfile.ListProfiles().Find(p => p.Name.Equals(Profile, StringComparison.InvariantCultureIgnoreCase));

            if (profile == null)
               throw new ConfigurationException($"Profile {Profile} not found.");

            if (Region == null && profile?.Region != null)
                Region = profile.Region.SystemName;
            
            return AWSCredentialsFactory.GetAWSCredentials(profile, sharedProfile);
        }

        public AWSCredentials GetCredential() {

            AWSCredentials credentials = null;

            if (Profile.IsNotNullOrEmpty())
            {
                credentials = GetCredentialsFromProfile();
            }

            if (credentials != null)
                return credentials;

            if (AccessKey!=null && SecretKey!=null)
            {
                credentials = new BasicAWSCredentials(AccessKey, SecretKey);
            }

            return credentials;
        }
    }
}
