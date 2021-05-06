// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Innovt.Domain.Core.Repository;

namespace Innovt.Domain.Security
{
    public interface ISecurityRepository : IRepository<SecurityGroup>
    {
        Task AddPolicy(Policy policy);

        Task RemovePolicy(Policy policy);

        Task<IList<Policy>> GetPolicies(string name = null, string description = null);

        Task AddPermission(Permission permission);

        Task RemovePermission(Permission permission);

        Task<IList<Permission>> GetPermissionsBy(string domain = null, string resource = null, string name = null);

        Task<IList<Permission>> GetUserPermissions(string userId, string domain = null, string resource = null);
    }
}