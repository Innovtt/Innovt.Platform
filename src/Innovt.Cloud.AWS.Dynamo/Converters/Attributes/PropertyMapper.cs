using System.Reflection;
using Innovt.Cloud.AWS.Dynamo.Mapping.Builder;

namespace Innovt.Cloud.AWS.Dynamo.Converters.Attributes;

internal static class PropertyMapper
{
    /// <summary>
    ///     Invoke all mapped properties to fill the object
    /// </summary>
    /// <param name="typeBuilder"></param>
    /// <param name="properties"></param>
    /// <param name="instance"></param>
    /// <typeparam name="T"></typeparam>
    public static void InvokeMappedProperties<T>(
        EntityTypeBuilder typeBuilder,
        PropertyInfo[] properties,
        T instance) where T : class
    {
        if (typeBuilder == null || properties.Length == 0 || instance == null)
            return;

        foreach (var property in properties)
        {
            var propertyTypeBuilder = typeBuilder.GetProperty(property.Name) as PropertyBuilder<T>;
            propertyTypeBuilder?.InvokeMaps(instance);
        }
    }
}