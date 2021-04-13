// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cqrs
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.Validation;

namespace Innovt.Cqrs.Queries.Decorators
{
    public sealed class QueryValidationDecorator<TFilter, TResult> : IQueryHandler<TFilter, TResult>
        where TFilter : IFilter where TResult : class
    {
        private readonly IQueryHandler<TFilter, TResult> queryHandler;

        public QueryValidationDecorator(IQueryHandler<TFilter, TResult> queryHandler)
        {
            this.queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
        }

        public TResult Handle(TFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            filter.EnsureIsValid();

            return queryHandler.Handle(filter);
        }
    }
}