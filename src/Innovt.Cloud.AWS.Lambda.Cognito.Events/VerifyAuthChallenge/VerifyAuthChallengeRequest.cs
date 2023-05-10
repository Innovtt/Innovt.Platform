// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Amazon.Lambda.CognitoEvents;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;

public class VerifyAuthChallengeRequest : CognitoVerifyAuthChallengeRequest
{
    [DataMember(Name = "challengeAnswer")]
    [JsonPropertyName("challengeAnswer")]
    public new string ChallengeAnswer { get; set; }
}