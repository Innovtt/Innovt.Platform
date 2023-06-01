// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

public class TriggerCallerContext
{
    [DataMember(Name = "awsSdkVersion")]
    [JsonPropertyName("awsSdkVersion")]
    public string AwsSdkVersion { get; set; }
    
    [DataMember(Name = "clientId")]
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }
}