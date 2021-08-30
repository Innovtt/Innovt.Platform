using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.AspNetCore.ViewModels;
using Innovt.Contrib.Authorization.Platform.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Innovt.Contrib.Authorization.AspNetCore
{
    [ApiController]
    [Route("Authorization/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAuthorizationAppService authorizationAppService;
        private readonly IActionDescriptorCollectionProvider actionDescriptorProvider;

        public AdminController(IAuthorizationAppService authorizationAppService, IActionDescriptorCollectionProvider actionDescriptorProvider)
        {   
            this.authorizationAppService = authorizationAppService ?? throw new ArgumentNullException(nameof(authorizationAppService));
            this.actionDescriptorProvider = actionDescriptorProvider ?? throw new ArgumentNullException(nameof(actionDescriptorProvider));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Init(InitViewModel command, CancellationToken cancellationToken =default)
        {
            await authorizationAppService.Init(command.ToCommand(actionDescriptorProvider), cancellationToken);
            
            return Ok();
        }
    }
}
