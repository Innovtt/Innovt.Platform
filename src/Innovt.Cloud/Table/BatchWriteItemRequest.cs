// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System;
using System.Collections.Generic;

namespace Innovt.Cloud.Table;

/// <summary>
/// Represents a request to batch write items to multiple tables.
/// </summary>
public class BatchWriteItemRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BatchWriteItemRequest"/> class.
    /// </summary>
    public BatchWriteItemRequest()
    {
        Items = new Dictionary<string, List<BatchWriteItem>>();
        MaxRetry = 3;
        RetryDelay = TimeSpan.FromSeconds(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BatchWriteItemRequest"/> class with a table name and batch write item.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="batchWriteItem">The batch write item for the specified table.</param>
    public BatchWriteItemRequest(string tableName, BatchWriteItem batchWriteItem) : this()
    {
        AddItem(tableName, batchWriteItem);
    }

    /// <summary>
    /// Gets the items to be written in the batch, grouped by table name.
    /// </summary>
    public Dictionary<string, List<BatchWriteItem>> Items { get; private set; }

    /// <summary>
    /// Gets or sets the delay between retry attempts in case of failures.
    /// </summary>
    public TimeSpan RetryDelay { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of retry attempts in case of failures.
    /// </summary>
    public int MaxRetry { get; set; }

    /// <summary>
    /// Adds a batch write item for a specific table.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="batchRequestItem">The batch write item for the specified table.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="batchRequestItem"/> is null.</exception>
    public void AddItem(string tableName, BatchWriteItem batchRequestItem)
    {
        if (batchRequestItem is null) throw new ArgumentNullException(nameof(batchRequestItem));

        if (!Items.ContainsKey(tableName))
            Items.Add(tableName, new List<BatchWriteItem>()
            {
                batchRequestItem
            });
        else
            Items[tableName].Add(batchRequestItem);
    }
}