// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cqrs
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Cqrs.Queries;
using Innovt.Core.CrossCutting.Log;
using Innovt.Cqrs.Decorators;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cqrs.Queries.Decorators
{
    public sealed class DatabaseAsyncRetryDecorator<TFilter, TResult> : BaseDatabaseRetryDecorator,
        IAsyncQueryHandler<TFilter, TResult> where TFilter : IFilter where TResult : class
    {
        private readonly IAsyncQueryHandler<TFilter, TResult> queryHandler;

        public DatabaseAsyncRetryDecorator(IAsyncQueryHandler<TFilter, TResult> queryHandler, ILogger logger,
            int retryCount = 3) : base(logger, retryCount)
        {
            this.queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
        }


        public Task<TResult> HandleAsync(TFilter filter, CancellationToken cancellationToken = default)
        {
            return CreateAsyncPolicy()
                .ExecuteAsync(async () => await queryHandler.HandleAsync(filter, cancellationToken));
        }
    }
}