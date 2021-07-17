// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCoreTests
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;

namespace Innovt.AspNetCoreTests
{
    public class Startup : ApiStartupBase
    {
        public Startup(IConfiguration configuration) : base(configuration, "sampleMichel", "Title", "description",
            "1.0")
        {
        }


        protected override void AddDefaultServices(IServiceCollection services)
        {
            
        }

        protected override void ConfigureIoC(IServiceCollection services)
        {
            
        }

        public override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            
        }

        protected override void ConfigureOpenTelemetry(TracerProviderBuilder builder)
        {
            builder.AddZipkinExporter();
        }
    }
}