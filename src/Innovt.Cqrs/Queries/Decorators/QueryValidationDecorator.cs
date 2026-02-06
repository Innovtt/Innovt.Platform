// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.Validation;

namespace Innovt.Cqrs.Queries.Decorators;

/// <summary>
///     Decorates a query handler to include validation before handling the query.
/// </summary>
/// <typeparam name="TFilter">The type of filter for the query.</typeparam>
/// <typeparam name="TResult">The type of result expected from the query.</typeparam>
public sealed class QueryValidationDecorator<TFilter, TResult> : IQueryHandler<TFilter, TResult>
    where TFilter : IFilter where TResult : class
{
    private readonly IQueryHandler<TFilter, TResult> queryHandler;

    /// <summary>
    ///     Initializes a new instance of the <see cref="QueryValidationDecorator{TFilter, TResult}" /> class.
    /// </summary>
    /// <param name="queryHandler">The query handler to be decorated.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="queryHandler" /> is null.</exception>
    public QueryValidationDecorator(IQueryHandler<TFilter, TResult> queryHandler)
    {
        this.queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
    }

    /// <summary>
    ///     Handles the specified query after ensuring its validity.
    /// </summary>
    /// <param name="filter">The filter for the query.</param>
    /// <returns>The result of handling the query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="filter" /> is null.</exception>
    public TResult Handle(TFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        filter.EnsureIsValid();

        return queryHandler.Handle(filter);
    }
}