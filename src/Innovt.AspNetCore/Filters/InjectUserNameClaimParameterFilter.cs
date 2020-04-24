using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Innovt.Core.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innovt.AspNetCore.Filters
{

    /// <summary>
    /// This Filter Will inject the username claim in the action Parameters
    /// </summary>
    public class InjectUserNameClaimParameterFilter : ActionFilterAttribute
    {
        private readonly string defaultAuthorizationProperty;
        private readonly string[] actionParameters;

        public InjectUserNameClaimParameterFilter(string defaultAuthorizationProperty, params string[] actionParameters)
        {
            Check.NotNull(defaultAuthorizationProperty,nameof(defaultAuthorizationProperty));
            Check.NotNull(actionParameters,nameof(actionParameters));

            this.defaultAuthorizationProperty = defaultAuthorizationProperty;
            this.actionParameters = actionParameters;
        }

        /// <summary>
        /// Using defaults: ExternalId  and "filter","command"
        /// </summary>
        public InjectUserNameClaimParameterFilter():this("ExternalId","filter","command")
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        { 
            var userName = filterContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!userName.IsNullOrEmpty())
            {
                foreach (var actionParameter in actionParameters)
                {
                    filterContext.ActionArguments.TryGetValue(actionParameter, out var inputParam);

                    if (inputParam == null) continue;

                    var property = inputParam.GetType().GetProperty(defaultAuthorizationProperty);

                    if (property != null)
                    {
                        property.SetValue(inputParam, userName);
                    }
                }
            }
            
            await next();
        }
    }
}