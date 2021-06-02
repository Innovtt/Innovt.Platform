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
using Innovt.AspNetCoreTests.Model;
using Innovt.Core.Serialization;
using Innovt.Domain.Core.Events;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using OpenTelemetry.Trace;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Innovt.AspNetCoreTests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //using var tracerProvider = Sdk.CreateTracerProviderBuilder().Build();
            //using (Sdk.CreateTracerProviderBuilder().AddXRayTraceId().Build())

            ////initialize on your IOC
            //DeserializerFactory.Instance.AddMapping<AnticipationRequestCreated>();
            //DeserializerFactory.Instance.AddMapping<AnticipationRequestClosedOrCanceled>();
            //DeserializerFactory.Instance.AddMapping<AnticipationRequestChangePaused>();
            //DeserializerFactory.Instance.AddMapping<InvoiceSyncFinished>();

            var content = "";

            var dEvent = DeserializerFactory.Instance.Deserialize<DomainEvent>("", content);


            var jsonType = "A";
            var jsonPayload = "A";

            object res;


            switch (jsonType)
            {
                case "A":
                    res = JsonSerializer.Deserialize<A>(jsonPayload);
                    break;

                case "B":
                    res = JsonSerializer.Deserialize<B>(jsonPayload);
                    break;
            }


            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            var activity = new Activity("Get");

            //activity.SetParentId("");

            using (Sdk.CreateTracerProviderBuilder().Build())
            {
                activity.Start();


                //using (var activitySource = new ActivitySource("TestTraceIdBasedSamplerOn"))
                //{
                //    var a = activitySource.StartActivity("Get", ActivityKind.Producer);

                //    a.AddTag("Name", "MIchel");
                //    a.AddTag("FirstName", "MIchel");


                //    using (var activity = activitySource.StartActivity("RootActivity", ActivityKind.Internal))
                //    {
                //        //Assert.True(activity.ActivityTraceFlags == ActivityTraceFlags.Recorded);
                //    }
                //}


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
}