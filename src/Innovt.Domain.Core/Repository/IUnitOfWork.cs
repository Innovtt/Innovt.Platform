// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        void Rollback();
    }
}