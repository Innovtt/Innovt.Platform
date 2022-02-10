// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

// Code provided by tiago@innovt.com.br and welbert@antecipa.com
//
namespace Innovt.AspNetCore.Handlers
{
    public class HasScopeRequirement : AuthorizationHandler<HasScopeRequirement>, IAuthorizationRequirement
    {
        private string Issuer { get; }
        private string Scope { get; }

        public HasScopeRequirement():this(string.Empty,string.Empty)
        {
            
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="issuer"></param>
        public HasScopeRequirement(string scope, string issuer)
        {
            Scope = scope;
            Issuer = issuer;
        }

  

        /// <summary>
        ///     HandleRequirementAsync
        /// </summary>
        /// <param name="context">AuthorizationHandlerContext</param>
        /// <param name="requirement">HasScopeRequirement</param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            HasScopeRequirement requirement)
        {
            var actionContext = context?.Resource as ActionContext;

            if (context is null || actionContext is null)
                return Task.CompletedTask;

            var controller =
                (ControllerActionDescriptor) actionContext.ActionDescriptor;


            var actionName = controller.ActionName;
            var controllerName = controller.ControllerName;

            // First check for permissions, they may show up in addition to or instead of scopes...
            if (context.User.HasClaim(c =>
                c.Type == "permissions" && c.Issuer == requirement.Issuer &&
                c.Value?.ToLower(CultureInfo.CurrentCulture) ==
                $"{Scope}:{controllerName}:{actionName}".ToLower(CultureInfo.CurrentCulture)))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // This is the Auth0 version, which only checks for scopes instead of permissions.
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer)
                ?.Value
                .Split(' ');


            if (scopes == null)
                return Task.CompletedTask;

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}