// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Cqrs.Queries;

/// <summary>
/// Defines an asynchronous query handler for a specific type of filter and result.
/// </summary>
/// <typeparam name="TFilter">The type of filter for the query.</typeparam>
/// <typeparam name="TResult">The type of result expected from the query.</typeparam>
public interface IAsyncQueryHandler<in TFilter, TResult> where TFilter : IFilter where TResult : class
{
    /// <summary>
    /// Handles the specified query asynchronously.
    /// </summary>
    /// <param name="filter">The filter for the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous handling of the query and the result.</returns>
    Task<TResult> HandleAsync(TFilter filter, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines an asynchronous query handler for counting entities based on a filter.
/// </summary>
/// <typeparam name="T">The type of filter for the query.</typeparam>
public interface ICountAsyncQueryHandler<in T> where T : IFilter
{
    /// <summary>
    /// Counts the entities that match the specified filter asynchronously.
    /// </summary>
    /// <param name="filter">The filter for the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous counting operation.</returns>
    Task<int> CountAsync(T filter, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines an asynchronous query handler for checking the existence of entities based on a filter.
/// </summary>
/// <typeparam name="T">The type of filter for the query.</typeparam>
public interface IExistAsyncQueryHandler<in T> where T : IFilter
{
    /// <summary>
    /// Checks the existence of entities that match the specified filter asynchronously.
    /// </summary>
    /// <param name="filter">The filter for the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous existence check operation.</returns>
    Task<bool> ExistAsync(T filter, CancellationToken cancellationToken = default);
}