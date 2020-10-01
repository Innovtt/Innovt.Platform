using System;
using System.Net;
using System.Threading.Tasks;
using Innovt.AspNetCore.Model;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace Innovt.AspNetCore.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IStringLocalizer stringLocalizer;
       
        public ApiExceptionFilterAttribute(IStringLocalizer localizer)
        {
            this.stringLocalizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public ApiExceptionFilterAttribute()
        {
            
        }


        private string Translate(string message)
        {
            if (stringLocalizer == null)
                return message;

            return stringLocalizer[message];
        }

        public override void OnException(ExceptionContext context)
        {
            //logger?.Error(context.Exception);

            var result = new ResponseError
            {
                Message = Translate(context.Exception.Message),
                TraceId = context.HttpContext.TraceIdentifier,
                Detail = context.Exception.StackTrace
            };

            if (context.Exception is BusinessException bex)
            {
                result.Code = $"{StatusCodes.Status400BadRequest}";
                result.Detail = bex.Errors;
                context.Result = new BadRequestObjectResult(result);
            }
            else
            {
                result.Code = $"{StatusCodes.Status500InternalServerError}";
                context.Result = new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
            }

            base.OnException(context);
        }
    }
}