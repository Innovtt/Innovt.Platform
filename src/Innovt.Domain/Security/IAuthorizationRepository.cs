// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Security;
/// <summary>
/// Represents a repository for authorization-related operations.
/// </summary>
public interface IAuthorizationRepository
{
    /// <summary>
    /// Gets a user by their external identifier.
    /// </summary>
    /// <param name="externalId">The external identifier of the user.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe the operation cancellation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="AuthUser"/> associated with the external identifier.</returns>
    Task<AuthUser> GetUserByExternalId(string externalId, CancellationToken cancellationToken = default);
}