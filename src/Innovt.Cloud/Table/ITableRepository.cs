using Innovt.Core.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.Table
{
    public interface ITableRepository
    {
        Task<T> GetByIdAsync<T>(object id, string rangeKey = null, CancellationToken cancellationToken = default)
            where T : ITableMessage;

        Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default) where T : ITableMessage;

        Task DeleteAsync<T>(object id, string rangeKey = null, CancellationToken cancellationToken = default)
            where T : ITableMessage;

        Task AddAsync<T>(T message, CancellationToken cancellationToken = default) where T : ITableMessage;

        Task AddAsync<T>(IList<T> message, CancellationToken cancellationToken = default) where T : ITableMessage;

        Task<T> QueryFirstAsync<T>(object id, CancellationToken cancellationToken = default);

        Task<IList<T>> QueryAsync<T>(object id, CancellationToken cancellationToken = default);

        Task<IList<T>> QueryAsync<T>(Innovt.Cloud.Table.QueryRequest request,
            CancellationToken cancellationToken = default);

        Task<T> QueryFirstOrDefaultAsync<T>(Table.QueryRequest request, CancellationToken cancellationToken = default);

        Task<(List<TResult1> first, List<TResult2> second)> QueryMultipleAsync<T, TResult1, TResult2>(
            Table.QueryRequest request, string splitBy, CancellationToken cancellationToken = default);

        Task<(List<TResult1> first, List<TResult2> second, List<TResult3> third)>
            QueryMultipleAsync<T, TResult1, TResult2, TResult3>(Table.QueryRequest request, string[] splitBy,
                CancellationToken cancellationToken = default);

        Task<IList<T>> ScanAsync<T>(Innovt.Cloud.Table.ScanRequest request,
            CancellationToken cancellationToken = default);

        Task<PagedCollection<T>> ScanPaginatedByAsync<T>(Innovt.Cloud.Table.ScanRequest request,
            CancellationToken cancellationToken = default);

        Task<PagedCollection<T>> QueryPaginatedByAsync<T>(Innovt.Cloud.Table.QueryRequest request,
            CancellationToken cancellationToken = default);

        //where T : ITableMessage;
    }
}