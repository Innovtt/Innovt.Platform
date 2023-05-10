// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Amazon.Lambda.CognitoEvents;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.DefineAuthChallenge;

public class DefineAuthChallengeResponse : CognitoDefineAuthChallengeResponse
{
    [DataMember(Name = "issueTokens")]
    [JsonPropertyName("issueTokens")]
    public new bool? IssueTokens { get; set; }

    [DataMember(Name = "failAuthentication")]
    [JsonPropertyName("failAuthentication")]
    public new bool? FailAuthentication { get; set; }
}