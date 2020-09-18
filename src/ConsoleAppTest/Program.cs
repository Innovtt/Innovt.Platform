using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.IOC;
using Innovt.CrossCutting.Log.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
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

            var conditions = new List<FilterCondition>();

            //conditions.Add(new FilterCondition("Subject", ComparisonOperator.Contains, "Subject"));
            conditions.Add(new FilterCondition("Subject", ComparisonOperator.Null));

            //var result = await dynamoService.ScanAsync<DynamoTable>(condition);

            var scanRequest = new ScanRequest()
            {
                PageSize = 1,
                ConditionalOperator = ConditionalOperator.And,
               
            };

            scanRequest.AddCondition("Id", ComparisonOperator.Contains, "");
            scanRequest.AddCondition("Id", ComparisonOperator.Contains, "");
            scanRequest.AddCondition("Id", ComparisonOperator.Contains, "");
            scanRequest.AddCondition("Id", ComparisonOperator.Contains, "");
            scanRequest.AddCondition("Id", ComparisonOperator.Contains, "");
            scanRequest.AddCondition("Id", ComparisonOperator.Contains, "");



            scanRequest.AddCondition(conditions[0]);

            var resulta = await dynamoService.GetByIdAsync<DynamoTable>("SendAntecipationRequestClose","01");



            var result = await dynamoService.ScanPaginatedBy<DynamoTable>(scanRequest);

            scanRequest.PaginationToken = result.PaginationToken;


            result = await dynamoService.ScanPaginatedBy<DynamoTable>(scanRequest);


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
