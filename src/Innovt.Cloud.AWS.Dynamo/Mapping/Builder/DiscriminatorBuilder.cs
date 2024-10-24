using System;
using System.Collections.Concurrent;
using System.Globalization;
using Innovt.Cloud.AWS.Dynamo.Exceptions;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

/// <summary>
///  Represents a discriminator builder for defining the properties of an entity type.
/// </summary>
public abstract class DiscriminatorBuilder(string name, EntityTypeBuilder builder)
{
    /// <summary>
    /// The column name that will be used to store the discriminator.
    /// </summary>
    public string Name { get; set;  } = name ?? throw new ArgumentNullException(nameof(name));

    /// <summary>
    /// The values attached to the discriminator.
    /// </summary>
    private ConcurrentDictionary<string,Type> TypeValues { get; set;  } = new();
    private ConcurrentDictionary<string,object> InstanceValues { get; set;  } = new();

    public EntityTypeBuilder Builder { get; private set; } = builder ?? throw new ArgumentNullException(nameof(builder));

    /// <summary>
    /// Simple case of a class that can inherit from another class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DiscriminatorBuilder HasValue<T>() where T : new()
    {
        TypeValues.TryAdd(typeof(T).Name,typeof(T));
        
        return this;
    }
    
    /// <summary>
    /// Use this method when you need to define a value for a specific type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DiscriminatorBuilder HasValue<T>(T value)
    {   
        InstanceValues.TryAdd(typeof(T).Name,value);
        
        return this;
    }
  
    /// <summary>
    /// Use this method when you have a type T with a parameterless constructor and will be created when matched the whenValue parameter
    /// </summary>
    /// <param name="whenValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DiscriminatorBuilder HasValue<T>(string whenValue) where T : new()
    {
        ArgumentNullException.ThrowIfNull(whenValue);
        
        TypeValues.TryAdd(whenValue,typeof(T));
     
        return this;
    }
    
    /// <summary>
    /// Use this method when you have a type T with a parameterless constructor and will be created when matched the whenValue parameter
    /// </summary>
    /// <param name="whenValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DiscriminatorBuilder HasValue<T>(int whenValue) where T : new()
    {
        return HasValue<T>(whenValue.ToString(CultureInfo.InvariantCulture));
    }
    
    /// <summary>
    /// Use this method when you want to define a value for a specific type and a specific value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="whenValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DiscriminatorBuilder HasValue<T>(T value, string whenValue)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(whenValue);
        
        InstanceValues.TryAdd(whenValue,value);
        
        return this;
    }
    
    /// <summary>
    /// Use this method when you want to define a value for a specific type and a specific value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="whenValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public DiscriminatorBuilder HasValue<T>(T value, int whenValue)
    {
        return HasValue(value, whenValue.ToString(CultureInfo.InvariantCulture));
    }
    
    /// <summary>
    /// Get the type for the discriminator.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Type GetTypeForDiscriminator(string value)
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
    
    /// <summary>
    /// Get the type for the discriminator.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Type GetTypeForDiscriminator(int value)
    {
        return GetTypeForDiscriminator(value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Get the value type for the discriminator value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public T GetValue<T>(string value)
    {
        //Check is an instance value exists
        if (InstanceValues.TryGetValue(value, out var defaultObj))
        {
            return (T)defaultObj;
        }

        //Check if a type value exists
        if(TypeValues.TryGetValue(value, out var typeValue))
        {
            return (T)ReflectionTypeUtil.CreateInstance(typeValue)();
        }
        
        throw new InvalidDiscriminatorException(value);
    }

    /// <summary>
    /// Get the value type for the discriminator value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public T GetValue<T>(int value)
    {
        return GetValue<T>(value.ToString(CultureInfo.InvariantCulture));
    }
    
    /// <summary>
    /// Get the instance of a object with a discriminator value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public object GetValue(string value)
    {
        if (InstanceValues.TryGetValue(value, out var defaultObj))
        {
            return defaultObj;
        }

        return TypeValues.TryGetValue(value, out var typeValue) ? ReflectionTypeUtil.CreateInstance(typeValue)() : null;
    }
    
    /// <summary>
    /// Get the instance of a object with a discriminator value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public object GetValue(int value)
    {
        return GetValue(value.ToString(CultureInfo.InvariantCulture));
    }
}