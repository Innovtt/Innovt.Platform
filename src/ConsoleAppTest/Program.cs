using Amazon.DynamoDBv2.DataModel;
using ConsoleAppTest.Domain;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.IOC;
using Innovt.CrossCutting.Log.Serilog;
using Innovt.Notification.Core.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
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

            collection.AddScoped<SqsService>();

            //SqsService
        }
    }

    [DynamoDBTable("NotificationsTemplate")]
    public class NotificationTemplate2 : NotificationTemplate,ITableMessage
    {
        public string PartitionKey { get; set; }
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

            var sqs = container.Resolve<SqsService>();

            var item = await sqs.GetByIdAsync<NotificationTemplate2>("TemplateWithParameters","01");

           // var item = await sqs.QueryAsync<TableRepo>("TemplateWithParameters","01");



            Console.WriteLine(item);

            // var configuration = container.Resolve<IAWSConfiguration>();

            //var credential = configuration.GetCredential();

            
            //var sqs = container.Resolve<SqsService>();




            //var repo = new UserRepository(new Innovt.CrossCutting.Log.Serilog.Logger());

            //var id = "123456";

            //var res = await repo.GetByIdAsync<User>(id);

        }
    }
}
