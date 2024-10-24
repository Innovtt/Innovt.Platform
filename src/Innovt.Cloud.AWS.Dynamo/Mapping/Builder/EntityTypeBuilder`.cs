using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

/// <summary>
///     A builder for defining the entity type and its properties for use with DynamoDB.
/// </summary>
/// <typeparam name="T">The type of the entity being defined.</typeparam>
public sealed class EntityTypeBuilder<T> : EntityTypeBuilder
{   
    public EntityTypeBuilder(bool ignoreNonNativeTypes) : base(ignoreNonNativeTypes){}
   
    public EntityTypeBuilder(){}
    
    /// <summary>
    /// When typed the default value of the entity type will be the name of the entity
    /// </summary>
    public new string EntityType { get; private set; } = typeof(T).Name.ToUpper(CultureInfo.InvariantCulture);
    
    public DiscriminatorBuilder<T> Discriminator { get; private set; }
    
    /// <summary>
    ///     Sets the table name associated with the entity type.
    /// </summary>
    /// <param name="tableName">The table name to set.</param>
    /// <param name="keySeparator">If you want to define a key prefix like USER#ID the separator will be #</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public new EntityTypeBuilder<T> HasTableName(string tableName, string keySeparator = null) => (EntityTypeBuilder<T>)base.HasTableName(tableName, keySeparator);
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="EntityTypeBuilder{TEntity}" /> class.
    ///     In this case the default PK and SK will be created
    /// </summary>
    /// <returns></returns>
    public new EntityTypeBuilder<T> HasDefaultKeys() => (EntityTypeBuilder<T>)base.HasDefaultKeys();

    /// <summary>
    ///     Sets the partition key for the DynamoDB table using a provided hash key function.
    /// </summary>
    /// <param name="expression">The hash key function to generate the partition key.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public PropertyBuilder<T> HasHashKey<TProperty>(Expression<Func<T, TProperty>> expression) => 
        (PropertyBuilder<T>) base.HasHashKey(EntityTypeBuilder.GetPropertyName(expression));

    /// <summary>
    ///     Sets the partition key for the DynamoDB table using a specified partition key.
    /// </summary>
    /// <param name="hashKey">The partition key to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public new PropertyBuilder<T> HasHashKey(string hashKey = "PK") => (PropertyBuilder<T>) base.HasHashKey(hashKey);

    public new EntityTypeBuilder<T> HasHashKeyPrefix(string hashKeyPrefix)=> (EntityTypeBuilder<T>)base.HasHashKeyPrefix(hashKeyPrefix);
    
    /// <summary>
    ///     Sets the sort key for the DynamoDB table using a provided sort key function.
    /// </summary>
    /// <param name="expression">The sort key function to generate the sort key.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public new PropertyBuilder<T> HasRangeKey<TProperty>(Expression<Func<T, TProperty>> expression)=>
        (PropertyBuilder<T>) base.HasRangeKey(EntityTypeBuilder.GetPropertyName(expression));
    
    /// <summary>
    ///     Sets the range key for the DynamoDB table using a specified sort key.
    /// </summary>
    /// <param name="rangeKey">The sort key to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public new PropertyBuilder<T> HasRangeKey(string rangeKey = "SK") => (PropertyBuilder<T>) base.HasRangeKey(rangeKey);

    /// <summary>
    ///     Sets the sort/Range key prefix for the DynamoDB table.
    /// </summary>
    /// <param name="rangeKeyPrefix"></param>
    /// <returns></returns>
    public new EntityTypeBuilder<T> HasRangeKeyPrefix(string rangeKeyPrefix) => (EntityTypeBuilder<T>)base.HasRangeKeyPrefix(rangeKeyPrefix);
    
    /// <summary>
    ///     Sets the entity type for the DynamoDB table using a specified entity type.
    /// </summary>
    /// <param name="entityTypeName">The entity type to set.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public new EntityTypeBuilder<T> HasEntityType(string entityTypeName) => (EntityTypeBuilder<T>)base.HasEntityType(entityTypeName);

    /// <summary>
    ///     The default entity type column name is EntityType. This is used to split the entities in the same table.
    /// </summary>
    /// <param name="entityTypeColumnName">The name of your customized entity type column</param>
    /// <returns></returns>
    public new EntityTypeBuilder<T> HasEntityTypeColumnName(string entityTypeColumnName = "EntityType")=> (EntityTypeBuilder<T>)base.HasEntityTypeColumnName(entityTypeColumnName);
    
    /// <summary>
    /// Define a discriminator for the entity. Entities with discriminator mean that more than one type can be inherited from the same type.
    /// </summary>
    /// <param name="name">The name of the column that will be used to check the value.</param>
    /// <returns></returns>
    public DiscriminatorBuilder<T> HasDiscriminator(string name)
    {
        Discriminator ??= new DiscriminatorBuilder<T>(name,this);
        return Discriminator;
    }
    /// <summary>
    ///     Defines a property for the entity using a provided property function.
    /// </summary>
    /// <param name="propertyExpression">The property function to generate the property.</param>
    /// <returns>The property type builder for further property configuration.</returns>
    public PropertyBuilder<T> Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression)=>
        (PropertyBuilder<T>) base.Property(GetPropertyName<T,TProperty>(propertyExpression));
    
    /// <summary>
    ///     Defines a property for the entity using a specified property name.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>The property type builder for further property configuration.</returns>
    public new PropertyBuilder<T> Property(string name)=>
        (PropertyBuilder<T>) base.Property(name);
    
    protected override PropertyBuilder<T> AddProperty(string name, Type type = null)
    {
        ArgumentNullException.ThrowIfNull(name);

        var currentProperty =
            PropertyBuilders.SingleOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (currentProperty != null)
            return (PropertyBuilder<T>)currentProperty;

        var builder = new PropertyBuilder<T>(p => name, type, this);

        PropertyBuilders.Add(builder);

        return builder;
    }

    private static string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        Check.NotNull(propertyExpression, nameof(propertyExpression));

        if (propertyExpression.Body is MemberExpression memberExpression) return memberExpression.Member.Name;

        throw new ArgumentException("The expression is not a member access expression.", nameof(propertyExpression));
    }

    /// <summary>
    ///     Ignores a property during mapping.
    /// </summary>
    /// <param name="expression">The property to ignore.</param>
    public EntityTypeBuilder<T> Ignore<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        return Ignore(GetPropertyName(expression));
    }

    /// <summary>
    ///     Ignores a property by name during mapping.
    /// </summary>
    /// <param name="name">The name of the property to ignore.</param>
    /// <returns>The current instance of <see cref="EntityTypeBuilder{T}" />.</returns>
    public new EntityTypeBuilder<T> Ignore(string name) => (EntityTypeBuilder<T>)base.Ignore(name);
    

    /// <summary>
    ///     Starts a reflection process to auto map all properties of the entity type.
    /// </summary>
    /// <param name="withDefaultKeys">Default keys are PK and SK</param>
    /// <param name="ignoreNonNativeTypes">Ignore all complex types</param>
    /// <returns></returns>
    public EntityTypeBuilder<T> AutoMap(bool withDefaultKeys = true, bool? ignoreNonNativeTypes = null)
    {
        var entityType = typeof(T);

        //Set the table name as the entity name
        TableName = entityType.Name;

        var properties = entityType.GetProperties(
            BindingFlags.Public | BindingFlags.Instance);

        foreach (var propertyInfo in properties)
        {
            var property = AddProperty(propertyInfo.Name, propertyInfo.GetType());
            
            var ignoreProperty = !TypeUtil.IsPrimitive(propertyInfo.PropertyType) && ShouldIgnoreNonNativeTypes(ignoreNonNativeTypes);
            
            if (ignoreProperty)
                property.Ignore();
        }

        if (withDefaultKeys)
            HasDefaultKeys();

        //Set the entity type as the entity name
        if (EntityTypeColumnName.IsNotNullOrEmpty())
            Property(EntityTypeColumnName).SetDynamicValue(p => EntityType);

        return this;
    }
    
    public new PropertyBuilder<T> GetProperty(string name)=> (PropertyBuilder<T>)base.GetProperty(name);
}