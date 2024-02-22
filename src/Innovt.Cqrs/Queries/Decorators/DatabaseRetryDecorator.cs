// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.CrossCutting.Log;
using Innovt.Cqrs.Decorators;

namespace Innovt.Cqrs.Queries.Decorators;

/// <summary>
///     Decorates a query handler to include retry logic in case of failures.
/// </summary>
/// <typeparam name="TFilter">The type of filter for the query.</typeparam>
/// <typeparam name="TResult">The type of result expected from the query.</typeparam>
public sealed class DatabaseRetryDecorator<TFilter, TResult> : BaseDatabaseRetryDecorator,
    IQueryHandler<TFilter, TResult> where TFilter : IPagedFilter where TResult : class
{
    private readonly IQueryHandler<TFilter, TResult> queryHandler;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DatabaseRetryDecorator{TFilter, TResult}" /> class.
    /// </summary>
    /// <param name="queryHandler">The query handler to be decorated.</param>
    /// <param name="logger">The logger for capturing retry attempts.</param>
    /// <param name="retryCount">The number of retry attempts (default is 3).</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="queryHandler" /> or <paramref name="logger" /> is
    ///     null.
    /// </exception>
    public DatabaseRetryDecorator(IQueryHandler<TFilter, TResult> queryHandler, ILogger logger, int retryCount = 3)
        : base(logger, retryCount)
    {
        this.queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
    }

    /// <summary>
    ///     Handles the specified query with retry logic in case of failures.
    /// </summary>
    /// <param name="filter">The filter for the query.</param>
    /// <returns>The result of handling the query.</returns>
    public TResult Handle(TFilter filter)
    {
        return CreatePolicy().Execute(() => queryHandler.Handle(filter));
    }
}