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
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.container.Dispose();
                }

                this.disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}