// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Dynamo.Converters;
using Innovt.Cloud.Table;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;
using BatchGetItemRequest = Innovt.Cloud.Table.BatchGetItemRequest;
using BatchWriteItemRequest = Amazon.DynamoDBv2.Model.BatchWriteItemRequest;
using QueryRequest = Amazon.DynamoDBv2.Model.QueryRequest;
using ScanRequest = Amazon.DynamoDBv2.Model.ScanRequest;

namespace Innovt.Cloud.AWS.Dynamo.Helpers;

/// <summary>
///     A utility class providing helper methods for various tasks.
/// </summary>
internal static class QueryHelper
{
    /// <summary>
    ///     A constant string used as a separator for pagination tokens.
    /// </summary>
    private const string PaginationTokenSeparator = "|";

    /// <summary>
    ///     A constant string used to denote entity splitting in attribute names.
    /// </summary>
    private const string EntitySplitter = "EntityType";

    /// <summary>
    ///     Creates a dictionary of expression attribute values based on the provided filter object and attribute names.
    /// </summary>
    /// <param name="filter">The filter object used to extract property values for attribute values.</param>
    /// <param name="attributes">A string containing attribute names for which attribute values are needed.</param>
    /// <returns>
    ///     A dictionary of expression attribute values where keys are placeholders (e.g., ":PropertyName") and values are
    ///     corresponding attribute values extracted from the filter object.
    /// </returns>
    /// <remarks>
    ///     This method scans the properties of the filter object and checks if their names match the provided attribute names.
    ///     For each matching attribute name, it creates an expression attribute value and adds it to the dictionary.
    ///     The keys in the dictionary are formatted as placeholders (e.g., ":PropertyName") and are case-insensitive.
    /// </remarks>
    /// <seealso cref="AttributeConverter.CreateAttributeValue" />
    private static Dictionary<string, AttributeValue> CreateExpressionAttributeValues(object filter, string attributes)
    {
        if (filter == null)
            return new Dictionary<string, AttributeValue>();

        var attributeValues = new Dictionary<string, AttributeValue>();

        var properties = filter.GetType().GetProperties();

        if (properties.Length == 0 && filter is ExpandoObject expando)
            return CreateExpressionAttributeValues(expando, attributes);

        foreach (var item in properties)
        {
            var key = $":{item.Name}".ToLower(CultureInfo.CurrentCulture);

            if (!attributes.Contains(key, StringComparison.InvariantCultureIgnoreCase) ||
                attributeValues.ContainsKey(key)) continue;

            var value = item.GetValue(filter);
            attributeValues.Add(key, AttributeConverter.CreateAttributeValue(value));
        }

        return attributeValues;
    }

    /// <summary>
    ///     Create a list of attribute values based on the Expando object filter and attribute names.
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="attributes"></param>
    /// <returns></returns>
    private static Dictionary<string, AttributeValue> CreateExpressionAttributeValues(ExpandoObject filter,
        string attributes)
    {
        if (filter == null)
            return new Dictionary<string, AttributeValue>();

        var attributeValues = new Dictionary<string, AttributeValue>();

        var properties = filter as IDictionary<string, object>;

        if (properties.Count <= 0)
            return attributeValues;

        foreach (var (name, value) in properties)
        {
            var key = $":{name}".ToLower(CultureInfo.CurrentCulture);

            if (attributes.Contains(key, StringComparison.InvariantCultureIgnoreCase) &&
                !attributeValues.ContainsKey(key))
                attributeValues.Add(key, AttributeConverter.CreateAttributeValue(value));
        }

        return attributeValues;
    }

