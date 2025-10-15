// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Dynamo.Converters.Attributes;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Helpers;

/// <summary>
///     A utility class providing helper methods for various tasks.
/// </summary>
internal static class TableHelper
{
    /// <summary>
    ///     Retrieves the DynamoDB table name associated with the specified type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type for which to retrieve the DynamoDB table name.</typeparam>
    /// <returns>
    ///     The DynamoDB table name associated with the type <typeparamref name="T" />.
    ///     If no table name attribute is defined, it returns the name of the type <typeparamref name="T" />.
    /// </returns>
    internal static string GetTableName<T>(DynamoContext context = null) where T : class
    {
        if (context != null && context.HasTypeBuilder<T>()) return context.GetEntityBuilder<T>().TableName;

        return Attribute.GetCustomAttribute(typeof(T), typeof(DynamoDBTableAttribute)) is not DynamoDBTableAttribute
            attribute
            ? typeof(T).Name
            : attribute.TableName;
    }

    /// <summary>
    ///     Get the hash key name for the specified type <typeparamref name="T" />.
    /// </summary>
    /// <param name="context">
    ///     If the context is not null, it will get from the context otherwise from the
    ///     DynamoDBHashKeyAttribute
    /// </param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static string GetHashKeyName<T>(DynamoContext context = null) where T : class
    {
        if (context != null && context.HasTypeBuilder<T>()) return context.GetEntityBuilder<T>().Pk;

        var hashKey =
            typeof(T).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<DynamoDBHashKeyAttribute>() != null)?
                .GetCustomAttribute<DynamoDBHashKeyAttribute>()?
                .AttributeName ?? "PK";

