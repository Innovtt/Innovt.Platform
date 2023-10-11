// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore

using System.Net;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore;
/// <summary>
/// API controller for administering authorization operations.
/// </summary>
[ApiController]
[Route("Authorization/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAuthorizationAppService authorizationAppService;
    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </summary>
    /// <param name="authorizationAppService">The authorization application service.</param>
    public AdminController(IAuthorizationAppService authorizationAppService)
    {
        this.authorizationAppService = authorizationAppService ??
                                       throw new ArgumentNullException(nameof(authorizationAppService));
    }
    /// <summary>
    /// Registers an admin user.
    /// </summary>
    /// <param name="command">The command to register an admin user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the registration operation.</returns>
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