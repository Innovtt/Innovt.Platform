using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Dynamo.Converters.Attributes.Exceptions;
using Innovt.Cloud.AWS.Dynamo.Helpers;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Converters.Attributes;

internal static class AttributeConverter
{
    private static readonly ConcurrentDictionary<Type, Lazy<PropertyInfo[]>> PropertiesCache = new();

    private static readonly ConcurrentDictionary<(Type type, string propertyName), Lazy<PropertyInfo>>
        PropertyLookupCache = new();

    private static readonly ConcurrentDictionary<PropertyInfo, Lazy<DynamoDBPropertyAttribute>> AttributeCache = new();

    static AttributeConverter()
    {
        TypeUtil.AddPrimitiveType(typeof(Primitive));
        TypeUtil.AddPrimitiveType(typeof(TimeOnly));
        TypeUtil.AddPrimitiveType(typeof(TimeOnly?));
    }

    internal static Dictionary<string, AttributeValue>? ConvertToAttributeValues(Dictionary<string, object> items,
        DynamoContext? context = null)
    {
        if (context is null)
            return items?.Select(i =>
                new
                {
                    i.Key,
                    Value = CreateAttributeValue(i.Value)
                }).ToDictionary(x => x.Key, x => x.Value);

        return items?.Select(i =>
            new
            {
                i.Key,
                Value = ConvertToAttributeValueMap(i.Value, context)
            }).ToDictionary(x => x.Key, x => new AttributeValue { M = x.Value });
    }

    internal static AttributeValue CreateAttributeValue(object? value)
    {
        return AttributeValueConverterManager.CreateAttributeValue(value);
    }

    private static PropertyInfo? GetProperty(PropertyInfo[] properties, string propertyName, Type declaringType)
    {
        var cacheKey = (declaringType, propertyName);

        return PropertyLookupCache.GetOrAdd(cacheKey, key => new Lazy<PropertyInfo>(() =>
        {
            var instanceProps = properties.Where(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (instanceProps.Count == 0)
            {
                instanceProps = properties.Where(p =>
                {
                    var attr = AttributeCache.GetOrAdd(p,
                            prop => new Lazy<DynamoDBPropertyAttribute>(prop
                                .GetCustomAttribute<DynamoDBPropertyAttribute>))
                        .Value;

                    return attr != null && propertyName.Equals(attr.AttributeName, StringComparison.OrdinalIgnoreCase);
                }).ToList();
            }

            if (instanceProps.Count == 0)
                return null;

            var prop = instanceProps.Find(p => p.DeclaringType == declaringType) ?? instanceProps[0];

            return prop?.CanWrite == true ? prop : null;
        })).Value;
    }

    private static DynamoDBEntry ConvertAttributeValue(AttributeValue? attributeValue)
    {
        if (attributeValue is null)
            return new DynamoDBNull();

        if (attributeValue.IsBOOLSet)
            return new DynamoDBBool(attributeValue.BOOL.GetValueOrDefault());

        if (attributeValue.B is not null)
            return new Primitive(attributeValue.B);

        return attributeValue.N is not null ? new Primitive(attributeValue.N, true) : new Primitive(attributeValue.S);
    }

    private static object? ConvertNonPrimitiveType(PropertyInfo property, AttributeValue attributeValue, object? value,
        IPropertyConverter? propertyConverter = null)
    {
        if (value is null)
            return null;

        if (property.PropertyType.IsEnum)
            return Enum.Parse(property.PropertyType, value.ToString()!, true);

        var customConverter = propertyConverter ?? AttributeCache.GetOrAdd(property, prop =>
            new Lazy<DynamoDBPropertyAttribute>(() =>
                prop.GetCustomAttributes<DynamoDBPropertyAttribute>()
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    .FirstOrDefault(a => a.Converter != null))).Value?.Converter as IPropertyConverter;

        if (customConverter is null)
            return value;

        var convertedEntry = ConvertAttributeValue(attributeValue);

        return customConverter.FromEntry(convertedEntry);
    }

    public static T? ConvertAttributeValuesToType<T>(Dictionary<string, AttributeValue>? items,
        DynamoContext? context = null)
        where T : class
    {
        if (items is null) return null;

        var instance = InstanceCreator.CreateInstance<T>(items, context);

        var properties = PropertiesCache.GetOrAdd(instance.GetType(), type =>
            new Lazy<PropertyInfo[]>(() =>
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty))).Value;

        if (properties.Length == 0)
            return instance;

        var typeBuilder = context?.HasTypeBuilder<T>() == true ? context.GetEntityBuilder<T>() : null;

        foreach (var attributeValue in items)
        {
            try
            {
                var propertyKey = attributeValue.Key;
                var newPropertyKey = typeBuilder?.GetProperty(propertyKey)?.Name;

                if (newPropertyKey != null)
                    propertyKey = newPropertyKey;

                var property = GetProperty(properties, propertyKey, instance.GetType());

                if (property is null)
                    continue;

                var convertedValue = ConvertAttributeValueToObject(attributeValue.Value, property, context);
                property.SetValue(instance, convertedValue, null);
            }
            catch (Exception ex)
            {
                throw new CriticalException(
                    $"Error parsing data from database to object. Property {attributeValue.Key}", ex);
            }
        }

        PropertyMapper.InvokeMappedProperties(typeBuilder, properties, instance);

        return instance;
    }

