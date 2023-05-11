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

    /// <summary>The ID of the client associated with the user pool.</summary>
    [DataMember(Name = "clientId")]
    public string ClientId { get; set; }
}