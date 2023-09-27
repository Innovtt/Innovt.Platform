// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Core.Repository;
/// <summary>
/// Represents a unit of work for managing transactions and changes.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Commits the changes made within the unit of work.
    /// </summary>
    /// <returns>The number of affected entries.</returns>
    int Commit();
    /// <summary>
    /// Asynchronously commits the changes made within the unit of work.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The number of affected entries.</returns>
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Rolls back any changes made within the unit of work.
    /// </summary>
    void Rollback();
}