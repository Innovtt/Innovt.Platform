// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

[DataContract]
public abstract class TriggerResponse
{
    [DataMember(Name = "publicChallengeParameters")]
    public Dictionary<string, string> PublicChallengeParameters { get; set; } = new Dictionary<string, string>();

    [DataMember(Name = "privateChallengeParameters")]
    public Dictionary<string, string> PrivateChallengeParameters { get; set; } = new Dictionary<string, string>();

    [DataMember(Name = "challengeMetadata")]
    public string ChallengeMetadata { get; set; }
}