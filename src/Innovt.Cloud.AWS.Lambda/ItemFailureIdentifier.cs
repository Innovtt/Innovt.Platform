// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda

using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda;

/// <summary>
///     Represents an identifier for an item that has failed in a batch operation.
/// </summary>
public sealed class ItemFailureIdentifier
{
    // <summary>
    /// Initializes a new instance of the
    /// <see cref="ItemFailureIdentifier" />
    /// class with the specified item identifier.
    /// </summary>
    /// <param name="itemIdentifier">The identifier of the failed item.</param>
    public ItemFailureIdentifier(string itemIdentifier)
    {
        ItemIdentifier = itemIdentifier;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ItemFailureIdentifier" /> class.
    /// </summary>
    public ItemFailureIdentifier()
    {
    }

    /// <summary>
    ///     Gets or sets the identifier of the failed item.
    /// </summary>
    [JsonPropertyName("itemIdentifier")]
    public string ItemIdentifier { get; set; }
}