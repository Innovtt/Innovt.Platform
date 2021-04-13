// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Innovt.AspNetCore.Extensions;
using Innovt.Core.Utilities;
using Innovt.Domain.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innovt.AspNetCore.Filters
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter, IAuthorizationFilter
    {
        private readonly ISecurityRepository securityRepository;

        public AuthorizationFilter(ISecurityRepository securityRepository)
        {
            this.securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (AllowAnonymous(context))
                return;

            if (!IsUserAuthenticated(context))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = GetUserId(context);

            if (userId.IsNullOrEmpty()) throw new InvalidOperationException("No Claim SID found for loggerd user.");


            var (area, controller, action) = GetActionInfo(context);

            // Se ele tem o /controller ele tem permissao em todo o controller
            // sele etem permissao no /area ele tem permissar na area toda
            // se ele tem permissar no /action ele tem apenas na action 
            // as permissoes sao em cascata : ex se ele tem no action entao é geral 
            // Module can be area
            // Admin /resource * 
            // Admin/
            // * - mean that you want to authorize the full path/domain
            // Controller/* mean that you can althorize all actions
            // Controller/Action mean that you want to authorize only this action
            var hasPermission = await HasPermission(userId, area, controller, action).ConfigureAwait(false);

            if (!hasPermission)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            context.Result = new ForbidResult();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            AsyncHelper.RunSync(async () => await OnAuthorizationAsync(context).ConfigureAwait(false));
        }

        private static bool AllowAnonymous(FilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                return true;

            return false;
        }


        private static bool IsUserAuthenticated(AuthorizationFilterContext context)
        {
            if (context?.HttpContext.User.Identity is null)
                return false;

            return context.HttpContext.User.Identity.IsAuthenticated;
        }


        private static string GetUserId(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User?.GetClaim(ClaimTypes.Sid);

            return userId ?? string.Empty;
        }


        private static (string area, string controller, string action) GetActionInfo(AuthorizationFilterContext context)
        {
            var controllerActionDescriptor = (ControllerActionDescriptor) context.ActionDescriptor;

            var area = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue;

            var controller = controllerActionDescriptor.ControllerName;

            var action = controllerActionDescriptor.ActionName;

            return (area, controller, action);
        }


        private async Task<bool> HasPermission(string userId, string module, string controller, string action)
        {
            var userPermissions = await securityRepository.GetUserPermissions(userId).ConfigureAwait(false);

            if (!userPermissions.Any())
                return false;

            if (module.IsNotNullOrEmpty())
            {
                //permission to full module
                var hasModulePermission = userPermissions.Any(p => p.Domain == module && p.Resource == $"{module}/*");

                if (hasModulePermission)
                    return true;
            }

            //permission to full controller
            var hasControllerPermission = userPermissions.Any(p => p.Resource == $"{controller}/*");

            if (hasControllerPermission)
                return true;


            //permission to specific action
            var hasActionPermission = userPermissions.Any(p => p.Resource == $"{controller}/{action}");

            return hasActionPermission;
        }
    }
}