// TotalAcesso.Admin.Web

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Innovt.AspNetCore.Filters
{
    public class AutoValidateModelStateFilterAttribute:ActionFilterAttribute
    {
        private void ValidateModelState(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ValidateModelState(context);

            base.OnActionExecuting(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ValidateModelState(context);

            return base.OnActionExecutionAsync(context, next);
        }
    }
}