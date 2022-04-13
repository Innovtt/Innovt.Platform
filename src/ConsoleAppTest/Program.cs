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
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OpenTracing;
using OpenTracing.Mock;
using OpenTracing.Noop;
using OpenTracing.Util;

namespace ConsoleAppTest
{
    public class SupplierConsultancyRegisterFilter : IFilter {

        public string ConsultancyId { get; set; }
        public string SupplierId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
    public class IocTestModule : Innovt.Core.CrossCutting.Ioc.IOCModule
    {
        public IocTestModule(IConfiguration configuration)
        {
            var collection = this.GetServices();

            collection.AddSingleton(configuration);

            collection.AddScoped<IAwsConfiguration, DefaultAWSConfiguration>();

            ITracer tracer = Datadog.Trace.OpenTracing.OpenTracingTracerFactory.CreateTracer();

            GlobalTracer.Register(tracer);

            collection.AddScoped<ITracer>(t => tracer);

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
    
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var env = "dev";
            var confbuild = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                .AddJsonFile($"appsettings.json");

            var build = confbuild.Build();

            var container = new Container();

            container.AddModule(new IocTestModule(build));


            //var repo = new UserRepository();





            //var logger = container.Resolve<ILogger>();


            //container.CheckConfiguration();
            //var tracer = GlobalTracer.Instance;

            //var logger = container.Resolve<ILogger>();

            //using (var scope = tracer.BuildSpan("teste").StartActive()) 
            //{
            //    logger.Info("my name is {@name}", "michel");

            //    logger.Error("borges");

            //}

            try
            {
                var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromMinutes(10), BaseAddress =  new Uri("https://antecipaapirestp14qhi5h8t.br1.hana.ondemand.com/"), 
                };


            var byteArray = Encoding.ASCII.GetBytes("antecipa:@!ntEcIpaRece$biveis");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
          
            var sw = new Stopwatch();

                sw.Start();

                var res = await client.GetAsync(
                    "sap-integration/api/v2/buyer/find-account-documents?buyerErpId=2000&startDate=11/01/2018&endDate=11/01/2018&compensationTree=true&destinationId=grupo_gol_qas");

                res.EnsureSuccessStatusCode();

                var response = await res.Content.ReadAsStringAsync();
                
                //var response = await client.GetStringAsync(
                //    "sap-integration/api/v2/buyer/find-account-documents?buyerErpId=2000&startDate=11/01/2018&endDate=11/01/2018&compensationTree=true&destinationId=grupo_gol_qas");

                Console.WriteLine(response);
                Console.WriteLine(sw.Elapsed.TotalMinutes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            //DAta mnodel 
            // Entity 
            // ("50f67209-7732-47b8-a0d3-c3e6c7730992", "3fea4ece-c746-440a-b6cc-2e229f85837e");

            //var where = new StringBuilder("Deleted = :deleted AND Enabled = :enabled");

            ////where.Append(" AND (contains(FirstName, :term) OR contains(LastName, :term) OR contains(Email, :term))");
            //var filter = new Innovt.Cloud.Table.QueryRequest()
            //{
            //    KeyConditionExpression = "ConsultancyId = :consultancyid AND SupplierId = :supplierid",
            //    Filter = new SupplierConsultancyRegisterFilter() { ConsultancyId = "50f67209-7732-47b8-a0d3-c3e6c7730992", SupplierId = "3fea4ece-c746-440a-b6cc-2e229f85837e" }
            //};

            ////var request = new ScanRequest()
            ////{
            ////    FilterExpression = where.ToString(),
            ////    Filter = filter,
            ////    PageSize = filter.PageSize,
            ////    Page = filter.PaginationToken
            ////}, cancellationToken);

            //var res = await dynamoService.QueryFirstOrDefaultAsync<SupplierConsultancyRegister>(filter);

            // Console.WriteLine(res);


            //// BuyerByDocumentFilter
            //var scanRequest = new ScanRequest()
            //{
            //    FilterExpression = "Configuration.Enabled = :enabled",
            //    Filter = new BuyerByDocumentFilter() { Enabled = 0 }
            //};  


            //var buyer = await dynamoService.ScanAsync<DynamoTable>(scanRequest);

            // var scanRequest = new QueryRequest()
            // {
            //     KeyConditionExpression = "BuyerId = :document",
            //     Filter = new BuyerByDocumentFilter() { Document = "DDCC8E14-7421-46A4-9C71-15B203E5DE07" },
            //     PageSize = 2
            // };

            // var buyer = await dynamoService.ScanAsync<DynamoTable>(scanRequest);

            //var buyer = await dynamoService.QueryPaginatedByAsync<DynamoTable>(scanRequest);

            //Console.WriteLine(buyer);

            //  var buyer = await dynamoService.ScanPaginatedByAsync<DynamoTable>(scanRequest);

            //var scanRequest = new ScanRequest()
            //{
            //    FilterExpression = "Document = :document",
            //    Filter = new BuyerByDocumentFilter() { Document = "59770464000120" },
            //    PageSize = 2
            //};

            //var buyer = await dynamoService.ScanPaginatedByAsync<DynamoTable>(scanRequest);

            // return buyer.Items?.FirstOrDefault();            


            //var result1 = await dynamoService.GetByIdAsync<DynamoTable>("af51abef-91bf-4642-94b7-4349288f62cb");

            //Console.WriteLine(result1);


            //var queryRequest = new QueryRequest()
            //{
            //    Filter = new Filter() { Id = "af51abef-91bf-4642-94b7-4349288f62cb" },
            //    PageSize = 2,
            //    KeyConditionExpression = "Id = :id"
            //};

            //var result = await dynamoService.QueryPaginatedByAsync<DynamoTable>(queryRequest);


            //Console.WriteLine(result1);



            //var scanRequest = new ScanRequest()
            //{
            //    Filter = new Filter() { Id = "af51abef-91bf-4642-94b7-4349288f62cb" },
            //    PageSize = 2,
            //   // FilterExpression = " begins_with (Subject, :subject)"
            //};

            //var result = await dynamoService.ScanPaginatedByAsync<DynamoTable>(queryRequest);

            //queryRequest.Page = result.Page;

            //var result = await dynamoService.QueryAsync<DynamoTable>("SendBuyerLotNotAntecipatedScheduler");


            //   var result = await dynamoService.GetByIdAsync<DynamoTable>("SendBuyerLotNotAntecipatedScheduler");
            // var result = await dynamoService.GetAll();


            //var result = await dynamoService.GetByIdAsync<DynamoTable>("SendBuyerLotNotAntecipatedScheduler");

            //Console.WriteLine(result);


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
