using System;
using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

/// <summary>
///     Represents a builder for defining the properties of an entity type.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>

public sealed class PropertyBuilder<T>: PropertyBuilder
{
    private readonly List<Action<T>> mappedActions = [];
    
    public new EntityTypeBuilder<T> Builder { get; set; }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyBuilder{T}" /> class with a specified property name and
    ///     type.
    /// </summary>
    /// <param name="propertyName">The function to retrieve the property name.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="builder">This is the main build to help the user with fluent api</param>
    public PropertyBuilder(Func<T, string> propertyName, Type propertyType, EntityTypeBuilder<T> builder) : this(
        propertyName, builder)
    {
        Type = propertyType;
        Builder = builder;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyBuilder{T}" /> class with a specified property name.
    /// </summary>
    /// <param name="propertyName">The function to retrieve the property name.</param>
    /// <param name="builder">This is the main build to help the user with fluent api</param>
    public PropertyBuilder(Func<T, string> propertyName, EntityTypeBuilder<T> builder)
    {
        ArgumentNullException.ThrowIfNull(propertyName);
        Name = propertyName.Invoke(default);
        Type = propertyName.Invoke(default).GetType();
        Builder = builder;
    }
    
    private new bool HasMapAction => mappedActions.Count > 0;
    
    /// <summary>
    ///     Specifies a custom column name for the property in the database.
    /// </summary>
    /// <param name="name">The custom column name.</param>
    /// <returns>The current instance of <see cref="PropertyBuilder{T}" />.</returns>
    public new PropertyBuilder<T> HasColumnName(string name)=> (PropertyBuilder<T>)base.HasColumnName(name);
    
    /// <summary>
    ///     Specifies that the property is of decimal type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyBuilder{T}" />.</returns>
    public new PropertyBuilder<T> HasMaxLength(int maxLength) => (PropertyBuilder<T>)base.HasMaxLength(maxLength);

    /// <summary>
    ///     It set a value for property.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public new PropertyBuilder<T> HasDefaultValue(object value) => (PropertyBuilder<T>)base.HasDefaultValue(value);

    /// <summary>
    ///     Define when a fi
    /// </summary>
    /// eld will be required during save operations
    /// <param name="isRequired">Default value if true.</param>
    /// <returns></returns>
    public new PropertyBuilder<T> IsRequired(bool isRequired = true) => (PropertyBuilder<T>)base.IsRequired(isRequired);




    /// <summary>
    ///     Define a delegate to parse the property.
    /// </summary>
    /// <param name="actionMap">The action to parse the property.</param>
    /// <returns>The current instance of <see cref="PropertyBuilder{T}" />.</returns>
    /// 
    public PropertyBuilder<T> WithMap(Action<T> actionMap)
    {
        ArgumentNullException.ThrowIfNull(actionMap);

        mappedActions.Add(actionMap);

        return this;
    }
    
    /// <summary>
    ///     Define a delegate to set the value of the property based on the entity.
    /// </summary>
    /// <param name="valueDelegate"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public PropertyBuilder<T> SetDynamicValue(Func<T, object> valueDelegate)
    {
        ArgumentNullException.ThrowIfNull(valueDelegate);
    
        SetValueDelegate = obj => valueDelegate((T)obj);
        
        return this;
    }
    
    /// <summary>
    ///  Invoke all map actions
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public PropertyBuilder<T> InvokeMaps<TEntity>(TEntity entity) where TEntity : T
    {
        if (!HasMapAction)
            return this;

        foreach (var action in mappedActions)
            action(entity);

        return this;
    }
}