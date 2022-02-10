// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.CrossCutting.IOC
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Innovt.CrossCutting.IOC
{
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
            if (disposing)
            {
                container?.Dispose();
            }
        }
    }
}