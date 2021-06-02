using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Contrib.Authorization.Platform.Application.Dtos;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.Authorization.AspNetCore
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController:ControllerBase
    {
        private readonly IAuthorizationAppService authorizationAppService;
        
        public RoleController(IAuthorizationAppService authorizationAppService)
        {
            this.authorizationAppService = authorizationAppService ?? throw new ArgumentNullException(nameof(authorizationAppService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<RoleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(RoleFilter filter, CancellationToken cancellationToken = default)
        {
            var roles = await authorizationAppService.FindRoleBy(filter, cancellationToken);

            return Ok(roles);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(AddRoleCommand command, CancellationToken cancellationToken = default)
        {
            var roleId = await authorizationAppService.AddRole(command, cancellationToken);

            return Ok(roleId);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Remove(RemoveRoleCommand command, CancellationToken cancellationToken = default)
        {
            await authorizationAppService.RemoveRole(command, cancellationToken);

            return Ok();
        }

    }
}
