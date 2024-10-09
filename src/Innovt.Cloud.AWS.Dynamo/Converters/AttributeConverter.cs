// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Converters;

/// <summary>
///     A utility class for converting between different attribute types.
/// </summary>
internal static class AttributeConverter
{
    /// <summary>
    ///     Add a new type to the list of recognized primitive types.
    /// </summary>
    static AttributeConverter()
    {
        TypeUtil.AddPrimitiveType(typeof(Primitive));
    }

    /// <summary>
    ///     Converts a dictionary of string and object pairs to DynamoDB AttributeValues.
    /// </summary>
    /// <param name="items">The dictionary to convert.</param>
    /// <param name="context"></param>
    /// <returns>A dictionary of string and AttributeValue pairs.</returns>
    internal static Dictionary<string, AttributeValue> ConvertToAttributeValues(Dictionary<string, object> items,
        DynamoContext context = null)
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

    /// <summary>
    ///     Conversion from object to attribute Value.PS: Created by Michel and improved by chatgpt
    /// </summary>
    /// <param name="value">Any object</param>
    /// <returns></returns>
    internal static AttributeValue CreateAttributeValue(object value)
    {
        return AttributeValueConverterManager.CreateAttributeValue(value);
    }

    /// <summary>
    ///     Converts a DynamoDB AttributeValue to an object of the specified desiredType.
    /// </summary>
    /// <param name="value">The DynamoDB AttributeValue to convert.</param>
    /// <param name="desiredType">The desired Type to convert to.</param>
    /// <returns>
    ///     An object of the specified desiredType containing the converted value from the AttributeValue.
    /// </returns>
    private static object CreateAttributeValueToObject(AttributeValue value, Type desiredType)
    {
        return AttributeValueToObjectConverterManager.CreateAttributeValueToObject(value, desiredType);
    }

    /// <summary>
    ///     Converts a collection of items to the specified target type.
    /// </summary>
    /// <param name="targetType">The desired Type to convert the items to.</param>
    /// <param name="items">The collection of items to convert.</param>
    /// <returns>
    ///     An object of the specified target type containing the converted items.
    /// </returns>
    private static object ItemsToCollection(Type targetType, IEnumerable<object> items)
    {
        return !targetType.IsArray ? ItemsToIList(targetType, items) : ItemsToArray(targetType, items);
    }

    /// <summary>
    ///     Retrieves the element of type a collection or array type.
    /// </summary>
    /// <param name="collectionType">The Type representing a collection or array.</param>
    /// <returns>
    ///     The Type of the elements contained within the collection or array, or null if the element type could not be
    ///     determined.
    /// </returns>
    private static Type GetElementType(Type collectionType)
    {
        var elementType = collectionType.GetElementType();

        if (elementType != null) return elementType;

        var genericArguments = collectionType.GetTypeInfo().GetGenericArguments();
        if (genericArguments is { Length: 1 })
            elementType = genericArguments[0];

        return elementType;
    }

    /// <summary>
    ///     Converts a collection of objects to an array of the specified target type.
    /// </summary>
    /// <param name="targetType">The Type representing the target array type.</param>
    /// <param name="items">The collection of objects to convert to an array.</param>
    /// <returns>
    ///     An array of the specified target type containing the converted objects, or null if the input collection is
    ///     null.
    /// </returns>
    private static Array ItemsToArray(Type targetType, IEnumerable<object> items)
    {
        if (items is null)
            return null;

        var list = items.ToList();
        var elementType = GetElementType(targetType);

        var array = (Array)ReflectionTypeUtil.CreateInstance(targetType, list.Count)();
        for (var index = 0; index < list.Count; ++index)
            array.SetValue(TypeUtil.IsPrimitive(elementType) ? ConvertType(elementType, list[index]) : list[index],
                index);

        return array;
    }

    /// <summary>
    ///     Converts a collection of objects to an instance of the specified target type that implements IList.
    /// </summary>
    /// <param name="targetType">The Type representing the target list type.</param>
    /// <param name="items">The collection of objects to convert to a list.</param>
    /// <returns>
    ///     An instance of the specified target type containing the converted objects,
    ///     or null if the input collection is null or the target type cannot be instantiated.
    /// </returns>
    private static object ItemsToIList(Type targetType, IEnumerable<object> items)
    {
        if (items is null)
            return null;

        var result = ReflectionTypeUtil.CreateInstance(targetType)();
        var elementType = GetElementType(targetType);

        if (result is IList list)
        {
            foreach (var obj in items)
                list.Add(TypeUtil.IsPrimitive(elementType) ? ConvertType(elementType, obj) : obj);

            return result;
        }

