// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.CrossCutting.Log;
using Microsoft.AspNetCore.Mvc;

namespace Innovt.AspNetCore.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected BaseApiController(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected ILogger Logger { get; }
    }
}