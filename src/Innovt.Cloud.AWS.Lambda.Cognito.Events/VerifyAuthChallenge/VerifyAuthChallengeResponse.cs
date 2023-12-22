// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.VerifyAuthChallenge;

/// <summary>
///     Represents a response for verifying authentication challenges.
/// </summary>
public class VerifyAuthChallengeResponse : TriggerResponse
{
    /// <summary>
    ///     Gets or sets a value indicating whether the challenge answer is correct.
    /// </summary>
    [DataMember(Name = "answerCorrect")]
    [JsonPropertyName("answerCorrect")]
    public bool? AnswerCorrect { get; set; }
}