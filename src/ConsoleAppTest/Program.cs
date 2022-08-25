using Datadog.Trace.OpenTracing;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.IOC;
using Innovt.CrossCutting.Log.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppTest;

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
      /*  collection.AddScoped<RedisProviderConfiguration>(p => new RedisProviderConfiguration()
        {
            ReadWriteHosts = new[] { "localhost:6379" },
            ReadOnlyHosts = new[] { "localhost:6379" }
            //ReadWriteHosts = new[] { "app-cluster.lxgfsw.ng.0001.use1.cache.amazonaws.com:6379" },
            //ReadOnlyHosts = new[] { "app-cluster-ro.lxgfsw.ng.0001.use1.cache.amazonaws.com:6379" }
        });

        collection.AddScoped<ICacheService, RedisCacheService>();*/
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
    private static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
            .Build();

        var container = new Container();

        container.AddModule(new IocTestModule(configuration));
        
//        var invoiceRepo = new InvoiceRepository(container.Resolve<ILogger>(), container.Resolve<IAwsConfiguration>());
        var userRepo = new UserRepository(container.Resolve<ILogger>(), container.Resolve<IAwsConfiguration>());


        var res =  await userRepo.GetCapitalSources(CancellationToken.None);

        //var result = await userRepo.GetAuthProvider();

        //await invoiceRepo.IncrementOrderSequence(CancellationToken.None);

        /*

        var cacheService = container.Resolve<ICacheService>();

        var key = "user123";

        cacheService.SetValue(key, new User() { Name = "Michel" }, TimeSpan.FromHours(1));

        var value = cacheService.GetValue<User>(key);


        Console.WriteLine(value);*/
        //var taskList = new List<Task>();

        //for (int i = 0; i < 500; i++)
        //{
        //    try
        //    {
        //        var invoiceRepo = new InvoiceRepository(container.Resolve<ILogger>(), container.Resolve<IAwsConfiguration>());

        //        taskList.Add(invoiceRepo.IncrementOrderSequence(CancellationToken.None));
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}

        //Task.WaitAll(taskList.ToArray());

        //Console.ReadKey();


        //var result = await invoiceRepo.GetBatchUsers();

        //var companyId = Guid.Parse("4e680340-98bc-4f57-930e-48ad2904cdb5");

        ////var users = await invoiceRepo.QueryAsync<InvoicesAggregationCompanyDataModel>(new QueryRequest()
        ////{
        ////    KeyConditionExpression = "PK=:pk",
        ////    Filter = new { pk = "E#ed791722c6f5b3733a06238fba0c2577" }
        ////});

        //var list = new List<InvoicesAggregationCompanyDataModel>();

        //for (int i = 0; i < 10; i++)
        //{

        //    list.Add(new InvoicesAggregationCompanyDataModel()
        //    {
        //        CompanyId = companyId.ToString(),
        //        Currency = "R$",
        //        PK = $"M#{companyId}",
        //        SK1 = $"SampleMichel#{i}#{DateTime.Now}",
        //        TotalValue = 10
        //    });
        //}



        //var invoices = await invoiceRepo.QueryAsync<InvoicesAggregationCompanyDataModel>(new QueryRequest()
        //{
        //    KeyConditionExpression = "PK=:pk",
        //    Filter = new { pk = $"M#{companyId}" }
        //});


        //foreach (var model in invoices)
        //{
        //    list.Add(new InvoicesAggregationCompanyDataModel()
        //    {
        //        CompanyId = companyId.ToString(),
        //        Currency = "R$",
        //        PK = $"M#{companyId}",
        //        SK1 = model.SK1,
        //        TotalValue = 10,
        //        Quantity = 1
        //    });
        //}
        ////for (int i = 0; i < 25; i++)
        ////{

        ////    list.add(new invoicesaggregationcompanydatamodel()
        ////    {
        ////        companyid = companyid.tostring(),
        ////        currency = "r$",
        ////        pk = $"m#{companyid}",
        ////        sk1 = $"samplemichel#{i}#{datetime.now}",
        ////        totalvalue = 10
        ////    });
        ////}
        ////await invoiceRepo.BatchInsert(list);

        ////var taskList = new List<Task>();

        //await invoiceRepo.SaveAll(list);


        //for (int i = 0; i < 10; i++)
        //{   
        //    taskList.Add(invoiceRepo.BatchInsert(list));
        //}

        //try
        //{
        //    Task.WaitAll(taskList.ToArray());
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e);
        //    throw;
        //}
        Console.WriteLine("Aqu");
        //Console.WriteLine("Hello World!");

        //using var tracerProvider = Sdk.CreateTracerProviderBuilder()
        //    //.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MySample"))
        //    .AddSource("ConsoleAppTest")
        //    .AddConsoleExporter()
        //    .AddJaegerExporter(e =>
        //   {
        //       e.AgentPort = 6831;
        //       e.AgentHost = "localhost";
        //   })
        //    .Build();

        //DoSomething();

        //tracerProvider.ForceFlush();
    }

    private static void DoSomething2()
    {
        throw new Exception("Dosomething erros ");

    }

    private static void DoSomething()
    {
        using (Activity activity = source.StartActivity("SomeWork"))
        {
            activity?.SetTag("foo", "foo");
            activity?.SetTag("bar", "bar");

            var logger = new Logger(new DataDogEnrich());

            logger.Info("Teste INfo");
            try
            {
                DoSomething2();
            }
            catch (Exception e)
            {
                logger.Error(e, "Deu merda");
                throw;
            }

        }
    }

}