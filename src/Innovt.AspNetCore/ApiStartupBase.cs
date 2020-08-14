using System;
using System.IO;
using System.Reflection;
using Innovt.AspNetCore.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Innovt.AspNetCore
{
    public abstract class ApiStartupBase
    {
        private readonly string healthPath= "/health";

        public IConfiguration Configuration { get; }
        private readonly string apiTitle;
        private readonly string apiDescription;
        private readonly string apiVersion;

        protected ApiStartupBase(IConfiguration configuration, string apiTitle,
            string apiDescription, string apiVersion,string healthPath="/health")
        {
            Configuration = configuration;

            this.apiTitle = apiTitle;
            this.apiDescription = apiDescription;
            this.apiVersion = apiVersion;
            this.healthPath = healthPath;
        }


        protected ApiStartupBase(IConfiguration configuration):this(configuration,null,null,null)
        {
            
        }


        internal bool isSwaggerEnabled() {
            return !(apiTitle == null && apiVersion == null);
        }

        protected virtual void AddSwagger(IServiceCollection services)
        {   
            if(!isSwaggerEnabled())
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

        protected abstract void ConfigureIoC(IServiceCollection services);

        /// <summary>
        /// Implement only the AddHealthChecks by default
        /// </summary>
        /// <param name="services"></param>
        protected virtual void ConfigureHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        protected virtual void ConfigureOpenTracing(IServiceCollection services)
        {
            services.AddOpenTracing();
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

            ConfigureHealthChecks(services);

            ConfigureOpenTracing(services);

            AddSwagger(services);
        }


        protected virtual void ConfigureSwaggerUi(IApplicationBuilder app)
        {
            if (!isSwaggerEnabled())
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

            ConfigureSwaggerUi(app);
        }

        protected abstract void AddDefaultServices(IServiceCollection services);

        public abstract void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory);
    }
}