    /// <summary>
    ///     Creates a QueryRequest object based on the provided Table.QueryRequest and generic type T.
    /// </summary>
    /// <typeparam name="T">The type representing the DynamoDB table.</typeparam>
    /// <param name="request">The Table.QueryRequest object containing query parameters.</param>
    /// <param name="context">The DynamoContext.</param>
    /// <returns>A QueryRequest object configured based on the provided Table.QueryRequest.</returns>
    /// <remarks>
    ///     This method creates a QueryRequest object for querying a DynamoDB table based on the provided query parameters.
    ///     It sets properties such as TableName, IndexName, FilterExpression, KeyConditionExpression, ProjectionExpression,
    ///     ExclusiveStartKey, ExpressionAttributeValues, and Limit based on the input Table.QueryRequest.
    ///     The TableName is determined based on the generic type T and DynamoDBTableAttribute.
    ///     If the IndexName is specified in the request, it is set in the IndexName property; otherwise, it uses the primary
    ///     index.
    ///     The FilterExpression, KeyConditionExpression, and ProjectionExpression are copied from the request.
    ///     The ExclusiveStartKey is parsed from the request's pagination token.
    ///     ExpressionAttributeValues are generated based on the provided filter object and a combined string of
    ///     KeyConditionExpression and FilterExpression.
    ///     If a PageSize is specified in the request, the Limit property is set accordingly.
    /// </remarks>
    /// <seealso cref="Table.QueryRequest" />
    internal static QueryRequest CreateQueryRequest<T>(Table.QueryRequest request, DynamoContext context = null)
        where T : class
    {
        var queryRequest = new QueryRequest
        {
            IndexName = request.IndexName,
            TableName = TableHelper.GetTableName<T>(context),
            ConsistentRead = request.IndexName == null,
            FilterExpression = request.FilterExpression,
            ScanIndexForward = request.ScanIndexForward,
            KeyConditionExpression = request.KeyConditionExpression,
            ProjectionExpression = request.AttributesToGet,
            ExclusiveStartKey = PaginationTokenToDictionary(request.Page),
            ExpressionAttributeValues = CreateExpressionAttributeValues(request.Filter,
                string.Join(',', request.KeyConditionExpression, request.FilterExpression)),
            ExpressionAttributeNames = request.ExpressionAttributeNames
        };

        if (request.PageSize.HasValue)
            queryRequest.Limit = request.PageSize == 0 ? 1 : request.PageSize.Value;

        return queryRequest;
    }

    /// <summary>
    ///     Creates a ScanRequest object based on the provided Table.ScanRequest and generic type T.
    /// </summary>
    /// <typeparam name="T">The type representing the DynamoDB table.</typeparam>
    /// <param name="request">The Table.ScanRequest object containing scan parameters.</param>
    /// <param name="context">The repository context</param>
    /// <returns>A ScanRequest object configured based on the provided Table.ScanRequest.</returns>
    /// <remarks>
    ///     This method creates a ScanRequest object for scanning a DynamoDB table based on the provided scan parameters.
    ///     It sets properties such as TableName, IndexName, FilterExpression, ProjectionExpression,
    ///     ExclusiveStartKey, ExpressionAttributeValues, and Limit based on the input Table.ScanRequest.
    ///     The TableName is determined based on the generic type T and DynamoDBTableAttribute.
    ///     If the IndexName is specified in the request, it is set in the IndexName property; otherwise, it uses the primary
    ///     index.
    ///     The FilterExpression and ProjectionExpression are copied from the request.
    ///     The ExclusiveStartKey is parsed from the request's pagination token.
    ///     ExpressionAttributeValues are generated based on the provided filter object and FilterExpression.
    ///     If a PageSize is specified in the request, the Limit property is set accordingly.
    /// </remarks>
    /// <seealso cref="Table.ScanRequest" />
    internal static ScanRequest CreateScanRequest<T>(Table.ScanRequest request, DynamoContext context = null)
        where T : class
    {
        var scanRequest = new ScanRequest
        {
            IndexName = request.IndexName,
            TableName = TableHelper.GetTableName<T>(context),
            ConsistentRead = request.IndexName == null,
            FilterExpression = request.FilterExpression,
            ProjectionExpression = request.AttributesToGet,
            ExclusiveStartKey = PaginationTokenToDictionary(request.Page),
            ExpressionAttributeValues =
                CreateExpressionAttributeValues(request.Filter, string.Join(',', request.FilterExpression)),
            ExpressionAttributeNames = request.ExpressionAttributeNames
        };

        if (request.PageSize.HasValue)
            scanRequest.Limit = request.PageSize == 0 ? 1 : request.PageSize.Value;

        return scanRequest;
    }

