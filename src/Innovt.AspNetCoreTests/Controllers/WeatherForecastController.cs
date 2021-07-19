// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCoreTests
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Innovt.AspNetCoreTests.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Serialization;
using Innovt.CrossCutting.Log.Serilog;
using Innovt.Domain.Core.Events;
using Innovt.Domain.Core.Streams;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using OpenTelemetry.Trace;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Innovt.AspNetCoreTests.Controllers
{
    public class InvoiceAdded:IDataStream
    {
        public string Name { get; set; }
        public string EventId { get; set; }
        public string Version { get; set; }
        public string Partition { get; set; }
        public string TraceId { get; set; }
        public DateTime ApproximateArrivalTimestamp { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ILogger logger;

        public WeatherForecastController(ILogger logger)
        {
            this.logger = logger;
        }


        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            logger.Info("My name is {name}", "michel");
            
            logger.Info("My name is {name} {lastname}", "michel", "borges");


            var awsConfig = new DefaultAWSConfiguration();

            var producer = new Innovt.Cloud.AWS.Kinesis.EventHandler("Sample", new Logger(),awsConfig);

            await producer.Publish(new List<DomainEvent>());

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