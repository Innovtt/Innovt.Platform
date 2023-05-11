// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.DefineAuthChallenge;

[DataContract]
public class DefineAuthChallengeResponse : TriggerResponse
{
    [DataMember(Name = "challengeName")]
    [JsonPropertyName("challengeName")]
    public string ChallengeName { get; set; }

    [DataMember(Name = "issueTokens")]
    [JsonPropertyName("issueTokens")]
    public bool IssueTokens { get; set; }


    [DataMember(Name = "failAuthentication")]
    [JsonPropertyName("failAuthentication")]
    public bool FailAuthentication { get; set; }
}