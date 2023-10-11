// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;

/// <summary>
/// Represents a request for verifying authentication challenges.
/// </summary>
public class VerifyAuthChallengeRequest : TriggerRequest
{
    /// <summary>
    /// Gets or sets private challenge parameters associated with the request.
    /// </summary>
    [DataMember(Name = "privateChallengeParameters")]
    [JsonPropertyName("privateChallengeParameters")]
    public Dictionary<string, string> PrivateChallengeParameters { get; set; } = new();

    /// <summary>
    /// Gets or sets client-specific metadata associated with the request.
    /// </summary>
    [DataMember(Name = "clientMetadata")]
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user was not found.
    /// </summary>
    [DataMember(Name = "userNotFound")]
    [JsonPropertyName("userNotFound")]
    public bool UserNotFound { get; set; }

    /// <summary>
    /// Gets or sets the challenge answer provided by the user.
    /// </summary>
    [DataMember(Name = "challengeAnswer")]
    [JsonPropertyName("challengeAnswer")]
    public string ChallengeAnswer { get; set; }
}