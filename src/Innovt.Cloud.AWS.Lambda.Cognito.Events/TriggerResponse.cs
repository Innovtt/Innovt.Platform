// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

[DataContract]
public abstract class TriggerResponse
{
    [DataMember(Name = "publicChallengeParameters")]
    [JsonPropertyName("publicChallengeParameters")]
    public Dictionary<string, string> PublicChallengeParameters { get; set; } = new Dictionary<string, string>();

    [DataMember(Name = "privateChallengeParameters")]
    [JsonPropertyName("privateChallengeParameters")]
    public Dictionary<string, string> PrivateChallengeParameters { get; set; } = new Dictionary<string, string>();

    [DataMember(Name = "challengeMetadata")]
    [JsonPropertyName("challengeMetadata")]
    public string ChallengeMetadata { get; set; }
}