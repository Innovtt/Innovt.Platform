using Innovt.AspNetCore;
using Innovt.CrossCutting.Log.Serilog;
using Innovt.OpenTelemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Trace;
using ILogger = Innovt.Core.CrossCutting.Log.ILogger;

namespace SampleAspNetWebApiTest
{
    public class Startup : ApiStartupBase
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env, "WebApplication1",
            "Anticipation Api", "", "v1")
        {

        }

        protected override void AddDefaultServices(IServiceCollection services)
        {
            services
                .AddLocalization()
                .AddMvc();

            services.AddMvc();

            Sdk.SetDefaultTextMapPropagator(new AWSXRayPropagator());
        }

        protected override void ConfigureIoC(IServiceCollection services)
        {
            services.AddOpenTelemetryMetrics();

            services.AddTransient<ILogger, Logger>();//p=>new Logger(logConfiguration));
        }

        public override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseXRay("WebApplication1");

            //app.UseSerilogRequestLogging();
        }

        protected override void ConfigureOpenTelemetry(TracerProviderBuilder builder)
        {
            builder.AddLoggerExporter(builder.GetServices());

            builder.AddJaegerExporter(e =>
            {
                e.AgentPort = 6831;
                e.AgentHost = "localhost";
            });

            //AddAWSLambdaConfigurations();
        }
    }
}