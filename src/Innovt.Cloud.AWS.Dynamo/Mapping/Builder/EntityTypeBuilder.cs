using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

/// <summary>
///     A builder for defining the entity type and its properties for use with DynamoDB.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being defined.</typeparam>
public sealed class EntityTypeBuilder<TEntity> //where TEntity:class
{
    /// <summary>
    ///     Gets or sets the table name associated with the entity type.
    /// </summary>
    public string TableName { get; private set; }

    /// <summary>
    ///     Gets or sets the partition key for the DynamoDB table.
    /// </summary>
    public string Pk { get; private set; }

    public string HashKeyPrefix { get; private set; }

    /// <summary>
    ///     Define the key prefix separator
    /// </summary>
    public string KeySeparator { get; private set; }

    /// <summary>
    ///     Gets or sets the sort key for the DynamoDB table.
    /// </summary>
    public string Sk { get; private set; }

    /// <summary>
    ///     Gets or sets the range key prefix for the DynamoDB table.
    /// </summary>
    public string RangeKeyPrefix { get; private set; }

    /// <summary>
    ///     Gets or sets the entity type for the DynamoDB table.
    /// </summary>
    public string EntityType { get; private set; } = typeof(TEntity).Name;

    /// <summary>
    ///     Gets or sets the list of property type builders for defining properties.
    /// </summary>
    private List<PropertyTypeBuilder<TEntity>> Properties { get; } = [];

    /// <summary>
    ///     Sets the table name associated with the entity type.
    /// </summary>
    /// <param name="tableName">The table name to set.</param>
    /// <param name="keySeparator">If you want to define a key prefix like USER#ID the separator will be #</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public EntityTypeBuilder<TEntity> WithTableName(string tableName, string keySeparator = null)
    {
        TableName = tableName;
        KeySeparator = keySeparator;
        return this;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityTypeBuilder{TEntity}" /> class.
    /// In this case the default PK and SK will be created
    /// </summary>
    /// <returns></returns>
    public EntityTypeBuilder<TEntity> WithDefaultKeys()
    {
        WithHashKey();
        WithRangeKey();
        return this;
    }

    /// <summary>
    ///     Sets the partition key for the DynamoDB table using a provided hash key function.
    /// </summary>
    /// <param name="expression">The hash key function to generate the partition key.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<TEntity> WithHashKey<TProperty>(Expression<Func<TEntity, TProperty>> expression)
    {
        return WithHashKey(GetPropertyName(expression));
    }

    /// <summary>
    ///     Sets the partition key for the DynamoDB table using a specified partition key.
    /// </summary>
    /// <param name="hashKey">The partition key to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<TEntity> WithHashKey(string hashKey = "PK")
    {
        Pk = hashKey;
        return Property(Pk);
    }
    
    public EntityTypeBuilder<TEntity> WithHashKeyPrefix(string hashKeyPrefix)
    {
        HashKeyPrefix = hashKeyPrefix;
        return this;
    }

    /// <summary>
    ///     Sets the sort key for the DynamoDB table using a provided sort key function.
    /// </summary>
    /// <param name="expression">The sort key function to generate the sort key.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<TEntity> WithRangeKey<TProperty>(Expression<Func<TEntity, TProperty>> expression)
    {
        return WithRangeKey(GetPropertyName(expression));
    }

    /// <summary>
    ///     Sets the range key for the DynamoDB table using a specified sort key.
    /// </summary>
    /// <param name="rangeKey">The sort key to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public PropertyTypeBuilder<TEntity> WithRangeKey(string rangeKey = "SK")
    {
        Sk = rangeKey;
        return Property(Sk);
    }

    /// <summary>
    ///     Sets the sort/Range key prefix for the DynamoDB table.
    /// </summary>
    /// <param name="rangeKeyPrefix"></param>
    /// <returns></returns>
    public EntityTypeBuilder<TEntity> WithRangeKeyPrefix(string rangeKeyPrefix)
    {
        RangeKeyPrefix = rangeKeyPrefix;
        return this;
    }

    /// <summary>
    ///     Sets the entity type for the DynamoDB table using a provided entity type function.
    /// </summary>
    /// <param name="expression">The entity type function to generate the entity type.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public EntityTypeBuilder<TEntity> WithEntityType<TProperty>(Expression<Func<TEntity, TProperty>> expression)
    {
        return WithEntityType(GetPropertyName(expression));
    }

    /// <summary>
    ///     Sets the entity type for the DynamoDB table using a specified entity type.
    /// </summary>
    /// <param name="entityTypeName">The entity type to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public EntityTypeBuilder<TEntity> WithEntityType(string entityTypeName)
    {
        EntityType = entityTypeName;
        return this;
    }

    /// <summary>
    ///     Defines a property for the entity using a provided property function.
    /// </summary>
    /// <param name="propertyExpression">The property function to generate the property.</param>
    /// <returns>The property type builder for further property configuration.</returns>
    public PropertyTypeBuilder<TEntity> Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
    {
        return Property(GetPropertyName(propertyExpression));
    }

    /// <summary>
    ///     Defines a property for the entity using a specified property name.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>The property type builder for further property configuration.</returns>
    public PropertyTypeBuilder<TEntity> Property(string name)
    {
        return AddProperty(name);
    }
    
    private PropertyTypeBuilder<TEntity> AddProperty(string name, Type type=null)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        var currentProperty =
            Properties.SingleOrDefault(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        if (currentProperty != null)
            return currentProperty;

        var builder = new PropertyTypeBuilder<TEntity>(p => name, type, this);

        Properties.Add(builder);

        return builder;
    }

    private static string GetPropertyName<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
    {
        Check.NotNull(propertyExpression, nameof(propertyExpression));

        if (propertyExpression.Body is MemberExpression memberExpression) return memberExpression.Member.Name;

        throw new ArgumentException("The expression is not a member access expression.", nameof(propertyExpression));
    }

    /// <summary>
    ///     Ignores a property during mapping.
    /// </summary>
    /// <param name="expression">The property to ignore.</param>
    public EntityTypeBuilder<TEntity> Ignore<TProperty>(Expression<Func<TEntity, TProperty>> expression)
    {
        return Ignore(GetPropertyName(expression));
    }

    /// <summary>
    ///     Ignores a property by name during mapping.
    /// </summary>
    /// <param name="name">The name of the property to ignore.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public EntityTypeBuilder<TEntity> Ignore(string name)
    {
        var property = GetProperty(name);

        if (property is not null)
            Properties.Remove(property);

        return this;
    }

    /// ///
    /// <summary>
    ///     Starts a reflection process to auto map all properties of the entity type.
    /// </summary>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public EntityTypeBuilder<TEntity> AutoMap(bool withDefaultKeys = true)
    {
        var entityType = typeof(TEntity);
        
        //Set the table name as the entity name
        TableName = entityType.Name;

        var properties = entityType.GetProperties(
            BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var propertyInfo in properties)
        {
            AddProperty(propertyInfo.Name, propertyInfo.GetType());
        }
        
        if (withDefaultKeys)
            WithDefaultKeys();

        return this;
    }

    /// <summary>
    ///     Returns all properties defined for the entity type.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyCollection<PropertyTypeBuilder<TEntity>> GetProperties()
    {
        return Properties.AsReadOnly();
    }

    /// <summary>
    ///     Gets a property by its column name or Name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public PropertyTypeBuilder<TEntity> GetProperty(string name)
    {
        var property = Properties?.SingleOrDefault(p =>
                           p.Name != null && p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) ??
                       Properties?.SingleOrDefault(p =>
                           p.ColumnName != null &&
                           p.ColumnName.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        return property;
    }
}