// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using Innovt.AspNetCore.Filters;
using Innovt.AspNetCore.Infrastructure;
using Innovt.AspNetCore.Model;
using Innovt.Core.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Innovt.AspNetCore;
/// <summary>
/// Base class for configuring API startup settings and dependencies.
/// </summary>
public abstract class ApiStartupBase
{
    /// <summary>
    /// Initializes a new instance of the ApiStartupBase class with the specified configuration and environment.
    /// </summary>
    /// <param name="configuration">The configuration for the application.</param>
    /// <param name="environment">The hosting environment for the application.</param>
    /// <param name="appName">The name of the application.</param>
    /// <exception cref="ArgumentNullException">Thrown if configuration, environment, or appName is null.</exception>
    protected ApiStartupBase(IConfiguration configuration, IWebHostEnvironment environment, string appName)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        AppName = appName ?? throw new ArgumentNullException(nameof(appName));
        Localization = new DefaultApiLocalization();
        DefaultHealthPath = "/health";
    }
    /// <summary>
    /// Initializes a new instance of the ApiStartupBase class with additional API documentation details.
    /// </summary>
    /// <param name="configuration">The configuration for the application.</param>
    /// <param name="environment">The hosting environment for the application.</param>
    /// <param name="appName">The name of the application.</param>
    /// <param name="apiTitle">The title of the API.</param>
    /// <param name="apiDescription">The description of the API.</param>
    /// <param name="apiVersion">The version of the API.</param>
    /// <param name="contactName">The name of the API contact (optional).</param>
    /// <param name="contactEmail">The email of the API contact (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown if configuration, environment, appName, apiTitle, apiDescription, or apiVersion is null.</exception>
    protected ApiStartupBase(IConfiguration configuration, IWebHostEnvironment environment, string appName,
        string apiTitle, string apiDescription,
        string apiVersion, string? contactName=null, string? contactEmail=null) : this(configuration, environment, appName)
    {
        Documentation = new DefaultApiDocumentation(apiTitle, apiDescription, apiVersion, contactName,contactEmail);
    }
    /// <summary>
    /// Gets the name of the application.
    /// </summary>
    public string AppName { get; }
    /// <summary>
    /// Gets or sets the default health path for the application.
    /// </summary>
    protected string DefaultHealthPath { get; set; }
    /// <summary>
    /// Gets or sets the API documentation details.
    /// </summary>
    protected DefaultApiDocumentation Documentation { get; set; }
    /// <summary>
    /// Gets or sets the localization settings for the API.
    /// </summary>
    protected DefaultApiLocalization Localization { get; set; }
    /// <summary>
    /// Gets the configuration for the application.
    /// </summary>
    public IConfiguration Configuration { get; }
    /// <summary>
    /// Gets the hosting environment for the application.
    /// </summary>
    public IWebHostEnvironment Environment { get; }
    /// <summary>
    /// Checks if Swagger documentation is enabled.
    /// </summary>
    /// <returns>True if Swagger documentation is enabled; otherwise, false.</returns>
    private bool IsSwaggerEnabled()
    {
        return Documentation is { };
    }
    /// <summary>
    /// Checks if the application is running in a development environment.
    /// </summary>
    /// <returns>True if the application is in development; otherwise, false.</returns>
    protected bool IsDevelopmentEnvironment()
    {
        return Environment.IsDevelopment();
    }
    /// <summary>
    /// Adds Swagger generation to the specified services.
    /// </summary>
    /// <param name="services">The service collection to add Swagger to.</param>
    protected virtual void AddSwagger(IServiceCollection services)
    {
        if (!IsSwaggerEnabled())
            return;

        services.AddSwaggerGen(options =>
        {
            options.SchemaFilter<SwaggerExcludeFilter>();
            options.OperationFilter<SwaggerExcludeFilter>();
            options.SwaggerDoc(Documentation.ApiVersion,
                new OpenApiInfo
                {
                    Description = Documentation.ApiDescription,
                    Title = Documentation.ApiTitle,
                    Version = Documentation.ApiVersion,
                    Contact = Documentation.ContactName is null ? null : new OpenApiContact
                    {
                        Name = Documentation.ContactName,
                        Email = Documentation.ContactEmail
                    }
                });

            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();
        });
    }

    /// <summary>
    ///     Implement only the AddHealthChecks by default
    /// </summary>
    /// <param name="services"></param>
    protected virtual void ConfigureHealthChecks(IServiceCollection services)
    {
        services.AddHealthChecks();
    }
    /// <summary>
    /// Adds tracing and telemetry to the specified services.
    /// </summary>
    /// <param name="services">The service collection to add tracing to.</param>
    protected virtual void AddTracing(IServiceCollection services)
    {   
        services.AddOpenTelemetry().WithTracing(builder =>
        {
            builder.AddSource(AppName).SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(AppName))
                .SetErrorStatusOnException();

            ConfigureOpenTelemetry(builder);
        });
    }
    /// <summary>
    /// Adds localization services to the specified MVC builder and service collection.
    /// </summary>
    /// <param name="mvcBuilder">The MVC builder to add localization to.</param>
    /// <param name="services">The service collection to add localization to.</param>
    private void AddLocalization(IMvcBuilder mvcBuilder, IServiceCollection services)
    {
        if (Localization?.DefaultLocalizeResource == null) return;

        services.AddLocalization();

        mvcBuilder.AddMvcLocalization(op =>
            {
                op.DataAnnotationLocalizerProvider =
                    (type, factory) => factory.Create(Localization.DefaultLocalizeResource);
            })
            .AddDataAnnotationsLocalization(op =>
            {
                op.DataAnnotationLocalizerProvider =
                    (type, factory) => factory.Create(Localization.DefaultLocalizeResource);
            });
        
    }
    /// <summary>
    /// Adds core services needed for API configuration.
    /// </summary>
    /// <param name="services">The service collection to add core services to.</param>
    private void AddCoreServices(IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<ApiExceptionFilter>();

        var provider = services.BuildServiceProvider();

        var mvcBuilder = services.AddControllers(op =>
        {
            op.Filters.Add(provider.GetService<ApiExceptionFilter>() ?? throw new InvalidOperationException());
        });

       AddLocalization(mvcBuilder,services);
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    /// <summary>
    ///     Configure services will register default services for api and mvc applications. AddHealthChecks
    /// </summary>
    /// <param name="services"></param>
    public virtual void ConfigureServices(IServiceCollection services)
    {
        ConfigureIoC(services);

        AddDefaultServices(services);

        AddCoreServices(services);

        services.Configure(ConfigureApiBehavior());

        ConfigureHealthChecks(services);

        AddTracing(services);

        AddSwagger(services);
    }
    /// <summary>
    /// Configures Swagger UI for API documentation.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The hosting environment.</param>
    protected virtual void ConfigureSwaggerUi(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!IsSwaggerEnabled()) return;

        app.UseRewriter(new RewriteOptions().AddRedirect("(.*)docs$", "$1docs/index.html"));

        app.UseSwagger(s => { s.RouteTemplate = "docs/{documentName}/swagger.json"; }).UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"{Documentation.ApiVersion}/swagger.json", Documentation.ApiTitle);
            c.RoutePrefix = "docs";
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// <summary>
    ///  Configure Will Add All main Services as Default for Api and MVC Applications
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="loggerFactory"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            ConfigureSwaggerUi(app, env);
        }

        app.UseHttpsRedirection().UseRouting();

        ConfigureCultures(app);

        app.UseHealthChecks(DefaultHealthPath);

        ConfigureApp(app, env, loggerFactory);

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
    /// <summary>
    /// Configures API behavior options.
    /// </summary>
    /// <returns>An action that configures <see cref="ApiBehaviorOptions"/>.</returns>
    protected virtual Action<ApiBehaviorOptions> ConfigureApiBehavior()
    {
        return options =>
        {
            options.InvalidModelStateResponseFactory = InvalidModelStateResponse.CreateCustomErrorResponse;
            options.SuppressInferBindingSourcesForParameters = true;
            options.SuppressMapClientErrors = true;
        };
    }
    /// <summary>
    /// Configures request cultures for the application.
    /// </summary>
    /// <param name="app">The application builder.</param>
    protected virtual void ConfigureCultures(IApplicationBuilder app)
    {
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(Localization.RequestCulture),
            SupportedCultures = Localization.SupportedCultures,
            SupportedUICultures = Localization.SupportedCultures
        });
    }
    /// <summary>
    /// Adds default services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add default services to.</param>
    protected abstract void AddDefaultServices(IServiceCollection services);
    /// <summary>
    /// Configures the IoC container for the application.
    /// </summary>
    /// <param name="services">The service collection to configure for IoC.</param>
    protected abstract void ConfigureIoC(IServiceCollection services);
    /// <summary>
    /// Configures the application.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The hosting environment.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    // ReSharper disable once MemberCanBeProtected.Global
    public abstract void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory);
    /// <summary>
    /// Configures OpenTelemetry for tracing.
    /// </summary>
    /// <param name="builder">The TracerProviderBuilder for configuring tracing.</param>
    protected abstract void ConfigureOpenTelemetry(TracerProviderBuilder builder);
}