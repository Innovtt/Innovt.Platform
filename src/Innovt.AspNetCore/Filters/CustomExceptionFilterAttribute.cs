// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using Innovt.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innovt.AspNetCore.Filters;

/// <summary>
///     An exception filter that handles exceptions of type <see cref="BusinessException" />.
/// </summary>
public sealed class CustomExceptionFilterAttribute : ExceptionFilterAttribute
{
    /// <summary>
    ///     Overrides the <see cref="ExceptionFilterAttribute.OnException" /> method to handle <see cref="BusinessException" />
    ///     .
    /// </summary>
    /// <param name="context">The exception context.</param>
    public override void OnException(ExceptionContext context)
    {
        if (context is null || context.ExceptionHandled) return;

        if (context.Exception is not BusinessException) return;

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