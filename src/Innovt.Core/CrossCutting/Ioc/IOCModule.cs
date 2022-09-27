// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc;

public class IOCModule
{
    private readonly IServiceCollection services;

    public IOCModule()
    {
        services = new ServiceCollection();
    }

    public IServiceCollection GetServices()
    {
        return services;
    }
}