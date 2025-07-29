// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.PostAuthentication;

/// <summary>
///     Represents a response for post-confirmation actions.
/// </summary>
public class PostAuthenticationResponse : TriggerResponse
{
    [DataMember(Name = "claimsOverrideDetails")]
    [JsonPropertyName("claimsOverrideDetails")]
    public ClaimOverrideDetails ClaimsOverrideDetails { get; set; } = new();
}