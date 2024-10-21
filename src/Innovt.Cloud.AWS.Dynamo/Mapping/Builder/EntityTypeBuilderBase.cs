using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;


public abstract class EntityTypeBuilderBase 
{
    /// <summary>
    ///     Gets or sets the table name associated with the entity type.
    /// </summary>
    public string TableName { get; protected set; }

    /// <summary>
    ///     Gets or sets the partition key for the DynamoDB table.
    /// </summary>
    public string Pk { get; protected set; }

    public string HashKeyPrefix { get; protected set; }

    /// <summary>
    ///     Define the key prefix separator
    /// </summary>
    public string KeySeparator { get; protected set; }

    /// <summary>
    ///     Gets or sets the sort key for the DynamoDB table.
    /// </summary>
    public string Sk { get; protected set; }

    /// <summary>
    ///     Gets or sets the range key prefix for the DynamoDB table.
    /// </summary>
    public string RangeKeyPrefix { get; protected set; }

    /// <summary>
    ///     Gets or sets the entity type for the DynamoDB table.
    /// </summary>
    public abstract string EntityType { get;  set; }

    public virtual string EntityTypeColumnName { get; private set; } = "EntityType";

    public abstract EntityTypeBuilder<TEntity> AutoMap<TEntity>(bool withDefaultKeys = true,
        bool? ignoreNonNativeTypes = null) where TEntity : class;

    public abstract IReadOnlyCollection<PropertyTypeBuilder<TEntity>> GetProperties<TEntity>();

   
    public abstract PropertyTypeBuilder<TEntity> GetProperty<TEntity>(string name);
}