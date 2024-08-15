using System;
using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

/// <summary>
///     Represents a builder for defining the properties of an entity type.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class PropertyTypeBuilder<T>
{
    
    private List<Action<T>> mapActions = [];
    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyTypeBuilder{T}" /> class with a specified property name and
    ///     type.
    /// </summary>
    /// <param name="propertyName">The function to retrieve the property name.</param>
    /// <param name="propertyType">The type of the property.</param>
    public PropertyTypeBuilder(Func<T, string> propertyName, Type propertyType) : this(propertyName)
    {
        Type = propertyType;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyTypeBuilder{T}" /> class with a specified property name.
    /// </summary>
    /// <param name="propertyName">The function to retrieve the property name.</param>
    public PropertyTypeBuilder(Func<T, string> propertyName)
    {
        Name = propertyName.Invoke(default);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyTypeBuilder{T}" /> class.
    /// </summary>
    public PropertyTypeBuilder()
    {
    }

    /// <summary>
    ///     Gets a value indicating whether the property is of string type.
    /// </summary>
    public bool IsString { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether the property is of decimal type.
    /// </summary>
    public bool IsDecimal { get; private set; }
    
    /// <summary>
    /// Gets a value if the field is required.
    /// </summary>
    public bool Required { get; private set; }
    
    /// <summary>
    /// Has map actions is when you need to map some property.
    /// </summary>
    public bool HasMapAction => mapActions.Count > 0;

    /// <summary>
    ///     Gets the name of the property.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     Gets the type of the property.
    /// </summary>
    public Type Type { get; private set; }

    /// <summary>
    ///     Gets or sets the column name associated with the property.
    /// </summary>
    public string ColumnName { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether the property is of binary type.
    /// </summary>
    public bool IsBinary { get; private set; }
    
    /// <summary>
    /// The max length of the property
    /// </summary>
    public int MaxLength { get; private set; }

    /// <summary>
    ///     Specifies that the property is of string type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> AsString()
    {
        IsString = true;
        return this;
    }

    /// <summary>
    ///     Specifies that the property is of decimal type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> AsDecimal()
    {
        IsDecimal = true;
        return this;
    }

    /// <summary>
    ///     Specifies that the property is of binary type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> AsBinary()
    {
        IsBinary = true;
        return this;
    }

    /// <summary>
    ///     Specifies a custom column name for the property in the database.
    /// </summary>
    /// <param name="columnName">The custom column name.</param>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> HasName(string columnName)
    {
        ColumnName = columnName;
        return this;
    }
    
    /// <summary>
    ///     Specifies that the property is of decimal type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<T> HasMaxLength(int maxLength)
    {
        MaxLength = maxLength;
        return this;
    }

    /// <summary>
    /// Define when a fi
    /// </summary>eld will be required during save operations
    /// <param name="isRequired">Default value if true.</param>
    /// <returns></returns>
    public PropertyTypeBuilder<T> IsRequired(bool isRequired = true)
    {
        Required = isRequired;
        return this;
    }
    
    /// <summary>
    ///     Specifies a custom converter for the property.
    /// </summary>
    /// <param name="parserDelegate">The action to parse the property.</param>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}" />.</returns>
    public void HasMap(Action<T> parserDelegate)
    {
        if (parserDelegate == null) throw new ArgumentNullException(nameof(parserDelegate));

        mapActions.Add(parserDelegate);
    }
    
    public void InvokeMaps(T entity)
    {
        if(!HasMapAction)
            return;
        
        foreach (var action in mapActions)
        {
            action(entity);
        }
    }
    
}