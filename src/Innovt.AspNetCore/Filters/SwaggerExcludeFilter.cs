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
public class SwaggerExcludeFilter : ISchemaFilter, IOperationFilter,IOperationAsyncFilter
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SwaggerExcludeFilter" /> class.
    /// </summary>
    public SwaggerExcludeFilter()
    {   
    }
    
    /// <summary>
    /// Apply the filter to the schem and remove the excluded properties.
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(context);

        if (schema.Properties.Count == 0)
            return;

        var excludeAttributes = context.Type.GetCustomAttributes<ModelExcludeFilterAttribute>(true)
            .SelectMany(a => a.ExcludeAttributes).ToList();

        RemoveParameterFromSchema(excludeAttributes, schema);
    }
    
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        RemoveExcludedParametersFromOperation(operation, context);
    }
    
    
    public Task ApplyAsync(OpenApiOperation operation, OperationFilterContext context, CancellationToken cancellationToken)
    {
        RemoveExcludedParametersFromOperation(operation, context);

        return Task.CompletedTask;
    }
    
    
    private static void RemoveExcludedParametersFromOperation(OpenApiOperation operation, OperationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(context);
        

        // Get excluded properties from both controller and method
        var methodExclusions = context.MethodInfo
            .GetCustomAttributes<ModelExcludeFilterAttribute>(true)
            .SelectMany(a => a.ExcludeAttributes);

        var controllerExclusions = context.MethodInfo.DeclaringType?
            .GetCustomAttributes<ModelExcludeFilterAttribute>(true)
            .SelectMany(a => a.ExcludeAttributes) ?? [];

        // Combine and get unique exclusions
        var ignoredProperties = methodExclusions
            .Concat(controllerExclusions)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        
        if (ignoredProperties.Count == 0) return;
        
        foreach (var prop in ignoredProperties)
        {   
            var schemaProp = context.ApiDescription.ParameterDescriptions
                .SingleOrDefault(p => string.Equals(p.Name, prop, StringComparison.OrdinalIgnoreCase));

            if (schemaProp != null)
                context.ApiDescription.ParameterDescriptions.Remove(schemaProp);
        }
    }
    
    private static void RemoveParameterFromSchema(List<string> allExclusions, OpenApiSchema schema)
    {
        
        foreach (var prop in allExclusions)
        {
            var schemaProp = schema.Properties.Where(p =>
                    string.Equals(p.Key, prop, StringComparison.OrdinalIgnoreCase)).Select(p => p.Key)
                .SingleOrDefault();

            if (schemaProp != null)
                schema.Properties.Remove(schemaProp);
        }
    }

}