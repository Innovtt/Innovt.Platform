// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS

namespace Innovt.Cloud.AWS.Configuration;

public interface IAssumeRoleAWSConfiguration: IAwsConfiguration
{
    string RoleArn { get; set; }
    string ExternalId { get; set; }
    string RoleSessionName { get; set; }
}