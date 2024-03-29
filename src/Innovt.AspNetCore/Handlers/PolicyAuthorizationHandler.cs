﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

//using System;
//using System.Linq;
//using System.Reflection;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Innovt.AspNetCore.Extensions;
//using Innovt.Core.Utilities;
//using Innovt.Domain.Security;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization.Infrastructure;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Authorization;
//using Microsoft.AspNetCore.Mvc.Controllers;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace Innovt.AspNetCore.Handlers
//{
//    public class PolicyAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
//    {
//        private readonly IAuthorizationRepository securityRepository;

//        public PolicyAuthorizationHandler(IAuthorizationRepository securityRepository)
//        {
//            this.securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));
//        }

//        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
//        {
//            if (context == null) throw new ArgumentNullException(nameof(context));

//            if (AllowAnonymous(context))
//                return;

//            if (!IsUserAuthenticated(context))
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }

//            var userId = GetUserId(context);

//            if (userId.IsNullOrEmpty()) throw new InvalidOperationException("No Claim SID found for loggerd user.");


//            var (area, controller, action) = GetActionInfo(context);

//            // Se ele tem o /controller ele tem permissao em todo o controller
//            // sele etem permissao no /area ele tem permissar na area toda
//            // se ele tem permissar no /action ele tem apenas na action 
//            // as permissoes sao em cascata : ex se ele tem no action entao é geral 
//            // Module can be area
//            // Admin /resource * 
//            // Admin/
//            // * - mean that you want to authorize the full path/domain
//            // Controller/* mean that you can althorize all actions
//            // Controller/Action mean that you want to authorize only this action
//            var hasPermission = await HasPermission(userId, area, controller, action).ConfigureAwait(false);

//            if (!hasPermission)
//            {
//                context.Result = new UnauthorizedResult();
//                return;
//            }

//            context.Result = new ForbidResult();
//        }

//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//            AsyncHelper.RunSync(async () => await OnAuthorizationAsync(context).ConfigureAwait(false));
//        }

//        private static bool AllowAnonymous(FilterContext context)
//        {
//            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
//                return true;

//            return false;
//        }


//        private static bool IsUserAuthenticated(AuthorizationHandlerContext context)
//        {
//            if (context?.User.Identity is null)
//                return false;

//            return context.User.Identity.IsAuthenticated;
//        }

//        private static string GetUserId(AuthorizationFilterContext context)
//        {
//            var userId = context.HttpContext.User?.GetClaim(ClaimTypes.Sid);

//            return userId ?? string.Empty;
//        }


//        private static (string scope, string controller, string action) GetActionInfo(AuthorizationFilterContext context)
//        {
//            var controllerActionDescriptor = (ControllerActionDescriptor) context.ActionDescriptor;

//            var scope = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue;

//            var controller = controllerActionDescriptor.ControllerName;

//            var action = controllerActionDescriptor.ActionName;

//            return (scope, controller, action);
//        }


//        private async Task<bool> HasPermission(string userId, string scope, string controller, string action)
//        {
//            var userPermissions = await securityRepository.GetUser(userId,scope).ConfigureAwait(false);
//            //.GetUserPermissions(userId).ConfigureAwait(false);


//            return true;
//            //if (!userPermissions.Any())
//            //    return false;

//            //if (scope.IsNotNullOrEmpty())
//            //{
//            //    //permission to full module
//            //    var hasModulePermission = userPermissions.Any(p => p.Scope == scope && p.Resource == $"{scope}/*");

//            //    if (hasModulePermission)
//            //        return true;
//            //}

//            ////permission to full controller
//            //var hasControllerPermission = userPermissions.Any(p => p.Resource == $"{controller}/*");

//            //if (hasControllerPermission)
//            //    return true;

//            ////permission to specific action
//            //var hasActionPermission = userPermissions.Any(p => p.Resource == $"{controller}/{action}");

//            //return hasActionPermission;
//        }

//        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
//        {

//            if (!IsUserAuthenticated(context))
//            {
//                context.Fail();
//                return;
//            }


//            throw new NotImplementedException();
//        }
//    }
//}

