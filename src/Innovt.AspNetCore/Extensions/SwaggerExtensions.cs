using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Innovt.AspNetCore.Extensions;

public static class SwaggerExtensions
{
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