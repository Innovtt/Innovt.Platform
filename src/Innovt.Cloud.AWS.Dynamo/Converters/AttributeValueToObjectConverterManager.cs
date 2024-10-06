using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.Model;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Converters;

public static class AttributeValueToObjectConverterManager
{
    private static readonly Dictionary<string, Func<AttributeValue, Type, object>> Converters = new()
    {
        { "BOOL", (value, _) => value.BOOL },
        {
            "L", (value, desiredType) =>
                value.L.Select(l => CreateAttributeValueToObject(l, desiredType.GetGenericArguments()[0])).ToList()
        },
        {
            "M", (value, desiredType) =>
            {
                if (TypeUtil.IsDictionary(desiredType))
                    return ItemsToDictionary(desiredType, value.M);

                var method = typeof(AttributeConverter).GetMethod(nameof(AttributeConverter.ConvertAttributesToType),
                    BindingFlags.Static | BindingFlags.NonPublic, null,
                    new[] { typeof(Dictionary<string, AttributeValue>) }, null);
                return method?.MakeGenericMethod(desiredType).Invoke(null, new object[] { value.M });
            }
        },
        { "BS", (value, _) => value.BS },
        { "N", (value, _) => value.N },
        { "NS", (value, _) => value.NS },
        { "SS", (value, _) => value.SS },
        { "S", (value, _) => value.S }
    };

    // Create a unique key to determine which characteristic of AttributeValue is set
    private static string GetKeyForAttributeValue(AttributeValue value)
    {
        if (value.IsBOOLSet) return "BOOL";
        if (value.IsLSet) return "L";
        if (value.IsMSet) return "M";
        if (value.BS.IsNotNullOrEmpty()) return "BS";
        if (value.N != null) return "N";
        if (value.NS.IsNotNullOrEmpty()) return "NS";
        if (value.SS.IsNotNullOrEmpty()) return "SS";

        return value.S.IsNotNullOrEmpty() ? "S" : null;
    }

    /// <summary>
    ///     This method transforms an AttributeValue into an object of the desired type.
    /// </summary>
    /// <param name="value">The dynamo db attribute value.</param>
    /// <param name="desiredType">Thy desired type to match with the property.</param>
    /// <returns>An object.</returns>
    public static object CreateAttributeValueToObject(AttributeValue value, Type desiredType)
    {
        if (value == null)
            return default;

        var key = GetKeyForAttributeValue(value);
        if (key != null && Converters.TryGetValue(key, out var converter)) return converter(value, desiredType);

        // If no match is found, return default value
        return default;
    }

    private static Dictionary<string, object> ItemsToDictionary(Type desiredType,
        Dictionary<string, AttributeValue> attributeMap)
    {
        return attributeMap.ToDictionary(
            kvp => kvp.Key,
            kvp => CreateAttributeValueToObject(kvp.Value, desiredType.GetGenericArguments()[1]));
    }
}