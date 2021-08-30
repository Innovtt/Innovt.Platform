// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-18
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Domain;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.Log.Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.IOC
{
    public class AuthorizationModule:IOCModule
    {
        public AuthorizationModule(IServiceCollection services, string moduleName)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton(new ModuleConfiguration(moduleName));

            services.AddScoped<IAuthorizationAppService, AuthorizationAppService>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();

            var builder = services.BuildServiceProvider();

            if (builder.GetService<ILogger>() is null) 
            {
                services.AddScoped<ILogger, Logger>();
            }

            if (builder.GetService<IAwsConfiguration>() is null)
            {
                services.AddScoped<IAwsConfiguration, DefaultAWSConfiguration>();
            }            
        }
    }
}