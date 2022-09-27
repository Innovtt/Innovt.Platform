// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using Innovt.Core.CrossCutting.Log;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.AspNetCore.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected BaseApiController(ILogger logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected ILogger Logger { get; }
}