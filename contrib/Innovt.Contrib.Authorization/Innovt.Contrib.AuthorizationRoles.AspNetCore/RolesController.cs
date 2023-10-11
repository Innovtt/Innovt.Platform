// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore

using System.Net;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Dtos;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore;
/// <summary>
/// API controller for managing roles.
/// </summary>
[ApiController]
[Route("Authorization/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IAuthorizationAppService authorizationAppService;
    /// <summary>
    /// Initializes a new instance of the <see cref="RolesController"/> class.
    /// </summary>
    /// <param name="authorizationAppService">The authorization application service.</param>
    public RolesController(IAuthorizationAppService authorizationAppService)
    {
        this.authorizationAppService = authorizationAppService ??
                                       throw new ArgumentNullException(nameof(authorizationAppService));
    }
    /// <summary>
    /// Gets a list of roles based on the provided filter.
    /// </summary>
    /// <param name="filter">The filter for role retrieval.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of roles based on the filter.</returns>
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