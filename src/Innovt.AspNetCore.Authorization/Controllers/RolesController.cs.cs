using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Innovt.AspNetCore.Controllers;
using ILogger = Innovt.Core.CrossCutting.Log.ILogger;

namespace Innovt.AspNetCore.Authorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : BaseApiController
    {
        public RolesController(ILogger logger) : base(logger)
        {
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
            })
            .ToArray();
        }

      
    }
}
