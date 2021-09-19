// Company: Antecipa
// Project: Innovt.Contrib.Authorization.AspNetCore
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

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
    [Route("Authorization/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IAuthorizationAppService authorizationAppService;

        public PermissionController(IAuthorizationAppService authorizationAppService)
        {
            this.authorizationAppService = authorizationAppService ??
                                           throw new ArgumentNullException(nameof(authorizationAppService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<PermissionDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(PermissionFilter filter, CancellationToken cancellationToken = default)
        {
            var groups = await authorizationAppService.FindPermissionBy(filter, cancellationToken);

            return Ok(groups);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(AddPermissionCommand command,
            CancellationToken cancellationToken = default)
        {
            var permissionId = await authorizationAppService.AddPermission(command, cancellationToken);

            return Ok(permissionId);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Remove(RemovePermissionCommand command,
            CancellationToken cancellationToken = default)
        {
            await authorizationAppService.RemovePermission(command, cancellationToken);

            return Ok();
        }
    }
}