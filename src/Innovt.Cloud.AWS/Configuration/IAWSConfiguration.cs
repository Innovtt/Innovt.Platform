// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS

using Amazon.Runtime;
using System;

namespace Innovt.Cloud.AWS.Configuration;
[CLSCompliant(true)]
public interface IAwsConfiguration : IConfiguration
{
    string AccountNumber { get; set; }

    string Profile { get; set; }

    public AWSCredentials GetCredential();
}