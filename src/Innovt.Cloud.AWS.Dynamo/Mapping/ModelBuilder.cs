using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Mapping;

public class ModelBuilder
{
    private readonly Dictionary<string, object> entities = new();
    private readonly Dictionary<Type, IPropertyConverter> converters = new();
    
    public ModelBuilder AddConfiguration<T>(IEntityTypeDataModelMapper<T> entityTypeDataModelMapper) where T:class
    {
        if (entityTypeDataModelMapper == null) throw new ArgumentNullException(nameof(entityTypeDataModelMapper));
        
        var entityTypeBuilder = new EntityTypeBuilder<T>();
        
        entityTypeDataModelMapper.Configure(entityTypeBuilder);
        
        entities.TryAdd(typeof(T).Name, entityTypeBuilder);
        
        return this;
    }
    
    
    public ModelBuilder AddPropertyConverter(Type type,IPropertyConverter converter)
    {
        Check.NotNull(type,nameof(type));
        Check.NotNull(converter,nameof(converter));
        
        converters.TryAdd(type,converter);

        return this;
    }
    
    public EntityTypeBuilder<TEntity> Entity<TEntity>() where TEntity:class
    {   
        var entityTypeBuilder = new EntityTypeBuilder<TEntity>();
        
        entities.TryAdd(typeof(TEntity).Name, entityTypeBuilder);

        return entityTypeBuilder;
    }
    
    public Dictionary<string, object> Entities => entities;
    
    public Dictionary<Type, IPropertyConverter> Converters => converters;
}