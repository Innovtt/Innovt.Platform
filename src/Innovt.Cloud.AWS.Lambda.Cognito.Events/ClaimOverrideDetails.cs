using System;
using Amazon.Lambda.CognitoEvents;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events
{
    [DataContract]
	public class ClaimOverrideDetails
	{
        [DataMember(Name = "claimsToAddOrOverride")]
        [JsonPropertyName("claimsToAddOrOverride")]
        public Dictionary<string, string> ClaimsToAddOrOverride { get; set; } = new Dictionary<string, string>();


        [DataMember(Name = "claimsToSuppress")]
        [JsonPropertyName("claimsToSuppress")]
        public List<string> ClaimsToSuppress { get; set; } = new List<string>();


        [DataMember(Name = "groupOverrideDetails")]
        [JsonPropertyName("groupOverrideDetails")]
        public GroupConfiguration GroupOverrideDetails { get; set; } = new GroupConfiguration();
    }
}

