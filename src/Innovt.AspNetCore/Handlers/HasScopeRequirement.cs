using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

// Code provided by tiago@innovt.com.br and welbert@antecipa.com
//
namespace Innovt.AspNetCore.Handlers
{
    public class HasScopeRequirement : AuthorizationHandler<HasScopeRequirement>, IAuthorizationRequirement
    {
        private string Issuer { get; set; }

        private string Scope { get; set; }

        public HasScopeRequirement()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="issuer"></param>
        public HasScopeRequirement(string scope, string issuer)
        {
            this.Scope = scope;
            this.Issuer = issuer;
        }

        /// <summary>
        /// HandleRequirementAsync
        /// </summary>
        /// <param name="context">AuthorizationHandlerContext</param>
        /// <param name="requirement">HasScopeRequirement</param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            HasScopeRequirement requirement)
        {
            var controller =
                ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)
                    ((Microsoft.AspNetCore.Mvc.ActionContext) context.Resource).ActionDescriptor).ControllerName;
            var action =
                ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)
                    ((Microsoft.AspNetCore.Mvc.ActionContext) context.Resource).ActionDescriptor).ActionName;

            // First check for permissions, they may show up in addition to or instead of scopes...
            if (context.User.HasClaim(c =>
                c.Type == "permissions" && c.Issuer == requirement.Issuer &&
                c.Value?.ToLower(CultureInfo.CurrentCulture) ==
                $"{Scope}:{controller}:{action}".ToLower(CultureInfo.CurrentCulture)))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // This is the Auth0 version, which only checks for scopes instead of permissions.
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value
                .Split(' ');

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}