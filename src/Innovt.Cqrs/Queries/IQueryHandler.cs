// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cqrs
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Cqrs.Queries;

namespace Innovt.Cqrs.Queries
{
    public interface IQueryHandler<in TFilter, out TResult> where TFilter : IPagedFilter where TResult : class
    {
        TResult Handle(TFilter filter);
    }
}