// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.AspNetCore.Model;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.AspNetCore.Infrastructure;

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