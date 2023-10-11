// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.Validation;

namespace Innovt.Cqrs.Queries.Decorators;

/// <summary>
/// Decorates an asynchronous query handler to include validation before handling the query.
/// </summary>
/// <typeparam name="TFilter">The type of filter for the query.</typeparam>
/// <typeparam name="TResult">The type of result expected from the query.</typeparam>
public sealed class QueryAsyncValidationDecorator<TFilter, TResult> : IAsyncQueryHandler<TFilter, TResult>
    where TFilter : IFilter where TResult : class
{
    private readonly IAsyncQueryHandler<TFilter, TResult> queryHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryAsyncValidationDecorator{TFilter, TResult}"/> class.
    /// </summary>
    /// <param name="queryHandler">The asynchronous query handler to be decorated.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="queryHandler"/> is null.</exception>
    public QueryAsyncValidationDecorator(IAsyncQueryHandler<TFilter, TResult> queryHandler)
    {
        this.queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
    }

    /// <summary>
    /// Handles the specified query asynchronously after ensuring its validity.
    /// </summary>
    /// <param name="filter">The filter for the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous handling of the query and the result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="filter"/> is null.</exception>
    public async Task<TResult> HandleAsync(TFilter filter, CancellationToken cancellationToken = default)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        filter.EnsureIsValid();

        return await queryHandler.HandleAsync(filter, cancellationToken).ConfigureAwait(false);
    }
}