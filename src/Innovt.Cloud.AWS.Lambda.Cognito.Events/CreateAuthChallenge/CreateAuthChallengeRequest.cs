// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using Amazon.Lambda.CognitoEvents;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;

public class CreateAuthChallengeRequest : CognitoCreateAuthChallengeRequest
{
    [DataMember(Name = "userNotFound")]
    [JsonPropertyName("userNotFound")]
    public bool? UserNotFound { get; set; }
}