        return hashKey;
    }

    private static object GetPropertyValue<T>(T instance, string propertyName) where T : class
    {
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(propertyName);

        var property = typeof(T).GetProperty(propertyName);

        if (property != null)
        {
            return property.GetValue(instance);
        }

        //Check if  property has a DynamoDBAttribute
        var dynamoDbProperties = typeof(T).GetProperties().Where(p =>
            p.GetCustomAttribute<DynamoDBPropertyAttribute>() != null).ToList();

        if (dynamoDbProperties.Count == 0)
            return null;

        var originalProperty = dynamoDbProperties.FirstOrDefault(p =>
            p.GetCustomAttribute<DynamoDBPropertyAttribute>()?.AttributeName == propertyName);

        return originalProperty?.GetValue(instance);
    }

    /// <summary>
    ///     Get the range key name for the specified type <typeparamref name="T" />.
    /// </summary>
    /// <param name="context">
    ///     If the context is not null, it will get from the context otherwise from the
    ///     DynamoDBRangeKeyAttribute
    /// </param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static string GetRangeKeyName<T>(DynamoContext context = null) where T : class
    {
        if (context != null && context.HasTypeBuilder<T>())
            return context.GetEntityBuilder<T>().Sk;

        var rangeKey =
            typeof(T).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<DynamoDBRangeKeyAttribute>() != null)?
                .GetCustomAttribute<DynamoDBRangeKeyAttribute>()?
                .AttributeName;

        return rangeKey;
    }

    /// <summary>
    ///     The default key expression is a pk and sk if the range key is not null. It returns a PkName=:pk AND SkName=:sk
    /// </summary>
    /// <param name="hasRangeKeyValue"></param>
    /// <param name="context">The context can be null and the system will get the DynamoDb Default Attributes</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>A string with the default key expression.</returns>
    internal static string BuildDefaultKeyExpression<T>(bool hasRangeKeyValue = false, DynamoContext context = null)
        where T : class
    {
        var hashKeyName = GetHashKeyName<T>(context);
        var rangeKeyName = GetRangeKeyName<T>(context);

        return hasRangeKeyValue ? $"{hashKeyName}=:pk AND {rangeKeyName}=:sk" : $"{hashKeyName}=:pk";
    }

    /// <summary>
    ///     This method gets the value of a mapped key from the specified type but not the name of the key or hash key.
    /// </summary>
    /// <param name="value">The instance.</param>
    /// <param name="context">The available context where the system will get the mapping for the instance entity.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The hash and range key values</returns>
    internal static (object HashKey, object RangeKey) GetKeyValues<T>(T value, DynamoContext context = null)
        where T : class
    {
        Check.NotNull(value, nameof(value));

        var hashKeyName = GetHashKeyName<T>(context);
        var rangeKeyName = GetRangeKeyName<T>(context);

        var hashKeyValue = GetPropertyValue(value, hashKeyName);
        var rangeKeyValue = GetPropertyValue(value, rangeKeyName);

        if (context is null || !context.HasTypeBuilder<T>())
        {
            return (hashKeyValue, rangeKeyValue);
        }

        //It will build the value from the delegate using the type builder

        if (context.GetEntityBuilder<T>() is not EntityTypeBuilder<T> entityBuilder)
            return (hashKeyValue, rangeKeyValue);

        //invoke the map action to get the updated value
        hashKeyValue = entityBuilder.GetProperty(hashKeyName)?.InvokeMaps(value).GetDefaultValue(value);
        rangeKeyValue = entityBuilder.GetProperty(rangeKeyName)?.InvokeMaps(value).GetDefaultValue(value);

        return (hashKeyValue, rangeKeyValue);
    }

    /// <summary>
    ///     Build the hash and range keys from the specified instance.
    /// </summary>
    /// <param name="id">The entity hash key value.</param>
    /// <param name="rangeKeyValue">The entity range key value</param>
    /// <param name="context">The current context if available</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static ((string Name, AttributeValue Value) HashKey, (string Name, AttributeValue Value)? RangeKey)
        BuildKeysAttributeValues<T>(object id, object rangeKeyValue = null, DynamoContext context = null)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(id);

        var entityBuilder = context?.GetEntityBuilder<T>();
        var hashKeyPrefix = entityBuilder?.HashKeyPrefix;
        var keySeparator = entityBuilder?.KeySeparator;

        hashKeyPrefix = hashKeyPrefix.IsNotNullOrEmpty() ? $"{hashKeyPrefix}{keySeparator}" : string.Empty;

        //To avoid situations where the id is already prefixed
        if (id.ToStringOrDefault().Contains(hashKeyPrefix, StringComparison.CurrentCultureIgnoreCase))
            hashKeyPrefix = string.Empty;

        //Get the name of the hash key
        var hashKey = (GetHashKeyName<T>(context), new AttributeValue($"{hashKeyPrefix}{id}"));

        if (rangeKeyValue == null)
            return (hashKey, null);

        var rangeKeyName = GetRangeKeyName<T>(context);
        var rangeKeyPrefix = entityBuilder?.RangeKeyPrefix;

        rangeKeyPrefix = rangeKeyPrefix.IsNotNullOrEmpty() ? $"{rangeKeyPrefix}{keySeparator}" : string.Empty;

        if (rangeKeyValue.ToStringOrDefault().Contains(rangeKeyPrefix, StringComparison.CurrentCultureIgnoreCase))
            rangeKeyPrefix = string.Empty;

        var rangeKey = (rangeKeyName, new AttributeValue($"{rangeKeyPrefix}{rangeKeyValue}"));

        return (hashKey, rangeKey);
    }

    /// <summary>
    ///     Extract the hash keys and range keys from the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="context"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal static Dictionary<string, AttributeValue> ExtractKeyAttributeValueMap<T>(T value,
        DynamoContext context = null)
        where T : class
    {
        Check.NotNull(value, nameof(value));

        var keyValues = GetKeyValues(value, context);

        return ParseKeysToAttributeValueMap<T>(keyValues.HashKey, keyValues.RangeKey, context);
    }

    /// <summary>
    ///     Extract the hash keys and range keys from the specified value.
    /// </summary>
    /// <param name="id">The id provided for the entity.</param>
    /// <param name="rangeKeyValue">The range key provided.</param>
    /// <param name="context">The context where the system will get information to build the key.</param>
    /// <typeparam name="T">A T type that is mapped to the context.</typeparam>
    /// <returns>The list of key using dynamo attribute value</returns>
    internal static Dictionary<string, AttributeValue> ParseKeysToAttributeValueMap<T>(object id,
        object rangeKeyValue = null, DynamoContext context = null) where T : class
    {
        ArgumentNullException.ThrowIfNull(id);

        //Get the name of the PK and Sk converting to attribute values
        var tupleKeys = BuildKeysAttributeValues<T>(id, rangeKeyValue, context);

        var keys = new Dictionary<string, AttributeValue>
        {
            { tupleKeys.HashKey.Name, tupleKeys.HashKey.Value }
        };

        if (tupleKeys.RangeKey.HasValue)
            keys.Add(tupleKeys.RangeKey.Value.Name, tupleKeys.RangeKey.Value.Value);

        return keys;
    }

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
}