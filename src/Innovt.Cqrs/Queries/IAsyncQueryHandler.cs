using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Cqrs.Queries
{
    public interface IAsyncQueryHandler<T, R> where T : IFilter where R : class
    {
        Task<R> HandleAsync(T filter, CancellationToken cancellationToken = default);
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