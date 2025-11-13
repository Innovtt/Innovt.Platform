using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Innovt.AspNetCore.Extensions;

/// <summary>
///     Extension methods for configuring Swagger authorization schemes.
/// </summary>
public static class SwaggerExtensions
{   /// <summary>
    ///     Configures Bearer token authorization for Swagger.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    /// <param name="customOptions">Custom options for Swagger configuration.</param>
    public static void ConfigureBearerAuthorization(this IServiceCollection services, Action<SwaggerGenOptions>? customOptions = null)
    {
        services.ConfigureSwaggerGen((Action<SwaggerGenOptions>)(options =>
        {
            options.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });
            
            options.AddSecurityRequirement(p => new OpenApiSecurityRequirement
            {
                { new OpenApiSecuritySchemeReference("bearer"), [] }
            });
            
            customOptions?.Invoke(options);
        }));
    }

    /// <summary>
    ///     Configures Basic authentication for Swagger.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    /// <param name="customOptions">Custom options for Swagger configuration.</param>
    public static void ConfigureBasicAuthorization(this IServiceCollection services,
        Action<SwaggerGenOptions>? customOptions = null)
    {
        services.ConfigureSwaggerGen((Action<SwaggerGenOptions>)(options =>
        {
            options.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Description = "Basic authorization"
            });
            
            options.AddSecurityRequirement(p => new OpenApiSecurityRequirement
            {
                { new OpenApiSecuritySchemeReference("basic"), [] }
            });
            
            customOptions?.Invoke(options);
        }));
    }
}