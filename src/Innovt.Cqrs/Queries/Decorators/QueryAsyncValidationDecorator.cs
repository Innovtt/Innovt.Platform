// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cqrs
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.Validation;

namespace Innovt.Cqrs.Queries.Decorators
{
    public sealed class QueryAsyncValidationDecorator<TFilter, TResult> : IAsyncQueryHandler<TFilter, TResult>
        where TFilter : IFilter where TResult : class
    {
        private readonly IAsyncQueryHandler<TFilter, TResult> queryHandler;

        public QueryAsyncValidationDecorator(IAsyncQueryHandler<TFilter, TResult> queryHandler)
        {
            this.queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
        }

        public async Task<TResult> HandleAsync(TFilter filter, CancellationToken cancellationToken = default)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            filter.EnsureIsValid();

            return await queryHandler.HandleAsync(filter, cancellationToken).ConfigureAwait(false);
        }
    }
}