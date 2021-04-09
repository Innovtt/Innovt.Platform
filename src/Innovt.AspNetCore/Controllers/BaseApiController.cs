using Innovt.Core.CrossCutting.Log;
using Microsoft.AspNetCore.Mvc;
using System;


namespace Innovt.AspNetCore.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected ILogger Logger { get; }

        protected BaseApiController(ILogger logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}