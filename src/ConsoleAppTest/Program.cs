using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Table;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.IOC;
using Innovt.CrossCutting.Log.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class Filter : IFilter {

        public string Id { get; set; }
        public string Subject { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
    public class IocTestModule: Innovt.Core.CrossCutting.Ioc.IOCModule
    {
        public IocTestModule(IConfiguration configuration)
        {
            var collection = this.GetServices();

            collection.AddSingleton(configuration);

            collection.AddScoped<IAWSConfiguration, DefaultAWSConfiguration>();

            collection.AddScoped<ILogger, Logger>();

            collection.AddScoped<DynamoService>();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var env = "dev";
            var confbuild = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                .AddJsonFile($"appsettings.json");

            var build  = confbuild.Build();
            
            var container = new Container();

            container.AddModule(new IocTestModule(build));

            container.CheckConfiguration();

            var dynamoService = container.Resolve<DynamoService>();


            var queryRequest = new QueryRequest()
            {
                Filter = new Filter() { Id= "SendBuyerLotNotAntecipatedScheduler", Subject = "{{"},
                PageSize = 10,
                Page = "1",
               // AttributesToGet = 
                KeyConditionExpression = "Id = :id", //10000 Chinese Wall + Nombanco23456789
                FilterExpression = " begins_with (Subject, :subject)"
            };

            var result = await dynamoService.QueryPaginatedByAsync<DynamoTable>(queryRequest);

            queryRequest.Page = result.Page;

            //var result = await dynamoService.QueryAsync<DynamoTable>("SendBuyerLotNotAntecipatedScheduler");


            //   var result = await dynamoService.GetByIdAsync<DynamoTable>("SendBuyerLotNotAntecipatedScheduler");
            // var result = await dynamoService.GetAll();


            //var result = await dynamoService.GetByIdAsync<DynamoTable>("SendBuyerLotNotAntecipatedScheduler");

            Console.WriteLine(result);


           // var item = await sqs.GetByIdAsync<NotificationTemplate2>("TemplateWithParameters","01");

            // var item = await sqs.QueryAsync<TableRepo>("TemplateWithParameters","01");

            //Console.WriteLine(item);

            // var configuration = container.Resolve<IAWSConfiguration>();

            //var credential = configuration.GetCredential();

            
            //var sqs = container.Resolve<SqsService>();




            //var repo = new UserRepository(new Innovt.CrossCutting.Log.Serilog.Logger());

            //var id = "123456";

            //var res = await repo.GetByIdAsync<User>(id);

        }
    }
}
