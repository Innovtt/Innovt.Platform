// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Security
{
    public interface ISecurityRepository
    {
        Task AddPermission(Permission permission, CancellationToken cancellationToken = default);

        Task RemovePermission(Guid permissionId, CancellationToken cancellationToken = default);

        Task AddRole(Role role, CancellationToken cancellationToken = default);

        Task RemoveRole(Guid roleId, CancellationToken cancellationToken = default);

        Task UpdateRole(Role role, CancellationToken cancellationToken = default);
        
        Task AddGroup(Group group, CancellationToken cancellationToken = default);

        Task UpdateGroup(Group group, CancellationToken cancellationToken = default);

        Task RemoveGroup(Guid groupId, CancellationToken cancellationToken = default);

        Task<IList<Group>> GetGroupsBy(string name, CancellationToken cancellationToken = default);//todo filter
        
        Task<IList<Group>> GetUserGroups(string userId, CancellationToken cancellationToken = default);
        
        Task<IList<Permission>> GetPermissionsBy(string domain = null, string resource = null, string name = null, CancellationToken cancellationToken = default);

        Task<IList<Permission>> GetUserPermissions(string userId, string domain = null, string resource = null, CancellationToken cancellationToken = default);
    }
}