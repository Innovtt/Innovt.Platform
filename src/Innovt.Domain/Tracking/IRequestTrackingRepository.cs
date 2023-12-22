// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System.Threading.Tasks;

namespace Innovt.Domain.Tracking;

/// <summary>
/// Interface for a repository responsible for request tracking operations.
/// </summary>
public interface IRequestTrackingRepository
{
    /// <summary>
    /// Adds a request tracking record.
    /// </summary>
    /// <param name="tracking">The request tracking object to be added.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddTracking(RequestTracking tracking);
}