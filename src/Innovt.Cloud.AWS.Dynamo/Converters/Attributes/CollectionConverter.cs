using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Converters.Attributes;

internal static class CollectionConverter
{
    /// <summary>
    ///     Converts a collection of items to the specified target type.
    /// </summary>
    /// <param name="targetType">The desired Type to convert the items to.</param>
    /// <param name="items">The collection of items to convert.</param>
    /// <returns>
    ///     An object of the specified target type containing the converted items.
    /// </returns>
    public static object ItemsToCollection(Type targetType, IEnumerable<object> items)
    {
        return !targetType.IsArray
            ? ItemsToIList(targetType, items)
            : ItemsToArray(targetType, items);
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
        if (items == null)
            return null;

        var list = items.ToList();
        var elementType = GetElementType(targetType);
        var array = (Array)ReflectionTypeUtil.CreateInstance(targetType, list.Count)();

        for (var index = 0; index < list.Count; index++)
        {
            array.SetValue(
                TypeUtil.IsPrimitive(elementType)
                    ? TypeConverter.ConvertType(elementType, list[index])
                    : list[index],
                index);
        }

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
        if (items == null)
            return null;

        var result = ReflectionTypeUtil.CreateInstance(targetType)();
        var elementType = GetElementType(targetType);

        if (result is IList list)
        {
            foreach (var obj in items)
                list.Add(TypeUtil.IsPrimitive(elementType) ? TypeConverter.ConvertType(elementType, obj) : obj);

            return result;
        }

        var method = targetType.GetTypeInfo().GetMethod("Add");
        if (method == null) return null;

        foreach (var obj in items)
            method.Invoke(result, [
                TypeUtil.IsPrimitive(elementType) ? TypeConverter.ConvertType(elementType, obj) : obj
            ]);

        return result;
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
        if (elementType != null)
            return elementType;

        var genericArguments = collectionType.GetTypeInfo().GetGenericArguments();
        return genericArguments is { Length: 1 } ? genericArguments[0] : null;
    }
}