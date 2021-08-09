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
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Security;
using Innovt.Domain.Users;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure
{


    internal class AuthorizationRepository : Repository, IAuthorizationRepository
    {
        public AuthorizationRepository(ILogger logger, IAwsConfiguration awsConfiguration) : base(logger, awsConfiguration)
        {
        }

        //public async Task AddPermission(Permission permission,CancellationToken cancellationToken = default)
        //{
        //    Check.NotNull(permission, nameof(permission));
            
        //    await AddAsync(PermissionDataModel.FromPermission(permission), cancellationToken).ConfigureAwait(false);
        //}

       
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
        public Task AddPermission(Permission permission, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemovePermission(Permission permission, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Permission> GetPermissionsById(Guid permissionId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddRole(Role role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRole(Role role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetRoleById(Guid roleId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetRoleByName(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRole(Role role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddGroup(Group @group, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateGroup(Group @group, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Group> GetGroupById(Guid groupId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Group> GetGroupBy(string name, string domain, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public Task RemoveGroup(Group @group, CancellationToken cancellationToken = default)
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
            //if (categoryFilter == null) throw new ArgumentNullException(nameof(categoryFilter));

            //var request = new Innovt.Cloud.Table.QueryRequest()
            //{
            //    KeyConditionExpression = $"PK = :pk AND begins_with(SK,:sk)",
            //    Filter = new { pk = $"C#", sk = $"S#True#CAT#" },
            //    AttributesToGet = "CategoryName,CategoryIconUrl,CategoryId"
            //};
            //

            //var category = await base.QueryAsync<DashboardDataModel>(request, cancellationToken).ConfigureAwait(false);
            return null;
        }

        public object GetAdminUser(string userName, string password)
        {
            var request = new Innovt.Cloud.Table.QueryRequest()
            {
                KeyConditionExpression = $"PK = :pk",
                Filter = new { pk = $"C#" }                
            };
            
            var user = base.QueryFirstOrDefaultAsync<BaseUser>(request).Result;


            return user;
        }
    }
}