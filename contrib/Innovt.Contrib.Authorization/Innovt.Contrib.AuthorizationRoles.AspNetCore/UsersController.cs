﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore

using System.Net;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore;

/// <summary>
///     Controller for managing users and their roles.
/// </summary>
[ApiController]
[Route("Authorization/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IAuthorizationAppService authorizationAppService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UsersController" /> class.
    /// </summary>
    /// <param name="authorizationAppService">The authorization application service.</param>
    public UsersController(IAuthorizationAppService authorizationAppService)
    {
        this.authorizationAppService = authorizationAppService ??
                                       throw new ArgumentNullException(nameof(authorizationAppService));
    }

    /// <summary>
    ///     Adds a new user.
    /// </summary>
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

    /// <summary>
    ///     Deletes a user.
    /// </summary>
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

    /// <summary>
    ///     Assigns a role to a user.
    /// </summary>
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

    /// <summary>
    ///     Unassigns a role from a user.
    /// </summary>
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