// Company: Antecipa
// Project: Innovt.Contrib.AuthorizationRoles.AspNetCore
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Dtos;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.AuthorizationRoles.AspNetCore
{
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
        [ProducesResponseType(typeof(IList<RoleDto>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery]RoleByUserFilter filter, CancellationToken cancellationToken = default)
        {
            var roles = await authorizationAppService.GetUserRoles(filter, cancellationToken).ConfigureAwait(false);

            return Ok(roles);
        }

    }
}