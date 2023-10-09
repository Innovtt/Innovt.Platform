using System;
using System.Linq;
using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

public sealed class EntityTypeBuilder<T> where T : class
{
    public string TableName { get; private set; }
    public string Pk { get; private set; }
    public string Sk { get; private set; }
    public string EntityType { get; private set; }
    private List<string> IgnoredProperties { get; set; } = new();

    private List<PropertyTypeBuilder<T>> Properties { get; set; } = new();

    public EntityTypeBuilder<T> WithTableName(string tableName)
    {
        TableName = tableName;
        return this;
    }

    public EntityTypeBuilder<T> WithOneTableHashKey() => HasHashKey("PK");
    
    public EntityTypeBuilder<T> HasHashKey(Func<T, string> hashKey) => HasHashKey(hashKey.Invoke(default));
    public EntityTypeBuilder<T> HasHashKey(string hashKey)
    {
        Pk = hashKey;
        return this;
    }

    public EntityTypeBuilder<T> WithOneTableRangeKey() => HasRangeKey("PK");
    public EntityTypeBuilder<T> HasRangeKey(Func<T, string> rangeKey) => HasRangeKey(rangeKey.Invoke(default));
    public EntityTypeBuilder<T> HasRangeKey(string rangeKey)
    {
        Sk = rangeKey;
        return this;
    }
    public EntityTypeBuilder<T> WithEntityType(Func<T, string> entityTypeName) => WithEntityType(entityTypeName.Invoke(default));
    public EntityTypeBuilder<T> WithEntityType(string entityTypeName)
    {
        EntityType = entityTypeName;
        return this;
    }
    
    public PropertyTypeBuilder<T> WithProperty(Func<T, string> property) => WithProperty(property.Invoke(default));
    public PropertyTypeBuilder<T> WithProperty(string name)
    {
        var builder = new PropertyTypeBuilder<T>(p=>name);
     
        Properties.Add(builder);
        
        return builder;
    }
    
    public EntityTypeBuilder<T> IgnoreProperty(Func<T, string> property)=> IgnoreProperty(property.Invoke(default));
    public EntityTypeBuilder<T> IgnoreProperty(string name)
    {
        if(IgnoredProperties.Any(p=>p.Equals(name, StringComparison.InvariantCultureIgnoreCase)))    
            return this;
        
        IgnoredProperties.Add(name);
        
        return this;
    }
    
    /// <summary>
    /// Starts a reflection process to auto map all properties
    /// </summary>
    /// <returns></returns>
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