// TotalAcesso.Admin.Web

using System.Linq;
using Innovt.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Innovt.AspNetCore.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled) return;

            if (!(context.Exception is BusinessException)) return;

            var bEx = (BusinessException) context.Exception;

            if (bEx.Errors.Any())
            {
                foreach (var error in bEx.Errors)
                {
                    context.ModelState.AddModelError(error.PropertyName, error.Message);
                }
            }
            else
            {
                context.ModelState.AddModelError("", bEx.Message);
            }

            context.Result = new ViewResult
            {
                ViewName = ((RouteData) context.RouteData).Values["action"].ToString()
            };
            context.ExceptionHandled = true;

            base.OnException(context);
        }
    }
}