using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.Table
{
    public interface ITableService<T> where T : ITableMessage
    {
        string TableName { get; }

        Task<T> GetByIdAsync(string id, string partitionKey, CancellationToken cancellationToken = default);

        T GetById(string id, string partitionKey);

        Task DeleteAsync(string id, string partitionKey, CancellationToken cancellationToken = default);

        void Delete(string id, string partitionKey);

        Task AddAsync(T message, CancellationToken cancellationToken = default);

        void Add(T message);

        /// <summary>
        /// Using Default Key for partitionKey and ProvisionedThroughput of 5 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateIfNotExistAsync(CancellationToken cancellationToken = default);

        Task<List<T>> QueryAsync(object hashKeyValue, CancellationToken cancellationToken = default);
    }
}