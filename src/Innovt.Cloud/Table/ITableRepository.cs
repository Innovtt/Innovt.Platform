using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.Table
{
    public interface ITableRepository
    {
        Task<T> GetByIdAsync<T>(object hashKey, string partitionKey, CancellationToken cancellationToken = default) where T : ITableMessage;

        Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default) where T : ITableMessage;

        Task DeleteAsync<T>(object hashKey, string partitionKey, CancellationToken cancellationToken = default) where T : ITableMessage;

        Task AddAsync<T>(T message, CancellationToken cancellationToken = default) where T : ITableMessage;

        Task AddAsync<T>(IList<T> message, CancellationToken cancellationToken = default) where T : ITableMessage;

        Task<IList<T>> QueryAsync<T>(object hashKey, CancellationToken cancellationToken = default) where T : ITableMessage;

    }
}