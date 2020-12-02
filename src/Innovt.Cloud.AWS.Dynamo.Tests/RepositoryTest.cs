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

namespace Innovt.Cloud.AWS.Dynamo.Tests
{
    public class Tests
    {
        private ILogger loggerMock = null;
        private BaseRepository baseRepository = null;
        
        [SetUp]
        public void Setup()
        {
            loggerMock = NSubstitute.Substitute.For<ILogger>();
                
            baseRepository= new BaseRepository(loggerMock, new DefaultAWSConfiguration());
        }

        [Test]
        public async Task Test1()
        {
            try
            {
                //var profile = new CredentialProfile("antecipa-dev",new CredentialProfileOptions());
            
            var client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);
            
            var request = new Amazon.DynamoDBv2.Model.QueryRequest()
            {
                TableName = "Invoices",
                IndexName = "BuyerId-InvoiceId-Index",
                KeyConditionExpression = "BuyerId = :bid",
               // FilterExpression = " PaymentOrderStatusId = :pid OR (PaymentOrderStatusId = :psid AND PaymentTypeId = :pti)",
               FilterExpression = " PaymentOrderStatusId = :pid ",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {":bid", new AttributeValue(){ S = "f2c9e6ba-2735-43e1-82f2-0ddf5c766c42"}},
                    {":pid", new AttributeValue(){ N = "5"}},
                  //  {":psid", new AttributeValue(){ N = "5"}},
                  //  {":pti", new AttributeValue(){ N = "2"}},
                }, 
                Limit = 10,
                //f2c9e6ba-2735-43e1-82f2-0ddf5c766c42
            };
            
            var result = client.Paginators.Query(request);


            var res = result.Responses.GetAsyncEnumerator();
            
            
            
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

            foreach (var item in res.Current.Items)
            {
                Console.WriteLine(item);
            }

            //   var res = await baseRepository.QueryAsync<DataModel>();

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