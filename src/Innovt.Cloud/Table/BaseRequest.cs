// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud.Table;

/// <summary>
/// Represents a base request for querying or scanning a database.
/// </summary>
public class BaseRequest
{
    /// <summary>
    /// Gets or sets the index name to query or scan.
    /// </summary>
    public string IndexName { get; set; }

    /// <summary>
    /// Gets or sets the filter to apply to the query or scan.
    /// </summary>
    public object Filter { get; set; }

    /// <summary>
    /// Gets or sets the attributes to retrieve in the query or scan.
    /// </summary>
    public string AttributesToGet { get; set; }

    /// <summary>
    /// Gets or sets the page size for the query or scan.
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// Gets or sets the page token for pagination in the query or scan.
    /// </summary>
    public string Page { get; set; }

    /// <summary>
    /// Gets or sets the filter expression for the query or scan.
    /// </summary>
    public string FilterExpression { get; set; }
}