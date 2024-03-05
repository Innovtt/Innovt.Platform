// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.IOC

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC;

/// <summary>
///     Builder for creating an <see cref="IServiceProvider" /> using Lamar IoC container.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="ContainerBuilder" /> class using the provided services.
/// </remarks>
/// <param name="services">The collection of services for the container.</param>
public class ContainerBuilder(IServiceCollection services) : IDisposable
{
    private readonly Container container = new Container(services);

    /// <summary>
    ///     Gets the configured <see cref="IServiceProvider" />.
    /// </summary>
    public IServiceProvider GetServiceProvider => new ServiceProvider(container);

    /// <summary>
    ///     Releases the resources used by the <see cref="ContainerBuilder" />.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Releases the resources used by the <see cref="ContainerBuilder" />.
    /// </summary>
    /// <param name="disposing">True if disposing; otherwise, false.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing) container?.Dispose();
    }
}