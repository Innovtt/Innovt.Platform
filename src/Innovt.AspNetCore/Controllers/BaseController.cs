using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace Innovt.AspNetCore.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger logger;

        protected BaseController(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected IActionResult RedirectToLocal(string redirectUrl, RedirectToActionResult redirect = null,string defaultAction = "Index", string defaultController = "Home")
        { 
            if (Url.IsLocalUrl(redirectUrl))
                return Redirect(redirectUrl);

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
                return new JsonResult(new {ex.Message});
            }
            catch (Exception ex)
            {
                logger.Error(ex, errorMessage);
                throw;
            }
        }

        protected JsonResult JsonOk(object data)
        {
            return new JsonResult(data)
            {
                StatusCode = 200
            };
        }

      
    }
}