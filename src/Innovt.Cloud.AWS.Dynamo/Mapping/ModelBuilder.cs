using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Mapping;

public class ModelBuilder
{
    public Dictionary<string, object> Entities { get; } = new();

    public Dictionary<Type, IPropertyConverter> Converters { get; } = new();

    public ModelBuilder AddConfiguration<T>(IEntityTypeDataModelMapper<T> entityTypeDataModelMapper) where T : class
    {
        if (entityTypeDataModelMapper == null) throw new ArgumentNullException(nameof(entityTypeDataModelMapper));

        var entityTypeBuilder = new EntityTypeBuilder<T>();

        entityTypeDataModelMapper.Configure(entityTypeBuilder);
        
        AddTypeBuilder(entityTypeBuilder);

        return this;
    }


    public ModelBuilder AddPropertyConverter(Type type, IPropertyConverter converter)
    {
        Check.NotNull(type, nameof(type));
        Check.NotNull(converter, nameof(converter));

        Converters.TryAdd(type, converter);

        return this;
    }
    
    public EntityTypeBuilder<TEntity> Entity<TEntity>() where TEntity : class
    {
        return AddTypeBuilder<TEntity>();
    }
    
    /// <summary>
    /// Manage the EntityTypeBuilder creation for each entity.
    /// </summary>
    /// <param name="entityTypeBuilder"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    private EntityTypeBuilder<TEntity> AddTypeBuilder<TEntity>(EntityTypeBuilder<TEntity> entityTypeBuilder=null) where TEntity : class
    {
        var entityName = typeof(TEntity).Name;
        
        if (Entities.TryGetValue(entityName, out var entity))
            return (EntityTypeBuilder<TEntity>) entity;
        
        //Initialize the entityTypeBuilder if it is null
        entityTypeBuilder ??= new EntityTypeBuilder<TEntity>();

        Entities.TryAdd(typeof(TEntity).Name, entityTypeBuilder);

        return entityTypeBuilder;
    }
}