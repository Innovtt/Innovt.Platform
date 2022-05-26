// Company: Antecipa
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System.Net;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore;

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

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Add([FromBody] AddUserCommand command,
        CancellationToken cancellationToken = default)
    {
        await authorizationAppService.AddUser(command, cancellationToken).ConfigureAwait(false);

        return Ok();
    }

    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Delete([FromBody] RemoveUserCommand command,
        CancellationToken cancellationToken = default)
    {
        await authorizationAppService.RemoveUser(command, cancellationToken).ConfigureAwait(false);

        return Ok();
    }


    [HttpPut("AssignRole")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleCommand command,
        CancellationToken cancellationToken = default)
    {
        await authorizationAppService.AssignRole(command, cancellationToken).ConfigureAwait(false);

        return Ok();
    }

    [HttpPut("UnAssignRole")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UnAssignRole([FromBody] UnAssignUserRoleCommand command,
        CancellationToken cancellationToken = default)
    {
        await authorizationAppService.UnAssignRole(command, cancellationToken).ConfigureAwait(false);

        return Ok();
    }
}