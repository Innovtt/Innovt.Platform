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

    public IOCModule(IServiceCollection services) : this()
    {
        AppendServices(services);
    }

    public IServiceCollection GetServices()
    {
        return services;
    }

    public IServiceCollection Services => services;

    public IServiceCollection AppendServices(IServiceCollection externalServices)
    {
        if (externalServices == null)
            return services;

        foreach (var serviceDescriptor in externalServices)
        {
            services.Add(serviceDescriptor);
        }

        return services;
    }
}