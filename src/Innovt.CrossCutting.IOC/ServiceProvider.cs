// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.CrossCutting.IOC
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Core.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC
{
    public class ServiceProvider : IServiceProvider, ISupportRequiredService, IDisposable
    {
        private readonly IContainer container;
        private bool disposed;


        public ServiceProvider(IContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        public object GetRequiredService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) container.Dispose();

                disposed = true;
            }
        }
    }
}