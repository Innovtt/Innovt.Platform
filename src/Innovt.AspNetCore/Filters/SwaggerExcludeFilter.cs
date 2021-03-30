using System;
using System.Linq;
using System.Reflection;
using Innovt.Core.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Innovt.AspNetCore.Filters
{
    public class SwaggerExcludeFilter : ISchemaFilter, IOperationFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties.Count == 0)
                return;

            var excludeAttributes = context.Type.GetCustomAttributes<ModelExcludeFilterAttribute>(true)
                .SelectMany(a => a.ExcludeAttributes).ToList();

            if (!excludeAttributes.Any())
                return;

            foreach (var prop in excludeAttributes)
            {
                var schemaProp = schema.Properties.Where(p =>
                        string.Equals(p.Key, prop, StringComparison.InvariantCultureIgnoreCase)).Select(p => p.Key)
                    .SingleOrDefault();

                if (schemaProp != null)
                    schema.Properties.Remove(schemaProp);
            }
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var ignoredProperties = context.MethodInfo.GetCustomAttributes<ModelExcludeFilterAttribute>(true)
                .SelectMany(a => a.ExcludeAttributes).ToList();

            if (!ignoredProperties.Any()) return;


            foreach (var prop in ignoredProperties)
            {
                var schemaProp = operation.Parameters
                    .SingleOrDefault(p => string.Equals(p.Name, prop, StringComparison.InvariantCultureIgnoreCase));

                if (schemaProp != null)
                    operation.Parameters.Remove(schemaProp);
            }
        }
    }
}