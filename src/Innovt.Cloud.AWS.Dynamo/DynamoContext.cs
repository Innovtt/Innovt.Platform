using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo;

/// <summary>
///     This class is responsible for managing the context of the DynamoDB for Code First strategy.
/// </summary>
public abstract class DynamoContext
{
    private static readonly object ObjLock = new();

    public CultureInfo DefaultCulture { get; set; } = CultureInfo.CurrentCulture;
    
    protected DynamoContext()
    {
        BuildModel();
    }

    private ModelBuilder ModelBuilder { get; set; }

    /// <summary>
    ///     It tells the context to ignore null properties when saving an entity.
    /// </summary>
    public bool IgnoreNullValues { get; set; } = true;

    private void BuildModel()
    {
        lock (ObjLock)
        {
            if (ModelBuilder != null)
                return;

            ModelBuilder = new ModelBuilder();

            OnModelCreating(ModelBuilder);
        }
    }

    public EntityTypeBuilder<T> GetTypeBuilder<T>()
    {
        return ModelBuilder?.GetTypeBuilder<T>();
    }

    public IPropertyConverter GetPropertyConverter(Type type)
    {
        return ModelBuilder?.GetPropertyConverter(type);
    }

    public bool HasTypeBuilder(Type type)
    {
        return ModelBuilder.HasTypeBuilder(type);
    }

    /// <summary>
    ///     Utility method to add a new entity to the context
    /// </summary>
    /// <param name="instance"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasTypeBuilder<T>(object instance = null)
    {
        return ModelBuilder.HasTypeBuilder<T>(instance);
    }


    protected abstract void OnModelCreating([DisallowNull] ModelBuilder modelBuilder);
}