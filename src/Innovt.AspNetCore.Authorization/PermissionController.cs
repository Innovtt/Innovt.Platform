using System;
using Innovt.Authorization.Platform.Application;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.AspNetCore.Authorization
{
    [ApiController]
    [Area("Authorization")]
    [Route("[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IAuthorizationAppService authorizationAppService;

        public PermissionController(IAuthorizationAppService authorizationAppService)
        {
            this.authorizationAppService = authorizationAppService ?? throw new ArgumentNullException(nameof(authorizationAppService));
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok();

        }

    }
}
