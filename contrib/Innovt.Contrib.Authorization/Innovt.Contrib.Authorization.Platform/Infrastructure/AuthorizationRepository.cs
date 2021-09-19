// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
using Innovt.Cloud.Table;
using Innovt.Contrib.Authorization.Platform.Domain;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Security;
using IAuthorizationPolicyRepository = Innovt.Contrib.Authorization.Platform.Domain.IAuthorizationPolicyRepository;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure
{
    public class AuthorizationRepository : Repository, IAuthorizationPolicyRepository, IAuthorizationRoleRepository
    {
        public AuthorizationRepository(ILogger logger, IAwsConfiguration awsConfiguration) : base(logger,
            awsConfiguration)
        {
        }

        public async Task AddPermission(Permission permission, CancellationToken cancellationToken = default)
        {
            if (permission is null) throw new ArgumentNullException(nameof(permission));

            var permissionDataModel = PermissionDataModel.FromPermission(permission);

            await AddAsync(permissionDataModel, cancellationToken).ConfigureAwait(false);
        }

        public async Task RemovePermission(Permission permission, CancellationToken cancellationToken = default)
        {
            if (permission is null) throw new ArgumentNullException(nameof(permission));

            var permissionDataModel = PermissionDataModel.FromPermission(permission);

            await DeleteAsync(permissionDataModel, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<Permission>> GetPermissionsBy(string scope = null, string resource = null,
            string name = null,
            CancellationToken cancellationToken = default)
        {
            var request = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk",
                Filter = new { pk = $"P#{resource}", sk = $"S#{scope}" }
            };

            if (resource != null) request.KeyConditionExpression += " AND SK=:sk ";

            var permission = await QueryAsync<PermissionDataModel>(request, cancellationToken).ConfigureAwait(false);

            return PermissionDataModel.ToDomain(permission);
        }

        public async Task<Permission> GetPermissionsById(Guid permissionId,
            CancellationToken cancellationToken = default)
        {
            var request = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk",
                IndexName = "SK-PK-ID"
                //Filter = new { pk = $"P#{resource}", sk = $"S#{scope}" },
            };

            var permission = await QueryFirstOrDefaultAsync<PermissionDataModel>(request, cancellationToken)
                .ConfigureAwait(false);

            return PermissionDataModel.ToDomain(permission);
        }

        public async Task AddRole(Role role, CancellationToken cancellationToken = default)
        {
            if (role is null)
                throw new ArgumentNullException(nameof(role));

            var roleDataModel = RoleDataModel.FromDomain(role);

            await AddAsync(roleDataModel, cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveRole(Role role, CancellationToken cancellationToken = default)
        {
            if (role is null) throw new ArgumentNullException(nameof(role));

            var roleDataModel = RoleDataModel.FromDomain(role);

            await DeleteAsync(roleDataModel, cancellationToken).ConfigureAwait(false);
        }

        public Task<Role> GetRoleById(Guid roleId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetRoleByName(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AuthUser> GetUserById(UserByIdFilter filter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        public Task UpdateRole(Role role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task AddGroup(Group group, CancellationToken cancellationToken = default)
        {
            if (group is null) throw new ArgumentNullException(nameof(@group));

            var groupDataModel = GroupDataModel.FromGroup(group);

            await AddAsync(groupDataModel, cancellationToken).ConfigureAwait(false);
        }

        public Task UpdateGroup(Group group, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Group> GetGroupById(Guid groupId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Group> GetGroupBy(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public Task RemoveGroup(Group group, CancellationToken cancellationToken = default)
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

        public async Task<AdminUser> GetAdminUser(UserFilter userFilter, CancellationToken cancellationToken)
        {
            if (userFilter is null) throw new ArgumentNullException(nameof(userFilter));

            var request = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND SK=:sk",
                Filter = new { pk = $"MU#{userFilter.Email}", sk = "ADMINUSER" }
            };

            var user = await QueryFirstOrDefaultAsync<AdminUserDataModel>(request, cancellationToken).ConfigureAwait(false);

            return AdminUserDataModel.ToUser(user);
        }

        public async Task Save(AdminUser adminUser, CancellationToken cancellationToken)
        {
            if (adminUser is null) throw new ArgumentNullException(nameof(adminUser));

            var user = AdminUserDataModel.FromUser(adminUser);

            await AddAsync(user, cancellationToken).ConfigureAwait(false);
        }

        Task<IList<Role>> IAuthorizationPolicyRepository.GetRoleBy(RoleFilter roleFilter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthUser> GetUser(string userId, CancellationToken cancellationToken = default)
        {
            var request = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND begins_with(SK,:sk)",
                Filter = new { pk = $"U#{userId}", sk = "DID#" }
            };

            var user = await QueryFirstOrDefaultAsync<UserDataModel>(request, cancellationToken).ConfigureAwait(false);

            return UserDataModel.ToUser(user);
        }

        public async Task Save(AuthUser user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            var authUser = UserDataModel.FromUser(user);

            await AddAsync(authUser, cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveUser(AuthUser user, CancellationToken cancellationToken)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            var authUser = UserDataModel.FromUser(user);

            await DeleteAsync(authUser, cancellationToken).ConfigureAwait(false);
        }
    }
}