// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events



using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Amazon.Lambda.CognitoEvents;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.PostConfirmation;

public class PostConfirmationRequest : TriggerRequest
{
    [DataMember(Name = "clientMetadata")]
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; } = new Dictionary<string, string>();
}