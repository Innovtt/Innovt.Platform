// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.DefineAuthChallenge;

/// <summary>
///     Represents a response for defining authentication challenges.
/// </summary>
[DataContract]
public class DefineAuthChallengeResponse : TriggerResponse
{
    /// <summary>
    ///     Gets or sets the name of the authentication challenge.
    /// </summary>
    [DataMember(Name = "challengeName")]
    [JsonPropertyName("challengeName")]
    public string ChallengeName { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to issue tokens.
    /// </summary>
    [DataMember(Name = "issueTokens")]
    [JsonPropertyName("issueTokens")]
    public bool? IssueTokens { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether authentication should fail.
    /// </summary>
    [DataMember(Name = "failAuthentication")]
    [JsonPropertyName("failAuthentication")]
    public bool? FailAuthentication { get; set; }
}