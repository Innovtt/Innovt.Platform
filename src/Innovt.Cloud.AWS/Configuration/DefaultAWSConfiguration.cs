using Innovt.Core.Utilities;
using Microsoft.Extensions.Configuration;
using Innovt.Core.Exceptions;
using Amazon.Runtime;

namespace Innovt.Cloud.AWS.Configuration
{
    public class DefaultAWSConfiguration: IAWSConfiguration
    {
        private const string configSection = "AWS";

        public string AccountNumber { get; set; }

        public string SecretKey { get; set; }

        public string AccessKey { get; set; }

        public string DefaultRegion { get; set; }

       
        /// <summary>
        /// Using this constructor you can set the parameters manually or use the overrided constructor
        /// </summary>
        /// <param name="accountNumber">Account number.</param>
        /// <param name="accessKey">Access key.</param>
        /// <param name="secretKey">Secret key.</param>
        /// <param name="defaultRegion">Default region.</param>
        public DefaultAWSConfiguration(string accessKey,string secretKey,string defaultRegion, string accountNumber =null)
        {
            
            Check.NotNull(accessKey, nameof(accessKey));
            Check.NotNull(secretKey, nameof(secretKey));
            Check.NotNull(defaultRegion, nameof(defaultRegion));
            
            this.AccountNumber = accountNumber;
            this.AccessKey = accessKey;
            this.SecretKey = secretKey;
            this.DefaultRegion = defaultRegion;
        }


        /// <summary>
        /// This Constructor will use the Autobind from GetSection. 
        /// </summary>
        /// <param name="configuration">IConfiguration from .Net Core</param>
        /// <param name="sectionName"> The default is AW. </param>
        public DefaultAWSConfiguration(Microsoft.Extensions.Configuration.IConfiguration configuration,string sectionName = configSection)
        {
            Check.NotNull(configuration, nameof(configuration));
          
            var section = configuration.GetSection(sectionName);

            if (section == null)
                throw new CriticalException("Section not Found!");

            section.Bind(this);
        }

        /// <summary>
        /// Using profile
        /// </summary>
        public DefaultAWSConfiguration()
        {  
        }


        /// <summary>
        /// Initializes a new instance of the DefaultAWSConfiguration class using IConfiguration from .net Core.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public DefaultAWSConfiguration(Microsoft.Extensions.Configuration.IConfiguration configuration): this(configuration, configSection)
        {

        }

        public AWSCredentials GetCredential() {

            var credential = new BasicAWSCredentials(AccessKey, SecretKey);

            return credential;
        }
    }
}
