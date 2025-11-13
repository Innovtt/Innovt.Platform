using Innovt.AspNetCore.Extensions;
using Innovt.AspNetCore.Filters.Swagger;
using Innovt.CrossCutting.Log.Serilog;
using OpenTelemetry.Trace;
using ILogger = Innovt.Core.CrossCutting.Log.ILogger;

namespace Innovt.AspNetCore.Application.Tests;

public class Startup : ApiStartupBase
{
    public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env, "Sample Api",
        "Innovt sample Api", "The api sample for test only.", "v1")
    {
    }

    protected override void AddDefaultServices(IServiceCollection services)
    {
        services.AddScoped<ILogger, Logger>();
    }

    protected override void ConfigureIoC(IServiceCollection services)
    {
    }

    protected override void AddSwagger(IServiceCollection services)
    {
        base.AddSwagger(services);
    
        // services.ConfigureBearerAuthorization(options =>
        //     {
        //         options.OperationFilter<AddCustomHeaderParameter>("ExternalId",
        //             "The cognito sub id for authenticated user", false);
        //     }
        // );
    }


    public override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
    }

    protected override void ConfigureOpenTelemetry(TracerProviderBuilder builder)
    {
    }
}