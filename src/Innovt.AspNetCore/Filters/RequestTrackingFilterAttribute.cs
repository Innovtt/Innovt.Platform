using System;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Innovt.AspNetCore.Extensions;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Model.Tracking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innovt.AspNetCore.Filters
{
    public class RequestTrackingFilterAttribute : ActionFilterAttribute
    {
        private readonly IRequestTrackingRepository trackingRepository;
        private ILogger logger;

        private RequestTracking tracking = null;


        public RequestTrackingFilterAttribute(IRequestTrackingRepository  _trackingRepository,ILogger _logger)
        {
            trackingRepository = _trackingRepository ?? throw new System.ArgumentNullException(nameof(_trackingRepository));
            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }
        

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            try
            {

                tracking.ResponseStatusCode = context.HttpContext.Response?.StatusCode;

                await trackingRepository.AddTracking(tracking);
            }
            catch (Exception ex)
            {
                logger.Error(ex,"OnResultExecutionAsync");
            }
            
            await base.OnResultExecutionAsync(context, next);
        }


        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User?.GetClaim(ClaimTypes.Email)  ?? "Anonymous";

            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            
             var area = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue;

            tracking = new RequestTracking()
            {
                UserId = user,
                Area = area,
                Controller = controllerActionDescriptor.ControllerName,
                Action = controllerActionDescriptor.ActionName,
                Verb = context.HttpContext.Request.Method,
                Host = context.HttpContext.Request.Host.ToString()
            };
            
            return base.OnActionExecutionAsync(context, next);
        }
    }
}