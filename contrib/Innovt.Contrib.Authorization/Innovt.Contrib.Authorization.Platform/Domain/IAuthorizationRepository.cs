// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Domain;

public interface IAuthorizationRepository : Innovt.Domain.Security.IAuthorizationRepository
{
    Task Save(AdminUser adminUser, CancellationToken cancellationToken);

    Task Save(AuthUser user, CancellationToken cancellationToken);

    Task RemoveUser(AuthUser user, CancellationToken cancellationToken);

    Task<AdminUser> GetAdminUser(UserFilter userFilter, CancellationToken cancellationToken);
    Task<IList<Role>> GetUserRolesBy(RoleByUserFilter filter, CancellationToken cancellationToken);
}