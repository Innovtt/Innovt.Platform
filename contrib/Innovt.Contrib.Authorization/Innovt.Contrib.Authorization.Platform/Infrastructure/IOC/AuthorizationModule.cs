// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform
using Innovt.Cloud.AWS.Configuration;
using Innovt.Contrib.Authorization.Platform.Application;
using Innovt.Contrib.Authorization.Platform.Domain;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.Log.Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.IOC;
/// <summary>
/// Represents a module for configuring authorization-related services in the Inversion of Control (IoC) container.
/// </summary>
public class AuthorizationModule : IOCModule
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationModule"/> class.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure services.</param>
    public AuthorizationModule(IServiceCollection services=null):base(services)
    {
        Services.AddScoped<IAuthorizationAppService, AuthorizationAppService>();
        Services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
        Services.TryAddScoped<IAwsConfiguration, DefaultAWSConfiguration>();
        Services.TryAddScoped<ILogger, Logger>();
    }
}