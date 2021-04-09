using System;
using Innovt.Core.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Innovt.AspNetCore.Filters
{
    /// <summary>
    /// This Filter Will inject the username claim in the action Parameters
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class InjectUserNameClaimParameterFilter : ActionFilterAttribute
    {  
        public string DefaultAuthorizationProperty { get; }
        public string[] ActionParameters { get; }

        public InjectUserNameClaimParameterFilter(string defaultAuthorizationProperty, params string[] actionParameters)
        {
            Check.NotNull(defaultAuthorizationProperty, nameof(defaultAuthorizationProperty));
            Check.NotNull(actionParameters, nameof(actionParameters));

            this.DefaultAuthorizationProperty = defaultAuthorizationProperty;
            this.ActionParameters = actionParameters;
        }

        /// <summary>
        /// Using defaults: ExternalId  and "filter","command"
        /// </summary>
        public InjectUserNameClaimParameterFilter() : this("ExternalId", "filter", "command")
        {
        }


        private void InjectUserName(ActionExecutingContext context)
        {

            if(context is null)
                return;

            var userName = context.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userName.IsNullOrEmpty()) return;

            foreach (var actionParameter in ActionParameters)
            {
                context.ActionArguments.TryGetValue(actionParameter, out var inputParam);

                if (inputParam == null) continue;

                var property = inputParam.GetType().GetProperty(DefaultAuthorizationProperty);

                if (property != null) property.SetValue(inputParam, userName);
            }
        }

    

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            InjectUserName(context);
            
            base.OnActionExecuting(context);
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            InjectUserName(context);
            
            //TODO: Not sure yet
            await base.OnActionExecutionAsync(context, next).ConfigureAwait(true);
        }

    }
}