// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;

/// <summary>
/// Class to represents a response for creating authentication challenges.
/// </summary>
public class CreateAuthChallengeResponse : TriggerResponse
{
    /// <summary>
    /// Gets or sets the public challenge parameters associated with the response.
    /// </summary>
    [DataMember(Name = "publicChallengeParameters")]
    [JsonPropertyName("publicChallengeParameters")]
    public Dictionary<string, string> PublicChallengeParameters { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets the private challenge parameters associated with the response.
    /// </summary>
    [DataMember(Name = "privateChallengeParameters")]
    [JsonPropertyName("privateChallengeParameters")]
    public Dictionary<string, string> PrivateChallengeParameters { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets metadata associated with the authentication challenge.
    /// </summary>
    [DataMember(Name = "challengeMetadata")]
    [JsonPropertyName("challengeMetadata")]
    public string ChallengeMetadata { get; set; }

}