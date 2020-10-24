using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using System;
using System.Threading.Tasks;

namespace Innovt.AspNetCore.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger Logger;
        protected readonly ITracer Tracer;

        protected BaseController(ILogger logger,ITracer tracer)
        {
            this.Tracer = tracer ?? throw new ArgumentNullException(nameof(tracer));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected IActionResult RedirectToLocal(string returnUrl, RedirectToActionResult redirect = null, string defaultAction = "Index", string defaultController = "Home")
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            if (redirect != null)
                return redirect;

            return RedirectToAction(defaultAction, defaultController);
        }


        protected async Task<JsonResult> JsonExecute(Func<Task<JsonResult>> func, string errorMessage)
        {
            try
            {
               return await func();
            }
            catch (BusinessException ex)
            {
               return new JsonResult(new { ex.Message });
            }
            catch (Exception ex)
            {
                Logger.Error(ex,errorMessage);
                throw ex;
            }
        }

        protected JsonResult JsonOK(object data)
        {
            return new JsonResult(data)
            {
                StatusCode = 200
            };
        }
    }
}