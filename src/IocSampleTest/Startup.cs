using Innovt.AspNetCore;
using Innovt.AspNetCore.Filters;
using Innovt.AspNetCore.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace IocSample
{   
    public class Startup : ApiStartupBase
    {
        public Startup(IConfiguration configuration):base(configuration)
        {   
            
        }

        public override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        protected override void AddDefaultServices(IServiceCollection services)
        {
            //services.AddScoped<IStringLocalizer<IExceptionResource>,ExceptionResource>();


            //services.AddScoped<ApiExceptionFilter>();
            
            ////[ServiceFilter(typeof(ApiExceptionFilter))]
            //services.AddControllers(op =>
            //{   
            //    op.Filters.Add<ApiExceptionFilter>();
            //});
        }

        protected override void ConfigureIoC(IServiceCollection services)
        {
            services.AddScoped<Innovt.Core.CrossCutting.Log.ILogger>(l=> new Innovt.CrossCutting.Log.Serilog.Logger());
        }

        protected override ITracer CreateTracer(IServiceCollection services)
        {  
            return Datadog.Trace.OpenTracing.OpenTracingTracerFactory.CreateTracer();
        }
    }

   
}
