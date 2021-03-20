using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Innovt.Cloud.AWS.Dynamo
{
    internal static class Helpers
    {
        const string encriptionKey = "AAECAwQFBgcICQoL";
        private const string paginationTokenseparator = "|";
            
            
        internal static DynamoDBEntry ConvertObjectToDynamoDbEntry(object value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
                 
            return DynamoDBEntryConversion.V2.ConvertToEntry(value.ToString());
        }


        internal static string GetTableName<T>()
        {
            if (!(Attribute.GetCustomAttribute(typeof(T), typeof(DynamoDBTableAttribute)) is DynamoDBTableAttribute attribute))
                return typeof(T).Name;

            return attribute.TableName;
        }
    
        internal static Amazon.DynamoDBv2.Model.AttributeValue CreateAttributeValue(object value)
        {
            if (value is null)
                return new AttributeValue() { NULL = true };

            if (value is MemoryStream)
                return new AttributeValue() { B = value as MemoryStream };

            if (value is bool)
                return new AttributeValue() { BOOL = bool.Parse(value.ToString()) };

            if (value is List<MemoryStream>)
                return new AttributeValue() { BS = value as List<MemoryStream> };

            if (value is List<string>)
                return new AttributeValue(value as List<string>);

            if (value is int || value is double || value is float | value is decimal)
                return new AttributeValue { N = value.ToString() };
            
            if (value is DateTime time)
                return new AttributeValue { S = time.ToString("s") };

            if (value is IList<int> || value is IList<double> || value is IList<float> | value is IList<decimal>)
            {
                var array = (value as IList).Cast<string>();

                return new AttributeValue { NS = array.ToList<string>() };
            }

            if (value is IDictionary<string,object>)
            {
                var array = new Dictionary<string, AttributeValue>();

                foreach (var item in (value as IDictionary<string, object>))
                {
                    array.Add(item.Key, CreateAttributeValue(item.Value));
                }

                return new AttributeValue { M = array };
            }

            return new AttributeValue(value.ToString());

        }
        private static Dictionary<string, AttributeValue> CreateExpressionAttributeValues(object  filter, string attributes)
        {
            if (filter == null)
                return null;

            var attributeValues = new Dictionary<string, AttributeValue>();

            var properties = filter.GetType().GetProperties();
                
            if (properties.Length > 0)
            {
                foreach (var item in properties)
                {
                    var key = $":{item.Name}".ToLower();

                    if (attributes.Contains(key) && !attributeValues.ContainsKey(key))
                    {
                        var value = item.GetValue(filter);
                        
                        attributeValues.Add(key,  CreateAttributeValue(value));
                    }
                }
            }

            return attributeValues;
        }


        internal static Amazon.DynamoDBv2.Model.QueryRequest CreateQueryRequest<T>(Innovt.Cloud.Table.QueryRequest request)
        {
            var queryRequest = new Amazon.DynamoDBv2.Model.QueryRequest()
            {
                IndexName = request.IndexName,
                TableName = GetTableName<T>(),
                ConsistentRead = request.IndexName == null,
                FilterExpression = request.FilterExpression,
                ScanIndexForward = request.ScanIndexForward,
                KeyConditionExpression = request.KeyConditionExpression,
                ProjectionExpression = request.AttributesToGet,
                ExclusiveStartKey = PaginationTokenToDictionary(request.Page),
                ExpressionAttributeValues = CreateExpressionAttributeValues(request.Filter,string.Join(',',request.KeyConditionExpression, request.FilterExpression) )
            };

            if (request.PageSize.HasValue)
                queryRequest.Limit = request.PageSize == 0 ? 1 : request.PageSize.Value;

            return queryRequest;
        }


        internal static Amazon.DynamoDBv2.Model.ScanRequest CreateScanRequest<T>(Innovt.Cloud.Table.ScanRequest request)
        {
            var scanRequest =  new Amazon.DynamoDBv2.Model.ScanRequest()
            {
                IndexName = request.IndexName,
                TableName = GetTableName<T>(),
                ConsistentRead = request.IndexName == null,
                FilterExpression = request.FilterExpression,
                ProjectionExpression = request.AttributesToGet,
                ExclusiveStartKey = PaginationTokenToDictionary(request.Page),
                ExpressionAttributeValues = CreateExpressionAttributeValues(request.Filter, string.Join(',', request.FilterExpression))
            };

            if (request.PageSize.HasValue)
                scanRequest.Limit = request.PageSize ==0 ? 1 : request.PageSize.Value;

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
        
        internal static (List<T1> first,List<T2> seccond) ConvertAttributesToType<T1,T2>(List<Dictionary<string, AttributeValue>> items,string splitBy,  DynamoDBContext context)
        {
            if (items is null)
                return (null,null);

            var result1 = new List<T1>();
            var result2 = new List<T2>();
           
            foreach (var item in items)
            {
                var doc = Document.FromAttributeMap(item);

                if(item.ContainsKey("EntityType") && (item["EntityType"].S == splitBy))
                {
                    result1.Add(context.FromDocument<T1>(doc));
                }
                else
                {
                    result2.Add(context.FromDocument<T2>(doc));
                }
            }

            return (result1,result2);
        }
        internal static (List<T1> first,List<T2> seccond,List<T3> third) ConvertAttributesToType<T1,T2,T3>(List<Dictionary<string, AttributeValue>> items,string[] splitBy, DynamoDBContext context)
        {
            if (items is null)
                return (null,null,null);

            var result1 = new List<T1>();
            var result2 = new List<T2>();
            var result3 = new List<T3>();
           
            foreach (var item in items)
            {
                var doc = Document.FromAttributeMap(item);
              
                if(!item.ContainsKey("EntityType"))
                    continue;
                    
                if(item["EntityType"].S == splitBy[0])
                {
                    result1.Add(context.FromDocument<T1>(doc));
                }
                else
                {
                    if (item["EntityType"].S == splitBy[1])
                    {
                        result2.Add(context.FromDocument<T2>(doc));
                    }
                    else
                    {
                        result3.Add(context.FromDocument<T3>(doc));
                    }
                }
            }

            return (result1,result2,result3);
        }
        
        internal static string CreatePaginationToken(Dictionary<string, AttributeValue> lastEvaluatedKey)
        {
            if (lastEvaluatedKey.IsNullOrEmpty())
                return null;

            var stringBuilder = new StringBuilder();

            foreach (var (attributeKey,attributeValue) in lastEvaluatedKey)
            {   
                var value = attributeValue.S != null ? $"S:{attributeValue.S}" : $"N:{attributeValue.N}";

                stringBuilder.Append($"{attributeKey}{paginationTokenseparator}{value}\\r\\n");
            }

            return Convert.ToBase64String(stringBuilder.ToString().Zip()).UrlEncode();
        }

        private static Dictionary<string, AttributeValue> PaginationTokenToDictionary(string paginationToken)
        {
            if (paginationToken.IsNullOrEmpty())
                return null;

            var decryptedToken = Convert.FromBase64String(paginationToken.UrlDecode()).Unzip();
            
            var result = new Dictionary<string, AttributeValue>();

            var keys = decryptedToken.Split("\\r\\n");

            foreach (var key in keys)
            {
                var attributes = key.Split(paginationTokenseparator);

                if (attributes.Length < 2)
                    continue;

                var attributeKey = attributes[0];
                //Just in case that the separator was used
                var attributeValue = string.Join(paginationTokenseparator, attributes,1,attributes.Length - 1);

                if(attributeValue.StartsWith("S:"))
                {
                    result.Add(attributeKey, new AttributeValue(attributeValue.Substring(2,attributeValue.Length - 2)));
                }
                else
                {
                    result.Add(attributeKey, new AttributeValue()
                    {
                        N = attributeValue.Substring(2,attributeValue.Length - 2)
                    });

                }
            }

            return result;
        }
    }
}
