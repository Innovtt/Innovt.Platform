// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Cqrs.Queries;

/// <summary>
/// Represents a base class for implementing paged filters.
/// </summary>
/// <remarks>
/// This class provides a foundation for creating paged filters that can be used to filter and paginate data in a query.
/// Paged filters typically include criteria such as search terms, sorting options, and pagination settings.
/// </remarks>
public class PagedFilterBase : PagedFilterBase<string>
{
    // This class inherits from PagedFilterBase<string>, so it does not introduce any new members.
    // You can add additional members specific to this derived class if needed.
}

/// <summary>
/// Represents a generic base class for implementing paged filters.
/// </summary>
/// <typeparam name="T">The type of search term or filter criteria.</typeparam>
/// <remarks>
/// This generic class provides a foundation for creating paged filters with specific filter criteria.
/// Paged filters typically include criteria such as search terms, sorting options, and pagination settings.
/// </remarks>
public class PagedFilterBase<T> : IPagedFilter
{
    /// <summary>
    /// Gets or sets the search term or filter criteria.
    /// </summary>
    public T Term { get; set; }

    /// <summary>
    /// Gets or sets the field by which to order the results.
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Gets or sets the direction of the sorting (e.g., ascending or descending).
    /// </summary>
    public string OrderByDirection { get; set; }

    /// <summary>
    /// Gets or sets the page number for pagination.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the number of items to display per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <inheritdoc />
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Implement validation logic specific to this paged filter if needed.
        return new List<ValidationResult>();
    }
}