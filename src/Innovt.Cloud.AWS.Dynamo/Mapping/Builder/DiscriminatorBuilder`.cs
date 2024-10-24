namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

/// <summary>
///  Represents a discriminator builder for defining the properties of an entity type.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class DiscriminatorBuilder<TEntity>(string name, EntityTypeBuilder<TEntity> builder):DiscriminatorBuilder(name, builder)
{
    public new EntityTypeBuilder<TEntity> Builder => (EntityTypeBuilder<TEntity>)base.Builder;

    /// <summary>
    /// Simple case of a class that can inherit from another class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public new DiscriminatorBuilder<TEntity> HasValue<T>() where T : TEntity, new() =>
        (DiscriminatorBuilder<TEntity>)base.HasValue<T>();
    
    /// <summary>
    /// Use this method when you need to define a value for a specific type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public new DiscriminatorBuilder<TEntity> HasValue<T>(T value) where T : TEntity,new() => 
        (DiscriminatorBuilder<TEntity>)base.HasValue(value);
  
    /// <summary>
    /// Use this method when you have a type T with a parameterless constructor and will be created when matched the whenValue parameter
    /// </summary>
    /// <param name="whenValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public new DiscriminatorBuilder<TEntity> HasValue<T>(string whenValue) where T : TEntity, new() =>
        (DiscriminatorBuilder<TEntity>)base.HasValue<T>(whenValue);
    
    /// <summary>
    /// Use this method when you have a type T with a parameterless constructor and will be created when matched the whenValue parameter
    /// </summary>
    /// <param name="whenValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public new DiscriminatorBuilder<TEntity> HasValue<T>(int whenValue) where T : TEntity, new() =>
        (DiscriminatorBuilder<TEntity>)base.HasValue<T>(whenValue);
    
    /// <summary>
    /// Use this method when you want to define a value for a specific type and a specific value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="whenValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public new DiscriminatorBuilder<TEntity> HasValue<T>(T value, string whenValue) where T : TEntity,new()=>
        (DiscriminatorBuilder<TEntity>)base.HasValue(value, whenValue);
    
    /// <summary>
    /// Use this method when you want to define a value for a specific type and a specific value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="whenValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public new DiscriminatorBuilder<TEntity> HasValue<T>(T value, int whenValue) where T : TEntity,new()=>
        (DiscriminatorBuilder<TEntity>)base.HasValue(value, whenValue);
}