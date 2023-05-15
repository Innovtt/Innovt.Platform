// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;

public class CreateAuthChallengeResponse : TriggerResponse
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