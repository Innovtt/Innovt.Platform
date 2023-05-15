// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.DefineAuthChallenge;

public class DefineAuthChallengeRequest : TriggerRequest
{
    
    [DataMember(Name = "clientMetadata")]
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; } = new Dictionary<string, string>();


    [DataMember(Name = "session")]
    [JsonPropertyName("session")]
    public List<ChallengeResult> Session { get; set; } = new List<ChallengeResult>();


    [DataMember(Name = "userNotFound")]
    [JsonPropertyName("userNotFound")]
    public bool UserNotFound { get; set; }
}