    /// <summary>
    ///     Creates a dictionary of KeysAndAttributes for BatchGetItem based on the provided Table.BatchGetItemRequest.
    /// </summary>
    /// <param name="request">The Table.BatchGetItemRequest object containing batch get item parameters.</param>
    /// <param name="context">Ths repository context for dynamo.</param>
    /// <returns>A dictionary where the key is the table name and the value is a KeysAndAttributes object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input request is null.</exception>
    /// <remarks>
    ///     This method creates a dictionary of KeysAndAttributes for BatchGetItem based on the provided
    ///     Table.BatchGetItemRequest.
    ///     It processes each item in the request and adds a key-value pair to the result dictionary.
    ///     The key is the table name, and the value is a KeysAndAttributes object containing parameters such as
    ///     ConsistentRead,
    ///     ExpressionAttributeNames, ProjectionExpression, and Keys.
    ///     The Keys are derived by converting the provided attribute values to AttributeValue objects.
    /// </remarks>
    /// <seealso cref="Table.BatchGetItemRequest" />
    internal static Dictionary<string, KeysAndAttributes> CreateBatchGetItemRequest(BatchGetItemRequest request,
        DynamoContext context = null)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        var result = new Dictionary<string, KeysAndAttributes>();

        foreach (var item in request.Items)
            result.Add(item.Key, new KeysAndAttributes
            {
                ConsistentRead = item.Value.ConsistentRead,
                ExpressionAttributeNames = item.Value.ExpressionAttributeNames,
                ProjectionExpression = item.Value.ProjectionExpression,
                Keys = item.Value.Keys.Select(k => AttributeConverter.ConvertToAttributeValues(k, context)).ToList()
            });

