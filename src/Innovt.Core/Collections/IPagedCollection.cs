// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Innovt.Core.Collections;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "<Pending>")]
/// <summary>
/// Represents a paged collection of items of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
/// <remarks>
/// This interface defines properties and methods for working with paged collections of items.
/// It is commonly used to represent the result of paginated queries or data retrieval operations.
/// </remarks>
public interface IPagedCollection<T>
{
    /// <summary>
    /// Gets or sets the collection of items in the current page.
    /// </summary>
    IEnumerable<T> Items { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    string Page { get; set; }

    /// <summary>
    /// Gets the total number of pages in the paged collection.
    /// </summary>
    int PageCount { get; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of records in the entire collection.
    /// </summary>
    int TotalRecords { get; set; }

    /// <summary>
    /// Determines whether there is a next page in the paged collection.
    /// </summary>
    /// <returns><c>true</c> if there is a next page; otherwise, <c>false</c>.</returns>
    bool HasNext();

    /// <summary>
    /// Determines whether there is a previous page in the paged collection.
    /// </summary>
    /// <returns><c>true</c> if there is a previous page; otherwise, <c>false</c>.</returns>
    bool HasPrevious();
}