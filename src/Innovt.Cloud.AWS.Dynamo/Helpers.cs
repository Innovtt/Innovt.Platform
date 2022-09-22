// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Dynamo
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.Table;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BatchWriteItemRequest = Amazon.DynamoDBv2.Model.BatchWriteItemRequest;
using QueryRequest = Amazon.DynamoDBv2.Model.QueryRequest;
using ScanRequest = Amazon.DynamoDBv2.Model.ScanRequest;

namespace Innovt.Cloud.AWS.Dynamo;

internal static class Helpers
{
    private const string PaginationTokenSeparator = "|";
    private const string EntitySplitter = "EntityType";

    //code from Aws SDK 
    
    private static string GetTableName<T>()
    {
        if (Attribute.GetCustomAttribute(typeof(T), typeof(DynamoDBTableAttribute)) is not DynamoDBTableAttribute
            attribute)
            return typeof(T).Name;

        return attribute.TableName;
    }


    private static Dictionary<string, AttributeValue> CreateExpressionAttributeValues(object filter, string attributes)
    {
        if (filter == null)
            return new Dictionary<string, AttributeValue>();

        var attributeValues = new Dictionary<string, AttributeValue>();

        var properties = filter.GetType().GetProperties();

        if (properties.Length <= 0) return attributeValues;

        foreach (var item in properties)
        {
            var key = $":{item.Name}".ToLower(CultureInfo.CurrentCulture);

            if (attributes.Contains(key, StringComparison.InvariantCultureIgnoreCase) &&
                !attributeValues.ContainsKey(key))
            {
                var value = item.GetValue(filter);

                attributeValues.Add(key, AttributeConverter.CreateAttributeValue(value));
            }
        }

        return attributeValues;
    }

    internal static QueryRequest CreateQueryRequest<T>(
        Table.QueryRequest request)
    {
        var queryRequest = new QueryRequest
        {
            IndexName = request.IndexName,
            TableName = GetTableName<T>(),
            ConsistentRead = request.IndexName == null,
            FilterExpression = request.FilterExpression,
            ScanIndexForward = request.ScanIndexForward,
            KeyConditionExpression = request.KeyConditionExpression,
            ProjectionExpression = request.AttributesToGet,
            ExclusiveStartKey = PaginationTokenToDictionary(request.Page),
            ExpressionAttributeValues = CreateExpressionAttributeValues(request.Filter,
                string.Join(',', request.KeyConditionExpression, request.FilterExpression))
        };

        if (request.PageSize.HasValue)
            queryRequest.Limit = request.PageSize == 0 ? 1 : request.PageSize.Value;

        return queryRequest;
    }


    internal static ScanRequest CreateScanRequest<T>(Table.ScanRequest request)
    {
        var scanRequest = new ScanRequest
        {
            IndexName = request.IndexName,
            TableName = GetTableName<T>(),
            ConsistentRead = request.IndexName == null,
            FilterExpression = request.FilterExpression,
            ProjectionExpression = request.AttributesToGet,
            ExclusiveStartKey = PaginationTokenToDictionary(request.Page),
            ExpressionAttributeValues =
                CreateExpressionAttributeValues(request.Filter, string.Join(',', request.FilterExpression))
        };

        if (request.PageSize.HasValue)
            scanRequest.Limit = request.PageSize == 0 ? 1 : request.PageSize.Value;

        return scanRequest;
    }

    internal static Dictionary<string, KeysAndAttributes> CreateBatchGetItemRequest(Table.BatchGetItemRequest request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        var result = new Dictionary<string, KeysAndAttributes>();

        foreach (var item in request.Items)
        {
            result.Add(item.Key, new KeysAndAttributes()
            {
                 ConsistentRead =  item.Value.ConsistentRead,
                 ExpressionAttributeNames =item.Value.ExpressionAttributeNames,
                 ProjectionExpression = item.Value.ProjectionExpression,
                 Keys = item.Value.Keys.Select(AttributeConverter.ConvertToAttributeValues).ToList()
            });

        }

        return result;
    }

    internal static BatchWriteItemRequest CreateBatchWriteItemRequest(Table.BatchWriteItemRequest request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        
        var writeRequest = new BatchWriteItemRequest()
        {
            RequestItems = new Dictionary<string, List<WriteRequest>>()
        };

        foreach (var item in request.Items)
        {
            var writeRequests = (from r in item.Value
                select new WriteRequest()
                {
                    DeleteRequest = r.DeleteRequest is null ? null :  new DeleteRequest(AttributeConverter.ConvertToAttributeValues(r.DeleteRequest)),
                    PutRequest = r.PutRequest is null ? null: new PutRequest(AttributeConverter.ConvertToAttributeValues(r.PutRequest))
                }).ToList();

            writeRequest.RequestItems.Add(item.Key, writeRequests);
        }

        return writeRequest;
    }
    

