// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Cqrs.Queries;

/// <summary>
/// Represents an interface for defining filters.
/// </summary>
/// <remarks>
/// This interface serves as a base for defining filters that can be used to filter data in queries or operations.
/// Filters may include various criteria to narrow down data selection.
/// </remarks>
public interface IFilter : IValidatableObject
{
    // You can add specific filter properties and methods if needed in derived interfaces or classes.
}

/// <summary>
/// Represents an interface for defining paged filters with pagination options.
/// </summary>
/// <remarks>
/// This interface extends the <see cref="IFilter"/> interface and includes properties for pagination control.
/// Paged filters are typically used when retrieving data in paginated views or queries.
/// </remarks>
public interface IPagedFilter : IFilter
{
    /// <summary>
    /// Gets or sets the page number for pagination.
    /// </summary
    int Page { get; set; }

    /// <summary>
    /// Gets or sets the number of items to display per page.
    /// </summary>
    int PageSize { get; set; }
}