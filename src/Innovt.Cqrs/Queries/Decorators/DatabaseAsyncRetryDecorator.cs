// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.CrossCutting.Log;
using Innovt.Cqrs.Decorators;

namespace Innovt.Cqrs.Queries.Decorators;
/// <summary>
/// Decorates an asynchronous query handler to include retry logic in case of failures.
/// </summary>
/// <typeparam name="TFilter">The type of filter for the query.</typeparam>
/// <typeparam name="TResult">The type of result expected from the query.</typeparam>
public sealed class DatabaseAsyncRetryDecorator<TFilter, TResult> : BaseDatabaseRetryDecorator,
    IAsyncQueryHandler<TFilter, TResult> where TFilter : IFilter where TResult : class
{
    private readonly IAsyncQueryHandler<TFilter, TResult> queryHandler;
    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseAsyncRetryDecorator{TFilter, TResult}"/> class.
    /// </summary>
    /// <param name="queryHandler">The asynchronous query handler to be decorated.</param>
    /// <param name="logger">The logger for capturing retry attempts.</param>
    /// <param name="retryCount">The number of retry attempts (default is 3).</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="queryHandler"/> or <paramref name="logger"/> is null.</exception>
    public DatabaseAsyncRetryDecorator(IAsyncQueryHandler<TFilter, TResult> queryHandler, ILogger logger,
        int retryCount = 3) : base(logger, retryCount)
    {
        this.queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
    }

    /// <summary>
    /// Handles the specified query asynchronously with retry logic in case of failures.
    /// </summary>
    /// <param name="filter">The filter for the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous handling of the query and the result.</returns>
    public Task<TResult> HandleAsync(TFilter filter, CancellationToken cancellationToken = default)
    {
        return CreateAsyncPolicy()
            .ExecuteAsync(async () => await queryHandler.HandleAsync(filter, cancellationToken));
    }
}