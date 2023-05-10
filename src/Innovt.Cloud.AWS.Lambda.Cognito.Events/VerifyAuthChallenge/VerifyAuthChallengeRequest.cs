// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Amazon.Lambda.CognitoEvents;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;

public class VerifyAuthChallengeRequest : CognitoTriggerRequest
{
    [DataMember(Name = "privateChallengeParameters")]
    [JsonPropertyName("privateChallengeParameters")]
    public Dictionary<string, string> PrivateChallengeParameters { get; set; } = new Dictionary<string, string>();

    
    [DataMember(Name = "clientMetadata")]
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; }

    [DataMember(Name = "userNotFound")]
    [JsonPropertyName("userNotFound")]
    public bool? UserNotFound { get; set; }


    [DataMember(Name = "challengeAnswer")]
    [JsonPropertyName("challengeAnswer")]
    public new string ChallengeAnswer { get; set; }
}