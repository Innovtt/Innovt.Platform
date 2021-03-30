using Microsoft.Extensions.DependencyInjection;
using System;

namespace Innovt.CrossCutting.IOC
{
    public class ServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            return new ContainerBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            return containerBuilder.GetServiceProvider();
        }
    }
}