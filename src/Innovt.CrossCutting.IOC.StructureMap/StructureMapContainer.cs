using System;
using Innovt.Core.CrossCutting.Ioc;
using StructureMap;
using StructureMap.Graph;

namespace Innovt.CrossCutting.IOC.StructureMap
{
    public class StructureMapContainer : Container, Core.CrossCutting.Ioc.IContainer
    {
        public StructureMapContainer()
        {

        }
        public StructureMapContainer(Action<ConfigurationExpression> action) : base(action)
        {
            
        }
        public StructureMapContainer(Registry registry) : base(registry)
        {
        }

        public StructureMapContainer(PluginGraph pluginGraph) : base(pluginGraph)
        {
        }

        public void AddModule(IOCModule module)
        {
            if (module == null) throw new ArgumentNullException(nameof(module));

            var services = module.GetServices();

            Configure(config =>
            {   
                config.Scan(s=>s.WithDefaultConventions());
                config.Populate(services);
            });
        }

        public void Register<T>(T type)
        {
            Configure(config =>
            {
                config.Scan(s => s.WithDefaultConventions());
                config.For<T>();
            });
        }

        public void Register<T>(T type, string instanceKey)
        {
            Configure(config =>
            {
                config.Scan(s => s.WithDefaultConventions());
                config.For<T>().Add<T>().Named(instanceKey);
            });
        }

        public object Resolve(Type type)
        {
            return GetInstance(type);
        }

        public TService Resolve<TService>()
        {
            return GetInstance<TService>();
        }
        

        public TService Resolve<TService>(Type type,string instanceKey)
        {
            return (TService)GetInstance(type, instanceKey);
        }

        public TService Resolve<TService>(string intanceKey)
        {
            return GetInstance<TService>(intanceKey);
        }
        
        public TService Resolve<TService>(Type type)
        {
            return (TService)GetInstance(type);
        }

        public void CheckConfiguration()
        {
            AssertConfigurationIsValid();
        }
    }


}
