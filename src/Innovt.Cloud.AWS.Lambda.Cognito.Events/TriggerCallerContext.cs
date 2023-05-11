// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

public class TriggerCallerContext
{
    [DataMember(Name = "awsSdkVersion")]
    public string AwsSdkVersion { get; set; }
    
    [DataMember(Name = "clientId")]
    public string ClientId { get; set; }
}