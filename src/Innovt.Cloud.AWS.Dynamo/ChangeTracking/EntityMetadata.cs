// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Dynamo.ChangeTracking;

internal sealed class EntityMetadata
{
    private static readonly ConcurrentDictionary<Type, EntityMetadata> Cache = new();

    private EntityMetadata(Func<object, object?>[] getters, bool allScalar)
    {
        Getters = getters;
        AllScalar = allScalar;
    }

    public Func<object, object?>[] Getters { get; }

    public bool AllScalar { get; }

    public static EntityMetadata For(Type type)
    {
        return Cache.GetOrAdd(type, Build);
    }

    internal static bool IsSimple(Type type)
    {
        if (Nullable.GetUnderlyingType(type) is { } underlying)
            type = underlying;

        if (type.IsPrimitive || type.IsEnum)
            return true;

        return type == typeof(string)
               || type == typeof(decimal)
               || type == typeof(DateTime)
               || type == typeof(DateTimeOffset)
               || type == typeof(DateOnly)
               || type == typeof(TimeOnly)
               || type == typeof(TimeSpan)
               || type == typeof(Guid)
               || type == typeof(Uri);
    }

    private static EntityMetadata Build(Type type)
    {
        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && !p.IsDefined(typeof(JsonIgnoreAttribute), inherit: true))
            .ToArray();

        var getters = new Func<object, object?>[properties.Length];
        var allScalar = true;

        for (var i = 0; i < properties.Length; i++)
        {
            getters[i] = CompileGetter(properties[i]);
            if (allScalar && !IsSimple(properties[i].PropertyType))
                allScalar = false;
        }

        return new EntityMetadata(getters, allScalar);
    }

    private static Func<object, object?> CompileGetter(PropertyInfo property)
    {
        var parameter = Expression.Parameter(typeof(object), "instance");
        var cast = Expression.Convert(parameter, property.DeclaringType!);
        var access = Expression.Property(cast, property);
        var boxed = Expression.Convert(access, typeof(object));

        return Expression.Lambda<Func<object, object?>>(boxed, parameter).Compile();
    }
}
