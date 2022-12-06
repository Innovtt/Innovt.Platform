// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS

using Amazon.Runtime;

namespace Innovt.Cloud.AWS.Configuration;

public interface IAwsConfiguration : IConfiguration
{
    string AccountNumber { get; set; }

    string Profile { get; set; }

    public AWSCredentials GetCredential();
}