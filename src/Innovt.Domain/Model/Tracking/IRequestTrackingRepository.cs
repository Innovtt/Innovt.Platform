using System.Threading.Tasks;

namespace Innovt.Domain.Model.Tracking
{
    public interface IRequestTrackingRepository
    {
        Task AddTracking(RequestTracking traking);
    }
}
