// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Amazon.Lambda.CognitoEvents;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;

public class VerifyAuthChallengeResponse : CognitoVerifyAuthChallengeResponse
{
    [DataMember(Name = "answerCorrect")]
    [JsonPropertyName("answerCorrect")]
    public new bool? AnswerCorrect { get; set; }
}