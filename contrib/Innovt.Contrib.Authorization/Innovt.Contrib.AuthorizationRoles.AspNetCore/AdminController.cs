// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore

using System.Net;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore;

[ApiController]
[Route("Authorization/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAuthorizationAppService authorizationAppService;

    public AdminController(IAuthorizationAppService authorizationAppService)
    {
        this.authorizationAppService = authorizationAppService ??
                                       throw new ArgumentNullException(nameof(authorizationAppService));
    }

    [HttpPost("RegisterAdmin")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminCommand command,
        CancellationToken cancellationToken = default)
    {
        await authorizationAppService.RegisterAdmin(command, cancellationToken).ConfigureAwait(false);

        return Ok();
    }
}