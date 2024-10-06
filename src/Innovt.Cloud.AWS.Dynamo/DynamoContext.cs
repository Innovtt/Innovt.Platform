using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.AWS.Dynamo.Exceptions;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo;

/// <summary>
///     This class is responsible for managing the context of the DynamoDB for Code First strategy.
/// </summary>
public abstract class DynamoContext
{
    private static readonly object ObjLock = new();
    private ModelBuilder modelBuilder = null!;


    protected DynamoContext()
    {
        BuildModel();
    }

    /// <summary>
    ///     Returns the entities that have been mapped.
    /// </summary>
    public Dictionary<string, object> Entities => modelBuilder.Entities;

    /// <summary>
    ///     It tells the context to ignore null properties when saving an entity.
    /// </summary>
    public bool IgnoreNullValues { get; set; } = true;

    private void BuildModel()
    {
        lock (ObjLock)
        {
            if (modelBuilder != null)
                return;

            modelBuilder = new ModelBuilder();

            OnModelCreating(modelBuilder);
        }
    }

    private static Type GetEntityType<T>(object instance=null)
    {
        if (instance is null)
            return typeof(T);

        return instance.GetType();
    }

    private static string GetEntityName<T>(object instance=null)
    {
        var instanceType = GetEntityType<T>(instance);

        return instanceType.Name;
    }

    public bool HasTypeBuilder<T>(object instance=null)
    {
        var entityName = GetEntityName<T>(instance);

        return Entities.TryGetValue(entityName, out var value);
    }
    public bool HasTypeBuilder(Type type)
    {
        var entityName = type.Name;

        return Entities.TryGetValue(entityName, out var value);
    }
    
    public EntityTypeBuilder<T> GetTypeBuilder<T>()
    {
        var entityName = GetEntityName<T>();

        if (!Entities.TryGetValue(entityName, out var value))
            throw new MissingEntityMapException(entityName);

        if (value is EntityTypeBuilder<T> entityTypeBuilder) return entityTypeBuilder;

        throw new MissingEntityMapException(entityName);
    }

    public IPropertyConverter GetPropertyConverter(Type type)
    {
        return modelBuilder?.Converters?.GetValueOrDefault(type);
    }

    protected abstract void OnModelCreating([DisallowNull] ModelBuilder modelBuilder);
}