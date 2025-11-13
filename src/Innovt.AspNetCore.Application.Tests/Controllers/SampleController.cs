using System.Net;
using Innovt.AspNetCore.Application.Tests.ViewModels;
using Innovt.AspNetCore.Controllers;
using Innovt.AspNetCore.Filters;
using Innovt.Core.Attributes;
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
    [HttpPost("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ModelExcludeFilter("parameterTobeRemoved")]
    [ServiceToServiceAuthorize("Teste")]
    public IActionResult Add([FromRoute] Guid id, Guid parameterTobeRemoved, [FromBody] AddUserViewModel command,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }


    //Sample for ModelExcludeFilter
    [HttpPost("Add2{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ServiceToServiceAuthorize("Teste")]
    public IActionResult Add2([FromRoute] Guid id, Guid parameterTobeRemoved, [FromBody] AddUserViewModel command,
        CancellationToken cancellationToken = default)
    {
        return Ok();
    }
}