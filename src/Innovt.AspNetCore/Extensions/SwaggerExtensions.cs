using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Innovt.AspNetCore.Extensions;
/// <summary>
/// Extension methods for configuring Swagger authorization schemes.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Creates a Bearer type security scheme for Swagger.
    /// </summary>
    /// <param name="name">The name of the scheme.</param>
    /// <param name="description">The description of the scheme.</param>
    /// <param name="scheme">The scheme type (e.g., "bearer").</param>
    /// <returns>An <see cref="OpenApiSecurityScheme"/> object representing the scheme.</returns>
    private static OpenApiSecurityScheme CreateScheme(string name, string description, string scheme)
  {
    return new OpenApiSecurityScheme()
    {
      Name = name,
      Type = SecuritySchemeType.Http,
      Scheme = scheme,
      In = ParameterLocation.Header,
      Description = description
    };
  }
    /// <summary>
    /// Creates a security requirement for Swagger.
    /// </summary>
    /// <param name="id">The ID of the security scheme.</param>
    /// <param name="name">The name of the security scheme.</param>
    /// <param name="scheme">The scheme type (e.g., "oauth2").</param>
    /// <returns>An <see cref="OpenApiSecurityRequirement"/> object representing the security requirement.</returns>
    private static OpenApiSecurityRequirement CreateSecurityRequirement(string id,string name, string? scheme)
  {
    return new OpenApiSecurityRequirement()
    {
      {
        new OpenApiSecurityScheme()
        {
          Reference = new OpenApiReference()
          {
            Type = new ReferenceType?(ReferenceType.SecurityScheme),
            Id = id
          },
          Scheme = scheme,
          Name = name,
          In = ParameterLocation.Header
        },new List<string>()
      }
    };
  }

    /// <summary>
    /// Configures Bearer token authorization for Swagger.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <param name="customOptions">Custom options for Swagger configuration.</param>
    public static void ConfigureBearerAuthorization(this IServiceCollection services, Action<SwaggerGenOptions>? customOptions=null)
    {
      var bearerScheme = CreateScheme("Authorization","Bearer authorization", "bearer");
      var securityScheme = CreateSecurityRequirement("bearer", "bearer", "oauth2");
      
      bearerScheme.BearerFormat = "JWT";
      
      services.ConfigureSwaggerGen((Action<SwaggerGenOptions>) (options =>
      {
        options.AddSecurityDefinition("bearer", bearerScheme);
        customOptions?.Invoke(options);
        options.AddSecurityRequirement(securityScheme);
      }));
    }
    /// <summary>
    /// Configures Basic authentication for Swagger.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <param name="customOptions">Custom options for Swagger configuration.</param>
    public static void ConfigureBasicAuthorization(this IServiceCollection services, Action<SwaggerGenOptions>? customOptions = null)
    {
      var basicScheme = CreateScheme("Authorization","Basic authorization", "basic");
      var securityScheme = CreateSecurityRequirement("basic", "basic", null);
      
      
      services.ConfigureSwaggerGen((Action<SwaggerGenOptions>) (options =>
      {
        options.AddSecurityDefinition("basic", basicScheme);
        customOptions?.Invoke(options);
        options.AddSecurityRequirement(securityScheme);
      }));
  }
}