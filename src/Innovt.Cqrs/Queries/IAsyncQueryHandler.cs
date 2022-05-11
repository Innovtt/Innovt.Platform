// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cqrs
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Cqrs.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cqrs.Queries;

public interface IAsyncQueryHandler<in TFilter, TResult> where TFilter : IFilter where TResult : class
{
    Task<TResult> HandleAsync(TFilter filter, CancellationToken cancellationToken = default);
}

public interface ICountAsyncQueryHandler<in T> where T : IFilter
{
    Task<int> CountAsync(T filter, CancellationToken cancellationToken = default);
}

public interface IExistAsyncQueryHandler<in T> where T : IFilter
{
    Task<bool> ExistAsync(T filter, CancellationToken cancellationToken = default);
}