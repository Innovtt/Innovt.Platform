// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Dynamo.Tests
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Dynamo.Tests
{
    public class Result
    {
        public int SEQUENCE { get; set; }
    }


    public class BaseRepository : Repository
    {
        public BaseRepository(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
        {
        }

        public BaseRepository(ILogger logger, IAwsConfiguration configuration, string region) : base(logger,
            configuration, region)
        {
        }

        public async Task<int> UpdateSequence()
        {
            //var keys = new Dictionary<string, AttributeValue>
            //    {
            //        {"PK", new AttributeValue("ORDER")},
            //        {"SK", new AttributeValue("SEQUENCE")}
            //    };

            //var updateValues = new Dictionary<string, AttributeValueUpdate>
            //    {
            //    {
            //        "SEQUENCE",
            //        new AttributeValueUpdate(new AttributeValue {N = "1" }, AttributeAction.ADD)
            //    }
            //    };

            //await UpdateAsync("Anticipation", keys, updateValues, CancellationToken.None).ConfigureAwait(false);


            var keys = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue("ORDER") },
                { "SK", new AttributeValue("SEQUENCE") }
            };

            var updateValues = new Dictionary<string, AttributeValueUpdate>
            {
                {
                    "SEQUENCE",
                    new AttributeValueUpdate(new AttributeValue { N = "1" }, AttributeAction.ADD)
                }
            };

            var res = await UpdateAsync<Result>(new UpdateItemRequest
            {
                AttributeUpdates = updateValues,
                Key = keys,
                TableName = "Anticipation",
                ReturnValues = ReturnValue.UPDATED_NEW
            }, CancellationToken.None).ConfigureAwait(false);


            return 0;
        }
    }
}