using System.Net;
using Innovt.AspNetCore.Application.Tests.ViewModels;
using Innovt.AspNetCore.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ILogger = Innovt.Core.CrossCutting.Log.ILogger;

namespace Innovt.AspNetCore.Application.Tests.Controllers;

[EnableCors]
[Produces("application/json")]
[Route("[controller]")]
[ApiController]
public class SampleController : BaseApiController
{
    public SampleController(ILogger logger) : base(logger)
    {
    }

    //Sample for ModelExcludeFilter
    [HttpPost("{userId:guid}")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    //[ModelExcludeFilter(excludeAttributes: new[]{ "ExternalId", "UserId"})]
    public IActionResult Add([FromRoute] Guid userId, [FromBody] AddUserViewModel command,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }
}