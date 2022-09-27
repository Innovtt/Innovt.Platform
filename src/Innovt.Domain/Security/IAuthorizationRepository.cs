// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Security;

public interface IAuthorizationRepository
{
    Task<AuthUser> GetUserByExternalId(string externalId, CancellationToken cancellationToken = default);
}