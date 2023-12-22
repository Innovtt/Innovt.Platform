// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

/// <summary>
///     Represents context information about the trigger caller.
/// </summary>
public class TriggerCallerContext
{
    /// <summary>
    ///     Gets or sets the AWS SDK version used by the caller.
    /// </summary>
    [DataMember(Name = "awsSdkVersion")]
    [JsonPropertyName("awsSdkVersion")]
    public string AwsSdkVersion { get; set; }

    /// <summary>
    ///     Gets or sets the client ID associated with the caller.
    /// </summary>
    [DataMember(Name = "clientId")]
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }
}