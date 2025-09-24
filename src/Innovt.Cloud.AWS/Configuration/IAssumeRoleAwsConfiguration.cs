// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS

using System;

namespace Innovt.Cloud.AWS.Configuration;

/// <summary>
///     Represents the configuration for assuming an AWS IAM role.
/// </summary>
[CLSCompliant(false)]
public interface IAssumeRoleAwsConfiguration : IAwsConfiguration
{
    /// <summary>
    ///     Gets or sets the Amazon Resource Name (ARN) of the IAM role to be assumed.
    /// </summary>
    public string RoleArn { get; set; }

    /// <summary>
    ///     Gets or sets an external identifier used when assuming the IAM role (optional).
    /// </summary>
    public string? ExternalId { get; set; }

    /// <summary>
    ///     Gets or sets the name of the assumed role session (optional).
    /// </summary>
    public string? RoleSessionName { get; set; }
}