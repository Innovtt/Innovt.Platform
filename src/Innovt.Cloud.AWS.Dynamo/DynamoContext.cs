using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.AWS.Dynamo.Mapping;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo;

/// <summary>
/// This class is responsible for managing the context of the DynamoDB for Code First strategy.
/// </summary>
public abstract class DynamoContext
{
    public bool EnableChangingTracking { get; set; } = false;

    private static ModelBuilder modelBuilder = null!;
    private static readonly object ObjLock = new();
    
    protected DynamoContext()
    {   
        BuildModel();
    }
    public Dictionary<string, object> Entities => modelBuilder.Entities;

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
    
    public EntityTypeBuilder<T> GetTypeBuilder<T>()
    {
        var entityName = typeof(T).Name;

        if (!Entities.TryGetValue(entityName, out var value))
            throw new InvalidOperationException($"Entity {entityName} not found in model");

        if (value is not EntityTypeBuilder<T> entityTypeBuilder)
            throw new InvalidOperationException($"Entity {entityName} not found in model");
        
        return entityTypeBuilder;
    }
    public IPropertyConverter GetPropertyConverter(Type type)
    {
        return modelBuilder?.Converters?.GetValueOrDefault(type);
    }
    
    protected abstract void OnModelCreating([DisallowNull]ModelBuilder modelBuilder);
}