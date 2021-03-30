using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Cqrs.Queries
{
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
}