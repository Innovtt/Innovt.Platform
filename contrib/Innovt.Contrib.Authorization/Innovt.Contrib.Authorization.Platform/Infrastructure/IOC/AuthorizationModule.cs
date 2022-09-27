// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Domain;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.Log.Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.IOC;

public class AuthorizationModule : IOCModule
{
    public AuthorizationModule(IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddScoped<IAuthorizationAppService, AuthorizationAppService>();
        services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
        services.TryAddScoped<IAwsConfiguration, DefaultAWSConfiguration>();
        services.TryAddScoped<ILogger, Logger>();
    }
}