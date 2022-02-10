// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.AspNetCore.Model;
using Innovt.AspNetCore.Resources;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace Innovt.AspNetCore.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public ILogger Logger { get; private set; }
        public ApiExceptionFilter(ILogger logger, IStringLocalizer<IExceptionResource> stringLocalizer)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            StringLocalizer = stringLocalizer ?? throw new ArgumentNullException(nameof(stringLocalizer));
        }

        public ApiExceptionFilter(ILogger logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IStringLocalizer<IExceptionResource> StringLocalizer { get; }

        private string Translate(string message)
        {
            return StringLocalizer?[message] ?? message;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var baseException = context.Exception;

            var result = new ResponseError
            {
                Message = Translate(baseException.Message),
                TraceId = context.HttpContext.TraceIdentifier
            };

            if (baseException is BusinessException bex)
            {
                result.Code = $"{StatusCodes.Status400BadRequest}";
                result.Detail = bex.Errors;
                context.Result = new BadRequestObjectResult(result);
            }
            else
            {
                result.Code = $"{StatusCodes.Status500InternalServerError}";
                result.Detail = $"Server Error: {baseException.Message}. Check your backend log for more detail.";
                context.Result = new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
                Logger.Error(context.Exception, "InternalServerError");
            }
        }
    }
}