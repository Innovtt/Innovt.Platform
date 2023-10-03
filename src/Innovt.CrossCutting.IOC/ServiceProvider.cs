// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.IOC

using System;
using Innovt.Core.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC;
/// <summary>
/// Implementation of the <see cref="IServiceProvider"/> and <see cref="ISupportRequiredService"/> interfaces using Lamar IoC container.
/// </summary>
public class ServiceProvider : IServiceProvider, ISupportRequiredService, IDisposable
{
    private readonly IContainer container;
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceProvider"/> class using the provided Lamar IoC container.
    /// </summary>
    /// <param name="container">The Lamar IoC container.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="container"/> is null.</exception>
    public ServiceProvider(IContainer container)
    {
        this.container = container ?? throw new ArgumentNullException(nameof(container));
    }
    /// <summary>
    /// Releases the resources used by the <see cref="ServiceProvider"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    /// <summary>
    /// Gets the service of the specified type from the Lamar IoC container.
    /// </summary>
    /// <param name="serviceType">The type of service to get.</param>
    /// <returns>The service object, or null if there is no service of the specified type.</returns>
    public object GetService(Type serviceType)
    {
        return container.Resolve(serviceType);
    }
    /// <summary>
    /// Gets the service of the specified type from the Lamar IoC container.
    /// </summary>
    /// <param name="serviceType">The type of service to get.</param>
    /// <returns>The service object.</returns>
    /// <exception cref="InvalidOperationException">Thrown when there is no service of the specified type.</exception>
    public object GetRequiredService(Type serviceType)
    {
        return container.Resolve(serviceType);
    }
    /// <summary>
    /// Releases the resources used by the <see cref="ServiceProvider"/>.
    /// </summary>
    /// <param name="disposing">True if disposing; otherwise, false.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing) container.Dispose();

            disposed = true;
        }
    }
}