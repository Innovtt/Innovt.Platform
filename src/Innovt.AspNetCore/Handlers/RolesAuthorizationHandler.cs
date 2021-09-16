// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Innovt.AspNetCore.Extensions;
using Innovt.Core.Collections;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Innovt.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

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

            //var roles = requirement.AllowedRoles;
            var user = await securityRepository.GetUser(userId, CancellationToken.None).ConfigureAwait(false);
            
            if (user is null)
            {
                Fail(context, $"User of id {userId} not found for role authorization.");
                return;
            }

            var roles = user.Groups.SelectMany(g => g.Roles).ToList();
            
            if (roles.IsNullOrEmpty())
            {
                Fail(context, $"User of id {userId} has no roles defined.");
                return;
            }

            var hasPermission = roles.Any(r => requirement.AllowedRoles.Contains(r.Name));
            
            if (hasPermission)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}