using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Dynamo.Converters.Attributes.Exceptions;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Converters.Attributes;

internal static class InstanceCreator
{
    public static T CreateInstance<T>(Dictionary<string, AttributeValue> items, DynamoContext context = null)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(items);

        if (context?.HasTypeBuilder<T>() != true)
            return ReflectionTypeUtil.CreateInstance<T>()();

        var typeBuilder = context.GetEntityBuilder<T>();

        if (typeBuilder?.Discriminator == null)
            return ReflectionTypeUtil.CreateInstance<T>()();

        var discriminatorName = typeBuilder.Discriminator.Name;
        if (!items.TryGetValue(discriminatorName, out var discriminatorAttribute))
            throw new DiscriminatorException($"Discriminator value not found for property {discriminatorName}");

        var discriminatorValue = discriminatorAttribute.N ?? discriminatorAttribute.S;

        if (string.IsNullOrEmpty(discriminatorValue))
            throw new DiscriminatorException("Discriminator value is null or empty");

        return (T)typeBuilder.Discriminator.GetValue(discriminatorValue);
    }
}