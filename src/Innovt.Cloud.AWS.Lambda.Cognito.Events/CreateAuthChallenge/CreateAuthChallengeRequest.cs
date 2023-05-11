// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;

public class CreateAuthChallengeRequest : TriggerRequest
{
    [DataMember(Name = "clientMetadata")]
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; } = new Dictionary<string, string>();

    [DataMember(Name = "challengeName")]
    [JsonPropertyName("challengeName")]
    public string ChallengeName { get; set; }

    [DataMember(Name = "session")]
    [JsonPropertyName("session")]
    public List<ChallengeResult> Session { get; set; } = new List<ChallengeResult>();

    [DataMember(Name = "userNotFound")]
    [JsonPropertyName("userNotFound")]
    public bool UserNotFound { get; set; }

}