using System;
using System.Collections.Concurrent;
using Innovt.Cloud.AWS.Dynamo.Exceptions;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

/// <summary>
///  Represents a discriminator builder for defining the properties of an entity type.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class DiscriminatorBuilder<TEntity>(string name, Type type, EntityTypeBuilder<TEntity> builder)
{
    /// <summary>
    /// The column name that will be used to store the discriminator.
    /// </summary>
    private string Name { get; set;  } = name ?? throw new ArgumentNullException(nameof(name));

    /// <summary>
    /// The discriminator type. It will be used to cast the values 
    /// </summary>
    private Type Type { get; set;  } = type ?? throw new ArgumentNullException(nameof(type));

    /// <summary>
    /// The values attached to the discriminator.
    /// </summary>
    private ConcurrentDictionary<object,Type> TypeValues { get; set;  } = new();
    private ConcurrentDictionary<object,object> InstanceValues { get; set;  } = new();

    public EntityTypeBuilder<TEntity> Builder { get; private set; } = builder ?? throw new ArgumentNullException(nameof(builder));

    /// <summary>
    /// Simple case of a class that can inherit from another class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DiscriminatorBuilder<TEntity> HasValue<T>() where T : TEntity, new()
    {
        TypeValues.TryAdd(typeof(T).Name,typeof(T));
        
        return this;
    }
    
    /// <summary>
    /// Use this method when you need to define a value for a specific type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DiscriminatorBuilder<TEntity> HasValue<T>(T value) where T : TEntity
    {   
        InstanceValues.TryAdd(typeof(T).Name,value);
        
        return this;
    }
  
    /// <summary>
    /// Use this method when you have a type T with a parameterless constructor and will be instanciated when matched the whenValue parameter
    /// </summary>
    /// <param name="whenValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DiscriminatorBuilder<TEntity> HasValue<T>(object whenValue) where T : TEntity, new()
    {
        ArgumentNullException.ThrowIfNull(whenValue);
        
        TypeValues.TryAdd(whenValue,typeof(T));
        
        return this;
    }
    
    /// <summary>
    /// Use this method when you want to define a value for a specific type and a specific value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="whenValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DiscriminatorBuilder<TEntity> HasValue<T>(T value, object whenValue) where T : TEntity
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(whenValue);
        
        InstanceValues.TryAdd(whenValue,value);
        
        return this;
    }
    
    /// <summary>
    /// Get the type for the discriminator.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Type GetTypeForDiscriminator(object value)
    {
        if(TypeValues.TryGetValue(value, out var typeValue))
        {
            return typeValue;
        }
        
        if(InstanceValues.TryGetValue(value, out var defaultObj))
        {
            return defaultObj.GetType();
        }
        
        throw new InvalidDiscriminatorException(value);
    }
    
    public T GetValue<T>(object value) where T : TEntity
    {
        //Check is an instance value exists
        if (InstanceValues.TryGetValue(value, out var defaultObj))
        {
            return (T)defaultObj;
        }

        //Check if a type value exists
        if(TypeValues.TryGetValue(value, out var typeValue))
        {
            return (T)ReflectionTypeUtil.CreateInstance(type)();
        }
        
        throw new InvalidDiscriminatorException(value);
    }
    public object GetValue(object value)
    {
        if (InstanceValues.TryGetValue(value, out var defaultObj))
        {
            return defaultObj;
        }

        return TypeValues.TryGetValue(value, out var typeValue) ? ReflectionTypeUtil.CreateInstance(typeValue)() : null;
    }
}