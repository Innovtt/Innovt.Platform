using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

/// <summary>
///     Represents configuration for overriding groups and IAM roles.
/// </summary>
[DataContract]
public class GroupConfiguration
{
    /// <summary>
    ///     Gets or sets the list of groups to override.
    /// </summary>
    [DataMember(Name = "groupsToOverride")]
    [JsonPropertyName("groupsToOverride")]
    public List<string> GroupsToOverride { get; set; } = new();

    /// <summary>
    ///     Gets or sets the list of IAM roles to override.
    /// </summary>
    [DataMember(Name = "iamRolesToOverride")]
    [JsonPropertyName("iamRolesToOverride")]
    public List<string> IamRolesToOverride { get; set; } = new();

    /// <summary>
    ///     Gets or sets the preferred role.
    /// </summary>
    [DataMember(Name = "preferredRole")]
    [JsonPropertyName("preferredRole")]
    public string PreferredRole { get; set; }
}