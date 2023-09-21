// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda

using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda;

/// <summary>
/// Represents a response containing information about batch item failures.
/// </summary>
public class BatchFailureResponse
{
    /// <summary>
    /// Gets the list of batch item failures identified by their item identifiers.
    /// </summary>
    [JsonPropertyName("batchItemFailures")]
    public IList<ItemFailureIdentifier> BatchItemFailures { get; private set; }

    /// <summary>
    /// Adds a batch item failure to the response using its item identifier.
    /// </summary>
    /// <param name="itemIdentifier">The identifier of the failed item to add to the response.</param>
    public void AddItem(string itemIdentifier)
    {
        BatchItemFailures ??= new List<ItemFailureIdentifier>();

        if (BatchItemFailures.Any(i => i.ItemIdentifier == itemIdentifier))
            return;

        BatchItemFailures.Add(new ItemFailureIdentifier(itemIdentifier));
    }

    /// <summary>
    /// Adds multiple batch item failures to the response using their item identifiers.
    /// </summary>
    /// <param name="itemsIdentifier">A collection of item identifiers for the failed items to add to the response.</param>
    public void AddItems(IEnumerable<string> itemsIdentifier)
    {
        if (itemsIdentifier == null)
            return;

        foreach (var item in itemsIdentifier) AddItem(item);
    }
}