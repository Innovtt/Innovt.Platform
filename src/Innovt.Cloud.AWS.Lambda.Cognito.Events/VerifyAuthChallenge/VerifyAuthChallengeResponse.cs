// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;

public class VerifyAuthChallengeResponse : TriggerResponse
{
    [DataMember(Name = "answerCorrect")]
    [JsonPropertyName("answerCorrect")]
    public bool? AnswerCorrect { get; set; }
}