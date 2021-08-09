using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Contrib.Authorization.Platform.Application.Dtos;
using Innovt.Contrib.Authorization.Platform.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.Contrib.Authorization.AspNetCore
{
    [ApiController]    
    [Route("Authorization/[controller]")]
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
            if (filter is null)throw new ArgumentNullException(nameof(filter));            

            var groups = await authorizationAppService.FindGroupBy(filter, cancellationToken);

            return Ok(groups);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(AddGroupCommand command, CancellationToken cancellationToken =default)
{
            if (command is null) throw new ArgumentNullException(nameof(command));

            var groupId = await authorizationAppService.AddGroup(command, cancellationToken);
            
            return Ok(groupId);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Remove(RemoveGroupCommand command, CancellationToken cancellationToken = default)
        {
            if (command is null) throw new ArgumentNullException(nameof(command));

            await authorizationAppService.RemoveGroup(command, cancellationToken);

            return Ok();
        }
    }
}
