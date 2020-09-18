
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.Table;
using Innovt.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innovt.Cloud.AWS.Dynamo
{
    internal static class Helpers
    {
        const string encriptionKey = "AAECAwQFBgcICQoL";

        internal static DynamoDBEntry ConvertObjectToDynamoDbEntry(object value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
                 
            return DynamoDBEntryConversion.V2.ConvertToEntry(value.ToString());
        }


        internal static string GetTableName<T>()
        {
            if (!(Attribute.GetCustomAttribute(typeof(T), typeof(DynamoDBTableAttribute)) is DynamoDBTableAttribute attribute))
                return nameof(T);

            return attribute.TableName;
        }

        //internal static Filter CreateScanFilter(IList<FilterCondition> filters)
        //{
        //    if (filters is null)
        //        return null;

        //    var  scanFilter = new ScanFilter();

        //    foreach (var condition in filters)
        //    {
        //        if (condition.Value is null)
        //        {
        //            scanFilter.AddCondition(condition.AttributeName, (ScanOperator)condition.Operator.Id);
        //        }
        //        else
        //        {
        //            var dynamoDbValue = ConvertObjectToDynamoDbEntry(condition.Value);
        //            scanFilter.AddCondition(condition.AttributeName, (ScanOperator)condition.Operator.Id, dynamoDbValue);
        //        }
        //    }

        //    return scanFilter;
        //}

        internal static Amazon.DynamoDBv2.Model.QueryRequest CreateQueryRequest<T>(Innovt.Cloud.Table.QueryRequest request)
        {
            var queryRequest =  new Amazon.DynamoDBv2.Model.QueryRequest()
            {
                IndexName = request.IndexName,
                TableName = GetTableName<T>(),
                ConsistentRead = request.IndexName == null,
                Limit = request.PageSize == 0 ? 1 : request.PageSize,
                FilterExpression = request.FilterExpression,
                KeyConditionExpression = request.KeyConditionExpression,
                ProjectionExpression = string.Join(',', request.AttributesToGet),
                ExclusiveStartKey = PaginationTokenToDictionary(request.PaginationToken)
            };

            if (request.Filter != null) {

                var properties = typeof(T).GetProperties();

                if (properties.Length > 0)
                {
                    queryRequest.ExpressionAttributeValues = new Dictionary<string, AttributeValue>();



                }


    //            "Id = :v_Id and ReplyDateTime > :v_twoWeeksAgo",
    //ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
    //    {":v_Id", new AttributeValue { S =  "Amazon DynamoDB#DynamoDB Thread 2" }},
    //    {":v_twoWeeksAgo", new AttributeValue { S =  twoWeeksAgoString }}
    //},

            }
           // queryRequest.KeyConditionExpression


            return queryRequest;
        }

        internal static Amazon.DynamoDBv2.Model.ScanRequest CreateScanRequest<T>(Innovt.Cloud.Table.ScanRequest request)
        {
            var scanRequest =  new Amazon.DynamoDBv2.Model.ScanRequest()
            {
                IndexName = request.IndexName,
                TableName = GetTableName<T>(),
                ConsistentRead = request.IndexName == null,
                Limit = request.PageSize == 0 ? 1 : request.PageSize,
                FilterExpression = request.FilterExpression,
                ProjectionExpression = string.Join(',', request.AttributesToGet),
                ExclusiveStartKey = PaginationTokenToDictionary(request.PaginationToken)
            };

           //scanRequest.ExpressionAttributeNames


            return scanRequest;
        }
    

      internal static List<T> ConvertAttributesToType<T>(List<Dictionary<string, AttributeValue>> items, DynamoDBContext context)
    {
            if (items is null)
                return null;

            var result = new List<T>();

            items.ForEach(item =>
            {
                var doc = Document.FromAttributeMap(item);
                result.Add(context.FromDocument<T>(doc));
            });

            return result;
        }


    internal static string CreatePaginationToken(Dictionary<string, AttributeValue> lastEvaluatedKey)
        {
            if (lastEvaluatedKey is null)
                return null;

            var stringBuilder = new StringBuilder();

            foreach (var item in lastEvaluatedKey)
            {
                //TODO:Binary not supported
                var value = item.Value.S != null ? $"S:{item.Value.S}" : $"N:{item.Value.N}";

                stringBuilder.Append($"{item.Key}:{value},");
            }

            return Cryptography.AesEncrypt(stringBuilder.ToString(), encriptionKey);
        }



        internal static Dictionary<string, AttributeValue> PaginationTokenToDictionary(string paginationToken)
        {
            if (paginationToken is null)
                return null;

            var decriptedToken = Cryptography.AesDecrypt(paginationToken, encriptionKey);

            var result = new Dictionary<string, AttributeValue>();

            var keys = decriptedToken.Split(",");

            foreach (var key in keys)
            {
                var attributes = key.Split(":");

                if (attributes.Length != 3)
                    continue;

                AttributeValue attributeValue;

                if(attributes[1] == "S")
                {
                    attributeValue = new AttributeValue(attributes[2]);
                }
                else
                {
                    attributeValue = new AttributeValue()
                    {
                        N = attributes[2]
                    };
                }

                result.Add(attributes[0], attributeValue);
            }

            return result;
        }
    }
}
