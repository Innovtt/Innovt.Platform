﻿// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Innovt.AspNetCore.Extensions;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Innovt.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;


namespace Innovt.AspNetCore.Handlers
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private readonly IAuthorizationRepository securityRepository;
        private readonly ILogger logger;

        public RolesAuthorizationHandler(IAuthorizationRepository securityRepository, ILogger logger)
        {
            this.securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private static string GetUserId(AuthorizationHandlerContext context)
        {
            var userId = context.User?.GetClaim(ClaimTypes.Sid);

            return userId ?? string.Empty;
        }

        private static void SetUserDomainId(AuthUser authUser,AuthorizationHandlerContext context)
        {
            context.User.AddIdentity(new ClaimsIdentity(new[] { new Claim(ClaimTypes.PrimarySid, authUser.DomainId) }));
        }

        private static bool IsUserAuthenticated(AuthorizationHandlerContext context)
        {
            if (context?.User.Identity is null)
                return false;

            return context.User.Identity.IsAuthenticated;
        }


        private void Fail(AuthorizationHandlerContext context, string reason)
        {  
            logger.Warning(reason);
            context.Fail();
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            if (!IsUserAuthenticated(context) || requirement is null)
            {
                Fail(context, "User not authenticated.");
                return;
            }

            var userId = GetUserId(context);
            
            if (requirement.AllowedRoles?.Any() ==false || userId.IsNullOrEmpty())
            {
                Fail(context, $"Invalid user roles or id.The current user id is {userId}.");
                return;
            }

            var user = await securityRepository.GetUserByExternalId(userId, CancellationToken.None).ConfigureAwait(false);
            
            if (user is null)
            {
                Fail(context, $"User of id {userId} not found for role authorization.");
                return;
            }
            var roles = GetUserRoles(user);
            
            if (roles is null)
            {
                Fail(context, $"User of id {userId} has no roles defined.");
                return;
            }

            var hasPermission = roles.Any(r => requirement.AllowedRoles.Contains(r.Name));
            
            if (hasPermission)
            {
                SetUserDomainId(user,context);

                context.Succeed(requirement);
            }
            else
            {
                Fail(context, $"User of id {userId} has no roles defined.");
                context.Fail();
            }
        }

        private static List<Role> GetUserRoles(AuthUser user)
        {
            var roles = new List<Role>();

            if (user.Roles is { })
                roles.AddRange(user.Roles);

            var groupRoles = user.Groups?.SelectMany(g => g.Roles).ToList();

            if (groupRoles is { })
                roles.AddRange(groupRoles);

            return roles;
        }
    }
}