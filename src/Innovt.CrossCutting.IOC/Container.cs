// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.CrossCutting.IOC
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Core.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC
{
    public sealed class Container : IContainer
    {
        private readonly Lamar.Container container;

        public Container(IServiceCollection services)
        {
            container = new Lamar.Container(services);
        }
        /// <summary>
        ///     With default scan
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

        public Container(IOCModule iocModule)
        {
            if (iocModule is null) throw new ArgumentNullException(nameof(iocModule));

            container = new Lamar.Container(iocModule.GetServices());
        }

        public void AddModule(IOCModule iocModule)
        {
            if (iocModule is null) throw new ArgumentNullException(nameof(iocModule));

            var services = iocModule.GetServices();

            container.Configure(services);
        }

        public void CheckConfiguration()
        {
            container.AssertConfigurationIsValid();
        }

        public object Resolve(Type type)
        {
            return container.GetInstance(type);
        }

        public TService Resolve<TService>()
        {
            return container.GetInstance<TService>();
        }

        public TService Resolve<TService>(Type type)
        {
            return (TService) container.GetInstance(type);
        }

        public TService Resolve<TService>(string instanceKey)
        {
            return container.GetInstance<TService>(instanceKey);
        }

        public void Dispose()
        {
            container?.Dispose();
        }

        public void Release(object obj)
        {
            container.TryAddDisposable(obj);            
        }

        public IServiceScope CreateScope()
        {
            return container.CreateScope();
        }

        public object GetService(Type serviceType)
        {
            return container.GetInstance(serviceType);
        }
    }
}