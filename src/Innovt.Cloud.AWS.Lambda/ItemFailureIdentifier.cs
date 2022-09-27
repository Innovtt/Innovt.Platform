// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda

using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda;

public sealed class ItemFailureIdentifier
{
    public ItemFailureIdentifier(string itemIdentifier)
    {
        ItemIdentifier = itemIdentifier;
    }

    public ItemFailureIdentifier()
    {
    }

    [JsonPropertyName("itemIdentifier")] public string ItemIdentifier { get; set; }
}