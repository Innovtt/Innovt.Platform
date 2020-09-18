using Innovt.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

    //        var log = new LoggerConfiguration(url: "http-intake.logs.datadoghq.com")
    //            .WriteTo.DatadogLogs("<API_KEY>")
    //.CreateLogger();

        }

        protected override void AddDefaultServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        protected override void ConfigureIoC(IServiceCollection services)
        {
          
            //IServiceProvider

        }
    }

   
}
