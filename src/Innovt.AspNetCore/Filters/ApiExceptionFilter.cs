using System;
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
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IStringLocalizer<IExceptionResource> stringLocalizer;
        private readonly ILogger logger;

        public ApiExceptionFilter(ILogger logger, IStringLocalizer<IExceptionResource> localizer)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.stringLocalizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public ApiExceptionFilter(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private string Translate(string message)
        {
            return stringLocalizer == null ? message : stringLocalizer[message];
        }

        public override void OnException(ExceptionContext context)
        {
            var baseException = context.Exception.GetBaseException();

            var result = new ResponseError
            {
                Message = Translate(baseException.Message),
                TraceId = context.HttpContext.TraceIdentifier,
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
                context.Result = new ObjectResult(result) {StatusCode = StatusCodes.Status500InternalServerError};
                logger?.Error(context.Exception, "Message: {@Message} TraceId: @{TraceId} ", baseException.Message,
                    result.TraceId);
            }
        }
    }
}