    internal static IList<T> ConvertAttributesToType<T>(IList<Dictionary<string, AttributeValue>> items)
    {
        return items is null ? new List<T>() : items.Select(AttributeConverter.ConvertAttributesToType<T>).ToList();
    }

    internal static (IList<T1> first, IList<T2> seccond) ConvertAttributesToType<T1, T2>(IList<Dictionary<string, AttributeValue>> items, string splitBy)
    {
        if (items is null)
            return (null, null);

        var result1 = new List<T1>();
        var result2 = new List<T2>();

        foreach (var item in items)
        {
            if (item.ContainsKey(EntitySplitter) && item["EntityType"].S == splitBy)
            {
                result1.Add(AttributeConverter.ConvertAttributesToType<T1>(item));
            }
            else
                result2.Add(AttributeConverter.ConvertAttributesToType<T2>(item));
        }

        return (result1, result2);
    }

    internal static (List<T1> first, IList<T2> seccond, IList<T3> third) ConvertAttributesToType<T1, T2, T3>(IList<Dictionary<string, AttributeValue>> items, string[] splitBy)
    {
        if (items is null)
            return (null, null, null);

        var result1 = new List<T1>();
        var result2 = new List<T2>();
        var result3 = new List<T3>();

        foreach (var item in items)
        {
            if (!item.ContainsKey("EntityType"))
                continue;

            if (item["EntityType"].S == splitBy[0])
            {
                result1.Add(AttributeConverter.ConvertAttributesToType<T1>(item));
            }
            else
            {
                if (item["EntityType"].S == splitBy[1])
                    result2.Add(AttributeConverter.ConvertAttributesToType<T2>(item));
                else
                    result3.Add(AttributeConverter.ConvertAttributesToType<T3>(item));
            }
        }

        return (result1, result2, result3);
    }

    internal static (IList<T1> first, IList<T2> seccond, IList<T3> third, IList<T4> fourth) ConvertAttributesToType<T1,
        T2, T3, T4>(
        IList<Dictionary<string, AttributeValue>> items, string[] splitBy)
    {
        if (items is null)
            return (null, null, null, null);

        var result1 = new List<T1>();
        var result2 = new List<T2>();
        var result3 = new List<T3>();
        var result4 = new List<T4>();

        foreach (var item in items)
        {
            if (!item.ContainsKey("EntityType"))
                continue;

            if (item["EntityType"].S == splitBy[0])
            {
                result1.Add(AttributeConverter.ConvertAttributesToType<T1>(item));
            }
            else
            {
                if (item["EntityType"].S == splitBy[1])
                {
                    result2.Add(AttributeConverter.ConvertAttributesToType<T2>(item));
                }
                else
                {
                    if (item["EntityType"].S == splitBy[2])
                        result3.Add(AttributeConverter.ConvertAttributesToType<T3>(item));
                    else
                        result4.Add(AttributeConverter.ConvertAttributesToType<T4>(item));
                }
            }
        }

        return (result1, result2, result3, result4);
    }

    internal static (IList<T1> first, IList<T2> seccond, IList<T3> third, IList<T4> fourth, IList<T5> fifth)
        ConvertAttributesToType<T1, T2, T3, T4, T5>(
            IList<Dictionary<string, AttributeValue>> items, string[] splitBy)
    {
        if (items is null)
            return (null, null, null, null, null);

        var result1 = new List<T1>();
        var result2 = new List<T2>();
        var result3 = new List<T3>();
        var result4 = new List<T4>();
        var result5 = new List<T5>();

        foreach (var item in items)
        {
            if (!item.ContainsKey("EntityType"))
                continue;
            
            if (item["EntityType"].S == splitBy[0])
            {
                result1.Add(AttributeConverter.ConvertAttributesToType<T1>(item));
            }
            else
            {
                if (item["EntityType"].S == splitBy[1])
                {
                    result2.Add(AttributeConverter.ConvertAttributesToType<T2>(item));
                }
                else
                {
                    if (item["EntityType"].S == splitBy[2])
                    {
                        result3.Add(AttributeConverter.ConvertAttributesToType<T3>(item));
                    }
                    else
                    {
                        if (item["EntityType"].S == splitBy[3])
                            result4.Add(AttributeConverter.ConvertAttributesToType<T4>(item));
                        else
                            result5.Add(AttributeConverter.ConvertAttributesToType<T5>(item));
                    }
                }
            }
        }

        return (result1, result2, result3, result4, result5);
    }

