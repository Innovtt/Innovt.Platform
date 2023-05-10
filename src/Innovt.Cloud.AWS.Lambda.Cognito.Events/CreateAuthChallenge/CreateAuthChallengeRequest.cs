// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using Amazon.Lambda.CognitoEvents;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;

public class CreateAuthChallengeRequest : CognitoTriggerRequest
{
    [DataMember(Name = "clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; } = new Dictionary<string, string>();

    [DataMember(Name = "challengeName")]
    public string ChallengeName { get; set; }

    [DataMember(Name = "session")]
    public List<ChallengeResultElement> Session { get; set; } = new List<ChallengeResultElement>();

    [DataMember(Name = "userNotFound")]
    [JsonPropertyName("userNotFound")]
    public bool? UserNotFound { get; set; }

}