﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Util.Internal;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo;

internal static class AttributeConverter
{
    private static readonly Type[] primitiveTypesArray = new Type[19]
    {
        typeof(bool),
        typeof(byte),
        typeof(char),
        typeof(DateTime),
        typeof(decimal),
        typeof(double),
        typeof(int),
        typeof(long),
        typeof(sbyte),
        typeof(short),
        typeof(float),
        typeof(string),
        typeof(uint),
        typeof(ulong),
        typeof(ushort),
        typeof(Guid),
        typeof(byte[]),
        typeof(MemoryStream),
        typeof(Primitive)
    };

    private static readonly HashSet<ITypeInfo> PrimitiveTypeInfos =
        new(((IEnumerable<Type>)primitiveTypesArray).Select<Type, ITypeInfo>(
            (Func<Type, ITypeInfo>)TypeFactory.GetTypeInfo));

    public static bool IsPrimitive(Type type)
    {
        var typeWrapper = TypeFactory.GetTypeInfo(type);
        return PrimitiveTypeInfos.Any<ITypeInfo>((Func<ITypeInfo, bool>)(ti => typeWrapper.IsAssignableFrom(ti)));
    }

    public static bool IsCollection(Type type)
    {
        return type.IsArray || (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type));
    }

    public static bool IsDictionary(Type type)
    {
        return type.IsArray || (type.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(type)) ||
               typeof(IDictionary).IsAssignableFrom(type);
    }

    internal static Dictionary<string, AttributeValue> ConvertToAttributeValues(Dictionary<string, object> items)
    {
        return items?.Select(i =>
            new
            {
                i.Key,
                Value = CreateAttributeValue(i.Value)
            }).ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    /// Conversion from object to attribute Value
    /// </summary>
    /// <param name="value">Any object</param>
    /// <returns></returns>
    internal static AttributeValue CreateAttributeValue(object value)
    {
        switch (value)
        {
            case null:
                return new AttributeValue { NULL = true };
            case MemoryStream stream:
                return new AttributeValue { B = stream };
            case bool:
                return new AttributeValue { BOOL = bool.Parse(value.ToString()) };
            case List<MemoryStream> streams:
                return new AttributeValue { BS = streams };
            case List<string> list:
                return new AttributeValue() { SS = list };
            case int or double or float or decimal or long:
                return new AttributeValue { N = value.ToString() };
            case DateTime time:
                return new AttributeValue { S = time.ToString("s") };
            case IList<int> or IList<double> or IList<float> or IList<decimal> or IList<long>:
            {
                var array = (value as IList).Cast<string>();

                return new AttributeValue { NS = array.ToList() };
            }
            case IDictionary<string, object> objects:
            {
                var array = objects.ToDictionary(item => item.Key, item => CreateAttributeValue(item.Value));

                return new AttributeValue { M = array };
            }
            case IList<object> objects:
            {
                return new AttributeValue { L = objects.Select(CreateAttributeValue).ToList() };
            }
            default:
                return new AttributeValue(value.ToString());
        }
    }

    internal static object CreateAttributeValueToObject(AttributeValue value, Type desiredType)
    {
        if (value is null)
            return default;

        if (value.IsBOOLSet)
        {
            return value.BOOL;
        }

        if (value.IsLSet)
        {
            return value.L.Select(l => CreateAttributeValueToObject(l, desiredType.GetGenericArguments()[0])).ToList();
        }

        //Nested Type
        if (value.IsMSet)
        {
            if (IsDictionary(desiredType))
            {
                return ItemsToDictionary(desiredType, value.M);
            }
            else
            {
                var method = typeof(AttributeConverter).GetMethod(nameof(ConvertAttributesToType),
                    BindingFlags.Static | BindingFlags.NonPublic, null,
                    new Type[] { typeof(Dictionary<string, AttributeValue>) }, null);
                return method?.MakeGenericMethod(desiredType).Invoke(null, new object[] { value.M });
            }
        }

        if (value.BS.IsNotNullOrEmpty())
        {
            return value.BS;
        }

        if (value.N is { })
        {
            return value.N;
        }

        if (value.NS.IsNotNullOrEmpty())
        {
            return value.NS;
        }

        if (value.SS.IsNotNullOrEmpty())
        {
            return value.SS;
        }

        return value.S.IsNotNullOrEmpty() ? value.S : default(object);
    }
    
    public static object ItemsToCollection(Type targetType, IEnumerable<object> items)
    {
        return !targetType.IsArray ? ItemsToIList(targetType, items) : ItemsToArray(targetType, items);
    }

    public static object ItemsToDictionary(Type targetType, Dictionary<string, AttributeValue> items)
    {
        if (items is null || targetType is null)
            return null;

        var genericArguments = targetType.GetGenericArguments();

        //not supported
        if (genericArguments.Length != 2)
            return null;

        var dictionary = Activator.CreateInstance(targetType) as IDictionary;

        if (dictionary is null)
            return null;

        var valueType = genericArguments[1];

        foreach (var item in items)
        {
            dictionary.Add(item.Key, CreateAttributeValueToObject(item.Value, valueType));
        }

        return dictionary;
    }

    private static Type GetElementType(Type collectionType)
    {
        var elementType = collectionType.GetElementType();

        if (elementType == (Type)null)
        {
            var genericArguments = TypeFactory.GetTypeInfo(collectionType).GetGenericArguments();
            if (genericArguments is { Length: 1 })
                elementType = genericArguments[0];
        }

        return elementType;
    }
    
    private static object ItemsToArray(Type targetType, IEnumerable<object> items)
    {
        if (items is null)
            return null;

        var list = items.ToList<object>();
        var array = (Array)Activator.CreateInstance(targetType, list.Count);

        var elementType = GetElementType(targetType);

        for (var index = 0; index < list.Count; ++index)
        {
            array.SetValue(IsPrimitive(elementType) ? ConvertType(elementType, list[index]) : list[index], index);
        }

        return (object)array;
    }

    private static object ItemsToIList(Type targetType, IEnumerable<object> items)
    {
        var result = Activator.CreateInstance(targetType);
        var elementType = GetElementType(targetType);

        if (result is IList list)
        {
            foreach (var obj in items)
            {
                list.Add(IsPrimitive(elementType) ? ConvertType(elementType, obj) : obj);
            }

            return result;
        }

        var method = TypeFactory.GetTypeInfo(targetType).GetMethod("Add");

        if (method != (MethodInfo)null)
        {
            foreach (var obj in items)
                method.Invoke(result, new object[1]
                {
                    IsPrimitive(elementType) ? ConvertType(elementType, obj) : obj
                });

            return result;
        }

        return null;
    }

    private static PropertyInfo GetProperty(PropertyInfo[] properties, string propertyName, Type declaringType)
    {
        var instanceProps = properties.Where(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        //Custom attributes
        if (!instanceProps.Any())
            instanceProps = properties.Where(p => p.GetCustomAttribute<DynamoDBPropertyAttribute>() != null &&
                                                  propertyName.Equals(
                                                      p.GetCustomAttribute<DynamoDBPropertyAttribute>().AttributeName,
                                                      StringComparison.OrdinalIgnoreCase)).ToList();

        if (!instanceProps.Any())
            return null;

        var prop = instanceProps.FirstOrDefault(p => p.DeclaringType == declaringType) ?? instanceProps.First();

        if (prop is null || !prop.CanWrite)
        {
            return null;
        }

        return prop;
    }


    private static object ConvertType(Type propertyType, object value)
    {
        if (value is null)
            return default;

        var typeConverter = TypeDescriptor.GetConverter(propertyType);

        if (typeConverter.CanConvertFrom(value.GetType()))
        {
            //workaround compatibility v1 and v2
            if (typeConverter is BooleanConverter && (value.ToString() == "1" || value.ToString() == "0"))
                return typeConverter.ConvertFrom(null!, CultureInfo.InvariantCulture,
                    value.ToString() == "1" ? "true" : "false");

            return typeConverter.ConvertFrom(null!, CultureInfo.InvariantCulture, value);
        }

        var destinationType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        return Convert.ChangeType(value, destinationType, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Convert an attribute array to specific type
    /// </summary>
    /// <typeparam name="T">The desired Type </typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    internal static T ConvertAttributesToType<T>(Dictionary<string, AttributeValue> items)
    {
        if (items is null) return default;

        var instance = Activator.CreateInstance<T>();
        var properties = instance.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

        if (!properties.Any())
            return instance;

        foreach (var attributeValue in items)
        {
            var prop = GetProperty(properties, attributeValue.Key, instance.GetType());

            if (prop is null)
                continue;

            object convertedValue = null;
            var value = CreateAttributeValueToObject(attributeValue.Value, prop.PropertyType);

            if (IsPrimitive(prop.PropertyType))
            {
                convertedValue = ConvertType(prop.PropertyType, value);
            }
            else
            {
                if (IsCollection(prop.PropertyType))
                {
                    convertedValue = IsDictionary(prop.PropertyType)
                        ? value
                        : ItemsToCollection(prop.PropertyType, (IEnumerable<object>)value);
                }
                else
                {
                    convertedValue = prop.PropertyType.IsEnum
                        ? Enum.Parse(prop.PropertyType, value.ToString(), true)
                        : value;
                }
            }

            prop.SetValue(instance, convertedValue, null);
        }

        return instance;
    }
}