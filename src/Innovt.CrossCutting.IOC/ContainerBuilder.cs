// Innovt Company
// Author: Michel Borges
// Project: Innovt.CrossCutting.IOC

using System;
using Innovt.Core.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC;

public class ContainerBuilder : IDisposable
{
    private readonly IContainer container;

    public ContainerBuilder(IServiceCollection services)
    {
        container = new Container(services);
    }

    public IServiceProvider GetServiceProvider => new ServiceProvider(container);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) container?.Dispose();
    }
}