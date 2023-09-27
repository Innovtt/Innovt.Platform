// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.DefineAuthChallenge;

/// <summary>
/// Represents a request for defining authentication challenges.
/// </summary>
public class DefineAuthChallengeRequest : TriggerRequest
{

    /// <summary>
    /// Gets or sets the client-specific metadata associated with the request.
    /// </summary>
    [DataMember(Name = "clientMetadata")]
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; } = new Dictionary<string, string>();


    /// <summary>
    /// Gets or sets a list of challenge result items associated with the user's session.
    /// </summary>
    [DataMember(Name = "session")]
    [JsonPropertyName("session")]
    public List<ChallengeResultItem> Session { get; set; } = new List<ChallengeResultItem>();


    /// <summary>
    /// Gets or sets a value indicating whether the user was not found.
    /// </summary>
    [DataMember(Name = "userNotFound")]
    [JsonPropertyName("userNotFound")]
    public bool UserNotFound { get; set; }
}