        var method = targetType.GetTypeInfo().GetMethod("Add");

        if (method == null) return null;


        foreach (var obj in items)
            method.Invoke(result, [
                TypeUtil.IsPrimitive(elementType) ? ConvertType(elementType, obj) : obj
            ]);

        return result;
    }

    /// <summary>
    ///     Gets the PropertyInfo for a property with the specified name in the given set of properties, considering
    ///     case-insensitive matches and custom attributes.
    /// </summary>
    /// <param name="properties">The array of PropertyInfo objects to search for the property.</param>
    /// <param name="propertyName">The name of the property to find.</param>
    /// <param name="declaringType">The Type that declares the property, used to disambiguate properties with the same name.</param>
    /// <returns>
    ///     The PropertyInfo for the property with the specified name, considering case-insensitive matches and custom
    ///     attributes,
    ///     or null if the property is not found or cannot be written to.
    /// </returns>
    private static PropertyInfo GetProperty(PropertyInfo[] properties, string propertyName, Type declaringType)
    {
        var instanceProps = properties.Where(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        //Custom attributes
        if (instanceProps.Count == 0)
            instanceProps = properties.Where(p => p.GetCustomAttribute<DynamoDBPropertyAttribute>() != null &&
                                                  propertyName.Equals(
                                                      p.GetCustomAttribute<DynamoDBPropertyAttribute>().AttributeName,
                                                      StringComparison.OrdinalIgnoreCase)).ToList();

        if (instanceProps.Count == 0)
            return null;

        var prop = instanceProps.Find(p => p.DeclaringType == declaringType) ??
                   instanceProps[0];

        if (prop is null || !prop.CanWrite) return null;

        return prop;
    }

    /// <summary>
    ///     Converts a value to the specified property type, considering type conversion and compatibility workarounds.
    /// </summary>
    /// <param name="propertyType">The target property type to convert the value to.</param>
    /// <param name="value">The value to be converted.</param>
    /// <returns>
    ///     The converted value of the specified property type, or the default value of the property type if the input value is
    ///     null.
    /// </returns>
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
    ///     Converts an AttributeValue to a DynamoDBEntry, handling different data types.
    /// </summary>
    /// <param name="attributeValue">The AttributeValue to be converted.</param>
    /// <returns>
    ///     A DynamoDBEntry representing the converted AttributeValue, or a DynamoDBNull if the input is null.
    /// </returns>
    private static DynamoDBEntry ConvertAttributeValue(AttributeValue attributeValue)
    {
        if (attributeValue is null)
            return new DynamoDBNull();

        if (attributeValue.IsBOOLSet)
            return new DynamoDBBool(attributeValue.BOOL);

        if (attributeValue.B is not null)
            return new Primitive(attributeValue.B);

        return attributeValue.N is not null ? new Primitive(attributeValue.N, true) : new Primitive(attributeValue.S);
    }

    /// <summary>
    ///     Converts a non-primitive property value to the desired type, considering custom converters.
    /// </summary>
    /// <param name="property">The PropertyInfo object representing the property.</param>
    /// <param name="attributeValue">The AttributeValue to be converted.</param>
    /// <param name="value">The current value of the property.</param>
    /// <param name="propertyConverter">The converter for custom converters.</param>
    /// <returns>
    ///     The converted value of the property, considering custom converters if available, or the original value if no custom
    ///     converter is found.
    /// </returns>
    private static object ConvertNonPrimitiveType(PropertyInfo property, AttributeValue attributeValue, object value,
        IPropertyConverter propertyConverter = null)
    {
        if (property.PropertyType.IsEnum)
            return Enum.Parse(property.PropertyType, value.ToString(), true);

        var customConverter = propertyConverter ?? property.GetCustomAttributes<DynamoDBPropertyAttribute>()
            .FirstOrDefault(a => a.Converter != null)?.Converter as IPropertyConverter;

        if (customConverter is null)
            return value;

        var convertedEntry = ConvertAttributeValue(attributeValue);

        return convertedEntry is null ? null : customConverter.FromEntry(convertedEntry);
    }

    /// <summary>
    ///     Convert an attribute array to specific type. The method is called when the object is being retrieved from DynamoDB
    /// </summary>
    /// <typeparam name="T">The desired Type </typeparam>
    /// <param name="items"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    internal static T ConvertAttributeValuesToType<T>(Dictionary<string, AttributeValue> items,
        DynamoContext context = null)
        where T : class, new()
    {
        if (items is null) return default;

        var instance = new T();

        var properties = instance.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

        if (properties.Length == 0)
            return instance;

        //Iterating over the attributes from dynamoDB
        foreach (var attributeValue in items)
        {
            var propertyKey = attributeValue.Key; //The default property name is the one from the database.
            var propertyTypeBuilder = context?.GetTypeBuilder<T>().GetProperty(propertyKey);

            //This will guarantee that only mapped(No ignored) Columns will be filled.
            if (propertyTypeBuilder is null && context != null)
                continue;

            if (propertyTypeBuilder is not null) propertyKey = propertyTypeBuilder.Name; // get the object property name

            var prop = GetProperty(properties, propertyKey, instance.GetType());

            if (prop is null)
                continue;

            object convertedValue;
            var value = CreateAttributeValueToObject(attributeValue.Value, prop.PropertyType);

            if (TypeUtil.IsPrimitive(prop.PropertyType))
            {
                convertedValue = ConvertType(prop.PropertyType, value);
            }
            else
            {
                if (TypeUtil.IsCollection(prop.PropertyType))
                    convertedValue = TypeUtil.IsDictionary(prop.PropertyType)
                        ? value
                        : ItemsToCollection(prop.PropertyType, (IEnumerable<object>)value);
                else
                    convertedValue = ConvertNonPrimitiveType(prop, attributeValue.Value, value,
                        context?.GetPropertyConverter(prop.PropertyType));
            }

            //setting the converted value
            prop.SetValue(instance, convertedValue, null);
        }

        InvokeMappedProperties(context, properties, instance);

        return instance;
    }

