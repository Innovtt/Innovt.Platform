// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-12
// Contact: michel@innovt.com.br or michelmob@gmail.com
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Authorization.Platform.Infrastructure.DataModel;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Innovt.Domain.Security;

namespace Innovt.Authorization.Platform.Infrastructure
{
    public class SecurityRepository : Repository, ISecurityRepository
    {
        public SecurityRepository(ILogger logger, IAWSConfiguration awsConfiguration) : base(logger, awsConfiguration)
        {
        }

        public async Task AddPermission(Permission permission,CancellationToken cancellationToken = default)
        {
            Check.NotNull(permission, nameof(permission));
            
            await AddAsync(PermissionDataModel.FromPermission(permission), cancellationToken).ConfigureAwait(false);
        }

        public Task RemovePermission(Guid permissionId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task AddRole(Role role, CancellationToken cancellationToken = default)
        {
            Check.NotNull(role, nameof(role));

            await AddAsync(RoleDataModel.FromRole(role), cancellationToken).ConfigureAwait(false);
        }

        public Task RemoveRole(Guid roleId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRole(Role role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task AddGroup(Group group, CancellationToken cancellationToken = default)
        {
            Check.NotNull(group, nameof(group));

            await AddAsync(GroupDataModel.FromGroup(group), cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateGroup(Group group, CancellationToken cancellationToken = default)
        {
            Check.NotNull(group, nameof(group));

            //await UpdateAsync(GroupDataModel.FromGroup(group), cancellationToken).ConfigureAwait(false);
        }

        public Task RemoveGroup(Guid groupId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Group>> GetGroupsBy(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Group>> GetUserGroups(string userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Permission>> GetPermissionsBy(string domain = null, string resource = null, string name = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Permission>> GetUserPermissions(string userId, string domain = null, string resource = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        //public Task<IList<Permission>> GetUserPermissions(string userId, string domain = null, string resource = null)
        //{
        //    if (categoryFilter == null) throw new ArgumentNullException(nameof(categoryFilter));

        //    var request = new Innovt.Cloud.Table.QueryRequest()
        //    {
        //        KeyConditionExpression = $"PK = :pk AND begins_with(SK,:sk)",
        //        Filter = new { pk = $"C#{categoryFilter.UserIdentity.CompanyId}", sk = $"S#True#CAT#" },
        //        AttributesToGet = "CategoryName,CategoryIconUrl,CategoryId"
        //    };

        //    var category = await base.QueryAsync<DashboardDataModel>(request, cancellationToken).ConfigureAwait(false);

        //    return CategoryDataModel.ToCategory(category);
        //}
    }
}