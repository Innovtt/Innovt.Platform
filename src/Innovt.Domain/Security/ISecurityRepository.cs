// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Innovt.Domain.Security
{
    public interface ISecurityRepository
    {
        Task AddPermission(Permission permission);

        Task RemovePermission(Guid permissionId);

        Task AddRole(Role role);

        Task RemoveRole(Guid roleId);

        Task UpdateRole(Role role);
        
        Task AddGroup(Group group);

        Task UpdateGroup(Group group);

        Task RemoveGroup(Guid groupId);

        Task<IList<Group>> GetGroupsBy(string name);//todo filter
        
        Task<IList<Group>> GetUserGroups(string userId);
        
        Task<IList<Permission>> GetPermissionsBy(string domain = null, string resource = null, string name = null);

        Task<IList<Permission>> GetUserPermissions(string userId, string domain = null, string resource = null);
    }
}