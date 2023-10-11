// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud.Table;

/// <summary>
/// Represents a message associated with a table, defining a unique identifier.
/// </summary>
public interface ITableMessage
{
    /// <summary>
    /// Gets or sets the unique identifier for the table message.
    /// </summary>
    string Id { get; set; }
}