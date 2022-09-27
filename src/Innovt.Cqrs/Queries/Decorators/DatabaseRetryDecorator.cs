// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.CrossCutting.Log;
using Innovt.Cqrs.Decorators;

namespace Innovt.Cqrs.Queries.Decorators;

public sealed class DatabaseRetryDecorator<TFilter, TResult> : BaseDatabaseRetryDecorator,
    IQueryHandler<TFilter, TResult> where TFilter : IPagedFilter where TResult : class
{
    private readonly IQueryHandler<TFilter, TResult> queryHandler;

    public DatabaseRetryDecorator(IQueryHandler<TFilter, TResult> queryHandler, ILogger logger, int retryCount = 3)
        : base(logger, retryCount)
    {
        this.queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
    }

    public TResult Handle(TFilter filter)
    {
        return CreatePolicy().Execute(() => queryHandler.Handle(filter));
    }
}