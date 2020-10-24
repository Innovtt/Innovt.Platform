using System;
using System.IO;
using System.Reflection;
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
using OpenTracing;
using OpenTracing.Noop;
using OpenTracing.Util;

namespace Innovt.AspNetCore
{  
    public abstract class ApiStartupBase
    {
        protected string DefaultHealthPath => "/health";

        protected DefaultApiDocumentation Documentation { get; set; }
        protected DefaultApiLocalization Localization { get;  set; }
        protected bool DisableTracing { get; set; }

        public IConfiguration Configuration { get; }

        protected ApiStartupBase(IConfiguration configuration, string apiTitle,
            string apiDescription, string apiVersion,bool enableDocInProduction=false, bool disableTracing=false):this(configuration)
        {
            Documentation = new DefaultApiDocumentation(enableDocInProduction,apiTitle,apiDescription,apiVersion);
            this.DisableTracing = disableTracing;
        }

        protected ApiStartupBase(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Localization = new DefaultApiLocalization();
        }

        internal bool IsSwaggerEnabled()
        {
            return (Documentation is {});
        }

        protected virtual void AddSwagger(IServiceCollection services)
        {   
            if(!IsSwaggerEnabled())
                return;
          
            services.AddSwaggerGen(options =>
            {
                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();

                options.SchemaFilter<SwaggerExcludeFilter>();
                options.OperationFilter<SwaggerExcludeFilter>();

                options.SwaggerDoc(Documentation.ApiVersion,
                    new Microsoft.OpenApi.Models.OpenApiInfo{Title = Documentation.ApiTitle, Description = Documentation.ApiDescription, Version = Documentation.ApiVersion});

                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml");

                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });
        }

        /// <summary>
        /// Implement only the AddHealthChecks by default
        /// </summary>
        /// <param name="services"></param>
        protected virtual void ConfigureHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        protected virtual void AddTracing(IServiceCollection services)
        {
            if (DisableTracing)
                return;

            services.AddOpenTracing(b => b.AddAspNetCore());

            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var tracer = CreateTracer(services);

                GlobalTracer.Register(tracer);

                return tracer;
            });
        }


        private void AddCoreServices(IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<ApiExceptionFilter>();

            var provider = services.BuildServiceProvider();
            
            var mvcBuilder = services.AddControllers(op =>
            {
                //workaround because the add filter is not been resolved 
                op.Filters.Add(provider.GetService<ApiExceptionFilter>());
            });

            if (Localization?.DefaultLocalizeResource != null)
            {
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
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configure services will register default services for api and mvc applications. AddHealthChecks 
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

        protected virtual void ConfigureSwaggerUi(IApplicationBuilder app,IWebHostEnvironment env)
        {
            if (!IsSwaggerEnabled() || (env.IsProduction() && !Documentation.EnableDocInProduction))
            {
              return;
            }


            app.UseRewriter(new RewriteOptions().AddRedirect("(.*)docs$", "$1docs/index.html"));
 
            app.UseSwagger(s=>{ 
                s.RouteTemplate = "docs/{documentName}/swagger.json";
                }).UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{Documentation.ApiVersion}/swagger.json", Documentation.ApiTitle);
                c.RoutePrefix ="docs";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configure Will Add All main Services as Default for Api and MVC Applications
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureCultures(app);

            app.UseHealthChecks(DefaultHealthPath);

            ConfigureApp(app, env, loggerFactory);

            ConfigureSwaggerUi(app, env);
        }

        protected abstract void AddDefaultServices(IServiceCollection services);

        protected abstract void ConfigureIoC(IServiceCollection services);

        public abstract void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory);

        protected virtual ITracer CreateTracer(IServiceCollection services)
        {
            return NoopTracerFactory.Create();
        }

        protected virtual Action<ApiBehaviorOptions> ConfigureApiBehavior()
        {
           return (options =>
            {
                options.InvalidModelStateResponseFactory = InvalidModelStateResponse.CreateCustomErrorResponse;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressMapClientErrors = true;
            });
        }

        protected virtual void ConfigureCultures(IApplicationBuilder app)
        {
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(Localization.RequestCulture),
                SupportedCultures   = Localization.SupportedCultures,
                SupportedUICultures = Localization.SupportedCultures
            });
        }
    }
}
