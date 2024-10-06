using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Dynamo.Helpers;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Converters;

public static class AttributeValueConverterManager
{
    #region [Converters]
    /// <summary>
    ///     This dictionary strategy will have a complexity of O(1) for the lookup and O(1) for the invocation.
    /// </summary>
    private static readonly Dictionary<Type, Func<object, AttributeValue>> Converters = new()
    {
        { typeof(MemoryStream), value => new AttributeValue { B = (MemoryStream)value } },
        { typeof(byte[]), value => new AttributeValue { B = new MemoryStream((byte[])value) } },
        { typeof(bool), value => new AttributeValue { BOOL = (bool)value } },
        { typeof(List<MemoryStream>), value => new AttributeValue { BS = (List<MemoryStream>)value } },
        {
            typeof(List<byte[]>),
            value => new AttributeValue { BS = ((List<byte[]>)value).Select(b => new MemoryStream(b)).ToList() }
        },
        { typeof(List<string>), value => new AttributeValue { SS = (List<string>)value } },
        { typeof(HashSet<string>), value => new AttributeValue { SS = ((HashSet<string>)value).ToList() } },
        { typeof(int), value => new AttributeValue { N = Convert.ToString(value, CultureInfo.InvariantCulture) } },
        { typeof(double), value => new AttributeValue { N = Convert.ToString(value, CultureInfo.InvariantCulture) } },
        { typeof(float), value => new AttributeValue { N = Convert.ToString(value, CultureInfo.InvariantCulture) } },
        { typeof(decimal), value => new AttributeValue { N = Convert.ToString(value, CultureInfo.InvariantCulture) } },
        { typeof(long), value => new AttributeValue { N = Convert.ToString(value, CultureInfo.InvariantCulture) } },
        {
            typeof(HashSet<int>),
            value => new AttributeValue { NS = ((HashSet<int>)value).Select(i => i.ToString()).ToList() }
        },
        {
            typeof(DateTime),
            value => new AttributeValue { S = ((DateTime)value).ToString("s", CultureInfo.InvariantCulture) }
        },
        { typeof(Guid), value => new AttributeValue { S = value.ToString() } },
        { typeof(TimeSpan), value => new AttributeValue { S = value.ToString() } }
    };

    #endregion

    /// <summary>
    ///     Creates a dynamo db attribute value from an object.
    /// </summary>
    /// <param name="value">An object.</param>
    /// <param name="visitedObjects">This is a hash set to control circular reference.</param>
    /// <returns>A dynamo db attribute value.</returns>
    public static AttributeValue CreateAttributeValue(object value, HashSet<object> visitedObjects = null )
    {
        if (value == null) return new AttributeValue { NULL = true };

        // Initialize the visited objects set if it's not provided
        visitedObjects ??= new HashSet<object>( new ReferenceEqualityComparer());

        if(value is Primitive)
            return new AttributeValue { S = value.ToString() };
        
        // Check for circular references. Add the value if it's not already in the set
        if (!visitedObjects.Add(value))
        {
            return new AttributeValue { NULL = true };
        }

        var valueType = value.GetType();

        // If we have a direct match, use the converter
        if (Converters.TryGetValue(valueType, out var converter)) return converter(value);

        // Handle numeric lists using a custom check
        if (value is IList list && TypeUtil.IsNumericList(list))
            return new AttributeValue
            {
                NS = list.Cast<object>().Select(o => Convert.ToString(o, CultureInfo.InvariantCulture)).ToList()
            };
        
        if (value is IDictionary<string, object> dict)
            return new AttributeValue
            {
                M = dict.ToDictionary(item => item.Key, item => CreateAttributeValue(item.Value,visitedObjects))
            };
        
        if (value is IList<object> objectList)
            return new AttributeValue { L = objectList.Select(o=>CreateAttributeValue(o,visitedObjects)).ToList() };

        // Handle complex objects (non-string classes)
        if (valueType.IsClass && valueType != typeof(string))
        {   
            return new AttributeValue
            {
                M = valueType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop => prop.CanRead)
                    .GroupBy(prop => prop.Name)
                    .Select(group => group.First())
                    .ToDictionary(
                        prop => prop.Name,
                        prop => CreateAttributeValue(prop.GetValue(value), visitedObjects)
                    )
            };
        }
        
        return new AttributeValue { S = value.ToString() };
    }
}