// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Util;
using Datadog.Trace.OpenTracing;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.S3;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.IOC;
using Innovt.CrossCutting.Log.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing.Util;

namespace ConsoleAppTest;

public class IocTestModule : IOCModule
{
    public IocTestModule(IConfiguration configuration)
    {
        var collection = GetServices();

        collection.AddSingleton(configuration);

        collection.AddScoped<IAwsConfiguration>(a => new DefaultAWSConfiguration());

        //collection.AddScoped<IAwsConfiguration, DefaultAWSConfiguration>();

        var tracer = OpenTracingTracerFactory.CreateTracer();

        GlobalTracer.Register(tracer);

        collection.AddScoped(t => tracer);

        //collection.AddScoped<ILogger, Logger>();
        collection.AddSingleton<ILogger>(new Logger(new DataDogEnrich()));
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
    private static ActivitySource source = new("ConsoleAppTest");

    private static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
            .Build();

        
        
        //var container = new Container();

        //container.AddModule(new IocTestModule(configuration));

        try
        {
            Console.Write("Listing instanceProfile");

            var profile = new InstanceProfileAWSCredentials();



            Console.Write(profile.Role);
            var cred = await profile.GetCredentialsAsync();

            Console.WriteLine(cred.AccessKey);
            Console.WriteLine(cred.SecretKey);







            //var service = new S3FileSystem(container.Resolve<ILogger>(), container.Resolve<IAwsConfiguration>());

            //var content = await service.PutObjectAsync("antecipa-files-uat", "E:\\filekeycdc.txt", null, "SSES");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        
        Console.ReadKey();
    }
}