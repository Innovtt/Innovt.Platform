using Innovt.Domain.Core.Repository;
using Innovt.Domain.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Innovt.Data.EFCore.Repositories
{
    public class SecurityRepository : RepositoryBase<SecurityGroup>, ISecurityRepository
    {
        public SecurityRepository(IExtendedUnitOfWork context) : base(context)
        {
        }

        public async Task AddPermission(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            await Context.AddAsync(permission);
        }

        public async Task AddPolicy(Policy policy)
        {
            if (policy == null) throw new ArgumentNullException(nameof(policy));

            await Context.AddAsync(policy);
        }

        public async Task<IList<Permission>> GetPermissionsBy(string domain = null, string resource = null,
            string name = null)
        {
            var query = Context.Queryable<Permission>();

            if (!string.IsNullOrEmpty(domain))
                query = query.Where(p => p.Domain.Contains(domain));

            if (!string.IsNullOrEmpty(resource))
                query = query.Where(p => p.Resource.Contains(resource));

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            return await query.ToListAsync();
        }

        public async Task<IList<Permission>> GetUserPermissions(string userId, string domain = null,
            string resource = null)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            var dbSet = Context.Queryable<SecurityGroupUser>().Where(sg => sg.UserId.ToString() == userId)
                .Include("SecurityGroup.Policies.Policy.Permissions");


            var policyPermissions = from p in dbSet
                                    select p.SecurityGroup.Policies.SelectMany(po => po.Policy.Permissions);

            var permissions = await policyPermissions.SelectMany(po => po.Select(p => p.Permission)).ToListAsync();


            if (!string.IsNullOrEmpty(domain))
                permissions = permissions.Where(p => p.Domain == domain).ToList();

            if (!string.IsNullOrEmpty(resource))
                permissions = permissions.Where(p => p.Resource == resource).ToList();

            return permissions.ToList();
        }


        public async Task<IList<Policy>> GetPolicies(string name = null, string description = null)
        {
            var query = Context.Queryable<Policy>();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrEmpty(description))
                query = query.Where(p => p.Description.Contains(description));

            return await query.ToListAsync();
        }

        public async Task RemovePermission(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            Context.Remove(permission);

            await Task.CompletedTask;
        }

        public async Task RemovePolicy(Policy policy)
        {
            if (policy == null) throw new ArgumentNullException(nameof(policy));

            Context.Remove(policy);

            await Task.CompletedTask;
        }
    }
}