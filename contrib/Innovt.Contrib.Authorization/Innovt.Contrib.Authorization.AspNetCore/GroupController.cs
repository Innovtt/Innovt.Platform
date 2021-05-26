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
    [Area("Authorization")]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IAuthorizationAppService authorizationAppService;

        public GroupController(IAuthorizationAppService authorizationAppService)
        {
            this.authorizationAppService = authorizationAppService ?? throw new ArgumentNullException(nameof(authorizationAppService));
        }


        [HttpGet]
        [ProducesResponseType(typeof(IList<GroupDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get(GroupFilter filter, CancellationToken cancellationToken = default)
        {
            var groups = await authorizationAppService.FindGroupBy(filter, cancellationToken);
            
            return Ok(groups);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(AddGroupCommand command, CancellationToken cancellationToken =default)
        {
            var groupId = await authorizationAppService.AddGroup(command, cancellationToken);
            
            return Ok(groupId);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Remove(RemoveGroupCommand command, CancellationToken cancellationToken = default)
        {
            await authorizationAppService.RemoveGroup(command, cancellationToken);

            return Ok();
        }
    }
}
