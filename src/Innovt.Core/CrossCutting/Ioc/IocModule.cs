// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.CrossCutting.Ioc;

/// <summary>
///     Represents a module for configuring and managing services within a dependency injection container.
/// </summary>
/// <remarks>
///     This class provides a way to define and manage services within a dependency injection container.
///     Developers can create custom modules to configure and add services to the container, making it easier to organize
///     and maintain the service registration code.
/// </remarks>
public class IocModule
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="IocModule" /> class with a new <see cref="ServiceCollection" />.
    /// </summary>
    public IocModule()
    {
        Services = new ServiceCollection();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="IocModule" /> class with the provided <paramref name="services" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to be used for service registration.</param>
    public IocModule(IServiceCollection services)
    {
        this.Services = services;
    }

    /// <summary>
    ///     Gets the <see cref="IServiceCollection" /> used for service registration and configuration.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    ///     Gets the <see cref="IServiceCollection" /> used for service registration and configuration.
    /// </summary>
    /// <returns>The <see cref="IServiceCollection" /> associated with this module.</returns>
    public IServiceCollection GetServices()
    {
        return Services;
    }

    /// <summary>
    ///     Appends services from an external <see cref="IServiceCollection" /> to the module's services.
    /// </summary>
    /// <param name="externalServices">The external <see cref="IServiceCollection" /> containing services to append.</param>
    /// <returns>The updated <see cref="IServiceCollection" /> after appending external services.</returns>
    public IServiceCollection AppendServices(IServiceCollection externalServices)
    {
        if (externalServices == null)
            return Services;

        foreach (var serviceDescriptor in externalServices) Services.Add(serviceDescriptor);

        return Services;
    }
}