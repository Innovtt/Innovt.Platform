// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalh�es
// Project: Innovt.Cloud.AWS.Dynamo.Tests
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Dynamo.Tests
{
    public class RepositoryTests
    {
        private IAwsConfiguration configuration;
        private ILogger loggerMock;

        [SetUp]
        public void Setup()
        {
            loggerMock = Substitute.For<ILogger>();


            configuration = new DefaultAWSConfiguration("antecipa-dev");
        }

        [Test]
        public async Task Test1()
        {
            try
            {
                loggerMock.Info(configuration.AccountNumber);

                using var repo = new BaseRepository(loggerMock, configuration);

                var request = new TransactionWriteRequest()
                {
                    TransactItems = new List<TransactionWriteItem>()
                };

                request.TransactItems.Add(new TransactionWriteItem()
                {
                    TableName = "Users",
                    OperationType = TransactionWriteOperationType.Delete,
                    //UpdateExpression = "SET Deleted = :up ",
                    //ConditionExpression = "Deleted = :d",
                    Keys = new Dictionary<string, object>()
                    {
                        { "PK", "U#michemob@gmail.com" },
                        { "SK", "PROFILE" }
                    },
                    ExpressionAttributeValues = new Dictionary<string, object>()
                    {
                        //{":d", true }
                        //{":up", true }
                    }
                });


                //request.TransactItems.Add(new TransactionWriteItem() {
                //    TableName= "Users",
                //    OperationType = TransactionWriteOperationType.Update,
                //    UpdateExpression = "SET Deleted = :up ",
                //    ConditionExpression = "Deleted = :d",
                //    Keys = new Dictionary<string, object>()
                //    {
                //        {"PK", "U#alexcar15969937alexcar1596@antecipa.com" },
                //        {"SK", "PROFILE" }
                //    },
                //    ExpressionAttributeValues = new Dictionary<string, object>()
                //    {
                //        {":d", false },
                //        {":up", true }
                //    }
                //});

                await repo.TransactWriteItemsAsync(request, CancellationToken.None).ConfigureAwait(false);


                //            UpdateExpression = "SET Deleted = :up ",
                //            Key = new Dictionary<string, AttributeValue>()
                //            {
                //                {"PK", new AttributeValue(){ S = "U#alexcar15969937alexcar1596@antecipa.com" }},
                //                {"SK", new AttributeValue(){ S = "PROFILE" }}
                //            },
                //            ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                //            {
                //                {":d", new AttributeValue(){ BOOL = true }},
                //                {":up", new AttributeValue(){ BOOL = false }}
                //            }


                //await repo.TransactWriteItemsAsync(request,CancellationToken.None).ConfigureAwait(false);


                ////var profile = new CredentialProfile("antecipa-dev",new CredentialProfileOptions());
                //using var client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);

                ////var result = await client.QueryAsync(new Amazon.DynamoDBv2.Model.QueryRequest("Users")
                ////{
                ////    KeyConditionExpression = "SK=:sk",
                ////    IndexName = "SK-PK-Index",
                ////    ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                ////     {
                ////         {":sk", new AttributeValue(){ S = "PROFILE"}}
                ////     }
                ////}).ConfigureAwait(false);

                //var itens = new List<TransactWriteItem>();

                //itens.Add(new TransactWriteItem()
                //{}

                //    Update = new Update()
                //    {
                //        TableName = "Users",
                //        UpdateExpression = "SET Deleted = :up ",
                //        Key = new Dictionary<string, AttributeValue>()
                //            {
                //                {"PK", new AttributeValue(){ S = "U#alexcar15969937alexcar1596@antecipa.com" }},
                //                {"SK", new AttributeValue(){ S = "PROFILE" }}
                //            },
                //        ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                //            {
                //                {":d", new AttributeValue(){ BOOL = true }},
                //                {":up", new AttributeValue(){ BOOL = false }}
                //            }
                //    }
                //});

                //var updateResult = await client.TransactWriteItemsAsync(new TransactWriteItemsRequest()
                //{
                //    TransactItems = itens,

                //}).ConfigureAwait(false);

                //Console.Write(updateResult);

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

                //var filter = new {sid = "b503630a-1d6e-4f98-a2d0-2c50fdc360b1"};

                //var queryRequest = new QueryRequest
                //{
                //    IndexName = "SupplierId-DueDate-Index",
                //    KeyConditionExpression = "SupplierId = :sid", //n invoices que pertencem a um buyer 
                //    //  FilterExpression = "PaymentOrderStatusId = :pid",
                //    Filter = filter,
                //    //    Page = "%2fV5spd%2fkrcGowMd1g58YpeiOAjD%2bbWhUvsZx6lrG5%2bDtKveLYKwXr1FuQq6Pw2XwOdsRBCyvBGPSZq8Do8UJjmajqnGST7qKp3luOYlsb%2fs26Vn%2bJKAZ5bt88b945VVYZo0ZsgnKg7llHSRIX40FmXn2RjMdlGZwf%2bVUVNbWf9yswPiw%2bYyGj8I4OZDfBkeRI%2bRI7DZysjq556Bd4LipWymDgPB4aS9OcrCRdWCaifc%3d",
                //    PageSize = 10
                //};

                //TODO: Alter Query to accept pagesize
                //var res = await baseRepository.QueryPaginatedByAsync<DataModel>(queryRequest, CancellationToken.None);
                // var res = await baseRepository.QueryPaginatedByAsync<DataModel>(queryRequest, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Assert.Pass();
        }


        [Test]
        //[Ignore("Only Internal Tests")]
        public async Task ValidateSequence()
        {
            try
            {
                loggerMock.Info(configuration.AccountNumber);

                using var repo = new BaseRepository(loggerMock, configuration);

                var res = await repo.UpdateSequence();
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