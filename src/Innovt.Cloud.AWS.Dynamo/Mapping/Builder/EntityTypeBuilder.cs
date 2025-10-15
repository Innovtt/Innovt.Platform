using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

[SuppressMessage("Design", "CA1002:Do not expose generic lists")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public abstract class EntityTypeBuilder
{
    protected EntityTypeBuilder(bool ignoreNonNativeTypes) : this()
    {
        IgnoreNonNativeTypes = ignoreNonNativeTypes;
    }

    //Keep it for reflection
    protected EntityTypeBuilder()
    {
    }

    /// <summary>
    ///     Gets or sets the table name associated with the entity type.
    /// </summary>
    public string TableName { get; protected set; }

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

    public string EntityType { get; private set; }
    public string EntityTypeColumnName { get; set; } = "EntityType";

    /// <summary>
    ///     Tell the auto-map method to ignore all non-native types
    /// </summary>
    protected bool IgnoreNonNativeTypes { get; }

    /// <summary>
    /// Internal list of properties.
    /// </summary>
    protected List<PropertyBuilder> PropertyBuilders { get; private set; } = [];

    public DiscriminatorBuilder? Discriminator { get; protected set; }

    /// <summary>
    ///     Sets the table name associated with the entity type.
    /// </summary>
    /// <param name="tableName">The table name to set.</param>
    /// <param name="keySeparator">If you want to define a key prefix like USER#ID the separator will be #</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public EntityTypeBuilder HasTableName(string tableName, string keySeparator = null)
    {
        TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        KeySeparator = keySeparator;
        return this;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityTypeBuilder{TEntity}" /> class.
    ///     In this case the default PK and SK will be created
    /// </summary>
    /// <returns></returns>
    public EntityTypeBuilder HasDefaultKeys()
    {
        HasHashKey();
        HasRangeKey();
        return this;
    }

    /// <summary>
    ///     Sets the partition key for the DynamoDB table using a specified partition key.
    /// </summary>
    /// <param name="hashKey">The partition key to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public PropertyBuilder HasHashKey(string hashKey = "PK")
    {
        Pk = hashKey ?? throw new ArgumentNullException(nameof(hashKey));
        return Property(Pk);
    }

    public EntityTypeBuilder HasHashKeyPrefix(string hashKeyPrefix)
    {
        HashKeyPrefix = hashKeyPrefix ?? throw new ArgumentNullException(nameof(hashKeyPrefix));
        return this;
    }

    /// <summary>
    ///     Sets the range key for the DynamoDB table using a specified sort key.
    /// </summary>
    /// <param name="rangeKey">The sort key to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public PropertyBuilder HasRangeKey(string rangeKey = "SK")
    {
        Sk = rangeKey ?? throw new ArgumentNullException(nameof(rangeKey));
        return Property(Sk);
    }

    /// <summary>
    ///     Sets the sort/Range key prefix for the DynamoDB table.
    /// </summary>
    /// <param name="rangeKeyPrefix"></param>
    /// <returns></returns>
    public EntityTypeBuilder HasRangeKeyPrefix(string rangeKeyPrefix)
    {
        RangeKeyPrefix = rangeKeyPrefix ?? throw new ArgumentNullException(nameof(rangeKeyPrefix));
        return this;
    }

    /// <summary>
    ///     Sets the entity type for the DynamoDB table using a specified entity type.
    /// </summary>
    /// <param name="entityTypeName">The entity type to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public EntityTypeBuilder HasEntityType(string entityTypeName)
    {
        EntityType = entityTypeName ?? throw new ArgumentNullException(nameof(entityTypeName));
        return this;
    }

    /// <summary>
    ///     The default entity type column name is EntityType. This is used to split the entities in the same table.
    /// </summary>
    /// <param name="entityTypeColumnName">The name of your customized entity type column</param>
    /// <returns></returns>
    public EntityTypeBuilder HasEntityTypeColumnName(string entityTypeColumnName = "EntityType")
    {
        EntityTypeColumnName = entityTypeColumnName ?? throw new ArgumentNullException(nameof(entityTypeColumnName));
        return this;
    }

    /// <summary>
    ///     Defines a property for the entity using a specified property name.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>The property type builder for further property configuration.</returns>
    public PropertyBuilder Property(string name)
    {
        return AddProperty(name);
    }

    protected abstract PropertyBuilder AddProperty(string name, Type type = null);

    protected static string GetPropertyName<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
    {
        Check.NotNull(propertyExpression, nameof(propertyExpression));

        if (propertyExpression.Body is MemberExpression memberExpression) return memberExpression.Member.Name;

        throw new ArgumentException("The expression is not a member access expression.", nameof(propertyExpression));
    }

    /// <summary>
    ///     Ignores a property during mapping.
    /// </summary>
    /// <param name="expression">The property to ignore.</param>
    public EntityTypeBuilder Ignore<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return Ignore(GetPropertyName(expression));
    }

    /// <summary>
    ///     Ignores a property by name during mapping.
    /// </summary>
    /// <param name="name">The name of the property to ignore.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public EntityTypeBuilder Ignore(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        var property = GetProperty(name);

        property?.Ignore();

        return this;
    }

    /// <summary>
    ///     Include a property that was ignored during a map.
    /// </summary>
    /// <param name="expression">The property to include.</param>
    public EntityTypeBuilder Include<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return Include(GetPropertyName(expression));
    }

    /// <summary>
    ///   Include a property that was ignored during a map.
    /// </summary>
    /// <param name="name">The name of the property to include.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public EntityTypeBuilder Include(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        var property = GetProperty(name);

        property?.Include();

        return this;
    }

    /// <summary>
    ///   Check if complex types should be ignored. The default is true and can be changed by the user for each entity.
    /// </summary>
    /// <param name="ignoreNonNativeTypes"></param>
    /// <returns></returns>
    protected bool ShouldIgnoreNonNativeTypes(bool? ignoreNonNativeTypes)
    {
        return ignoreNonNativeTypes ?? IgnoreNonNativeTypes;
    }

    /// <summary>
    ///     Returns all properties defined for the entity type.
    /// </summary>
    /// <returns></returns>
    public List<PropertyBuilder> GetProperties()
    {
        return PropertyBuilders.Where(p => !p.Ignored).ToList();
    }

    /// <summary>
    ///     Gets a property by its column name or Name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public PropertyBuilder GetProperty(string name)
    {
        var property = PropertyBuilders?.SingleOrDefault(p =>
                           p.Name != null && p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ??
                       PropertyBuilders?.SingleOrDefault(p =>
                           p.ColumnName != null &&
                           p.ColumnName.Equals(name, StringComparison.OrdinalIgnoreCase));

        return property;
    }
}