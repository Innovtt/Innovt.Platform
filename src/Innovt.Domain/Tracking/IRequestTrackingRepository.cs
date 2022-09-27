// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System.Threading.Tasks;

namespace Innovt.Domain.Tracking;

public interface IRequestTrackingRepository
{
    Task AddTracking(RequestTracking tracking);
}