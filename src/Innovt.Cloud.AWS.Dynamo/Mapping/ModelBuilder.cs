using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.AWS.Dynamo.Exceptions;
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
    ///     Return the PropertyConverter for the given type.
    /// </summary>
    /// <param name="type">A type that the system will search for a converter</param>
    /// <returns>Returns a converter or null.</returns>
    public IPropertyConverter GetPropertyConverter(Type type)
    {
        return Converters?.GetValueOrDefault(type);
    }

    /// <summary>
    ///     THis is a utility method to get the entity type
    /// </summary>
    /// <param name="instance"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static Type GetEntityType<T>(object instance = null)
    {
        return instance is null ? typeof(T) : instance.GetType();
    }

    /// <summary>
    ///     A utility method to get the entity name
    /// </summary>
    /// <param name="instance"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static string GetEntityName<T>(object instance = null)
    {
        var instanceType = GetEntityType<T>(instance);

        return instanceType.Name;
    }

    public bool HasTypeBuilder(Type type)
    {
        var entityName = type.Name;

        return Entities.TryGetValue(entityName, out var value);
    }

    /// <summary>
    ///     Utility method to add a new entity to the context
    /// </summary>
    /// <param name="instance"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasTypeBuilder<T>(object instance = null)
    {
        var entityName = GetEntityName<T>(instance);

        return Entities.TryGetValue(entityName, out var value);
    }

    /// <summary>
    ///     Manage the EntityTypeBuilder creation for each entity.
    /// </summary>
    /// <param name="entityTypeBuilder"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    private EntityTypeBuilder<TEntity> AddTypeBuilder<TEntity>(EntityTypeBuilder<TEntity> entityTypeBuilder = null)
        where TEntity : class
    {
        var entityName = typeof(TEntity).Name;

        if (Entities.TryGetValue(entityName, out var entity))
            return (EntityTypeBuilder<TEntity>)entity;

        //Initialize the entityTypeBuilder if it is null
        entityTypeBuilder ??= new EntityTypeBuilder<TEntity>();

        Entities.TryAdd(typeof(TEntity).Name, entityTypeBuilder);

        return entityTypeBuilder;
    }

    /// <summary>
    ///     It returns a EntityTypeBuilder for the given entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="MissingEntityMapException"></exception>
    public EntityTypeBuilder<T> GetTypeBuilder<T>()
    {
        var entityName = GetEntityName<T>();

        if (!Entities.TryGetValue(entityName, out var value))
            throw new MissingEntityMapException(entityName);

        if (value is EntityTypeBuilder<T> entityTypeBuilder) return entityTypeBuilder;

        throw new MissingEntityMapException(entityName);
    }
}