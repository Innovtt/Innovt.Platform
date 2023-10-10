using System;
using Amazon.DynamoDBv2.DocumentModel;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
/// <summary>
/// Represents a builder for defining the properties of an entity type.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class PropertyTypeBuilder<T> where T : class
{
    private DynamoDBEntry entry;
    /// <summary>
    /// Gets a value indicating whether the property is of string type.
    /// </summary>
    internal bool IsString { get; private set; }
    /// <summary>
    /// Gets a value indicating whether the property is of decimal type.
    /// </summary>
    internal bool IsDecimal { get; private set; }
    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    internal string Name { get; private set; }
    /// <summary>
    /// Gets the type of the property.
    /// </summary>
    internal Type Type { get; private set; }
    /// <summary>
    /// Gets or sets the column name associated with the property.
    /// </summary>
    internal string ColumnName { get; private set; }
    /// <summary>
    /// Gets a value indicating whether the property is of binary type.
    /// </summary>
    internal bool IsBinary { get; private set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyTypeBuilder{T}"/> class with a specified property name and type.
    /// </summary>
    /// <param name="propertyName">The function to retrieve the property name.</param>
    /// <param name="propertyType">The type of the property.</param>
    public PropertyTypeBuilder(Func<T, string> propertyName, Type propertyType):this(propertyName)
    {
        Type = propertyType;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyTypeBuilder{T}"/> class with a specified property name.
    /// </summary>
    /// <param name="propertyName">The function to retrieve the property name.</param>
    public PropertyTypeBuilder(Func<T, string> propertyName)
    {
        Name = propertyName.Invoke(default);
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyTypeBuilder{T}"/> class.
    /// </summary>
    public PropertyTypeBuilder()
    {   
    }
    /// <summary>
    /// Specifies that the property is of string type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}"/>.</returns>
    public PropertyTypeBuilder<T> AsString()
    {
        IsString = true;
        return this;
    }
    /// <summary>
    /// Specifies that the property is of decimal type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}"/>.</returns>
    public PropertyTypeBuilder<T> AsDecimal()
    {
        IsDecimal = true;
        return this;
    }
    /// <summary>
    /// Specifies that the property is of binary type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}"/>.</returns>
    public PropertyTypeBuilder<T> AsBinary()
    {
        IsBinary = true;
        return this;
    }
    /// <summary>
    /// Specifies a custom column name for the property in the database.
    /// </summary>
    /// <param name="columnName">The custom column name.</param>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}"/>.</returns>
    public PropertyTypeBuilder<T> HasName(string columnName)
    {
        ColumnName = columnName;
        return this;
    }
    /// <summary>
    /// Specifies a custom converter for the property.
    /// </summary>
    /// <param name="parserDelegate">The action to parse the property.</param>
    /// <returns>The current instance of <see cref="PropertyTypeBuilder{T}"/>.</returns>
    public PropertyTypeBuilder<T> HasConverter(Action<T,object> parserDelegate)
    {
        //Como converter do que vem do banco para o que esta na classe
        
        
        
        return this;
    }
    
}