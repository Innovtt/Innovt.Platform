// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using Amazon.Lambda.CognitoEvents;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

public class TriggerEvent
{
    [DataContract]
    public abstract class CognitoTriggerEvent<TRequest, TResponse>
        where TRequest : TriggerRequest, new()
        where TResponse : TriggerResponse, new()
    {
        [DataMember(Name = "version")]
        public string Version { get; set; }

        [DataMember(Name = "region")]
        public string Region { get; set; }

        [DataMember(Name = "userPoolId")]
        public string UserPoolId { get; set; }

        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        [DataMember(Name = "callerContext")]
        public TriggerCallerContext CallerContext { get; set; } = new TriggerCallerContext();

        [DataMember(Name = "triggerSource")]
        public string TriggerSource { get; set; }

        [DataMember(Name = "request")]
        public TRequest Request { get; set; } = new TRequest();
        
        [DataMember(Name = "response")]
        public TResponse Response { get; set; } = new TResponse();
    }
}