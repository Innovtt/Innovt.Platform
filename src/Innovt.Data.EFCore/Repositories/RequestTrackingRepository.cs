using System.Threading.Tasks;
using Innovt.Domain.Core.Repository;
using Innovt.Domain.Tracking;


namespace Innovt.Data.EFCore.Repositories
{
    public class RequestTrackingRepository : IRequestTrackingRepository
    {
        private readonly IExtendedUnitOfWork context;

        public RequestTrackingRepository(IExtendedUnitOfWork context)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task AddTracking(RequestTracking tracking)
        {
            await context.AddAsync(tracking);

            await context.CommitAsync();
        }
    }
}