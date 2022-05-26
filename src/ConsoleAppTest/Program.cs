using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using Datadog.Trace.OpenTracing;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.Log.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTracing.Util;

namespace ConsoleAppTest;

public class SupplierConsultancyRegisterFilter : IFilter
{
    public string ConsultancyId { get; set; }
    public string SupplierId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return null;
    }
}

public class IocTestModule : IOCModule
{
    public IocTestModule(IConfiguration configuration)
    {
        var collection = GetServices();

        collection.AddSingleton(configuration);

        collection.AddScoped<IAwsConfiguration, DefaultAWSConfiguration>();

        var tracer = OpenTracingTracerFactory.CreateTracer();

        GlobalTracer.Register(tracer);

        collection.AddScoped(t => tracer);

        collection.AddScoped<ILogger, Logger>();

        collection.AddScoped<DynamoService>();
    }
}

public class BuyerByDocumentFilter : IFilter
{
    public int Enabled { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}

public class Program
{
    private static ActivitySource source = new ActivitySource("ConsoleAppTest");
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
           // .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MySample"))
            .AddSource("ConsoleAppTest")
            .AddConsoleExporter()
            .AddJaegerExporter(e =>
           {
               e.AgentPort = 6831;
               e.AgentHost = "localhost";

           })
            .Build();
        
        DoSomething();

        tracerProvider.ForceFlush();
    }


    private static void DoSomething()
    {
        using (Activity activity = source.StartActivity("SomeWork"))
        {
            activity?.SetTag("foo", "foo");
            activity?.SetTag("bar", "bar");

            var logger = new Logger(new DataDogEnrich());

            logger.Info("Teste INfo");
        }
    }

}