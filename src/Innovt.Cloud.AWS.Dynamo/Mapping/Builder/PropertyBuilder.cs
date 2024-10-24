using System;

namespace Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

public abstract class PropertyBuilder
{
    private string columnName;
    
    public EntityTypeBuilder Builder { get; set; }
    
    /// <summary>
    ///     Gets a value if the field is required.
    /// </summary>
    public bool Required { get; private set; }

    /// <summary>
    ///     Has map actions is when you need to map some property.
    /// </summary>
    public bool HasMapAction{ get; protected set; }

    /// <summary>
    ///     Gets the name of the property.
    /// </summary>
    public string Name { get; protected set;}

    /// <summary>
    ///     Get the value of the property.
    /// </summary>
    protected object Value { get; set; }

    /// <summary>
    ///     Gets the type of the property.
    /// </summary>
    protected Type Type { get; set; }

    /// <summary>
    ///     Gets or sets the column name associated with the property. if the column name is not set, the property name is
    ///     used.
    /// </summary>
    public string ColumnName
    {
        get => columnName ?? Name;
        private set => columnName = value;
    }

    public void Ignore()
    {
        Ignored = true;
    }

    public bool Ignored { get; private set; }

    /// <summary>
    ///     The max length of the property
    /// </summary>
    public int MaxLength { get; private set; }

    /// <summary>
    ///     Specifies a custom column name for the property in the database.
    /// </summary>
    /// <param name="name">The custom column name.</param>
    /// <returns>The current instance of <see cref="PropertyBuilder" />.</returns>
    protected PropertyBuilder HasColumnName(string name)
    {
        ColumnName = name;
        return this;
    }

    /// <summary>
    ///     Specifies that the property is of decimal type.
    /// </summary>
    /// <returns>The current instance of <see cref="PropertyBuilder" />.</returns>
    protected PropertyBuilder HasMaxLength(int maxLength)
    {
        MaxLength = maxLength;
        return this;
    }

    /// <summary>
    ///     It set a value for property.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected PropertyBuilder HasDefaultValue(object value)
    {
        Value = value;
        return this;
    }   

    /// <summary>
    ///     Define when a fi
    /// </summary>
    /// eld will be required during save operations
    /// <param name="isRequired">Default value if true.</param>
    /// <returns></returns>
    protected PropertyBuilder IsRequired(bool isRequired = true)
    {
        Required = isRequired;
        return this;
    }

    /// <summary>
    ///     Define a delegate to parse the property.
    /// </summary>
    /// <param name="actionMap">The action to parse the property.</param>
    /// <returns>The current instance of <see cref="PropertyBuilder" />.</returns>
    //public abstract PropertyBuilder WithMap<T>(Action<T> actionMap);
    
    /// <summary>
    ///     Define a delegate to set the value of the property based on the entity.
    /// </summary>
    /// <param name="valueDelegate"></param>
    /// <exception cref="ArgumentNullException"></exception>
    //public abstract PropertyBuilder SetDynamicValue<T>(Func<T, object> valueDelegate);

    /// <summary>
    ///     Get the instance value using a fixed value or a delegate.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    //public abstract object GetValue<T>(T entity);
    //public abstract PropertyBuilder InvokeMaps<T>(T entity);
}