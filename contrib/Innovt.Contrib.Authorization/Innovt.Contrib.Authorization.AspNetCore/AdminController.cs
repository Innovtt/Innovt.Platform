﻿// Company: Antecipa
// Project: Innovt.Contrib.Authorization.AspNetCore
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.Authorization.AspNetCore
{
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
        public async Task<IActionResult> RegisterAdmin(RegisterAdminCommand command,
            CancellationToken cancellationToken = default)
        {
            await authorizationAppService.RegisterAdmin(command, cancellationToken);

            return Ok();
        }
    }
}