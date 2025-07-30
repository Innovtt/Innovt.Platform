// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events


using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.PostAuthentication;

/// <summary>
///     Represents a request for post-confirmation actions.
/// </summary>
public class PostAuthenticationRequest : TriggerRequest
{
    /// <summary>
    ///     Gets or sets the client-specific metadata associated with the request.
    /// </summary>
    [DataMember(Name = "clientMetadata")]
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; } = new();
}