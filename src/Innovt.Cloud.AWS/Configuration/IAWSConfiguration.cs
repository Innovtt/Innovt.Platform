// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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