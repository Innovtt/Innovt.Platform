// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.PreTokenGeneration;

public class PreTokenGenerationResponse : TriggerResponse
{
    [DataMember(Name = "claimsOverrideDetails")]
    [JsonPropertyName("claimsOverrideDetails")]
    public ClaimOverrideDetails ClaimsOverrideDetails { get; set; } = new ClaimOverrideDetails();
}