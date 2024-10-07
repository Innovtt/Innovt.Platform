// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS

using System;
using Amazon.Runtime;

namespace Innovt.Cloud.AWS.Configuration;

[CLSCompliant(false)]
public interface IAwsConfiguration : IConfiguration
{
    public string AccountNumber { get; set; }

    public string Profile { get; set; }

    public AWSCredentials GetCredential();
}