    private static object ConvertAttributeValueToObject(AttributeValue attributeValue, PropertyInfo property,
        DynamoContext? context = null)
    {
        var value = AttributeValueToObjectConverterManager.CreateAttributeValueToObject(attributeValue,
            property.PropertyType, context);

        if (TypeUtil.IsPrimitive(property.PropertyType))
            return TypeConverter.ConvertType(property.PropertyType, value);

        if (TypeUtil.IsCollection(property.PropertyType))
            return TypeUtil.IsDictionary(property.PropertyType)
                ? value
                : CollectionConverter.ItemsToCollection(property.PropertyType, (IEnumerable<object>)value);

        return ConvertNonPrimitiveType(property, attributeValue, value,
            context?.GetPropertyConverter(property.PropertyType));
    }

    internal static Dictionary<string, AttributeValue> ConvertToAttributeValueMap<T>(T instance,
        DynamoContext? context = null)
        where T : class
    {
        Check.NotNull(instance, nameof(instance));

        var properties = PropertiesCache.GetOrAdd(instance.GetType(), type =>
            new Lazy<PropertyInfo[]>(() =>
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty))).Value;

        if (properties.Length == 0)
            return new Dictionary<string, AttributeValue>();

        var attributes = new Dictionary<string, AttributeValue>();

        var typeBuilder = context?.HasTypeBuilder<T>() == true ? context.GetEntityBuilder<T>() : null;

        if (typeBuilder is null)
        {
            ConvertToAttributeValueMapWithoutContext(instance, properties, attributes);
        }
        else
        {
            ConvertToAttributeValueMapWithContext(instance, context, properties, typeBuilder, attributes);
        }

        return attributes;
    }

    private static void ConvertToAttributeValueMapWithContext<T>(T instance, DynamoContext? context,
        PropertyInfo[] properties, EntityTypeBuilder typeBuilder, Dictionary<string, AttributeValue> attributes)
        where T : class
    {
        PropertyMapper.InvokeMappedProperties(typeBuilder, properties, instance);

        var mappedProperties = typeBuilder.GetProperties().ToList();

        if (typeBuilder.Discriminator is not null)
            mappedProperties.AddRange(
                DiscriminatorManager.GetDiscriminatorProperties(context, typeBuilder, properties, instance));

        foreach (var mappedProperty in mappedProperties)
        {
            try
            {
                var propertyKey = mappedProperty.Name;
                var property = Array.Find(properties, p => p.Name == propertyKey);

                var propertyType = property?.PropertyType;
                var propertyValue = property?.GetValue(instance);

                if (property is null)
                {
                    propertyValue = mappedProperty.GetDefaultValue(instance);
                    propertyType = mappedProperty.Type;
                }

                if ((context.IgnoreNullValues && propertyValue is null) || propertyType is null)
                    continue;

                var converter = context.GetPropertyConverter(propertyType);

                if (converter is not null)
                    propertyValue = converter.ToEntry(propertyValue).ToString();

                attributes.Add(mappedProperty.ColumnName, CreateAttributeValue(propertyValue));
            }
            catch (Exception ex)
            {
                throw new ConversionException(
                    $"Error parsing entity to Dynamo properties. Property {mappedProperty.Name}", ex);
            }
        }
    }

    /// <summary>
    /// No context mapping for properties
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="properties"></param>
    /// <param name="attributes"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="ConversionException"></exception>
    private static void ConvertToAttributeValueMapWithoutContext<T>(T instance, PropertyInfo[] properties,
        Dictionary<string, AttributeValue> attributes)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(instance);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentNullException.ThrowIfNull(attributes);

        foreach (var property in properties)
        {
            try
            {
                var propertyKey = property.Name;

                var hasIgnoreAttribute = property.GetCustomAttribute<DynamoDBIgnoreAttribute>() != null;

                if (hasIgnoreAttribute)
                    continue;

                var dynamoAttribute = property.GetCustomAttribute<DynamoDBPropertyAttribute>();

                if (dynamoAttribute != null)
                    propertyKey = dynamoAttribute.AttributeName;

                attributes.Add(propertyKey, CreateAttributeValue(property.GetValue(instance)));
            }
            catch (Exception ex)
            {
                throw new ConversionException(
                    $"Error parsing entity to Dynamo properties. Property {property.Name}", ex);
            }
        }
    }


    public static void ClearCaches()
    {
        PropertiesCache.Clear();
        PropertyLookupCache.Clear();
        AttributeCache.Clear();
    }
}