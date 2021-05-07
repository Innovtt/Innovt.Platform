// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCoreTests
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Innovt.AspNetCoreTests
{
    public class Startup : ApiStartupBase
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        //public IConfiguration Configuration { get; }

        //// This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{

        //    services.AddControllers();
        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Innovt.AspNetCoreTests", Version = "v1" });
        //    });
        //}

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    //if (env.IsDevelopment())
        //    //{
        //        app.UseDeveloperExceptionPage();
        //        app.UseSwagger();
        //        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Innovt.AspNetCoreTests v1"));
        //   // }

        //    app.UseRouting();

        //    app.UseAuthorization();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllers();
        //    });
        //}

        public Startup(IConfiguration configuration) : base(configuration,"sampleMIchel", "Title", "description", "1.0")
        {
        }


        protected override void AddDefaultServices(IServiceCollection services)
        {
            // throw new System.NotImplementedException();
        }

        protected override void ConfigureIoC(IServiceCollection services)
        {
            // throw new System.NotImplementedException();
        }

        public override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // throw new System.NotImplementedException();
        }

        protected override void ConfigureOpenTelemetry(TracerProviderBuilder builder)
        {
            builder.AddZipkinExporter();

            //builder.AddZipkinExporter(c =>
            //    {
            //        c.Endpoint = new Uri("http://127.1.1.1:9410/api/v2/spans");
            //        c.ExportProcessorType = ExportProcessorType.Simple;
            //    }
            //);

        }
    }
}