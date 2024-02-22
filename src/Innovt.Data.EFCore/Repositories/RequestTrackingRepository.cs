// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using System.Threading.Tasks;
using Innovt.Domain.Core.Repository;
using Innovt.Domain.Tracking;

namespace Innovt.Data.EFCore.Repositories;

/// <summary>
///     Repository for managing request tracking entities.
/// </summary>
public class RequestTrackingRepository : IRequestTrackingRepository
{
    private readonly IExtendedUnitOfWork context;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RequestTrackingRepository" /> class.
    /// </summary>
    /// <param name="context">The extended unit of work for data access.</param>
    /// <exception cref="ArgumentNullException">Thrown when the context is null.</exception>
    public RequestTrackingRepository(IExtendedUnitOfWork context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    ///     Adds request tracking information to the repository.
    /// </summary>
    /// <param name="tracking">The request tracking information to be added.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddTracking(RequestTracking tracking)
    {
        await context.AddAsync(tracking).ConfigureAwait(false);

        await context.CommitAsync().ConfigureAwait(false);
    }
}