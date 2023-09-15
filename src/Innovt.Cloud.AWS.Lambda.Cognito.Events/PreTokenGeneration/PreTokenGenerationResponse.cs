// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.PreTokenGeneration;

/// <summary>
/// Represents a response for pre-token generation actions.
/// </summary>
public class PreTokenGenerationResponse : TriggerResponse
{
    /// <summary>
    /// Gets or sets the claim override details associated with the response.
    /// </summary>
    [DataMember(Name = "claimsOverrideDetails")]
    [JsonPropertyName("claimsOverrideDetails")]
    public ClaimOverrideDetails ClaimsOverrideDetails { get; set; } = new();
}