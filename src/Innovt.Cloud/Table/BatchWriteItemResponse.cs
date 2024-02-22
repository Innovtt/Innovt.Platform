// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Table;

/// <summary>
///     Represents a response from a batch write items operation.
/// </summary>
public class BatchWriteItemResponse
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BatchWriteItemResponse" /> class.
    /// </summary>
    public BatchWriteItemResponse()
    {
        UnprocessedItems = new Dictionary<string, List<BatchWriteItem>>();
    }

    /// <summary>
    ///     Gets the unprocessed items from the batch write operation, grouped by table name.
    /// </summary>
    public Dictionary<string, List<BatchWriteItem>> UnprocessedItems { get; }

    /// <summary>
    ///     Gets a value indicating whether the batch write operation was successful.
    /// </summary>
    public bool Success => UnprocessedItems is null || UnprocessedItems.Count == 0;
}