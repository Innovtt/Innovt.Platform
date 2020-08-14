using Amazon.Runtime;

namespace Innovt.Cloud.AWS.Configuration
{
    public interface IAWSConfiguration : IConfiguration
    {
        string AccountNumber { get; set; }

        string Profile { get; set; }

        AWSCredentials GetCredential();
    }
}
