using System;
using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

/// <summary>
///     Represents a builder for defining the properties of an entity type.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class PropertyTypeBuilder<T>
{
    private readonly List<Action<T>> mapActions = [];

    private string columnName;
    private Func<T, object> setValueDelegate;


    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyTypeBuilder{T}" /> class with a specified property name and
    ///     type.
    /// </summary>
    /// <param name="propertyName">The function to retrieve the property name.</param>
    /// <param name="propertyType">The type of the property.</param>
    /// <param name="builder">This is the main build to help the user with fluent api</param>
    public PropertyTypeBuilder(Func<T, string> propertyName, Type propertyType,EntityTypeBuilder<T> builder) : this(propertyName,builder)
    {
        Type = propertyType;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyTypeBuilder{T}" /> class with a specified property name.
    /// </summary>
    /// <param name="propertyName">The function to retrieve the property name.</param>
    /// <param name="builder">This is the main build to help the user with fluent api</param>
    public PropertyTypeBuilder(Func<T, string> propertyName, EntityTypeBuilder<T> builder)
    {
        Name = propertyName.Invoke(default);
        Type = propertyName.Invoke(default).GetType();
        Builder = builder;
    }

   
    public EntityTypeBuilder<T> Builder { get; set; }
    
    /// <summary>
    ///     Gets a value if the field is required.
    /// </summary>
    public bool Required { get; private set; }

    /// <summary>
    ///     Has map actions is when you need to map some property.
    /// </summary>
    public bool HasMapAction => mapActions.Count > 0;

    /// <summary>
    ///     Gets the name of the property.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Get the value of the property.
    /// </summary>
    private object Value { get; set; }

    /// <summary>
    ///     Gets the type of the property.
    /// </summary>
    public Type Type { get; private set; }

    /// <summary>
    ///     Gets or sets the column name associated with the property. if the column name is not set, the property name is
    ///     used.
    /// </summary>
    public string ColumnName
    {
        get => columnName ?? Name;
        private set => columnName = value;
    }

    /// <summary>
    ///     The max length of the property
    /// </summary>
    public int MaxLength { get; private set; }

    /// <summary>
    ///     This template property will be user to create values for the property dynamicaly.
    /// </summary>
    public string Template { get; private set; }

    /// <summary>
    ///     Specifies that the property is of string type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> AsString()
    {
        Type = typeof(string);
        return this;
    }

    /// <summary>
    ///     Specifies that the property is of decimal type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> AsDecimal()
    {
        Type = typeof(decimal);
        return this;
    }

    /// <summary>
    ///     Specifies that the property is of binary type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> AsBinary()
    {
        Type = typeof(byte[]);
        return this;
    }

    /// <summary>
    ///     Specifies a custom column name for the property in the database.
    /// </summary>
    /// <param name="name">The custom column name.</param>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> WithColumnName(string name)
    {
        ColumnName = name;
        return this;
    }

    /// <summary>
    ///     Specifies that the property is of decimal type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> WithMaxLength(int maxLength)
    {
        MaxLength = maxLength;
        return this;
    }

    //Set a default value for the property. Use HasMappedValue to set a value based on the entity.
    public PropertyTypeBuilder<T> WithValue(object value)
    {
        Value = value;
        return this;
    }

    /// <summary>
    ///     Define when a fi
    /// </summary>
    /// eld will be required during save operations
    /// <param name="isRequired">Default value if true.</param>
    /// <returns></returns>
    public PropertyTypeBuilder<T> IsRequired(bool isRequired = true)
    {
        Required = isRequired;
        return this;
    }

    public PropertyTypeBuilder<T> WithTemplate(string template)
    {
        Template = template;
        return this;
    }

    /// <summary>
    ///     Define a delegate to parse the property.
    /// </summary>
    /// <param name="parserDelegate">The action to parse the property.</param>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> WithMap(Action<T> parserDelegate)
    {
        if (parserDelegate == null) throw new ArgumentNullException(nameof(parserDelegate));
  
        mapActions.Add(parserDelegate);

        return this;
    }

    /// <summary>
    ///     Define a delegate to set the value of the property based on the entity.
    /// </summary>
    /// <param name="valueDelegate"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public PropertyTypeBuilder<T> SetDynamicValue(Func<T, object> valueDelegate)
    {
        setValueDelegate = valueDelegate ?? throw new ArgumentNullException(nameof(valueDelegate));

        return this;
    }

    public void InvokeMaps(T entity)
    {
        if (!HasMapAction)
            return;

        foreach (var action in mapActions) action(entity);
    }

    /// <summary>
    ///     Get the instance value using a fixed value or a delegate.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public object GetValue(T entity)
    {
        if (setValueDelegate != null)
            Value = setValueDelegate(entity);

        Type = Value?.GetType() ?? Type;

        return Value;
    }
}