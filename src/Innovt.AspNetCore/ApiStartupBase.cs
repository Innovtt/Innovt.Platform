// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.AspNetCore.Filters;
using Innovt.AspNetCore.Infrastructure;
using Innovt.AspNetCore.Model;
using Microsoft.AspNetCore.Builder;
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

public abstract class ApiStartupBase
{
    protected ApiStartupBase(IConfiguration configuration, IWebHostEnvironment environment, string appName)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        AppName = appName ?? throw new ArgumentNullException(nameof(appName));
        Localization = new DefaultApiLocalization();
        DefaultHealthPath = "/health";
    }

    protected ApiStartupBase(IConfiguration configuration, IWebHostEnvironment environment, string appName,
        string apiTitle, string apiDescription,
        string apiVersion) : this(configuration, environment, appName)
    {
        Documentation = new DefaultApiDocumentation(apiTitle, apiDescription, apiVersion);
    }

    public string AppName { get; }

    protected string DefaultHealthPath { get; set; }

    protected DefaultApiDocumentation Documentation { get; set; }

    protected DefaultApiLocalization Localization { get; set; }

    public IConfiguration Configuration { get; }

    public IWebHostEnvironment Environment { get; }

    private bool IsSwaggerEnabled()
    {
        return Documentation is { };
    }

    protected bool IsDevelopmentEnvironment()
    {
        return Environment.IsDevelopment();
    }

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
                    Version = Documentation.ApiVersion
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

    protected virtual void AddTracing(IServiceCollection services)
    {
        services.AddOpenTelemetryTracing(builder =>
        {
            builder.AddSource(AppName).SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService(serviceName: AppName))
                .SetErrorStatusOnException(true);

            ConfigureOpenTelemetry(builder);
        });
    }

    private void AddCoreServices(IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<ApiExceptionFilter>();

        var provider = services.BuildServiceProvider();

        var mvcBuilder = services.AddControllers(op =>
        {
            op.Filters.Add(provider.GetService<ApiExceptionFilter>() ?? throw new InvalidOperationException());
        });

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
    ///     Configure Will Add All main Services as Default for Api and MVC Applications
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

    protected virtual Action<ApiBehaviorOptions> ConfigureApiBehavior()
    {
        return options =>
        {
            options.InvalidModelStateResponseFactory = InvalidModelStateResponse.CreateCustomErrorResponse;
            options.SuppressInferBindingSourcesForParameters = true;
            options.SuppressMapClientErrors = true;
        };
    }

    protected virtual void ConfigureCultures(IApplicationBuilder app)
    {
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(Localization.RequestCulture),
            SupportedCultures = Localization.SupportedCultures,
            SupportedUICultures = Localization.SupportedCultures
        });
    }

    protected abstract void AddDefaultServices(IServiceCollection services);

    protected abstract void ConfigureIoC(IServiceCollection services);

    public abstract void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory);

    protected abstract void ConfigureOpenTelemetry(TracerProviderBuilder builder);
}