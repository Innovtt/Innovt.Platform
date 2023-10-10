using System;
using System.Linq;
using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;
/// <summary>
/// A builder for defining the entity type and its properties for use with DynamoDB.
/// </summary>
/// <typeparam name="T">The type of the entity being defined.</typeparam>
public sealed class EntityTypeBuilder<T> where T : class
{
    /// <summary>
    /// Gets or sets the table name associated with the entity type.
    /// </summary>
    public string TableName { get; private set; }
    /// <summary>
    /// Gets or sets the partition key for the DynamoDB table.
    /// </summary>
    public string Pk { get; private set; }
    /// <summary>
    /// Gets or sets the sort key for the DynamoDB table.
    /// </summary>
    public string Sk { get; private set; }
    /// <summary>
    /// Gets or sets the entity type for the DynamoDB table.
    /// </summary>
    public string EntityType { get; private set; }
    /// <summary>
    /// Gets or sets the list of ignored property names for mapping.
    /// </summary>
    private List<string> IgnoredProperties { get; set; } = new();
    /// <summary>
    /// Gets or sets the list of property type builders for defining properties.
    /// </summary>
    private List<PropertyTypeBuilder<T>> Properties { get; set; } = new();
    /// <summary>
    /// Sets the table name associated with the entity type.
    /// </summary>
    /// <param name="tableName">The table name to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> WithTableName(string tableName)
    {
        TableName = tableName;
        return this;
    }
    /// <summary>
    /// Sets the partition key for the DynamoDB table to "PK".
    /// </summary>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> WithOneTableHashKey() => HasHashKey("PK");
    /// <summary>
    /// Sets the partition key for the DynamoDB table using a provided hash key function.
    /// </summary>
    /// <param name="hashKey">The hash key function to generate the partition key.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> HasHashKey(Func<T, string> hashKey) => HasHashKey(hashKey.Invoke(default));
    /// <summary>
    /// Sets the partition key for the DynamoDB table using a specified partition key.
    /// </summary>
    /// <param name="hashKey">The partition key to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> HasHashKey(string hashKey)
    {
        Pk = hashKey;
        return this;
    }
    /// <summary>
    /// Sets the sort key for the DynamoDB table to "PK".
    /// </summary>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> WithOneTableRangeKey() => HasRangeKey("PK");
    /// <summary>
    /// Sets the sort key for the DynamoDB table using a provided range key function.
    /// </summary>
    /// <param name="rangeKey">The range key function to generate the sort key.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> HasRangeKey(Func<T, string> rangeKey) => HasRangeKey(rangeKey.Invoke(default));
    /// <summary>
    /// Sets the sort key for the DynamoDB table using a specified sort key.
    /// </summary>
    /// <param name="rangeKey">The sort key to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> HasRangeKey(string rangeKey)
    {
        Sk = rangeKey;
        return this;
    }
    /// <summary>
    /// Sets the entity type for the DynamoDB table using a provided entity type function.
    /// </summary>
    /// <param name="entityTypeName">The entity type function to generate the entity type.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> WithEntityType(Func<T, string> entityTypeName) => WithEntityType(entityTypeName.Invoke(default));
    /// <summary>
    /// Sets the entity type for the DynamoDB table using a specified entity type.
    /// </summary>
    /// <param name="entityTypeName">The entity type to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> WithEntityType(string entityTypeName)
    {
        EntityType = entityTypeName;
        return this;
    }
    /// <summary>
    /// Defines a property for the entity using a provided property function.
    /// </summary>
    /// <param name="property">The property function to generate the property.</param>
    /// <returns>The property type builder for further property configuration.</returns>
    public PropertyTypeBuilder<T> WithProperty(Func<T, string> property) => WithProperty(property.Invoke(default));
    /// <summary>
    /// Defines a property for the entity using a specified property name.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>The property type builder for further property configuration.</returns>
    public PropertyTypeBuilder<T> WithProperty(string name)
    {
        var builder = new PropertyTypeBuilder<T>(p=>name);
     
        Properties.Add(builder);
        
        return builder;
    }
    /// <summary>
    /// Ignores a property during mapping.
    /// </summary>
    /// <param name="property">The property to ignore.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> IgnoreProperty(Func<T, string> property)=> IgnoreProperty(property.Invoke(default));
    /// <summary>
    /// Ignores a property by name during mapping.
    /// </summary>
    /// <param name="name">The name of the property to ignore.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> IgnoreProperty(string name)
    {
        if(IgnoredProperties.Any(p=>p.Equals(name, StringComparison.InvariantCultureIgnoreCase)))    
            return this;
        
        IgnoredProperties.Add(name);
        
        return this;
    }

    /// /// <summary>
    /// Starts a reflection process to auto map all properties of the entity type.
    /// </summary>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}"/>.</returns>
    public EntityTypeBuilder<T> AutoMap()
    {
        var item = typeof(T).GetProperties();
        
        foreach (var propertyInfo in item)
        { 
            var prop = new PropertyTypeBuilder<T>(p=>propertyInfo.Name,propertyInfo.GetType());
            
            Properties.Add(prop);
        }
        
        return this;
    }

    
    
    
}