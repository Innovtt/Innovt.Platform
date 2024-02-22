// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

/// <summary>
///     Represents a base class for trigger request objects.
/// </summary>
[DataContract]
public abstract class TriggerRequest
{
    /// <summary>
    ///     Gets or sets user attributes associated with the request.
    /// </summary>
    [DataMember(Name = "userAttributes")]
    [JsonPropertyName("userAttributes")]
    public Dictionary<string, string> UserAttributes { get; set; } = new();
}