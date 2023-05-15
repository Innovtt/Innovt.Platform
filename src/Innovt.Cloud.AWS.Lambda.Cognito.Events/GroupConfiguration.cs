using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events
{
    [DataContract]
	public class GroupConfiguration
	{
        [DataMember(Name = "groupsToOverride")]
        [JsonPropertyName("groupsToOverride")]
        public List<string> GroupsToOverride { get; set; } = new List<string>();


        [DataMember(Name = "iamRolesToOverride")]
        [JsonPropertyName("iamRolesToOverride")]
        public List<string> IamRolesToOverride { get; set; } = new List<string>();


        [DataMember(Name = "preferredRole")]
        [JsonPropertyName("preferredRole")]
        public string PreferredRole { get; set; }
    }
}

