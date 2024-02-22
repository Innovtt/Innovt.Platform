// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.Reflection;
using Innovt.Core.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Innovt.AspNetCore.Filters;

/// <summary>
///     A filter used to exclude specified properties or parameters from Swagger documentation.
/// </summary>
public class SwaggerExcludeFilter : ISchemaFilter, IOperationFilter
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SwaggerExcludeFilter" /> class.
    /// </summary>
    public SwaggerExcludeFilter()
    {
    }

    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(context);

        var ignoredProperties = context.MethodInfo.GetCustomAttributes<ModelExcludeFilterAttribute>(true)
            .SelectMany(a => a.ExcludeAttributes).ToList();

        if (ignoredProperties.Count == 0) return;


        foreach (var prop in ignoredProperties)
        {
            // var schemaProp = operation.Parameters
            //   .SingleOrDefault(p => string.Equals(p.Name, prop, StringComparison.OrdinalIgnoreCase));
            var schemaProp = context.ApiDescription.ParameterDescriptions
                .SingleOrDefault(p => string.Equals(p.Name, prop, StringComparison.OrdinalIgnoreCase));

            if (schemaProp != null)
                context.ApiDescription.ParameterDescriptions.Remove(schemaProp);

            //operation.Parameters.Remove(schemaProp);
        }
    }

    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(context);

        if (schema.Properties.Count == 0)
            return;

        var excludeAttributes = context.Type.GetCustomAttributes<ModelExcludeFilterAttribute>(true)
            .SelectMany(a => a.ExcludeAttributes).ToList();

        if (excludeAttributes.Count == 0)
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