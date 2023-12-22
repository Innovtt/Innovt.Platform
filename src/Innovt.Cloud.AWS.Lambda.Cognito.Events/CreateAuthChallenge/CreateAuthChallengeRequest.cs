// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.CreateAuthChallenge;

/// <summary>
///     Class to represents a request for creating authentication challenges.
/// </summary>
public class CreateAuthChallengeRequest : TriggerRequest
{
    /// <summary>
    ///     Gets or sets the client-specific metadata associated with the request.
    /// </summary>
    [DataMember(Name = "clientMetadata")]
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; } = new();

    /// <summary>
    ///     Gets or sets the name of the authentication challenge.
    /// </summary>
    [DataMember(Name = "challengeName")]
    [JsonPropertyName("challengeName")]
    public string ChallengeName { get; set; }

    /// <summary>
    ///     Gets or sets a list of challenge result items associated with the user's session.
    /// </summary>
    [DataMember(Name = "session")]
    [JsonPropertyName("session")]
    public List<ChallengeResultItem> Session { get; set; } = new();

    /// <summary>
    ///     Gets or sets a value indicating whether the user was not found.
    /// </summary>
    [DataMember(Name = "userNotFound")]
    [JsonPropertyName("userNotFound")]
    public bool UserNotFound { get; set; }
}