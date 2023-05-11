// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

[DataContract]
public abstract class TriggerRequest
{
    [DataMember(Name = "userAttributes")]
    [JsonPropertyName("userAttributes")]
    public Dictionary<string, string> UserAttributes { get; set; } = new Dictionary<string, string>();
}