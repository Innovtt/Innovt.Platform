// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.IOC

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC;

public class ServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        return new ContainerBuilder(services);
    }

    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        if (containerBuilder is null) throw new ArgumentNullException(nameof(containerBuilder));

        return containerBuilder.GetServiceProvider;
    }
}