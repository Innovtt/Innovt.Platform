using System;
using System.IO;
using System.Reflection;
using Innovt.AspNetCore.Filters;
using Innovt.AspNetCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        private readonly string healthPath= "/health";
        private readonly bool enableDocInProduction;

        public IConfiguration Configuration { get; }
        private readonly string apiTitle;
        private readonly string apiDescription;
        private readonly string apiVersion;
        private readonly bool disableTrace;

        protected ApiStartupBase(IConfiguration configuration, string apiTitle,
            string apiDescription, string apiVersion,string healthPath="/health",bool enableDocInProduction=false, bool disableTrace=false)
        {
            Configuration = configuration;

            this.apiTitle = apiTitle;
            this.apiDescription = apiDescription;
            this.apiVersion = apiVersion;
            this.healthPath = healthPath;
            this.enableDocInProduction = enableDocInProduction;
            this.disableTrace = disableTrace;
        }

        protected ApiStartupBase(IConfiguration configuration):this(configuration,null,null,null)
        {   
        }

        internal bool IsSwaggerEnabled() {
            return !(apiTitle == null && apiVersion == null);
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

                options.SwaggerDoc(apiVersion,
                    new Microsoft.OpenApi.Models.OpenApiInfo{Title = apiTitle, Description = apiDescription, Version = apiVersion});

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
            if (disableTrace)
                return;

            services.AddOpenTracing(b => b.AddAspNetCore());

            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var tracer = CreateTracer(services);

                GlobalTracer.Register(tracer);
                return tracer;
            });
        }

       

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configure services will register default services for api and mvc applications. AddHealthChecks 
        /// </summary>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services)
        {  
            ConfigureIoC(services);
            
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = InvalidModelStateResponse.CreateCustomErrorResponse;
            });

            AddDefaultServices(services);

            ConfigureHealthChecks(services);

            AddTracing(services);

            AddSwagger(services);
        }

        protected virtual void ConfigureSwaggerUi(IApplicationBuilder app)
        {
            if (!IsSwaggerEnabled())
                return;

            app.UseRewriter(new RewriteOptions().AddRedirect("(.*)docs$", "$1docs/index.html"));
            
            app.UseSwagger(s=>{ 
                s.RouteTemplate = "docs/{documentName}/swagger.json";
                }).UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{apiVersion}/swagger.json", apiTitle);
                c.RoutePrefix ="docs";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configure Will Add All main Services as Default for Api and MVC Applications
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
          
            app.UseHealthChecks(healthPath);

            ConfigureApp(app, env, loggerFactory);

            if ((env.IsProduction() && enableDocInProduction) || !env.IsProduction())
            {
                ConfigureSwaggerUi(app);
            }
        }

        protected abstract void AddDefaultServices(IServiceCollection services);

        protected abstract void ConfigureIoC(IServiceCollection services);

        public abstract void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory);

        protected virtual ITracer CreateTracer(IServiceCollection services)
        {
            return NoopTracerFactory.Create();
        }
    }
}
