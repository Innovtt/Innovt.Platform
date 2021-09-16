using System;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Innovt.Domain.Security;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Contrib.Authorization.Platform.Domain
{
    public interface IAuthorizationRepository:  Innovt.Domain.Security.IAuthorizationRepository
    {
        Task Save(AdminUser adminUser, CancellationToken cancellationToken);

        Task<AuthUser> GetUserById(UserByIdFilter filter, CancellationToken cancellationToken);


        Task<IList<Role>> GetRoleBy(RoleFilter roleFilter, CancellationToken cancellationToken);

        Task<AdminUser> GetAdminUser(UserFilter userFilter, CancellationToken cancellationToken);

        
        Task AddPermission(Permission permission, CancellationToken cancellationToken = default);

        Task RemovePermission(Permission permission, CancellationToken cancellationToken = default);

        Task<Permission> GetPermissionsById(Guid permissionId, CancellationToken cancellationToken = default);

        Task AddRole(Role role, CancellationToken cancellationToken = default);

        Task RemoveRole(Role role, CancellationToken cancellationToken = default);

        Task<Role> GetRoleById(Guid roleId, CancellationToken cancellationToken = default);

        Task<Role> GetRoleByName(string name, CancellationToken cancellationToken = default);

        Task UpdateRole(Role role, CancellationToken cancellationToken = default);

        Task AddGroup(Group group, CancellationToken cancellationToken = default);

        Task UpdateGroup(Group group, CancellationToken cancellationToken = default);

        Task<Group> GetGroupById(Guid groupId, CancellationToken cancellationToken = default);

        Task<Group> GetGroupBy(string name, string scope, CancellationToken cancellationToken = default);

        Task RemoveGroup(Group group, CancellationToken cancellationToken = default);

        Task<IList<Group>> GetGroupsBy(string name, CancellationToken cancellationToken = default); //todo filter

        Task<IList<Group>> GetUserGroups(string userId, CancellationToken cancellationToken = default);

        Task<IList<Permission>> GetPermissionsBy(string scope = null, string resource = null, string name = null,CancellationToken cancellationToken = default);

    }
}
