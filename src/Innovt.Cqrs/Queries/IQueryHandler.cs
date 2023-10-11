// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using Innovt.Core.Cqrs.Queries;

namespace Innovt.Cqrs.Queries;

/// <summary>
/// Defines a synchronous query handler for a specific type of filter and result.
/// </summary>
/// <typeparam name="TFilter">The type of filter for the query.</typeparam>
/// <typeparam name="TResult">The type of result expected from the query.</typeparam>
public interface IQueryHandler<in TFilter, out TResult> where TFilter : IFilter where TResult : class
{
    /// <summary>
    /// Handles the specified query synchronously.
    /// </summary>
    /// <param name="filter">The filter for the query.</param>
    /// <returns>The result of handling the query.</returns>
    TResult Handle(TFilter filter);
}