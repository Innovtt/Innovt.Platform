// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using Innovt.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innovt.AspNetCore.Filters;

public sealed class CustomExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context is null || context.ExceptionHandled) return;

        if (!(context.Exception is BusinessException)) return;

        var bEx = (BusinessException)context.Exception;

        if (bEx.Errors.Any())
            foreach (var error in bEx.Errors)
                context.ModelState.AddModelError(error.PropertyName, error.Message);
        else
            context.ModelState.AddModelError("", bEx.Message);

        context.Result = new ViewResult
        {
            ViewName = context.RouteData?.Values["action"]?.ToString()
        };
        context.ExceptionHandled = true;

        base.OnException(context);
    }
}