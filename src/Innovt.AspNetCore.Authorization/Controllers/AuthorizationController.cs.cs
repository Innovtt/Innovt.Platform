using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Innovt.AspNetCore.Controllers;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.AspNetCore.Authorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : BaseApiController
    {
        public AuthorizationController(ILogger logger) : base(logger)
        {
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return null;
        }

      
    }
}
