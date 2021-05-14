using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTelemetry.Trace;

namespace Innovt.AspNetCore.Authorization
{
    public class Startup:ApiStartupBase
    {
        public Startup(IConfiguration configuration) : base(configuration, "Innovt Authorization", "Authorization","Api that users can manage Authorizations", "1.0")
        {
        }

        protected override void AddDefaultServices(IServiceCollection services)
        {
            
        }

        protected override void ConfigureIoC(IServiceCollection services)
        {
        }

        public override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            
        }

        protected override void ConfigureOpenTelemetry(TracerProviderBuilder builder)
        {
            
        }
    }
}
