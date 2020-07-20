using System;
using System.Net;
using System.Threading.Tasks;
using Innovt.AspNetCore.Model;
using Innovt.Core.Exceptions;
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


        private void CheckExceptionResult(ExceptionContext context)
        {
            if (context.Exception is BusinessException bex)
            {
                var result = new ResponseError
                {
                    Message = Translate(bex.Message),
                    Code = bex.Code,
                    Errors = bex.Errors
                };

                context.Result = new BadRequestObjectResult(result);
            }
        }

        public override void OnException(ExceptionContext context)
        {
            context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            CheckExceptionResult(context);
            
            base.OnException(context);
        }


        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            
            CheckExceptionResult(context);

            await base.OnExceptionAsync(context);
        }
    }
}