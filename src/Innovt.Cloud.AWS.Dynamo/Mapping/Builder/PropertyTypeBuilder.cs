using System;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

public class PropertyTypeBuilder<T> where T : class
{
    internal bool IsString { get; private set; }
    
    internal bool IsDecimal { get; private set; }
    
    internal string Name { get; private set; }
    internal Type Type { get; private set; }
    
    internal string ColumnName { get; private set; }
    
    internal bool IsBinary { get; private set; }

    public PropertyTypeBuilder(Func<T, string> propertyName, Type propertyType):this(propertyName)
    {
        Type = propertyType;
    }
    
    public PropertyTypeBuilder(Func<T, string> propertyName)
    {
        Name = propertyName.Invoke(default);
    }
    
    public PropertyTypeBuilder()
    {   
    }
    
    public PropertyTypeBuilder<T> AsString()
    {
        IsString = true;
        return this;
    }
    
    public PropertyTypeBuilder<T> AsDecimal()
    {
        IsDecimal = true;
        return this;
    }
    
    public PropertyTypeBuilder<T> AsBinary()
    {
        IsBinary = true;
        return this;
    }
    public PropertyTypeBuilder<T> HasName(string columnName)
    {
        ColumnName = columnName;
        return this;
    }

    public PropertyTypeBuilder<T> HasConverter(Action<T,object> parserDelegate)
    {
        //Como converter do que vem do banco para o que esta na classe
        
        
        
        return this;
    }
    
}