    /// <summary>
    ///     Invoke all mapped properties to fill the object
    /// </summary>
    /// <param name="context"></param>
    /// <param name="properties"></param>
    /// <param name="instance"></param>
    /// <typeparam name="T"></typeparam>
    private static void InvokeMappedProperties<T>(DynamoContext context, PropertyInfo[] properties, T instance)
    {
        if (context is null || properties is null || instance is null)
            return;

        if (!context.HasTypeBuilder<T>())
            return;

        var typeBuilder = context.GetTypeBuilder<T>();

        //All mapping properties that has action mapping will be called here
        foreach (var property in properties)
        {
            var propertyTypeBuilder = typeBuilder.GetProperty(property.Name);
            
            propertyTypeBuilder?.InvokeMaps(instance);
        }
    }

    /// <summary>
    ///     Converts a type to a dictionary of attributes and values. This happens when we are sending an object to DynamoDB
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="context"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal static Dictionary<string, AttributeValue> ConvertToAttributeValueMap<T>(T instance,
        DynamoContext context = null)
        where T : class
    {
        Check.NotNull(instance, nameof(instance));

        var properties = instance.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

        if (properties.Length == 0)
            return new Dictionary<string, AttributeValue>();

        var attributes = new Dictionary<string, AttributeValue>();

        var typeBuilder = context?.HasTypeBuilder<T>() == true ? context.GetTypeBuilder<T>() : null;

        //No Mapped properties - All properties will be filled using only the object properties
        if (typeBuilder is null)
        {
            foreach (var property in properties)
                attributes.Add(property.Name, CreateAttributeValue(property.GetValue(instance)));
        }
        else
        {
            //Considering all the mapped properties is has map. The system will get from the map and fill the attributes
            InvokeMappedProperties(context, properties, instance);
            
            var mappedProperties = typeBuilder.GetProperties();

            //get the mapped properties
            foreach (var mappedProperty in mappedProperties)
            {
                var propertyKey = mappedProperty.Name;

                var propertyTypeBuilder = typeBuilder.GetProperty(propertyKey);

                //This will guarantee that only mapped (No ignored) Columns will be filled.
                if (propertyTypeBuilder is null)
                    continue;

                var property = properties.FirstOrDefault(p => p.Name == propertyKey);

                var propertyType = property?.PropertyType;
                var propertyValue = property?.GetValue(instance);

                //Virtual Property - Mean that exists only in the mapping and database but not in the object
                if (property is null)
                {
                    //Invoke the map action to get the value.
                    propertyValue = mappedProperty.GetValue(instance);
                    propertyType = mappedProperty.Type;
                }

                //This is a property that is not mapped and not in the object
                if ((context.IgnoreNullValues && propertyValue is null) || propertyType is null)
                    continue;

                var converter = context.GetPropertyConverter(propertyType);

                if (converter is not null)
                    propertyValue = converter.ToEntry(propertyValue).ToString();

                attributes.Add(mappedProperty.ColumnName, CreateAttributeValue(propertyValue));
            }
        }

        return attributes;
    }
}