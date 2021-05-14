// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-12
// Contact: michel@innovt.com.br or michelmob@gmail.com
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Innovt.Domain.Security;

namespace Innovt.Authorization.Platform.Infrastructure
{
    public class SecurityRepository: ISecurityRepository
    {
        public Task AddPermission(Permission permission)
        {
            throw new NotImplementedException();
        }

        public Task RemovePermission(Guid permissionId)
        {
            throw new NotImplementedException();
        }

        public Task AddRole(Role role)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRole(Guid roleId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRole(Role role)
        {
            throw new NotImplementedException();
        }

        public Task AddGroup(Group @group)
        {
            throw new NotImplementedException();
        }

        public Task UpdateGroup(Group @group)
        {
            throw new NotImplementedException();
        }

        public Task RemoveGroup(Guid groupId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Group>> GetGroupsBy(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Group>> GetUserGroups(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Permission>> GetPermissionsBy(string domain = null, string resource = null, string name = null)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Permission>> GetUserPermissions(string userId, string domain = null, string resource = null)
        {
            throw new NotImplementedException();
        }
    }
}