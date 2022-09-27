// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda

using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Lambda;

public class BatchFailureResponse
{
    [JsonPropertyName("batchItemFailures")]
    public IList<ItemFailureIdentifier> BatchItemFailures { get; private set; }

    public void AddItem(string itemIdentifier)
    {
        BatchItemFailures ??= new List<ItemFailureIdentifier>();

        if (BatchItemFailures.Any(i => i.ItemIdentifier == itemIdentifier))
            return;

        BatchItemFailures.Add(new ItemFailureIdentifier(itemIdentifier));
    }

    public void AddItems(IEnumerable<string> itemsIdentifier)
    {
        if (itemsIdentifier == null)
            return;

        foreach (var item in itemsIdentifier) AddItem(item);
    }
}