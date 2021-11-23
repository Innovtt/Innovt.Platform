// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cqrs
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Cqrs.Queries
{
    public interface IAsyncQueryHandler<in TFilter, TResult> where TFilter : IPagedFilter where TResult : class
    {
        Task<TResult> HandleAsync(TFilter filter, CancellationToken cancellationToken = default);
    }

    public interface ICountAsyncQueryHandler<in T> where T : IPagedFilter
    {
        Task<int> CountAsync(T filter, CancellationToken cancellationToken = default);
    }

    public interface IExistAsyncQueryHandler<in T> where T : IPagedFilter
    {
        Task<bool> ExistAsync(T filter, CancellationToken cancellationToken = default);
    }
}