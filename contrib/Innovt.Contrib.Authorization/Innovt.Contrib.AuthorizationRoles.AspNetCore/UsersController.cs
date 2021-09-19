// Company: Antecipa
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-17

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore
{
    [ApiController]
    [Route("Authorization/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthorizationAppService authorizationAppService;

        public UsersController(IAuthorizationAppService authorizationAppService)
        {
            this.authorizationAppService = authorizationAppService ??
                                           throw new ArgumentNullException(nameof(authorizationAppService));
        }

        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(AddUserCommand command, CancellationToken cancellationToken = default)
        {
            await authorizationAppService.AddUser(command, cancellationToken);

            return Ok();
        }

        [HttpDelete()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(RemoveUserCommand command,
            CancellationToken cancellationToken = default)
        {
            await authorizationAppService.RemoveUser(command, cancellationToken);

            return Ok();
        }


        [HttpPut()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AssignRole(AssignRoleCommand command, CancellationToken cancellationToken = default)
        {
            await authorizationAppService.AssignRole(command, cancellationToken);

            return Ok();
        }

        [HttpPut()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UnAssignRole(UnAssignUserRoleCommand command, CancellationToken cancellationToken = default)
        {
            await authorizationAppService.UnAssignRole(command, cancellationToken);

            return Ok();
        }
    }
}