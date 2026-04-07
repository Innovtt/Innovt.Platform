using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Innovt.AspNetCore.Filters.Swagger;

public class ApiGatewayDocumentFilter(
    bool hasCognitoAuth,
    string accessControlAllowedHeaders = "Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token",
    string accessControlAllowedOrigin = "*")
    : IDocumentFilter
{
    private bool HasCognitoAuth { get; } = hasCognitoAuth;
    private string AccessControlAllowedHeaders { get; } = accessControlAllowedHeaders;
    private string AccessControlAllowedOrigin { get; } = accessControlAllowedOrigin;
    

    public ApiGatewayDocumentFilter() : this(false, "Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token", "*")
    {
    }
    
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {   
        AddCognitoSecurityScheme(swaggerDoc);
        
        AddGlobalSecurityRequirement(swaggerDoc);
        
        AddCorsOptionsMethods(swaggerDoc);
    }

    protected virtual void AddCognitoSecurityScheme(OpenApiDocument? doc)
    {
        if(!HasCognitoAuth || doc is null)
            return;
            
      
        var cognitoScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = ParameterLocation.Header
        };

        cognitoScheme.Extensions ??= new Dictionary<string, IOpenApiExtension>();
        cognitoScheme.Extensions["x-amazon-apigateway-authtype"] = new JsonNodeExtension(JsonValue.Create("cognito_user_pools"));

        cognitoScheme.Extensions["x-amazon-apigateway-authorizer"] = new JsonNodeExtension(new JsonObject
        {
            ["type"] = "cognito_user_pools",
            ["providerARNs"] = new JsonArray
            {
                new JsonObject
                {
                    ["Fn::Sub"] =
                        "arn:aws:cognito-idp:${AWS::Region}:${AWS::AccountId}:userpool/${CognitoUserPoolId}"
                }
            }
        });

        doc.Components ??= new OpenApiComponents();
        doc.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        doc.Components.SecuritySchemes["CognitoAuth"] = cognitoScheme;
    }

    protected virtual void AddGlobalSecurityRequirement(OpenApiDocument? doc)
    {
        if(!HasCognitoAuth || doc is null)
            return;
        
        var requirement = new OpenApiSecurityRequirement();
        var schemeRef = new OpenApiSecuritySchemeReference("CognitoAuth", doc);
        requirement.Add(schemeRef, []);
        
        doc.Security ??= new List<OpenApiSecurityRequirement>();
        doc.Security.Add(requirement);
    }

    protected virtual void AddCorsOptionsMethods(OpenApiDocument? doc)
    {
        if(doc is null)
            return;
        
        var pathKeys = doc.Paths.Keys.ToList();

        foreach (var pathKey in pathKeys)
        {
            var pathItem = doc.Paths[pathKey];
            
            if (pathItem.Operations is null || pathItem.Operations.ContainsKey(HttpMethod.Options))
                continue;

            var allowedMethods = string.Join(",",
                pathItem.Operations.Keys.Select(k => k.Method.ToUpperInvariant())
                    .Append("OPTIONS"));

            var optionsOperation = new OpenApiOperation
            {
                Summary = "CORS preflight",
                Responses = new OpenApiResponses
                {
                    ["200"] = new OpenApiResponse
                    {
                        Description = "CORS preflight response",
                        Headers = new Dictionary<string, IOpenApiHeader>
                        {
                            ["Access-Control-Allow-Headers"] = new OpenApiHeader
                                { Schema = new OpenApiSchema { Type = JsonSchemaType.String } },
                            ["Access-Control-Allow-Methods"] = new OpenApiHeader
                                { Schema = new OpenApiSchema { Type = JsonSchemaType.String } },
                            ["Access-Control-Allow-Origin"] = new OpenApiHeader
                                { Schema = new OpenApiSchema { Type = JsonSchemaType.String } }
                        }
                    }
                },
                Security = new List<OpenApiSecurityRequirement>() // No auth on OPTIONS
            };

            optionsOperation.Extensions ??= new Dictionary<string, IOpenApiExtension>();
            optionsOperation.Extensions["x-amazon-apigateway-integration"] = new JsonNodeExtension(new JsonObject
            {
                ["type"] = "mock",
                ["requestTemplates"] = new JsonObject
                {
                    ["application/json"] = "{\"statusCode\": 200}"
                },
                ["responses"] = new JsonObject
                {
                    ["default"] = new JsonObject
                    {
                        ["statusCode"] = "200",
                        ["responseParameters"] = new JsonObject
                        {
                            ["method.response.header.Access-Control-Allow-Headers"] = $"'{AccessControlAllowedHeaders}'",
                            ["method.response.header.Access-Control-Allow-Methods"] = $"'{allowedMethods}'",
                            ["method.response.header.Access-Control-Allow-Origin"] = $"'{AccessControlAllowedOrigin}'"
                        }
                    }
                }
            });

            pathItem.Operations[HttpMethod.Options] = optionsOperation;
        }
    }
}
