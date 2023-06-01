// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;
[DataContract]
public abstract class TriggerEvent<TRequest, TResponse>
    where TRequest : TriggerRequest, new()
    where TResponse : TriggerResponse, new()
{
    [DataMember(Name = "version")]
    [JsonPropertyName("version")]
    public string Version { get; set; }

    [DataMember(Name = "region")]
    [JsonPropertyName("region")]
    public string Region { get; set; }

    [DataMember(Name = "userPoolId")]
    [JsonPropertyName("userPoolId")]
    public string UserPoolId { get; set; }

    [DataMember(Name = "userName")]
    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    [DataMember(Name = "callerContext")]
    [JsonPropertyName("callerContext")]
    public TriggerCallerContext CallerContext { get; set; } = new();

    [DataMember(Name = "triggerSource")]
    [JsonPropertyName("triggerSource")]
    public string TriggerSource { get; set; }

    [DataMember(Name = "request")]
    [JsonPropertyName("request")]
    public TRequest Request { get; set; } = new TRequest();

    [DataMember(Name = "response")]
    [JsonPropertyName("response")]
    public TResponse Response { get; set; } = new TResponse();
}