// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System;

namespace Innovt.Cloud.Table;
/// <summary>
/// Represents a message associated with a table, implementing the ITableMessage interface.
/// </summary>
public class TableMessage : ITableMessage
{
    /// <summary>
    /// Default constructor for TableMessage.
    /// </summary>
    public TableMessage()
    {
    }
    /// <summary>
    /// Constructor for TableMessage with an identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the message.</param>
    public TableMessage(string id)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

        Id = id;
    }
    /// <summary>
    /// Constructor for TableMessage with an identifier and a range key.
    /// </summary>
    /// <param name="id">The unique identifier for the message.</param>
    /// <param name="rangeKey">The range key for the message.</param>
    public TableMessage(string id, string rangeKey) : this(id)
    {
        if (string.IsNullOrEmpty(rangeKey)) throw new ArgumentNullException(nameof(rangeKey));

        RangeKey = rangeKey;
    }
    /// <summary>
    /// Gets or sets the range key associated with the message.
    /// </summary>
    public string RangeKey { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier for the message.
    /// </summary>
    public string Id { get; set; }
}