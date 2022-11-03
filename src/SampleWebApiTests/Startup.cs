using Innovt.AspNetCore;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Infrastructure;
using Innovt.Contrib.AuthorizationRoles.AspNetCore;
using Innovt.CrossCutting.Log.Serilog;
using Innovt.Domain.Security;
using OpenTelemetry.Trace;
using ILogger = Innovt.Core.CrossCutting.Log.ILogger;

namespace SampleWebApiTests
{
    public class Startup : ApiStartupBase
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env, "AccountTransactions Services Api", "Api microservice for account transactions.", "AccountTransaction Services api", "v1")
        {
            Localization = new Innovt.AspNetCore.Model.DefaultApiLocalization();
        }
        
        protected override void AddDefaultServices(IServiceCollection services)
        {
            services.AddInnovtRolesAuthorization();

            services
                .AddLocalization()
                .AddMvc(options =>
                {
                });

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressMapClientErrors = true;
            });
        }

        protected override void ConfigureIoC(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
        }

        public override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRouting();

            app.UseCors();
            
        }

        protected override void ConfigureOpenTelemetry(TracerProviderBuilder builder)
        {
            
        }
    }
}