// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using Innovt.AspNetCore.Model;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.AspNetCore.Infrastructure;

/// <summary>
///     Helper class for creating a custom bad request result with validation error details.
/// </summary>
public static class InvalidModelStateResponse
{
    /// <summary>
    ///     Create a custom bad request result
    /// </summary>
    /// <returns>BadRequestObjectResult object</returns>
    public static BadRequestObjectResult CreateCustomErrorResponse(ActionContext actionContext)
    {
        if (actionContext == null) throw new ArgumentNullException(nameof(actionContext));

        var result = new ResponseError
        {
            TraceId = actionContext.HttpContext.TraceIdentifier,
            Message = "One or more validation errors occurred.",
            Detail = actionContext.ModelState.Where(modelError => modelError.Value.Errors.Count > 0)
                .Select(modelError => new
                {
                    Property = modelError.Key,
                    Errors = modelError.Value.Errors.Select(e => e.ErrorMessage).ToList()
                }).ToList()
        };


        return new BadRequestObjectResult(result);
    }
}