using Innovt.Core.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System;

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
        /// With default scan
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

        public Container(IOCModule module)
        {
            if (module is null) throw new ArgumentNullException(nameof(module));

            container = new Lamar.Container(module.GetServices());
        }

        public void AddModule(IOCModule module)
        {
            if (module is null) throw new ArgumentNullException(nameof(module));

            var services = module.GetServices();

            container.Configure(services);
        }

        public void CheckConfiguration()
        {
            container.AssertConfigurationIsValid();
        }

        public object GetService(Type serviceType)
        {
            return container.GetInstance(serviceType);
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
    }
}