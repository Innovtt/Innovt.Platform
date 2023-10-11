// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.IOC

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC;

/// <summary>
/// Factory for creating an <see cref="IServiceProvider"/> using a <see cref="ContainerBuilder"/>.
/// </summary>
public class ServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    /// <summary>
    /// Creates a new <see cref="ContainerBuilder"/> using the specified collection of services.
    /// </summary>
    /// <param name="services">The collection of services for the container.</param>
    /// <returns>A new instance of <see cref="ContainerBuilder"/>.</returns>
    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        return new ContainerBuilder(services);
    }

    /// <summary>
    /// Creates an <see cref="IServiceProvider"/> using the specified <see cref="ContainerBuilder"/>.
    /// </summary>
    /// <param name="containerBuilder">The <see cref="ContainerBuilder"/> to use for creating the service provider.</param>
    /// <returns>An <see cref="IServiceProvider"/> based on the provided <see cref="ContainerBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="containerBuilder"/> is null.</exception>
    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        if (containerBuilder is null) throw new ArgumentNullException(nameof(containerBuilder));

        return containerBuilder.GetServiceProvider;
    }
}