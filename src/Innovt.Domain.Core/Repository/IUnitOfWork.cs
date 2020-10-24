using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Core.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        int Commit();
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        void Rollback();
    }
}
