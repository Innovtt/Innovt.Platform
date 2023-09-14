// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

/// <summary>
/// Represents a base class for trigger events with request and response types.
/// </summary>
/// <typeparam name="TRequest">The type of the trigger request.</typeparam>
/// <typeparam name="TResponse">The type of the trigger response.</typeparam>
[DataContract]
public abstract class TriggerEvent<TRequest, TResponse>
    where TRequest : TriggerRequest, new()
    where TResponse : TriggerResponse, new()
{
    /// <summary>
    /// Gets or sets the version associated with the event.
    /// </summary>
    [DataMember(Name = "version")]
    [JsonPropertyName("version")]
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the region associated with the event.
    /// </summary>
    [DataMember(Name = "region")]
    [JsonPropertyName("region")]
    public string Region { get; set; }

    /// <summary>
    /// Gets or sets the user pool ID associated with the event.
    /// </summary>
    [DataMember(Name = "userPoolId")]
    [JsonPropertyName("userPoolId")]
    public string UserPoolId { get; set; }

    /// <summary>
    /// Gets or sets the user name associated with the event.
    /// </summary>
    [DataMember(Name = "userName")]
    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the caller context associated with the event.
    /// </summary>
    [DataMember(Name = "callerContext")]
    [JsonPropertyName("callerContext")]
    public TriggerCallerContext CallerContext { get; set; } = new();

    /// <summary>
    /// Gets or sets the trigger source associated with the event.
    /// </summary>
    [DataMember(Name = "triggerSource")]
    [JsonPropertyName("triggerSource")]
    public string TriggerSource { get; set; }

    /// <summary>
    /// Gets or sets the request associated with the event.
    /// </summary>
    [DataMember(Name = "request")]
    [JsonPropertyName("request")]
    public TRequest Request { get; set; } = new TRequest();

    /// <summary>
    /// Gets or sets the response associated with the event.
    /// </summary>
    [DataMember(Name = "response")]
    [JsonPropertyName("response")]
    public TResponse Response { get; set; } = new TResponse();
}