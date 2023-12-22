﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.IOC

using System;
using Innovt.Core.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC;

/// <summary>
/// Implementation of the IoC container interface.
/// </summary>
public sealed class Container : IContainer
{
    private readonly Lamar.Container container;

    /// <summary>
    /// Initializes a new instance of the <see cref="Container"/> class using the provided services.
    /// </summary>
    /// <param name="services">The collection of services for the container.</param>
    public Container(IServiceCollection services)
    {
        container = new Lamar.Container(services);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Container"/> class with default scan settings.
    /// </summary>
    public Container()
    {
        container = new Lamar.Container(c =>
        {
            c.Scan(s =>
            {
                s.TheCallingAssembly();
                s.WithDefaultConventions();
            });
        });
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Container"/> class using the specified IoC module.
    /// </summary>
    /// <param name="iocModule">The IoC module providing services for the container.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="iocModule"/> is null.</exception>
    public Container(IOCModule iocModule)
    {
        if (iocModule is null) throw new ArgumentNullException(nameof(iocModule));

        container = new Lamar.Container(iocModule.GetServices());
    }

    /// <summary>
    /// Adds services from the specified IoC module to the container.
    /// </summary>
    /// <param name="iocModule">The IoC module providing services to be added.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="iocModule"/> is null.</exception>
    public void AddModule(IOCModule iocModule)
    {
        if (iocModule is null) throw new ArgumentNullException(nameof(iocModule));

        var services = iocModule.GetServices();

        container.Configure(services);
    }

    /// <summary>
    /// Checks the configuration of the container.
    /// </summary>
    public void CheckConfiguration()
    {
        container.AssertConfigurationIsValid();
    }

    /// <summary>
    /// Resolves an instance of the specified type.
    /// </summary>
    /// <param name="type">The type to be resolved.</param>
    /// <returns>An instance of the specified type.</returns>
    public object Resolve(Type type)
    {
        return container.GetInstance(type);
    }

    /// <summary>
    /// Resolves an instance of the specified type.
    /// </summary>
    /// <typeparam name="TService">The type to be resolved.</typeparam>
    /// <returns>An instance of the specified type.</returns>
    public TService Resolve<TService>()
    {
        return container.GetInstance<TService>();
    }

    /// <summary>
    /// Resolves an instance of the specified type.
    /// </summary>
    /// <typeparam name="TService">The type to be resolved.</typeparam>
    /// <param name="type">The type to be resolved.</param>
    /// <returns>An instance of the specified type.</returns>
    public TService Resolve<TService>(Type type)
    {
        return (TService)container.GetInstance(type);
    }

    /// <summary>
    /// Resolves an instance of the specified type using the provided instance key.
    /// </summary>
    /// <typeparam name="TService">The type to be resolved.</typeparam>
    /// <param name="instanceKey">The instance key for resolving the service.</param>
    /// <returns>An instance of the specified type.</returns>
    public TService Resolve<TService>(string instanceKey)
    {
        return container.GetInstance<TService>(instanceKey);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        container?.Dispose();
    }

    /// <summary>
    /// Releases the specified object from the container.
    /// </summary>
    /// <param name="obj">The object to be released.</param>
    public void Release(object obj)
    {
        container.TryAddDisposable(obj);
    }

    /// <summary>
    /// Creates a new service scope within the container.
    /// </summary>
    /// <returns>A new service scope.</returns>
    public IServiceScope CreateScope()
    {
        return container.ServiceProvider?.CreateScope();
    }

    /// <summary>
    /// Gets the service of the specified type from the container.
    /// </summary>
    /// <param name="serviceType">The type of service to get.</param>
    /// <returns>The service object, or null if there is no service of the specified type.</returns>
    public object GetService(Type serviceType)
    {
        return container.GetInstance(serviceType);
    }
}