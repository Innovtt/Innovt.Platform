using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Innovt.AspNetCore.Filters.Swagger;

/// <summary>
/// You can use this class to add custom header to swagger.
/// </summary>
public class AddCustomHeaderParameter: IOperationFilter
{
    private readonly string name;
    private readonly string? description;
    private OpenApiSchema? apiSchema;
    private readonly string schemaType;
    private readonly string schemaFormat;
    private readonly bool required;
    
    public AddCustomHeaderParameter(string name,string? description,bool required, string schemaType="string", string schemaFormat = "uuid")
    { 
        this.name = name ?? throw new ArgumentNullException(nameof(name));
        this.description = description;
        this.schemaType = schemaType;
        this.schemaFormat = schemaFormat;
        this.required = required;
    }
    
    public AddCustomHeaderParameter(string name,string? description,bool required, OpenApiSchema apiSchema)
    { 
        this.name = name ?? throw new ArgumentNullException(nameof(name));
        this.description = description;
        this.apiSchema = apiSchema;
        this.required = required;
    }
    
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if(operation is null)
            return;

        operation.Parameters ??= new List<OpenApiParameter>();

        apiSchema ??= new OpenApiSchema()
        {
            Type = schemaType,
            Format = schemaFormat
        };
        
        operation.Parameters.Add(new OpenApiParameter()
        {
            Name = name,
            Description = description,
            In = new ParameterLocation?(ParameterLocation.Header),
            Schema = apiSchema,
            Required = required
        });
    }
}