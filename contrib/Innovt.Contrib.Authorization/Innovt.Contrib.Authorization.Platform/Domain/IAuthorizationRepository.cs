using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Innovt.Domain.Security;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Contrib.Authorization.Platform.Domain
{
    public interface IAuthorizationRepository:  Innovt.Domain.Security.IAuthorizationRepository
    {
        Task<IList<Role>> GetRoleBy(RoleFilter roleFilter, CancellationToken cancellationToken);

        Task<AdminUser> GetAdminUser(UserFilter userFilter, CancellationToken cancellationToken);

        Task Save(AdminUser adminUser, CancellationToken cancellationToken);
    }
}
