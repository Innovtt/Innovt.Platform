using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Innovt.Core.Utilities;

public static class TypeUtil
{
    /// <summary>
    ///     An array containing primitive types that can be represented as attributes.
    /// </summary>
    private static readonly List<Type> PrimitiveTypesList =
    [
        typeof(bool),
        typeof(byte),
        typeof(char),
        typeof(DateTime),
        typeof(DateTime?),
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
        typeof(Stream),
        typeof(TimeSpan),
        typeof(DateTimeOffset),
        typeof(DateTimeOffset?),
        typeof(bool?),
        typeof(byte?),
        typeof(char?),
        typeof(decimal?),
        typeof(double?),
        typeof(int?),
        typeof(long?),
        typeof(sbyte?),
        typeof(short?),
        typeof(float?),
        typeof(uint?),
        typeof(ulong?),
        typeof(ushort?),
        typeof(Guid?),
        typeof(TimeSpan?),
        typeof(TimeOnly),
        typeof(TimeOnly?)
    ];


    private static readonly HashSet<TypeInfo> PrimitiveTypeInfos
        =
        [..PrimitiveTypesList.Select(t => t.GetTypeInfo())];

    /// <summary>
    ///     Adds a new type to the list of recognized primitive types.
    /// </summary>
    /// <param name="type">The type to add.</param>
    public static void AddPrimitiveType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (PrimitiveTypesList.Contains(type)) return;

        PrimitiveTypesList.Add(type);
        PrimitiveTypeInfos.Add(type.GetTypeInfo());
    }

    /// <summary>
    ///     Checks if a given type is a primitive DynamoDB type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is primitive; otherwise, false.</returns>
    public static bool IsPrimitive(Type type)
    {
        var typeWrapper = type.GetTypeInfo();

        return PrimitiveTypeInfos.Any(ti => typeWrapper.IsAssignableFrom(ti));
    }

    /// <summary>
    ///     Checks if a given type is a collection (array or IEnumerable).
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is a collection; otherwise, false.</returns>
    public static bool IsCollection(Type type)
    {
        if (type == null) return false;
        
        return type.IsArray || (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type));
    }

    /// <summary>
    ///     Checks if a given type is a dictionary.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is a dictionary; otherwise, false.</returns>
    public static bool IsDictionary(Type type)
    {
        if (type == null) return false;
        
        return type.IsArray || (type.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(type)) ||
               typeof(IDictionary).IsAssignableFrom(type);
    }

    /// <summary>
    ///     Returns true if the type is a numeric type.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool IsNumericList(IList list)
    {
        if (list == null) return false;
        if (list.Count == 0) return false;
        
        var type = list[0].GetType();
        return type == typeof(int) || type == typeof(double) || type == typeof(float) ||
               type == typeof(decimal) || type == typeof(long);
    }
}