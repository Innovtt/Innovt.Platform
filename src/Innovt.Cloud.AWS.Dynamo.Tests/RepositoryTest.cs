using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.CredentialManagement;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;
using NUnit.Framework;
using QueryRequest = Innovt.Cloud.Table.QueryRequest;

namespace Innovt.Cloud.AWS.Dynamo.Tests
{
    public class Tests
    {
        private ILogger loggerMock = null;
        private BaseRepository baseRepository;
        private IAWSConfiguration configuration;
        
        [SetUp]
        public void Setup()
        {
            loggerMock = NSubstitute.Substitute.For<ILogger>();


            configuration = new DefaultAWSConfiguration("antecipa-dev");
        }

        [Test]
        public async Task Test1()
        {
            try
            {
                //var bucketName = "users-antecipa-dev";

                //var fileSystem = new S3FileSystem(loggerMock, configuration);

                //var file = System.IO.File.OpenRead(@"D:\Pessoal\Curriculum\Michel Borges's_Resume_V2.pdf");


                //var resUrl = await fileSystem.UploadAsync(bucketName, file, "michel/testemichel.pdf", serverSideEncryptionMethod: "AES256");
                
                //var profile = new CredentialProfile("antecipa-dev",new CredentialProfileOptions());

                // var client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);


                // var request = new Amazon.DynamoDBv2.Model.QueryRequest()
                // {
                //     TableName = "Invoices",
                //     IndexName = "BuyerId-InvoiceId-Index",
                //     KeyConditionExpression = "BuyerId = :bid",
                //    // FilterExpression = " PaymentOrderStatusId = :pid OR (PaymentOrderStatusId = :psid AND PaymentTypeId = :pti)",
                //    FilterExpression = " PaymentOrderStatusId = :pid ",
                //     ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                //     {
                //         {":bid", new AttributeValue(){ S = "f2c9e6ba-2735-43e1-82f2-0ddf5c766c42"}},
                //         {":pid", new AttributeValue(){ N = "5"}},
                //       //  {":psid", new AttributeValue(){ N = "5"}},
                //       //  {":pti", new AttributeValue(){ N = "2"}},
                //     }, 
                //     //Limit = 10,
                //     //f2c9e6ba-2735-43e1-82f2-0ddf5c766c42
                // };
                //
                // //Filter Expression: " PaymentOrderStatusId = 5" ou " PaymentOrderStatusId = 5 AND PaymentTypeId = 2"
                //
                // var result = await client.QueryAsync(request, CancellationToken.None);

                var filter = new { sid= "b503630a-1d6e-4f98-a2d0-2c50fdc360b1" };
            
            var queryRequest = new QueryRequest()
            {
                IndexName = "SupplierId-DueDate-Index",
                KeyConditionExpression =  "SupplierId = :sid", //n invoices que pertencem a um buyer 
              //  FilterExpression = "PaymentOrderStatusId = :pid",
                Filter = filter,
            //    Page = "%2fV5spd%2fkrcGowMd1g58YpeiOAjD%2bbWhUvsZx6lrG5%2bDtKveLYKwXr1FuQq6Pw2XwOdsRBCyvBGPSZq8Do8UJjmajqnGST7qKp3luOYlsb%2fs26Vn%2bJKAZ5bt88b945VVYZo0ZsgnKg7llHSRIX40FmXn2RjMdlGZwf%2bVUVNbWf9yswPiw%2bYyGj8I4OZDfBkeRI%2bRI7DZysjq556Bd4LipWymDgPB4aS9OcrCRdWCaifc%3d",
               PageSize = 10
            };

            //TODO: Alter Query to accept pagesize
              var res = await baseRepository.QueryPaginatedByAsync<DataModel>(queryRequest, CancellationToken.None);
             // var res = await baseRepository.QueryPaginatedByAsync<DataModel>(queryRequest, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            Assert.Pass();
        }
    }
}