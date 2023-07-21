// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.Reflection;
using Innovt.Core.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Innovt.AspNetCore.Filters;

public class SwaggerExcludeFilter : ISchemaFilter, IOperationFilter
{
    public SwaggerExcludeFilter()
    {
        
    }
    
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        if (context == null) throw new ArgumentNullException(nameof(context));

        var ignoredProperties = context.MethodInfo.GetCustomAttributes<ModelExcludeFilterAttribute>(true)
            .SelectMany(a => a.ExcludeAttributes).ToList();

        if (!ignoredProperties.Any()) return;


        foreach (var prop in ignoredProperties)
        {
            var schemaProp = operation.Parameters
                .SingleOrDefault(p => string.Equals(p.Name, prop, StringComparison.OrdinalIgnoreCase));

            if (schemaProp != null)
                operation.Parameters.Remove(schemaProp);
        }
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema == null) throw new ArgumentNullException(nameof(schema));
        if (context == null) throw new ArgumentNullException(nameof(context));

        if (schema.Properties.Count == 0)
            return;

        var excludeAttributes = context.Type.GetCustomAttributes<ModelExcludeFilterAttribute>(true)
            .SelectMany(a => a.ExcludeAttributes).ToList();

        if (!excludeAttributes.Any())
            return;

        foreach (var prop in excludeAttributes)
        {
            var schemaProp = schema.Properties.Where(p =>
                    string.Equals(p.Key, prop, StringComparison.OrdinalIgnoreCase)).Select(p => p.Key)
                .SingleOrDefault();

            if (schemaProp != null)
                schema.Properties.Remove(schemaProp);
        }
    }
}