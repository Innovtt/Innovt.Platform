using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Innovt.Cloud.AWS.Dynamo.Mapping;

namespace Innovt.Cloud.AWS.Dynamo;

public abstract class DynamoContext
{
    public bool EnableChangingTracking { get; set; }
    
    private static ModelBuilder _modelBuilder;

    protected DynamoContext()
    {
        EnableChangingTracking = false;
    }
    
    ~DynamoContext()
    {
        _modelBuilder = null;
    }
    internal void BuildModel()
    {
        if(_modelBuilder is not null)
            return;
        
        _modelBuilder ??= new ModelBuilder();

        OnModelCreating(_modelBuilder);
    }
    
    protected abstract void OnModelCreating([DisallowNull]ModelBuilder modelBuilder);
}

public class ModelBuilder
{
    private readonly Dictionary<string, object> entities = new();
    
    public void AddConfiguration<T>(IEntityTypeDataModelMapper<T> entityTypeBuilder) where T:class
    {
        var entityName = typeof(T).Name;
        
        if(entities.ContainsKey(entityName))
            return;
        
        entities.Add(entityName,entityTypeBuilder);
    }

    public Dictionary<string, object> Entities => entities;
    
}