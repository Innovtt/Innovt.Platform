// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.CrossCutting.IOC
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Microsoft.Extensions.DependencyInjection;
using System;

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