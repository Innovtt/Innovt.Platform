using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Innovt.Cloud.AWS.Dynamo.Converters.Attributes.Exceptions;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo.Converters.Attributes;

public static class DiscriminatorManager
{
    private static readonly ConcurrentDictionary<(string discriminatorName, string discriminatorValue), Type>
        DiscriminatorTypeCache = new();

    internal static List<PropertyBuilder> GetDiscriminatorProperties<T>(DynamoContext context,
        EntityTypeBuilder typeBuilder,
        PropertyInfo[] properties, T instance)
        where T : class
    {
        if (typeBuilder?.Discriminator is null)
            return [];

        var discriminatorName = typeBuilder.Discriminator.Name;

        var discriminatorProperty = Array.Find(properties, p => p.Name == discriminatorName);
        var discriminatorValue = discriminatorProperty?.GetValue(instance)?.ToString();

        if (discriminatorValue is null)
            throw new DiscriminatorException(
                $"The instance has no value for discriminator property {discriminatorName}");

        var discriminatorType = GetDiscriminatorType(typeBuilder, discriminatorValue);

        if (discriminatorType is null)
            return [];

        if (!context.HasTypeBuilder(discriminatorType.Name))
            throw new DiscriminatorException(
                $"The discriminator type {discriminatorType.Name} does not exist in the context.Please check your context.");

        var typeBuildForDiscriminator = context.GetEntityBuilder(discriminatorType.Name);

        var baseEntityProperties = typeBuilder.GetProperties();
        var discriminatorProperties = typeBuildForDiscriminator?.GetProperties() ?? [];

        if (discriminatorProperties.Count == 0 || baseEntityProperties.Count == 0)
            return discriminatorProperties;

        //Select only properties that are not in the original entity
        discriminatorProperties = discriminatorProperties.Where(p =>
            baseEntityProperties.Select(prop => prop.Name).All(prop => prop != p.Name)).ToList();

        return discriminatorProperties;
    }


    private static Type? GetDiscriminatorType(EntityTypeBuilder typeBuilder, string discriminatorValue)
    {
        if (typeBuilder?.Discriminator is null)
            return null;

        var cacheKey = (typeBuilder.Discriminator.Name, discriminatorValue);

        return DiscriminatorTypeCache.GetOrAdd(cacheKey, key =>
            typeBuilder.Discriminator.GetTypeForDiscriminator(discriminatorValue));
    }
}