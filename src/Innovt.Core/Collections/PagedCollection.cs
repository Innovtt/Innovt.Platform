// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.Collections.Generic;
using System.Globalization;
using Innovt.Core.Utilities;

namespace Innovt.Core.Collections;

/// <summary>
///     Represents a paged collection of items of type <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
/// <remarks>
///     This class implements the <see cref="IPagedCollection{T}" /> interface to represent a paged collection of items.
///     It provides properties and methods to work with paged collections, including pagination information and navigation.
/// </remarks>
public class PagedCollection<T> : IPagedCollection<T>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedCollection{T}" /> class with the specified collection of items,
    ///     page number, and page size.
    /// </summary>
    /// <param name="collection">The collection of items to be paged.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    public PagedCollection(IEnumerable<T> collection, int? page = null, int? pageSize = null) : this(collection,
        page?.ToString(CultureInfo.CurrentCulture), pageSize)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedCollection{T}" /> class with the specified collection of items,
    ///     page number, and page size.
    /// </summary>
    /// <param name="collection">The collection of items to be paged.</param>
    /// <param name="page">The current page number as a string.</param>
    /// <param name="pageSize">The number of items per page.</param>
    public PagedCollection(IEnumerable<T> collection, string page = null, int? pageSize = null)
    {
        Items = collection;
        Page = page;
        PageSize = pageSize.GetValueOrDefault();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedCollection{T}" /> class with the specified collection of items.
    /// </summary>
    /// <param name="collection">The collection of items to be paged.</param>
    public PagedCollection(IEnumerable<T> collection) : this(collection, "0", 0)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedCollection{T}" /> class with an empty collection.
    /// </summary>
    public PagedCollection()
    {
        Items = new List<T>();
    }

    /// <summary>
    ///     Gets a value indicating whether the page number is a valid numeric representation.
    /// </summary>
    public bool IsNumberPagination => Page.IsNumber();

    /// <inheritdoc />
    public IEnumerable<T> Items { get; set; }

    /// <inheritdoc />
    public int TotalRecords { get; set; }

    /// <inheritdoc />
    public string Page { get; set; }

    /// <inheritdoc />
    public int PageSize { get; set; }

    /// <inheritdoc />
    public int PageCount => PageSize <= 0 ? 0 : TotalRecords / PageSize;

    /// <inheritdoc />
    public bool HasNext()
    {
        if (TotalRecords <= 0 || !IsNumberPagination)
            return false;

        //Page +1 because of the indice will be 0
        var actualPage = int.Parse(Page) + 1 * PageSize;

        return TotalRecords > actualPage;
    }

    /// <inheritdoc />
    public bool HasPrevious()
    {
        if (TotalRecords <= 0 || !IsNumberPagination)
            return false;
        
        return int.Parse(Page) > 1;
    }
}