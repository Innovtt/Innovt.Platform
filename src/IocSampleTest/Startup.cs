using Innovt.AspNetCore;
using Innovt.AspNetCore.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;

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
            services.AddControllers(op =>
            {
                op.Filters.Add(new ApiExceptionFilterAttribute());
            });
        }

        protected override void ConfigureIoC(IServiceCollection services)
        {
          
            //IServiceProvider

        }

        //protected override void ConfigureTracer(TracerProviderBuilder tracerBuilder)
        //{
        //    tracerBuilder.AddConsoleExporter();
        //}
    }

   
}
