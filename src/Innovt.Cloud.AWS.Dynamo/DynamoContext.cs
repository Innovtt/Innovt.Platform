using System.Collections.Generic;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo;

public abstract class DynamoContext
{
    private static ModelBuilder modelBuilder;
    
    public DynamoContext()
    {
    }
    
    ~DynamoContext()
    {
        modelBuilder = null;
    }

    internal void BuildModel()
    {
        if(modelBuilder is not null)
            return;
        
        modelBuilder ??= new ModelBuilder();

        OnModelCreating(modelBuilder);
    }
    
    protected abstract void OnModelCreating(ModelBuilder modelBuilder);
}

public class ModelBuilder
{
    private Dictionary<string, object> entities = new();
    
    public void AddConfiguration<T>(EntityTypeBuilder<T>  entityTypeBuilder) where T:class
    {
        var entityName = typeof(T).Name;
        
        if(entities.ContainsKey(entityName))
            return;
        
        entities.Add(entityName,entityTypeBuilder);
    }


    public Dictionary<string, object> Entities => entities;
    
}