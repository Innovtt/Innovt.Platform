using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innovt.AspNetCore.Controllers;
using Innovt.AspNetCore.Filters;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace IocSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private readonly ITracer tracer;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(ILogger logger, ITracer tracer) : base(logger,tracer)
        {
            this.tracer = tracer;
        }

        private void AAA()
        {
            throw new BusinessException("HUahuahuaa");

        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            try
            {
                AAA();
            }
            catch (Exception e)
            {
              throw  new Exception("zicou");
            }
          

            var a = tracer;

            using IScope childScope = tracer.BuildSpan("Child").StartActive(finishSpanOnDispose: true);
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();

            //Tracer.BuildSpan("s").Start()
           
        }
    }
}
