﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore

using System.Net;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Dtos;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore;

[ApiController]
[Route("Authorization/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IAuthorizationAppService authorizationAppService;

    public RolesController(IAuthorizationAppService authorizationAppService)
    {
        this.authorizationAppService = authorizationAppService ??
                                       throw new ArgumentNullException(nameof(authorizationAppService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<RoleDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Get([FromQuery] RoleByUserFilter filter,
        CancellationToken cancellationToken = default)
    {
        var roles = await authorizationAppService.GetUserRoles(filter, cancellationToken).ConfigureAwait(false);

        return Ok(roles);
    }
}