    internal static string CreatePaginationToken(Dictionary<string, AttributeValue> lastEvaluatedKey)
    {
        if (lastEvaluatedKey.IsNullOrEmpty())
            return null;

        var stringBuilder = new StringBuilder();

        foreach (var (attributeKey, attributeValue) in lastEvaluatedKey)
        {
            var value = attributeValue.S != null ? $"S:{attributeValue.S}" : $"N:{attributeValue.N}";

            stringBuilder.Append($"{attributeKey}{PaginationTokenSeparator}{value}\\r\\n");
        }

        return Convert.ToBase64String(stringBuilder.ToString().Zip()).UrlEncode();
    }

    private static Dictionary<string, AttributeValue> PaginationTokenToDictionary(string paginationToken)
    {
        if (paginationToken is null)
            return null;

        var decryptedToken = Convert.FromBase64String(paginationToken.UrlDecode()).Unzip();

        var result = new Dictionary<string, AttributeValue>();

        var keys = decryptedToken.Split("\\r\\n");

        foreach (var key in keys)
        {
            var attributes = key.Split(PaginationTokenSeparator);

            if (attributes.Length < 2)
                continue;

            var attributeKey = attributes[0];
            //Just in case that the separator was used
            var attributeValue = string.Join(PaginationTokenSeparator, attributes, 1, attributes.Length - 1);

            if (attributeValue.StartsWith("S:", StringComparison.OrdinalIgnoreCase))
                result.Add(attributeKey,
                    new AttributeValue(attributeValue[2..]));
            else
                result.Add(attributeKey, new AttributeValue
                {
                    N = attributeValue[2..]
                });
        }

        return result;
    }

    private static Put CreatePutTransactItem(TransactionWriteItem transactionWriteItem)
    {
        if (transactionWriteItem.OperationType != TransactionWriteOperationType.Put)
            return null;

        return new Put
        {
            ConditionExpression = transactionWriteItem.ConditionExpression,
            TableName = transactionWriteItem.TableName,
            ExpressionAttributeValues = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.ExpressionAttributeValues),
            Item = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.Items)
        };
    }


    private static ConditionCheck CreateConditionCheckTransactItem(TransactionWriteItem transactionWriteItem)
    {
        if (transactionWriteItem.OperationType != TransactionWriteOperationType.Check)
            return null;

        return new ConditionCheck
        {
            ConditionExpression = transactionWriteItem.ConditionExpression,
            TableName = transactionWriteItem.TableName,
            ExpressionAttributeValues = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.ExpressionAttributeValues),
            Key = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.Keys)
        };
    }

    private static Delete CreateDeleteTransactItem(TransactionWriteItem transactionWriteItem)
    {
        if (transactionWriteItem.OperationType != TransactionWriteOperationType.Delete)
            return null;

        return new Delete
        {
            ConditionExpression = transactionWriteItem.ConditionExpression,
            TableName = transactionWriteItem.TableName,
            ExpressionAttributeValues = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.ExpressionAttributeValues),
            Key = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.Keys)
        };
    }

    private static Update CreateUpdateTransactItem(TransactionWriteItem transactionWriteItem)
    {
        if (transactionWriteItem.OperationType != TransactionWriteOperationType.Update)
            return null;

        return new Update
        {
            ConditionExpression = transactionWriteItem.ConditionExpression,
            TableName = transactionWriteItem.TableName,
            UpdateExpression = transactionWriteItem.UpdateExpression,
            ExpressionAttributeValues = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.ExpressionAttributeValues),
            Key = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.Keys)
        };
    }

    internal static TransactWriteItem CreateTransactionWriteItem(TransactionWriteItem transactionWriteItem)
    {
        if (transactionWriteItem is null) throw new ArgumentNullException(nameof(transactionWriteItem));

        return new TransactWriteItem
        {
            ConditionCheck = CreateConditionCheckTransactItem(transactionWriteItem),
            Put = CreatePutTransactItem(transactionWriteItem),
            Delete = CreateDeleteTransactItem(transactionWriteItem),
            Update = CreateUpdateTransactItem(transactionWriteItem)
        };
    }


    internal static TransactGetItem CreateTransactionGetItem(TransactionWriteItem transactionWriteItem)
    {
        if (transactionWriteItem is null) throw new ArgumentNullException(nameof(transactionWriteItem));

        return new TransactGetItem
        {
            Get = new Get()
        };
    }
}