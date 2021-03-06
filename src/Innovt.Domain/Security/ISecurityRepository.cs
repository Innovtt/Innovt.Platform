﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Innovt.Domain.Core.Repository;

namespace Innovt.Domain.Security
{
    public interface ISecurityRepository: IRepository<SecurityGroup>
    {
        Task AddPolicy(Policy policy);

        Task RemovePolicy(Policy policy);

        Task<IList<Policy>> GetPolicies(string name=null, string description = null);

        Task AddPermission(Permission permission);

        Task RemovePermission(Permission permission);

        Task<IList<Permission>> GetPermissionsBy(string domain=null, string resource = null, string name = null);

        Task<IList<Permission>> GetUserPermissions(string userId, string domain = null, string resource = null);


    }
}