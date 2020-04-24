using System.Threading.Tasks;
using Innovt.Domain.Model.Tracking;
using Innovt.Domain.Repository;


namespace Innovt.Data.EFCore.Repositories
{

    public class RequestTrackingRepository : IRequestTrackingRepository
    {
        private readonly IExtendedUnitOfWork context;

        public RequestTrackingRepository(IExtendedUnitOfWork context)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task AddTracking(RequestTracking traking)
        {
            await context.AddAsync(traking);

            await context.CommitAsync();
        }
    }
}
