﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

/// <summary>
///     Represents an item containing information about a challenge result.
/// </summary>
[DataContract]
public class ChallengeResultItem
{
    /// <summary>
    ///     Gets or sets the name of the challenge.
    /// </summary>
    [DataMember(Name = "challengeName")]
    [JsonPropertyName("challengeName")]
    public string ChallengeName { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the challenge was successful.
    /// </summary>
    [DataMember(Name = "challengeResult")]
    [JsonPropertyName("challengeResult")]
    public bool ChallengeResult { get; set; }

    /// <summary>
    ///     Gets or sets metadata associated with the challenge.
    /// </summary>
    [DataMember(Name = "challengeMetadata")]
    [JsonPropertyName("challengeMetadata")]
    public string ChallengeMetadata { get; set; }
}