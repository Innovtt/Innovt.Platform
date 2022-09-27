﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

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
using Innovt.Core.Utilities;
using Innovt.Domain.Security;
using IAuthorizationRepository = Innovt.Contrib.Authorization.Platform.Domain.IAuthorizationRepository;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure;

public class AuthorizationRepository : Repository, IAuthorizationRepository
{
    public AuthorizationRepository(ILogger logger, IAwsConfiguration awsConfiguration) : base(logger,
        awsConfiguration)
    {
    }

    public async Task<AdminUser> GetAdminUser(UserFilter userFilter, CancellationToken cancellationToken)
    {
        if (userFilter is null) throw new ArgumentNullException(nameof(userFilter));

        var request = new QueryRequest
        {
            KeyConditionExpression = "PK=:pk AND SK=:sk",
            Filter = new { pk = $"MU#{userFilter.Email}", sk = "ADMINUSER" }
        };

        var user = await QueryFirstOrDefaultAsync<AdminUserDataModel>(request, cancellationToken)
            .ConfigureAwait(false);

        return AdminUserDataModel.ToUser(user);
    }

    public async Task<IList<Role>> GetUserRolesBy(RoleByUserFilter filter, CancellationToken cancellationToken)
    {
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        var request = new QueryRequest
        {
            KeyConditionExpression = "PK=:pk AND begins_with(SK,:sk)"
        };

        if (filter.ExternalId.IsNotNullOrEmpty())
        {
            request.Filter = new { pk = $"U#{filter.ExternalId}", sk = "DID#" };
        }
        else
        {
            request.IndexName = "SK-PK-Index";
            request.Filter = new { pk = $"DID#{filter.DomainId}", sk = "U#" };
        }

        var user = await QueryFirstOrDefaultAsync<UserDataModel>(request, cancellationToken).ConfigureAwait(false);

        return UserDataModel.ToUser(user)?.Roles;
    }

    public async Task Save(AdminUser adminUser, CancellationToken cancellationToken)
    {
        if (adminUser is null) throw new ArgumentNullException(nameof(adminUser));

        var user = AdminUserDataModel.FromUser(adminUser);

        await AddAsync(user, cancellationToken).ConfigureAwait(false);
    }

    public async Task<AuthUser> GetUserByExternalId(string externalId,
        CancellationToken cancellationToken = default)
    {
        var request = new QueryRequest
        {
            KeyConditionExpression = "PK=:pk AND begins_with(SK,:sk)",
            Filter = new { pk = $"U#{externalId}", sk = "DID#" }
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