        return result;
    }

    /// <summary>
    ///     Creates a BatchWriteItemRequest based on the provided Table.BatchWriteItemRequest.
    /// </summary>
    /// <param name="request">The Table.BatchWriteItemRequest object containing batch write item parameters.</param>
    /// <param name="context">The dynamo db context that can contain the entities map.</param>
    /// <returns>A BatchWriteItemRequest object representing the batch write item request.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input request is null.</exception>
    /// <remarks>
    ///     This method creates a BatchWriteItemRequest based on the provided Table.BatchWriteItemRequest.
    ///     It processes each item in the request and constructs a BatchWriteItemRequest object with RequestItems
    ///     containing a dictionary of table names and corresponding lists of WriteRequest objects.
    ///     Each WriteRequest in the list can be a DeleteRequest or PutRequest, depending on the input.
    ///     The AttributeConverter.ConvertToAttributeValues method is used to convert the provided attribute values to
    ///     AttributeValue objects.
    /// </remarks>
    /// <seealso cref="Table.BatchWriteItemRequest" />
    internal static BatchWriteItemRequest CreateBatchWriteItemRequest(Table.BatchWriteItemRequest request,
        DynamoContext context = null)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        var writeRequest = new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, List<WriteRequest>>()
        };

        foreach (var item in request.Items)
        {
            var writeRequests = (from r in item.Value
                select new WriteRequest
                {
                    DeleteRequest = r.DeleteRequest is null
                        ? null
                        : new DeleteRequest(AttributeConverter.ConvertToAttributeValues(r.DeleteRequest, context)),
                    PutRequest = r.PutRequest is null
                        ? null
                        : new PutRequest(AttributeConverter.ConvertToAttributeValues(r.PutRequest, context))
                }).ToList();

            writeRequest.RequestItems.Add(item.Key, writeRequests);
        }

        return writeRequest;
    }

    /// <summary>
    ///     Converts a list of dictionaries of AttributeValue objects into a list of strongly typed objects of type T.
    /// </summary>
    /// <typeparam name="T">The desired type to convert each dictionary to.</typeparam>
    /// <param name="items">A list of dictionaries, where each dictionary represents a set of attribute values for an item.</param>
    /// <param name="context">A dynamo context.</param>
    /// <returns>
    ///     A list of strongly typed objects of type T, where each object is created by mapping the attribute values
    ///     from the corresponding dictionary to the properties of the type T.
    /// </returns>
    /// <remarks>
    ///     This method takes a list of dictionaries, where each dictionary represents a set of attribute values for an item,
    ///     and converts them into a list of strongly typed objects of type T. The conversion is performed by mapping the
    ///     attribute
    ///     values from each dictionary to the properties of the type T. The AttributeConverter.ConvertAttributesToType method
    ///     is used to perform the conversion for each dictionary.
    ///     If the input list is null, an empty list of type T is returned.
    /// </remarks>
    internal static IList<T> ConvertAttributesToType<T>(IList<Dictionary<string, AttributeValue>> items,
        DynamoContext context = null) where T : class, new()
    {
        return items is null
            ? []
            : items.Select(i => AttributeConverter.ConvertAttributesToType<T>(i, context)).ToList();
    }

    /// <summary>
    ///     Converts a list of dictionaries of AttributeValue objects into two lists of strongly typed objects, one for each
    ///     specified type (T1 and T2).
    /// </summary>
    /// <typeparam name="T1">The desired type for the first list.</typeparam>
    /// <typeparam name="T2">The desired type for the second list.</typeparam>
    /// <param name="items">A list of dictionaries, where each dictionary represents a set of attribute values for an item.</param>
    /// <param name="splitBy">A string value used to split the items into two lists based on a specific attribute value.</param>
    /// <param name="context">Context if exists</param>
    /// <returns>
    ///     A tuple containing two lists of strongly typed objects, where the first list (T1) includes objects created by
    ///     mapping the attribute values
    ///     of items that have the specified "EntityType" attribute matching the "splitBy" value to the properties of type T1,
    ///     and the second list (T2)
    ///     includes objects created by mapping the attribute values of other items to the properties of type T2.
    /// </returns>
    /// <remarks>
    ///     This method takes a list of dictionaries, where each dictionary represents a set of attribute values for an item,
    ///     and converts them into two separate lists of strongly typed objects. The conversion is based on the presence of the
    ///     "EntityType"
    ///     attribute in each dictionary and its value matching the "splitBy" parameter.
    ///     Items with the specified "EntityType" attribute value matching "splitBy" will be converted to objects of type T1
    ///     and included in the first list.
    ///     Other items will be converted to objects of type T2 and included in the second list.
    ///     If the input list is null, the method returns a tuple with two null lists.
    /// </remarks>
    internal static (IList<T1> first, IList<T2> seccond) ConvertAttributesToType<T1, T2>(
        IList<Dictionary<string, AttributeValue>> items, string splitBy, DynamoContext context = null)
        where T1 : class, new()
        where T2 : class, new()
    {
        if (items is null)
            return (null, null);

        var result1 = new List<T1>();
        var result2 = new List<T2>();

        foreach (var item in items)
            if (item.TryGetValue(EntitySplitter, out var value) && value.S == splitBy)
                result1.Add(AttributeConverter.ConvertAttributesToType<T1>(item, context));
            else
                result2.Add(AttributeConverter.ConvertAttributesToType<T2>(item, context));

        return (result1, result2);
    }

    /// <summary>
    ///     Converts a list of dictionaries of AttributeValue objects into three lists of strongly typed objects, one for each
    ///     specified type (T1, T2, and T3).
    /// </summary>
    /// <typeparam name="T1">The desired type for the first list.</typeparam>
    /// <typeparam name="T2">The desired type for the second list.</typeparam>
    /// <typeparam name="T3">The desired type for the third list.</typeparam>
    /// <param name="items">A list of dictionaries, where each dictionary represents a set of attribute values for an item.</param>
    /// <param name="splitBy">
    ///     An array of string values used to split the items into three lists based on a specific attribute
    ///     value.
    /// </param>
    /// <param name="context">Context if exists</param>
    /// <returns>
    ///     A tuple containing three lists of strongly typed objects, where the first list (T1) includes objects created by
    ///     mapping the attribute values
    ///     of items that have the specified "EntityType" attribute matching the first value in the "splitBy" array to the
    ///     properties of type T1,
    ///     the second list (T2) includes objects created by mapping the attribute values of items that have the specified
    ///     "EntityType" attribute
    ///     matching the second value in the "splitBy" array to the properties of type T2, and the third list (T3) includes
    ///     objects created by mapping
    ///     the attribute values of other items to the properties of type T3.
    /// </returns>
    /// <remarks>
    ///     This method takes a list of dictionaries, where each dictionary represents a set of attribute values for an item,
    ///     and converts them into three separate lists of strongly typed objects. The conversion is based on the presence of
    ///     the "EntityType"
    ///     attribute in each dictionary and its value matching one of the values in the "splitBy" array.
    ///     Items with the specified "EntityType" attribute value matching the first value in "splitBy" will be converted to
    ///     objects of type T1
    ///     and included in the first list. Items matching the second value in "splitBy" will be converted to objects of type
    ///     T2 and included in
    ///     the second list. Other items will be converted to objects of type T3 and included in the third list.
    ///     If the input list is null, the method returns a tuple with three null lists.
    /// </remarks>
    internal static (List<T1> first, IList<T2> seccond, IList<T3> third) ConvertAttributesToType<T1, T2, T3>(
        IList<Dictionary<string, AttributeValue>> items, string[] splitBy, DynamoContext context = null)
        where T1 : class, new()
        where T2 : class, new()
        where T3 : class, new()
    {
        if (items is null)
            return (null, null, null);

        var result1 = new List<T1>();
        var result2 = new List<T2>();
        var result3 = new List<T3>();

        foreach (var item in items)
        {
            if (!item.ContainsKey(EntitySplitter))
                continue;

            if (item[EntitySplitter].S == splitBy[0])
            {
                result1.Add(AttributeConverter.ConvertAttributesToType<T1>(item, context));
            }
            else
            {
                if (item[EntitySplitter].S == splitBy[1])
                    result2.Add(AttributeConverter.ConvertAttributesToType<T2>(item, context));
                else
                    result3.Add(AttributeConverter.ConvertAttributesToType<T3>(item, context));
            }
        }

        return (result1, result2, result3);
    }

    /// <summary>
    ///     Converts a list of dictionaries of AttributeValue objects into four lists of strongly typed objects, one for each
    ///     specified type (T1, T2, T3, and T4).
    /// </summary>
    /// <typeparam name="T1">The desired type for the first list.</typeparam>
    /// <typeparam name="T2">The desired type for the second list.</typeparam>
    /// <typeparam name="T3">The desired type for the third list.</typeparam>
    /// <typeparam name="T4">The desired type for the fourth list.</typeparam>
    /// <param name="items">A list of dictionaries, where each dictionary represents a set of attribute values for an item.</param>
    /// <param name="splitBy">
    ///     An array of string values used to split the items into four lists based on a specific attribute
    ///     value.
    /// </param>
    /// <param name="context">Context if exists.</param>
    /// <returns>
    ///     A tuple containing four lists of strongly typed objects, where the first list (T1) includes objects created by
    ///     mapping the attribute values
    ///     of items that have the specified "EntityType" attribute matching the first value in the "splitBy" array to the
    ///     properties of type T1,
    ///     the second list (T2) includes objects created by mapping the attribute values of items that have the specified
    ///     "EntityType" attribute
    ///     matching the second value in the "splitBy" array to the properties of type T2, the third list (T3) includes objects
    ///     created by mapping
    ///     the attribute values of items that have the specified "EntityType" attribute matching the third value in the
    ///     "splitBy" array to the properties of type T3,
    ///     and the fourth list (T4) includes objects created by mapping the attribute values of other items to the properties
    ///     of type T4.
    /// </returns>
    /// <remarks>
    ///     This method takes a list of dictionaries, where each dictionary represents a set of attribute values for an item,
    ///     and converts them into four separate lists of strongly typed objects. The conversion is based on the presence of
    ///     the "EntityType"
    ///     attribute in each dictionary and its value matching one of the values in the "splitBy" array.
    ///     Items with the specified "EntityType" attribute value matching the first value in "splitBy" will be converted to
    ///     objects of type T1
    ///     and included in the first list. Items matching the second value in "splitBy" will be converted to objects of type
    ///     T2 and included in
    ///     the second list. Items matching the third value in "splitBy" will be converted to objects of type T3 and included
    ///     in the third list.
    ///     Other items will be converted to objects of type T4 and included in the fourth list.
    ///     If the input list is null, the method returns a tuple with four null lists.
    /// </remarks>
    internal static (IList<T1> first, IList<T2> seccond, IList<T3> third, IList<T4> fourth) ConvertAttributesToType<T1,
        T2, T3, T4>(
        IList<Dictionary<string, AttributeValue>> items, string[] splitBy, DynamoContext context = null)
        where T1 : class, new()
        where T2 : class, new()
        where T3 : class, new()
        where T4 : class, new()

    {
        if (items is null)
            return (null, null, null, null);

        var result1 = new List<T1>();
        var result2 = new List<T2>();
        var result3 = new List<T3>();
        var result4 = new List<T4>();

        foreach (var item in items)
        {
            if (!item.ContainsKey(EntitySplitter))
                continue;

            if (item[EntitySplitter].S == splitBy[0])
            {
                result1.Add(AttributeConverter.ConvertAttributesToType<T1>(item, context));
            }
            else
            {
                if (item[EntitySplitter].S == splitBy[1])
                {
                    result2.Add(AttributeConverter.ConvertAttributesToType<T2>(item, context));
                }
                else
                {
                    if (item[EntitySplitter].S == splitBy[2])
                        result3.Add(AttributeConverter.ConvertAttributesToType<T3>(item, context));
                    else
                        result4.Add(AttributeConverter.ConvertAttributesToType<T4>(item, context));
                }
            }
        }

        return (result1, result2, result3, result4);
    }

    /// <summary>
    ///     Converts a list of dictionaries containing attribute values into five separate lists of specified types, based on
    ///     the "EntityType" attribute value.
    /// </summary>
    /// <typeparam name="T1">Type for the first entity.</typeparam>
    /// <typeparam name="T2">Type for the second entity.</typeparam>
    /// <typeparam name="T3">Type for the third entity.</typeparam>
    /// <typeparam name="T4">Type for the fourth entity.</typeparam>
    /// <typeparam name="T5">Type for the fifth entity.</typeparam>
    /// <param name="items">List of dictionaries containing attribute values.</param>
    /// <param name="splitBy">An array of strings used to split the entities based on the "EntityType" attribute value.</param>
    /// <param name="context">Context if exists.</param>
    /// <returns>A tuple containing five lists of specified entity types.</returns>
    internal static (IList<T1> first, IList<T2> seccond, IList<T3> third, IList<T4> fourth, IList<T5> fifth)
        ConvertAttributesToType<T1, T2, T3, T4, T5>(
            IList<Dictionary<string, AttributeValue>> items, string[] splitBy, DynamoContext context = null)
        where T1 : class, new()
        where T2 : class, new()
        where T3 : class, new()
        where T4 : class, new()
        where T5 : class, new()

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
            if (!item.ContainsKey(EntitySplitter))
                continue;

            if (item[EntitySplitter].S == splitBy[0])
            {
                result1.Add(AttributeConverter.ConvertAttributesToType<T1>(item, context));
            }
            else
            {
                if (item[EntitySplitter].S == splitBy[1])
                {
                    result2.Add(AttributeConverter.ConvertAttributesToType<T2>(item, context));
                }
                else
                {
                    if (item[EntitySplitter].S == splitBy[2])
                    {
                        result3.Add(AttributeConverter.ConvertAttributesToType<T3>(item, context));
                    }
                    else
                    {
                        if (item[EntitySplitter].S == splitBy[3])
                            result4.Add(AttributeConverter.ConvertAttributesToType<T4>(item, context));
                        else
                            result5.Add(AttributeConverter.ConvertAttributesToType<T5>(item, context));
                    }
                }
            }
        }

        return (result1, result2, result3, result4, result5);
    }

    /// <summary>
    ///     Creates a pagination token from a dictionary of attribute values.
    /// </summary>
    /// <param name="lastEvaluatedKey">Dictionary of attribute values representing the last evaluated key.</param>
    /// <returns>The pagination token as a string.</returns>
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

    /// <summary>
    ///     Converts a pagination token string into a dictionary of attribute values.
    /// </summary>
    /// <param name="paginationToken">Pagination token string to convert.</param>
    /// <returns>A dictionary of attribute values representing the pagination token.</returns>
    private static Dictionary<string, AttributeValue> PaginationTokenToDictionary(string paginationToken)
    {
        var result = new Dictionary<string, AttributeValue>();

        if (paginationToken is null)
            return result;

        var decryptedToken = Convert.FromBase64String(paginationToken.UrlDecode()).Unzip();

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

    /// <summary>
    ///     Creates a Put operation for a transactional write item.
    /// </summary>
    /// <param name="transactionWriteItem">Transactional write item information.</param>
    /// <returns>A Put operation if the operation type is Put; otherwise, null.</returns>
    private static Put CreatePutTransactItem(TransactionWriteItem transactionWriteItem)
    {
        if (transactionWriteItem.OperationType != TransactionWriteOperationType.Put)
            return null;

        return new Put
        {
            ConditionExpression = transactionWriteItem.ConditionExpression,
            TableName = transactionWriteItem.TableName,
            ExpressionAttributeValues =
                AttributeConverter.ConvertToAttributeValues(transactionWriteItem.ExpressionAttributeValues),
            Item = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.Items)
        };
    }

    /// <summary>
    ///     Creates a ConditionCheck operation for a transactional write item.
    /// </summary>
    /// <param name="transactionWriteItem">Transactional write item information.</param>
    /// <returns>A ConditionCheck operation if the operation type is Check; otherwise, null.</returns>
    private static ConditionCheck CreateConditionCheckTransactItem(TransactionWriteItem transactionWriteItem)
    {
        if (transactionWriteItem.OperationType != TransactionWriteOperationType.Check)
            return null;

        return new ConditionCheck
        {
            ConditionExpression = transactionWriteItem.ConditionExpression,
            TableName = transactionWriteItem.TableName,
            ExpressionAttributeValues =
                AttributeConverter.ConvertToAttributeValues(transactionWriteItem.ExpressionAttributeValues),
            Key = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.Keys)
        };
    }

    /// <summary>
    ///     Creates a Delete operation for a transactional write item.
    /// </summary>
    /// <param name="transactionWriteItem">Transactional write item information.</param>
    /// <returns>A Delete operation if the operation type is Delete; otherwise, null.</returns>
    private static Delete CreateDeleteTransactItem(TransactionWriteItem transactionWriteItem)
    {
        if (transactionWriteItem.OperationType != TransactionWriteOperationType.Delete)
            return null;

        return new Delete
        {
            ConditionExpression = transactionWriteItem.ConditionExpression,
            TableName = transactionWriteItem.TableName,
            ExpressionAttributeValues =
                AttributeConverter.ConvertToAttributeValues(transactionWriteItem.ExpressionAttributeValues),
            Key = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.Keys)
        };
    }

    /// <summary>
    ///     Creates an Update operation for a transactional write item.
    /// </summary>
    /// <param name="transactionWriteItem">Transactional write item information.</param>
    /// <returns>An Update operation if the operation type is Update; otherwise, null.</returns>
    private static Update CreateUpdateTransactItem(TransactionWriteItem transactionWriteItem)
    {
        if (transactionWriteItem.OperationType != TransactionWriteOperationType.Update)
            return null;

        return new Update
        {
            ConditionExpression = transactionWriteItem.ConditionExpression,
            TableName = transactionWriteItem.TableName,
            UpdateExpression = transactionWriteItem.UpdateExpression,
            ExpressionAttributeValues =
                AttributeConverter.ConvertToAttributeValues(transactionWriteItem.ExpressionAttributeValues),
            Key = AttributeConverter.ConvertToAttributeValues(transactionWriteItem.Keys)
        };
    }

    /// <summary>
    ///     Creates a TransactWriteItem based on the transactional write item information.
    /// </summary>
    /// <param name="transactionWriteItem">Transactional write item information.</param>
    /// <returns>A TransactWriteItem based on the provided transaction write item.</returns>
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
}