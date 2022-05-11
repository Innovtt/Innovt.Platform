using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda;

public sealed class ItemFailureIdentifier
{
    [JsonPropertyName("itemIdentifier")] public string ItemIdentifier { get; set; }

    public ItemFailureIdentifier(string itemIdentifier)
    {
        ItemIdentifier = itemIdentifier;
    }

    public ItemFailureIdentifier()
    {
    }
}