using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Innovt.AspNetCore.Tests
{
    public class Startup:ApiStartupBase
    {
        public Startup(IConfiguration configuration) : base(configuration, "", "", "", false)
        {
        }
        
        aws --profile antecipa-prod ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 861734233337.dkr.ecr.us-east-1.amazonaws.com
            
            
        
        
        // public Startup(IConfiguration configuration)
        // {
        //     Configuration = configuration;
        // }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Innovt.AspNetCore.Tests", Version = "v1"});
            });
        }

        protected override void AddDefaultServices(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        protected override void ConfigureIoC(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        public override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        // {
        //     if (env.IsDevelopment())
        //     {
        //         app.UseDeveloperExceptionPage();
        //         app.UseSwagger();
        //         app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Innovt.AspNetCore.Tests v1"));
        //     }
        //
        //     app.UseHttpsRedirection();
        //
        //     app.UseRouting();
        //
        //     app.UseAuthorization();
        //
        //     app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        // }

    }
}