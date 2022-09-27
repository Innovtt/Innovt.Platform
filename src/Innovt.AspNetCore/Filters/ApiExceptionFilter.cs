// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.Diagnostics;
using Innovt.AspNetCore.Model;
using Innovt.AspNetCore.Resources;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace Innovt.AspNetCore.Filters;

[AttributeUsage(AttributeTargets.All)]
public sealed class ApiExceptionFilter : ExceptionFilterAttribute
{
    public ApiExceptionFilter(ILogger logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ApiExceptionFilter(ILogger logger, IStringLocalizer<IExceptionResource> stringLocalizer) : this(logger)
    {
        StringLocalizer = stringLocalizer ?? throw new ArgumentNullException(nameof(stringLocalizer));
    }

    public ILogger Logger { get; }

    public IStringLocalizer<IExceptionResource> StringLocalizer { get; }

    private string Translate(string message)
    {
        return StringLocalizer?[message] ?? message;
    }

    public override void OnException(ExceptionContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var baseException = context.Exception;

        var requestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

        var result = new ResponseError
        {
            Message = "Internal server error.Please try again or contact your system administrator.",
            TraceId = requestId
        };

        if (baseException is BusinessException bex)
        {
            result.Message = Translate(bex.Message);
            result.Code = bex.Code.IsNullOrEmpty() ? $"{StatusCodes.Status400BadRequest}" : bex.Code;
            result.Detail = bex.Errors;
            context.Result = new BadRequestObjectResult(result);
        }
        else
        {
            result.Code = $"{StatusCodes.Status500InternalServerError}";
            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            Logger.Error(context.Exception, "InternalServerError");
        }

        Activity.Current?.SetStatus(ActivityStatusCode.Error, baseException.Message);
    }
}