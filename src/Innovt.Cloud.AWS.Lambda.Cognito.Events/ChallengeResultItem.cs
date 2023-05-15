// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

[DataContract]
public class ChallengeResultItem
{
    [DataMember(Name = "challengeName")]
    [JsonPropertyName("challengeName")]
    public string ChallengeName { get; set; }

    [DataMember(Name = "challengeResult")]
    [JsonPropertyName("challengeResult")]
    public bool ChallengeResult { get; set; }

    [DataMember(Name = "challengeMetadata")]
    [JsonPropertyName("challengeMetadata")]
    public string ChallengeMetadata { get; set; }
}

