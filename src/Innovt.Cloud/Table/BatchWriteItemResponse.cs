// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Table;

public class BatchWriteItemResponse
{
    public BatchWriteItemResponse()
    {
        UnprocessedItems = new Dictionary<string, List<BatchWriteItem>>();
    }

    public Dictionary<string, List<BatchWriteItem>> UnprocessedItems { get; private set; }

    public bool Success => UnprocessedItems is null || UnprocessedItems.Count == 0;
}