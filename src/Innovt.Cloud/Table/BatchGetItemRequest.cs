// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System;
using System.Collections.Generic;

namespace Innovt.Cloud.Table;

/// <summary>
/// Represents a request to batch get items from multiple tables.
/// </summary>
public class BatchGetItemRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BatchGetItemRequest"/> class.
    /// </summary>
    public BatchGetItemRequest()
    {
        Items = new Dictionary<string, BatchGetItem>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BatchGetItemRequest"/> class with a table name and batch request item.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="batchRequestItem">The batch request item for the specified table.</param>
    public BatchGetItemRequest(string tableName, BatchGetItem batchRequestItem) : this()
    {
        AddItem(tableName, batchRequestItem);
    }

    /// <summary>
    /// Gets the items to be retrieved in the batch, keyed by table name.
    /// </summary>
    public Dictionary<string, BatchGetItem> Items { get; private set; }

    /// <summary>
    /// Adds a batch request item for a specific table.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="batchRequestItem">The batch request item for the specified table.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="batchRequestItem"/> is null.</exception>
    public void AddItem(string tableName, BatchGetItem batchRequestItem)
    {
        if (batchRequestItem is null) throw new ArgumentNullException(nameof(batchRequestItem));

        if (Items.TryGetValue(tableName, out var tableItem))
            tableItem.Keys.AddRange(batchRequestItem.Keys);
        else
            Items.Add(tableName, batchRequestItem);
    }
}