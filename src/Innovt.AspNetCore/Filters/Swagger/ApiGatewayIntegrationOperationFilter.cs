using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Innovt.AspNetCore.Filters.Swagger;

public class ApiGatewayIntegrationOperationFilter(string lambdaFunctionName) : IOperationFilter
{
    private readonly string lambdaFunctionName = lambdaFunctionName ?? throw new ArgumentNullException(nameof(lambdaFunctionName));

    public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if(operation is null)
            return;
        
        var sub = "arn:aws:apigateway:${AWS::Region}:lambda:path/2015-03-31/functions/${ApiLambdaFunction.Arn}/invocations".
            Replace("ApiLambdaFunction", lambdaFunctionName);
            
        var integration = new JsonObject
        {
            ["type"] = "aws_proxy",
            ["httpMethod"] = "POST",
            ["uri"] = new JsonObject
            {
                ["Fn::Sub"] = sub
            },
            ["passthroughBehavior"] = "when_no_match",
            ["timeoutInMillis"] = 29000
        };

        operation.Extensions ??= new Dictionary<string, IOpenApiExtension>();
        operation.Extensions["x-amazon-apigateway-integration"] = new JsonNodeExtension(integration);
    }
}
