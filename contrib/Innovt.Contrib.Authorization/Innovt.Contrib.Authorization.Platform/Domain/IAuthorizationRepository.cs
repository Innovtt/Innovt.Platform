// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-19

using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Domain
{
    public interface IAuthorizationRepository : Innovt.Domain.Security.IAuthorizationRepository
    {
        Task Save(AdminUser adminUser, CancellationToken cancellationToken);

        Task Save(AuthUser user, CancellationToken cancellationToken);

        Task RemoveUser(AuthUser user, CancellationToken cancellationToken);

        Task<AuthUser> GetUserById(UserByIdFilter filter, CancellationToken cancellationToken);

        Task<AdminUser> GetAdminUser(UserFilter userFilter, CancellationToken cancellationToken);
    }
}