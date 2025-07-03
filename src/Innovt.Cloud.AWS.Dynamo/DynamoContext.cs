using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.AWS.Dynamo.Converters.Attributes;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo;

/// <summary>
///     This class is responsible for managing the context of the DynamoDB for Code First strategy.
/// </summary>
public abstract class DynamoContext
{
    private static readonly object ObjLock = new();

    private  CultureInfo DefaultCulture { get; set; } = CultureInfo.CurrentCulture;
    
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

            //Clear all caches to avoid any issue with mapping
            AttributeConverter.ClearCaches();
            
            ModelBuilder = new ModelBuilder();

            OnModelCreating(ModelBuilder);
        }
    }
    
    public EntityTypeBuilder? GetEntityBuilder<T>()
    {
        return ModelBuilder.GetEntityBuilder<T>();
    }
    public EntityTypeBuilder GetEntityBuilder(string name)
    {
        return ModelBuilder.GetEntityBuilder(name);
    }
    
    public IPropertyConverter? GetPropertyConverter(Type type)
    {
        return ModelBuilder.GetPropertyConverter(type);
    }

    /// <summary>
    /// Set the culture that will be used to convert the properties.
    /// </summary>
    /// <param name="cultureInfo"></param>
    /// <returns></returns>
    public bool SetCulture(CultureInfo cultureInfo)
    {
        if (cultureInfo == null)
            return false;

        DefaultCulture = cultureInfo;
        
        Thread.CurrentThread.CurrentCulture = DefaultCulture;
        
        return true;
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
    public bool HasTypeBuilder<T>(object? instance = null)
    {
        return ModelBuilder.HasTypeBuilder<T>(instance);
    }
    
    /// <summary>
    /// Check if the entity has a builder.
    /// </summary>
    /// <param name="entityName">The name of the typebuilder</param>
    /// <returns></returns>
    public bool HasTypeBuilder(string entityName)
    {
        return ModelBuilder.HasTypeBuilder(entityName);
    }
    
    protected abstract void OnModelCreating([DisallowNull] ModelBuilder modelBuilder);
}