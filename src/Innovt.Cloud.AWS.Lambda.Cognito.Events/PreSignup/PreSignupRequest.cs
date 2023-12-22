// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events


using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events.PreSignup;

/// <summary>
/// Represents a request for post-confirmation actions.
/// </summary>
public class PreSignupRequest : TriggerRequest
{
    /// <summary>
    /// Gets or sets the client-specific metadata associated with the request.
    /// </summary>
    [DataMember(Name = "clientMetadata")]
    [JsonPropertyName("clientMetadata")]
    public Dictionary<string, string> ClientMetadata { get; set; } = new();

    /// <summary>
    /// One or more name-value pairs containing the validation data in the request to register a user.
    /// The validation data is set and then passed from the client in the request to register a user.
    /// You can pass this data to your Lambda function by using the ClientMetadata parameter in the InitiateAuth and AdminInitiateAuth API actions.
    /// </summary>
    [DataMember(Name = "validationData")]
    [JsonPropertyName("validationData")]
    public Dictionary<string, string> ValidationData { get; set; } = new();
}