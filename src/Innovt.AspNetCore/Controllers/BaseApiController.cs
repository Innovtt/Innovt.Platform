// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using Innovt.Core.CrossCutting.Log;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.AspNetCore.Controllers;
/// <summary>
/// Abstract base class for API controllers.
/// </summary>
[ApiController]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseApiController"/> class with the specified logger.
    /// </summary>
    /// <param name="logger">The logger.</param>
    protected BaseApiController(ILogger logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    /// <summary>
    /// Gets the logger.
    /// </summary>
    protected ILogger Logger { get; }
}