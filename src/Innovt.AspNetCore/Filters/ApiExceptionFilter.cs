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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Innovt.AspNetCore.Filters;
/// <summary>
/// An exception filter attribute for handling exceptions globally and providing standardized error responses.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public sealed class ApiExceptionFilter : ExceptionFilterAttribute
{
    /// <summary>
    /// Default Constructor 
    /// </summary>
    public ApiExceptionFilter() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiExceptionFilter"/> class with a logger.
    /// </summary>
    /// <param name="logger">The logger to use for logging exceptions.</param>
    public ApiExceptionFilter(ILogger logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiExceptionFilter"/> class with a logger and a string localizer for exception messages.
    /// </summary>
    /// <param name="logger">The logger to use for logging exceptions.</param>
    /// <param name="stringLocalizer">The string localizer for localizing exception messages.</param>
    public ApiExceptionFilter(ILogger logger, IStringLocalizer<IExceptionResource> stringLocalizer) : this(logger)
    {
        StringLocalizer = stringLocalizer ?? throw new ArgumentNullException(nameof(stringLocalizer));
    }
    /// <summary>
    /// Gets or sets the logger.
    /// </summary>
    public ILogger? Logger { get; private set; }
    /// <summary>
    /// Gets the string localizer for localizing exception messages.
    /// </summary>
    public IStringLocalizer<IExceptionResource> StringLocalizer { get; } = null!;
    /// <summary>
    /// Translates the given message using the provided string localizer.
    /// If the string localizer is null or the message is not found, the original message is returned.
    /// </summary>
    /// <param name="message">The message to translate.</param>
    /// <returns>The translated message or the original message if not found.</returns>
    private string Translate(string message)
    {
        return StringLocalizer?[message] ?? message;
    }
    /// <summary>
    /// Writes an error log using the provided logger and context.
    /// If the logger is null, the error message is logged to the console.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="message">The error message.</param>
    /// <param name="ex">The exception associated with the error.</param>
    private void WriteLog(HttpContext context, string message, Exception ex)
    {
        Logger ??= context?.RequestServices.GetService<ILogger>();

        if (Logger is null)
        {
            Console.WriteLine("ApiExceptionFilter is not resolving the innovt logger.");
            Console.WriteLine($"Message: {message}, Exception: {ex.Message}");
        }
        else
        {
            Logger.Error(ex, message);
        }
    }
    /// <inheritdoc />
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
            result.Detail = bex.Detail;
            context.Result = new BadRequestObjectResult(result);
        }
        else
        {
            result.Code = $"{StatusCodes.Status500InternalServerError}";
            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            WriteLog(context.HttpContext,"InternalServerError", context.Exception);
        }
        Activity.Current?.SetStatus(ActivityStatusCode.Error, baseException.Message);
    }
}