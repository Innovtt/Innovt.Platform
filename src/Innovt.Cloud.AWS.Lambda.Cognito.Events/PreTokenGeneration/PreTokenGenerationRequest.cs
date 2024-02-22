// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.PreTokenGeneration;

/// <summary>
///     Represents a request for pre-token generation actions.
/// </summary>
public class PreTokenGenerationRequest : TriggerRequest
{
    /// <summary>
    ///     Gets or sets the group configuration associated with the request.
    /// </summary>
    [DataMember(Name = "groupConfiguration")]
    [JsonPropertyName("groupConfiguration")]
    public GroupConfiguration GroupConfiguration { get; set; } = new();

    /// <summary>
    ///     Gets or sets the client-specific metadata associated with the request.
    /// </summary>
    [DataMember(Name = "clientMetadata")]
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; } = new();
}