using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innovt.AspNetCore.Controllers;
using Innovt.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace IocSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(Innovt.Core.CrossCutting.Log.ILogger logger,ITracer tracer) : base(logger, tracer)
        {

           
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            //throw new BusinessException("erorrr");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
