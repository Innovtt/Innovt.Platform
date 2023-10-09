// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

namespace Innovt.AspNetCore.ViewModel;
/// <summary>
/// Base view model for displaying a paginated list of entities.
/// </summary>
/// <typeparam name="T">Type of entities being displayed.</typeparam>
public abstract class ListViewModel<T> : ViewModelBase where T : class
{
    /// <summary>
    /// Initializes a new instance of the ListViewModel class with default values.
    /// </summary>
    protected ListViewModel()
    {
    }
    /// <summary>
    /// Initializes a new instance of the ListViewModel class with specified pagination parameters.
    /// </summary>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    protected ListViewModel(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// Gets or sets the total number of records/entities.
    /// </summary>
    public int TotalRecords { get; set; }
    /// <summary>
    /// Gets or sets a search term for filtering entities.
    /// </summary>
    public string Term { get; set; }
}