// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using System.Threading.Tasks;
using Innovt.Domain.Core.Repository;
using Innovt.Domain.Tracking;

namespace Innovt.Data.EFCore.Repositories;

public class RequestTrackingRepository : IRequestTrackingRepository
{
    private readonly IExtendedUnitOfWork context;

    public RequestTrackingRepository(IExtendedUnitOfWork context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddTracking(RequestTracking tracking)
    {
        await context.AddAsync(tracking).ConfigureAwait(false);

        await context.CommitAsync().ConfigureAwait(false);
    }
}