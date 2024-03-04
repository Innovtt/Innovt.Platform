using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

/// <summary>
///     Represents details for overriding claims in a token.
/// </summary>
[DataContract]
public class ClaimOverrideDetails
{
    /// <summary>
    ///     Gets or sets the claims to add or override in the token.
    /// </summary>
    [DataMember(Name = "claimsToAddOrOverride")]
    [JsonPropertyName("claimsToAddOrOverride")]
    public Dictionary<string, string> ClaimsToAddOrOverride { get; set; } = [];

    /// <summary>
    ///     Gets or sets the claims to suppress in the token.
    /// </summary>
    [DataMember(Name = "claimsToSuppress")]
    [JsonPropertyName("claimsToSuppress")]
    public List<string> ClaimsToSuppress { get; set; } = [];

    /// <summary>
    ///     Gets or sets group override details associated with the token.
    /// </summary>
    [DataMember(Name = "groupOverrideDetails")]
    [JsonPropertyName("groupOverrideDetails")]
    public GroupConfiguration GroupOverrideDetails { get; set; } = new();
}